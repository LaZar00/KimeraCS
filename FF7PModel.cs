using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{

    using Defines;

    using static FrmPEditor;

    using static FF7Skeleton;

    using static ModelDrawing;

    using static Utils;
    using static OpenGL32;
    using static FileTools;

    public class FF7PModel
    {

        public const int PPOLY_TAG2 = 0xCFCEA00;  //217901568
        public const int I_COMPUTENORMALS_VERTEXTHRESHOLD = 58;

        public struct PHeader
        {
            public int version;
            public int off04;
            public int vertexColor;
            public int numVerts;
            public int numNormals;
            public int numXYZ;
            public int numTexCs;
            public int numNormIdx;
            public int numEdges;
            public int numPolys;
            public int off28;
            public int off2C;
            public int mirex_h;
            public int numGroups;
            public int mirex_g;
            public int off3C;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public int[] unknown;
        }

        public struct PEdge
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public ushort[] Verts;

            // This is for create a new deep copy of PEdge
            // We will use normally the creator like '= new PEdge();' but there are some exceptions
            public PEdge(PEdge pedgeIn)
            {
                Verts = new ushort[pedgeIn.Verts.Length];
                pedgeIn.Verts.CopyTo(Verts, 0);
            }
        }

        public struct PPolygon
        {
            public short tag1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public ushort[] Verts;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public ushort[] Normals;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public ushort[] Edges;
            public int tag2;

            // This is for create a new empty PPolygon with arrays defined
            public PPolygon(int tag2In)
            {
                tag1 = 0;
                tag2 = tag2In;

                Verts = new ushort[3];
                Normals = new ushort[3];
                Edges = new ushort[3];
            }

            // This is for create a new deep copy of PPolygon
            // We will use normally the creator like '= new PPolygon();' but there are some exceptions
            public PPolygon(PPolygon ppolygonIn)
            {
                tag1 = ppolygonIn.tag1;
                tag2 = ppolygonIn.tag2;

                Verts = new ushort[ppolygonIn.Verts.Length];
                ppolygonIn.Verts.CopyTo(Verts, 0);

                Normals = new ushort[ppolygonIn.Normals.Length];
                ppolygonIn.Normals.CopyTo(Normals, 0);

                Edges = new ushort[ppolygonIn.Edges.Length];
                ppolygonIn.Edges.CopyTo(Edges, 0);
            }
        }

        // Masks for field_8 and field_C:
        // 0x1: V_WIREFRAME
        // 0x2: V_TEXTURE
        // 0x4: V_LINEARFILTER
        // 0x8: V_PERSPECTIVE
        // 0x10: V_TMAPBLEND
        // 0x20: V_WRAP_U
        // 0x40: V_WRAP_V
        // 0x80: V_UNKNOWN80
        // 0x100: V_COLORKEY
        // 0x200: V_DITHER
        // 0x400: V_ALPHABLEND
        // 0x800: V_ALPHATEST
        // 0x1000: V_ANTIALIAS
        // 0x2000: V_CULLFACE
        // 0x4000: V_NOCULL
        // 0x8000: V_DEPTHTEST
        // 0x10000: V_DEPTHMASK
        // 0x20000: V_SHADEMODE
        // 0x40000: V_SPECULAR
        // 0x80000: V_LIGHTSTATE
        // 0x100000: V_FOG
        // 0x200000: V_TEXADDR
        public struct PHundret
        {
            public int field_0;
            public int field_4;
            public int field_8;            // Render state part value (if it's actually changed)
            public int field_C;            // Change render state part?
            public int texID;              // Texture identifier for the corresponding group. For consistency sake should be the same as on the group,
                                           // but this is the one FF7 actually uses.
            public int texture_set_ptr;    //This should be filled in real time
            public int field_18;
            public int field_1C;
            public int field_20;
            public int shademode;
            public int lightstate_ambient;
            public int field_2C;
            public int lightstate_material_ptr;
            public int srcblend;
            public int destblend;
            public int field_3C;
            public int alpharef;
            public int blend_mode;     // 0 - Average, source color / 2 + destination color / 2.
                                       // 1 - Additive, source color + destination color.
                                       // 2 - Subtractive, broken and unused but it should be destination color - source color.
                                       // 3 - Not sure, but it seems broken and is never used.
                                       // 4 - No blending (FF8 only)
            public int zSort;          // Filled in real time
            public int field_4C;
            public int field_50;
            public int field_54;
            public int field_58;
            public int vertex_alpha;
            public int field_60;
        }

        public struct PGroup
        {
            public int polyType;    //  Specifies the Polygon Type for this Group:
                                    //  1 - nontextured Polygons
                                    //  2 - textured Polygons with normals
                                    //  3 - textured Polygons without normals
            public int offsetPoly;
            public int numPoly;
            public int offsetVert;
            public int numVert;
            public int offsetEdge;
            public int numEdge;
            public int off1C;
            public int off20;
            public int off24;
            public int off28;
            public int offsetTex;
            public int texFlag;
            public int texID;
            // added attributes
            public float repGroupX, repGroupY, repGroupZ;
            public float rszGroupX, rszGroupY, rszGroupZ;
            public float rotGroupAlpha, rotGroupBeta, rotGroupGamma;
            public Quaternion rotationQuaternionGroup;
            public int DListNum;
            public bool HiddenQ;           // Hidden groups aren't rendered and can't be changed _
                                           // save for the basic geometrical transformations(rotation, scaling and panning),
                                           // palletizzed opeartions and group deletion
            public int realGID;            // We will use this as maintain the real Group position number of the list for
                                           // Remove/Duplicate/Add features. This is the Border DListNum?
        }

        public struct PBoundingBox
        {
            public int unknown4bytes;           // It seems that there are 4bytes before BoundingBox. This 4 bytes are unknown.

            public float max_x;
            public float max_y;
            public float max_z;
            public float min_x;
            public float min_y;
            public float min_z;
        }

        public struct PModel
        {
            public string fileName;
            public PHeader Header;
            public Point3D[] Verts;
            public Point3D[] Normals;
            public Point3D[] XYZ;
            public Point2D[] TexCoords;
            public Color[] Vcolors;
            public Color[] Pcolors;
            public PEdge[] Edges;
            public PPolygon[] Polys;
            public PHundret[] Hundrets;
            public PGroup[] Groups;
            public PBoundingBox BoundingBox;
            public int[] NormalIndex;
            // added attributes
            public float repositionX, repositionY, repositionZ;
            public float resizeX, resizeY, resizeZ;
            public float rotateAlpha, rotateBeta, rotateGamma;
            public Quaternion rotationQuaternion;
            public float diameter;
            public int DListNum;

        }


        public static void LoadPModel(ref PModel Model, string strPFolder, string strPFileName, bool bComputeNormals)
        {
            byte[] fileBuffer;
            long fileBufferPos = 0;
            string strPFullFileName = strPFolder + "\\" + strPFileName;

            // Let's read P file into memory.
            // First check if exists
            if (!File.Exists(strPFullFileName))
            {
                //  Debug.Print fileName
                throw new FileNotFoundException("Error opening .P Model " + strPFileName + " file.");
            }

            // Read All P Model file into memory
            fileBuffer = File.ReadAllBytes(strPFullFileName);

            //// Read P Model structure.
            // Put name of P file
            Model.fileName = strPFileName.ToUpper();

            // Header
            Model.Header = new PHeader();
            ReadPHeader(fileBuffer, ref fileBufferPos, ref Model.Header, strPFullFileName);

            // Check numVerts
            if (Model.Header.numVerts <= 0)
            {
                throw new ApplicationException("The number of vertices of the P Model: " + Model.fileName + 
                                               " is not correct.");
            }

            // Verts
            Model.Verts = new Point3D[Model.Header.numVerts];
            ReadPVerts(fileBuffer, ref fileBufferPos, Model.Header.numVerts, ref Model.Verts);

            // Normals
            Model.Normals = new Point3D[Model.Header.numNormals];
            ReadPNormals(fileBuffer, ref fileBufferPos, Model.Header.numNormals, ref Model.Normals);

            // TryVerts
            if (Model.Header.numXYZ > 0)
            {
                Model.XYZ = new Point3D[Model.Header.numXYZ];
                ReadPXYZ(fileBuffer, ref fileBufferPos, Model.Header.numXYZ, ref Model.XYZ);
            }
            //else
            //{
            //    Model.Header.numXYZ = 200;
            //    Model.XYZ = new Point3D[Model.Header.numXYZ];
            //}

            // Texture Coordinates
            Model.TexCoords = new Point2D[Model.Header.numTexCs];
            ReadPTexCoords(fileBuffer, ref fileBufferPos, Model.Header.numTexCs, ref Model.TexCoords);

            //  Vertex Colors
            Model.Vcolors = new Color[Model.Header.numVerts];
            ReadPPVColors(fileBuffer, ref fileBufferPos, Model.Header.numVerts, ref Model.Vcolors);

            //  Polygon Colors
            Model.Pcolors = new Color[Model.Header.numPolys];
            ReadPPVColors(fileBuffer, ref fileBufferPos, Model.Header.numPolys, ref Model.Pcolors);

            // Edges
            Model.Edges = new PEdge[Model.Header.numEdges];
            ReadPEdges(fileBuffer, ref fileBufferPos, Model.Header.numEdges, ref Model.Edges);

            // Polygons
            Model.Polys = new PPolygon[Model.Header.numPolys];
            ReadPPolygons(fileBuffer, ref fileBufferPos, Model.Header.numPolys, ref Model.Polys);

            // Hundrets
            Model.Hundrets = new PHundret[Model.Header.mirex_h];
            ReadPHundrets(fileBuffer, ref fileBufferPos, Model.Header.mirex_h, ref Model.Hundrets);

            // Groups
            Model.Groups = new PGroup[Model.Header.numGroups];
            ReadPGroups(fileBuffer, ref fileBufferPos, Model.Header.numGroups, ref Model.Groups);

            // BoundingBox
            Model.BoundingBox = new PBoundingBox();
            ReadPBoundingBox(fileBuffer, ref fileBufferPos, ref Model.BoundingBox);

            // NormalIndex
            Model.NormalIndex = new int[Model.Header.numNormIdx];
            ReadPNormalIndex(fileBuffer, ref fileBufferPos, Model.Header.numNormIdx, ref Model.NormalIndex);

            // added attributes
            Model.resizeX = 1;
            Model.resizeY = 1;
            Model.resizeZ = 1;
            Model.rotateAlpha = 0;
            Model.rotateBeta = 0;
            Model.rotateGamma = 0;

            Model.rotationQuaternion.x = 0;
            Model.rotationQuaternion.y = 0;
            Model.rotationQuaternion.z = 0;
            Model.rotationQuaternion.w = 1;

            Model.repositionX = 0;
            Model.repositionY = 0;
            Model.repositionZ = 0;
            Model.diameter = ComputeDiameter(Model.BoundingBox);

            Model.DListNum = 0;

            RepairGroups(ref Model);
            AssignRealGID(ref Model);
            //CheckModelConsistency(ref Model);

            if (!bDontCheckRepairPolys) RepairPolys(ref Model);

            KillUnusedVertices(ref Model);
            ComputeBoundingBox(ref Model);

            if (bComputeNormals)
            {
                ComputeEdges(ref Model);
                ComputeNormals(ref Model);
            }

            CreateDListsFromPModel(ref Model);
        }



        /////////////////////////////////////////////////////////////////////////////////////////////
        // Load Model functions
        public static int ReadPHeader(byte[] fileBuffer, ref long pos, ref PHeader Header, string fileName)
        {

            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    Header.version = memReader.ReadInt32();
                    Header.off04 = memReader.ReadInt32();

                    if (Header.version != 1 || Header.off04 != 1)
                    {
                        MessageBox.Show("The file header of the P file " + fileName + " is not correct.",
                                        "Error");
                        return 0;
                    }

                    Header.vertexColor = memReader.ReadInt32();
                    Header.numVerts = memReader.ReadInt32();
                    Header.numNormals = memReader.ReadInt32();
                    Header.numXYZ = memReader.ReadInt32();
                    Header.numTexCs = memReader.ReadInt32();
                    Header.numNormIdx = memReader.ReadInt32();
                    Header.numEdges = memReader.ReadInt32();
                    Header.numPolys = memReader.ReadInt32();
                    Header.off28 = memReader.ReadInt32();
                    Header.off2C = memReader.ReadInt32();
                    Header.mirex_h = memReader.ReadInt32();
                    Header.numGroups = memReader.ReadInt32();
                    Header.mirex_g = memReader.ReadInt32();
                    Header.off3C = memReader.ReadInt32();

                    Header.unknown = new int[16];

                    for (var i = 0; i < 16; i++) Header.unknown[i] = memReader.ReadInt32();

                    pos = memReader.BaseStream.Position;
                }
            }

            return 1;
        }

        public static void ReadPVerts(byte[] fileBuffer, ref long pos, long numVerts, ref Point3D[] Verts)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numVerts; i++)
                    {
                        Verts[i].x = memReader.ReadSingle();
                        Verts[i].y = memReader.ReadSingle();
                        Verts[i].z = memReader.ReadSingle();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPNormals(byte[] fileBuffer, ref long pos, long numNormals, ref Point3D[] Normals)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    if (numNormals > 0)
                    {
                        for (var i = 0; i < numNormals; i++)
                        {
                            Normals[i].x = memReader.ReadSingle();
                            Normals[i].y = memReader.ReadSingle();
                            Normals[i].z = memReader.ReadSingle();
                        }
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPXYZ(byte[] fileBuffer, ref long pos, long numTryVerts, ref Point3D[] TryVerts)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numTryVerts; i++)
                    {
                        TryVerts[i].x = memReader.ReadSingle();
                        TryVerts[i].y = memReader.ReadSingle();
                        TryVerts[i].z = memReader.ReadSingle();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPTexCoords(byte[] fileBuffer, ref long pos, long numTexCs, ref Point2D[] TexCoordinates)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    if (numTexCs > 0)
                    {
                        for (var i = 0; i < numTexCs; i++)
                        {
                            TexCoordinates[i].x = memReader.ReadSingle();
                            TexCoordinates[i].y = memReader.ReadSingle();
                        }

                        pos = memReader.BaseStream.Position;
                    }
                }
            }
        }

        public static void ReadPPVColors(byte[] fileBuffer, ref long pos, long numVerts, ref Color[] Vcolors)
        {
            byte red, green, blue, alpha;

            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numVerts; i++)
                    {
                        blue = memReader.ReadByte();
                        green = memReader.ReadByte();
                        red = memReader.ReadByte();
                        alpha = memReader.ReadByte();

                        Vcolors[i] = Color.FromArgb(alpha, red, green, blue);
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPEdges(byte[] fileBuffer, ref long pos, long numEdges, ref PEdge[] Edges)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numEdges; i++)
                    {
                        Edges[i].Verts = new ushort[2];

                        Edges[i].Verts[0] = memReader.ReadUInt16();
                        Edges[i].Verts[1] = memReader.ReadUInt16();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPPolygons(byte[] fileBuffer, ref long pos, long numPolys, ref PPolygon[] Polys)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numPolys; i++)
                    {
                        Polys[i].tag1 = memReader.ReadInt16();

                        Polys[i].Verts = new ushort[3];
                        Polys[i].Verts[0] = memReader.ReadUInt16();
                        Polys[i].Verts[1] = memReader.ReadUInt16();
                        Polys[i].Verts[2] = memReader.ReadUInt16();

                        Polys[i].Normals = new ushort[3];
                        Polys[i].Normals[0] = memReader.ReadUInt16();
                        Polys[i].Normals[1] = memReader.ReadUInt16();
                        Polys[i].Normals[2] = memReader.ReadUInt16();

                        Polys[i].Edges = new ushort[3];
                        Polys[i].Edges[0] = memReader.ReadUInt16();
                        Polys[i].Edges[1] = memReader.ReadUInt16();
                        Polys[i].Edges[2] = memReader.ReadUInt16();

                        Polys[i].tag2 = memReader.ReadInt32();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPHundrets(byte[] fileBuffer, ref long pos, long numHundrets, ref PHundret[] Hundrets)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numHundrets; i++)
                    {
                        Hundrets[i].field_0 = memReader.ReadInt32();
                        Hundrets[i].field_4 = memReader.ReadInt32();
                        Hundrets[i].field_8 = memReader.ReadInt32();
                        Hundrets[i].field_C = memReader.ReadInt32();
                        Hundrets[i].texID = memReader.ReadInt32();
                        Hundrets[i].texture_set_ptr = memReader.ReadInt32();
                        Hundrets[i].field_18 = memReader.ReadInt32();
                        Hundrets[i].field_1C = memReader.ReadInt32();
                        Hundrets[i].field_20 = memReader.ReadInt32();
                        Hundrets[i].shademode = memReader.ReadInt32();
                        Hundrets[i].lightstate_ambient = memReader.ReadInt32();
                        Hundrets[i].field_2C = memReader.ReadInt32();
                        Hundrets[i].lightstate_material_ptr = memReader.ReadInt32();
                        Hundrets[i].srcblend = memReader.ReadInt32();
                        Hundrets[i].destblend = memReader.ReadInt32();
                        Hundrets[i].field_3C = memReader.ReadInt32();
                        Hundrets[i].alpharef = memReader.ReadInt32();
                        Hundrets[i].blend_mode = memReader.ReadInt32();
                        Hundrets[i].zSort = memReader.ReadInt32();
                        Hundrets[i].field_4C = memReader.ReadInt32();
                        Hundrets[i].field_50 = memReader.ReadInt32();
                        Hundrets[i].field_54 = memReader.ReadInt32();
                        Hundrets[i].field_58 = memReader.ReadInt32();
                        Hundrets[i].vertex_alpha = memReader.ReadInt32();
                        Hundrets[i].field_60 = memReader.ReadInt32();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPGroups(byte[] fileBuffer, ref long pos, long numGroups, ref PGroup[] Groups)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numGroups; i++)
                    {
                        Groups[i].polyType = memReader.ReadInt32();
                        Groups[i].offsetPoly = memReader.ReadInt32();
                        Groups[i].numPoly = memReader.ReadInt32();
                        Groups[i].offsetVert = memReader.ReadInt32();
                        Groups[i].numVert = memReader.ReadInt32();
                        Groups[i].offsetEdge = memReader.ReadInt32();
                        Groups[i].numEdge = memReader.ReadInt32();
                        Groups[i].off1C = memReader.ReadInt32();
                        Groups[i].off20 = memReader.ReadInt32();
                        Groups[i].off24 = memReader.ReadInt32();
                        Groups[i].off28 = memReader.ReadInt32();
                        Groups[i].offsetTex = memReader.ReadInt32();
                        Groups[i].texFlag = memReader.ReadInt32();
                        Groups[i].texID = memReader.ReadInt32();
                        // added attributes
                        Groups[i].rszGroupX = 1;
                        Groups[i].rszGroupY = 1;
                        Groups[i].rszGroupZ = 1;
                        Groups[i].repGroupX = 0;
                        Groups[i].repGroupY = 0;
                        Groups[i].repGroupZ = 0;
                        Groups[i].rotGroupAlpha = 0;
                        Groups[i].rotGroupBeta = 0;
                        Groups[i].rotGroupGamma = 0;

                        Groups[i].rotationQuaternionGroup = new Quaternion() { x = 0, y = 0, z = 0, w = 1 };

                        Groups[i].DListNum = -1;
                        Groups[i].HiddenQ = false;
                        Groups[i].realGID = i;
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPBoundingBox(byte[] fileBuffer, ref long pos, ref PBoundingBox BoundingBox)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    // There are .P models, like magic/bari_a1 and magic/bari_a2 that
                    // does not seem to have this unknown4bytes.
                    if (memReader.BaseStream.Length - memReader.BaseStream.Position - 24 > 0)
                        BoundingBox.unknown4bytes = memReader.ReadInt32();           // It seems that there are 4bytes before BoundingBox. This 4 bytes are unknown.

                    BoundingBox.max_x = memReader.ReadSingle();
                    BoundingBox.max_y = memReader.ReadSingle();
                    BoundingBox.max_z = memReader.ReadSingle();

                    // There are .P models, like magic/bari_a2 that
                    // does not seem to have min_x.
                    if (memReader.BaseStream.Length - memReader.BaseStream.Position - 12 > 0)
                    {
                        BoundingBox.min_x = memReader.ReadSingle();
                        BoundingBox.min_y = memReader.ReadSingle();
                        BoundingBox.min_z = memReader.ReadSingle();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPNormalIndex(byte[] fileBuffer, ref long pos, int numNormIdx, ref int[] NormalIndex)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; (i < numNormIdx && memReader.BaseStream.Position < memReader.BaseStream.Length); i++)
                        NormalIndex[i] = memReader.ReadInt32();

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        // In this procedure we will check the realGID assigned when loading
        // the model is correct (this equals the offsetPoly incremental number order).
        public static void AssignRealGID(ref PModel Model)
        {
            int iGroupIdx, iGroupIdxCheck, iMinOffsetPoly = 0, iMaxOffsetPoly = 999999, iGroupFound = 0, iRealGIDCounter = 0;

            if (Model.Header.numGroups > 1)
            {

                iGroupIdx = 0;
                while (iGroupIdx < Model.Header.numGroups)
                {

                    if (iGroupIdx == 0)
                    {
                        iGroupIdxCheck = 0;
                        while (Model.Groups[iGroupIdxCheck].offsetPoly != 0 &&
                               Model.Groups[iGroupIdxCheck].numPoly > 0) iGroupIdxCheck++;

                        iGroupFound = iGroupIdxCheck;
                    }
                    else
                    {
                        iGroupIdxCheck = 0;
                        while (iGroupIdxCheck < Model.Header.numGroups)
                        {
                            if (Model.Groups[iGroupIdxCheck].offsetPoly < iMaxOffsetPoly &&
                                Model.Groups[iGroupIdxCheck].offsetPoly > iMinOffsetPoly)
                            {
                                iMaxOffsetPoly = Model.Groups[iGroupIdxCheck].offsetPoly;
                                iGroupFound = iGroupIdxCheck;
                            }

                            iGroupIdxCheck++;
                        }
                    }

                    Model.Groups[iGroupFound].realGID = iRealGIDCounter;
                    iMinOffsetPoly = Model.Groups[iGroupFound].offsetPoly;
                    iMaxOffsetPoly = 99999999;
                    iRealGIDCounter++;

                    iGroupIdx++;
                }
            }
        }



        //  ---------------------------------------------------------------------------------------------------------
        //  ------------------------------------------ COMBINING/MERGING --------------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        public static void CombineGroups(ref PGroup[] outMergedGroup, PGroup[] inGroup)
        {
            int iGroupIdx, maxtiGroup, numGroupsMergedGroup, numGroupsinGroup,
                iNumPolys, iNumEdges, iNumVerts, iNumTexCs;

            numGroupsMergedGroup = outMergedGroup.Length;
            numGroupsinGroup = inGroup.Length;

            Array.Resize(ref outMergedGroup, numGroupsMergedGroup + numGroupsinGroup);

            maxtiGroup = 0;

            for (iGroupIdx = 0; iGroupIdx < numGroupsMergedGroup; iGroupIdx++)
            {
                if (outMergedGroup[iGroupIdx].texFlag == 1)
                    if (outMergedGroup[iGroupIdx].texID > maxtiGroup) 
                            maxtiGroup = outMergedGroup[iGroupIdx].texID;
            }

            iNumPolys = outMergedGroup[numGroupsMergedGroup - 1].offsetPoly + 
                        outMergedGroup[numGroupsMergedGroup - 1].numPoly;
            iNumEdges = outMergedGroup[numGroupsMergedGroup - 1].offsetEdge + 
                        outMergedGroup[numGroupsMergedGroup - 1].numEdge;
            iNumVerts = outMergedGroup[numGroupsMergedGroup - 1].offsetVert + 
                        outMergedGroup[numGroupsMergedGroup - 1].numVert;

            if (outMergedGroup[numGroupsMergedGroup].texFlag == 1)
                iNumTexCs = outMergedGroup[numGroupsMergedGroup - 1].offsetTex + 
                            outMergedGroup[numGroupsMergedGroup - 1].numVert;
            else
                iNumTexCs = outMergedGroup[numGroupsMergedGroup - 1].offsetTex;

            for (iGroupIdx = 0; iGroupIdx < numGroupsinGroup; iGroupIdx++)
            {
                inGroup[iGroupIdx].offsetPoly = inGroup[iGroupIdx].offsetPoly + iNumPolys;
                inGroup[iGroupIdx].offsetVert = inGroup[iGroupIdx].offsetVert + iNumVerts;
                inGroup[iGroupIdx].offsetEdge = inGroup[iGroupIdx].offsetEdge + iNumEdges;
                inGroup[iGroupIdx].offsetTex = inGroup[iGroupIdx].offsetTex + iNumTexCs;

                if (inGroup[iGroupIdx].texFlag == 1) 
                    inGroup[iGroupIdx].texID = inGroup[iGroupIdx].texID + maxtiGroup;

                outMergedGroup[numGroupsMergedGroup + iGroupIdx] = inGroup[iGroupIdx];
            }
        }

        public static void MergeBoundingBox(ref PBoundingBox BoundingBox, PBoundingBox inBoundingBox)
        {
            if (BoundingBox.max_x < inBoundingBox.max_x) BoundingBox.max_x = inBoundingBox.max_x;
            if (BoundingBox.max_y < inBoundingBox.max_y) BoundingBox.max_y = inBoundingBox.max_y;
            if (BoundingBox.max_z < inBoundingBox.max_z) BoundingBox.max_z = inBoundingBox.max_z;

            if (BoundingBox.min_x > inBoundingBox.min_x) BoundingBox.min_x = inBoundingBox.min_x;
            if (BoundingBox.min_y > inBoundingBox.min_y) BoundingBox.min_y = inBoundingBox.min_y;
            if (BoundingBox.min_z > inBoundingBox.min_z) BoundingBox.min_z = inBoundingBox.min_z;
        }

        public static void MergeHeader(ref PHeader Header, PHeader inHeader)
        {
            Header.numVerts += inHeader.numVerts;
            Header.numNormals += inHeader.numNormals;
            Header.numTexCs += inHeader.numTexCs;
            Header.numNormIdx += inHeader.numNormIdx;
            Header.numEdges += inHeader.numEdges;
            Header.numPolys += inHeader.numPolys;
            Header.mirex_h += inHeader.mirex_h;
            Header.numGroups += inHeader.numGroups;
        }

        public static void MergePModels(ref PModel Model, PModel inModel)
        {
            try
            {
                // Merge Verts
                Model.Verts = Model.Verts.Concat(inModel.Verts).ToArray();
                // Merge Normals
                //Model.Normals = Model.Normals.Concat(inModel.Normals).ToArray();

                // Texture Coordinates
                if (Model.Header.numTexCs == 0) 
                    Model.TexCoords = inModel.TexCoords;
                else 
                    if (inModel.Header.numTexCs > 0) 
                        Model.TexCoords = Model.TexCoords.Concat(inModel.TexCoords).ToArray();

                // VColors
                Model.Vcolors = Model.Vcolors.Concat(inModel.Vcolors).ToArray();
                // PColors
                Model.Pcolors = Model.Pcolors.Concat(inModel.Pcolors).ToArray();

                // Model.Edges = Model.Edges.Concat(inModel.Edges).ToArray();

                // PPolygons
                Model.Polys = Model.Polys.Concat(inModel.Polys).ToArray();

                // Hundrets
                Model.Hundrets = Model.Hundrets.Concat(inModel.Hundrets).ToArray();

                // Groups
                CombineGroups(ref Model.Groups, inModel.Groups);

                // BoundingBoxes
                MergeBoundingBox(ref Model.BoundingBox, inModel.BoundingBox);

                //// Normal's Indexes
                //Model.NormalIndex = Model.NormalIndex.Concat(inModel.NormalIndex).ToArray();

                // Headers
                MergeHeader(ref Model.Header, inModel.Header);

                ComputeNormals(ref Model);
                ComputeEdges(ref Model);

                CheckModelConsistency(ref Model);
            }
            catch
            {
                MessageBox.Show("Error merging " + Model.fileName + " with " + inModel.fileName + "!!!", "Error", MessageBoxButtons.OK);
            }
        }

        public static void MergeGroupsIntoOne(PModel inPModel, out PModel outPModel, bool bIncludeTextures)
        {
            int iGroupIdx;

            // Temporary backup of model
            PModel tmpPModel = CopyPModel(inPModel);

            outPModel = new PModel()
            {
                Groups = new PGroup[1],
                fileName = inPModel.fileName,
            };

            // For do the merging without textures will try to do some easy solution
            // 1. Erase all the textured groups of the model
            // 2. Process the groups of the model (theorically not texturized)
            // 3. Add the texturized groups from the temporary saved P Model if we want textured groups
            if (!bIncludeTextures)
            {
                for (iGroupIdx = 0; iGroupIdx < inPModel.Header.numGroups; iGroupIdx++)
                {
                    if (inPModel.Groups[iGroupIdx].texFlag == 1)
                    {
                        RemoveGroup(ref inPModel, iGroupIdx);
                        iGroupIdx--;
                    }
                }
            }


            // 2. Process the groups of the model 
            // Header
            outPModel.Header.version = inPModel.Header.version;
            outPModel.Header.off04 = inPModel.Header.off04;
            outPModel.Header.numXYZ = inPModel.Header.numXYZ;
            outPModel.Header.off28 = inPModel.Header.off28;
            outPModel.Header.off2C = inPModel.Header.off2C;
            outPModel.Header.off3C = inPModel.Header.off3C;
            outPModel.Header.vertexColor = inPModel.Header.vertexColor;

            outPModel.Header.numVerts = inPModel.Header.numVerts;
            outPModel.Verts = new Point3D[inPModel.Header.numVerts];
            Array.Copy(inPModel.Verts, outPModel.Verts, inPModel.Header.numVerts);

            outPModel.Header.numPolys = inPModel.Header.numPolys;
            outPModel.Polys = new PPolygon[inPModel.Header.numPolys];
            Array.Copy(inPModel.Polys, outPModel.Polys, inPModel.Header.numPolys);

            outPModel.Vcolors = new Color[inPModel.Header.numVerts];
            Array.Copy(inPModel.Vcolors, outPModel.Vcolors, inPModel.Header.numVerts);

            outPModel.Pcolors = new Color[inPModel.Header.numPolys];
            Array.Copy(inPModel.Pcolors, outPModel.Pcolors, inPModel.Header.numPolys);

            outPModel.Groups[0].polyType = 1;
            if (inPModel.Header.numTexCs > 0)
            {
                // We will avoid Texture Coordinates
                outPModel.Header.numTexCs = inPModel.Header.numTexCs;
                outPModel.TexCoords = new Point2D[inPModel.Header.numTexCs];
                Array.Copy(inPModel.TexCoords, outPModel.TexCoords, inPModel.Header.numTexCs);

                outPModel.Groups[0].polyType = 2;
            }

            outPModel.Hundrets = new PHundret[1];
            Array.Copy(inPModel.Hundrets, outPModel.Hundrets, 1);

            outPModel.Header.mirex_h = 1;
            outPModel.Header.mirex_g = inPModel.Header.mirex_g;
            outPModel.Header.unknown = new int[16];

            outPModel.Header.numGroups = 1;


            // Apply dimensional data
            outPModel.Groups[0].repGroupX = inPModel.Groups[0].repGroupX;
            outPModel.Groups[0].repGroupY = inPModel.Groups[0].repGroupY;
            outPModel.Groups[0].repGroupZ = inPModel.Groups[0].repGroupZ;
            outPModel.Groups[0].rszGroupX = inPModel.Groups[0].rszGroupX;
            outPModel.Groups[0].rszGroupY = inPModel.Groups[0].rszGroupY;
            outPModel.Groups[0].rszGroupZ = inPModel.Groups[0].rszGroupZ;
            outPModel.Groups[0].rotGroupAlpha = inPModel.Groups[0].rotGroupAlpha;
            outPModel.Groups[0].rotGroupBeta = inPModel.Groups[0].rotGroupBeta;
            outPModel.Groups[0].rotGroupGamma = inPModel.Groups[0].rotGroupGamma;

            // Apply the group number data of the input model to the output model
            outPModel.Groups[0].numVert = inPModel.Header.numVerts;
            outPModel.Groups[0].numPoly = inPModel.Header.numPolys;
            outPModel.Groups[0].numEdge = inPModel.Header.numEdges;
            outPModel.Groups[0].DListNum = 1;

            // Recalculate tris with the new vertex/polys/edges/normals indexes
            iGroupIdx = GetNextGroup(inPModel, GetNextGroup(inPModel, -1));

            while (iGroupIdx != -1)
            {
                // Polys
                for (int iPolyCounter = inPModel.Groups[iGroupIdx].offsetPoly;
                         iPolyCounter < inPModel.Groups[iGroupIdx].numPoly + inPModel.Groups[iGroupIdx].offsetPoly;
                         iPolyCounter++)
                {
                    outPModel.Polys[iPolyCounter].Verts[0] += (ushort)inPModel.Groups[iGroupIdx].offsetVert;
                    outPModel.Polys[iPolyCounter].Verts[1] += (ushort)inPModel.Groups[iGroupIdx].offsetVert;
                    outPModel.Polys[iPolyCounter].Verts[2] += (ushort)inPModel.Groups[iGroupIdx].offsetVert;

                }

                iGroupIdx = GetNextGroup(inPModel, iGroupIdx);
            }


            // 3. Add the texturized groups from the temporary saved P Model if we want textured groups
            if (!bIncludeTextures)
            {
                iGroupIdx = GetNextGroup(tmpPModel, -1);

                while (iGroupIdx != -1)
                {
                    if (tmpPModel.Groups[iGroupIdx].texFlag == 1)
                    {
                        AddGroup(ref outPModel,
                            tmpPModel.Verts.Skip(tmpPModel.Groups[iGroupIdx].offsetVert).
                                                    Take(tmpPModel.Groups[iGroupIdx].numVert).ToArray(),
                            tmpPModel.Polys.Skip(tmpPModel.Groups[iGroupIdx].offsetPoly).
                                                    Take(tmpPModel.Groups[iGroupIdx].numPoly).ToArray(),
                            tmpPModel.TexCoords.Skip(tmpPModel.Groups[iGroupIdx].offsetTex).
                                                    Take(tmpPModel.Groups[iGroupIdx].numVert).ToArray(),
                            tmpPModel.Vcolors.Skip(tmpPModel.Groups[iGroupIdx].offsetVert).
                                                    Take(tmpPModel.Groups[iGroupIdx].numVert).ToArray(),
                            tmpPModel.Pcolors.Skip(tmpPModel.Groups[iGroupIdx].offsetPoly).
                                                    Take(tmpPModel.Groups[iGroupIdx].numPoly).ToArray(),
                            tmpPModel.Groups[iGroupIdx].texID);

                        outPModel.Hundrets[outPModel.Header.numGroups - 1] =
                                                    CopyPHundret(tmpPModel.Hundrets[iGroupIdx]);
                    }

                    iGroupIdx = GetNextGroup(tmpPModel, iGroupIdx);
                }
            }
        }


        //  ---------------------------------------------------------------------------------------------------------
        //  ----------------------------------------- AUXILIARY FUNCTIONS -------------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        public static float ComputeDiameter(PBoundingBox BoundingBox)
        {
            float diffx, diffy, diffz;

            diffx = BoundingBox.max_x - BoundingBox.min_x;
            diffy = BoundingBox.max_y - BoundingBox.min_y;
            diffz = BoundingBox.max_z - BoundingBox.min_z;

            if (diffx > diffy)
            {
                if (diffx > diffz) return diffx;
                else return diffz;
            }
            else
            {
                if (diffy > diffz) return diffy;
                else return diffz;
            }
        }

        public static void ComputePModelBoundingBox(PModel Model, ref Point3D p_min, ref Point3D p_max)
        {
            Point3D p_min_aux = new Point3D();
            Point3D p_max_aux = new Point3D();
            double[] MV_matrix = new double[16];

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPushMatrix();
            glLoadIdentity();

            p_min_aux.x = Model.BoundingBox.min_x;
            p_min_aux.y = Model.BoundingBox.min_y;
            p_min_aux.z = Model.BoundingBox.min_z;

            p_max_aux.x = Model.BoundingBox.max_x;
            p_max_aux.y = Model.BoundingBox.max_y;
            p_max_aux.z = Model.BoundingBox.max_z;

            ConcatenateCameraModelView(Model.repositionX, Model.repositionY, Model.repositionZ,
                                       Model.rotateAlpha, Model.rotateBeta, Model.rotateGamma,
                                       Model.resizeX, Model.resizeY, Model.resizeZ);

            glGetDoublev((uint)GLCapability.GL_MODELVIEW_MATRIX, MV_matrix);

            ComputeTransformedBoxBoundingBox(MV_matrix, ref p_min_aux, ref p_max_aux, ref p_min, ref p_max);

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            glPopMatrix();
        }

        public static bool CheckModelConsistency(ref PModel Model)
        {
            int iNumTextures, iNumNormIdx, iNumNormals;
            int offsetPoly, offsetTex, end_group_polys, end_group_verts; // offsetVert, 
            int iGroupIdx, iPolyIdx;

            bool iCheckModelConsistencyResult = true;

            // Prepare Bounds
            iNumNormIdx = 0;
            if (Model.NormalIndex.Length > 0) iNumNormIdx = Model.NormalIndex.Length - 1;

            iNumNormals = 0;
            if (Model.Normals.Length > 0) iNumNormals = Model.Normals.Length - 1;

            iNumTextures = 0;
            if (Model.TexCoords != null)
                if (Model.TexCoords.Length > 0) iNumTextures = Model.TexCoords.Length - 1;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                //offsetVert = Model.Groups[iGroupIdx].offsetVert;
                end_group_verts = Model.Groups[iGroupIdx].numVert;
                offsetPoly = Model.Groups[iGroupIdx].offsetPoly;
                end_group_polys = Model.Groups[iGroupIdx].numPoly;
                offsetTex = Model.Groups[iGroupIdx].offsetTex;

                for (iPolyIdx = offsetPoly; iPolyIdx < end_group_polys; iPolyIdx++)
                {
                    if (Model.Polys[iPolyIdx].Verts[0] < 0 || Model.Polys[iPolyIdx].Verts[0] > end_group_verts)
                    {
                        //MessageBox.Show("Polys_Verts inconsistence.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    if (Model.Polys[iPolyIdx].Verts[1] < 0 || Model.Polys[iPolyIdx].Verts[1] > end_group_verts)
                    {
                        //MessageBox.Show("Polys_Verts inconsistence.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    if (Model.Polys[iPolyIdx].Verts[2] < 0 || Model.Polys[iPolyIdx].Verts[2] > end_group_verts)
                    {
                        //MessageBox.Show("Polys_Verts inconsistence.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }

                    if (Model.Polys[iPolyIdx].Normals[0] > iNumNormIdx)
                    {
                        //MessageBox.Show("Polys_Normals 0 > num_norm_indx.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    else
                    {
                        if (iNumNormals > 0 && 
                            Model.NormalIndex[Model.Polys[iPolyIdx].Normals[0]] > iNumNormals)
                        {
                            //MessageBox.Show("Polys_Normals 0 > num_norm.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                    }

                    if (Model.Polys[iPolyIdx].Normals[1] > iNumNormIdx)
                    {
                        //MessageBox.Show("Polys_Normals 1 > num_norm_indx.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    else
                    {
                        if (iNumNormals > 0 && 
                            Model.NormalIndex[Model.Polys[iPolyIdx].Normals[1]] > iNumNormals)
                        {
                            //MessageBox.Show("Polys_Normals 1 > num_norm.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                    }

                    if (Model.Polys[iPolyIdx].Normals[2] > iNumNormIdx)
                    {
                        //MessageBox.Show("Polys_Normals 2 > num_norm_indx.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    else
                    {
                        if (iNumNormals > 0 &&
                            Model.NormalIndex[Model.Polys[iPolyIdx].Normals[2]] > iNumNormals)
                        {
                            //MessageBox.Show("Polys_Normals 2 > num_norm.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                    }

                    if (Model.Groups[iGroupIdx].texFlag == 1)
                    {
                        if (Model.Polys[iPolyIdx].Verts[0] + offsetTex > iNumTextures)
                        {
                            //MessageBox.Show("Polys Verts 0 > num_textures.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                        if (Model.Polys[iPolyIdx].Verts[1] + offsetTex > iNumTextures)
                        {
                            //MessageBox.Show("Polys Verts 1 > num_textures.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                        if (Model.Polys[iPolyIdx].Verts[2] + offsetTex > iNumTextures)
                        {
                            //MessageBox.Show("Polys Verts 2 > num_textures.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                    }
                }
            }

            return iCheckModelConsistencyResult;
        }

        public static void KillUnusedVertices(ref PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx, iVertIdxNext,
                 iActualVertIdx, iTexCoordIdx, iTexCoordIdxGlobal, iNextGroup;
            int[] vertsUsage = new int[Model.Header.numVerts];

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++) vertsUsage[iVertIdx] = 0;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                     iPolyIdx < (Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly);
                     iPolyIdx++)
                {
                    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                    {
                        vertsUsage[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert] += 1;
                    }
                }
            }

            iVertIdx = 0;
            iActualVertIdx = 0;
            iTexCoordIdxGlobal = 0;

            iGroupIdx = GetNextGroup(Model, -1);
            while (iGroupIdx != -1)
            {
                while (iVertIdx < Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert)
                {
                    if (vertsUsage[iActualVertIdx] == 0)
                    {
                        //  If the vertex is unused, let's destroy it
                        for (iVertIdxNext = iVertIdx; 
                             iVertIdxNext < Model.Header.numVerts - 1; 
                             iVertIdxNext++)
                        {
                            Model.Verts[iVertIdxNext] = Model.Verts[iVertIdxNext + 1];
                            Model.Vcolors[iVertIdxNext] = Model.Vcolors[iVertIdxNext + 1];
                        }

                        if (Model.Groups[iGroupIdx].texFlag == 1)
                        {
                            for (iTexCoordIdx = iTexCoordIdxGlobal; 
                                 iTexCoordIdx < Model.Header.numTexCs - 1; 
                                 iTexCoordIdx++)
                                Model.TexCoords[iTexCoordIdx] = Model.TexCoords[iTexCoordIdx + 1];

                            Model.Header.numTexCs--;
                            Array.Resize(ref Model.TexCoords, Model.Header.numTexCs);
                        }

                        Model.Header.numVerts--;
                        Array.Resize(ref Model.Verts, Model.Header.numVerts);
                        Array.Resize(ref Model.Vcolors, Model.Header.numVerts);

                        for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                             iPolyIdx < (Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly);
                             iPolyIdx++)
                        {
                            for (iVertIdxNext = 0; iVertIdxNext < 3; iVertIdxNext++)
                            {
                                if (Model.Polys[iPolyIdx].Verts[iVertIdxNext] > iVertIdx - Model.Groups[iGroupIdx].offsetVert)
                                    Model.Polys[iPolyIdx].Verts[iVertIdxNext] -= 1;
                            }
                        }

                        iNextGroup = GetNextGroup(Model, iGroupIdx);

                        while (iNextGroup != -1)
                        {
                            Model.Groups[iNextGroup].offsetVert--;

                            if (Model.Groups[iGroupIdx].texFlag == 1 && 
                                Model.Groups[iNextGroup].offsetTex > 0)
                                        Model.Groups[iNextGroup].offsetTex--;

                            iNextGroup = GetNextGroup(Model, iNextGroup);
                        }

                        Model.Groups[iGroupIdx].numVert--;
                    }
                    else
                    {
                        iVertIdx++;
                        if (Model.Groups[iGroupIdx].texFlag == 1) iTexCoordIdxGlobal++;
                    }

                    iActualVertIdx++;
                }

                iGroupIdx = GetNextGroup(Model, iGroupIdx);
            }

        }

        // This procedure removes the Groups with numpolys = 0.
        // This should never happen (but Kaldarasha reported models created with numpolys = 0).
        public static void RepairGroups(ref PModel Model)
        {
            int iGroupIdx;

            // Delete group if numpolys = 0
            for (iGroupIdx = 0; iGroupIdx < Model.Groups.Length; iGroupIdx++)
            {
                if (Model.Groups[iGroupIdx].numPoly == 0 ||
                    Model.Groups[iGroupIdx].numVert == 0)
                {
                    RemoveGroup(ref Model, iGroupIdx);
                    iGroupIdx--;
                }
            }
        }


        // This procedure removes polys that has some vertices with the
        // same index (then supposedly is a line).
        public static void RepairPolys(ref PModel Model)
        {
            int iGroupIdx, iPolyIdx, iV0, iV1, iV2;
            bool bRepairYes = false;

            // Poly with duplicated vertex indices
            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                     iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                     iPolyIdx++)
                {
                    iV0 = Model.Polys[iPolyIdx].Verts[0];
                    iV1 = Model.Polys[iPolyIdx].Verts[1];
                    iV2 = Model.Polys[iPolyIdx].Verts[2];

                    if (iV0 == iV1 || iV0 == iV2 || iV1 == iV2)
                    {

                        if (!bRepairYes)
                        {
                            if (MessageBox.Show("The model: " + Model.fileName + " has one vertex index duplicated in " +
                                                "the same triangle/poly. Do you want to fix it?\n(the triangle will be " +
                                                "removed)\n\nNOTE: This answer will be used for all the model and will " +
                                                "cancel the check of duplicated vertex coordinates.\n\n" +
                                                "[INFO] Group:     " + iGroupIdx.ToString() + "\n" +
                                                "       Poly:      " + iPolyIdx.ToString() + "\n" +
                                                "       Vertex V0: " + iV0.ToString() + "\n" +
                                                "       Vertex V1: " + iV1.ToString() + "\n" +
                                                "       Vertex V2: " + iV2.ToString(),
                                                "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                bRepairYes = true;
                            else return;
                        }

                        if (bRepairYes)
                        {
                            RemovePolygon(ref Model, iPolyIdx);

                            if (Model.Groups[iGroupIdx].numPoly > 1 &&
                                Model.Header.numPolys > iPolyIdx)
                                iPolyIdx--;
                        }
                    }
                }
            }

            // Poly with duplicate vertex coordinates (different vertex indices)
            bRepairYes = false;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                     iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                     iPolyIdx++)
                {
                    iV0 = Model.Polys[iPolyIdx].Verts[0];
                    iV1 = Model.Polys[iPolyIdx].Verts[1];
                    iV2 = Model.Polys[iPolyIdx].Verts[2];

                    if (ComparePoints3D(Model.Verts[iV0 + Model.Groups[iGroupIdx].offsetVert],
                                        Model.Verts[iV1 + Model.Groups[iGroupIdx].offsetVert]) ||
                        ComparePoints3D(Model.Verts[iV0 + Model.Groups[iGroupIdx].offsetVert],
                                        Model.Verts[iV2 + Model.Groups[iGroupIdx].offsetVert]) ||
                        ComparePoints3D(Model.Verts[iV1 + Model.Groups[iGroupIdx].offsetVert],
                                        Model.Verts[iV2 + Model.Groups[iGroupIdx].offsetVert]))
                    {

                        if (!bRepairYes)
                        {
                            if (MessageBox.Show("The model: " + Model.fileName + " has one vertex coordinate duplicated in " +
                                                "the same triangle/poly. Do you want to fix it?\n(the triangle will be " +
                                                "removed)\n\nNOTE: This answer will be used for all the model.\n\n" +
                                                "[INFO] Group:     " + iGroupIdx.ToString() + "\n" +
                                                "       Poly:      " + iPolyIdx.ToString() + "\n" +
                                                "       Vertex V0: " + iV0.ToString() + "\n" +
                                                "       Vertex V1: " + iV1.ToString() + "\n" +
                                                "       Vertex V2: " + iV2.ToString() + "\n" +
                                                "       Vertex V0.x: " + Model.Verts[iV0].x.ToString() + "\n" +
                                                "       Vertex V0.y: " + Model.Verts[iV0].y.ToString() + "\n" +
                                                "       Vertex V0.z: " + Model.Verts[iV0].z.ToString() + "\n" +
                                                "       Vertex V1.x: " + Model.Verts[iV1].x.ToString() + "\n" +
                                                "       Vertex V1.y: " + Model.Verts[iV1].y.ToString() + "\n" +
                                                "       Vertex V1.z: " + Model.Verts[iV1].z.ToString() + "\n" +
                                                "       Vertex V2.x: " + Model.Verts[iV2].x.ToString() + "\n" +
                                                "       Vertex V2.y: " + Model.Verts[iV2].y.ToString() + "\n" +
                                                "       Vertex V2.z: " + Model.Verts[iV2].z.ToString() + "\n",
                                                "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                bRepairYes = true;
                            else return;
                        }

                        if (bRepairYes)
                        {
                            RemovePolygon(ref Model, iPolyIdx);

                            if (Model.Groups[iGroupIdx].numPoly > 1 &&
                                Model.Header.numPolys > iPolyIdx)
                                iPolyIdx--;
                        }
                    }
                }
            }
        }

        public static void CreateDListFromPGroup(ref PGroup Group, PPolygon[] Polys,  Point3D[] Verts,
                                                 Color[] Vcolors, Point3D[] Normals, int[] NormalsIndex,
                                                 Point2D[] TexCoords, PHundret Hundret)
        {

            if (Group.DListNum < 0)
            {
                Group.DListNum = (int)glGenLists(1);
            }
            else
            {
                glDeleteLists((uint)Group.DListNum, 1);
                Group.DListNum = (int)glGenLists(1);
            }

            glNewList((uint)Group.DListNum, GLListMode.GL_COMPILE);

            DrawGroup(Group, Polys, Verts, Vcolors, Normals, NormalsIndex, TexCoords, Hundret, false);
            glEndList();
        }

        public static void CreateDListsFromPModel(ref PModel Model)
        {
            int iGroupIdx;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                CreateDListFromPGroup(ref Model.Groups[iGroupIdx], Model.Polys, 
                                      Model.Verts, Model.Vcolors, Model.Normals, Model.NormalIndex, 
                                      Model.TexCoords, Model.Hundrets[iGroupIdx]);
            }
        }

        public static void RotatePModelGroupModifiers(ref PGroup Group, float alpha, float beta, float gamma)
        {

            float diff_alpha, diff_beta, diff_gamma;
            Quaternion aux_quat = new Quaternion();
            Quaternion res_quat = new Quaternion();

            if (alpha == 0 || beta == 0 || gamma == 0)
            {
                //  This works if there are at most 2 active axes
                BuildQuaternionFromEuler(alpha, beta, gamma, ref Group.rotationQuaternionGroup);
            }
            else
            {
                //  Else add up the quaternion difference
                diff_alpha = alpha - Group.rotGroupAlpha;
                diff_beta = beta - Group.rotGroupBeta;
                diff_gamma = gamma - Group.rotGroupGamma;

                BuildQuaternionFromEuler(diff_alpha, diff_beta, diff_gamma, ref aux_quat);

                MultiplyQuaternions(Group.rotationQuaternionGroup, aux_quat, ref res_quat);

                Group.rotationQuaternionGroup = res_quat;
            }

            Group.rotGroupAlpha = alpha;
            Group.rotGroupBeta = beta;
            Group.rotGroupGamma = gamma;
        }

        public static void RotatePModelModifiers(ref PModel Model, float alpha, float beta, float gamma)
        {

            float diff_alpha, diff_beta, diff_gamma;
            Quaternion aux_quat = new Quaternion();
            Quaternion res_quat = new Quaternion();

            if (alpha == 0 || beta == 0 || gamma == 0)
            {
                //  This works if there are at most 2 active axes
                BuildQuaternionFromEuler(alpha, beta, gamma, ref Model.rotationQuaternion);
            }
            else
            {
                //  Else add up the quaternion difference
                diff_alpha = alpha - Model.rotateAlpha;
                diff_beta = beta - Model.rotateBeta;
                diff_gamma = gamma - Model.rotateGamma;

                BuildQuaternionFromEuler(diff_alpha, diff_beta, diff_gamma, ref aux_quat);

                MultiplyQuaternions(Model.rotationQuaternion, aux_quat, ref res_quat);

                Model.rotationQuaternion = res_quat;
            }

            Model.rotateAlpha = alpha;
            Model.rotateBeta = beta;
            Model.rotateGamma = gamma;
        }

        private static void FillHundrestsDefaultValues(ref PHundret hundret, bool bHasTexCoords)
        {
            hundret.field_0 = 1;
            hundret.field_4 = 1;

            if (bHasTexCoords)
            {
                hundret.field_8 = 229894; // V_TEXTURE = 1 && V_LINEARFILTER = 1
                hundret.field_C = 131074; // V_TrueTEXTURE = 1 && V_TrueLINEARFILTER = 0
            }
            else
            {
                hundret.field_8 = 229888; // V_TEXTURE = 0 && V_LINEARFILTER = 0
                hundret.field_C = 131072; // V_TrueTEXTURE = 0 && V_TrueLINEARFILTER = 0
            }

            hundret.texID = 0;
            hundret.texture_set_ptr = 0;
            hundret.field_18 = 0;
            hundret.field_1C = 0;
            hundret.field_20 = 0;
            hundret.shademode = 2;
            hundret.lightstate_ambient = -1;
            hundret.field_2C = 0;
            hundret.lightstate_material_ptr = 0;
            hundret.srcblend = 2;
            hundret.destblend = 1;
            hundret.field_3C = 2;
            hundret.alpharef = 0;
            hundret.blend_mode = 4;
            hundret.zSort = 0;
            hundret.field_4C = 0;
            hundret.field_50 = 0;
            hundret.field_54 = 0;
            hundret.field_58 = 0;
            hundret.vertex_alpha = 255;
            hundret.field_60 = 0;
        }


        //  ---------------------------------------------------------------------------------------------------------
        //  --------------------------------------------------SETTERS------------------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        public static void AddGroup(ref PModel Model, 
                                    Point3D[] aVerts, PPolygon[] aPolys,
                                    Point2D[] aTexCoords, Color[] aVColors, Color[] aPColors,
                                    int iTextureID)
        {
            //  ------------------- Warning! Causes the Normals to be inconsistent.------------------------------
            //  --------------------------------Must call ComputeNormals ----------------------------------------

            int iGroupIdx, iPolyIdx, iVertIdx, iTexCoordIdx, iNumVerts, iNumPolys, iNumTexCoords;

            iNumVerts = aVerts.Length;
            iNumPolys = aPolys.Length;

            if (aTexCoords != null) iNumTexCoords = aTexCoords.Length;
            else iNumTexCoords = 0;

            if (Model.Groups != null)
            {
                iGroupIdx = Model.Groups.Length;
                Array.Resize(ref Model.Groups, iGroupIdx + 1);
                Model.Groups[iGroupIdx] = new PGroup();
            }
            else
            {
                Model.Groups = new PGroup[1];
                iGroupIdx = 0;
            }

            if (iNumTexCoords > 0) Model.Groups[iGroupIdx].polyType = 2;
            else Model.Groups[iGroupIdx].polyType = 1;

            if (Model.Verts != null) Model.Groups[iGroupIdx].offsetVert = Model.Verts.Length;
            else Model.Groups[iGroupIdx].offsetVert = 0;
            Model.Groups[iGroupIdx].numVert = iNumVerts;

            if (Model.Polys != null) Model.Groups[iGroupIdx].offsetPoly = Model.Polys.Length;
            else Model.Groups[iGroupIdx].offsetPoly = 0;
            Model.Groups[iGroupIdx].numPoly = iNumPolys;

            if (Model.Edges != null) Model.Groups[iGroupIdx].offsetEdge = Model.Edges.Length;
            else Model.Groups[iGroupIdx].offsetEdge = 0;
            Model.Groups[iGroupIdx].numEdge = iNumPolys * 3;

            Model.Groups[iGroupIdx].off1C = 0;
            Model.Groups[iGroupIdx].off20 = 0;
            Model.Groups[iGroupIdx].off24 = 0;
            Model.Groups[iGroupIdx].off28 = 0;

            // Texture Coordinates (UVs)
            Model.Groups[iGroupIdx].texFlag = (iNumTexCoords > 0) ? 1 : 0;
            Model.Groups[iGroupIdx].texID = 0;

            if (Model.TexCoords != null)
                if (Model.Groups[iGroupIdx].texFlag == 1)
                    Model.Groups[iGroupIdx].offsetTex = Model.TexCoords.Length;
                else 
                    Model.Groups[iGroupIdx].offsetTex = 0;

            Model.Groups[iGroupIdx].HiddenQ = false;


            // Add new Verts/Normals to the Group
            int iMainModelNum = Model.Header.numVerts;
            Model.Header.numVerts += iNumVerts;
            //Model.Header.numNormals += iNumVerts;
            //Model.Header.numNormIdx += iNumVerts;
            Array.Resize(ref Model.Verts, Model.Header.numVerts);
            Array.Resize(ref Model.Vcolors, Model.Header.numVerts);
            //Array.Resize(ref Model.Normals, Model.Header.numNormals);
            //Array.Resize(ref Model.NormalIndex, Model.Header.numNormIdx);
            for (iVertIdx = 0; iVertIdx < iNumVerts; iVertIdx++)
            {
                Model.Verts[iMainModelNum + iVertIdx].x = aVerts[iVertIdx].x;
                Model.Verts[iMainModelNum + iVertIdx].y = aVerts[iVertIdx].y;
                Model.Verts[iMainModelNum + iVertIdx].z = aVerts[iVertIdx].z;

                //  Add new Vertex Colors to the Group
                Model.Vcolors[iMainModelNum + iVertIdx] = aVColors[iVertIdx];
            }


            // Add new Polys to the Group
            iMainModelNum = Model.Header.numPolys;
            Model.Header.numPolys += iNumPolys;
            Array.Resize(ref Model.Polys, Model.Header.numPolys);
            Array.Resize(ref Model.Pcolors, Model.Header.numPolys);
            for (iPolyIdx = 0; iPolyIdx < iNumPolys; iPolyIdx++)
            {
                Model.Polys[iMainModelNum + iPolyIdx].Edges = aPolys[iPolyIdx].Edges;
                Model.Polys[iMainModelNum + iPolyIdx].Normals = aPolys[iPolyIdx].Normals;
                Model.Polys[iMainModelNum + iPolyIdx].tag1 = aPolys[iPolyIdx].tag1;
                Model.Polys[iMainModelNum + iPolyIdx].tag2 = aPolys[iPolyIdx].tag2;
                Model.Polys[iMainModelNum + iPolyIdx].Verts = aPolys[iPolyIdx].Verts;

                //  Add new Poly Colors to the Group
                Model.Pcolors[iMainModelNum + iPolyIdx] = aPColors[iPolyIdx];

            }

            // Add new Texture Coordinates to the Group
            iMainModelNum = Model.Header.numTexCs;

            if (iNumTexCoords > 0)
            {
                Model.Header.numTexCs += iNumTexCoords;
                Model.Groups[iGroupIdx].texID = iTextureID;

                Array.Resize(ref Model.TexCoords, Model.Header.numTexCs);
                for (iTexCoordIdx = 0; iTexCoordIdx < iNumTexCoords; iTexCoordIdx++)
                {
                    Model.TexCoords[iMainModelNum + iTexCoordIdx].x = aTexCoords[iTexCoordIdx].x;
                    Model.TexCoords[iMainModelNum + iTexCoordIdx].y = aTexCoords[iTexCoordIdx].y;
                }
            }

            // Increase number of Groups
            Model.Header.numGroups += 1;
            Model.Header.mirex_g = 1;

            // Increase Hundrets
            Model.Header.mirex_h += 1;
            Array.Resize(ref Model.Hundrets, Model.Header.mirex_h);
            FillHundrestsDefaultValues(ref Model.Hundrets[Model.Header.mirex_h - 1], Model.Groups[iGroupIdx].texFlag == 1);

            if (Model.Header.mirex_h > 1)
                Model.Hundrets[Model.Header.mirex_h - 1].texID = Model.Hundrets[Model.Header.mirex_h - 2].texID;

            // Assign other non-group vars as realGID or Reposition/Resize/Rotate.
            Model.Groups[iGroupIdx].realGID = Model.Groups.Length - 1;

            Model.Groups[iGroupIdx].rszGroupX = 1;
            Model.Groups[iGroupIdx].rszGroupY = 1;
            Model.Groups[iGroupIdx].rszGroupZ = 1;
            Model.Groups[iGroupIdx].repGroupX = 0;
            Model.Groups[iGroupIdx].repGroupY = 0;
            Model.Groups[iGroupIdx].repGroupZ = 0;
            Model.Groups[iGroupIdx].rotGroupAlpha = 0;
            Model.Groups[iGroupIdx].rotGroupBeta = 0;
            Model.Groups[iGroupIdx].rotGroupGamma = 0;

            Model.Groups[iGroupIdx].rotationQuaternionGroup = new Quaternion() { x = 0, y = 0, z = 0, w = 1 };
        }

        public static void ApplyCurrentVColors(ref PModel Model)
        {
            int iVertIdx;

            SetBlendMode(BLEND_MODE.BLEND_DISABLED);

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {
                Model.Vcolors[iVertIdx] =
                    GetVertColor(Model.Verts[iVertIdx],
                                 Model.Normals[Model.NormalIndex[iVertIdx]],
                                 Model.Vcolors[iVertIdx]);
            }

        }



        //  ----------------------------------------------------------------------------------------------------
        //  =============================================REPAIRING==============================================
        //  ----------------------------------------------------------------------------------------------------
        public static void ComputeBoundingBox(ref PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx;

            Model.BoundingBox.max_x = (float)-INFINITY_SINGLE;
            Model.BoundingBox.max_y = (float)-INFINITY_SINGLE;
            Model.BoundingBox.max_z = (float)-INFINITY_SINGLE;
            Model.BoundingBox.min_x = (float)INFINITY_SINGLE;
            Model.BoundingBox.min_y = (float)INFINITY_SINGLE;
            Model.BoundingBox.min_z = (float)INFINITY_SINGLE;

            try
            {
                for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
                {
                    for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                         iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                         iPolyIdx++)
                    {
                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                        {
                            if (Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].x > Model.BoundingBox.max_x)
                                Model.BoundingBox.max_x = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].x;
                            if (Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].y > Model.BoundingBox.max_y)
                                Model.BoundingBox.max_y = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].y;
                            if (Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].z > Model.BoundingBox.max_z)
                                Model.BoundingBox.max_z = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].z;

                            if (Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].x < Model.BoundingBox.min_x)
                                Model.BoundingBox.min_x = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].x;
                            if (Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].y < Model.BoundingBox.min_y)
                                Model.BoundingBox.min_y = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].y;
                            if (Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].z < Model.BoundingBox.min_z)
                                Model.BoundingBox.min_z = Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert].z;
                        }
                    }
                }

                Model.diameter = (float)Math.Sqrt(Math.Pow(Model.BoundingBox.max_x, 2) + Math.Pow(Model.BoundingBox.max_y, 2) + Math.Pow(Model.BoundingBox.max_z, 2) +
                                                  Math.Pow(Model.BoundingBox.min_x, 2) + Math.Pow(Model.BoundingBox.min_y, 2) + Math.Pow(Model.BoundingBox.min_z, 2));
            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;
            }


        }

        public static void ComputeCurrentBoundingBox(ref PModel Model)
        {
            Point3D p3DTemp;

            ComputeBoundingBox(ref Model);

            p3DTemp = 
                new Point3D(Model.BoundingBox.max_x, Model.BoundingBox.max_y, Model.BoundingBox.max_z);

            p3DTemp = GetEyeSpaceCoords(p3DTemp);
            Model.BoundingBox.max_x = p3DTemp.x;
            Model.BoundingBox.max_y = p3DTemp.y;
            Model.BoundingBox.max_z = p3DTemp.z;

            p3DTemp.x = Model.BoundingBox.min_x;
            p3DTemp.y = Model.BoundingBox.min_y;
            p3DTemp.z = Model.BoundingBox.min_z;

            p3DTemp = GetEyeSpaceCoords(p3DTemp);
            Model.BoundingBox.min_x = p3DTemp.x;
            Model.BoundingBox.min_y = p3DTemp.y;
            Model.BoundingBox.min_z = p3DTemp.z;
        }


        ////// COMPUTE NORMALS
        public struct STVertexNormals
        {
            public int iVertexNormalsCounter;
            public Point3D p3DVertex;
            public Point3D p3DNormal;
            public HashSet<int> hsActualVertex;

        }

        public static List<STVertexNormals> stVertexNormals;

        public static void GenerateNormalsList(PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx, iActualVertex, iVertexNormalsIdx;
            STVertexNormals stTmpVertexNormals;
            Point3D p3DNormal;

            stVertexNormals = new List<STVertexNormals>();

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                     iPolyIdx < Model.Groups[iGroupIdx].numPoly + Model.Groups[iGroupIdx].offsetPoly;
                     iPolyIdx++)
                {
                    p3DNormal = CalculateNormal(Model.Verts[Model.Polys[iPolyIdx].Verts[0] +
                                                            Model.Groups[iGroupIdx].offsetVert],
                                                Model.Verts[Model.Polys[iPolyIdx].Verts[1] +
                                                            Model.Groups[iGroupIdx].offsetVert],
                                                Model.Verts[Model.Polys[iPolyIdx].Verts[2] +
                                                            Model.Groups[iGroupIdx].offsetVert]);


                    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                    {
                        iActualVertex = Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                        Model.Groups[iGroupIdx].offsetVert;

                        Model.Polys[iPolyIdx].Normals[iVertIdx] = (ushort)iActualVertex;
                        Model.NormalIndex[iActualVertex] = iActualVertex;

                        iVertexNormalsIdx = stVertexNormals.
                                FindIndex(x => x.p3DVertex.x == Model.Verts[iActualVertex].x &&
                                               x.p3DVertex.y == Model.Verts[iActualVertex].y &&
                                               x.p3DVertex.z == Model.Verts[iActualVertex].z);

                        if (iVertexNormalsIdx == -1)
                        {
                            stTmpVertexNormals = new STVertexNormals()
                            {
                                p3DVertex = Model.Verts[iActualVertex],
                                p3DNormal = new Point3D(p3DNormal.x, p3DNormal.y, p3DNormal.z),

                                hsActualVertex = new HashSet<int>(),
                                iVertexNormalsCounter = 1,
                            };

                            stTmpVertexNormals.hsActualVertex.Add(iActualVertex);

                            stVertexNormals.Add(stTmpVertexNormals);
                        }
                        else
                        {
                            stTmpVertexNormals = stVertexNormals[iVertexNormalsIdx];

                            stTmpVertexNormals.p3DNormal = AddPoint3D(stTmpVertexNormals.p3DNormal, p3DNormal);
                            stTmpVertexNormals.hsActualVertex.Add(iActualVertex);
                            stTmpVertexNormals.iVertexNormalsCounter++;

                            stVertexNormals[iVertexNormalsIdx] = stTmpVertexNormals;
                        }
                    }
                }
            }
        }

        public static void ComputeNormals4MoreVerticesThanPolys(ref PModel Model)
        {
            Point3D p3DTempNormal;
            int iVertexNormalIdx;

            Model.Normals = new Point3D[Model.Header.numVerts];
            Model.NormalIndex = new int[Model.Header.numVerts];

            Model.Header.numNormals = Model.Header.numVerts;
            Model.Header.numNormIdx = Model.Header.numVerts;

            GenerateNormalsList(Model);

            for (iVertexNormalIdx = 0; iVertexNormalIdx < stVertexNormals.Count; iVertexNormalIdx++)
            {
                if (stVertexNormals[iVertexNormalIdx].iVertexNormalsCounter > 0)
                {
                    p3DTempNormal = Normalize(new Point3D(
                        -stVertexNormals[iVertexNormalIdx].p3DNormal.x / stVertexNormals[iVertexNormalIdx].iVertexNormalsCounter,
                        -stVertexNormals[iVertexNormalIdx].p3DNormal.y / stVertexNormals[iVertexNormalIdx].iVertexNormalsCounter,
                        -stVertexNormals[iVertexNormalIdx].p3DNormal.z / stVertexNormals[iVertexNormalIdx].iVertexNormalsCounter));
                }
                else
                {
                    p3DTempNormal = new Point3D(0, 0, 0);
                }

                foreach (int itmInt in stVertexNormals[iVertexNormalIdx].hsActualVertex)
                {
                    Model.Normals[itmInt] = p3DTempNormal;
                }
            }

        }

        public static void ComputeNormals4MorePolysThanVertices(ref PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx, iActualVertIdx, iVertIdxNext;

            Point3D p3DTempNorm;

            Point3D[] sumNorms = new Point3D[Model.Header.numVerts];
            int[] polys_per_vert = new int[Model.Header.numVerts];

            Model.Normals = new Point3D[Model.Header.numVerts];
            Model.NormalIndex = new int[Model.Header.numVerts];

            Model.Header.numNormals = Model.Header.numVerts;
            Model.Header.numNormIdx = Model.Header.numVerts;

            for (iPolyIdx = 0; iPolyIdx < Model.Header.numPolys; iPolyIdx++)
            {
                //  This should never happen. What the hell is going on?! (normals should be between 0-1 -float)
                if (Model.Polys[iPolyIdx].Verts[0] < 0) Model.Polys[iPolyIdx].Verts[0] = 0;
                if (Model.Polys[iPolyIdx].Verts[1] < 0) Model.Polys[iPolyIdx].Verts[1] = 0;
                if (Model.Polys[iPolyIdx].Verts[2] < 0) Model.Polys[iPolyIdx].Verts[2] = 0;
            }

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                     iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                     iPolyIdx++)
                {
                    p3DTempNorm = CalculateNormal(Model.Verts[Model.Polys[iPolyIdx].Verts[0] +
                                                  Model.Groups[iGroupIdx].offsetVert],
                                                  Model.Verts[Model.Polys[iPolyIdx].Verts[1] +
                                                  Model.Groups[iGroupIdx].offsetVert],
                                                  Model.Verts[Model.Polys[iPolyIdx].Verts[2] +
                                                  Model.Groups[iGroupIdx].offsetVert]);

                    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                    {

                        iActualVertIdx = Model.Polys[iPolyIdx].Verts[iVertIdx] +
                                         Model.Groups[iGroupIdx].offsetVert;

                        sumNorms[iActualVertIdx] = AddPoint3D(sumNorms[iActualVertIdx], p3DTempNorm);

                        polys_per_vert[iActualVertIdx] += 1;

                        Model.Polys[iPolyIdx].Normals[iVertIdx] = (ushort)iActualVertIdx;

                    }
                }
            }

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {
                if (polys_per_vert[iVertIdx] > 0)
                {
                    for (iVertIdxNext = iVertIdx + 1; iVertIdxNext < Model.Header.numVerts; iVertIdxNext++)
                    {
                        if (ComparePoints3D(Model.Verts[iVertIdx], Model.Verts[iVertIdxNext]))
                        {
                            sumNorms[iVertIdx].x += sumNorms[iVertIdxNext].x;
                            sumNorms[iVertIdx].y += sumNorms[iVertIdxNext].y;
                            sumNorms[iVertIdx].z += sumNorms[iVertIdxNext].z;

                            sumNorms[iVertIdxNext] = sumNorms[iVertIdx];

                            polys_per_vert[iVertIdx] += polys_per_vert[iVertIdxNext];
                            polys_per_vert[iVertIdxNext] = -polys_per_vert[iVertIdx];
                        }
                    }
                }
                else
                {
                    for (iVertIdxNext = iVertIdx + 1; iVertIdxNext < Model.Header.numVerts; iVertIdxNext++)
                    {
                        if (ComparePoints3D(Model.Verts[iVertIdx], Model.Verts[iVertIdxNext]))
                        {
                            sumNorms[iVertIdx] = sumNorms[iVertIdxNext];
                            polys_per_vert[iVertIdx] = -polys_per_vert[iVertIdxNext];
                        }
                    }
                }

                polys_per_vert[iVertIdx] = Math.Abs(polys_per_vert[iVertIdx]);
            }

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {
                if (polys_per_vert[iVertIdx] > 0)
                {
                    sumNorms[iVertIdx].x = -sumNorms[iVertIdx].x / polys_per_vert[iVertIdx];
                    sumNorms[iVertIdx].y = -sumNorms[iVertIdx].y / polys_per_vert[iVertIdx];
                    sumNorms[iVertIdx].z = -sumNorms[iVertIdx].z / polys_per_vert[iVertIdx];
                }
                else
                {
                    sumNorms[iVertIdx].x = 0;
                    sumNorms[iVertIdx].y = 0;
                    sumNorms[iVertIdx].z = 0;
                }

                Model.Normals[iVertIdx] = Normalize(sumNorms[iVertIdx]);
                Model.NormalIndex[iVertIdx] = iVertIdx;
            }
        }

        // I decided to use 2 different functions for ComputeNormals
        // -the resulting normals are the same-
        // The difference is that one is faster for one type of models (more vertices than polys)
        // and the other for other type of models (more polys than vertices)
        // Let's say that the percentage needed to choose one or other depends on the
        // amount of vertices / polys. I use a threshold for the percentage (58).
        public static void ComputeNormals(ref PModel Model)
        {
            float iNumVertsPercent;
            float iNumVerts = Model.Verts.Length;

            iNumVertsPercent = (iNumVerts / (Model.Verts.Length + Model.Polys.Length)) * 100;

            if (iNumVertsPercent > I_COMPUTENORMALS_VERTEXTHRESHOLD)
                ComputeNormals4MoreVerticesThanPolys(ref Model);
            else
                ComputeNormals4MorePolysThanVertices(ref Model);

        }

        public static void DisableNormals(ref PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx, iNormalIdx;

            Model.Normals = new Point3D[0];
            Model.NormalIndex[0] = 0;

            for (iPolyIdx = 0; iPolyIdx < Model.Header.numPolys; iPolyIdx++)
            {
                for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                {
                    Model.Polys[iPolyIdx].Normals[iVertIdx] = 0;
                }
            }

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (Model.Groups[iGroupIdx].polyType == 2) Model.Groups[iGroupIdx].polyType = 3;
            }

            for (iNormalIdx = 0; iNormalIdx < Model.Header.numNormIdx; iNormalIdx++)
            {
                Model.NormalIndex[iNormalIdx] = 0;
            }

            Model.Header.numNormals = 0;
        }

        public static void UpdateNormal(ref PModel Model, List<int> lstVerts, int[] lstAdjacentPolysIdxs)
        {
            int iGroupIdx, iPolyIdx, iNumPolys, iOffsetVert;

            Point3D p3DCurrentNormal;
            Point3D p3DAccumNormal = new Point3D();

            if (lstAdjacentPolysIdxs == null) return;

            iNumPolys = lstAdjacentPolysIdxs.Length;

            for (iPolyIdx = 0; iPolyIdx < iNumPolys; iPolyIdx++)
            {
                iGroupIdx = GetPolygonGroup(Model, lstAdjacentPolysIdxs[iPolyIdx]);
                iOffsetVert = Model.Groups[iGroupIdx].offsetVert;

                p3DCurrentNormal = CalculateNormal(
                    Model.Verts[Model.Polys[lstAdjacentPolysIdxs[iPolyIdx]].Verts[2] + iOffsetVert],
                    Model.Verts[Model.Polys[lstAdjacentPolysIdxs[iPolyIdx]].Verts[1] + iOffsetVert],
                    Model.Verts[Model.Polys[lstAdjacentPolysIdxs[iPolyIdx]].Verts[0] + iOffsetVert]);

                p3DAccumNormal.x += p3DCurrentNormal.x;
                p3DAccumNormal.y += p3DCurrentNormal.y;
                p3DAccumNormal.z += p3DCurrentNormal.z;
            }

            p3DAccumNormal.x /= iNumPolys;
            p3DAccumNormal.y /= iNumPolys;
            p3DAccumNormal.z /= iNumPolys;

            p3DAccumNormal = Normalize(p3DAccumNormal);

            foreach (int itmVert in lstVerts)
                Model.Normals[itmVert] = p3DAccumNormal;
        }

        public static void UpdateNormals(ref PModel Model, List<int> lstVerts, int[] lstAdjacentPolysIdxs,
                                         STIntVector[] lstAdjacentVertsIdsx, STIntVector[] lstAdjacentAdjacentPolysIdx)
        {
            int iVertIdx;

            if (lstAdjacentPolysIdxs == null) return;

            UpdateNormal(ref Model, lstVerts, lstAdjacentPolysIdxs);

            for (iVertIdx = 0; iVertIdx < lstAdjacentPolysIdxs.Length; iVertIdx++)
                UpdateNormal(ref Model, lstAdjacentVertsIdsx[iVertIdx].vector.ToList(), 
                                        lstAdjacentAdjacentPolysIdx[iVertIdx].vector);
        }



        //
        //
        //  IMPORTANT
        //
        //  The commented part of function is commented in original VB6 Kimera.
        public static void ComputeEdges(ref PModel Model)
        {
            int iGroupIdx;// iPolyIdx, iEdgeIdx, iVertIdx;
            //int iNumEdges;
            //bool bFound;

            Model.Edges = new PEdge[Model.Header.numPolys * 3];

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numPolys * 3; iGroupIdx++) 
                Model.Edges[iGroupIdx].Verts = new ushort[2];

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {

                // -- Commented in KimeraVB6
                //Model.Groups[iGroupIdx].offsetEdge = iNumEdges;
                //for (iPolyIdx = (int)Model.Groups[iGroupIdx].offsetPoly; iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly; iPolyIdx++)
                //{
                //    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                //    {
                //        bFound = false;

                //        for (iEdgeIdx = 0; iEdgeIdx < iNumEdges; iEdgeIdx++)
                //        {
                //            if (Model.Edges[iEdgeIdx].Verts[0] == Model.Polys[iPolyIdx].Verts[iVertIdx] && Model.Edges[ei].Verts[1] == Model.Polys[iPolyIdx].Verts[(iVertIdx + 1) % 3] ||
                //                Model.Edges[iEdgeIdx].Verts[1] == Model.Polys[iPolyIdx].Verts[iVertIdx] && Model.Edges[ei].Verts[0] == Model.Polys[iPolyIdx].Verts[(iVertIdx + 1) % 3])
                //            {
                //                bFound = true;
                //                break;
                //            }
                //        }

                //        if (!bFound)
                //        {
                //            Model.Edges[iNumEdges].Verts[0] = Model.Polys[iPolyIdx].Verts[iVertIdx];
                //            Model.Edges[iNumEdges].Verts[1] = Model.Polys[iPolyIdx].Verts[(iVertIdx + 1) % 3];

                //            Model.Polys[iPolyIdx].Edges[iVertIdx] = (short)(numEdges - Model.Groups[iGroupIdx].offsetEdge);
                //            iNumEdges++;
                //        }
                //        else Model.Polys[iPolyIdx].Edges[iVertIdx] = (short)(ei - Model.Groups[iGroupIdx].offsetEdge);

                //    }
                //}

                Model.Groups[iGroupIdx].numEdge = Model.Groups[iGroupIdx].numPoly * 3;  // num_edges - Model.Groups[iGroupIdx].offsetEdge
            }

            Model.Header.numEdges = Model.Header.numPolys * 3;  // num_edges
        }

        public static void ApplyCurrentVCoords(ref PModel Model)
        {
            int iGroupIdx, iVertIdx;
 
             for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
             {
                //glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                //glLoadIdentity();
                //glPushMatrix();

                //glTranslatef(Model.Groups[iGroupIdx].repGroupX, Model.Groups[iGroupIdx].repGroupY, Model.Groups[iGroupIdx].repGroupZ);

                //BuildRotationMatrixWithQuaternionsXYZ(Model.Groups[iGroupIdx].rotGroupAlpha,
                //                                      Model.Groups[iGroupIdx].rotGroupBeta,
                //                                      Model.Groups[iGroupIdx].rotGroupGamma,
                //                                      ref rot_mat);
                //glMultMatrixd(rot_mat);

                //glScalef(Model.Groups[iGroupIdx].rszGroupX, Model.Groups[iGroupIdx].rszGroupY, Model.Groups[iGroupIdx].rszGroupZ);

                for (iVertIdx = Model.Groups[iGroupIdx].offsetVert;
                     iVertIdx < Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert;
                     iVertIdx++)
                {
                    Model.Verts[iVertIdx] = GetEyeSpaceCoords(Model.Verts[iVertIdx]);
                }

                //glPopMatrix();
            }
        }

        public static void ApplyCurrentVCoordsPE(ref PModel Model)
        {
            int iVertIdx, iActualGroup;
            double[] rot_mat = new double[16];

            SetCameraModelViewQuat(repXPE, repYPE, repZPE,
                                   EditedPModel.rotationQuaternion,
                                   rszXPE, rszYPE, rszZPE);

            iActualGroup = GetNextGroup(Model, -1);

            while (iActualGroup != -1)
            {
                glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
                //glLoadIdentity();
                glPushMatrix();

                glTranslatef(Model.Groups[iActualGroup].repGroupX, Model.Groups[iActualGroup].repGroupY, Model.Groups[iActualGroup].repGroupZ);

                glScalef(Model.Groups[iActualGroup].rszGroupX, Model.Groups[iActualGroup].rszGroupY, Model.Groups[iActualGroup].rszGroupZ);

                BuildRotationMatrixWithQuaternionsXYZ(Model.Groups[iActualGroup].rotGroupAlpha,
                                                      Model.Groups[iActualGroup].rotGroupBeta,
                                                      Model.Groups[iActualGroup].rotGroupGamma,
                                                      ref rot_mat);

                glMultMatrixd(rot_mat);

                for (iVertIdx = Model.Groups[iActualGroup].offsetVert; 
                     iVertIdx < Model.Groups[iActualGroup].offsetVert + Model.Groups[iActualGroup].numVert; 
                     iVertIdx++)
                {
                    Model.Verts[iVertIdx] = GetEyeSpaceCoords(Model.Verts[iVertIdx]);
                }

                glPopMatrix();

                iActualGroup = GetNextGroup(Model, iActualGroup);
            }
        }

        public static void ComputePColors(ref PModel Model)
        {
            int iGroupIdx, iPolyIdx, iVertIdx;
            int iTmpR, iTmpG, iTmpB;

            Model.Pcolors = new Color[Model.Header.numPolys];

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
                     iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                     iPolyIdx++)
                {
                    iTmpR = iTmpG = iTmpB = 0;

                    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                    {
                        iTmpR += Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                               Model.Groups[iGroupIdx].offsetVert].R;
                        iTmpG += Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                               Model.Groups[iGroupIdx].offsetVert].G;
                        iTmpB += Model.Vcolors[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                               Model.Groups[iGroupIdx].offsetVert].B;
                    }

                    Model.Pcolors[iPolyIdx] = Color.FromArgb(255,
                                                             iTmpR / 3, iTmpG / 3, iTmpB / 3);
                }
            }
        }

        public static void ApplyPChangesPE(ref PModel Model, bool DNormals)
        {
            try
            {
                KillUnusedVertices(ref Model);

                ApplyCurrentVCoordsPE(ref Model);

                ComputePColors(ref Model);
                ComputeEdges(ref Model);

                if (DNormals) DisableNormals(ref Model);
                else ComputeNormals(ref Model);

                ComputeBoundingBox(ref Model);

                for (int iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
                {
                    Model.Groups[iGroupIdx].rszGroupX = 1;
                    Model.Groups[iGroupIdx].rszGroupY = 1;
                    Model.Groups[iGroupIdx].rszGroupZ = 1;
                    Model.Groups[iGroupIdx].repGroupX = 0;
                    Model.Groups[iGroupIdx].repGroupY = 0;
                    Model.Groups[iGroupIdx].repGroupZ = 0;
                    Model.Groups[iGroupIdx].rotGroupAlpha = 0;
                    Model.Groups[iGroupIdx].rotGroupBeta = 0;
                    Model.Groups[iGroupIdx].rotGroupGamma = 0;
                    Model.Groups[iGroupIdx].rotationQuaternionGroup.x = 0;
                    Model.Groups[iGroupIdx].rotationQuaternionGroup.y = 0;
                    Model.Groups[iGroupIdx].rotationQuaternionGroup.z = 0;
                    Model.Groups[iGroupIdx].rotationQuaternionGroup.w = 1;
                }

                Model.resizeX = 1;
                Model.resizeY = 1;
                Model.resizeZ = 1;

                Model.repositionX = 0;
                Model.repositionY = 0;
                Model.repositionZ = 0;

                Model.rotateAlpha = 0;
                Model.rotateBeta = 0;
                Model.rotateGamma = 0;

            }
            catch
            {
                MessageBox.Show("Error applying PChange at " + Model.fileName + "!!!", "Error", MessageBoxButtons.OK);
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= SAVING ==============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void ApplyPChanges(ref PModel Model, bool DNormals)
        {
            try
            {
                KillUnusedVertices(ref Model);

                ApplyCurrentVCoords(ref Model);

                ComputePColors(ref Model);
                ComputeEdges(ref Model);

                if (DNormals) DisableNormals(ref Model);
                else ComputeNormals(ref Model);

                ComputeBoundingBox(ref Model);

                Model.repositionX = 0;
                Model.repositionY = 0;
                Model.repositionZ = 0;
                Model.resizeX = 1;
                Model.resizeY = 1;
                Model.resizeZ = 1;
                Model.rotateAlpha = 0;
                Model.rotateBeta = 0;
                Model.rotateGamma = 0;
                Model.rotationQuaternion.x = 0;
                Model.rotationQuaternion.y = 0;
                Model.rotationQuaternion.z = 0;
                Model.rotationQuaternion.w = 1;

            }
            catch
            {
                MessageBox.Show("Error applying PChange at " + Model.fileName + "!!!", "Error", MessageBoxButtons.OK);
            }
        }

        public static void SetVColorsAlphaMAX(ref Color[] Vcolors)
        {
            int nColors, iColorIdx;

            nColors = Vcolors.Length;

            for (iColorIdx = 0; iColorIdx < nColors; iColorIdx++)
            {
                Vcolors[iColorIdx] = Color.FromArgb(128,
                                             Vcolors[iColorIdx].R,
                                             Vcolors[iColorIdx].G,
                                             Vcolors[iColorIdx].B);
            }
        }

        public static void WriteGlobalPModel(ref PModel Model, string fileName)
        {
            int iGroupIdx;

            try
            {
                SetVColorsAlphaMAX(ref Model.Vcolors);

                FileStream writeStream;

                using (writeStream = new FileStream(fileName, FileMode.Create))
                {
                    using (BinaryWriter fileWriter = new BinaryWriter(writeStream))
                    {
                        // Write Header
                        fileWriter.Write(Model.Header.version);
                        fileWriter.Write(Model.Header.off04);
                        fileWriter.Write(Model.Header.vertexColor);
                        fileWriter.Write(Model.Header.numVerts);
                        fileWriter.Write(Model.Header.numNormals);
                        fileWriter.Write(Model.Header.numXYZ);
                        fileWriter.Write(Model.Header.numTexCs);
                        fileWriter.Write(Model.Header.numNormIdx);
                        fileWriter.Write(Model.Header.numEdges);
                        fileWriter.Write(Model.Header.numPolys);
                        fileWriter.Write(Model.Header.off28);
                        fileWriter.Write(Model.Header.off2C);
                        fileWriter.Write(Model.Header.mirex_h);
                        fileWriter.Write(Model.Header.numGroups);
                        fileWriter.Write(Model.Header.mirex_g);
                        fileWriter.Write(Model.Header.off3C);
                        foreach (int intItem in Model.Header.unknown) fileWriter.Write(intItem);

                        // Write Verts
                        foreach (Point3D up3DVert in Model.Verts)
                        {
                            fileWriter.Write(up3DVert.x);
                            fileWriter.Write(up3DVert.y);
                            fileWriter.Write(up3DVert.z);
                        }

                        // Write Normals
                        if (Model.Normals.Length > 0)
                            foreach (Point3D up3DNormal in Model.Normals)
                            {
                                fileWriter.Write(up3DNormal.x);
                                fileWriter.Write(up3DNormal.y);
                                fileWriter.Write(up3DNormal.z);
                            }

                        // Write TryVerts
                        if (Model.XYZ != null)
                        {

                            //for (int i = 0; i < Model.XYZ.Length; i++)
                            //{
                            //    Model.XYZ[i].x = 0.5f;
                            //    Model.XYZ[i].y = 0.5f;
                            //    Model.XYZ[i].z = 0.5f;
                            //}

                            foreach (Point3D up3DXYZ in Model.XYZ)
                            {
                                fileWriter.Write(up3DXYZ.x);
                                fileWriter.Write(up3DXYZ.y);
                                fileWriter.Write(up3DXYZ.z);
                            }
                        }

                        // Write Texture Coords
                        if (Model.TexCoords != null)
                        {
                            foreach (Point2D up2DTexCoord in Model.TexCoords)
                            {
                                fileWriter.Write(up2DTexCoord.x);
                                fileWriter.Write(up2DTexCoord.y);
                            }
                        }

                        // Write VColors
                        foreach (Color colVColor in Model.Vcolors)
                        {
                            fileWriter.Write(colVColor.B);
                            fileWriter.Write(colVColor.G);
                            fileWriter.Write(colVColor.R);
                            fileWriter.Write(colVColor.A);
                        }

                        // Write PColors
                        foreach (Color colPColor in Model.Pcolors)
                        {
                            fileWriter.Write(colPColor.B);
                            fileWriter.Write(colPColor.G);
                            fileWriter.Write(colPColor.R);
                            fileWriter.Write(colPColor.A);
                        }

                        // Write Edges
                        foreach (PEdge itmEdge in Model.Edges)
                            foreach (ushort itmUShort in itmEdge.Verts) fileWriter.Write(itmUShort);

                        // Write Polygons
                        foreach (PPolygon itmPolygon in Model.Polys)
                        {
                            fileWriter.Write(itmPolygon.tag1);

                            foreach (short itmShort in itmPolygon.Verts) fileWriter.Write(itmShort);
                            foreach (short itmShort in itmPolygon.Normals) fileWriter.Write(itmShort);
                            foreach (short itmShort in itmPolygon.Edges) fileWriter.Write(itmShort);

                            fileWriter.Write(itmPolygon.tag2);
                        }

                        // Write Hundrets
                        foreach (PHundret itmHundret in Model.Hundrets)
                        {
                            fileWriter.Write(itmHundret.field_0);
                            fileWriter.Write(itmHundret.field_4);
                            fileWriter.Write(itmHundret.field_8);
                            fileWriter.Write(itmHundret.field_C);
                            fileWriter.Write(itmHundret.texID);
                            fileWriter.Write(itmHundret.texture_set_ptr);
                            fileWriter.Write(itmHundret.field_18);
                            fileWriter.Write(itmHundret.field_1C);
                            fileWriter.Write(itmHundret.field_20);
                            fileWriter.Write(itmHundret.shademode);
                            fileWriter.Write(itmHundret.lightstate_ambient);
                            fileWriter.Write(itmHundret.field_2C);
                            fileWriter.Write(itmHundret.lightstate_material_ptr);
                            fileWriter.Write(itmHundret.srcblend);
                            fileWriter.Write(itmHundret.destblend);
                            fileWriter.Write(itmHundret.field_3C);
                            fileWriter.Write(itmHundret.alpharef);
                            fileWriter.Write(itmHundret.blend_mode);
                            fileWriter.Write(itmHundret.zSort);
                            fileWriter.Write(itmHundret.field_4C);
                            fileWriter.Write(itmHundret.field_50);
                            fileWriter.Write(itmHundret.field_54);
                            fileWriter.Write(itmHundret.field_58);
                            fileWriter.Write(itmHundret.vertex_alpha);
                            fileWriter.Write(itmHundret.field_60);
                        }

                        // Write Groups
                        for (iGroupIdx = 0; iGroupIdx < Model.Groups.Length; iGroupIdx++)
                        {
                            fileWriter.Write(Model.Groups[iGroupIdx].polyType);
                            fileWriter.Write(Model.Groups[iGroupIdx].offsetPoly);
                            fileWriter.Write(Model.Groups[iGroupIdx].numPoly);
                            fileWriter.Write(Model.Groups[iGroupIdx].offsetVert);
                            fileWriter.Write(Model.Groups[iGroupIdx].numVert);
                            fileWriter.Write(Model.Groups[iGroupIdx].offsetEdge);
                            fileWriter.Write(Model.Groups[iGroupIdx].numEdge);
                            fileWriter.Write(Model.Groups[iGroupIdx].off1C);
                            fileWriter.Write(Model.Groups[iGroupIdx].off20);
                            fileWriter.Write(Model.Groups[iGroupIdx].off24);
                            fileWriter.Write(Model.Groups[iGroupIdx].off28);
                            fileWriter.Write(Model.Groups[iGroupIdx].offsetTex);
                            fileWriter.Write(Model.Groups[iGroupIdx].texFlag);
                            fileWriter.Write(Model.Groups[iGroupIdx].texID);
                        }

                        // Unknown 4 bytes before BoundingBox                      
                        fileWriter.Write(Model.BoundingBox.unknown4bytes);

                        // Write BoundingBox
                        fileWriter.Write(Model.BoundingBox.max_x);
                        fileWriter.Write(Model.BoundingBox.max_y);
                        fileWriter.Write(Model.BoundingBox.max_z);
                        fileWriter.Write(Model.BoundingBox.min_x);
                        fileWriter.Write(Model.BoundingBox.min_y);
                        fileWriter.Write(Model.BoundingBox.min_z);

                        // Write Normal Indexes
                        foreach (int itmInt in Model.NormalIndex) fileWriter.Write(itmInt);


                        // Last Steps
                        Model.resizeX = 1;
                        Model.resizeY = 1;
                        Model.resizeZ = 1;
                        Model.rotateAlpha = 0;
                        Model.rotateBeta = 0;
                        Model.rotateGamma = 0;
                        Model.repositionX = 0;
                        Model.repositionY = 0;
                        Model.repositionZ = 0;
                    }
                }
            }
            catch
            {
                //  Debug.Print "Error writing P file!!!"
                MessageBox.Show("Error writing PModel file " + fileName + ".", "Error", MessageBoxButtons.OK);
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= DESTROY =============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void DestroyPModelResources(ref PModel Model)
        {
            int iGroupIdx;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                glDeleteLists((uint)Model.Groups[iGroupIdx].DListNum, 1);
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  =========================================== COPY PMODEL ===========================================
        //  ---------------------------------------------------------------------------------------------------
        public static PHundret CopyPHundret(PHundret hundretIn)
        {
            PHundret newPHundret = new PHundret()
            {
                alpharef = hundretIn.alpharef,
                blend_mode = hundretIn.blend_mode,
                destblend = hundretIn.destblend,

                field_0 = hundretIn.field_0,
                field_4 = hundretIn.field_4,
                field_8 = hundretIn.field_8,
                field_C = hundretIn.field_C,
                field_18 = hundretIn.field_18,
                field_1C = hundretIn.field_1C,
                field_20 = hundretIn.field_20,
                field_2C = hundretIn.field_2C,
                field_3C = hundretIn.field_3C,
                field_4C = hundretIn.field_4C,
                field_50 = hundretIn.field_50,
                field_54 = hundretIn.field_54,
                field_58 = hundretIn.field_58,
                field_60 = hundretIn.field_60,

                lightstate_ambient = hundretIn.lightstate_ambient,
                lightstate_material_ptr = hundretIn.lightstate_material_ptr,
                shademode = hundretIn.shademode,
                srcblend = hundretIn.srcblend,
                texID = hundretIn.texID,
                texture_set_ptr = hundretIn.texture_set_ptr,
                vertex_alpha = hundretIn.vertex_alpha,
                zSort = hundretIn.zSort,
            };

            return newPHundret;
        }

        public static PModel CopyPModel(PModel modelIn)
        {
            PModel modelOut;
            int iCounter;

            if (modelIn.Polys == null) return modelIn;

            modelOut = new PModel()
            {
                fileName = modelIn.fileName,
                Header = modelIn.Header,
                Verts = new Point3D[modelIn.Verts.Length],
            };

            for (iCounter = 0; iCounter < modelIn.Verts.Length; iCounter++)
                modelOut.Verts[iCounter] = modelIn.Verts[iCounter];

            modelOut.Polys = new PPolygon[modelIn.Polys.Length];
            iCounter = 0;
            foreach (PPolygon itmPolygon in modelIn.Polys)
            {
                modelOut.Polys[iCounter] = new PPolygon(itmPolygon);
                iCounter++;
            }

            if (modelIn.Normals != null)
            {
                modelOut.Normals = new Point3D[modelIn.Normals.Length];
                for (iCounter = 0; iCounter < modelIn.Normals.Length; iCounter++)
                    modelOut.Normals[iCounter] = modelIn.Normals[iCounter];
            }

            if (modelIn.TexCoords != null)
            {
                if (modelIn.TexCoords.Length > 0)
                {
                    modelOut.TexCoords = new Point2D[modelIn.TexCoords.Length];
                    for (iCounter = 0; iCounter < modelIn.TexCoords.Length; iCounter++)
                        modelOut.TexCoords[iCounter] = modelIn.TexCoords[iCounter];
                }
            }

            modelOut.Vcolors = new Color[modelIn.Vcolors.Length];
            for (iCounter = 0; iCounter < modelIn.Vcolors.Length; iCounter++)
                modelOut.Vcolors[iCounter] = modelIn.Vcolors[iCounter];

            modelOut.Pcolors = new Color[modelIn.Pcolors.Length];
            for (iCounter = 0; iCounter < modelIn.Pcolors.Length; iCounter++)
                modelOut.Pcolors[iCounter] = modelIn.Pcolors[iCounter];

            modelOut.Hundrets = new PHundret[modelIn.Hundrets.Length];
            for (iCounter = 0; iCounter < modelIn.Hundrets.Length; iCounter++)
                modelOut.Hundrets[iCounter] = modelIn.Hundrets[iCounter];

            modelOut.Groups = new PGroup[modelIn.Groups.Length];
            for (iCounter = 0; iCounter < modelIn.Groups.Length; iCounter++)
                modelOut.Groups[iCounter] = modelIn.Groups[iCounter];

            modelOut.BoundingBox = modelIn.BoundingBox;

            if (modelIn.NormalIndex != null)
            {
                modelOut.NormalIndex = new int[modelIn.NormalIndex.Length];
                for (iCounter = 0; iCounter < modelIn.NormalIndex.Length; iCounter++)
                    modelOut.NormalIndex[iCounter] = modelIn.NormalIndex[iCounter];
            }

            ComputeEdges(ref modelOut);

            modelOut.repositionX = modelIn.repositionX;
            modelOut.repositionY = modelIn.repositionY;
            modelOut.repositionZ = modelIn.repositionZ;
            modelOut.resizeX = modelIn.resizeX;
            modelOut.resizeY = modelIn.resizeY;
            modelOut.resizeZ = modelIn.resizeZ;
            modelOut.rotateAlpha = modelIn.rotateAlpha;
            modelOut.rotateBeta = modelIn.rotateBeta;
            modelOut.rotateGamma = modelIn.rotateGamma;

            modelOut.rotationQuaternion = modelIn.rotationQuaternion;

            modelOut.diameter = modelIn.diameter;
            modelOut.DListNum = modelIn.DListNum;

            return modelOut;
        }



        //  ---------------------------------------------------------------------------------------------------
        //  ====================================== HELP PMODEL FUNCTIONS ======================================
        //  ---------------------------------------------------------------------------------------------------
        public static int GetPModelType(string strGlobalPModelName)
        {
            string strExt = Path.GetExtension(strGlobalPModelName).ToUpper();

            if (strExt.Length <= 0) return K_P_BATTLE_MODEL;
            else if (strExt == ".P" && strExt.Length < 3) return K_P_FIELD_MODEL;
            else return K_P_MAGIC_MODEL;
        }



        //  -------------------------------------------------------------------------------------------------
        //  ====================================== REMOVE PMODEL GROUP ======================================
        //  -------------------------------------------------------------------------------------------------
        // If iGroupIdx == -1, the we return the group that has the lower realGID
        public static int GetNextGroup(PModel Model, int iGroupIdx)
        {
            int iMinGID, iMaxGID, iNextGroup = -1;
            int iGroupCounter = 0;
            bool bFoundNextGroup = false;

            if (iGroupIdx == -1)
            {
                iMinGID = 9999;

                iNextGroup = 0;
                iGroupCounter = 0;
                foreach (PGroup itmGroup in Model.Groups)
                {
                    if (iMinGID > itmGroup.realGID)
                    {
                        iNextGroup = iGroupCounter;
                        iMinGID = itmGroup.realGID;
                    }
                    iGroupCounter++;
                }
            }
            else
            {
                iMinGID = Model.Groups[iGroupIdx].realGID;
                iMaxGID = 9999;

                foreach (PGroup itmGroup in Model.Groups)
                {
                    if (itmGroup.realGID < iMaxGID && itmGroup.realGID > iMinGID)
                    {
                        iMaxGID = itmGroup.realGID;
                        iNextGroup = iGroupCounter;
                        bFoundNextGroup = true;
                    }

                    iGroupCounter++;
                }

                if (!bFoundNextGroup) iNextGroup = -1;
            }

            return iNextGroup;
        }

        public static void RemoveGroupVColors(ref PModel Model, int iGroupIdx)
        {
            int vci, vci2, iNextGroup;

            iNextGroup = GetNextGroup(Model, iGroupIdx);
            if (iNextGroup != -1)
            {
                vci2 = Model.Groups[iGroupIdx].offsetVert;

                for (vci = Model.Groups[iNextGroup].offsetVert; vci < Model.Header.numVerts; vci++)
                {
                    Model.Vcolors[vci2] = Model.Vcolors[vci];
                    vci2++;
                }
            }

            Array.Resize(ref Model.Vcolors, Model.Header.numVerts - Model.Groups[iGroupIdx].numVert);
        }

        public static void RemoveGroupVertices(ref PModel Model, int iGroupIdx)
        {
            int iVertIdx, iVertIdxNext, iNextGroup;

            iNextGroup = GetNextGroup(Model, iGroupIdx);
            if (iNextGroup != -1)
            {
                iVertIdxNext = Model.Groups[iGroupIdx].offsetVert;

                for (iVertIdx = Model.Groups[iNextGroup].offsetVert; 
                     iVertIdx < Model.Header.numVerts; 
                     iVertIdx++)
                {
                    Model.Verts[iVertIdxNext] = Model.Verts[iVertIdx];
                    iVertIdxNext++;
                }
            }

            Array.Resize(ref Model.Verts, Model.Header.numVerts - Model.Groups[iGroupIdx].numVert);
        }

        public static void RemoveGroupPColors(ref PModel Model, int iGroupIdx)
        {
            int iPColorIdx, iPColorIdxNext, iNextGroup;

            iNextGroup = GetNextGroup(Model, iGroupIdx);
            if (iNextGroup != -1)
            {
                iPColorIdxNext = Model.Groups[iGroupIdx].offsetPoly;

                for (iPColorIdx = Model.Groups[iNextGroup].offsetPoly; 
                     iPColorIdx < Model.Header.numPolys; 
                     iPColorIdx++)
                {
                    Model.Pcolors[iPColorIdxNext] = Model.Pcolors[iPColorIdx];
                    iPColorIdxNext++;
                }
            }

            Array.Resize(ref Model.Pcolors, Model.Header.numPolys - Model.Groups[iGroupIdx].numPoly);
        }

        public static void RemoveGroupPolys(ref PModel Model, int iGroupIdx)
        {
            int iPolyIdx, iPolyIdxNext, iNextGroup;
            iNextGroup = GetNextGroup(Model, iGroupIdx);

            if (iNextGroup != -1)
            {
                iPolyIdxNext = Model.Groups[iGroupIdx].offsetPoly;

                for (iPolyIdx = Model.Groups[iNextGroup].offsetPoly;
                     iPolyIdx < Model.Header.numPolys; iPolyIdx++)
                {
                    Model.Polys[iPolyIdxNext] = Model.Polys[iPolyIdx];
                    iPolyIdxNext++;
                }
            }

            Array.Resize(ref Model.Polys, Model.Header.numPolys - Model.Groups[iGroupIdx].numPoly);
        }

        //public static void RemoveGroupEdges(ref PModel Model, int iGroupIdx)
        //{
        //    int ei, ei2, iNextGroup, iCountNumEdges, iCountEdgesGroups;

        //    iNextGroup = GetNextGroup(Model, iGroupIdx);
        //    if (iNextGroup != -1)
        //    {
        //        iCountEdgesGroups = GetNextGroup(Model, -1);
        //        iCountNumEdges = 0;

        //        while (iCountEdgesGroups != iGroupIdx)
        //        {
        //            iCountNumEdges += Model.Groups[iCountEdgesGroups].numEdge;
        //            iCountEdgesGroups = GetNextGroup(Model, iCountEdgesGroups);
        //        }

        //        if (Model.Groups[iGroupIdx].offsetEdge < Model.Edges.Length)
        //        {
        //            ei2 = Model.Groups[iGroupIdx].offsetEdge;

        //            for (ei = iCountNumEdges; ei < Model.Header.numEdges; ei++)
        //            {
        //                Model.Edges[ei2] = Model.Edges[ei];
        //                ei2++;
        //            }
        //        }
        //    }

        //    Array.Resize(ref Model.Edges, Model.Header.numEdges - Model.Groups[iGroupIdx].numEdge);
        //}

        public static void RemoveGroupTexCoords(ref PModel Model, int iGroupIdx)
        {
            int iTexCoordGroupIdxCount, iTexCoordNextGroupCount, iNumTexCoordGroupIdx, iNextGroup;

            iNextGroup = GetNextGroup(Model, iGroupIdx);
            iNumTexCoordGroupIdx = Model.Groups[iGroupIdx].numVert;

            while (iNextGroup != -1)
            {

                if (Model.Groups[iGroupIdx].texFlag == 1)
                {

                    iTexCoordNextGroupCount = Model.Groups[iGroupIdx].offsetTex;

                    // Let's check if is the last array Tex Coords to move. Then there is no need to
                    // do the moving.
                    if (iTexCoordNextGroupCount + Model.Groups[iGroupIdx].numVert < Model.Header.numTexCs)
                    {
                        if (Model.Groups[iNextGroup].texFlag == 1)
                        {
                            for (iTexCoordGroupIdxCount = Model.Groups[iNextGroup].offsetTex;
                                 iTexCoordGroupIdxCount < Model.Groups[iNextGroup].offsetTex + Model.Groups[iNextGroup].numVert;
                                 iTexCoordGroupIdxCount++)
                            {
                                Model.TexCoords[iTexCoordNextGroupCount] = Model.TexCoords[iTexCoordGroupIdxCount];
                                iTexCoordNextGroupCount++;
                            }

                            iGroupIdx = iNextGroup;
                            iNextGroup = GetNextGroup(Model, iGroupIdx);

                                if (Model.Groups[iGroupIdx].texFlag == 1)
                                    Model.Groups[iGroupIdx].offsetTex = iTexCoordNextGroupCount;
                        }
                        else
                        {
                            iNextGroup = GetNextGroup(Model, iNextGroup);
                        }                        
                    }
                    else
                    {
                        iNextGroup = GetNextGroup(Model, iNextGroup);
                    }
                }
            }

            Array.Resize(ref Model.TexCoords, Model.Header.numTexCs - iNumTexCoordGroupIdx);

            //if (iNextGroup != -1)
            //{
            //    if (Model.Groups[iGroupIdx].texFlag == 1)
            //    {
            //        ti2 = Model.Groups[iGroupIdx].offsetTex;

            //        // Let's check if is the last array Tex Coords to move. Then there is no need to
            //        // do the moving.
            //        if (ti2 + Model.Groups[iGroupIdx].numVert < Model.Header.numTexCs)
            //        {
            //            for (ti = Model.Groups[iNextGroup].offsetTex; ti < Model.Header.numTexCs; ti++)
            //            {
            //                Model.TexCoords[ti2] = Model.TexCoords[ti];
            //                ti2++;
            //            }
            //        }
            //    }
            //}

            //Array.Resize(ref Model.TexCoords, Model.Header.numTexCs - Model.Groups[iGroupIdx].numVert);
        }

        public static void RemoveGroupHundret(ref PModel Model, int iGroupIdx)
        {
            int hi;

            if (iGroupIdx < Model.Hundrets.Length - 1)
            {
                for (hi = iGroupIdx + 1; hi < Model.Hundrets.Length; hi++)
                    Model.Hundrets[hi - 1] = Model.Hundrets[hi];
            }

            Array.Resize(ref Model.Hundrets, Model.Hundrets.Length -1);
        }

        public static void RemoveGroupGroup(ref PModel Model, int iGroupIdx)
        {
            int iGroupIdxNext;

            if (iGroupIdx < Model.Groups.Length - 1)
            {
                for (iGroupIdxNext = iGroupIdx + 1; 
                     iGroupIdxNext < Model.Groups.Length; 
                     iGroupIdxNext++)
                    Model.Groups[iGroupIdxNext - 1] = Model.Groups[iGroupIdxNext];
            }

            Array.Resize(ref Model.Groups, Model.Groups.Length - 1);
        }

        public static void RemoveGroupHeader(ref PModel Model, int iGroupIdx)
        {
            Model.Header.numPolys -= Model.Groups[iGroupIdx].numPoly;
            Model.Header.numEdges -= Model.Groups[iGroupIdx].numEdge;
            Model.Header.numVerts -= Model.Groups[iGroupIdx].numVert;

            if (Model.Groups[iGroupIdx].texFlag == 1)
                Model.Header.numTexCs -= Model.Groups[iGroupIdx].numVert;

            Model.Header.mirex_h--;

            Model.Header.numGroups--;
        }

        public static void RemoveGroup(ref PModel Model, int iGroupIdx)
        {
            //int gi, gi2, giActualGroup, giPrevGroup, giMinOffPoly, giMaxOffPoly;
            int iActualGroup, iNextGroup, iNumTexCoords, iRealGID;
            bool bGroupHasOffsetPolyZero;

            bGroupHasOffsetPolyZero = Model.Groups[iGroupIdx].offsetPoly == 0;

            if (Model.Groups[iGroupIdx].numVert > 0)
            {
                RemoveGroupVColors(ref Model, iGroupIdx);
                RemoveGroupVertices(ref Model, iGroupIdx);
                RemoveGroupPColors(ref Model, iGroupIdx);
                RemoveGroupPolys(ref Model, iGroupIdx);
                //RemoveGroupEdges(ref Model, iGroupIdx);

                if (Model.Groups[iGroupIdx].texFlag == 1)
                        RemoveGroupTexCoords(ref Model, iGroupIdx);
                else
                {
                    if  (Model.Groups[iGroupIdx].offsetTex > 0)
                        if (MessageBox.Show("Group " + iGroupIdx.ToString("00") + " seems to have Texture " +
                                            "Coordinates assigned, but it has not any Texture assigned.\nDo you " +
                                            "want -Reset to 0- the Texture Coordinates?\n\nNOTE: If you don't reset " +
                                            "them to 0, the Texture Flag will be enabled.", 
                                            "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Model.Groups[iGroupIdx].offsetTex = 0;
                        }
                }
            }
            else
            {
                //  Debug.Print obj.Groups(g_index).numvert; " "; obj.Groups(g_index).numPoly
            }

            RemoveGroupHundret(ref Model, iGroupIdx);
            RemoveGroupHeader(ref Model, iGroupIdx);
            RemoveGroupGroup(ref Model, iGroupIdx);


            // Now we need to recalculate offsets (verts, polys, edges and tex)
            if (Model.Groups.Length == 1)
            {
                Model.Groups[0].offsetVert = 0;
                Model.Groups[0].offsetPoly = 0;
                Model.Groups[0].offsetEdge = 0;
                Model.Groups[0].offsetTex = 0;
                Model.Groups[0].realGID = 0;
            }
            else
            {
                // As we can change the group order, we need to check the group by the
                // number of polys.
                iRealGID = 1;
                iGroupIdx = GetNextGroup(Model, -1);

                // Check if we want to remove a group of offset zero or not.
                if (bGroupHasOffsetPolyZero)
                {
                    // We have the group with polys 0. Update offsets directly.
                    Model.Groups[iGroupIdx].offsetVert = 0;
                    Model.Groups[iGroupIdx].offsetPoly = 0;
                    Model.Groups[iGroupIdx].offsetEdge = 0;
                    Model.Groups[iGroupIdx].offsetTex = 0;
                    Model.Groups[iGroupIdx].realGID = 0;
                }

                // While there are groups to recalculate:
                iActualGroup = iGroupIdx;
                iNextGroup = GetNextGroup(Model, iGroupIdx);
                iNumTexCoords = Model.Groups[iActualGroup].offsetTex;

                while (iNextGroup != -1)
                {
                    // Assign new offsets to the next group found
                    Model.Groups[iNextGroup].offsetVert =
                        Model.Groups[iActualGroup].offsetVert + Model.Groups[iActualGroup].numVert;
                    Model.Groups[iNextGroup].offsetPoly =
                        Model.Groups[iActualGroup].offsetPoly + Model.Groups[iActualGroup].numPoly;
                    Model.Groups[iNextGroup].offsetEdge =
                        Model.Groups[iActualGroup].offsetEdge + Model.Groups[iActualGroup].numEdge;

                    // Let's do TexCoords
                    if (Model.Groups[iActualGroup].texFlag == 1)       // Can be some poly with texture set to 0 but with UV.
                    {
                        Model.Groups[iActualGroup].offsetTex = iNumTexCoords;
                        iNumTexCoords += Model.Groups[iActualGroup].numVert;
                    }

                    // Update realGID of the group
                    Model.Groups[iNextGroup].realGID = iRealGID;
                    iRealGID++;

                    iActualGroup = iNextGroup;
                    iNextGroup = GetNextGroup(Model, iActualGroup);
                }

                // Let's assign latest TexCoords if needed.
                if (Model.Groups[iActualGroup].texFlag == 1)
                    Model.Groups[iActualGroup].offsetTex = iNumTexCoords;
           }

        }


        //  -------------------------------------------------------------------------------------------------
        //  ======================================= PEDITOR PROCEDURES ======================================
        //  -------------------------------------------------------------------------------------------------
        //public static int GetClosestVertex(PModel Model, int px, int py, float DIST0, PictureBox panelEditorPModel)
        public static int GetClosestVertex(PModel Model, int px, int py)
        {
            int iGetClosestVertexResult = -1;

            Point3D pUP3D = new Point3D();
            Point3D vpUP3D;
            int iGroupIdx, iPolyIdx, iVertIdx, iHeight;
            int[] vp = new int[4];

            float[] DIST = new float[3];
            float minDist;

            pUP3D.x = px;
            pUP3D.y = py;
            pUP3D.z = 0;

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            iHeight = vp[3];

            //pi = GetClosestPolygon(Model, px, py, DIST0, panelEditorPModel);
            iPolyIdx = GetClosestPolygon(Model, px, py);

            if (iPolyIdx > -1)
            {
                iGroupIdx = GetPolygonGroup(Model, iPolyIdx);

                pUP3D.y = iHeight - py;
                for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                {
                    vpUP3D = 
                        GetVertexProjectedCoords(Model.Verts, 
                                                 Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert);

                    DIST[iVertIdx] = CalculateDistance(vpUP3D, pUP3D);
                }

                minDist = DIST[0];
                iGetClosestVertexResult = Model.Polys[iPolyIdx].Verts[0] + Model.Groups[iGroupIdx].offsetVert;

                for (iVertIdx = 1; iVertIdx < 3; iVertIdx++)
                {
                    if (DIST[iVertIdx] < minDist)
                    {
                        minDist = DIST[iVertIdx];
                        iGetClosestVertexResult = Model.Polys[iPolyIdx].Verts[iVertIdx] + Model.Groups[iGroupIdx].offsetVert;
                    }
                }
            }

            return iGetClosestVertexResult;
        }

        //public static int GetClosestPolygon(PModel Model, int px, int py, float DIST, PictureBox panelEditorPModel)
        public static int GetClosestPolygon(PModel Model, int px, int py)
        {
            int iGetClosestPolygonResult;
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            int iGroupIdx, iPolyIdx, iVertIdx, iNumPolys, iHeight;
            float minZ;
            double[] rot_mat = new double[16];
            int[] vp = new int[4];
            
            int[] selBuff = new int[Model.Header.numPolys * 4];

            //glViewport(0, 0, panelEditorPModel.ClientRectangle.Width, panelEditorPModel.ClientRectangle.Height);
            //ClearPanel();

            //SetDefaultOGLRenderState();

            ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);

            SetCameraAroundModel(ref p_min, ref p_max,
                                 panXPE, panYPE, panZPE + DISTPE,
                                 alphaPE, betaPE, gammaPE, 1, 1, 1);

            glDisable(GLCapability.GL_LIGHTING);

            glMatrixMode(GLMatrixModeList.GL_MODELVIEW);

            glPushMatrix();

            glTranslatef(EditedPModel.repositionX,
                         EditedPModel.repositionY,
                         EditedPModel.repositionZ);

            BuildRotationMatrixWithQuaternionsXYZ(EditedPModel.rotateAlpha,
                                                  EditedPModel.rotateBeta,
                                                  EditedPModel.rotateGamma,
                                                  ref rot_mat);

            glMultMatrixd(rot_mat);
            glScalef(EditedPModel.resizeX,
                     EditedPModel.resizeY,
                     EditedPModel.resizeZ);

            glEnable(GLCapability.GL_POLYGON_OFFSET_FILL);
            glPolygonOffset(1, 1);

            glSelectBuffer(Model.Header.numPolys * 4, selBuff);

            glPolygonMode(GLFace.GL_FRONT, GLPolygon.GL_LINE);
            glPolygonMode(GLFace.GL_BACK, GLPolygon.GL_FILL);
            glEnable(GLCapability.GL_COLOR_MATERIAL);

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            iHeight = vp[3];

            glRenderMode(GLRenderingMode.GL_SELECT);

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glPushMatrix();
            glGetDoublev((uint)GLCapability.GL_PROJECTION_MATRIX, rot_mat);
            glLoadIdentity();

            gluPickMatrix(px - 1, iHeight - py + 1, 3, 3, vp);
            glMultMatrixd(rot_mat);

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                glInitNames();

                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly; 
                         iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                         iPolyIdx++)
                    {
                        glColor4f(Model.Pcolors[iPolyIdx].R / 255.0f, 
                                  Model.Pcolors[iPolyIdx].G / 255.0f, 
                                  Model.Pcolors[iPolyIdx].B / 255.0f, 
                                  Model.Pcolors[iPolyIdx].A / 255.0f);

                        glColorMaterial(GLFace.GL_FRONT_AND_BACK, GLMaterialParameter.GL_AMBIENT_AND_DIFFUSE);

                        glPushName((uint)iPolyIdx);
                        glBegin(GLDrawMode.GL_TRIANGLES);
                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                        {
                            glNormal3f(Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx]] +
                                                                       Model.Groups[iGroupIdx].offsetVert].x,
                                       Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx]] +
                                                                       Model.Groups[iGroupIdx].offsetVert].y,
                                       Model.Normals[Model.NormalIndex[Model.Polys[iPolyIdx].Verts[iVertIdx]] +
                                                                       Model.Groups[iGroupIdx].offsetVert].z);

                            glVertex3f(Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].x,
                                       Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].y,
                                       Model.Verts[Model.Polys[iPolyIdx].Verts[iVertIdx] + 
                                                   Model.Groups[iGroupIdx].offsetVert].z);
                        }
                        glEnd();
                        glPopName();
                    }
                }
            }
            
            glDisable(GLCapability.GL_POLYGON_OFFSET_FILL);
            glPopMatrix();

            iNumPolys = glRenderMode(GLRenderingMode.GL_RENDER);
            iGetClosestPolygonResult = -1;
            minZ = -1;

            for (iPolyIdx = 0; iPolyIdx < iNumPolys; iPolyIdx++)
            {
                if (CompareLongs((long)minZ, selBuff[iPolyIdx * 4 + 1]) ||
                    selBuff[iPolyIdx * 4 + 1] == minZ)
                {
                    minZ = selBuff[iPolyIdx * 4 + 1];
                    iGetClosestPolygonResult = selBuff[iPolyIdx * 4 + 3];
                }
            }

            glMatrixMode(GLMatrixModeList.GL_PROJECTION);
            glPopMatrix();

            return iGetClosestPolygonResult;
        }

        public static int GetPolygonGroup(PModel Model, int iPolyIdx)
        {
            int iPolyCounter, iNextGroup;

            // Get first 0 group
            iNextGroup = GetNextGroup(Model, -1);
            iPolyCounter = Model.Groups[iNextGroup].numPoly;

            // Get other groups
            while (iPolyCounter <= iPolyIdx)
            {
                iNextGroup = GetNextGroup(Model, iNextGroup);

                iPolyCounter += Model.Groups[iNextGroup].numPoly;
            }

            return iNextGroup;
        }


        //  -------------------------------WARNINGS!------------------------------
        //  -------*Causes the Normals to be inconsistent (call ComputeNormals).--
        //  -------*Causes inconsistent edges (call ComputeEdges).----------------
        //  -------*Can cause unused vertices (call KillUnusedVertices).----------
        public static void RemovePolygon(ref PModel Model, int iRemovePolyIdx)
        {
            int iRemoveGroupIdx, iPolyIdx, iNextGroup;

            iRemoveGroupIdx = GetPolygonGroup(Model, iRemovePolyIdx);

            // -- Commented in KimeraVB6
            //  if (Model.Header.numPolys == 1)
            //  {
            //      MessageBox.Show"A P model must have at least 1 polygon. Can't remove this polygon.", "Info");
            //      return;
            //  }

            Model.Header.numPolys--;

            // Move all polys array
            for (iPolyIdx = iRemovePolyIdx; iPolyIdx < Model.Header.numPolys; iPolyIdx++)
            {
                Model.Polys[iPolyIdx] = Model.Polys[iPolyIdx + 1];
                Model.Pcolors[iPolyIdx] = Model.Pcolors[iPolyIdx + 1];
            }

            Model.Groups[iRemoveGroupIdx].numPoly--;

            iNextGroup = GetNextGroup(Model, iRemoveGroupIdx);
            while (iNextGroup != -1)
            {
                Model.Groups[iNextGroup].offsetPoly--;

                iNextGroup = GetNextGroup(Model, iNextGroup);
            }

            //  This is technically wrong. The vector should be emptied if Model.Header.numPolys droped to 0,
            //  but they should be inmediately refilled with something else because a P Model can't have 0
            //  polygons.
            if (Model.Header.numPolys > 0)
            {
                Array.Resize(ref Model.Polys, Model.Header.numPolys);
                Array.Resize(ref Model.Pcolors, Model.Header.numPolys);
            }

        }

        public static int GetClosestEdge(PModel Model, int iPolyIdx, int px, int py, ref float alpha)
        {
            int iGetClosestEdgeReturn;

            Point3D tmpUP3D = new Point3D();
            Point3D p1Proj, p2Proj, p3Proj;
            Point3D p1, p2, p3;

            float d1, d2, d3;

            int height, offsetVerts;
            int[] vp = new int[4];

            glGetIntegerv((uint)GLCapability.GL_VIEWPORT, vp);
            height = vp[3];

            tmpUP3D.x = px;
            tmpUP3D.y = height - py;
            tmpUP3D.z = 0;

            offsetVerts = Model.Groups[GetPolygonGroup(Model, iPolyIdx)].offsetVert;

            // -- Commented in KimeraVB6
            //glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            //glPushMatrix();
            //glScalef(Model.resizeX, Model.resizeY, Model.resizeZ);
            //glRotatef(Model.rotateAlpha, 1, 0, 0);
            //glRotatef(Model.rotateBeta, 0, 1, 0);
            //glRotatef(Model.rotateGamma, 0, 0, 1);
            //glTranslatef(Model.repositionX, Model.repositionY, Model.repositionZ);

            p1Proj = GetVertexProjectedCoords(Model.Verts, Model.Polys[iPolyIdx].Verts[0] + offsetVerts);
            p2Proj = GetVertexProjectedCoords(Model.Verts, Model.Polys[iPolyIdx].Verts[1] + offsetVerts);
            p3Proj = GetVertexProjectedCoords(Model.Verts, Model.Polys[iPolyIdx].Verts[2] + offsetVerts);

            p1 = CalculatePoint2LineProjection(tmpUP3D, p1Proj, p2Proj);
            p2 = CalculatePoint2LineProjection(tmpUP3D, p2Proj, p3Proj);
            p3 = CalculatePoint2LineProjection(tmpUP3D, p3Proj, p1Proj);

            d1 = CalculateDistance(tmpUP3D, p1);
            d2 = CalculateDistance(tmpUP3D, p2);
            d3 = CalculateDistance(tmpUP3D, p3);

            if (d1 > d2)
            {
                if (d2 > d3)
                {
                    iGetClosestEdgeReturn = 2;
                    alpha = CalculatePoint2LineProjectionPosition(tmpUP3D, p3Proj, p1Proj);
                }
                else
                {
                    iGetClosestEdgeReturn = 1;
                    alpha = CalculatePoint2LineProjectionPosition(tmpUP3D, p2Proj, p3Proj);
                }
            }
            else
            {
                if (d1 > d3)
                {
                    iGetClosestEdgeReturn = 2;
                    alpha = CalculatePoint2LineProjectionPosition(tmpUP3D, p3Proj, p1Proj);
                }
                else
                {
                    iGetClosestEdgeReturn = 0;
                    alpha = CalculatePoint2LineProjectionPosition(tmpUP3D, p1Proj, p2Proj);
                }
            }

            // -- Commented in KimeraVB6
            //glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            //glPopMatrix();

            return iGetClosestEdgeReturn;
        }

        public static void CopyVPColors(Color[] vpcolorsIn, ref Color[] vpcolorsOut)
        {
            int iVertIdx;

            if (vpcolorsOut == null) vpcolorsOut = new Color[vpcolorsIn.Length];
            else Array.Resize(ref vpcolorsOut, vpcolorsIn.Length);

            for (iVertIdx = 0; iVertIdx < vpcolorsIn.Length; iVertIdx++)
                vpcolorsOut[iVertIdx] = vpcolorsIn[iVertIdx];
        }

        public static void CopyVPColors2Model(ref PModel Model, Color[] vcolorsIn, Color[] pcolorsIn)
        {
            CopyVPColors(vcolorsIn, ref Model.Vcolors);
            CopyVPColors(pcolorsIn, ref Model.Pcolors);
        }

        public static void CopyModelColors2VP(PModel Model, ref Color[] vcolorsOut, ref Color[] pcolorsOut)
        {
            CopyVPColors(Model.Vcolors, ref vcolorsOut);
            CopyVPColors(Model.Pcolors, ref pcolorsOut);
        }

        public static Point3D GetPointMirroredRelativeToPlane(Point3D tmpUP3D, float pA, float pB, float pC, float pD)
        {
            Point3D iGetPointMirroredRelativeToPlaneResult = new Point3D();
            float alpha;

            alpha = (-pA * tmpUP3D.x - pB * tmpUP3D.y - pC * tmpUP3D.z - pD) / (pA * pA + pB * pB + pC * pC);

            iGetPointMirroredRelativeToPlaneResult.x = tmpUP3D.x + 2 * alpha * pA;
            iGetPointMirroredRelativeToPlaneResult.y = tmpUP3D.y + 2 * alpha * pB;
            iGetPointMirroredRelativeToPlaneResult.z = tmpUP3D.z + 2 * alpha * pC;

            return iGetPointMirroredRelativeToPlaneResult;
        }

        public static void MirrorGroupRelativeToPlane(ref PModel Model, int iGroupIdx, float pA, float pB, float pC, float pD)
        {
            int iVertIdx, iPolyIdx, iTmp;
            Point3D tmpP3D;

            for (iVertIdx = Model.Groups[iGroupIdx].offsetVert; 
                 iVertIdx < Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert; 
                 iVertIdx++)
            {
                tmpP3D = GetPointMirroredRelativeToPlane(Model.Verts[iVertIdx], pA, pB, pC, pD);

                if (CalculateDistance(tmpP3D, Model.Verts[iVertIdx]) > 0.00001f) 
                            Model.Verts[iVertIdx] = tmpP3D;
            }

            //  Flip faces
            for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly; 
                 iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly; 
                 iPolyIdx++)
            {
                iTmp = Model.Polys[iPolyIdx].Verts[0];
                Model.Polys[iPolyIdx].Verts[0] = Model.Polys[iPolyIdx].Verts[1];
                Model.Polys[iPolyIdx].Verts[1] = (ushort)iTmp;
            }
        }

        public static void MirrorHemisphere(ref PModel Model, float pA, float pB, float pC, float pD)
        {
            int iGroupIdx;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    MirrorGroupRelativeToPlane(ref Model, iGroupIdx, pA, pB, pC, pD);
                }
            }
        }

        public static bool DuplicateGroup(ref PModel Model, int iGroupIdx)
        {
            bool iDuplicateGroupResult = false;
            int iPolyIdx, iVertIdx, iTexCoordIdx;

            Point3D[] aVerts;
            PPolygon[] aPolys;
            Point2D[] aTexCoords = null;
            Color[] aVColors;
            Color[] aPColors;

            //  Don't duplicate empty groups
            if (Model.Groups[iGroupIdx].numVert > 0 && Model.Groups[iGroupIdx].numPoly > 0)
            {
                aVerts = new Point3D[Model.Groups[iGroupIdx].numVert];
                aVColors = new Color[Model.Groups[iGroupIdx].numVert];
                for (iVertIdx = 0;
                     iVertIdx < Model.Groups[iGroupIdx].numVert;
                     iVertIdx++)
                {
                    aVerts[iVertIdx] = 
                        Model.Verts[Model.Groups[iGroupIdx].offsetVert + iVertIdx];
                    aVColors[iVertIdx] = 
                        Model.Vcolors[Model.Groups[iGroupIdx].offsetVert + iVertIdx];
                }

                aPolys = new PPolygon[Model.Groups[iGroupIdx].numPoly];
                aPColors = new Color[Model.Groups[iGroupIdx].numPoly];
                for (iPolyIdx = 0; iPolyIdx < Model.Groups[iGroupIdx].numPoly; iPolyIdx++)
                {
                    aPolys[iPolyIdx] = 
                        new PPolygon(Model.Polys[Model.Groups[iGroupIdx].offsetPoly + iPolyIdx]);
                    aPColors[iPolyIdx] = 
                        Model.Pcolors[Model.Groups[iGroupIdx].offsetPoly + iPolyIdx];
                }

                if (Model.Groups[iGroupIdx].texFlag == 1)
                {
                    aTexCoords = new Point2D[Model.Groups[iGroupIdx].numVert];
                    for (iTexCoordIdx = 0; 
                         iTexCoordIdx < Model.Groups[iGroupIdx].numVert; 
                         iTexCoordIdx++)
                    {
                        aTexCoords[iTexCoordIdx] = 
                            Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + iTexCoordIdx];
                    }
                }                           
                
                AddGroup(ref Model, aVerts, aPolys, aTexCoords, aVColors, aPColors, 
                         Model.Groups[iGroupIdx].texID);

                Model.Hundrets[Model.Header.numGroups - 1] = CopyPHundret(Model.Hundrets[iGroupIdx]);

                iDuplicateGroupResult = true;
            }

            return iDuplicateGroupResult;
        }

        public static void DuplicateMirrorHemisphere(ref PModel Model, float pA, float pB, float pC, float pD)
        {
            int iGroupIdx, iGroupIdxMirror, iNumGroups;

            iNumGroups = Model.Header.numGroups;
            iGroupIdxMirror = iNumGroups;

            for (iGroupIdx = 0; iGroupIdx < iNumGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    if (DuplicateGroup(ref Model, iGroupIdx))
                    {
                        MirrorGroupRelativeToPlane(ref Model, iGroupIdxMirror, pA, pB, pC, pD);
                        iGroupIdxMirror++;
                    }
                }
            }
        }

        public static Point3D GetPointInLine(Point3D p1, Point3D p2, float alpha)
        {
            return new Point3D(p1.x + (p2.x - p1.x) * alpha,
                               p1.y + (p2.y - p1.y) * alpha,
                               p1.z + (p2.z - p1.z) * alpha);
        }

        public static Point2D GetPointInLine2D(Point2D p1, Point2D p2, float alpha)
        {
            return new Point2D(p1.x + (p2.x - p1.x) * alpha,
                               p1.y + (p2.y - p1.y) * alpha);
        }

        public static Color CombineColor(Color colA, Color colB)
        {
            return Color.FromArgb((colA.A + colB.A) / 2,
                                  (colA.R + colB.R) / 2,
                                  (colA.G + colB.G) / 2,
                                  (colA.B + colB.B) / 2);
        }

        public static int FindVertexInGroup(PModel Model, Point3D vPoint3D, int iGroupIdx)
        {
            int iVertIdx;
            int iFindVertexInGroupResult;
            bool bFound = false;

            iVertIdx = Model.Groups[iGroupIdx].offsetVert;

            while (iVertIdx < Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert &&
                   !bFound)
            {
                if (Model.Verts[iVertIdx].x == vPoint3D.x &&
                    Model.Verts[iVertIdx].y == vPoint3D.y &&
                    Model.Verts[iVertIdx].z == vPoint3D.z)
                {
                    bFound = true;
                }
                else iVertIdx++;
            }

            if (bFound) iFindVertexInGroupResult = iVertIdx;
            else iFindVertexInGroupResult = -1;

            return iFindVertexInGroupResult;
        }

        public static int AddVertex(ref PModel Model, int iGroupIdx, int iInputVertIdx, 
                                    Point3D vPoint3D, Color vColor)
        {
            //  -------- Warning! Causes the Normals to be inconsistent if lights are disabled.------------------
            //  --------------------------------Must call ComputeNormals ----------------------------------------

            int iAddVertexResult = -1;
            int iVertIdx, iTexCoordIdx, baseVerts, baseTexCoords;
            int iNextGroup, iExistsVertex;

            // We must be sure that the vertex does not exists
            // If it exists we could reuse an original vertex
            // We can came from CutEdges (-999999) or from PEditor New Poly (iInputVertIdx)
            if (iInputVertIdx == -999999)
                iExistsVertex = FindVertexInGroup(Model, vPoint3D, iGroupIdx);
            else
                iExistsVertex = iInputVertIdx;

            // We found the vertex in the array. Let's reuse it
            if (iExistsVertex != -1)
            {
                iAddVertexResult = iExistsVertex;
            }
            else
            {
                // First increase model Header num Verts and Verts array to put New Vertex
                // We will also do the same for Vcolors array
                Model.Header.numVerts++;
                Array.Resize(ref Model.Verts, Model.Header.numVerts);
                Array.Resize(ref Model.Vcolors, Model.Header.numVerts);

                // In case we have Texture Coordinates, we will increase also its number
                if (Model.Groups[iGroupIdx].texFlag == 1)
                {
                    Model.Header.numTexCs++;
                    Array.Resize(ref Model.TexCoords, Model.Header.numTexCs);
                }

                // Insert Vert (and VColor and TextureCoordinate)
                iNextGroup = GetNextGroup(Model, iGroupIdx);
                if (iNextGroup != -1)
                {
                    baseVerts = Model.Groups[iNextGroup].offsetVert;

                    for (iVertIdx = Model.Header.numVerts - 1; iVertIdx >= baseVerts; iVertIdx--)
                    {
                        Model.Verts[iVertIdx] = Model.Verts[iVertIdx - 1];
                        Model.Vcolors[iVertIdx] = Model.Vcolors[iVertIdx - 1];
                    }

                    if (Model.Groups[iGroupIdx].texFlag == 1)
                    {
                        //baseTexCoords = Model.Groups[iGroupIdx].offsetTex + Model.Groups[iGroupIdx].numVert;
                        baseTexCoords = Model.Groups[iGroupIdx].offsetTex + Model.Groups[iGroupIdx].numVert;

                        for (iTexCoordIdx = Model.Header.numTexCs - 1; iTexCoordIdx >= baseTexCoords; iTexCoordIdx--)
                        {
                            Model.TexCoords[iTexCoordIdx] = Model.TexCoords[iTexCoordIdx - 1];
                        }
                    }

                    // Now we need to increase the offset of the vertices of remaining groups
                    // AND increment +3 each vertex index of the other polys
                    while (iNextGroup != -1)
                    {
                        Model.Groups[iNextGroup].offsetVert++;

                        if (Model.Groups[iGroupIdx].texFlag == 1 && Model.Groups[iNextGroup].texFlag == 1)
                            Model.Groups[iNextGroup].offsetTex++;

                        iNextGroup = GetNextGroup(Model, iNextGroup);
                    }
                }

                if (iGroupIdx < Model.Header.numGroups)
                {
                    iAddVertexResult = Model.Groups[iGroupIdx].numVert + Model.Groups[iGroupIdx].offsetVert;
                    Model.Verts[Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert] = vPoint3D;
                    Model.Vcolors[Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert] = vColor;                    
                    Model.Groups[iGroupIdx].numVert++;
                }

            }

            return iAddVertexResult;
        }

         public static void MoveVertex(ref PModel Model, int iVertIdx, float x, float y, float z)
        {
            Point3D tmpUP3D = new Point3D(x, y, z);

            tmpUP3D = GetUnProjectedCoords(tmpUP3D);

            Model.Verts[iVertIdx].x = tmpUP3D.x;
            Model.Verts[iVertIdx].y = tmpUP3D.y;
            Model.Verts[iVertIdx].z = tmpUP3D.z;
        }

        public static int GetVertexGroup(PModel Model, int iVertIdx)
        {
            int vBase = 0, iNextGroup;

            // Get first 0 group
            iNextGroup = GetNextGroup(Model, -1);
            vBase += Model.Groups[iNextGroup].numVert;

            // Get other groups
            while (vBase <= iVertIdx)
            {
                iNextGroup = GetNextGroup(Model, iNextGroup);

                vBase += Model.Groups[iNextGroup].numVert;
            }

            return iNextGroup;
        }

        // Function to insert a Polygon into a specific Group.
        // Returns the Polygon index
        public static int InsertPPolyIntoGroup(ref PModel Model, PPolygon tmpPPoly, int iGroupIdx)
        {
            int iPolyCounter, iTotalPolyofGroup, iNextGroup, iVertIdx;
            int tmpR = 0, tmpG = 0, tmpB = 0;

            // First increase model Header num Polys and Polys array to put New Polygon
            // We will also do the same for Pcolors array
            Model.Header.numPolys++;
            Array.Resize(ref Model.Polys, Model.Header.numPolys);
            Array.Resize(ref Model.Pcolors, Model.Header.numPolys);

            // Adjust Poly info of the Group
            Model.Groups[iGroupIdx].numPoly++;

            // Insert Poly (and PColor)
            iTotalPolyofGroup = Model.Groups[iGroupIdx].numPoly + Model.Groups[iGroupIdx].offsetPoly;
            for (iPolyCounter = Model.Header.numPolys - 1; iPolyCounter >= iTotalPolyofGroup; iPolyCounter--)
            {
                Model.Polys[iPolyCounter] = Model.Polys[iPolyCounter - 1];
                Model.Pcolors[iPolyCounter] = Model.Pcolors[iPolyCounter - 1];
            }

            Model.Polys[iPolyCounter] = tmpPPoly;
            Model.Polys[iPolyCounter].Verts[0] -= (ushort)Model.Groups[iGroupIdx].offsetVert;
            Model.Polys[iPolyCounter].Verts[1] -= (ushort)Model.Groups[iGroupIdx].offsetVert;
            Model.Polys[iPolyCounter].Verts[2] -= (ushort)Model.Groups[iGroupIdx].offsetVert;


            // We can add also the New Pcolor of the PPoly
            for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
            {
                //  tmpA += Model.Vcolors[vertsIndexBuff[iVertIdx]].A;  -- Commented in KimeraVB6
                tmpR += Model.Vcolors[tmpVNewPoly[iVertIdx]].R;
                tmpG += Model.Vcolors[tmpVNewPoly[iVertIdx]].G;
                tmpB += Model.Vcolors[tmpVNewPoly[iVertIdx]].B;
            }
            Model.Pcolors[iPolyCounter] = Color.FromArgb(255, tmpR / 3, tmpG / 3, tmpB / 3);


            // Now we need to increase the offset of the remaining groups
            // (after this New Poly own group)
            iNextGroup = GetNextGroup(Model, iGroupIdx);

            while (iNextGroup != -1)
            {
                Model.Groups[iNextGroup].offsetPoly++;
                iNextGroup = GetNextGroup(Model, iNextGroup);
            }

            return iPolyCounter;
        }

        // iArrayVNP = VertexNewPoly
        public static int AddPolygon(ref PModel Model, ref ushort[] iArrayVNP, int iGroupIdx)
        {
            //  -------- Warning! Can cause the Normals to be inconsistent if lights are disabled.-----
            //  ---------------------------------Must call ComputeNormals -----------------------------
            int iPolyResult = -1, iVertResult;
            PPolygon tmpPPoly;

            //iGroupIdx = GetVertexGroup(Model, iArrayVNP[0]);

            // Define new Poly
            tmpPPoly = new PPolygon()
            {
                Edges = new ushort[3] { 0, 0, 0 },          // This does not seem to be used in FF7 PC Port
                Normals = new ushort[3] { 0, 0, 0 },        // This will be calculated later with ComputeNormals
                Verts = new ushort[3],                      // We will assign the selected vertices
                tag1 = 0,
                tag2 = PPOLY_TAG2,
            };

            // We will need to add the three vertices of the New Polygon to the
            // Model.Verts array as new vertices (they can have new vertex color).
            // This will result in three new vertex index on the array and we must
            // put them in the vertex array interval of the Group.
            iVertResult = AddVertex(ref Model, iGroupIdx, iArrayVNP[0], Model.Verts[iArrayVNP[0]], Model.Vcolors[iArrayVNP[0]]);

            if (iVertResult != -1)
            {
                tmpPPoly.Verts[0] = (ushort)iVertResult;

                iVertResult = AddVertex(ref Model, iGroupIdx, iArrayVNP[1], Model.Verts[iArrayVNP[1]], Model.Vcolors[iArrayVNP[1]]);

                if (iVertResult != -1)
                {
                    tmpPPoly.Verts[1] = (ushort)iVertResult;

                    iVertResult = AddVertex(ref Model, iGroupIdx, iArrayVNP[2], Model.Verts[iArrayVNP[2]], Model.Vcolors[iArrayVNP[2]]);

                    if (iVertResult != -1)
                    {
                        tmpPPoly.Verts[2] = (ushort)iVertResult;
                    }
                }
            }

            // Check Add Vertex result
            if (iVertResult == -1)
            {
                MessageBox.Show("There has been some issue adding the New Poly vertices into the " +
                                "array of vertices.", "Warning");
            }
            else
            {
                // Add the New Poly to the Polys array of the Group
                // Now we need to put the New Poly in the interval of polys for that Group
                // We will take advantage of this and will add also the Poly color
                iPolyResult = InsertPPolyIntoGroup(ref Model, tmpPPoly, iGroupIdx);

                if (iPolyResult == -1)
                {
                    MessageBox.Show("There has been some issue adding the New Poly into the " +
                                    "array of polys.", "Warning");
                }
            }

            ComputeNormals(ref Model);

            return iPolyResult;
        }

        //public static void OrderVertices(PModel Model, ref int[] vertBuff)
        public static void OrderVertices(Point3D[] inP3DVertexArray, ref ushort[] iPolyIndices)
        {
            Point3D v1, v2, v3;
            ushort iTmpPolyIdx;

            // -- Commented in KimeraVB6
            //glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            //glPushMatrix();
            //glScalef(Model.resizeX, Model.resizeY, Model.resizeZ);
            //glRotatef(Model.rotateAlpha, 1, 0, 0);
            //glRotatef(Model.rotateBeta, 0, 1, 0);
            //glRotatef(Model.rotateGamma, 0, 0, 1);
            //glTranslatef(Model.repositionX, Model.repositionY, Model.repositionZ);

            v1 = GetVertexProjectedCoords(inP3DVertexArray, iPolyIndices[0]);
            v2 = GetVertexProjectedCoords(inP3DVertexArray, iPolyIndices[1]);
            v3 = GetVertexProjectedCoords(inP3DVertexArray, iPolyIndices[2]);

            if (CalculateNormal(v1, v2, v3).z > 0)
            {
                iTmpPolyIdx = iPolyIndices[0];
                iPolyIndices[0] = iPolyIndices[1];
                iPolyIndices[1] = iTmpPolyIdx;

                if (CalculateNormal(v2, v1, v3).z > 0)
                {
                    iTmpPolyIdx = iPolyIndices[1];
                    iPolyIndices[1] = iPolyIndices[2];
                    iPolyIndices[2] = iTmpPolyIdx;

                    if (CalculateNormal(v2, v3, v1).z > 0)
                    {
                        iTmpPolyIdx = iPolyIndices[0];
                        iPolyIndices[0] = iPolyIndices[1];
                        iPolyIndices[1] = iTmpPolyIdx;

                        if (CalculateNormal(v3, v2, v1).z > 0)
                        {
                            iTmpPolyIdx = iPolyIndices[1];
                            iPolyIndices[1] = iPolyIndices[2];
                            iPolyIndices[2] = iTmpPolyIdx;

                            if (CalculateNormal(v3, v1, v2).z > 0)
                            {
                                iTmpPolyIdx = iPolyIndices[0];
                                iPolyIndices[0] = iPolyIndices[1];
                                iPolyIndices[1] = iTmpPolyIdx;
                            }
                        }
                    }
                }
            }

            // -- Commented in KimeraVB6
            //glMatrixMode(GLMatrixModeList.GL_MODELVIEW);
            //glPopMatrix();
        }

        public static bool CutEdgeAtPoint(ref PModel Model, int iPolyIdx, int iEdgeIdx, 
                                          Point3D intersectionPoint, Point2D intersectionTexCoord)
        {
            int iGroupIdx, iVertIdx0, iVertIdx1, iVertIdx2, iVertIdxNew;
            ushort[] vBuff1 = new ushort[3];
            ushort[] vBuff2 = new ushort[3];
            Color tmpColor = new Color();

            iGroupIdx = GetPolygonGroup(Model, iPolyIdx);

            if (Model.Groups[iGroupIdx].HiddenQ) return false;

            iVertIdx0 = Model.Polys[iPolyIdx].Verts[0] + Model.Groups[iGroupIdx].offsetVert;
            iVertIdx1 = Model.Polys[iPolyIdx].Verts[1] + Model.Groups[iGroupIdx].offsetVert;
            iVertIdx2 = Model.Polys[iPolyIdx].Verts[2] + Model.Groups[iGroupIdx].offsetVert;

            switch (iEdgeIdx)
            {
                case 0:
                    //  It makes no sens cutting an edge through one of it's vertices)
                    if (ComparePoints3D(Model.Verts[iVertIdx0], intersectionPoint) ||
                        ComparePoints3D(Model.Verts[iVertIdx1], intersectionPoint))
                        return false;

                    tmpColor = CombineColor(Model.Vcolors[iVertIdx0], Model.Vcolors[iVertIdx1]);
                    break;

                case 1:
                    if (ComparePoints3D(Model.Verts[iVertIdx1], intersectionPoint) ||
                        ComparePoints3D(Model.Verts[iVertIdx2], intersectionPoint))
                        return false;

                    tmpColor = CombineColor(Model.Vcolors[iVertIdx1], Model.Vcolors[iVertIdx2]);
                    break;

                case 2:
                    if (ComparePoints3D(Model.Verts[iVertIdx2], intersectionPoint) ||
                        ComparePoints3D(Model.Verts[iVertIdx0], intersectionPoint))
                        return false;

                    tmpColor = CombineColor(Model.Vcolors[iVertIdx2], Model.Vcolors[iVertIdx0]);
                    break;
            }

            iVertIdxNew = AddVertex(ref Model, iGroupIdx, -999999, intersectionPoint, tmpColor);

            switch (iEdgeIdx)
            {
                case 0:
                    vBuff1[0] = (ushort)iVertIdx0;
                    vBuff1[1] = (ushort)iVertIdxNew;
                    vBuff1[2] = (ushort)iVertIdx2;

                    vBuff2[2] = (ushort)iVertIdx2;
                    vBuff2[1] = (ushort)iVertIdx1;
                    vBuff2[0] = (ushort)iVertIdxNew;
                    break;

                case 1:
                    vBuff1[0] = (ushort)iVertIdx0;
                    vBuff1[1] = (ushort)iVertIdx1;
                    vBuff1[2] = (ushort)iVertIdxNew;

                    vBuff2[2] = (ushort)iVertIdx2;
                    vBuff2[1] = (ushort)iVertIdxNew;
                    vBuff2[0] = (ushort)iVertIdx0;
                    break;

                case 2:
                    vBuff1[0] = (ushort)iVertIdx0;
                    vBuff1[1] = (ushort)iVertIdx1;
                    vBuff1[2] = (ushort)iVertIdxNew;

                    vBuff2[2] = (ushort)iVertIdx2;
                    vBuff2[1] = (ushort)iVertIdx1;
                    vBuff2[0] = (ushort)iVertIdxNew;
                    break;
            }

            RemovePolygon(ref Model, iPolyIdx);
            AddPolygon(ref Model, ref vBuff1, iGroupIdx);
            AddPolygon(ref Model, ref vBuff2, iGroupIdx);

            if (Model.Groups[iGroupIdx].texFlag == 1)
            {
                Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + iVertIdxNew - Model.Groups[iGroupIdx].offsetVert].x = 
                    intersectionTexCoord.x;

                Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + iVertIdxNew - Model.Groups[iGroupIdx].offsetVert].y = 
                    intersectionTexCoord.y;
            }

            return true;
        }
        
        //  Find the first edge between v1 and v2 (poly and edge id)
        public static bool FindNextAdjacentPolyEdge(PModel Model, Point3D v1Point3D, Point3D v2Point3D,
                                                    ref int iFromPolyIdx, ref int iEdgeIdx)
        {
            int iPolyIdx, iGroupIdx, offsetVert;
            bool bfoundQ = false;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    offsetVert = Model.Groups[iGroupIdx].offsetVert;

                    iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;

                    while (iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly)
                    {
                        if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v1Point3D) &&
                             ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v2Point3D)) ||
                            (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v2Point3D) &&
                             ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v1Point3D)))
                        {
                            iFromPolyIdx = iPolyIdx;
                            iEdgeIdx = 0;
                            bfoundQ = true;
                        }
                        else
                        {
                            if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v1Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v2Point3D)) ||
                                (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v2Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v1Point3D)))
                            {
                                iFromPolyIdx = iPolyIdx;
                                iEdgeIdx = 1;
                                bfoundQ = true;
                            }
                            else
                            {
                                if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v1Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v2Point3D)) ||
                                    (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v2Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v1Point3D)))
                                {
                                    iFromPolyIdx = iPolyIdx;
                                    iEdgeIdx = 2;
                                    bfoundQ = true;
                                }
                            }
                        }

                        iPolyIdx++;
                    }
                }
            }

            return bfoundQ;
        }

        //  This version of the function find the next matching edge after the one given as parameter
        public static bool FindNextAdjacentPolyEdgeForward(PModel Model, Point3D v1Point3D, Point3D v2Point3D, 
                                                           ref int iGroupIdx, ref int iFromPolyIdx, ref int iEdgeIdx)
        {
            int iPolyIdx, offsetVert;
            bool bPolyFound = false;

            iPolyIdx = iFromPolyIdx + 1;

            for (; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    offsetVert = Model.Groups[iGroupIdx].offsetVert;

                    while (iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly && !bPolyFound)
                    {
                        if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v1Point3D) &&
                            ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v2Point3D)) ||
                            (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v2Point3D) &&
                            ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v1Point3D)))
                        {
                            iFromPolyIdx = iPolyIdx;
                            iEdgeIdx = 0;
                            bPolyFound = true;
                        }
                        else
                        {
                            if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v1Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v2Point3D)) ||
                                (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[1]], v2Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v1Point3D)))
                            {
                                iFromPolyIdx = iPolyIdx;
                                iEdgeIdx = 1;
                                bPolyFound = true;
                            }
                            else
                            {
                                if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v1Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v2Point3D)) ||
                                    (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[2]], v2Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[iPolyIdx].Verts[0]], v1Point3D)))
                                {
                                    iFromPolyIdx = iPolyIdx;
                                    iEdgeIdx = 2;
                                    bPolyFound = true;
                                }
                            }
                        }

                        iPolyIdx++;
                    }                       
                }

                // Exit for if Poly found
                if (bPolyFound) break;
            }

            return bPolyFound;
        }

        public static int GetEqualVertices(PModel Model, int iMatchVertIdx, ref List<int> lstVerts)
        {
            int iVertIdx, iGroupIdx;
            Point3D tmpVPoint3D;
            HashSet<int> lstVertsUnique = new HashSet<int>();

            tmpVPoint3D = Model.Verts[iMatchVertIdx];
            lstVerts.Clear();

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
                    {
                        if (ComparePoints3D(Model.Verts[iVertIdx], tmpVPoint3D))
                        {
                            lstVertsUnique.Add(iVertIdx);
                        }
                    }
                }
            }

            lstVerts = lstVertsUnique.ToList();

            return lstVerts.Count;
        }

        public static bool CutPolygonThroughPlane(ref PModel Model, int iInputPolyIdx, int iGroupIdx,
                                                  float pA, float pB, float pC, float pD,
                                                  ref List<Point3D> knownPlaneVPoints)
        {
            int iPolyIdx, iEdgeIdx, offsetTex, offsetVert, numKnownPlaneVPoints;
            Point3D up3DPolyNormal;
            bool isPararlelQ, equalityValidQ, cutQ;
            float fEquality, fLambdaMultPlane, kPlane, alphaPlane;
            int p1Idx, p2Idx, t1Idx, t2Idx, iOldGroupIdx;
            Point3D intersectionPoint;
            Point2D intersectionTexCoord = new Point2D();
            int vIndexRectify;
            List<int> vEqualVIndices = new List<int>();
            bool p1IsContainedQ, p2IsContainedQ;

            iOldGroupIdx = iGroupIdx;
            cutQ = false;

            offsetVert = Model.Groups[iGroupIdx].offsetVert;
            offsetTex = Model.Groups[iGroupIdx].offsetTex;

            up3DPolyNormal = CalculateNormal(Model.Verts[Model.Polys[iInputPolyIdx].Verts[0] + offsetVert],
                                             Model.Verts[Model.Polys[iInputPolyIdx].Verts[1] + offsetVert],
                                             Model.Verts[Model.Polys[iInputPolyIdx].Verts[2] + offsetVert]);
            up3DPolyNormal = Normalize(up3DPolyNormal);

            numKnownPlaneVPoints = knownPlaneVPoints.Count;

            //  Check wether the planes are paralel or not.
            //  If they are, don't cut the polygon.
            isPararlelQ = true;
            equalityValidQ = false;
            fEquality = 0;

            if (up3DPolyNormal.x == 0 || pA == 0)
            {
                isPararlelQ = Math.Abs(up3DPolyNormal.x - pA) < 0.0001f;
            }
            else
            {
                equalityValidQ = true;
                fEquality = pA / up3DPolyNormal.x;
            }

            if (up3DPolyNormal.y == 0 || pB == 0)
            {
                isPararlelQ = isPararlelQ && (Math.Abs(up3DPolyNormal.y - pB) < 0.0001f);
            }
            else
            {
                if (equalityValidQ)
                {
                    isPararlelQ = isPararlelQ && (Math.Abs((pB / up3DPolyNormal.y) - fEquality) < 0.0001f);
                }
                else
                {
                    equalityValidQ = true;
                    fEquality = pB / up3DPolyNormal.y;
                }
            }

            if (up3DPolyNormal.z == 0 || pC == 0)
            {
                isPararlelQ = isPararlelQ && (up3DPolyNormal.z == pC);
            }
            else
            {
                if (equalityValidQ)
                {
                    isPararlelQ = isPararlelQ && (Math.Abs((pC / up3DPolyNormal.z) - fEquality) < 0.0001f);
                }
            }

            if (!isPararlelQ)
            {

                iEdgeIdx = 0;
                cutQ = false;

                do
                {
                    p1Idx = Model.Polys[iInputPolyIdx].Verts[iEdgeIdx] + offsetVert;
                    p2Idx = Model.Polys[iInputPolyIdx].Verts[(iEdgeIdx + 1) % 3] + offsetVert;

                    t1Idx = Model.Polys[iInputPolyIdx].Verts[iEdgeIdx] + offsetTex;
                    t2Idx = Model.Polys[iInputPolyIdx].Verts[(iEdgeIdx + 1) % 3] + offsetTex;

                    //  Degenerated triangle, don't bother
                    if (ComparePoints3D(Model.Verts[p2Idx], Model.Verts[p1Idx])) return false;

                    //  Check if the edge is contained on the plane
                    p1IsContainedQ = false;
                    p2IsContainedQ = false;

                    for (iPolyIdx = 0; iPolyIdx < knownPlaneVPoints.Count; iPolyIdx++)
                    {
                        if (ComparePoints3D(Model.Verts[p1Idx], knownPlaneVPoints[iPolyIdx]))
                            p1IsContainedQ = true;

                        if (ComparePoints3D(Model.Verts[p2Idx], knownPlaneVPoints[iPolyIdx]))
                            p2IsContainedQ = true;

                        if (p1IsContainedQ && p2IsContainedQ) return false;
                    }

                    //  If they aren't, find the cut point.
                    fLambdaMultPlane = -pA * Model.Verts[p1Idx].x - pB * Model.Verts[p1Idx].y - pC * Model.Verts[p1Idx].z;
                    kPlane = fLambdaMultPlane - pD;

                    fLambdaMultPlane = fLambdaMultPlane + pA * Model.Verts[p2Idx].x + pB * Model.Verts[p2Idx].y + pC * Model.Verts[p2Idx].z;

                    if (Math.Abs(fLambdaMultPlane) > 0.0000001f && kPlane != 0)
                    {
                        alphaPlane = kPlane / fLambdaMultPlane;
                        intersectionPoint = GetPointInLine(Model.Verts[p1Idx], Model.Verts[p2Idx], alphaPlane);

                        if (Model.Groups[iGroupIdx].texFlag == 1)
                            intersectionTexCoord = GetPointInLine2D(Model.TexCoords[t1Idx], Model.TexCoords[t2Idx], alphaPlane);

                        //  Finally check if cut point is actually inside the edge segment.
                        if (alphaPlane > 0.2f && alphaPlane < 0.8f)
                        {
                            cutQ = CutEdgeAtPoint(ref Model, iInputPolyIdx, iEdgeIdx, intersectionPoint, intersectionTexCoord);
                            CheckModelConsistency(ref Model);
                            iGroupIdx = GetPolygonGroup(Model, iInputPolyIdx);

                            while (FindNextAdjacentPolyEdgeForward(EditedPModel, Model.Verts[p1Idx], Model.Verts[p2Idx],
                                                                   ref iGroupIdx, ref iInputPolyIdx, ref iEdgeIdx))
                            {
                                //  Must recompute the texture junction point everytime we go beyond a textured
                                //  group boundaries.
                                if (iOldGroupIdx != iGroupIdx)
                                {
                                    if (Model.Groups[iGroupIdx].texFlag == 1)
                                    {
                                        offsetTex = Model.Groups[iGroupIdx].offsetTex;
                                        t1Idx = Model.Polys[iInputPolyIdx].Verts[iEdgeIdx] + offsetTex;
                                        t2Idx = Model.Polys[iInputPolyIdx].Verts[(iEdgeIdx + 1) % 3] + offsetTex;

                                        intersectionTexCoord = 
                                            GetPointInLine2D(Model.TexCoords[t1Idx], Model.TexCoords[t2Idx], alphaPlane);
                                    }

                                    iOldGroupIdx = iGroupIdx;
                                }

                                cutQ = CutEdgeAtPoint(ref EditedPModel, iInputPolyIdx, iEdgeIdx, 
                                                      intersectionPoint, intersectionTexCoord);
                            }

                            //  Add the new point to the known plane points list
                            //knownPlaneVPoints = new List<Point3D>();
                            //for (int i = 0; i < numKnownPlaneVPoints; i++) knownPlaneVPoints.Add(new Point3D());
                            knownPlaneVPoints.Add(intersectionPoint);
                            //  Just one cut per polygon. After cutting an edge, exit the loop.
                        }
                        else
                        {
                            //  If it's close enough, change the vertex location so that it's contained on the plane
                            if (alphaPlane <= 0.2f && alphaPlane >= 0) vIndexRectify = p1Idx;
                            else if (alphaPlane >= 0.8f && alphaPlane <= 1f) vIndexRectify = p2Idx;
                            else vIndexRectify = -1;

                            if (vIndexRectify != -1)
                            {
                                //  Add the rectified point to the known plane points list
                                //knownPlaneVPoints = new List<Point3D>();
                                //for (int i = 0; i < numKnownPlaneVPoints; i++) knownPlaneVPoints.Add(new Point3D());
                                knownPlaneVPoints.Add(intersectionPoint);
                                numKnownPlaneVPoints++;

                                GetEqualVertices(Model, vIndexRectify, ref vEqualVIndices);

                                //  Propagate changes to all equal vertices
                                foreach (int itmVIdx in vEqualVIndices)
                                    Model.Verts[itmVIdx] = intersectionPoint;

                                // -- Removed from KimeraVB6
                                //  cutQ = True
                                //  break;
                            }
                        }
                    }

                    iEdgeIdx++;

                } while (!cutQ && iEdgeIdx < 3);
            }
 
            return cutQ;
        }

        public static void CutPModelThroughPlane(ref PModel Model, 
                                                 float pA, float pB, float pC, float pD,
                                                 ref List<Point3D> knownPlaneVPoints)
        {
            int iGroupIdx, iPolyIdx;
            int offsetPoly;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                // We will do the cut only to the visible groups
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    offsetPoly = Model.Groups[iGroupIdx].offsetPoly;
                    iPolyIdx = offsetPoly;

                    while (iPolyIdx < offsetPoly + Model.Groups[iGroupIdx].numPoly)
                    {
                        if (!CutPolygonThroughPlane(ref Model, iPolyIdx, iGroupIdx, pA, pB, pC, pD, ref knownPlaneVPoints))
                        {
                            CheckModelConsistency(ref Model);
                            iPolyIdx++;
                        }
                    }
                }
            }
        }

        public static Point3D GetPoint3DOrthogonalProjection(Point3D uPoint3D, float pA, float pB, float pC, float pD)
        {
            float alpha;

            alpha = (-pA * uPoint3D.x - pB * uPoint3D.y - pC * uPoint3D.z - pD) / (pA * pA + pB + pB + pC * pC);

            return new Point3D(uPoint3D.x + alpha * pA,
                               uPoint3D.y + alpha * pB,
                               uPoint3D.z + alpha * pC);
        }

        public static bool IsPoint3DUnderPlane(Point3D uPoint3D, float pA, float pB, float pC, float pD)
        {
            Point3D orthogonalProjectionP3D;
            Point3D vectP3D;
            Point3D vectNormP3D;

            orthogonalProjectionP3D = GetPoint3DOrthogonalProjection(uPoint3D, pA, pB, pC, pD);

            vectP3D = new Point3D(orthogonalProjectionP3D.x - uPoint3D.x,
                                  orthogonalProjectionP3D.y - uPoint3D.y,
                                  orthogonalProjectionP3D.z - uPoint3D.z);

            vectNormP3D = Normalize(vectP3D);

            return !(Math.Abs(pA - vectNormP3D.x) < 0.0001f &&
                     Math.Abs(pB - vectNormP3D.y) < 0.0001f &&
                     Math.Abs(pC - vectNormP3D.z) < 0.0001f &&
                     !(CalculateDistance(uPoint3D, orthogonalProjectionP3D) < 0.0001f));
        }

        public static bool IsPoint3DAbovePlane(Point3D uPoint3D, float pA, float pB, float pC, float pD)
        {
            Point3D orthogonalProjectionP3D;
            Point3D vectP3D;
            Point3D vectNormP3D;

            orthogonalProjectionP3D = GetPoint3DOrthogonalProjection(uPoint3D, pA, pB, pC, pD);

            vectP3D = new Point3D(orthogonalProjectionP3D.x - uPoint3D.x,
                                  orthogonalProjectionP3D.y - uPoint3D.y,
                                  orthogonalProjectionP3D.z - uPoint3D.z);

            vectNormP3D = Normalize(vectP3D);

            return  (Math.Abs(pA - vectNormP3D.x) < 0.0001f &&
                     Math.Abs(pB - vectNormP3D.y) < 0.0001f &&
                     Math.Abs(pC - vectNormP3D.z) < 0.0001f &&
                     !(CalculateDistance(uPoint3D, orthogonalProjectionP3D) < 0.0001f));

        }

        public static void EraseHemisphereVertices(ref PModel Model, float pA, float pB, float pC, float pD,
                                                   bool underPlaneQ, ref List<Point3D> knownPlaneVPoints)
        {
            int iGroupIdx, iPolyIdx, iVertIdx, offsetVert, offsetPoly, iActualVertIdx, kppi;
            bool atLeastOneSparedQ, foundQ;

            for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    offsetVert = Model.Groups[iGroupIdx].offsetVert;
                    offsetPoly = Model.Groups[iGroupIdx].offsetPoly;

                    iPolyIdx = offsetPoly;

                    while (iPolyIdx < (offsetPoly + Model.Groups[iPolyIdx].numPoly) && Model.Header.numPolys > 1)
                    {
                        atLeastOneSparedQ = false;

                        for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                        {
                            foundQ = false;
                            iActualVertIdx = Model.Polys[iPolyIdx].Verts[iVertIdx] + offsetVert;

                            for (kppi = 0; kppi < knownPlaneVPoints.Count; kppi++)
                            {
                                if (ComparePoints3D(Model.Verts[iActualVertIdx], knownPlaneVPoints[kppi]))
                                {
                                    foundQ = true;
                                    break;
                                }
                            }

                            if (!foundQ)
                            {
                                if (underPlaneQ)
                                {
                                    if (IsPoint3DUnderPlane(Model.Verts[iActualVertIdx], pA, pB, pC, pD))
                                        atLeastOneSparedQ = true;
                                }
                                else
                                {
                                    if (IsPoint3DAbovePlane(Model.Verts[iActualVertIdx], pA, pB, pC, pD))
                                        atLeastOneSparedQ = true;
                                }
                            }
                        }

                        if (!atLeastOneSparedQ) RemovePolygon(ref Model, iPolyIdx);
                        else iPolyIdx++;
                    }
                }
            }

            if (Model.Header.numPolys == 1)
            {
                MessageBox.Show("A .P Model must have at least one polygon. The last triangle was spared.",
                                "Information", MessageBoxButtons.OK);
            }

            KillUnusedVertices(ref Model);

            RepairGroups(ref Model);

            ComputeNormals(ref Model);
            ComputeEdges(ref Model);
        }

        public static void ApplyPModelTransformation(ref PModel Model, double[] transMatrix)
        {
            Point3D tmpPoint3D = new Point3D();
            int iVertIdx;

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {
                MultiplyPoint3DByOGLMatrix(transMatrix, Model.Verts[iVertIdx], ref tmpPoint3D);
                Model.Verts[iVertIdx] = tmpPoint3D;
            }
        }

        public static void FattenPModel(ref PModel Model)
        {
            int iVertIdx;
            float fCentralZ, fMaxDiff, fMinDiff, fFactor;

            fCentralZ = 0;
            fMaxDiff = Math.Abs(Model.BoundingBox.max_z - fCentralZ);
            fMinDiff = Math.Abs(fCentralZ - Model.BoundingBox.min_z);

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {
                if (Model.Verts[iVertIdx].z > fCentralZ)
                {
                    if (fMaxDiff == 0) fFactor = 1;
                    else fFactor = (float)(1 + (1 - Math.Abs(Model.Verts[iVertIdx].z - fCentralZ) / fMaxDiff) * 0.1);
                }
                else
                {
                    if (fMinDiff == 0) fFactor = 1;
                    else fFactor = (float)(1 + (1 - Math.Abs(fCentralZ - Model.Verts[iVertIdx].z) / fMinDiff) * 0.1);
                }

                Model.Verts[iVertIdx].x *= fFactor;
                Model.Verts[iVertIdx].y *= fFactor;
            }
        }

        public static void SlimPModel(ref PModel Model)
        {
            int iVertIdx;
            float fCentralZ, fMaxDiff, fMinDiff, fFactor;

            fCentralZ = 0;
            fMaxDiff = Math.Abs(Model.BoundingBox.max_z - fCentralZ);
            fMinDiff = Math.Abs(fCentralZ - Model.BoundingBox.min_z);

            for (iVertIdx = 0; iVertIdx < Model.Header.numVerts; iVertIdx++)
            {
                if (Model.Verts[iVertIdx].z > fCentralZ)
                {
                    if (fMaxDiff == 0) fFactor = 1;
                    else fFactor = (float)(1 + (1 - Math.Abs(Model.Verts[iVertIdx].z - fCentralZ) / fMaxDiff) * 0.1);
                }
                else
                {
                    if (fMinDiff == 0) fFactor = 1;
                    else fFactor = (float)(1 + (1 - Math.Abs(fCentralZ - Model.Verts[iVertIdx].z) / fMinDiff) * 0.1);
                }

                Model.Verts[iVertIdx].x /= fFactor;
                Model.Verts[iVertIdx].y /= fFactor;
            }
        }

        public static int GetAdjacentPolygonsVertices(PModel Model, List<int> lstVerts, ref int[] lstPolysBuffer)
        {
            int iGroupIdx, iPolyIdx, iVertIdx, iNumPolys;

            iNumPolys = 0;
            lstPolysBuffer = new int[0];

            foreach (int itmVert in lstVerts)
            {
                iGroupIdx = GetVertexGroup(Model, itmVert);

                for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly; 
                     iPolyIdx < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly;
                     iPolyIdx++)
                {
                    for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                    {
                        if (Model.Polys[iPolyIdx].Verts[iVertIdx] == itmVert - Model.Groups[iGroupIdx].offsetVert)
                        {
                            Array.Resize(ref lstPolysBuffer, iNumPolys);
                            lstPolysBuffer[iNumPolys] = iPolyIdx;
                            iNumPolys++;
                            break;
                        }
                    }
                }
            }

            return iNumPolys;
        }

        public static float GetVertexProjectedDepth(ref Point3D[] lstVerts, int iVertIdx)
        {
            glClear(GLBufferMask.GL_DEPTH_BUFFER_BIT);
            return (float)GetDepthZ(lstVerts[iVertIdx]);
        }

        public static int GetPolygonAdjacentVertexIndexes(PModel Model, List<int> lstVertexPolys, 
                                                          ref int[] lstIndexesDiscarded, 
                                                          ref STIntVector[] lstAdjacentIndexesOut)
        {
            // nPolys is lstVertexPolys.Count
            int iGetPolygonAdjacentVertexIndexesResult = 0;
            int iGroupIdx, iPolyIdx, iVertIdx, iVertIdxDiscard, nDiscarded, offsetVert; // nEqualVerts, 
            List<int> lstEqualVerts = new List<int>();
            bool foundQ;

            nDiscarded = lstIndexesDiscarded.Length;
            
            for (iPolyIdx = 0; iPolyIdx < lstVertexPolys.Count; iPolyIdx++)
            {
                iGroupIdx = GetPolygonGroup(Model, lstVertexPolys[iPolyIdx]);
                offsetVert = Model.Groups[iGroupIdx].offsetVert;

                for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
                {
                    //  Check whether the vertex should be ignored or not
                    foundQ = false;

                    for (iVertIdxDiscard = 0; iVertIdxDiscard < nDiscarded; iVertIdxDiscard++)
                    {
                        if (lstIndexesDiscarded[iVertIdxDiscard] - offsetVert == 
                                Model.Polys[lstVertexPolys[iPolyIdx]].Verts[iVertIdx])
                        {
                            foundQ = true;
                            break;
                        }
                    }

                    if (!foundQ)
                    {
                        //  Check if the vertex (or similar) is already added to the list
                        for (iVertIdxDiscard = 0; 
                             iVertIdxDiscard < iGetPolygonAdjacentVertexIndexesResult; 
                             iVertIdxDiscard++)
                        {
                            if (ComparePoints3D(Model.Verts[lstAdjacentIndexesOut[iVertIdxDiscard].vector[0]],
                                                Model.Verts[Model.Polys[lstVertexPolys[iPolyIdx]].Verts[iVertIdx] + offsetVert]))
                            {
                                foundQ = true;
                                break;
                            }
                        }

                        if (!foundQ)
                        {
                            lstEqualVerts.Clear();

                            //  Find all similar vertices
                            GetEqualVertices(Model, Model.Polys[lstVertexPolys[iPolyIdx]].Verts[iVertIdx], ref lstEqualVerts);

                            //  Update the output data
                            iGetPolygonAdjacentVertexIndexesResult++;
                            Array.Resize(ref lstAdjacentIndexesOut, iGetPolygonAdjacentVertexIndexesResult);
                            lstAdjacentIndexesOut[iGetPolygonAdjacentVertexIndexesResult - 1].vector = lstEqualVerts.ToArray();

                            // -- Commented in KimeraVB6
                            //Model.Vcolors[lstAdjacentIndexesOut[iGetPolygonAdjacentVertexIndexesResult - 1].vector[0].R = 255;
                            //Model.Vcolors[lstAdjacentIndexesOut[iGetPolygonAdjacentVertexIndexesResult - 1].vector[0].G = 0;
                            //Model.Vcolors[lstAdjacentIndexesOut[iGetPolygonAdjacentVertexIndexesResult - 1].vector[0].B = 0;
                        }
                    }
                }
            }

            return iGetPolygonAdjacentVertexIndexesResult;
        }

        public static void GetAllNormalDependentPolys(PModel Model, List<int> lstVerts, ref int[] lstAdjacentPolysIdxs,
                                                      ref STIntVector[] lstAdjacentVertsIdxs, 
                                                      ref STIntVector[] lstAdjacentAdjacentPolysIdxs)
        {
            int iVertIdx, iNumPolys, nAdjacentVerts;

            //  Get the polygons adjacent to the selected vertices
            GetAdjacentPolygonsVertices(Model, lstVerts.ToList(), ref lstAdjacentPolysIdxs);

            //  Get the vertices adjacent to the selected vertices
            nAdjacentVerts = GetPolygonAdjacentVertexIndexes(Model, lstVerts.ToList(), ref lstAdjacentPolysIdxs, ref lstAdjacentVertsIdxs);

            //  Get polygons adjacent to the adjacent
            Array.Resize(ref lstAdjacentAdjacentPolysIdxs, nAdjacentVerts);

            for (iVertIdx = 0; iVertIdx < nAdjacentVerts; iVertIdx++)
            {
                iNumPolys = GetAdjacentPolygonsVertices(Model, 
                                                        lstAdjacentVertsIdxs[iVertIdx].vector.ToList(),
                                                        ref lstAdjacentAdjacentPolysIdxs[iVertIdx].vector);
                lstAdjacentAdjacentPolysIdxs[iVertIdx].length = iNumPolys;
            }
        }


    }
}


