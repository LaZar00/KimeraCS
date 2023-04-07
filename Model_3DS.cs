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
        public struct Rgb3DS
        {
            public byte red;
            public byte green;
            public byte blue;
        }

        public struct Vert3DS
        {
            public float x;
            public float z; // z and y are swapped
            public float y;
        }

        public struct Texcoord3DS
        {
            public float u;
            public float v;
        }

        public struct Face3DS
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

        public struct Map_list_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            public byte[] textureFileName;     // Mapping filename (Texture)
            public float u;                    // U scale
            public float v;                    // V scale
            public float uOff;                 // U offset
            public float vOff;                 // V offset
            public float rotation;             // Rotation angle
        }

        public struct Mat_list_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public byte[] materialName;        // Material name
            public Rgb3DS ambient;             // Ambient color
            public Rgb3DS diffuse;             // Diffuse color
            public Rgb3DS specular;            // Specular color
            public Map_list_node[] textureMapsV;       // Texture maps
        }

        public struct Face_mat_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public byte[] materialName;        // Material name
            public ushort numEntries;           // Num. of entries
            public ushort[] facesV;         // Faces assigned to this material
        }

        public struct Mesh_object_node
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
            public byte[] meshName;                    // Object name
            public Vert3DS[] vertsV;               // Vertex list
            public Face3DS[] facesV;               // Face list
            public ushort numVerts;                     // Num of vertices
            public ushort numFaces;                     // Num of faces
            public ushort numMappedVerts;               // Num of vertices having mapping coords.
            public Texcoord3DS[] texCoordsV;       // Mapping coords. as U,V pairs (actual texture coordinates)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public float[] localTransformationMatrix;  // Local transformation matrix (last row is allways 0 0 0 1)
            public Face_mat_node[] faceMaterialsV;
            public ushort[] faceMaterialIndicesV;   // Index of material for every face
        }

        public struct Model3DS
        {
            //public string modelName;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
            //public float[] translationMatrix;          // Translation matrix for objects (last row is allways 0 0 0 1)
            public Mesh_object_node[] meshesV;
            public Mat_list_node[] materialsV;
        }


        public static void ReadMaterial3DS(BinaryReader memReader, 
                                           ref int offset, int fileLength, int length, 
                                           ref Mat_list_node[] materialsV)
        {
            int iVColorIdx, iCount, iLlen, iMatIndex, iMapIndex;
            ushort usID;
            bool bDoneQ, bIsAmbientQ, bIsDiffuseQ; //, bIsSpecularQ;

            bIsAmbientQ = false;
            bIsDiffuseQ = false;
            //isSpecularQ = false;
            bDoneQ = false;
            iCount = offset + length - 6;

            if (materialsV != null)
            {
                iMatIndex = materialsV.Length;
                Array.Resize(ref materialsV, materialsV.Length + 1);
                materialsV[iMatIndex] = new Mat_list_node();
            }
            else
            {
                materialsV = new Mat_list_node[1];
                iMatIndex = 0;
            }

           do
           {
                memReader.BaseStream.Position = offset;
                usID = memReader.ReadUInt16();
                offset += 2;

                if (offset >= fileLength) break;

                memReader.BaseStream.Position = offset;
                iLlen = memReader.ReadInt32();
                offset += 4;

                switch(usID)
                {
                    case 0xA000:
                        //  Read material name
                        iVColorIdx = 0;

                        materialsV[iMatIndex].materialName = new byte[200];
                        do
                        {
                            memReader.BaseStream.Position = offset;
                            materialsV[iMatIndex].materialName[iVColorIdx] = memReader.ReadByte();
                            offset++;
                            iVColorIdx++;
                        } while (materialsV[iMatIndex].materialName[iVColorIdx - 1] != 0);

                        break;

                    case 0xA010:
                        //  Hey! AMBIENT
                        bIsDiffuseQ = false;
                        //isSpecularQ = false;
                        bIsAmbientQ = true;
                        materialsV[iMatIndex].ambient.red = 0;
                        materialsV[iMatIndex].ambient.green = 0;
                        materialsV[iMatIndex].ambient.blue = 0;
                        break;

                    case 0xA020:
                        //  Hey! DIFFUSE
                        bIsDiffuseQ = true;
                        //isSpecularQ = false;
                        bIsAmbientQ = false;
                        materialsV[iMatIndex].diffuse.red = 0;
                        materialsV[iMatIndex].diffuse.green = 0;
                        materialsV[iMatIndex].diffuse.blue = 0;
                        break;

                    case 0xA030:
                        //  Hey! SPECULAR
                        bIsDiffuseQ = false;
                        //isSpecularQ = true;
                        bIsAmbientQ = false;
                        materialsV[iMatIndex].specular.red = 0;
                        materialsV[iMatIndex].specular.green = 0;
                        materialsV[iMatIndex].specular.blue = 0;
                        break;

                    case 0xA200:
                        // Texture
                        if (materialsV[iMatIndex].textureMapsV != null)
                        {
                            iMapIndex = materialsV[iMatIndex].textureMapsV.Length + 1;
                            Array.Resize(ref materialsV[iMatIndex].textureMapsV, iMapIndex);
                            materialsV[iMatIndex].textureMapsV[iMapIndex] = new Map_list_node();
                        }
                        else
                        {
                            materialsV[iMatIndex].textureMapsV = new Map_list_node[1];
                            iMapIndex = 0;
                        }
                       
                        materialsV[iMatIndex].textureMapsV[iMapIndex].u = 0;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].v = 0;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].uOff = 0;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].vOff = 0;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].rotation = 0;

                        break;

                    case 0xA300:
                        //  Texture name (filename without path)
                        iMapIndex = materialsV[iMatIndex].textureMapsV.Length - 1;
                        iVColorIdx = 0;

                        materialsV[iMatIndex].textureMapsV[iMapIndex].textureFileName = new byte[256];
                        do
                        {
                            memReader.BaseStream.Position = offset;

                            materialsV[iMatIndex].textureMapsV[iMapIndex].textureFileName[iVColorIdx] = 
                                                    memReader.ReadByte();
                            offset++;
                            iVColorIdx++;

                        } while (materialsV[iMatIndex].textureMapsV[iMapIndex].
                                            textureFileName[iVColorIdx - 1] != 0);

                        break;

                    case 0xA354:
                        //  V coords
                        iMapIndex = materialsV[iMatIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].v = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA356:
                        //  U coords
                        iMapIndex = materialsV[iMatIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].u = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA358:
                        //  U offset
                        iMapIndex = materialsV[iMatIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].uOff = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA35A:
                        //  V offset
                        iMapIndex = materialsV[iMatIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].vOff = memReader.ReadSingle();
                        offset += 4;
                        break;

                    case 0xA35C:
                        //  Texture rotation angle
                        iMapIndex = materialsV[iMatIndex].textureMapsV.Length;

                        memReader.BaseStream.Position = offset;
                        materialsV[iMatIndex].textureMapsV[iMapIndex].rotation = memReader.ReadSingle();
                        offset += 4;
                       break;

                    case 0x11:
                        //  Read colors
                        if (bIsDiffuseQ)
                        {
                            materialsV[iMatIndex].diffuse.red = memReader.ReadByte();
                            materialsV[iMatIndex].diffuse.green = memReader.ReadByte();
                            materialsV[iMatIndex].diffuse.blue = memReader.ReadByte();
                        }
                        else if (bIsAmbientQ)
                        {
                            materialsV[iMatIndex].ambient.red = memReader.ReadByte();
                            materialsV[iMatIndex].ambient.green = memReader.ReadByte();
                            materialsV[iMatIndex].ambient.blue = memReader.ReadByte();
                        }
                        else
                        {
                            materialsV[iMatIndex].specular.red = memReader.ReadByte();
                            materialsV[iMatIndex].specular.green = memReader.ReadByte();
                            materialsV[iMatIndex].specular.blue = memReader.ReadByte();
                        }

                        offset += 3;
                        break;

                    default:
                        //  Unknown chunk
                        if (offset - 6 >= iCount)
                        {
                            offset -= 6;
                            bDoneQ = true;
                        }
                        else
                        {
                            offset = offset + iLlen - 6;
                            bDoneQ = offset >= fileLength;
                        }

                        break;
                }

            } while(!bDoneQ);
        }

        public static void ReadMesh3DS(BinaryReader memReader, ref int offset, int fileLength, int length, ref Mesh_object_node[] meshesV)
        {
            int iVColorIdx, iCount, iLlen, iVertIdx, iPolyIdx;
            int iMatCounter, iTexCoordIdx, iLocalTransMatrxIdx, iMatIndex, iMeshIndex;
            ushort usID;
            bool bDoneQ;
            Mesh_object_node tmpMesh;

            //string test_str;

            iCount = offset + length - 6;
            bDoneQ = false;

            iVColorIdx = 0;

            tmpMesh = new Mesh_object_node()
            {
                numVerts = 0,
                faceMaterialsV = null,

                localTransformationMatrix = new float[12],
                meshName = new byte[200],
            };

            do
            {
                memReader.BaseStream.Position = offset;
                tmpMesh.meshName[iVColorIdx] = memReader.ReadByte();
                offset++;
                iVColorIdx++;
            } while (tmpMesh.meshName[iVColorIdx - 1] != 0);

            do
            {
                memReader.BaseStream.Position = offset;
                usID = memReader.ReadUInt16();
                offset += 2;

                if (offset >= fileLength) break;

                memReader.BaseStream.Position = offset;
                iLlen = memReader.ReadInt32();
                offset += 4;

                switch(usID)
                {
                    case 0x4100:
                        // Object mesh. Do nothing.
                        break;

                    case 0x4110:
                        // Read vertices chunk
                        memReader.BaseStream.Position = offset;
                        tmpMesh.numVerts = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.vertsV = new Vert3DS[tmpMesh.numVerts];
                        memReader.BaseStream.Position = offset;

                        for (iVertIdx = 0; iVertIdx < tmpMesh.numVerts; iVertIdx++)
                        {
                            tmpMesh.vertsV[iVertIdx].x = memReader.ReadSingle();
                            tmpMesh.vertsV[iVertIdx].y = memReader.ReadSingle();
                            tmpMesh.vertsV[iVertIdx].z = memReader.ReadSingle();
                        }

                        offset += 3 * 4 * tmpMesh.numVerts;
                        break;

                    case 0x4120:
                        // Read vertices chunk
                        memReader.BaseStream.Position = offset;
                        tmpMesh.numFaces = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.facesV = new Face3DS[tmpMesh.numFaces];
                        memReader.BaseStream.Position = offset;

                        for (iPolyIdx = 0; iPolyIdx < tmpMesh.numFaces; iPolyIdx++)
                        {
                            tmpMesh.facesV[iPolyIdx].vertA = memReader.ReadUInt16();
                            tmpMesh.facesV[iPolyIdx].vertB = memReader.ReadUInt16();
                            tmpMesh.facesV[iPolyIdx].vertC = memReader.ReadUInt16();
                            tmpMesh.facesV[iPolyIdx].flags = memReader.ReadUInt16();
                        }

                        offset += 4 * 2 * tmpMesh.numFaces;
                        break;

                    case 0x4130:
                        // Read material mapping info
                        if (tmpMesh.faceMaterialsV != null)
                        {
                            iMatIndex = tmpMesh.faceMaterialsV.Length;
                            Array.Resize(ref tmpMesh.faceMaterialsV, iMatIndex + 1);
                            tmpMesh.faceMaterialsV[iMatIndex] = new Face_mat_node();
                        }
                        else 
                        {
                            tmpMesh.faceMaterialsV = new Face_mat_node[1];
                            iMatIndex = 0;
                        }

                        iVColorIdx = 0;

                        tmpMesh.faceMaterialsV[iMatIndex].materialName = new byte[200];
                        do
                        {

                            memReader.BaseStream.Position = offset;
                            tmpMesh.faceMaterialsV[iMatIndex].materialName[iVColorIdx] = 
                                                memReader.ReadByte();
                            offset++;
                            iVColorIdx++;

                        } while(tmpMesh.faceMaterialsV[iMatIndex].materialName[iVColorIdx - 1] != 0);

                        memReader.BaseStream.Position = offset;
                        tmpMesh.faceMaterialsV[iMatIndex].numEntries = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.faceMaterialsV[iMatIndex].facesV = 
                                    new ushort[tmpMesh.faceMaterialsV[iMatIndex].numEntries];

                        memReader.BaseStream.Position = offset;
                        for (iMatCounter = 0; 
                             iMatCounter < tmpMesh.faceMaterialsV[iMatIndex].numEntries; 
                             iMatCounter++)
                        {
                            tmpMesh.faceMaterialsV[iMatIndex].facesV[iMatCounter] = 
                                                memReader.ReadUInt16();
                        }

                        offset += 2 * tmpMesh.faceMaterialsV[iMatIndex].numEntries;
                        break;

                    case 0x4140:
                        // Read texture coordinates
                        memReader.BaseStream.Position = offset;

                        tmpMesh.numMappedVerts = memReader.ReadUInt16();
                        offset += 2;

                        tmpMesh.texCoordsV = new Texcoord3DS[tmpMesh.numMappedVerts];
                        for (iTexCoordIdx = 0; iTexCoordIdx < tmpMesh.numMappedVerts; iTexCoordIdx++)
                        {
                            tmpMesh.texCoordsV[iTexCoordIdx].u = memReader.ReadSingle();
                            tmpMesh.texCoordsV[iTexCoordIdx].v = memReader.ReadSingle();
                        }

                        offset += 2 * 4 * tmpMesh.numMappedVerts;
                        break;

                    case 0x4160:
                        // Local transformation matrix
                        memReader.BaseStream.Position = offset;

                        for (iLocalTransMatrxIdx = 0; iLocalTransMatrxIdx < 12; iLocalTransMatrxIdx++)
                            tmpMesh.localTransformationMatrix[iLocalTransMatrxIdx] = memReader.ReadSingle();

                        offset += 12 * 4;
                        break;

                    case 0x4000:
                        // Object
                        offset -= 6;
                        bDoneQ = true;
                        break;

                    default:
                        // Unknown chunk
                        if (offset - 6 >= iCount)
                        {
                            offset -= 6;
                            bDoneQ = true;
                        }
                        else
                        {
                            offset = offset + iLlen - 6;
                            bDoneQ = offset >= fileLength;
                        }
                        break;
                }

            } while (!bDoneQ);

            if (tmpMesh.numVerts > 0)
            {
                if (meshesV != null)
                {
                    iMeshIndex = meshesV.Length;
                    Array.Resize(ref meshesV, iMeshIndex + 1);
                    meshesV[iMeshIndex] = new Mesh_object_node();
                }
                else
                {
                    meshesV = new Mesh_object_node[1];
                    iMeshIndex = 0;
                }

                meshesV[iMeshIndex] = tmpMesh;

                // Debug.Print test_str; " "; temp_mesh.NumVerts; " "; temp_mesh.NumFaces
            }
        }

        public static void ReadObject3DS(BinaryReader memReader, 
                                         ref int offset, int fileLength, int length, 
                                         ref Model3DS[] modelsV)
        {
            int iCount, iLlen, iModelIndex;
            ushort usId;
            bool bDoneQ;

            iCount = offset + length - 6;
            bDoneQ = false;

            if (modelsV != null)
            {
                iModelIndex = modelsV.Length;
                Array.Resize(ref modelsV, iModelIndex + 1);
                modelsV[iModelIndex] = new Model3DS();
            }
            else
            {
                modelsV = new Model3DS[1];
                iModelIndex = 0;
            }          

            do
            {
                memReader.BaseStream.Position = offset;

                if (offset >= fileLength) break;

                usId = memReader.ReadUInt16();
                offset += 2;

                memReader.BaseStream.Position = offset;
                iLlen = memReader.ReadInt32();
                offset += 4;

                switch(usId)
                {
                    case 0x4000:
                        // Some object chunk (probably a mesh)
                        ReadMesh3DS(memReader, ref offset, fileLength, iLlen, 
                                    ref modelsV[iModelIndex].meshesV);
                        break;

                    case 0xAFFF:
                        // Material chunk
                        ReadMaterial3DS(memReader, ref offset, fileLength, iLlen, 
                                        ref modelsV[iModelIndex].materialsV);
                        break;

                    default:
                        // Unknown chunk
                        if (offset -6 >= iCount)
                        {
                            offset -= 6;
                            bDoneQ = true;
                        }
                        else
                        {
                            offset = offset + iLlen - 6;
                            bDoneQ = offset >= fileLength;
                        }
                        break;
                }
            }
            while (!bDoneQ);
        }

        public static void Read3DS(BinaryReader memReader, ref int offset, int fileLength, 
                                   ref Model3DS[] modelsV)
        {
            ushort usId;
            int  iLlen;
            bool bDoneQ;

            bDoneQ = false;

            do
            {
                memReader.BaseStream.Position = offset;

                if (offset >= fileLength) break;

                usId = memReader.ReadUInt16();
                offset += 2;

                iLlen = memReader.ReadInt32();
                offset += 4;

                switch(usId)
                {
                    case 0xFFFF:
                        bDoneQ = true;
                        break;

                    case 0x3D3D:
                        // Object chunk
                        ReadObject3DS(memReader, ref offset, fileLength, iLlen, ref modelsV);
                        break;

                    default:
                        // Unknown chunk
                        offset = (int)(offset + iLlen - 6);
                        bDoneQ = offset >= fileLength;
                        break;
                }
            }
            while (!bDoneQ);
        }

        public static bool ReadPrimaryChunk3DS(BinaryReader memReader, ref int offset, int fileLength, 
                                               ref Model3DS[] modelsV)
        {

            short sFlag;
            bool bReadPrimaryChunk3DSResult = false;

            memReader.BaseStream.Position = offset;

            sFlag = memReader.ReadInt16();

            if (sFlag == 0x4D4D)
            {
                offset = 28;
                memReader.BaseStream.Position = offset;
                memReader.ReadByte();       // version - not used

                offset = 16;
                Read3DS(memReader, ref offset, fileLength, ref modelsV);

                bReadPrimaryChunk3DSResult = true;
            }

            return bReadPrimaryChunk3DSResult;
        }

        public static void BuildFaceMaterialList(ref Model3DS Model)
        {
            //  Build the list of material indices for every face

            int iNumMeshes, iNumMaterials, iMeshIdx, iFaceMatIdx, iVColorIdx, iPolyIdx;
            int iNumFaceMatGroups, iNumFaces;
            ushort iMaterialIdx;
            bool bFoundQ;

            iNumMeshes = Model.meshesV.Length;

            if (Model.materialsV != null)
                iNumMaterials = Model.materialsV.Length;
            else
                iNumMaterials = 0;

            for (iMeshIdx = 0; iMeshIdx < iNumMeshes; iMeshIdx++)
            {
                if (Model.meshesV[iMeshIdx].faceMaterialsV != null)
                {
                    iNumFaceMatGroups = Model.meshesV[iMeshIdx].faceMaterialsV.Length;

                    Array.Resize(ref Model.meshesV[iMeshIdx].faceMaterialIndicesV, 
                                 Model.meshesV[iMeshIdx].numFaces);

                    for (iFaceMatIdx = 0; iFaceMatIdx < iNumFaceMatGroups; iFaceMatIdx++)
                    {
                        iMaterialIdx = 0;
                        
                        do
                        {
                            iVColorIdx = 0;

                            while (Model.meshesV[iMeshIdx].faceMaterialsV[iFaceMatIdx].materialName[iVColorIdx] == Model.materialsV[iMaterialIdx].materialName[iVColorIdx] &&
                                   Model.meshesV[iMeshIdx].faceMaterialsV[iFaceMatIdx].materialName[iVColorIdx] != 0 &&
                                   Model.materialsV[iMaterialIdx].materialName[iVColorIdx] != 0)
                            {
                                iVColorIdx++;
                            }

                            bFoundQ = 
                                Model.meshesV[iMeshIdx].faceMaterialsV[iFaceMatIdx].materialName[iVColorIdx] == 
                                Model.materialsV[iMaterialIdx].materialName[iVColorIdx];

                            iMaterialIdx++;

                        } while (!bFoundQ && iMaterialIdx != iNumMaterials);

                        iMaterialIdx--;

                        iNumFaces = Model.meshesV[iMeshIdx].faceMaterialsV[iFaceMatIdx].numEntries;

                        for (iPolyIdx = 0; iPolyIdx < iNumFaces; iPolyIdx++)
                        {
                            Model.meshesV[iMeshIdx].faceMaterialIndicesV[Model.meshesV[iMeshIdx].
                                faceMaterialsV[iFaceMatIdx].facesV[iPolyIdx]] = iMaterialIdx;
                        }
                    }
                }
            }
        }

        public static int Load3DS(string fileName, out Model3DS[] modelsV)
        {
            int iLoad3DSResult;
            byte[] fileBuffer;
            int iOffset, iFileLength, iNumModels, iModelIdx;

            modelsV = null;

            try
            {
                // Let's read 3DS file into memory.
                // Read All 3DS Model file into memory
                fileBuffer = File.ReadAllBytes(fileName);

                iOffset = 0;
                iFileLength = fileBuffer.Length;

                using (var fileMemory = new MemoryStream(fileBuffer))
                {
                    using (var memReader = new BinaryReader(fileMemory))
                    {
                        while (ReadPrimaryChunk3DS(memReader, ref iOffset, iFileLength, ref modelsV) &&
                               iOffset < iFileLength) ;
                    }
                }              

                if (modelsV.Length != 0) iNumModels = modelsV.Length;
                else iNumModels = 0;

                for (iModelIdx = 0; iModelIdx < iNumModels; iModelIdx++)
                {
                    if (modelsV[iModelIdx].meshesV != null)
                        BuildFaceMaterialList(ref modelsV[iModelIdx]);
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
        private static void GetVerts(Mesh_object_node mesh, out Point3D[] vertsV)
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

        private static void GetFaces(Mesh_object_node mesh, out PPolygon[] facesV)
        {
            int fi;

            facesV = new PPolygon[mesh.numFaces];

            for (fi = 0; fi < mesh.numFaces; fi++)
            {
                facesV[fi].tag1 = 0;
                facesV[fi].tag2 = PPOLY_TAG2; // 0xCFCEA00

                facesV[fi].Verts = new ushort[3];
                facesV[fi].Verts[0] = (ushort)mesh.facesV[fi].vertC;
                facesV[fi].Verts[1] = (ushort)mesh.facesV[fi].vertB;
                facesV[fi].Verts[2] = (ushort)mesh.facesV[fi].vertA;

                facesV[fi].Normals = new ushort[3];
                facesV[fi].Edges = new ushort[3];
            }
        }

        private static void GetTexCoords(Mesh_object_node mesh, out Point2D[] texCoordsV)
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

        //private static void GetVColors(Mesh_object_node mesh, Mat_list_node[] materialsV,
        //                               out Color[] vcolorsV)
        //{
        //    int iVColorIdx, iPolyIdx;
        //    STIntVector[] stFacesPerVert;
        //    int iTmpR, iTmpG, iTmpB, iVectorCount;

        //    int iVertIdx, iPolyCounterIdx;

        //    stFacesPerVert = new STIntVector[mesh.numVerts];

        //    for (iPolyIdx = 0; iPolyIdx < mesh.numFaces; iPolyIdx++)
        //    {
        //        iVertIdx = mesh.facesV[iPolyIdx].vertA;
        //        iPolyCounterIdx = stFacesPerVert[iVertIdx].length;
        //        Array.Resize(ref stFacesPerVert[iVertIdx].vector, iPolyCounterIdx + 1);
        //        stFacesPerVert[iVertIdx].vector[iPolyCounterIdx] = iPolyIdx;
        //        stFacesPerVert[iVertIdx].length = iPolyCounterIdx + 1;

        //        iVertIdx = mesh.facesV[iPolyIdx].vertB;
        //        iPolyCounterIdx = stFacesPerVert[iVertIdx].length;
        //        Array.Resize(ref stFacesPerVert[iVertIdx].vector, iPolyCounterIdx + 1);
        //        stFacesPerVert[iVertIdx].vector[iPolyCounterIdx] = iPolyIdx;
        //        stFacesPerVert[iVertIdx].length = iPolyCounterIdx + 1;

        //        iVertIdx = mesh.facesV[iPolyIdx].vertC;
        //        iPolyCounterIdx = stFacesPerVert[iVertIdx].length;
        //        Array.Resize(ref stFacesPerVert[iVertIdx].vector, iPolyCounterIdx + 1);
        //        stFacesPerVert[iVertIdx].vector[iPolyCounterIdx] = iPolyIdx;
        //        stFacesPerVert[iVertIdx].length = iPolyCounterIdx + 1;
        //    }

        //    vcolorsV = new Color[mesh.numVerts];

        //    if (mesh.faceMaterialIndicesV != null)
        //    {

        //        for (iVColorIdx = 0; iVColorIdx < mesh.numVerts; iVColorIdx++)
        //        {

        //            iTmpR = iTmpG = iTmpB = 0;
        //            iVectorCount = 0;

        //            foreach (int iVectIdx in stFacesPerVert[iVColorIdx].vector)
        //            {
        //                iTmpR += materialsV[mesh.faceMaterialIndicesV[iVectIdx]].diffuse.red;
        //                iTmpG += materialsV[mesh.faceMaterialIndicesV[iVectIdx]].diffuse.green;
        //                iTmpB += materialsV[mesh.faceMaterialIndicesV[iVectIdx]].diffuse.blue;

        //                iVectorCount++;
        //            }

        //            vcolorsV[iVColorIdx] = Color.FromArgb(255, iTmpR / iVectorCount, 
        //                                                       iTmpG / iVectorCount, 
        //                                                       iTmpB / iVectorCount);

        //        }
        //    }
        //}

        private static void GetVColors(Mesh_object_node mesh, Mat_list_node[] materialsV,
                               out Color[] vcolorsV)
        {
            int iVColorIdx, iPolyIdx;
            STIntVector[] stFacesPerVert;
            int iTmpR, iTmpG, iTmpB;

            int iVertIdx, iPolyCounterIdx;

            stFacesPerVert = new STIntVector[mesh.numVerts];

            for (iPolyIdx = 0; iPolyIdx < mesh.numFaces; iPolyIdx++)
            {
                iVertIdx = mesh.facesV[iPolyIdx].vertA;
                iPolyCounterIdx = stFacesPerVert[iVertIdx].length;
                Array.Resize(ref stFacesPerVert[iVertIdx].vector, iPolyCounterIdx + 1);
                stFacesPerVert[iVertIdx].vector[iPolyCounterIdx] = iPolyIdx;
                stFacesPerVert[iVertIdx].length = iPolyCounterIdx + 1;

                iVertIdx = mesh.facesV[iPolyIdx].vertB;
                iPolyCounterIdx = stFacesPerVert[iVertIdx].length;
                Array.Resize(ref stFacesPerVert[iVertIdx].vector, iPolyCounterIdx + 1);
                stFacesPerVert[iVertIdx].vector[iPolyCounterIdx] = iPolyIdx;
                stFacesPerVert[iVertIdx].length = iPolyCounterIdx + 1;

                iVertIdx = mesh.facesV[iPolyIdx].vertC;
                iPolyCounterIdx = stFacesPerVert[iVertIdx].length;
                Array.Resize(ref stFacesPerVert[iVertIdx].vector, iPolyCounterIdx + 1);
                stFacesPerVert[iVertIdx].vector[iPolyCounterIdx] = iPolyIdx;
                stFacesPerVert[iVertIdx].length = iPolyCounterIdx + 1;
            }

            vcolorsV = new Color[mesh.numVerts];

            for (iVColorIdx = 0; iVColorIdx < mesh.numVerts; iVColorIdx++)
            {
                iTmpR = 0;
                iTmpG = 0;
                iTmpB = 0;

                for (iPolyIdx = 0; iPolyIdx < stFacesPerVert[iVColorIdx].length; iPolyIdx++)
                {
                    if (mesh.faceMaterialIndicesV != null)
                    {
                        iTmpR += materialsV[mesh.faceMaterialIndicesV[stFacesPerVert[iVColorIdx].
                                        vector[iPolyIdx]]].diffuse.red;
                        iTmpG += materialsV[mesh.faceMaterialIndicesV[stFacesPerVert[iVColorIdx].
                                        vector[iPolyIdx]]].diffuse.green;
                        iTmpB += materialsV[mesh.faceMaterialIndicesV[stFacesPerVert[iVColorIdx].
                                        vector[iPolyIdx]]].diffuse.blue;
                    }
                }

                if (stFacesPerVert[iVColorIdx].length != 0)
                {
                    vcolorsV[iVColorIdx] = Color.FromArgb(255,
                                                  iTmpR / stFacesPerVert[iVColorIdx].length,
                                                  iTmpG / stFacesPerVert[iVColorIdx].length,
                                                  iTmpB / stFacesPerVert[iVColorIdx].length);
                }
            }
        }

        //private static void GetPColors(Mesh_object_node mesh, Mat_list_node[] materialsV, out Color[] pcolorsV)
        //{
        //    int iPColorIdx;

        //    pcolorsV = new Color[mesh.numFaces];

        //    for (iPColorIdx = 0; iPColorIdx < mesh.numFaces; iPColorIdx++)
        //    {
        //        if (mesh.faceMaterialIndicesV != null)                
        //            pcolorsV[iPColorIdx] = Color.FromArgb(255,
        //                materialsV[mesh.faceMaterialIndicesV[iPColorIdx]].diffuse.red,
        //                materialsV[mesh.faceMaterialIndicesV[iPColorIdx]].diffuse.green,
        //                materialsV[mesh.faceMaterialIndicesV[iPColorIdx]].diffuse.blue);
        //    }
        //}

        private static void GetPColors(Mesh_object_node mesh, Mat_list_node[] materialsV, out Color[] pcolorsV)
        {
            int iPColorIdx;

            pcolorsV = new Color[mesh.numFaces];

            for (iPColorIdx = 0; iPColorIdx < mesh.numFaces; iPColorIdx++)
            {
                if (mesh.faceMaterialIndicesV != null)
                {
                    pcolorsV[iPColorIdx] = Color.FromArgb(255,
                        pcolorsV[iPColorIdx].R + materialsV[mesh.faceMaterialIndicesV[iPColorIdx]].diffuse.red,
                        pcolorsV[iPColorIdx].G + materialsV[mesh.faceMaterialIndicesV[iPColorIdx]].diffuse.green,
                        pcolorsV[iPColorIdx].B + materialsV[mesh.faceMaterialIndicesV[iPColorIdx]].diffuse.blue);
                }
            }
        }

        private static void ConvertMesh3DSToPModel(Mesh_object_node mesh, Mat_list_node[] materialsV, ref PModel Model)
        {

            GetVerts(mesh, out Point3D[] vertsV);
            GetFaces(mesh, out PPolygon[] facesV);
            GetTexCoords(mesh, out Point2D[] texcoordsV);
            GetVColors(mesh, materialsV, out Color[] vcolorsV);
            GetPColors(mesh, materialsV, out Color[] pcolorsV);

            AddGroup(ref Model, vertsV, facesV, texcoordsV, vcolorsV, pcolorsV, 0);

        }

        public static void ConvertModel3DSToPModel(Model3DS Model, ref PModel outModel, bool bAdjust3DSModel)
        {
            int iMeshIdx, iNumMeshes;

            if (Model.meshesV != null)
            {
                iNumMeshes = Model.meshesV.Length;

                for (iMeshIdx = 0; iMeshIdx < iNumMeshes; iMeshIdx++)
                {
                    // Let's check the max number of polys and vertices
                    if (Model.meshesV[iMeshIdx].vertsV.Length > 0xFFFF)
                    {
                        MessageBox.Show("The mesh number: " + iMeshIdx.ToString() + " has " +
                                        Model.meshesV[iMeshIdx].vertsV.Length.ToString() + " vertices.\n" +
                                        "The max number of vertices allowed for a FF7 .P model is 65535.\n" +
                                        "The process of importing this mesh is cancelled.", "Warning", 
                                        MessageBoxButtons.OK);

                    }
                    else if (Model.meshesV[iMeshIdx].facesV.Length > 0xFFFF)
                    {
                        MessageBox.Show("The mesh number: " + iMeshIdx.ToString() + " has " +
                                        Model.meshesV[iMeshIdx].facesV.Length.ToString() + " faces.\n" +
                                        "The max number of faces allowed for a FF7 .P model is 65535.\n" +
                                        "The process of importing this mesh is cancelled.", "Warning",
                                        MessageBoxButtons.OK);
                    }
                    else
                    {
                        ConvertMesh3DSToPModel(Model.meshesV[iMeshIdx], Model.materialsV, ref outModel);

                        if (bAdjust3DSModel)
                        {
                            // Let's mirror and rotate to put the mesh in correct FF7 models position
                            // Mirror model on Z plane
                            MirrorGroupRelativeToPlane(ref outModel, outModel.Header.numGroups - 1,
                                                       0, 0, 1, 0);

                            // Rotate 180º Y axis will do this here, after adding the group
                            // when importing .3ds file
                            outModel.Groups[outModel.Header.numGroups - 1].rotGroupGamma = 180;
                        }
                    }
                }
            }
        }

        public static void ConvertModels3DSToPModel(Model3DS[] modelsV, ref PModel outModel, 
                                                    bool bAdjust3DSModel)
        {
            int iModelIdx, iNumModels;

            iNumModels = modelsV.Length;

            for (iModelIdx = 0; iModelIdx < iNumModels; iModelIdx++)
            {
                ConvertModel3DSToPModel(modelsV[iModelIdx], ref outModel, bAdjust3DSModel);
            }

            outModel.Header.version = 1;
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

            //ComputeNormals(ref outModel);
            //ComputeBoundingBox(ref outModel);
            //ComputeEdges(ref outModel);
        }
    }
}
