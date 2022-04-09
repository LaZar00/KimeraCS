//  Code ported from P.P.A.Narayanan c++ 3ds loader
//  http://www.gamedev.net/reference/articles/article1259.asp

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace KimeraCS
{

    using static FF7PModel;

    using static Utils;

    class Model_3DS
    {
        public struct rgb3DS
        {
            public byte red;
            public byte green;
            public byte blue;
        }

        public struct vert3DS
        {
            public float x;
            public float z; // z and y are swapped
            public float y;
        }

        public struct texcoord3DS
        {
            public float u;
            public float v;
        }

        public struct face3DS
        {
            public ushort vertA;
            public ushort vertB;
            public ushort vertC;
            public ushort flags;     // From lib3ds (http://www.koders.com/cpp/fid8EDB781A3412B5309868BD6B71F9A9FE01469EDD.aspx?s=bump+map)
                                     // Bit 0: Edge visibility AC
                                     // Bit 1: Edge visibility BC
                                     // Bit 2: Edge visibility AB
                                     // Bit 3: Face is at tex U wrap seam
                                     // Bit 4: Face is at tex V wrap seam
                                     // Bit 5-8: Unused ?
                                     // Bit 9-10: Random ?
                                     // Bit 11-12: Unused ?
                                     // Bit 13: Selection of the face in selection 3
                                     // Bit 14: Selection of the face in selection 2
                                     // Bit 15: Selection of the face in selection 1
        }

        public struct map_list_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] textureFileName;     // Mapping filename (Texture)
            public float u;                    // U scale
            public float v;                    // V scale
            public float uOff;                 // U offset
            public float vOff;                 // V offset
            public float rotation;             // Rotation angle
        }

        public struct mat_list_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public byte[] materialName;        // Material name
            public rgb3DS ambient;             // Ambient color
            public rgb3DS diffuse;             // Diffuse color
            public rgb3DS specular;            // Specular color
            public map_list_node[] textureMapsV;       // Texture maps
        }

        public struct face_mat_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public byte[] materialName;        // Material name
            public ushort numEntries;           // Num. of entries
            public ushort[] facesV;         // Faces assigned to this material
        }

        public struct mesh_object_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public byte[] meshName;                    // Object name
            public vert3DS[] vertsV;               // Vertex list
            public face3DS[] facesV;               // Face list
            public ushort numVerts;                     // Num of vertices
            public ushort numFaces;                     // Num of faces
            public ushort numMappedVerts;               // Num of vertices having mapping coords.
            public texcoord3DS[] texCoordsV;       // Mapping coords. as U,V pairs (actual texture coordinates)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public float[] localTransformationMatrix;  // Local transformation matrix (last row is allways 0 0 0 1)
            public face_mat_node[] faceMaterialsV;
            public ushort[] faceMaterialIndicesV;   // Index of material for every face
        }

        public struct Model3DS
        {
            //public string modelName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            //public float[] translationMatrix;          // Translation matrix for objects (last row is allways 0 0 0 1)
            public mesh_object_node[] meshesV;
            public mat_list_node[] materialsV;
        }


        public static void ReadMaterial3DS(BinaryReader memReader, ref int offset, int fileLength, int length, ref mat_list_node[] materialsV)
        {
            int ci, count, llen, matIndex, mapIndex;
            ushort id;
            bool doneQ, isAmbientQ, isDiffuseQ; //, isSpecularQ;

            isAmbientQ = false;
            isDiffuseQ = false;
            //isSpecularQ = false;
            doneQ = false;
            count = offset + length - 6;

            if (materialsV != null)
            {
                matIndex = materialsV.Length + 1;
                Array.Resize(ref materialsV, matIndex);
                materialsV[matIndex] = new mat_list_node();
            }
            else
            {
                materialsV = new mat_list_node[1];
                matIndex = 0;
            }

           do
           {
                memReader.BaseStream.Position = offset;
                id = memReader.ReadUInt16();
                offset += 2;

                if (offset >= fileLength)
                {
                    doneQ = true;
                    break;
                }

                memReader.BaseStream.Position = offset;
                llen = memReader.ReadInt32();
                offset += 4;

                switch(id)
                {
                    case 0xA000:
                        //  Read material name
                        ci = 0;

                        materialsV[matIndex].materialName = new byte[200];
                        do
                        {
                            memReader.BaseStream.Position = offset;
                            materialsV[matIndex].materialName[ci] = memReader.ReadByte();
                            offset++;
                            ci++;
                        } while (materialsV[matIndex].materialName[ci - 1] != 0);

                        break;

                    case 0xA010:
                        //  Hey! AMBIENT
                        isDiffuseQ = false;
                        //isSpecularQ = false;
                        isAmbientQ = true;
                        materialsV[matIndex].ambient.red = 0;
                        materialsV[matIndex].ambient.green = 0;
                        materialsV[matIndex].ambient.blue = 0;
                        break;

                    case 0xA020:
                        //  Hey! DIFFUSE
                        isDiffuseQ = true;
                        //isSpecularQ = false;
                        isAmbientQ = false;
                        materialsV[matIndex].diffuse.red = 0;
                        materialsV[matIndex].diffuse.green = 0;
                        materialsV[matIndex].diffuse.blue = 0;
                        break;

                    case 0xA030:
                        //  Hey! SPECULAR
                        isDiffuseQ = false;
                        //isSpecularQ = true;
                        isAmbientQ = false;
                        materialsV[matIndex].specular.red = 0;
                        materialsV[matIndex].specular.green = 0;
                        materialsV[matIndex].specular.blue = 0;
                        break;

                    case 0xA200:
                        // Texture
                        if (materialsV[matIndex].textureMapsV != null)
                        {
                            mapIndex = materialsV[matIndex].textureMapsV.Length + 1;
                            Array.Resize(ref materialsV[matIndex].textureMapsV, mapIndex);
                            materialsV[matIndex].textureMapsV[mapIndex] = new map_list_node();
                        }
                        else
                        {
                            materialsV[matIndex].textureMapsV = new map_list_node[1];
                            mapIndex = 0;
                        }
                       
                        materialsV[matIndex].textureMapsV[mapIndex].u = 0;
                        materialsV[matIndex].textureMapsV[mapIndex].v = 0;
                        materialsV[matIndex].textureMapsV[mapIndex].uOff = 0;
                        materialsV[matIndex].textureMapsV[mapIndex].vOff = 0;
                        materialsV[matIndex].textureMapsV[mapIndex].rotation = 0;

                        break;

                    case 0xA300:
                        //  Texture name (filename without path)
                        mapIndex = materialsV[matIndex].textureMapsV.Length - 1;
                        ci = 0;

                        materialsV[matIndex].textureMapsV[mapIndex].textureFileName = new byte[256];
                        do
                        {
                            memReader.BaseStream.Position = offset;

                            materialsV[matIndex].textureMapsV[mapIndex].textureFileName[ci] = memReader.ReadByte();
                            offset++;
                            ci++;
                        } while (materialsV[matIndex].textureMapsV[mapIndex].textureFileName[ci - 1] != 0);

                        break;

                    case 0xA354:
                        //  V coords
                        mapIndex = materialsV[matIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[matIndex].textureMapsV[mapIndex].v = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA356:
                        //  U coords
                        mapIndex = materialsV[matIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[matIndex].textureMapsV[mapIndex].u = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA358:
                        //  U offset
                        mapIndex = materialsV[matIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[matIndex].textureMapsV[mapIndex].uOff = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA35A:
                        //  V offset
                        mapIndex = materialsV[matIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[matIndex].textureMapsV[mapIndex].vOff = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA35C:
                        //  Texture rotation angle
                        mapIndex = materialsV[matIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[matIndex].textureMapsV[mapIndex].rotation = memReader.ReadSingle();
                        offset += 4;
                       break;

                    case 0x11:
                        //  Read colors
                        if (isDiffuseQ)
                        {
                            materialsV[matIndex].diffuse.red = memReader.ReadByte();
                            materialsV[matIndex].diffuse.green = memReader.ReadByte();
                            materialsV[matIndex].diffuse.blue = memReader.ReadByte();
                        }
                        else if (isAmbientQ)
                        {
                            materialsV[matIndex].ambient.red = memReader.ReadByte();
                            materialsV[matIndex].ambient.green = memReader.ReadByte();
                            materialsV[matIndex].ambient.blue = memReader.ReadByte();
                        }
                        else
                        {
                            materialsV[matIndex].specular.red = memReader.ReadByte();
                            materialsV[matIndex].specular.green = memReader.ReadByte();
                            materialsV[matIndex].specular.blue = memReader.ReadByte();
                        }

                        offset += 3;
                        break;

                    default:
                        //  Unknown chunk
                        if (offset - 6 >= count)
                        {
                            offset -= 6;
                            doneQ = true;
                        }
                        else
                        {
                            offset = offset + llen - 6;
                            doneQ = offset >= fileLength;
                        }

                        break;
                }

            } while(!doneQ);
        }

        public static void ReadMesh3DS(BinaryReader memReader, ref int offset, int fileLength, int length, ref mesh_object_node[] meshesV)
        {
            int ci, count, llen, i, matIndex, meshIndex;
            ushort id;
            bool doneQ;
            mesh_object_node tmpMesh;

            //string test_str;

            count = offset + length - 6;
            doneQ = false;

            ci = 0;

            tmpMesh = new mesh_object_node();
            tmpMesh.numVerts = 0;
            tmpMesh.meshName = null;
            tmpMesh.faceMaterialsV = null;
            tmpMesh.localTransformationMatrix = new float[12];

            tmpMesh.meshName = new byte[200];
            do
            {
                memReader.BaseStream.Position = offset;
                tmpMesh.meshName[ci] = memReader.ReadByte();
                offset++;
                ci++;
            } while (tmpMesh.meshName[ci - 1] != 0);

            do
            {
                memReader.BaseStream.Position = offset;
                id = memReader.ReadUInt16();
                offset += 2;

                if (offset >= fileLength)
                {
                    doneQ = true;
                    break;
                }

                memReader.BaseStream.Position = offset;
                llen = memReader.ReadInt32();
                offset += 4;

                switch(id)
                {
                    case 0x4100:
                        // Errr... don't know. Do nothing.
                        break;

                    case 0x4110:
                        // Read vertices chunk
                        memReader.BaseStream.Position = offset;
                        tmpMesh.numVerts = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.vertsV = new vert3DS[tmpMesh.numVerts];
                        memReader.BaseStream.Position = offset;

                        for (i = 0; i < tmpMesh.numVerts; i++)
                        {
                            tmpMesh.vertsV[i].x = memReader.ReadSingle();
                            tmpMesh.vertsV[i].y = memReader.ReadSingle();
                            tmpMesh.vertsV[i].z = memReader.ReadSingle();
                        }

                        offset = offset + 3 * 4 * tmpMesh.numVerts;
                        break;

                    case 0x4120:
                        // Read vertices chunk
                        memReader.BaseStream.Position = offset;
                        tmpMesh.numFaces = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.facesV = new face3DS[tmpMesh.numFaces];
                        memReader.BaseStream.Position = offset;

                        for (i = 0; i < tmpMesh.numFaces; i++)
                        {
                            tmpMesh.facesV[i].vertA = memReader.ReadUInt16();
                            tmpMesh.facesV[i].vertB = memReader.ReadUInt16();
                            tmpMesh.facesV[i].vertC = memReader.ReadUInt16();
                            tmpMesh.facesV[i].flags = memReader.ReadUInt16();
                        }

                        offset = offset + 4 * 2 * tmpMesh.numFaces;
                        break;

                    case 0x4130:
                        // Read material mapping info
                        if (tmpMesh.faceMaterialsV != null)
                        {
                            matIndex = tmpMesh.faceMaterialsV.Length + 1;
                            Array.Resize(ref tmpMesh.faceMaterialsV, matIndex);
                            tmpMesh.faceMaterialsV[matIndex] = new face_mat_node();
                        }
                        else 
                        {
                            tmpMesh.faceMaterialsV = new face_mat_node[1];
                            matIndex = 0;
                        }

                        ci = 0;

                        tmpMesh.faceMaterialsV[matIndex].materialName = new byte[200];
                        do
                        {
                            memReader.BaseStream.Position = offset;
                            tmpMesh.faceMaterialsV[matIndex].materialName[ci] = memReader.ReadByte();
                            offset++;
                            ci++;
                        } while(tmpMesh.faceMaterialsV[matIndex].materialName[ci - 1] != 0);

                        memReader.BaseStream.Position = offset;
                        tmpMesh.faceMaterialsV[matIndex].numEntries = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.faceMaterialsV[matIndex].facesV = new ushort[tmpMesh.faceMaterialsV[matIndex].numEntries];

                        memReader.BaseStream.Position = offset;
                        for (i = 0; i < tmpMesh.faceMaterialsV[matIndex].numEntries; i++)
                        {
                            tmpMesh.faceMaterialsV[matIndex].facesV[i] = memReader.ReadUInt16();
                        }

                        offset = offset + 2 * tmpMesh.faceMaterialsV[matIndex].numEntries;
                        break;

                    case 0x4140:
                        // Read texture coordinates
                        memReader.BaseStream.Position = offset;

                        tmpMesh.numMappedVerts = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.texCoordsV = new texcoord3DS[tmpMesh.numMappedVerts];
                        for (i = 0; i < tmpMesh.numMappedVerts; i++)
                        {
                            tmpMesh.texCoordsV[i].u = memReader.ReadSingle();
                            tmpMesh.texCoordsV[i].v = memReader.ReadSingle();
                        }

                        offset = offset + 2 * 4 * tmpMesh.numMappedVerts;
                        break;

                    case 0x4160:
                        // Local transformation matrix
                        memReader.BaseStream.Position = offset;

                        for (i = 0; i < 12; i++)
                            tmpMesh.localTransformationMatrix[i] = memReader.ReadSingle();

                        offset = offset + 12 * 4;
                        break;

                    case 0x4000:
                        // Object
                        offset -= 6;
                        doneQ = true;
                        break;

                    default:
                        // Unknown chunk
                        if (offset - 6 >= count)
                        {
                            offset -= 6;
                            doneQ = true;
                        }
                        else
                        {
                            offset = offset + llen - 6;
                            doneQ = offset >= fileLength;
                        }
                        break;
                }

            } while (!doneQ);

            if (tmpMesh.numVerts > 0)
            {
                if (meshesV != null)
                {
                    meshIndex = meshesV.Length + 1;
                    Array.Resize(ref meshesV, meshIndex);
                    meshesV[meshIndex] = new mesh_object_node();
                }
                else
                {
                    meshesV = new mesh_object_node[1];
                    meshIndex = 0;
                }

                meshesV[meshIndex] = tmpMesh;

                // Debug.Print test_str; " "; temp_mesh.NumVerts; " "; temp_mesh.NumFaces
            }
        }

        public static void ReadObject3DS(BinaryReader memReader, ref int offset, int fileLength, int length, ref Model3DS[] modelsV)
        {
            int count, llen, modelIndex;
            ushort id;
            bool doneQ;

            count = offset + length - 6;
            doneQ = false;

            if (modelsV != null)
            {
                modelIndex = modelsV.Length + 1;
                Array.Resize(ref modelsV, modelIndex);
                modelsV[modelIndex] = new Model3DS();
            }
            else
            {
                modelsV = new Model3DS[1];
                modelIndex = 0;
            }          

            do
            {
                memReader.BaseStream.Position = offset;
                id = memReader.ReadUInt16();
                offset += 2;

                if (offset >= fileLength)
                {
                    doneQ = true;
                    break;
                }

                memReader.BaseStream.Position = offset;
                llen = memReader.ReadInt32();
                offset += 4;

                switch(id)
                {
                    case 0x4000:
                        // Some object chunk (provably a mesh)
                        ReadMesh3DS(memReader, ref offset, fileLength, llen, ref modelsV[modelIndex].meshesV);
                        break;

                    case 0xAFFF:
                        // Material chunk
                        ReadMaterial3DS(memReader, ref offset, fileLength, llen, ref modelsV[modelIndex].materialsV);
                        break;

                    default:
                        // Unknown chunk
                        if (offset -6 >= count)
                        {
                            offset -= 6;
                            doneQ = true;
                        }
                        else
                        {
                            offset = offset + llen - 6;
                            doneQ = offset >= fileLength;
                        }
                        break;
                }
            }
            while (!doneQ);
        }

        public static void Read3DS(BinaryReader memReader, ref int offset, int fileLength, ref Model3DS[] modelsV)
        {
            ushort id;
            int  llen;
            bool doneQ;

            doneQ = false;

            do
            {
                memReader.BaseStream.Position = offset;

                id = memReader.ReadUInt16();
                offset += 2;

                if (offset > fileLength)
                {
                    doneQ = true;
                    break;
                }

                llen = memReader.ReadInt32();
                offset += 4;

                switch(id)
                {
                    case 0xFFFF:
                        doneQ = true;
                        break;

                    case 0x3D3D:
                        // Object chunk
                        ReadObject3DS(memReader, ref offset, fileLength, llen, ref modelsV);
                        break;

                    default:
                        // Unknown chunk
                        offset = (int)(offset + llen - 6);
                        doneQ = offset >= fileLength;
                        break;
                }
            }
            while (!doneQ);
        }

        public static bool ReadPrimaryChunk3DS(BinaryReader memReader, ref int offset, int fileLength, ref Model3DS[] modelsV)
        {
            byte version;
            short flag;
            bool bReadPrimaryChunk3DSResult = false;

            memReader.BaseStream.Position = offset;

            flag = memReader.ReadInt16();

            if (flag == 0x4D4D)
            {
                offset = 28;
                memReader.BaseStream.Position = offset;
                version = memReader.ReadByte();

                offset = 16;
                Read3DS(memReader, ref offset, fileLength, ref modelsV);

                bReadPrimaryChunk3DSResult = true;
            }

            return bReadPrimaryChunk3DSResult;
        }

        public static void BuildFaceMaterialList(ref Model3DS Model)
        {
            //  Build the list of material indices for every face

            int numMeshes, numMaterials, mei, mfi, ci, fi;
            ushort mai;
            bool foundQ;
            int numFaceMatGroups, numFaces;

            numMeshes = Model.meshesV.Length;
            numMaterials = Model.materialsV.Length;

            for (mei = 0; mei < numMeshes; mei++)
            {
                numFaceMatGroups = Model.meshesV[mei].faceMaterialsV.Length;

                Array.Resize(ref Model.meshesV[mei].faceMaterialIndicesV, Model.meshesV[mei].numFaces);

                for (mfi = 0; mfi < numFaceMatGroups; mfi++)
                {
                    mai = 0;
                    foundQ = false;

                    do
                    {
                        ci = 0;

                        while (Model.meshesV[mei].faceMaterialsV[mfi].materialName[ci] == Model.materialsV[mai].materialName[ci] &&
                               Model.meshesV[mei].faceMaterialsV[mfi].materialName[ci] != 0 &&
                               Model.materialsV[mai].materialName[ci] != 0)
                        {
                            ci++;
                        }

                        foundQ = Model.meshesV[mei].faceMaterialsV[mfi].materialName[ci] == Model.materialsV[mai].materialName[ci];

                        mai++;

                    } while (!foundQ && mai == numMaterials);

                    mai--;

                    numFaces = Model.meshesV[mei].faceMaterialsV[mfi].numEntries;

                    for (fi = 0; fi < numFaces; fi++)
                    {
                        Model.meshesV[mei].faceMaterialIndicesV[Model.meshesV[mei].faceMaterialsV[mfi].facesV[fi]] = mai;
                    }
                }
            }
        }

        public static int Load3DS(string fileName, out Model3DS[] modelsV)
        {
            int iLoad3DSResult = 0;
            byte[] fileBuffer;
            int offset, fileLength, numModels, mi;

            modelsV = null;

            try
            {
                // Let's read 3DS file into memory.
                // Read All 3DS Model file into memory
                fileBuffer = File.ReadAllBytes(fileName);

                offset = 0;
                fileLength = fileBuffer.Length;

                using (var fileMemory = new MemoryStream(fileBuffer))
                {
                    using (var memReader = new BinaryReader(fileMemory))
                    {
                        while (ReadPrimaryChunk3DS(memReader, ref offset, fileLength, ref modelsV) &&
                               offset < fileLength);
                    }
                }              

                if (modelsV.Length != 0) numModels = modelsV.Length;
                else numModels = 0;

                for (mi = 0; mi < numModels; mi++)
                {
                    BuildFaceMaterialList(ref modelsV[mi]);
                }

                iLoad3DSResult = 1;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error Loading 3DS.");
                iLoad3DSResult = -1;
            }

            return iLoad3DSResult;
        }





        //  ---------------------------------------------------------------------------------------------------------
        //  ---------------------------------------- 3Ds => PModel --------------------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        private static void GetVerts(ref mesh_object_node mesh, out Point3D[] vertsV)
        {
            int i;

            vertsV = new Point3D[mesh.numVerts];

            for (i = 0; i < mesh.numVerts; i++)
            {
                vertsV[i].x = mesh.vertsV[i].x;
                vertsV[i].y = mesh.vertsV[i].y;
                vertsV[i].z = mesh.vertsV[i].z;
            }
        }

        private static void GetFaces(ref mesh_object_node mesh, out PPolygon[] facesV)
        {
            int fi;

            facesV = new PPolygon[mesh.numFaces];

            for (fi = 0; fi < mesh.numFaces; fi++)
            {
                facesV[fi].tag1 = 0;
                facesV[fi].tag2 = 0xCFCEA00;

                facesV[fi].Verts = new short[3];
                facesV[fi].Verts[0] = (short)mesh.facesV[fi].vertC;
                facesV[fi].Verts[1] = (short)mesh.facesV[fi].vertB;
                facesV[fi].Verts[2] = (short)mesh.facesV[fi].vertA;

                facesV[fi].Normals = new short[3];
                facesV[fi].Edges = new short[3];
            }
        }

        private static void GetTexCoords(ref mesh_object_node mesh, out Point2D[] texCoordsV)
        {
            int i;

            texCoordsV = null;

            if (mesh.numMappedVerts > 0)
            {
                texCoordsV = new Point2D[mesh.numVerts];

                for (i = 0; i < mesh.numMappedVerts; i++)
                {
                    texCoordsV[i].x = mesh.texCoordsV[i].u;
                    texCoordsV[i].y = mesh.texCoordsV[i].v;
                }
            }
        }

        private static void GetVColors(ref mesh_object_node mesh, mat_list_node[] materialsV, out Color[] vcolorsV)
        {
            int ci, fi;
            stIntVector[] faces_per_vert;
            long temp_r, temp_g, temp_b;

            int v_index, face_index;

            faces_per_vert = new stIntVector[mesh.numVerts];

            for (fi = 0; fi < mesh.numFaces; fi++)
            {
                v_index = mesh.facesV[fi].vertA;
                face_index = faces_per_vert[v_index].length;
                Array.Resize(ref faces_per_vert[v_index].vector, face_index + 1);
                faces_per_vert[v_index].vector[face_index] = fi;
                faces_per_vert[v_index].length = face_index + 1;

                v_index = mesh.facesV[fi].vertB;
                face_index = faces_per_vert[v_index].length;
                Array.Resize(ref faces_per_vert[v_index].vector, face_index + 1);
                faces_per_vert[v_index].vector[face_index] = fi;
                faces_per_vert[v_index].length = face_index + 1;

                v_index = mesh.facesV[fi].vertC;
                face_index = faces_per_vert[v_index].length;
                Array.Resize(ref faces_per_vert[v_index].vector, face_index + 1);
                faces_per_vert[v_index].vector[face_index] = fi;
                faces_per_vert[v_index].length = face_index + 1;
            }

            vcolorsV = new Color[mesh.numVerts];

            for (ci = 0; ci < mesh.numVerts; ci++)
            {
                temp_r = 0;
                temp_g = 0;
                temp_b = 0;

                for (fi = 0; fi < faces_per_vert[ci].length; fi++)
                {
                    temp_r = temp_r + materialsV[mesh.faceMaterialIndicesV[faces_per_vert[ci].vector[fi]]].diffuse.red;
                    temp_g = temp_g + materialsV[mesh.faceMaterialIndicesV[faces_per_vert[ci].vector[fi]]].diffuse.green;
                    temp_b = temp_b + materialsV[mesh.faceMaterialIndicesV[faces_per_vert[ci].vector[fi]]].diffuse.blue;
                }

                if (faces_per_vert[ci].length != 0)
                {
                    vcolorsV[ci] = Color.FromArgb(255,
                                                  (int)(temp_r / faces_per_vert[ci].length),
                                                  (int)(temp_g / faces_per_vert[ci].length),
                                                  (int)(temp_b / faces_per_vert[ci].length));
                }
            }
        }

        private static void GetPColors(ref mesh_object_node mesh, mat_list_node[] materialsV, out Color[] pcolorsV)
        {
            int ci;

            pcolorsV = new Color[mesh.numFaces];

            for (ci = 0; ci < mesh.numFaces; ci++)
            {
                pcolorsV[ci] = Color.FromArgb(pcolorsV[ci].R + materialsV[mesh.faceMaterialIndicesV[ci]].diffuse.red,
                                              pcolorsV[ci].G + materialsV[mesh.faceMaterialIndicesV[ci]].diffuse.green,
                                              pcolorsV[ci].B + materialsV[mesh.faceMaterialIndicesV[ci]].diffuse.blue);
            }
        }

        private static void ConvertMesh3DSToPModel(ref mesh_object_node mesh, mat_list_node[] materialsV, out PModel Model)
        {
            Point3D[] vertsV;
            PPolygon[] facesV;
            Point2D[] texcoordsV;
            Color[] vcolorsV;
            Color[] pcolorsV;

            GetVerts(ref mesh, out vertsV);
            GetFaces(ref mesh, out facesV);
            GetTexCoords(ref mesh, out texcoordsV);
            GetVColors(ref mesh, materialsV, out vcolorsV);
            GetPColors(ref mesh, materialsV, out pcolorsV);

            Model = new PModel();
            AddGroup(ref Model, ref vertsV, ref facesV, ref texcoordsV, ref vcolorsV, ref pcolorsV);
        }

        public static void ConvertModel3DSToPModel(ref Model3DS Model, ref PModel outModel)
        {
            int mi, numMeshes;

            numMeshes = Model.meshesV.Length;

            for (mi = 0; mi < numMeshes; mi++)
            {
                ConvertMesh3DSToPModel(ref Model.meshesV[mi], Model.materialsV, out outModel);
            }
        }

        public static void ConvertModels3DSToPModel(Model3DS[] modelsV, ref PModel outModel)
        {
            int mi, numModels;

            numModels = modelsV.Length;

            for (mi = 0; mi < numModels; mi++)
            {
                ConvertModel3DSToPModel(ref modelsV[mi], ref outModel);
            }

            outModel.Header.off00 = 1;
            outModel.Header.off04 = 1;

            // Instantiate Header.unknown (in VB6 this is done directly, here we need to do it manually or when creating the PModel.
            // In the other P Models (Field, Battle or Magic) we will have this unknown data, but not when converting a 3DS to PModel.s
            outModel.Header.unknown = new int[16];

            outModel.resizeX = 1;
            outModel.resizeY = 1;
            outModel.resizeZ = 1;
            outModel.repositionX = 0;
            outModel.repositionY = 0;
            outModel.repositionZ = 0;
            outModel.rotateAlpha = 0;
            outModel.rotateBeta = 0;
            outModel.rotateGamma = 0;
            outModel.rotationQuaternion.x = 0;
            outModel.rotationQuaternion.y = 0;
            outModel.rotationQuaternion.z = 0;
            outModel.rotationQuaternion.w = 1;

            ComputeNormals(ref outModel);
            ComputeBoundingBox(ref outModel);
            ComputeEdges(ref outModel);
        }
    }
}