//public struct STVertexIndexData
//{
//    public int ActualVertex;
//    public int GroupIdx;
//    public int PolyIdx;
//    public int VertexIdx;
//}

//public struct STVertexNormals
//{
//    public int iVertexNormalsCounter;
//    public Point3D p3DVertex;
//    public Point3D p3DNormal;
//    public List<STVertexIndexData> lstVertexIndexData;
//}

//public static List<STVertexNormals> stVertexNormals;

//public static void GenerateNormalsList(PModel Model)
//{
//    int iGroupIdx, iPolyIdx, iVertIdx, iActualVertex, iVertexNormalsIdx;
//    STVertexNormals stTmpVertexNormals;
//    STVertexIndexData stTmpVertexIndexData;
//    Point3D p3DNormal;

//    stVertexNormals = new List<STVertexNormals>();

//    for (iGroupIdx = 0; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
//    {
//        for (iPolyIdx = Model.Groups[iGroupIdx].offsetPoly;
//             iPolyIdx < Model.Groups[iGroupIdx].numPoly + Model.Groups[iGroupIdx].offsetPoly;
//             iPolyIdx++)
//        {
//            p3DNormal = CalculateNormal(Model.Verts[Model.Polys[iPolyIdx].Verts[0] +
//                                                    Model.Groups[iGroupIdx].offsetVert],
//                                        Model.Verts[Model.Polys[iPolyIdx].Verts[1] +
//                                                    Model.Groups[iGroupIdx].offsetVert],
//                                        Model.Verts[Model.Polys[iPolyIdx].Verts[2] +
//                                                    Model.Groups[iGroupIdx].offsetVert]);

//            for (iVertIdx = 0; iVertIdx < 3; iVertIdx++)
//            {
//                iActualVertex = Model.Polys[iPolyIdx].Verts[iVertIdx] +
//                                Model.Groups[iGroupIdx].offsetVert;

//                iVertexNormalsIdx = stVertexNormals.
//                        FindIndex(x => x.p3DVertex.x == Model.Verts[iActualVertex].x &&
//                                       x.p3DVertex.y == Model.Verts[iActualVertex].y &&
//                                       x.p3DVertex.z == Model.Verts[iActualVertex].z);

//                if (iVertexNormalsIdx == -1)
//                {
//                    stTmpVertexNormals = new STVertexNormals();


//                    stTmpVertexNormals.p3DVertex = Model.Verts[iActualVertex];
//                    stTmpVertexNormals.p3DNormal = new Point3D(p3DNormal.x, p3DNormal.y, p3DNormal.z);

//                    stTmpVertexNormals.lstVertexIndexData = new List<STVertexIndexData>();

//                    stTmpVertexIndexData = new STVertexIndexData()
//                    {
//                        ActualVertex = iActualVertex,
//                        GroupIdx = iGroupIdx,
//                        PolyIdx = iPolyIdx,
//                        VertexIdx = iVertIdx,
//                    };

//                    stTmpVertexNormals.lstVertexIndexData.Add(stTmpVertexIndexData);


//                    stVertexNormals.Add(stTmpVertexNormals);
//                }
//                else
//                {
//                    stTmpVertexNormals = stVertexNormals[iVertexNormalsIdx];


//                    stTmpVertexNormals.p3DNormal = AddPoint3D(stTmpVertexNormals.p3DNormal, p3DNormal);

//                    stTmpVertexIndexData = new STVertexIndexData()
//                    {
//                        ActualVertex = iActualVertex,
//                        GroupIdx = iGroupIdx,
//                        PolyIdx = iPolyIdx,
//                        VertexIdx = iVertIdx,
//                    };

//                    stTmpVertexNormals.lstVertexIndexData.Add(stTmpVertexIndexData);


//                    stVertexNormals[iVertexNormalsIdx] = stTmpVertexNormals;
//                }
//            }
//        }
//    }

//}

//public static void ComputeNormals(ref PModel Model)
//{
//    int iGroupIdx, iPolyIdx, iVertIdx, iActualVertIdx, iVertIdxNext;

//    Point3D p3DTempNorm;

//    Point3D[] sumNorms = new Point3D[Model.Header.numVerts];
//    int[] polys_per_vert = new int[Model.Header.numVerts];

//    Model.Normals = new Point3D[Model.Header.numVerts];
//    Model.NormalIndex = new int[Model.Header.numVerts];

//    Model.Header.numNormals = Model.Header.numVerts;
//    Model.Header.numNormIdx = Model.Header.numVerts;

//    GenerateNormalsList(Model);

//    foreach (STVertexNormals itmVertexNormal in stVertexNormals)
//    {
//        foreach (STVertexIndexData itmVertexIndexData in itmVertexNormal.lstVertexIndexData)
//        {
//            Model.Polys[itmVertexIndexData.PolyIdx].Normals[itmVertexIndexData.VertexIdx] =
//                                    (ushort)itmVertexIndexData.ActualVertex;

//            Model.NormalIndex[itmVertexIndexData.ActualVertex] = itmVertexIndexData.ActualVertex;

//            Model.Normals[itmVertexIndexData.ActualVertex].x =
//                -itmVertexNormal.p3DNormal.x / itmVertexNormal.lstVertexIndexData.Count;
//            Model.Normals[itmVertexIndexData.ActualVertex].y =
//                -itmVertexNormal.p3DNormal.y / itmVertexNormal.lstVertexIndexData.Count;
//            Model.Normals[itmVertexIndexData.ActualVertex].z =
//                -itmVertexNormal.p3DNormal.z / itmVertexNormal.lstVertexIndexData.Count;

//            Model.Normals[itmVertexIndexData.ActualVertex] =
//                                Normalize(Model.Normals[itmVertexIndexData.ActualVertex]);

//        }

//    }

//}