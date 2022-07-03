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

    using static frmPEditor;

    using static FF7Skeleton;

    using static ModelDrawing;

    using static Utils;
    using static OpenGL32;

    public class FF7PModel
    {

        public struct PHeader
        {
            public int off00;
            public int off04;
            public int vertexColor;
            public int numVerts;
            public int numNormals;
            public int off14;
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
            public short[] Verts;

            // This is for create a new deep copy of PEdge
            // We will use normally the creator like '= new PEdge();' but there are some exceptions
            public PEdge(PEdge pedgeIn)
            {
                Verts = new short[pedgeIn.Verts.Length];
                pedgeIn.Verts.CopyTo(Verts, 0);
            }
        }

        public struct PPolygon
        {
            public short tag1;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] Verts;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] Normals;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
            public short[] Edges;
            public int tag2;

            // This is for create a new empty PPolygon with arrays defined
            public PPolygon(int tag2In)
            {
                tag1 = 0;
                tag2 = tag2In;

                Verts = new short[3];
                Normals = new short[3];
                Edges = new short[3];
            }

            // This is for create a new deep copy of PPolygon
            // We will use normally the creator like '= new PPolygon();' but there are some exceptions
            public PPolygon(PPolygon ppolygonIn)
            {
                tag1 = ppolygonIn.tag1;
                tag2 = ppolygonIn.tag2;

                Verts = new short[ppolygonIn.Verts.Length];
                ppolygonIn.Verts.CopyTo(Verts, 0);

                Normals = new short[ppolygonIn.Normals.Length];
                ppolygonIn.Normals.CopyTo(Normals, 0);

                Edges = new short[ppolygonIn.Edges.Length];
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
            public int polyType;
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
            public int DListNum;
            public bool HiddenQ;           // Hidden groups aren't rendered and can't be changed _
                                           // save for the basic geometrical transformations(rotation, scaling and panning),
                                           // palletizzed opeartions and group deletion
            public int realGID;            // We will use this as maintain the real Group position number of the list for
                                           // Remove Group feature.
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
            public float resizeX, resizeY, resizeZ;
            public float rotateAlpha, rotateBeta, rotateGamma;
            public Quaternion rotationQuaternion;
            public float repositionX, repositionY, repositionZ;
            public float diameter;
            public int DListNum;
        }

        public static void LoadPModel(ref PModel Model, string strPFolder, string strPFileName)
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
            Model.fileName = strPFileName;

            // Header
            Model.Header = new PHeader();
            ReadPHeader(fileBuffer, ref fileBufferPos, ref Model.Header, strPFullFileName);

            // Check numVerts
            if (Model.Header.numVerts <= 0)
            {
                throw new ApplicationException("The num Vertices value of the P file " + Model.fileName + " is not correct.");
            }

            // Verts
            Model.Verts = new Point3D[Model.Header.numVerts];
            ReadPVerts(fileBuffer, ref fileBufferPos, Model.Header.numVerts, ref Model.Verts, strPFullFileName);

            // Normals
            Model.Normals = new Point3D[Model.Header.numNormals];
            ReadPNormals(fileBuffer, ref fileBufferPos, Model.Header.numNormals, ref Model.Normals, strPFullFileName);

            // Texture Coordinates
            Model.TexCoords = new Point2D[Model.Header.numTexCs];
            ReadPTexCoords(fileBuffer, ref fileBufferPos, Model.Header.numTexCs, ref Model.TexCoords, strPFullFileName);

            //  Vertex Colors
            Model.Vcolors = new Color[Model.Header.numVerts];
            ReadPPVColors(fileBuffer, ref fileBufferPos, Model.Header.numVerts, ref Model.Vcolors, strPFullFileName);

            //  Polygon Colors
            Model.Pcolors = new Color[Model.Header.numPolys];
            ReadPPVColors(fileBuffer, ref fileBufferPos, Model.Header.numPolys, ref Model.Pcolors, strPFullFileName);

            // Edges
            Model.Edges = new PEdge[Model.Header.numEdges];
            ReadPEdges(fileBuffer, ref fileBufferPos, Model.Header.numEdges, ref Model.Edges, strPFullFileName);

            // Polygons
            Model.Polys = new PPolygon[Model.Header.numPolys];
            ReadPPolygons(fileBuffer, ref fileBufferPos, Model.Header.numPolys, ref Model.Polys, strPFullFileName);

            // Hundrets
            Model.Hundrets = new PHundret[Model.Header.mirex_h];
            ReadPHundrets(fileBuffer, ref fileBufferPos, Model.Header.mirex_h, ref Model.Hundrets, strPFullFileName);

            // Groups
            Model.Groups = new PGroup[Model.Header.numGroups];
            ReadPGroups(fileBuffer, ref fileBufferPos, Model.Header.numGroups, ref Model.Groups, strPFullFileName);

            // BoundingBox
            Model.BoundingBox = new PBoundingBox();
            ReadPBoundingBox(fileBuffer, ref fileBufferPos, ref Model.BoundingBox, strPFullFileName);

            // NormalIndex
            Model.NormalIndex = new int[Model.Header.numNormIdx];
            ReadPNormalIndex(fileBuffer, ref fileBufferPos, Model.Header.numNormIdx, ref Model.NormalIndex, strPFullFileName);

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

            CheckModelConsistency(ref Model);
            KillUnusedVertices(ref Model);
            ComputeBoundingBox(ref Model);
            ComputeNormals(ref Model);
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
                    Header.off00 = memReader.ReadInt32();
                    Header.off04 = memReader.ReadInt32();

                    if (Header.off00 != 1 || Header.off04 != 1)
                    {
                        MessageBox.Show("The file header of the P file " + fileName + " is not correct.",
                                        "Error");
                        return 0;
                    }

                    Header.vertexColor = memReader.ReadInt32();
                    Header.numVerts = memReader.ReadInt32();
                    Header.numNormals = memReader.ReadInt32();
                    Header.off14 = memReader.ReadInt32();
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

        public static void ReadPVerts(byte[] fileBuffer, ref long pos, long numVerts, ref Point3D[] Verts, string fileName)
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

        public static void ReadPNormals(byte[] fileBuffer, ref long pos, long numNormals, ref Point3D[] Normals, string fileName)
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

        public static void ReadPTexCoords(byte[] fileBuffer, ref long pos, long numTexCs, ref Point2D[] TexCoordinates, string fileName)
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

        public static void ReadPPVColors(byte[] fileBuffer, ref long pos, long numVerts, ref Color[] Vcolors, string fileName)
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

        public static void ReadPEdges(byte[] fileBuffer, ref long pos, long numEdges, ref PEdge[] Edges, string fileName)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numEdges; i++)
                    {
                        Edges[i].Verts = new short[2];

                        Edges[i].Verts[0] = memReader.ReadInt16();
                        Edges[i].Verts[1] = memReader.ReadInt16();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPPolygons(byte[] fileBuffer, ref long pos, long numPolys, ref PPolygon[] Polys, string fileName)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (var i = 0; i < numPolys; i++)
                    {
                        Polys[i].tag1 = memReader.ReadInt16();

                        Polys[i].Verts = new short[3];
                        Polys[i].Verts[0] = memReader.ReadInt16();
                        Polys[i].Verts[1] = memReader.ReadInt16();
                        Polys[i].Verts[2] = memReader.ReadInt16();

                        Polys[i].Normals = new short[3];
                        Polys[i].Normals[0] = memReader.ReadInt16();
                        Polys[i].Normals[1] = memReader.ReadInt16();
                        Polys[i].Normals[2] = memReader.ReadInt16();

                        Polys[i].Edges = new short[3];
                        Polys[i].Edges[0] = memReader.ReadInt16();
                        Polys[i].Edges[1] = memReader.ReadInt16();
                        Polys[i].Edges[2] = memReader.ReadInt16();

                        Polys[i].tag2 = memReader.ReadInt32();
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPHundrets(byte[] fileBuffer, ref long pos, long numHundrets, ref PHundret[] Hundrets, string fileName)
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

        public static void ReadPGroups(byte[] fileBuffer, ref long pos, long numGroups, ref PGroup[] Groups, string fileName)
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
                        Groups[i].DListNum = -1;
                        Groups[i].HiddenQ = false;
                        Groups[i].realGID = i;
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPBoundingBox(byte[] fileBuffer, ref long pos, ref PBoundingBox BoundingBox, string fileName)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    BoundingBox.unknown4bytes = memReader.ReadInt32();           // It seems that there are 4bytes before BoundingBox. This 4 bytes are unknown.

                    BoundingBox.max_x = memReader.ReadSingle();
                    BoundingBox.max_y = memReader.ReadSingle();
                    BoundingBox.max_z = memReader.ReadSingle();
                    BoundingBox.min_x = memReader.ReadSingle();
                    BoundingBox.min_y = memReader.ReadSingle();
                    BoundingBox.min_z = memReader.ReadSingle();

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadPNormalIndex(byte[] fileBuffer, ref long pos, int numNormIdx, ref int[] NormalIndex, string fileName)
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



        //  ---------------------------------------------------------------------------------------------------------
        //  ----------------------------------------------- MERGING -------------------------------------------------
        //  ---------------------------------------------------------------------------------------------------------
        public static void MergeGroups(ref PGroup[] Group, PGroup[] inGroup)
        {
            int gi, maxtiGroup, numGroupsGroup, numGroupsinGroup, numPolys, numEdges, numVerts, numTexCs;

            numGroupsGroup = Group.Length;
            numGroupsinGroup = inGroup.Length;

            Array.Resize(ref Group, numGroupsGroup + numGroupsinGroup);

            maxtiGroup = 0;

            for (gi = 0; gi < numGroupsGroup; gi++)
            {
                if (Group[gi].texFlag == 1)
                    if (Group[gi].texID > maxtiGroup) maxtiGroup = Group[gi].texID;
            }

            numPolys = Group[numGroupsGroup - 1].offsetPoly + Group[numGroupsGroup - 1].numPoly;
            numEdges = Group[numGroupsGroup - 1].offsetEdge + Group[numGroupsGroup - 1].numEdge;
            numVerts = Group[numGroupsGroup - 1].offsetVert + Group[numGroupsGroup - 1].numVert;

            if (Group[numGroupsGroup].texFlag == 1) numTexCs = Group[numGroupsGroup - 1].offsetTex + Group[numGroupsGroup - 1].numVert;
            else numTexCs = Group[numGroupsGroup - 1].offsetTex;

            for (gi = 0; gi < numGroupsinGroup; gi++)
            {
                inGroup[gi].offsetPoly = inGroup[gi].offsetPoly + numPolys;
                inGroup[gi].offsetVert = inGroup[gi].offsetVert + numVerts;
                inGroup[gi].offsetEdge = inGroup[gi].offsetEdge + numEdges;
                inGroup[gi].offsetTex = inGroup[gi].offsetTex + numTexCs;

                if (inGroup[gi].texFlag == 1) inGroup[gi].texID = inGroup[gi].texID + maxtiGroup;

                Group[numGroupsGroup + gi] = inGroup[gi];
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
                Model.Normals = Model.Normals.Concat(inModel.Normals).ToArray();

                // Texture Coordinates
                if (Model.Header.numTexCs == 0) Model.TexCoords = inModel.TexCoords;
                else if (inModel.Header.numTexCs > 0) Model.TexCoords = Model.TexCoords.Concat(inModel.TexCoords).ToArray();

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
                MergeGroups(ref Model.Groups, inModel.Groups);

                // BoundingBoxes
                MergeBoundingBox(ref Model.BoundingBox, inModel.BoundingBox);

                // Normal's Indexes
                Model.NormalIndex = Model.NormalIndex.Concat(inModel.NormalIndex).ToArray();

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

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
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

            glGetDoublev((uint)glCapability.GL_MODELVIEW_MATRIX, MV_matrix);

            ComputeTransformedBoxBoundingBox(MV_matrix, ref p_min_aux, ref p_max_aux, ref p_min, ref p_max);

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            glPopMatrix();
        }

        public static bool CheckModelConsistency(ref PModel Model)
        {
            int nTextures, nNormIdx, nNormals;
            int offsetPoly, offsetTex, end_group_polys, end_group_verts; // offsetVert, 
            int gi, pi;

            bool iCheckModelConsistencyResult = true;

            // Prepare Bounds
            nNormIdx = 0;
            if (Model.NormalIndex.Length > 0) nNormIdx = Model.NormalIndex.Length - 1;

            nNormals = 0;
            if (Model.Normals.Length > 0) nNormals = Model.Normals.Length - 1;

            nTextures = 0;
            if (Model.TexCoords.Length > 0) nTextures = Model.TexCoords.Length - 1;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                //offsetVert = Model.Groups[gi].offsetVert;
                end_group_verts = Model.Groups[gi].numVert;
                offsetPoly = Model.Groups[gi].offsetPoly;
                end_group_polys = Model.Groups[gi].numPoly;
                offsetTex = Model.Groups[gi].offsetTex;

                for (pi = offsetPoly; pi < end_group_polys; pi++)
                {
                    if (Model.Polys[pi].Verts[0] < 0 || Model.Polys[pi].Verts[0] > end_group_verts)
                    {
                        //MessageBox.Show("Polys_Verts inconsistence.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    if (Model.Polys[pi].Verts[1] < 0 || Model.Polys[pi].Verts[1] > end_group_verts)
                    {
                        //MessageBox.Show("Polys_Verts inconsistence.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    if (Model.Polys[pi].Verts[2] < 0 || Model.Polys[pi].Verts[2] > end_group_verts)
                    {
                        //MessageBox.Show("Polys_Verts inconsistence.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }

                    if (Model.Polys[pi].Normals[0] > nNormIdx)
                    {
                        //MessageBox.Show("Polys_Normals 0 > num_norm_indx.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    else
                    {
                        if (nNormals > 0 && Model.NormalIndex[Model.Polys[pi].Normals[0]] > nNormals)
                        {
                            //MessageBox.Show("Polys_Normals 0 > num_norm.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                    }

                    if (Model.Polys[pi].Normals[1] > nNormIdx)
                    {
                        //MessageBox.Show("Polys_Normals 1 > num_norm_indx.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    else
                    {
                        if (nNormals > 0 && Model.NormalIndex[Model.Polys[pi].Normals[1]] > nNormals)
                        {
                            //MessageBox.Show("Polys_Normals 1 > num_norm.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                    }

                    if (Model.Polys[pi].Normals[2] > nNormIdx)
                    {
                        //MessageBox.Show("Polys_Normals 2 > num_norm_indx.");     // Debug
                        iCheckModelConsistencyResult = false;
                    }
                    else
                    {
                        if (nNormals > 0 && Model.NormalIndex[Model.Polys[pi].Normals[2]] > nNormals)
                        {
                            //MessageBox.Show("Polys_Normals 2 > num_norm.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                    }

                    if (Model.Groups[gi].texFlag == 1)
                    {
                        if (Model.Polys[pi].Verts[0] + offsetTex > nTextures)
                        {
                            //MessageBox.Show("Polys Verts 0 > num_textures.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                        if (Model.Polys[pi].Verts[1] + offsetTex > nTextures)
                        {
                            //MessageBox.Show("Polys Verts 1 > num_textures.");     // Debug
                            iCheckModelConsistencyResult = false;
                        }
                        if (Model.Polys[pi].Verts[2] + offsetTex > nTextures)
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
            long gi, gi2, pi, vi, vi2, vit, tci, tciGlobal;
            int[] vertsUsage = new int[Model.Header.numVerts];

            for (vi = 0; vi < Model.Header.numVerts; vi++) vertsUsage[vi] = 0;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                for (pi = Model.Groups[gi].offsetPoly; pi < (Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly); pi++)
                {
                    for (vi = 0; vi < 3; vi++)
                    {
                        vertsUsage[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert] =
                                1 + vertsUsage[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert];
                    }
                }
            }

            vi = 0;
            vit = 0;
            tciGlobal = 0;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                while (vi < Model.Groups[gi].offsetVert + Model.Groups[gi].numVert)
                {
                    if (vertsUsage[vit] == 0)
                    {
                        //  If the vertex is unused, let's destroy it
                        for (vi2 = vi; vi2 < Model.Header.numVerts - 1; vi2++)
                        {
                            Model.Verts[vi2] = Model.Verts[vi2 + 1];
                            Model.Vcolors[vi2] = Model.Vcolors[vi2 + 1];
                        }

                        if (Model.Groups[gi].texFlag == 1)
                        {
                            for (tci = tciGlobal; tci < Model.Header.numTexCs - 1; tci++)
                                Model.TexCoords[tci] = Model.TexCoords[tci + 1];

                            Model.Header.numTexCs--;
                            Array.Resize(ref Model.TexCoords, Model.Header.numTexCs);
                        }

                        Model.Header.numVerts--;
                        Array.Resize(ref Model.Verts, Model.Header.numVerts);
                        Array.Resize(ref Model.Vcolors, Model.Header.numVerts);

                        for (pi = Model.Groups[gi].offsetPoly; pi < (Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly); pi++)
                        {
                            for (vi2 = 0; vi2 < 3; vi2++)
                            {
                                if (Model.Polys[pi].Verts[vi2] > vi - Model.Groups[gi].offsetVert)
                                    Model.Polys[pi].Verts[vi2] = (short)(Model.Polys[pi].Verts[vi2] - 1);
                            }
                        }

                        if (gi < Model.Header.numGroups - 1)
                        {
                            for (gi2 = gi + 1; gi2 < Model.Header.numGroups; gi2++)
                            {
                                Model.Groups[gi2].offsetVert--;

                                if (Model.Groups[gi].texFlag == 1 && Model.Groups[gi2].offsetTex > 0)
                                    Model.Groups[gi2].offsetTex--;

                            }
                        }

                        Model.Groups[gi].numVert--;
                    }
                    else
                    {
                        vi++;
                        if (Model.Groups[gi].texFlag == 1) tciGlobal++;
                    }

                    vit++;
                }
            }
        }

        public static void KillEmptyGroups(ref PModel Model)
        {
            int gi = 0;

            while (gi < Model.Header.numGroups)
            {
                if (Model.Groups[gi].numVert == 0) RemoveGroup(ref Model, gi);
                else gi++;
            }
        }

        public static void CreateDListFromPGroup(ref PGroup Group, ref PPolygon[] Polys, ref Point3D[] Verts,
                                                 ref Color[] Vcolors, ref Point3D[] Normals, ref Point2D[] TexCoords,
                                                 ref PHundret Hundret)
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

            glNewList((uint)Group.DListNum, glListMode.GL_COMPILE);

            DrawGroup(ref Group, ref Polys, ref Verts, ref Vcolors, ref Normals, ref TexCoords, ref Hundret, false);
            glEndList();
        }

        public static void CreateDListsFromPModel(ref PModel Model)
        {
            int gi;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                CreateDListFromPGroup(ref Model.Groups[gi], ref Model.Polys, ref Model.Verts, ref Model.Vcolors, ref Model.Normals,
                                      ref Model.TexCoords, ref Model.Hundrets[gi]);
            }
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

        private static void FillHundrestsDefaultValues(ref PHundret hundret)
        {
            hundret.field_0 = 1;
            hundret.field_4 = 1;
            hundret.field_8 = 246274;
            hundret.field_C = 147458;
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
        public static void AddGroup(ref PModel Model, ref Point3D[] vertsV, ref PPolygon[] facesV, ref Point2D[] texCoordsV,
                                    ref Color[] vcolorsV, ref Color[] pcolorsV)
        {
            //  ------------------- Warning! Causes the Normals to be inconsistent.------------------------------
            //  --------------------------------Must call ComputeNormals ----------------------------------------

            int i, gi, groupIndex, numVerts, numPolys, numTexCoords;

            numVerts = vertsV.Length;
            numPolys = facesV.Length;

            if (texCoordsV != null) numTexCoords = texCoordsV.Length;
            else numTexCoords = 0;

            if (Model.Groups != null)
            {
                groupIndex = Model.Groups.Length;
                Array.Resize(ref Model.Groups, groupIndex + 1);
                Model.Groups[groupIndex] = new PGroup();
            }
            else
            {
                Model.Groups = new PGroup[1];
                groupIndex = 0;
            }

            if (numTexCoords > 0) Model.Groups[groupIndex].polyType = 2;
            else Model.Groups[groupIndex].polyType = 1;

            if (Model.Polys != null) Model.Groups[groupIndex].offsetPoly = Model.Polys.Length;
            else Model.Groups[groupIndex].offsetPoly = 0;
            Model.Groups[groupIndex].numPoly = numPolys;

            if (Model.Verts != null) Model.Groups[groupIndex].offsetVert = Model.Verts.Length;
            else Model.Groups[groupIndex].offsetVert = 0;
            Model.Groups[groupIndex].numVert = numVerts;
            Model.Groups[groupIndex].offsetEdge = 0;
            Model.Groups[groupIndex].numEdge = 0;
            Model.Groups[groupIndex].off1C = 0;
            Model.Groups[groupIndex].off20 = 0;
            Model.Groups[groupIndex].off24 = 0;
            Model.Groups[groupIndex].off28 = 0;

            if (Model.TexCoords != null) Model.Groups[groupIndex].offsetTex = Model.TexCoords.Length;
            else Model.Groups[groupIndex].offsetTex = 0;

            Model.Groups[groupIndex].texFlag = (numTexCoords > 0) ? 1 : 0;
            Model.Groups[groupIndex].texID = 0;

            for (gi = 0; gi < groupIndex; gi++)
            {
                if (Model.Groups[groupIndex].texID <= Model.Groups[gi].texID)
                {
                    Model.Groups[groupIndex].texID = Model.Groups[gi].texID + 1;
                }
            }

            Model.Groups[groupIndex].HiddenQ = false;

            // Add new Verts to the Group
            int numDiff = Model.Header.numVerts;
            Model.Header.numVerts += numVerts;
            Array.Resize(ref Model.Verts, Model.Header.numVerts);
            Array.Resize(ref Model.Vcolors, Model.Header.numVerts);
            for (i = 0; i < numVerts; i++)
            {
                Model.Verts[numDiff + i].x = vertsV[i].x;
                Model.Verts[numDiff + i].y = vertsV[i].y;
                Model.Verts[numDiff + i].z = vertsV[i].z;

                //  Add new Vertex Colors to the Group
                Model.Vcolors[numDiff + i] = vcolorsV[i];
            }


            // Add new Polys to the Group
            numDiff = Model.Header.numPolys;
            Model.Header.numPolys += numPolys;
            Array.Resize(ref Model.Polys, Model.Header.numPolys);
            Array.Resize(ref Model.Pcolors, Model.Header.numPolys);
            for (i = 0; i < numPolys; i++)
            {
                Model.Polys[numDiff + i].Edges = facesV[i].Edges;
                Model.Polys[numDiff + i].Normals = facesV[i].Normals;
                Model.Polys[numDiff + i].tag1 = facesV[i].tag1;
                Model.Polys[numDiff + i].tag2 = facesV[i].tag2;
                Model.Polys[numDiff + i].Verts = facesV[i].Verts;

                //  Add new Poly Colors to the Group
                Model.Pcolors[numDiff + i] = pcolorsV[i];
            }

            // Add new Texture Coordinates to the Group
            numDiff = Model.Header.numTexCs;
            Model.Header.numTexCs += numTexCoords;
            if (numTexCoords > 0)
            {
                Array.Resize(ref Model.TexCoords, Model.Header.numTexCs);
                for (i = 0; i < numTexCoords; i++)
                {
                    Model.TexCoords[Model.Groups[groupIndex].offsetTex + i].x = texCoordsV[i].x;
                    Model.TexCoords[Model.Groups[groupIndex].offsetTex + i].y = texCoordsV[i].y;
                }
            }

            // Increase number of Groups
            Model.Header.numGroups += 1;

            Model.Header.mirex_g = 1;

            // Increase Hundrets
            Model.Header.mirex_h += 1;
            Array.Resize(ref Model.Hundrets, (int)Model.Header.mirex_h);
            FillHundrestsDefaultValues(ref Model.Hundrets[Model.Header.mirex_h - 1]);
        }

        public static void ApplyCurrentVColors(ref PModel Model)
        {
            int gi, vi;
            int[] vp = new int[4];

            glDisable(glCapability.GL_BLEND);

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                for (vi = Model.Groups[gi].offsetVert; vi < Model.Groups[gi].offsetVert + Model.Groups[gi].numVert; vi++)
                {
                    Model.Vcolors[vi] = GetVertColor(ref Model.Verts[vi], ref Model.Normals[vi], ref Model.Vcolors[vi]);
                }
            }
        }



        //  ----------------------------------------------------------------------------------------------------
        //  =============================================REPAIRING==============================================
        //  ----------------------------------------------------------------------------------------------------
        public static void ComputeBoundingBox(ref PModel Model)
        {
            int gi, vi;
            long pi;

            Model.BoundingBox.max_x = (float)-INFINITY_SINGLE;
            Model.BoundingBox.max_y = (float)-INFINITY_SINGLE;
            Model.BoundingBox.max_z = (float)-INFINITY_SINGLE;
            Model.BoundingBox.min_x = (float)INFINITY_SINGLE;
            Model.BoundingBox.min_y = (float)INFINITY_SINGLE;
            Model.BoundingBox.min_z = (float)INFINITY_SINGLE;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                for (pi = Model.Groups[gi].offsetPoly; pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly; pi++)
                {
                    for (vi = 0; vi < 3; vi++)
                    {
                        if (Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x > Model.BoundingBox.max_x)
                            Model.BoundingBox.max_x = Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x;
                        if (Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y > Model.BoundingBox.max_y)
                            Model.BoundingBox.max_y = Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y;
                        if (Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z > Model.BoundingBox.max_z)
                            Model.BoundingBox.max_z = Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z;

                        if (Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x < Model.BoundingBox.min_x)
                            Model.BoundingBox.min_x = Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x;
                        if (Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y < Model.BoundingBox.min_y)
                            Model.BoundingBox.min_y = Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y;
                        if (Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z < Model.BoundingBox.min_z)
                            Model.BoundingBox.min_z = Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z;
                    }
                }
            }

            Model.diameter = (float)Math.Sqrt(Math.Pow(Model.BoundingBox.max_x, 2) + Math.Pow(Model.BoundingBox.max_y, 2) + Math.Pow(Model.BoundingBox.max_z, 2) +
                                              Math.Pow(Model.BoundingBox.min_x, 2) + Math.Pow(Model.BoundingBox.min_y, 2) + Math.Pow(Model.BoundingBox.min_z, 2));
        }

        public static void ComputeCurrentBoundingBox(ref PModel Model)
        {
            Point3D p_temp;

            ComputeBoundingBox(ref Model);

            p_temp = new Point3D(Model.BoundingBox.max_x, Model.BoundingBox.max_y, Model.BoundingBox.max_z);

            p_temp = GetEyeSpaceCoords(p_temp);
            Model.BoundingBox.max_x = p_temp.x;
            Model.BoundingBox.max_y = p_temp.y;
            Model.BoundingBox.max_z = p_temp.z;

            p_temp.x = Model.BoundingBox.min_x;
            p_temp.y = Model.BoundingBox.min_y;
            p_temp.z = Model.BoundingBox.min_z;

            p_temp = GetEyeSpaceCoords(p_temp);
            Model.BoundingBox.min_x = p_temp.x;
            Model.BoundingBox.min_y = p_temp.y;
            Model.BoundingBox.min_z = p_temp.z;
        }

        public static void ComputeNormals(ref PModel Model)
        {
            int gi, pi, vi, vi2;

            Point3D tempNorm = new Point3D();

            Model.Normals = new Point3D[Model.Header.numVerts];
            Model.NormalIndex = new int[Model.Header.numVerts];

            Model.Header.numNormals = Model.Header.numVerts;
            Model.Header.numNormIdx = Model.Header.numVerts;

            Point3D[] sumNorms = new Point3D[Model.Header.numVerts];

            int[] polys_per_vert = new int[Model.Header.numVerts];

            for (pi = 0; pi < Model.Header.numPolys; pi++)
            {
                //  This should never happen. What the hell is going on?! (normals should be between 0-1 -float)
                if (Model.Polys[pi].Verts[0] < 0) Model.Polys[pi].Verts[0] = 0;
                if (Model.Polys[pi].Verts[1] < 0) Model.Polys[pi].Verts[1] = 0;
                if (Model.Polys[pi].Verts[2] < 0) Model.Polys[pi].Verts[2] = 0;
            }

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                for (pi = Model.Groups[gi].offsetPoly; pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly; pi++)
                {
                    tempNorm = CalculateNormal(ref Model.Verts[Model.Polys[pi].Verts[0] + Model.Groups[gi].offsetVert],
                                               ref Model.Verts[Model.Polys[pi].Verts[1] + Model.Groups[gi].offsetVert],
                                               ref Model.Verts[Model.Polys[pi].Verts[2] + Model.Groups[gi].offsetVert]);

                    for (vi = 0; vi < 3; vi++)
                    {
                        sumNorms[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x += tempNorm.x;
                        sumNorms[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y += tempNorm.y;
                        sumNorms[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z += tempNorm.z;

                        polys_per_vert[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert] =
                                1 + polys_per_vert[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert];

                        Model.Polys[pi].Normals[vi] = (short)(Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert);
                    }
                }
            }

            for (vi = 0; vi < Model.Header.numVerts; vi++)
            {
                if (polys_per_vert[vi] > 0)
                {
                    for (vi2 = vi + 1; vi2 < Model.Header.numVerts; vi2++)
                    {
                        if (ComparePoints3D(Model.Verts[vi], Model.Verts[vi2]))
                        {
                            sumNorms[vi].x = sumNorms[vi].x + sumNorms[vi2].x;
                            sumNorms[vi].y = sumNorms[vi].y + sumNorms[vi2].y;
                            sumNorms[vi].z = sumNorms[vi].z + sumNorms[vi2].z;

                            sumNorms[vi2] = sumNorms[vi];

                            polys_per_vert[vi] = polys_per_vert[vi] + polys_per_vert[vi2];
                            polys_per_vert[vi2] = -polys_per_vert[vi];
                        }
                    }
                }
                else
                {
                    for (vi2 = vi + 1; vi2 < Model.Header.numVerts; vi2++)
                    {
                        if (ComparePoints3D(Model.Verts[vi], Model.Verts[vi2]))
                        {
                            sumNorms[vi] = sumNorms[vi2];
                            polys_per_vert[vi] = -polys_per_vert[vi2];
                        }
                    }
                }

                polys_per_vert[vi] = Math.Abs(polys_per_vert[vi]);
            }

            for (vi = 0; vi < Model.Header.numVerts; vi++)
            {
                if (polys_per_vert[vi] > 0)
                {
                    sumNorms[vi].x = -sumNorms[vi].x / polys_per_vert[vi];
                    sumNorms[vi].y = -sumNorms[vi].y / polys_per_vert[vi];
                    sumNorms[vi].z = -sumNorms[vi].z / polys_per_vert[vi];
                }
                else
                {
                    sumNorms[vi].x = 0;
                    sumNorms[vi].y = 0;
                    sumNorms[vi].z = 0;
                }

                Model.Normals[vi] = Normalize(ref sumNorms[vi]);
                Model.NormalIndex[vi] = vi;
            }
        }

        public static void DisableNormals(ref PModel Model)
        {
            int vi, pi, gi, nii;

            Model.Normals = new Point3D[0];
            Model.NormalIndex[0] = 0;

            for (pi = 0; pi < Model.Header.numPolys; pi++)
            {
                for (vi = 0; vi < 3; vi++)
                {
                    Model.Polys[pi].Normals[vi] = 0;
                }
            }

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                if (Model.Groups[gi].polyType == 2) Model.Groups[gi].polyType = 3;
            }

            for (nii = 0; nii < Model.Header.numNormIdx; nii++)
            {
                Model.NormalIndex[nii] = 0;
            }

            Model.Header.numNormals = 0;
        }

        public static void UpdateNormal(ref PModel Model, List<int> lstVerts, int[] lstAdjacentPolysIdxs)
        {
            int pi, vi, nPolys, iGroupIdx, offsetVert;

            Point3D currentNormal = new Point3D();
            Point3D totalNormal = new Point3D();

            nPolys = lstAdjacentPolysIdxs.Length;

            for (pi = 0; pi < nPolys; pi++)
            {
                iGroupIdx = GetPolygonGroup(Model.Groups, lstAdjacentPolysIdxs[pi]);
                offsetVert = Model.Groups[iGroupIdx].offsetVert;

                currentNormal = CalculateNormal(ref Model.Verts[Model.Polys[lstAdjacentPolysIdxs[pi]].Verts[2] + offsetVert],
                                                ref Model.Verts[Model.Polys[lstAdjacentPolysIdxs[pi]].Verts[1] + offsetVert],
                                                ref Model.Verts[Model.Polys[lstAdjacentPolysIdxs[pi]].Verts[0] + offsetVert]);

                totalNormal.x += currentNormal.x;
                totalNormal.y += currentNormal.y;
                totalNormal.z += currentNormal.z;
            }

            totalNormal.x /= nPolys;
            totalNormal.y /= nPolys;
            totalNormal.z /= nPolys;
            totalNormal = Normalize(ref totalNormal);

            foreach (int itmVert in lstVerts)
                Model.Normals[itmVert] = totalNormal;
        }

        public static void UpdateNormals(ref PModel Model, List<int> lstVerts, int[] lstAdjacentPolysIdxs,
                                         stIntVector[] lstAdjacentVertsIdsx, stIntVector[] lstAdjacentAdjacentPolysIdx)
        {
            int vi;

            UpdateNormal(ref Model, lstVerts, lstAdjacentPolysIdxs);

            for (vi = 0; vi < lstAdjacentPolysIdxs.Length; vi++)
                UpdateNormal(ref Model, lstAdjacentVertsIdsx[vi].vector.ToList(), lstAdjacentAdjacentPolysIdx[vi].vector);
        }



        //
        //
        //  IMPORTANT
        //
        //  The commented part of function is commented in original VB6 Kimera.
        public static void ComputeEdges(ref PModel Model)
        {
            int gi;// pi, ei, vi;
            //int numEdges;
            //bool found;

            Model.Edges = new PEdge[Model.Header.numPolys * 3];
            for (gi = 0; gi < Model.Header.numPolys * 3; gi++) Model.Edges[gi].Verts = new short[2];

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {

                // -- Commented in KimeraVB6
                //Model.Groups[gi].offsetEdge = numEdges;
                //for (pi = (int)Model.Groups[gi].offsetPoly; pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly; pi++)
                //{
                //    for (vi = 0; vi < 3; vi++)
                //    {
                //        found = false;

                //        for (ei = 0; ei < numEdges; ei++)
                //        {
                //            if (Model.Edges[ei].Verts[0] == Model.Polys[pi].Verts[vi] && Model.Edges[ei].Verts[1] == Model.Polys[pi].Verts[(vi + 1) % 3] ||
                //                Model.Edges[ei].Verts[1] == Model.Polys[pi].Verts[vi] && Model.Edges[ei].Verts[0] == Model.Polys[pi].Verts[(vi + 1) % 3])
                //            {
                //                found = true;
                //                break;
                //            }
                //        }

                //        if (!found)
                //        {
                //            Model.Edges[numEdges].Verts[0] = Model.Polys[pi].Verts[vi];
                //            Model.Edges[numEdges].Verts[1] = Model.Polys[pi].Verts[(vi + 1) % 3];

                //            Model.Polys[pi].Edges[vi] = (short)(numEdges - Model.Groups[gi].offsetEdge);
                //            numEdges++;
                //        }
                //        else Model.Polys[pi].Edges[vi] = (short)(ei - Model.Groups[gi].offsetEdge);

                //    }
                //}

                Model.Groups[gi].numEdge = Model.Groups[gi].numPoly * 3;  // num_edges - Model.Groups[gi].offsetEdge
            }

            Model.Header.numEdges = Model.Header.numPolys * 3;  // num_edges
        }

        public static void ApplyCurrentVCoords(ref PModel Model)
        {
            int vi;

            for (vi = 0; vi < Model.Header.numVerts; vi++)
            {
                Model.Verts[vi] = GetEyeSpaceCoords(Model.Verts[vi]);
            }
        }

        public static void ComputePColors(ref PModel Model)
        {
            int gi, pi, vi;
            int temp_r, temp_g, temp_b;

            Model.Pcolors = new Color[Model.Header.numPolys];

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                for (pi = Model.Groups[gi].offsetPoly; pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly; pi++)
                {
                    temp_r = 0; temp_g = 0; temp_b = 0;

                    for (vi = 0; vi < 3; vi++)
                    {
                        temp_r += Model.Vcolors[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].R;
                        temp_g += Model.Vcolors[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].G;
                        temp_b += Model.Vcolors[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].B;
                    }

                    Model.Pcolors[pi] = Color.FromArgb(255,
                                                       temp_r / 3,
                                                       temp_g / 3,
                                                       temp_b / 3);
                }
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

                Model.resizeX = 1;
                Model.resizeY = 1;
                Model.resizeZ = 1;
                Model.repositionX = 0;
                Model.repositionY = 0;
                Model.repositionZ = 0;
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
            int nColors, ci;

            nColors = Vcolors.Length;

            for (ci = 0; ci < nColors; ci++)
            {
                Vcolors[ci] = Color.FromArgb(128,
                                             Vcolors[ci].R,
                                             Vcolors[ci].G,
                                             Vcolors[ci].B);
            }
        }

        public static void WriteGlobalPModel(ref PModel Model, string fileName)
        {
            try
            {
                SetVColorsAlphaMAX(ref Model.Vcolors);

                FileStream writeStream;

                using (writeStream = new FileStream(fileName, FileMode.Create))
                {
                    using (BinaryWriter fileWriter = new BinaryWriter(writeStream))
                    {
                        // Write Header
                        fileWriter.Write(Model.Header.off00);
                        fileWriter.Write(Model.Header.off04);
                        fileWriter.Write(Model.Header.vertexColor);
                        fileWriter.Write(Model.Header.numVerts);
                        fileWriter.Write(Model.Header.numNormals);
                        fileWriter.Write(Model.Header.off14);
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

                        // Write Texture Coords
                        foreach (Point2D up2DTexCoord in Model.TexCoords)
                        {
                            fileWriter.Write(up2DTexCoord.x);
                            fileWriter.Write(up2DTexCoord.y);
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
                        for (int gi = 0; gi < Model.Groups.Length; gi++)
                        {
                            fileWriter.Write(Model.Groups[gi].polyType);
                            fileWriter.Write(Model.Groups[gi].offsetPoly);
                            fileWriter.Write(Model.Groups[gi].numPoly);
                            fileWriter.Write(Model.Groups[gi].offsetVert);
                            fileWriter.Write(Model.Groups[gi].numVert);
                            fileWriter.Write(Model.Groups[gi].offsetEdge);
                            fileWriter.Write(Model.Groups[gi].numEdge);
                            fileWriter.Write(Model.Groups[gi].off1C);
                            fileWriter.Write(Model.Groups[gi].off20);
                            fileWriter.Write(Model.Groups[gi].off24);
                            fileWriter.Write(Model.Groups[gi].off28);
                            fileWriter.Write(Model.Groups[gi].offsetTex);
                            fileWriter.Write(Model.Groups[gi].texFlag);
                            fileWriter.Write(Model.Groups[gi].texID);
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
            int gi;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                glDeleteLists((uint)Model.Groups[gi].DListNum, 1);
            }
        }



        //  ---------------------------------------------------------------------------------------------------
        //  =========================================== COPY PMODEL ===========================================
        //  ---------------------------------------------------------------------------------------------------
        public static PModel CopyPModel(PModel modelIn)
        {
            PModel modelOut;
            int iCounter;

            if (modelIn.Polys == null) return modelIn;

            modelOut = new PModel();

            modelOut.fileName = modelIn.fileName;

            modelOut.Header = modelIn.Header;

            modelOut.Verts = new Point3D[modelIn.Verts.Length];
            modelIn.Verts.CopyTo(modelOut.Verts, 0);

            modelOut.Polys = new PPolygon[modelIn.Polys.Length];
            iCounter = 0;
            foreach (PPolygon itmPolygon in modelIn.Polys)
            {
                modelOut.Polys[iCounter] = new PPolygon(itmPolygon);
                iCounter++;
            }

            modelOut.Normals = new Point3D[modelIn.Normals.Length];
            modelIn.Normals.CopyTo(modelOut.Normals, 0);

            modelOut.TexCoords = new Point2D[modelIn.TexCoords.Length];
            modelIn.TexCoords.CopyTo(modelOut.TexCoords, 0);

            modelOut.Vcolors = new Color[modelIn.Vcolors.Length];
            modelIn.Vcolors.CopyTo(modelOut.Vcolors, 0);

            modelOut.Pcolors = new Color[modelIn.Pcolors.Length];
            modelIn.Pcolors.CopyTo(modelOut.Pcolors, 0);

            modelOut.Edges = new PEdge[modelIn.Edges.Length];
            iCounter = 0;
            foreach (PEdge itmEdge in modelIn.Edges)
            {
                modelOut.Edges[iCounter] = new PEdge(itmEdge);
                iCounter++;
            }

            modelOut.Hundrets = new PHundret[modelIn.Hundrets.Length];
            modelIn.Hundrets.CopyTo(modelOut.Hundrets, 0);

            modelOut.Groups = new PGroup[modelIn.Groups.Length];
            modelIn.Groups.CopyTo(modelOut.Groups, 0);

            modelOut.BoundingBox = modelIn.BoundingBox;

            modelOut.NormalIndex = new int[modelIn.NormalIndex.Length];
            modelIn.NormalIndex.CopyTo(modelOut.NormalIndex, 0);

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
            string strExt = Path.GetExtension(strGlobalPModelName);

            if (strExt.Length <= 0) return K_P_BATTLE_MODEL;
            else if (strExt == ".P" && strExt.Length < 3) return K_P_FIELD_MODEL;
            else return K_P_MAGIC_MODEL;
        }

        //public static void CommitCurrentVPColors(ref PModel Model)
        //{
        //    int gi, vi;
        //    int[] vp = new int[4];

        //    glDisable(glCapability.GL_BLEND);

        //    for (gi = 0; gi < Model.Header.numGroups; gi++)
        //    {
        //        // Vertex colors
        //        for (vi = Model.Groups[gi].offsetVert; vi < Model.Groups[gi].offsetVert + Model.Groups[gi].numVert; vi++)
        //        {
        //            Model.Vcolors[vi] = GetVertColor(ref Model.Verts[vi], ref Model.Normals[vi], ref Model.Vcolors[vi]);
        //        }
        //    }

        //}



        //  -------------------------------------------------------------------------------------------------
        //  ====================================== REMOVE PMODEL GROUP ======================================
        //  -------------------------------------------------------------------------------------------------
        public static int GetNextGroup(PModel Model, int iGroupIdx)
        {
            int iMinGID, iMaxGID, iNextGroup = -1;
            int iGroupCounter = 0;
            bool bFoundNextGroup = false;

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

            return iNextGroup;
        }

        public static void RemoveGroupVColors(ref PModel Model, int iGroupIdx)
        {
            int vci, vci2;
            //int iNextGroup, iActualGroup, iNumVColorsDelete;

            //iActualGroup = iGroupIdx;
            //iNumVColorsDelete = Model.Groups[iActualGroup].numVert;
            //iNextGroup = GetNextGroup(Model, iActualGroup);

            //while (iNextGroup != -1)
            //{
            //    vci2 = Model.Groups[iActualGroup].offsetVert;

            //    for (vci = Model.Groups[iNextGroup].offsetVert; vci < Model.Groups[iNextGroup].offsetVert + Model.Groups[iNextGroup].numVert; vci++)
            //    {
            //        Model.Vcolors[vci2] = Model.Vcolors[vci];
            //        vci2++;
            //    }

            //    iActualGroup = iNextGroup;
            //    iNextGroup = GetNextGroup(Model, iActualGroup);
            //}

            //Array.Resize(ref Model.Vcolors, Model.Header.numVerts - iNumVColorsDelete);

            if (GetNextGroup(Model, iGroupIdx) != -1)
            {
                if (iGroupIdx < Model.Header.numGroups - 1)
                {
                    vci2 = Model.Groups[iGroupIdx].offsetVert;

                    for (vci = Model.Groups[iGroupIdx + 1].offsetVert; vci < Model.Header.numVerts; vci++)
                    {
                        Model.Vcolors[vci2] = Model.Vcolors[vci];
                        vci2++;
                    }
                }
            }

            Array.Resize(ref Model.Vcolors, Model.Header.numVerts - Model.Groups[iGroupIdx].numVert);
        }

        public static void RemoveGroupVertices(ref PModel Model, int iGroupIdx)
        {
            int vi, vi2;
            //int iNextGroup, iActualGroup, iNumVertsDelete;

            //iActualGroup = iGroupIdx;
            //iNumVertsDelete = Model.Groups[iActualGroup].numVert;
            //iNextGroup = GetNextGroup(Model, iActualGroup);

            //while (iNextGroup != -1)
            //{
            //    vi2 = Model.Groups[iActualGroup].offsetVert;

            //    for (vi = Model.Groups[iNextGroup].offsetVert; vi < Model.Groups[iNextGroup].offsetVert + Model.Groups[iNextGroup].numVert; vi++)
            //    {
            //        Model.Verts[vi2] = Model.Verts[vi];
            //        vi2++;
            //    }

            //    iActualGroup = iNextGroup;
            //    iNextGroup = GetNextGroup(Model, iActualGroup);
            //}

            //Array.Resize(ref Model.Verts, Model.Header.numVerts - iNumVertsDelete);

            if (GetNextGroup(Model, iGroupIdx) != -1)
            {
                if (iGroupIdx < Model.Header.numGroups - 1)
                {
                    vi2 = Model.Groups[iGroupIdx].offsetVert;

                    for (vi = Model.Groups[iGroupIdx + 1].offsetVert; vi < Model.Header.numVerts; vi++)
                    {
                        Model.Verts[vi2] = Model.Verts[vi];
                        vi2++;
                    }
                }
            }

            Array.Resize(ref Model.Verts, Model.Header.numVerts - Model.Groups[iGroupIdx].numVert);
        }

        public static void RemoveGroupPColors(ref PModel Model, int iGroupIdx)
        {
            int pci, pci2;
            //int iNextGroup, iActualGroup, iNumPColorsDelete;

            //iActualGroup = iGroupIdx;
            //iNumPColorsDelete = Model.Groups[iActualGroup].numPoly;
            //iNextGroup = GetNextGroup(Model, iActualGroup);

            //while (iNextGroup != -1)
            //{
            //    pci2 = Model.Groups[iActualGroup].offsetPoly;

            //    for (pci = Model.Groups[iNextGroup].offsetPoly; pci < Model.Groups[iNextGroup].offsetPoly + Model.Groups[iNextGroup].numPoly; pci++)
            //    {
            //        Model.Pcolors[pci2] = Model.Pcolors[pci];
            //        pci2++;
            //    }

            //    iActualGroup = iNextGroup;
            //    iNextGroup = GetNextGroup(Model, iActualGroup);
            //}

            //Array.Resize(ref Model.Pcolors, Model.Header.numPolys - iNumPColorsDelete);

            if (GetNextGroup(Model, iGroupIdx) != -1)
            {
                if (iGroupIdx < Model.Header.numGroups - 1)
                {
                    pci2 = Model.Groups[iGroupIdx].offsetPoly;

                    for (pci = Model.Groups[iGroupIdx + 1].offsetPoly; pci < Model.Header.numPolys; pci++)
                    {
                        Model.Pcolors[pci2] = Model.Pcolors[pci];
                        pci2++;
                    }
                }
            }

            Array.Resize(ref Model.Pcolors, Model.Header.numPolys - Model.Groups[iGroupIdx].numPoly);
        }

        public static void RemoveGroupPolys(ref PModel Model, int iGroupIdx)
        {
            int pi, pi2;
            //int iNextGroup, iActualGroup, iNumPolysDelete;

            //iActualGroup = iGroupIdx;
            //iNumPolysDelete = Model.Groups[iActualGroup].numPoly;
            //iNextGroup = GetNextGroup(Model, iActualGroup);

            //while (iNextGroup != -1)
            //{
            //    pi2 = Model.Groups[iActualGroup].offsetPoly;

            //    for (pi = Model.Groups[iNextGroup].offsetPoly; pi < Model.Groups[iNextGroup].offsetPoly + Model.Groups[iNextGroup].numPoly; pi++)
            //    {
            //        Model.Polys[pi2] = new PPolygon(Model.Polys[pi]);
            //        pi2++;
            //    }

            //    iActualGroup = iNextGroup;
            //    iNextGroup = GetNextGroup(Model, iActualGroup);
            //}

            //Array.Resize(ref Model.Polys, Model.Header.numPolys - iNumPolysDelete);

            if (GetNextGroup(Model, iGroupIdx) != -1)
            {
                if (iGroupIdx < Model.Header.numGroups - 1)
                {
                    pi2 = Model.Groups[iGroupIdx].offsetPoly;

                    for (pi = Model.Groups[iGroupIdx + 1].offsetPoly; pi < Model.Header.numPolys; pi++)
                    {
                        Model.Polys[pi2] = Model.Polys[pi];
                        pi2++;
                    }
                }
            }

            Array.Resize(ref Model.Polys, Model.Header.numPolys - Model.Groups[iGroupIdx].numPoly);
        }

        public static void RemoveGroupEdges(ref PModel Model, int iGroupIdx)
        {
            int ei, ei2;
            //int iNextGroup, iActualGroup, iNumEdgesDelete;

            //iActualGroup = iGroupIdx;
            //iNumEdgesDelete = Model.Groups[iActualGroup].numEdge;
            //iNextGroup = GetNextGroup(Model, iActualGroup);

            //while (iNextGroup != -1)
            //{
            //    ei2 = Model.Groups[iActualGroup].offsetEdge;

            //    for (ei = Model.Groups[iNextGroup].offsetEdge; ei < Model.Groups[iNextGroup].offsetEdge + Model.Groups[iNextGroup].numEdge; ei++)
            //    {
            //        Model.Edges[ei2] = Model.Edges[ei];
            //        ei2++;
            //    }

            //    iActualGroup = iNextGroup;
            //    iNextGroup = GetNextGroup(Model, iActualGroup);
            //}

            //Array.Resize(ref Model.Edges, Model.Header.numEdges - iNumEdgesDelete);

            if (GetNextGroup(Model, iGroupIdx) != -1)
            {
                if (iGroupIdx < Model.Header.numGroups - 1)
                {
                    ei2 = Model.Groups[iGroupIdx].offsetEdge;

                    for (ei = Model.Groups[iGroupIdx + 1].offsetEdge; ei < Model.Header.numEdges; ei++)
                    {
                        Model.Edges[ei2] = Model.Edges[ei];
                        ei2++;
                    }
                }
            }

            Array.Resize(ref Model.Edges, Model.Header.numEdges - Model.Groups[iGroupIdx].numEdge);
        }

        public static void RemoveGroupTexCoords(ref PModel Model, int iGroupIdx)
        {
            int ti, ti2;
            //int iNextGroup, iActualGroup, iNumTexCsDelete;

            //iActualGroup = iGroupIdx;
            //iNumTexCsDelete = Model.Groups[iActualGroup].numVert;
            //iNextGroup = GetNextGroup(Model, iActualGroup);

            //while (iNextGroup != -1)
            //{
            //    if (Model.Groups[iActualGroup].texFlag == 1)
            //    {
            //        ti2 = Model.Groups[iActualGroup].offsetTex;

            //        for (ti = Model.Groups[iNextGroup].offsetTex; ti < Model.Groups[iNextGroup].offsetTex + Model.Groups[iNextGroup].numVert; ti++)
            //        {
            //            Model.TexCoords[ti2] = Model.TexCoords[ti];
            //            ti2++;
            //        }

            //        iActualGroup = iNextGroup;
            //        iNextGroup = GetNextGroup(Model, iActualGroup);
            //    }
            //}

            //if (Model.Groups[iActualGroup].texFlag == 1)
            //    Array.Resize(ref Model.TexCoords, Model.Header.numTexCs - iNumTexCsDelete);

            if (GetNextGroup(Model, iGroupIdx) != -1)
            {
                if (Model.Groups[iGroupIdx].texFlag == 1)
                {
                    if (iGroupIdx < Model.Header.numGroups - 1)
                    {
                        ti2 = Model.Groups[iGroupIdx].offsetTex;

                        for (ti = Model.Groups[iGroupIdx + 1].offsetTex; ti < Model.Header.numTexCs; ti++)
                        {
                            Model.TexCoords[ti2] = Model.TexCoords[ti];
                            ti2++;
                        }
                    }

                    Array.Resize(ref Model.TexCoords, Model.Header.numTexCs - Model.Groups[iGroupIdx].numVert);
                }
            }
        }

        public static void RemoveGroupHundret(ref PModel Model, int iGroupIdx)
        {
            int hi;

            if (iGroupIdx < Model.Header.numGroups - 1)
            {
                for (hi = iGroupIdx + 1; hi < Model.Header.numGroups; hi++)
                    Model.Hundrets[hi - 1] = Model.Hundrets[hi];
            }

            Array.Resize(ref Model.Hundrets, Model.Header.mirex_h - 1);
        }

        public static void RemoveGroupHeader(ref PModel Model, int iGroupIdx)
        {
            Model.Header.numPolys -= Model.Groups[iGroupIdx].numPoly;
            Model.Header.numEdges -= Model.Groups[iGroupIdx].numEdge;
            Model.Header.numVerts -= Model.Groups[iGroupIdx].numVert;
            Model.Header.mirex_h--;

            if (Model.Groups[iGroupIdx].texFlag == 1)
                Model.Header.numTexCs -= Model.Groups[iGroupIdx].numVert;

            Model.Header.numGroups--;
        }

        public static void RemoveGroupGroup(ref PModel Model, int iGroupIdx)
        {
            int gi;

            if (iGroupIdx < Model.Header.numGroups - 1)
            {
                for (gi = iGroupIdx + 1; gi <= Model.Header.numGroups; gi++)
                    Model.Groups[gi - 1] = Model.Groups[gi];
            }

            Array.Resize(ref Model.Groups, Model.Header.numGroups);
        }

        public static void RemoveGroup(ref PModel Model, int iGroupIdx)
        {
            //int gi, gi2, giActualGroup, giPrevGroup, giMinOffPoly, giMaxOffPoly;
            int gi, gi2, iActualGroup, iNextGroup;
            bool bGroupHasOffsetPolyZero;

            bGroupHasOffsetPolyZero = Model.Groups[iGroupIdx].offsetPoly == 0 ? true : false;

            if (Model.Groups[iGroupIdx].numVert > 0)
            {
                RemoveGroupVColors(ref Model, iGroupIdx);
                RemoveGroupVertices(ref Model, iGroupIdx);
                RemoveGroupPColors(ref Model, iGroupIdx);
                RemoveGroupPolys(ref Model, iGroupIdx);
                RemoveGroupEdges(ref Model, iGroupIdx);
                RemoveGroupTexCoords(ref Model, iGroupIdx);
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
            }
            else
            {
                // As we can change the group order, we need to check the group by the
                // number of polys.
                // Check if we want to remove a group of offset zero or not.
                if (bGroupHasOffsetPolyZero)
                {
                    // We have the group with polys 0. Update offsets directly.
                    Model.Groups[iGroupIdx].offsetVert = 0;
                    Model.Groups[iGroupIdx].offsetPoly = 0;
                    Model.Groups[iGroupIdx].offsetEdge = 0;
                    Model.Groups[iGroupIdx].offsetTex = 0;
                }
                else
                {
                    // Else we need to find the group with polys 0.
                    iGroupIdx = 0;
                    while (Model.Groups[iGroupIdx].offsetPoly != 0) iGroupIdx++;
                }

                // While there are groups to recalculate:
                iActualGroup = iGroupIdx;
                iNextGroup = GetNextGroup(Model, iGroupIdx);

                while (iNextGroup != - 1)
                {
                    // Assign new offsets to the next group found
                    Model.Groups[iNextGroup].offsetVert =
                        Model.Groups[iActualGroup].offsetVert + Model.Groups[iActualGroup].numVert;
                    Model.Groups[iNextGroup].offsetPoly =
                        Model.Groups[iActualGroup].offsetPoly + Model.Groups[iActualGroup].numPoly;
                    Model.Groups[iNextGroup].offsetEdge =
                        Model.Groups[iActualGroup].offsetEdge + Model.Groups[iActualGroup].numEdge;

                    if (Model.Groups[iNextGroup].texFlag == 1)
                        Model.Groups[iNextGroup].offsetTex =
                            Model.Groups[iActualGroup].offsetTex + Model.Groups[iActualGroup].numVert;
                    else
                        Model.Groups[iNextGroup].offsetTex = Model.Groups[iActualGroup].offsetTex;


                    iActualGroup = iNextGroup;
                    iNextGroup = GetNextGroup(Model, iActualGroup);
                }

                //giPrevGroup = Model.Groups.Length - 1;

                //gi = 0;
                //while (gi < Model.Header.numGroups - 1 &&
                //      (giActualGroup != 0 && giPrevGroup != 0))
                //{
                //    giPrevGroup = giActualGroup;

                //    giMinOffPoly = Model.Groups[giActualGroup].offsetPoly;
                //    giMaxOffPoly = 9999999;

                //    // Search the Next Group with the minimum offsetPoly value.
                //    for (gi2 = 0; gi2 < Model.Header.numGroups; gi2++)
                //    {
                //        if (Model.Groups[gi2].offsetPoly < giMaxOffPoly &&
                //            Model.Groups[gi2].offsetPoly > giMinOffPoly)
                //        {
                //            giMaxOffPoly = Model.Groups[gi2].offsetPoly;
                //            giActualGroup = gi2;
                //        }
                //    }

                //    // Assign new offsets to the next group found
                //    Model.Groups[giActualGroup].offsetVert =
                //        Model.Groups[giPrevGroup].offsetVert + Model.Groups[giPrevGroup].numVert;
                //    Model.Groups[giActualGroup].offsetPoly =
                //        Model.Groups[giPrevGroup].offsetPoly + Model.Groups[giPrevGroup].numPoly;
                //    Model.Groups[giActualGroup].offsetEdge =
                //        Model.Groups[giPrevGroup].offsetEdge + Model.Groups[giPrevGroup].numEdge;

                //    if (Model.Groups[giActualGroup].texFlag == 1)
                //        Model.Groups[giActualGroup].offsetTex =
                //            Model.Groups[giPrevGroup].offsetTex + Model.Groups[giPrevGroup].numVert;
                //    else
                //        Model.Groups[giActualGroup].offsetTex = Model.Groups[giPrevGroup].offsetTex;

                //    gi++;
                //}

            }

            // THIS HAVE TO BE MODIFIED. IF WE HAVE MOVED THE GROUP (LET'S SAY THE LAST GROUP TO THE TOP)
            // WE WILL ENCOUNTER PROBLEMS LATER BECAUSE GROUP 0 IS NOT TREATED WHEN REMOVING GROUP
            // FOR EXAMPLE.
            //for (gi = iGroupIdx; gi < Model.Header.numGroups; gi++)
            //{
            //    Model.Groups[gi] = Model.Groups[gi + 1];

            //    if (gi > 0)
            //    {
            //        Model.Groups[gi].offsetVert = Model.Groups[gi - 1].offsetVert + Model.Groups[gi - 1].numVert;
            //        Model.Groups[gi].offsetPoly = Model.Groups[gi - 1].offsetPoly + Model.Groups[gi - 1].numPoly;
            //        Model.Groups[gi].offsetEdge = Model.Groups[gi - 1].offsetEdge + Model.Groups[gi - 1].numEdge;

            //        if (Model.Groups[gi - 1].texFlag == 1)
            //            Model.Groups[gi].offsetTex = Model.Groups[gi - 1].offsetTex + Model.Groups[gi - 1].numVert;
            //        else
            //            Model.Groups[gi].offsetTex = Model.Groups[gi - 1].offsetTex;
            //    }
            //    else
            //    {
            //        Model.Groups[gi].offsetVert = 0;
            //        Model.Groups[gi].offsetPoly = 0;
            //        Model.Groups[gi].offsetTex = 0;
            //        Model.Groups[gi].offsetEdge = 0;
            //    }
            //}

            //Array.Resize(ref Model.Groups, Model.Header.numGroups);

            ComputeNormals(ref Model);
        }


        //  -------------------------------------------------------------------------------------------------
        //  ======================================= PEDITOR PROCEDURES ======================================
        //  -------------------------------------------------------------------------------------------------
        public static int GetClosestVertex(PModel Model, int px, int py, float DIST0, PictureBox panelEditorPModel)
        {
            int iGetClosestVertexResult = -1;

            Point3D pUP3D = new Point3D();
            Point3D vpUP3D = new Point3D();
            int gi, pi, vi, vi2, width, height;
            int[] vp = new int[4];

            float[] DIST = new float[3];
            float minDist;

            pUP3D.x = px;
            pUP3D.y = py;
            pUP3D.z = 0;

            glGetIntegerv((uint)glCapability.GL_VIEWPORT, vp);
            width = vp[2];
            height = vp[3];

            pi = GetClosestPolygon(Model, px, py, DIST0, panelEditorPModel);

            if (pi > -1)
            {
                gi = GetPolygonGroup(Model.Groups, pi);

                pUP3D.y = height - py;
                for (vi = 0; vi < 3; vi++)
                {
                    vi2 = Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert;
                    vpUP3D = GetVertexProjectedCoords(Model.Verts, vi2);
                    DIST[vi] = CalculateDistance(vpUP3D, pUP3D);
                }

                minDist = DIST[0];
                iGetClosestVertexResult = Model.Polys[pi].Verts[0] + Model.Groups[gi].offsetVert;

                for (vi = 1; vi < 3; vi++)
                {
                    if (DIST[vi] < minDist)
                    {
                        minDist = DIST[vi];
                        iGetClosestVertexResult = Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert;
                    }
                }
            }

            return iGetClosestVertexResult;
        }

        public static int GetClosestPolygon(PModel Model, int px, int py, float DIST, PictureBox panelEditorPModel)
        {
            int iGetClosestPolygonResult = 0;
            Point3D p_min = new Point3D();
            Point3D p_max = new Point3D();

            int gi, pi, vi, nPolys, width, height;
            float minZ;
            double[] rot_mat = new double[16];
            int[] vp = new int[4];

            int[] selBuff = new int[Model.Header.numPolys * 4];

            glViewport(0, 0, panelEditorPModel.ClientRectangle.Width, panelEditorPModel.ClientRectangle.Height);
            ClearPanel();
            SetDefaultOGLRenderState();

            ComputePModelBoundingBox(EditedPModel, ref p_min, ref p_max);

            SetCameraAroundModel(ref p_min, ref p_max,
                                 panXPE, panYPE, panZPE + DISTPE,
                                 alphaPE, betaPE, gammaPE, 1, 1, 1);

            glDisable(glCapability.GL_LIGHTING);

            glMatrixMode(glMatrixModeList.GL_MODELVIEW);
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

            glEnable(glCapability.GL_POLYGON_OFFSET_FILL);
            glPolygonOffset(1, 1);

            glSelectBuffer(Model.Header.numPolys * 4, selBuff);

            glPolygonMode(glFace.GL_FRONT, glPolygon.GL_LINE);
            glPolygonMode(glFace.GL_BACK, glPolygon.GL_FILL);
            glEnable(glCapability.GL_COLOR_MATERIAL);

            glGetIntegerv((uint)glCapability.GL_VIEWPORT, vp);
            width = vp[2];
            height = vp[3];

            glRenderMode(glRenderingMode.GL_SELECT);

            glMatrixMode(glMatrixModeList.GL_PROJECTION);
            glPushMatrix();
            glGetDoublev((uint)glCapability.GL_PROJECTION_MATRIX, rot_mat);
            glLoadIdentity();

            gluPickMatrix(px - 1, height - py + 1, 3, 3, vp);
            glMultMatrixd(rot_mat);

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                glInitNames();

                if (!Model.Groups[gi].HiddenQ)
                {
                    for (pi = Model.Groups[gi].offsetPoly; pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly; pi++)
                    {
                        glColor4f(Model.Pcolors[pi].R / 255.0f, Model.Pcolors[pi].G / 255.0f, Model.Pcolors[pi].B / 255.0f, Model.Pcolors[pi].A / 255.0f);
                        glColorMaterial(glFace.GL_FRONT_AND_BACK, glMaterialParameter.GL_AMBIENT_AND_DIFFUSE);

                        glPushName((uint)pi);
                        glBegin(glDrawMode.GL_TRIANGLES);
                        for (vi = 0; vi < 3; vi++)
                        {
                            glNormal3f(Model.Normals[Model.Polys[pi].Normals[vi]].x,
                                       Model.Normals[Model.Polys[pi].Normals[vi]].y,
                                       Model.Normals[Model.Polys[pi].Normals[vi]].z);

                            glVertex3f(Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].x,
                                       Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].y,
                                       Model.Verts[Model.Polys[pi].Verts[vi] + Model.Groups[gi].offsetVert].z);
                        }
                        glEnd();
                        glPopName();
                    }
                }
            }
            
            glDisable(glCapability.GL_POLYGON_OFFSET_FILL);
            glPopMatrix();

            nPolys = glRenderMode(glRenderingMode.GL_RENDER);
            iGetClosestPolygonResult = -1;
            minZ = -1;

            for (pi = 0; pi < nPolys; pi++)
            {
                if (CompareLongs((long)minZ, selBuff[pi * 4 + 1]) ||
                    selBuff[pi * 4 + 1] == minZ)
                {
                    minZ = selBuff[pi * 4 + 1];
                    iGetClosestPolygonResult = selBuff[pi * 4 + 3];
                }
            }

            glMatrixMode(glMatrixModeList.GL_PROJECTION);
            glPopMatrix();

            return iGetClosestPolygonResult;
        }

        public static int GetPolygonGroup(PGroup[] Groups, int pi)
        {
            int baseP = 0, iGetPolygonGroupResult = 0;

            baseP += Groups[0].numPoly;

            while (baseP <= pi)
            {
                iGetPolygonGroupResult++;
                baseP += Groups[iGetPolygonGroupResult].numPoly;
            }

            return iGetPolygonGroupResult;
        }

        //  -------------------------------WARNINGS!------------------------------
        //  -------*Causes the Normals to be inconsistent (call ComputeNormals).--
        //  -------*Causes inconsistent edges (call ComputeEdges).----------------
        //  -------*Can cause unused vertices (call KillUnusedVertices).----------
        public static void RemovePolygon(ref PModel Model, int indexP)
        {
            int gi, pi, iGroupIdx;

            iGroupIdx = GetPolygonGroup(Model.Groups, indexP);

            // -- Commented in KimeraVB6
            //  if (Model.Header.numPolys == 1)
            //  {
            //      MessageBox.Show"A P model must have at least 1 polygon. Can't remove this polygon.", "Info");
            //      return;
            //  }

            Model.Header.numPolys--;
            
            for (pi = indexP; pi < Model.Header.numPolys; pi++)
            {
                Model.Polys[pi] = new PPolygon(Model.Polys[pi + 1]);
                Model.Pcolors[pi] = Model.Pcolors[pi + 1];
            }

            if (iGroupIdx < Model.Header.numGroups - 1)
            {
                for (gi = iGroupIdx + 1; gi < Model.Header.numGroups; gi++)
                {
                    Model.Groups[gi].offsetPoly--;
                }
            }
            Model.Groups[iGroupIdx].numPoly--;

            //  This is technically wrong. The vector shold be emptied if Model.Header.numPolys droped to 0,
            //  but they should be inmediately refilled with something else because a P Model can't have 0
            //  polygons.
            if (Model.Header.numPolys > 0)
            {
                Array.Resize(ref Model.Polys, Model.Header.numPolys);
                Array.Resize(ref Model.Pcolors, Model.Header.numPolys);
            }
        }

        public static int GetClosestEdge(PModel Model, int pIndex, int px, int py, ref float alpha)
        {
            int iGetClosestEdgeReturn = -1;

            Point3D tmpUP3D = new Point3D();
            Point3D p1Proj = new Point3D();
            Point3D p2Proj = new Point3D();
            Point3D p3Proj = new Point3D();

            Point3D p1 = new Point3D();
            Point3D p2 = new Point3D();
            Point3D p3 = new Point3D();

            float d1, d2, d3;

            int width, height, offsetVerts;
            int[] vp = new int[4];

            glGetIntegerv((uint)glCapability.GL_VIEWPORT, vp);
            width = vp[2];
            height = vp[3];

            tmpUP3D.x = px;
            tmpUP3D.y = height - py;
            tmpUP3D.z = 0;

            offsetVerts = Model.Groups[GetPolygonGroup(Model.Groups, pIndex)].offsetVert;

            // -- Commented in KimeraVB6
            //glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            //glPushMatrix();
            //glScalef(Model.resizeX, Model.resizeY, Model.resizeZ);
            //glRotatef(Model.rotateAlpha, 1, 0, 0);
            //glRotatef(Model.rotateBeta, 0, 1, 0);
            //glRotatef(Model.rotateGamma, 0, 0, 1);
            //glTranslatef(Model.repositionX, Model.repositionY, Model.repositionZ);

            p1Proj = GetVertexProjectedCoords(Model.Verts, Model.Polys[pIndex].Verts[0] + offsetVerts);
            p2Proj = GetVertexProjectedCoords(Model.Verts, Model.Polys[pIndex].Verts[1] + offsetVerts);
            p3Proj = GetVertexProjectedCoords(Model.Verts, Model.Polys[pIndex].Verts[2] + offsetVerts);

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
            //glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            //glPopMatrix();

            return iGetClosestEdgeReturn;
        }

        public static void CopyVPColors(Color[] vpcolorsIn, ref Color[] vpcolorsOut)
        {
            int vi;

            if (vpcolorsOut == null) vpcolorsOut = new Color[vpcolorsIn.Length];
            else Array.Resize(ref vpcolorsOut, vpcolorsIn.Length);

            for (vi = 0; vi < vpcolorsIn.Length; vi++)
                vpcolorsOut[vi] = vpcolorsIn[vi];
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
            int vi, pi, iTmp;
            Point3D tmpP3D = new Point3D();

            for (vi = Model.Groups[iGroupIdx].offsetVert; vi < Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert; vi++)
            {
                tmpP3D = GetPointMirroredRelativeToPlane(Model.Verts[vi], pA, pB, pC, pD);

                if (CalculateDistance(tmpP3D, Model.Verts[vi]) > 0.00001f) Model.Verts[vi] = tmpP3D;
            }

            //  Flip faces
            for (pi = Model.Groups[iGroupIdx].offsetPoly; pi < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly; pi++)
            {
                iTmp = Model.Polys[pi].Verts[0];
                Model.Polys[pi].Verts[0] = Model.Polys[pi].Verts[1];
                Model.Polys[pi].Verts[1] = (short)iTmp;
            }
        }

        public static void MirrorHemisphere(ref PModel Model, float pA, float pB, float pC, float pD)
        {
            int gi;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                if (!Model.Groups[gi].HiddenQ)
                {
                    MirrorGroupRelativeToPlane(ref Model, gi, pA, pB, pC, pD);
                }
            }
        }

        public static bool DuplicateGroup(ref PModel Model, int iGroupIdx)
        {
            bool iDuplicateGroupResult = false;

            Point3D[] vVerts;
            PPolygon[] vFaces;
            Point2D[] vTexCoords = null;
            Color[] vVcolors;
            Color[] vPcolors;

            //  Don't duplicate empty groups
            if (Model.Groups[iGroupIdx].numVert > 0 && Model.Groups[iGroupIdx].numPoly > 0)
            {
                vVerts = new Point3D[Model.Groups[iGroupIdx].numVert];
                vVcolors = new Color[Model.Groups[iGroupIdx].numVert];
                Array.Copy(Model.Verts, Model.Groups[iGroupIdx].offsetVert, vVerts, 0, vVerts.Length);
                Array.Copy(Model.Vcolors, Model.Groups[iGroupIdx].offsetVert, vVcolors, 0, vVcolors.Length);

                vFaces = new PPolygon[Model.Groups[iGroupIdx].numPoly];
                vPcolors = new Color[Model.Groups[iGroupIdx].numPoly];
                Array.Copy(Model.Polys, Model.Groups[iGroupIdx].offsetPoly, vFaces, 0, vFaces.Length);
                Array.Copy(Model.Pcolors, Model.Groups[iGroupIdx].offsetPoly, vPcolors, 0, vPcolors.Length);

                if (Model.Groups[iGroupIdx].texFlag == 1)
                {
                    vTexCoords = new Point2D[Model.Groups[iGroupIdx].numVert];
                    Array.Copy(Model.TexCoords, Model.Groups[iGroupIdx].offsetTex, vTexCoords, 0, vTexCoords.Length);
                }                           
                
                AddGroup(ref Model, ref vVerts, ref vFaces, ref vTexCoords, ref vVcolors, ref vPcolors);

                Model.Groups[Model.Header.numGroups - 1].texID = Model.Groups[iGroupIdx].texID;

                iDuplicateGroupResult = true;
            }

            return iDuplicateGroupResult;
        }

        public static void DuplicateMirrorHemisphere(ref PModel Model, float pA, float pB, float pC, float pD)
        {
            int gi, giMirror, nGroups;

            nGroups = Model.Header.numGroups;
            giMirror = nGroups;

            for (gi = 0; gi < nGroups; gi++)
            {
                if (DuplicateGroup(ref Model, gi))
                {
                    MirrorGroupRelativeToPlane(ref Model, giMirror, pA, pB, pC, pD);
                    giMirror++;
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

        public static int AddVertex(ref PModel Model, int iGroupIdx, Point3D vPoint3D, Color vColor)
        {
            int iAddVertexResult = -1;
            //  -------- Warning! Causes the Normals to be inconsistent if lights are disabled.------------------
            //  --------------------------------Must call ComputeNormals ----------------------------------------
            int gi, vi, ni, tci, baseVerts, baseNormals, baseTexCoords;

            Model.Header.numVerts++;
            Array.Resize(ref Model.Verts, Model.Header.numVerts);
            Array.Resize(ref Model.Vcolors, Model.Header.numVerts);

            if (Model.Groups[iGroupIdx].texFlag == 1)
            {
                Model.Header.numTexCs++;
                Array.Resize(ref Model.TexCoords, Model.Header.numTexCs);
            }

            if (glIsEnabled(glCapability.GL_LIGHTING))
            {
                Model.Header.numNormals = Model.Header.numVerts;
                Array.Resize(ref Model.Normals, Model.Header.numNormals);
                Model.Header.numNormIdx = Model.Header.numVerts;
                Array.Resize(ref Model.NormalIndex, Model.Header.numNormIdx);
                Model.NormalIndex[Model.Header.numNormIdx - 1] = Model.Header.numNormIdx - 1;
            }

            if (iGroupIdx < Model.Header.numGroups - 1)
            {
                baseVerts = Model.Groups[iGroupIdx + 1].offsetVert;

                for (vi = Model.Header.numVerts - 1; vi >= baseVerts; vi--)
                {
                    Model.Verts[vi] = Model.Verts[vi - 1];
                    Model.Vcolors[vi] = Model.Vcolors[vi - 1];
                }

                if (Model.Groups[iGroupIdx].texFlag == 1)
                {
                    baseTexCoords = Model.Groups[iGroupIdx].offsetTex + Model.Groups[iGroupIdx].numVert;

                    for (tci = Model.Header.numTexCs -1; tci >= baseTexCoords; tci--)
                    {
                        Model.TexCoords[tci] = Model.TexCoords[tci - 1];
                    }
                }

                if (glIsEnabled(glCapability.GL_LIGHTING))
                {
                    if (Model.Groups[iGroupIdx].texFlag == 1)
                    {
                        baseNormals = Model.Groups[iGroupIdx + 1].offsetVert;

                        for (ni = Model.Header.numNormals - 1; ni >= baseNormals; ni--)
                        {
                            Model.Normals[ni] = Model.Normals[ni - 1];
                        }
                    }
                }

                for (gi = iGroupIdx + 1; gi < Model.Header.numGroups; gi++)
                {
                    Model.Groups[gi].offsetVert++;

                    if (Model.Groups[iGroupIdx].texFlag == 1 && Model.Groups[gi].texFlag == 1)
                    {
                        Model.Groups[gi].offsetTex++;
                    }

                }
            }

            if (iGroupIdx < Model.Header.numGroups)
            {
                Model.Verts[Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert] = vPoint3D;
                Model.Vcolors[Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert] = vColor;
                iAddVertexResult = Model.Groups[iGroupIdx].offsetVert + Model.Groups[iGroupIdx].numVert;
                Model.Groups[iGroupIdx].numVert++;
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

        public static int GetVertexGroup(PGroup[] pGroups, int iVertIdx)
        {
            int iGetVertexGroupResult = 0;
            int vBase = 0;

            vBase += pGroups[0].numVert;

            while (vBase <= iVertIdx)
            {
                iGetVertexGroupResult++;
                vBase += pGroups[iGetVertexGroupResult].numVert;
            }

            return iGetVertexGroupResult;
        }

        public static int AddPolygon(ref PModel Model, ref int[] vertsIndexBuff)
        {
            int iAddPolygonResult = -1;
            //  -------- Warning! Can cause the Normals to be inconsistent if lights are disabled.-----
            //  ---------------------------------Must call ComputeNormals -----------------------------
            int gi, pi, vi, iGroupIdx, basePolys, tmpR = 0, tmpG = 0, tmpB = 0;
            Point3D tmpVPoint3D = new Point3D();
            Color tmpColor = new Color();

            if (vertsIndexBuff[0] != vertsIndexBuff[1] &&
                vertsIndexBuff[0] != vertsIndexBuff[2])
            {
                iGroupIdx = GetVertexGroup(Model.Groups, vertsIndexBuff[0]);

                Model.Header.numPolys++;
                Array.Resize(ref Model.Polys, Model.Header.numPolys);
                Model.Polys[Model.Header.numPolys - 1] = new PPolygon(0);
                Array.Resize(ref Model.Pcolors, Model.Header.numPolys);

                if (iGroupIdx < Model.Header.numGroups - 1)
                {
                    basePolys = Model.Groups[iGroupIdx + 1].offsetPoly;

                    for (pi = Model.Header.numPolys - 1; pi >= basePolys; pi--)
                    {
                        Model.Polys[pi] = new PPolygon(Model.Polys[pi - 1]);
                        Model.Pcolors[pi] = Model.Pcolors[pi];
                    }

                    for (gi = iGroupIdx + 1; gi < Model.Header.numGroups; gi++)
                    {
                        Model.Groups[gi].offsetPoly++;
                    }
                }

                if (iGroupIdx < Model.Header.numGroups)
                {                      
                    // 0
                    Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[0] =
                            (short)(vertsIndexBuff[0] - Model.Groups[iGroupIdx].offsetVert);

                    if (Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[0] < 0 ||
                        Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[0] >= Model.Groups[iGroupIdx].numVert)
                    {
                        tmpVPoint3D = Model.Verts[vertsIndexBuff[0]];
                        tmpColor = Model.Vcolors[vertsIndexBuff[0]];
                        Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[0] =
                            (short)(AddVertex(ref Model, iGroupIdx, tmpVPoint3D, tmpColor) - Model.Groups[iGroupIdx].offsetVert);
                    }

                    // 1
                    Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[1] =
                            (short)(vertsIndexBuff[1] - Model.Groups[iGroupIdx].offsetVert);

                    if (Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[1] < 0 ||
                        Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[1] >= Model.Groups[iGroupIdx].numVert)
                    {
                        tmpVPoint3D = Model.Verts[vertsIndexBuff[1]];
                        tmpColor = Model.Vcolors[vertsIndexBuff[1]];
                        Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[1] =
                            (short)(AddVertex(ref Model, iGroupIdx, tmpVPoint3D, tmpColor) - Model.Groups[iGroupIdx].offsetVert);
                    }

                    // 2
                    Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[2] =
                            (short)(vertsIndexBuff[2] - Model.Groups[iGroupIdx].offsetVert);

                    if (Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[2] < 0 ||
                        Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[2] >= Model.Groups[iGroupIdx].numVert)
                    {
                        tmpVPoint3D = Model.Verts[vertsIndexBuff[2]];
                        tmpColor = Model.Vcolors[vertsIndexBuff[2]];
                        Model.Polys[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly].Verts[2] =
                            (short)(AddVertex(ref Model, iGroupIdx, tmpVPoint3D, tmpColor) - Model.Groups[iGroupIdx].offsetVert);
                    }

                    iAddPolygonResult = Model.Groups[iGroupIdx].numPoly;

                    for (vi = 0; vi < 3; vi++)
                    {
                        //  tmpA += Model.Vcolors[vertsIndexBuff[vi]].A;  -- Commented in KimeraVB6
                        tmpR += Model.Vcolors[vertsIndexBuff[vi]].R;
                        tmpG += Model.Vcolors[vertsIndexBuff[vi]].G;
                        tmpB += Model.Vcolors[vertsIndexBuff[vi]].B;
                    }

                    Model.Pcolors[Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly] =
                        Color.FromArgb(255, tmpR / 3, tmpG / 3, tmpB / 3);
                    Model.Groups[iGroupIdx].numPoly++;
                }

                // -- Commented in KimeraVB6
                //  if (!CheckModelConsistency(obj))
                //  {
                //      Debug.Print "WTF!!!"
                //  }
            }

            return iAddPolygonResult;
        }

        public static void OrderVertices(PModel Model, ref int[] vertBuff)
        {
            Point3D v1 = new Point3D();
            Point3D v2 = new Point3D();
            Point3D v3 = new Point3D();
            int tmpInt;

            // -- Commented in KimeraVB6
            //glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            //glPushMatrix();
            //glScalef(Model.resizeX, Model.resizeY, Model.resizeZ);
            //glRotatef(Model.rotateAlpha, 1, 0, 0);
            //glRotatef(Model.rotateBeta, 0, 1, 0);
            //glRotatef(Model.rotateGamma, 0, 0, 1);
            //glTranslatef(Model.repositionX, Model.repositionY, Model.repositionZ);

            v1 = GetVertexProjectedCoords(Model.Verts, vertBuff[0]);
            v2 = GetVertexProjectedCoords(Model.Verts, vertBuff[1]);
            v3 = GetVertexProjectedCoords(Model.Verts, vertBuff[2]);

            if (CalculateNormal(ref v1, ref v2, ref v3).z > 0)
            {
                tmpInt = vertBuff[0];
                vertBuff[0] = vertBuff[1];
                vertBuff[1] = tmpInt;

                if (CalculateNormal(ref v2, ref v1, ref v3).z > 0)
                {
                    tmpInt = vertBuff[1];
                    vertBuff[1] = vertBuff[2];
                    vertBuff[2] = tmpInt;

                    if (CalculateNormal(ref v2, ref v3, ref v1).z > 0)
                    {
                        tmpInt = vertBuff[0];
                        vertBuff[0] = vertBuff[1];
                        vertBuff[1] = tmpInt;

                        if (CalculateNormal(ref v3, ref v2, ref v2).z > 0)
                        {
                            tmpInt = vertBuff[1];
                            vertBuff[1] = vertBuff[2];
                            vertBuff[2] = tmpInt;

                            if (CalculateNormal(ref v3, ref v1, ref v2).z > 0)
                            {
                                tmpInt = vertBuff[0];
                                vertBuff[0] = vertBuff[1];
                                vertBuff[1] = tmpInt;
                            }
                        }
                    }
                }
            }

            // -- Commented in KimeraVB6
            //glMatrixMode(glMatrixModeList.GL_MODELVIEW);
            //glPopMatrix();
        }

        public static bool CutEdgeAtPoint(ref PModel Model, int iPolyIdx, int iEdgeIdx, Point3D intersectionPoint, Point2D intersectionTexCoord)
        {
            int iGroupIdx, vi1, vi2, vi3, viNew;
            int[] vBuff1 = new int[3];
            int[] vBuff2 = new int[3];
            Color tmpColor = new Color();

            iGroupIdx = GetPolygonGroup(Model.Groups, iPolyIdx);

            vi1 = Model.Polys[iPolyIdx].Verts[0] + Model.Groups[iGroupIdx].offsetVert;
            vi2 = Model.Polys[iPolyIdx].Verts[1] + Model.Groups[iGroupIdx].offsetVert;
            vi3 = Model.Polys[iPolyIdx].Verts[2] + Model.Groups[iGroupIdx].offsetVert;

            switch (iEdgeIdx)
            {
                case 0:
                    //  It makes no sens cutting an edge through one of it's vertices)
                    if (ComparePoints3D(Model.Verts[vi1], intersectionPoint) ||
                        ComparePoints3D(Model.Verts[vi2], intersectionPoint))
                        return false;

                    tmpColor = CombineColor(Model.Vcolors[vi1], Model.Vcolors[vi2]);
                    break;

                case 1:
                    if (ComparePoints3D(Model.Verts[vi2], intersectionPoint) ||
                        ComparePoints3D(Model.Verts[vi3], intersectionPoint))
                        return false;

                    tmpColor = CombineColor(Model.Vcolors[vi2], Model.Vcolors[vi3]);
                    break;

                case 2:
                    if (ComparePoints3D(Model.Verts[vi3], intersectionPoint) ||
                        ComparePoints3D(Model.Verts[vi1], intersectionPoint))
                        return false;

                    tmpColor = CombineColor(Model.Vcolors[vi3], Model.Vcolors[vi1]);
                    break;
            }

            viNew = AddVertex(ref Model, iGroupIdx, intersectionPoint, tmpColor);

            switch (iEdgeIdx)
            {
                case 0:
                    vBuff1[0] = vi1;
                    vBuff1[1] = viNew;
                    vBuff1[2] = vi3;

                    vBuff2[2] = vi3;
                    vBuff2[1] = vi2;
                    vBuff2[0] = viNew;
                    break;

                case 1:
                    vBuff1[0] = vi1;
                    vBuff1[1] = vi2;
                    vBuff1[2] = viNew;

                    vBuff2[2] = vi3;
                    vBuff2[1] = viNew;
                    vBuff2[0] = vi1;
                    break;

                case 2:
                    vBuff1[0] = vi1;
                    vBuff1[1] = vi2;
                    vBuff1[2] = viNew;

                    vBuff2[2] = vi3;
                    vBuff2[1] = vi2;
                    vBuff2[0] = viNew;
                    break;
            }

            RemovePolygon(ref Model, iPolyIdx);
            AddPolygon(ref Model, ref vBuff1);
            AddPolygon(ref Model, ref vBuff2);

            if (Model.Groups[iGroupIdx].texFlag == 1)
            {
                Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + viNew - Model.Groups[iGroupIdx].offsetVert].x = 
                    intersectionTexCoord.x;

                Model.TexCoords[Model.Groups[iGroupIdx].offsetTex + viNew - Model.Groups[iGroupIdx].offsetVert].y = 
                    intersectionTexCoord.y;
            }

            return true;
        }
        
        //  Find the first edge between v1 and v2 (poly and edge id)
        public static bool FindNextAdjacentPolyEdge(PModel Model, Point3D v1Point3D, Point3D v2Point3D,
                                                    ref int iPolyIdx, ref int iEdgeIdx)
        {
            int pi, gi, offsetVert;
            bool bfoundQ = false;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                if (!Model.Groups[gi].HiddenQ)
                {
                    offsetVert = Model.Groups[gi].offsetVert;

                    pi = Model.Groups[gi].offsetPoly;

                    while (pi < Model.Groups[gi].offsetPoly + Model.Groups[gi].numPoly)
                    {
                        if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v1Point3D) &&
                             ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v2Point3D)) ||
                            (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v2Point3D) &&
                             ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v1Point3D)))
                        {
                            iPolyIdx = pi;
                            iEdgeIdx = 0;
                            bfoundQ = true;
                        }
                        else
                        {
                            if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v1Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v2Point3D)) ||
                                (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v2Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v1Point3D)))
                            {
                                iPolyIdx = pi;
                                iEdgeIdx = 1;
                                bfoundQ = true;
                            }
                            else
                            {
                                if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v1Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v2Point3D)) ||
                                    (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v2Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v1Point3D)))
                                {
                                    iPolyIdx = pi;
                                    iEdgeIdx = 2;
                                    bfoundQ = true;
                                }
                            }
                        }

                        pi++;
                    }
                }
            }

            return bfoundQ;
        }

        //  This version of the function find the next matching edge after the one given as parameter
        public static bool FindNextAdjacentPolyEdgeForward(PModel Model, Point3D v1Point3D, Point3D v2Point3D, 
                                                           ref int iGroupIdx, ref int iPolyIdx, ref int iEdgeIdx)
        {
            int pi, offsetVert;
            bool bPolyFound = false;

            pi = iPolyIdx + 1;

            for (; iGroupIdx < Model.Header.numGroups; iGroupIdx++)
            {
                if (!Model.Groups[iGroupIdx].HiddenQ)
                {
                    offsetVert = Model.Groups[iGroupIdx].offsetVert;

                    while (pi < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly && !bPolyFound)
                    {
                        if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v1Point3D) &&
                            ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v2Point3D)) ||
                            (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v2Point3D) &&
                            ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v1Point3D)))
                        {
                            iPolyIdx = pi;
                            iEdgeIdx = 0;
                            bPolyFound = true;
                        }
                        else
                        {
                            if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v1Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v2Point3D)) ||
                                (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[1]], v2Point3D) &&
                                ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v1Point3D)))
                            {
                                iPolyIdx = pi;
                                iEdgeIdx = 1;
                                bPolyFound = true;
                            }
                            else
                            {
                                if ((ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v1Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v2Point3D)) ||
                                    (ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[2]], v2Point3D) &&
                                    ComparePoints3D(Model.Verts[offsetVert + Model.Polys[pi].Verts[0]], v1Point3D)))
                                {
                                    iPolyIdx = pi;
                                    iEdgeIdx = 2;
                                    bPolyFound = true;
                                }
                            }
                        }

                        pi++;
                    }                       
                }

                // Exit for if Poly found
                if (bPolyFound) break;
            }

            return bPolyFound;
        }

        public static int GetEqualVertices(PModel Model, int iVertIdx, ref List<int> lstVerts)
        {
            int vi, gi;
            Point3D tmpVPoint3D = new Point3D();
            HashSet<int> lstVertsUnique = new HashSet<int>();

            tmpVPoint3D = Model.Verts[iVertIdx];
            lstVerts.Clear();

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                if (!Model.Groups[gi].HiddenQ)
                {
                    for (vi = 0; vi < Model.Header.numVerts; vi++)
                    {
                        if (ComparePoints3D(Model.Verts[vi], tmpVPoint3D))
                        {
                            lstVertsUnique.Add(vi);
                        }
                    }
                }
            }

            lstVerts = lstVertsUnique.ToList();

            return lstVerts.Count;
        }

        public static bool CutPolygonThroughPlane(ref PModel Model, int iPolyIdx, int iGroupIdx,
                                                  float pA, float pB, float pC, float pD,
                                                  ref List<Point3D> knownPlaneVPoints)
        {
            int pi, ei, offsetTex, offsetVert, numKnownPlaneVPoints;
            Point3D up3DPolyNormal;
            bool isPararlelQ, equalityValidQ, cutQ;
            float fEquality, fPolyD;
            float fLambdaMultPlane, kPlane, alphaPlane;
            int p1Idx, p2Idx, t1Idx, t2Idx, iOldGroupIdx;
            Point3D intersectionPoint = new Point3D();
            Point2D intersectionTexCoord = new Point2D();
            int vIndexRectify;
            List<int> vEqualVIndices = new List<int>();
            bool p1IsContainedQ, p2IsContainedQ;

            iOldGroupIdx = iGroupIdx;
            cutQ = false;

            offsetVert = Model.Groups[iGroupIdx].offsetVert;
            offsetTex = Model.Groups[iGroupIdx].offsetTex;

            up3DPolyNormal = CalculateNormal(ref Model.Verts[Model.Polys[iPolyIdx].Verts[0] + offsetVert],
                                             ref Model.Verts[Model.Polys[iPolyIdx].Verts[1] + offsetVert],
                                             ref Model.Verts[Model.Polys[iPolyIdx].Verts[2] + offsetVert]);
            up3DPolyNormal = Normalize(ref up3DPolyNormal);

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
                isPararlelQ = isPararlelQ && Math.Abs(up3DPolyNormal.y - pB) < 0.0001f;
            }
            else
            {
                if (equalityValidQ)
                {
                    isPararlelQ = isPararlelQ && Math.Abs((pB / up3DPolyNormal.y) - fEquality) < 0.0001f;
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
                    isPararlelQ = isPararlelQ && Math.Abs((pC / up3DPolyNormal.z) - fEquality) < 0.0001f;
                }
                else
                {
                    equalityValidQ = true;
                    fEquality = pC / up3DPolyNormal.z;
                }
            }

            if (!isPararlelQ)
            {
                fPolyD = -up3DPolyNormal.x * Model.Verts[Model.Polys[iPolyIdx].Verts[0] + offsetVert].x -
                         up3DPolyNormal.y * Model.Verts[Model.Polys[iPolyIdx].Verts[0] + offsetVert].y -
                         up3DPolyNormal.z * Model.Verts[Model.Polys[iPolyIdx].Verts[0] + offsetVert].z;

                ei = 0;
                kPlane = 0;
                cutQ = false;

                do
                {
                    p1Idx = Model.Polys[iPolyIdx].Verts[ei] + offsetVert;
                    p2Idx = Model.Polys[iPolyIdx].Verts[(ei + 1) % 3] + offsetVert;

                    t1Idx = Model.Polys[iPolyIdx].Verts[ei] + offsetTex;
                    t2Idx = Model.Polys[iPolyIdx].Verts[(ei + 1) % 3] + offsetTex;

                    //  Degenerated triangle, don't bother
                    if (ComparePoints3D(Model.Verts[p2Idx], Model.Verts[p1Idx])) return false;

                    //  Check if the edge is contained on the plane
                    p1IsContainedQ = false;
                    p2IsContainedQ = false;

                    for (pi = 0; pi < knownPlaneVPoints.Count; pi++)
                    {
                        if (ComparePoints3D(Model.Verts[p1Idx], knownPlaneVPoints[pi]))
                            p1IsContainedQ = true;

                        if (ComparePoints3D(Model.Verts[p2Idx], knownPlaneVPoints[pi]))
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
                            cutQ = CutEdgeAtPoint(ref Model, iPolyIdx, ei, intersectionPoint, intersectionTexCoord);
                            CheckModelConsistency(ref Model);
                            iGroupIdx = GetPolygonGroup(Model.Groups, iPolyIdx);

                            while (FindNextAdjacentPolyEdgeForward(EditedPModel, Model.Verts[p1Idx], Model.Verts[p2Idx],
                                                                   ref iGroupIdx, ref iPolyIdx, ref ei))
                            {
                                //  Must recompute the texture junction point everytime we go beyond a textured
                                //  group boundaries.
                                if (iOldGroupIdx != iGroupIdx)
                                {
                                    if (Model.Groups[iGroupIdx].texFlag == 1)
                                    {
                                        offsetTex = Model.Groups[iGroupIdx].offsetTex;
                                        t1Idx = Model.Polys[iPolyIdx].Verts[ei] + offsetTex;
                                        t2Idx = Model.Polys[iPolyIdx].Verts[(ei + 1) % 3] + offsetTex;

                                        intersectionTexCoord = 
                                            GetPointInLine2D(Model.TexCoords[t1Idx], Model.TexCoords[t2Idx], alphaPlane);
                                    }

                                    iOldGroupIdx = iGroupIdx;
                                }

                                cutQ = CutEdgeAtPoint(ref EditedPModel, iPolyIdx, ei, intersectionPoint, intersectionTexCoord);
                            }

                            //  Add the new point to the known plane points list
                            knownPlaneVPoints = new List<Point3D>();
                            for (int i = 0; i < numKnownPlaneVPoints; i++) knownPlaneVPoints.Add(new Point3D());
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
                                knownPlaneVPoints = new List<Point3D>();
                                for (int i = 0; i < numKnownPlaneVPoints; i++) knownPlaneVPoints.Add(new Point3D());
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

                    ei++;
                } while (!cutQ && ei < 3);
            }
 
            return cutQ;
        }

        public static void CutPModelThroughPlane(ref PModel Model, 
                                                 float pA, float pB, float pC, float pD,
                                                 ref List<Point3D> knownPlaneVPoints)
        {
            int gi, pi;
            int offsetPoly;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                offsetPoly = Model.Groups[gi].offsetPoly;
                pi = offsetPoly;

                while (pi < offsetPoly + Model.Groups[gi].numPoly - 1)
                { 
                    if (!CutPolygonThroughPlane(ref Model, pi, gi, pA, pB, pC, pD, ref knownPlaneVPoints))
                    {
                        CheckModelConsistency(ref Model);
                        pi++;
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
            Point3D vectNormP3D = new Point3D();

            orthogonalProjectionP3D = GetPoint3DOrthogonalProjection(uPoint3D, pA, pB, pC, pD);

            vectP3D = new Point3D(orthogonalProjectionP3D.x - uPoint3D.x,
                                  orthogonalProjectionP3D.y - uPoint3D.y,
                                  orthogonalProjectionP3D.z - uPoint3D.z);

            vectNormP3D = Normalize(ref vectP3D);

            return !(Math.Abs(pA - vectNormP3D.x) < 0.0001f &&
                     Math.Abs(pB - vectNormP3D.y) < 0.0001f &&
                     Math.Abs(pC - vectNormP3D.z) < 0.0001f &&
                     !(CalculateDistance(uPoint3D, orthogonalProjectionP3D) < 0.0001f));
        }

        public static bool IsPoint3DAbovePlane(Point3D uPoint3D, float pA, float pB, float pC, float pD)
        {
            Point3D orthogonalProjectionP3D;
            Point3D vectP3D;
            Point3D vectNormP3D = new Point3D();

            orthogonalProjectionP3D = GetPoint3DOrthogonalProjection(uPoint3D, pA, pB, pC, pD);

            vectP3D = new Point3D(orthogonalProjectionP3D.x - uPoint3D.x,
                                  orthogonalProjectionP3D.y - uPoint3D.y,
                                  orthogonalProjectionP3D.z - uPoint3D.z);

            vectNormP3D = Normalize(ref vectP3D);

            return  (Math.Abs(pA - vectNormP3D.x) < 0.0001f &&
                     Math.Abs(pB - vectNormP3D.y) < 0.0001f &&
                     Math.Abs(pC - vectNormP3D.z) < 0.0001f &&
                     !(CalculateDistance(uPoint3D, orthogonalProjectionP3D) < 0.0001f));

        }

        public static void EraseHemisphereVertices(ref PModel Model, float pA, float pB, float pC, float pD,
                                                   bool underPlaneQ, ref List<Point3D> knownPlaneVPoints)
        {
            int gi, pi, vi, offsetVert, offsetPoly, iVertIdx, kppi;
            bool atLeastOneSparedQ, foundQ;

            for (gi = 0; gi < Model.Header.numGroups; gi++)
            {
                offsetVert = Model.Groups[gi].offsetVert;
                offsetPoly = Model.Groups[gi].offsetPoly;

                pi = offsetPoly;

                while (pi < (offsetPoly + Model.Groups[gi].numPoly) && Model.Header.numPolys > 1)
                {
                    atLeastOneSparedQ = false;

                    for (vi = 0; vi < 3; vi++)
                    {
                        foundQ = false;
                        iVertIdx = Model.Polys[pi].Verts[vi] + offsetVert;

                        for (kppi = 0; kppi < knownPlaneVPoints.Count; kppi++)
                        {
                            if (ComparePoints3D(Model.Verts[iVertIdx], knownPlaneVPoints[kppi]))
                            {
                                foundQ = true;
                                break;
                            }
                        }

                        if (!foundQ)
                        {
                            if (underPlaneQ)
                            {
                                if (IsPoint3DUnderPlane(Model.Verts[iVertIdx], pA, pB, pC, pD))
                                    atLeastOneSparedQ = true;
                            }
                            else
                            {
                                if (IsPoint3DAbovePlane(Model.Verts[iVertIdx], pA, pB, pC, pD))
                                    atLeastOneSparedQ = true;
                            }
                        }
                    }

                    if (!atLeastOneSparedQ) RemovePolygon(ref Model, pi);
                    else pi++;
                }
            }

            if (Model.Header.numPolys == 1)
            {
                MessageBox.Show("A .P Model must have at least one polygon. The last triangle was spared.",
                                "Information", MessageBoxButtons.OK);
            }

            KillUnusedVertices(ref Model);
            KillEmptyGroups(ref Model);
        }

        public static void ApplyPModelTransformation(ref PModel Model, double[] transMatrix)
        {
            Point3D tmpPoint3D = new Point3D();
            int vi;

            for (vi = 0; vi < Model.Header.numVerts; vi++)
            {
                MultiplyPoint3DByOGLMatrix(transMatrix, Model.Verts[vi], ref tmpPoint3D);
                Model.Verts[vi] = tmpPoint3D;
            }
        }

        public static void FattenPModel(ref PModel Model)
        {
            int vi;

            float centralZ, maxDiff, minDiff, factor;

            centralZ = 0;
            maxDiff = Math.Abs(Model.BoundingBox.max_z - centralZ);
            minDiff = Math.Abs(centralZ - Model.BoundingBox.min_z);

            for (vi = 0; vi < Model.Header.numVerts; vi++)
            {
                if (Model.Verts[vi].z > centralZ)
                {
                    if (maxDiff == 0) factor = 1;
                    else factor = (float)(1 + (1 - Math.Abs(Model.Verts[vi].z - centralZ) / maxDiff) * 0.1);
                }
                else
                {
                    if (minDiff == 0) factor = 1;
                    else factor = (float)(1 + (1 - Math.Abs(centralZ - Model.Verts[vi].z) / minDiff) * 0.1);
                }

                Model.Verts[vi].x *= factor;
                Model.Verts[vi].y *= factor;
            }
        }

        public static void SlimPModel(ref PModel Model)
        {
            int vi;

            float centralZ, maxDiff, minDiff, factor;

            centralZ = 0;
            maxDiff = Math.Abs(Model.BoundingBox.max_z - centralZ);
            minDiff = Math.Abs(centralZ - Model.BoundingBox.min_z);

            for (vi = 0; vi < Model.Header.numVerts; vi++)
            {
                if (Model.Verts[vi].z > centralZ)
                {
                    if (maxDiff == 0) factor = 1;
                    else factor = (float)(1 + (1 - Math.Abs(Model.Verts[vi].z - centralZ) / maxDiff) * 0.1);
                }
                else
                {
                    if (minDiff == 0) factor = 1;
                    else factor = (float)(1 + (1 - Math.Abs(centralZ - Model.Verts[vi].z) / minDiff) * 0.1);
                }

                Model.Verts[vi].x /= factor;
                Model.Verts[vi].y /= factor;
            }
        }

        public static int GetAdjacentPolygonsVertices(PModel Model, List<int> lstVerts, ref int[] lstPolysBuffer)
        {
            int iGroupIdx, pi, vi, pvi, nPolys;

            nPolys = 0;
            lstPolysBuffer = new int[0];

            foreach (int itmVert in lstVerts)
            {
                iGroupIdx = GetVertexGroup(Model.Groups, itmVert);

                for (pi = Model.Groups[iGroupIdx].offsetPoly; pi < Model.Groups[iGroupIdx].offsetPoly + Model.Groups[iGroupIdx].numPoly; pi++)
                {
                    for (pvi = 0; pvi < 3; pvi++)
                    {
                        if (Model.Polys[pi].Verts[pvi] == itmVert - Model.Groups[iGroupIdx].offsetVert)
                        {
                            Array.Resize(ref lstPolysBuffer, nPolys);
                            lstPolysBuffer[nPolys] = pi;
                            nPolys++;
                            break;
                        }
                    }
                }
            }

            return nPolys;
        }

        public static float GetVertexProjectedDepth(ref Point3D[] lstVerts, int vi)
        {
            glClear(glBufferMask.GL_DEPTH_BUFFER_BIT);
            return (float)GetDepthZ(lstVerts[vi]);
        }

        public static int GetPolygonAdjacentVertexIndexes(PModel Model, List<int> lstVertexPolys, ref int[] lstIndexesDiscarded, ref stIntVector[] lstAdjacentIndexesOut)
        {
            // nPolys is lstVertexPolys.Count
            int iGetPolygonAdjacentVertexIndexesResult = 0;
            int iGroupIdx, pi, pvi, vid, nDiscarded, offsetVert; // nEqualVerts, 
            List<int> lstEqualVerts = new List<int>();
            bool foundQ;

            nDiscarded = lstIndexesDiscarded.Length;
            
            for (pi = 0; pi < lstVertexPolys.Count; pi++)
            {
                iGroupIdx = GetPolygonGroup(Model.Groups, lstVertexPolys[pi]);
                offsetVert = Model.Groups[iGroupIdx].offsetVert;

                for (pvi = 0; pvi < 3; pvi++)
                {
                    //  Check whether the vertex should be ignored or not
                    foundQ = false;

                    for (vid = 0; vid < nDiscarded; vid++)
                    {
                        if (lstIndexesDiscarded[vid] - offsetVert == Model.Polys[lstVertexPolys[pi]].Verts[pvi])
                        {
                            foundQ = true;
                            break;
                        }
                    }

                    if (!foundQ)
                    {
                        //  Check if the vertex (or similar) is already added to the list
                        for (vid = 0; vid < iGetPolygonAdjacentVertexIndexesResult; vid++)
                        {
                            if (ComparePoints3D(Model.Verts[lstAdjacentIndexesOut[vid].vector[0]],
                                                Model.Verts[Model.Polys[lstVertexPolys[pi]].Verts[pvi] + offsetVert]))
                            {
                                foundQ = true;
                                break;
                            }
                        }

                        if (!foundQ)
                        {
                            lstEqualVerts.Clear();

                            //  Find all similar vertices
                            GetEqualVertices(Model, Model.Polys[lstVertexPolys[pi]].Verts[pvi], ref lstEqualVerts);

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
                                                      ref stIntVector[] lstAdjacentVertsIdxs, ref stIntVector[] lstAdjacentAdjacentPolysIdxs)
        {
            int vi, nPolys, nAdjacentVerts;

            //  Get the polygons adjacent to the selected vertices
            GetAdjacentPolygonsVertices(Model, lstVerts.ToList(), ref lstAdjacentPolysIdxs);

            //  Get the vertices adjacent to the selected vertices
            nAdjacentVerts = GetPolygonAdjacentVertexIndexes(Model, lstVerts.ToList(), ref lstAdjacentPolysIdxs, ref lstAdjacentVertsIdxs);

            //  Get polygons adjacent to the adjacent
            Array.Resize(ref lstAdjacentAdjacentPolysIdxs, nAdjacentVerts);

            for (vi = 0; vi < nAdjacentVerts; vi++)
            {
                nPolys = GetAdjacentPolygonsVertices(Model, lstAdjacentVertsIdxs[vi].vector.ToList(), ref lstAdjacentAdjacentPolysIdxs[vi].vector);
                lstAdjacentAdjacentPolysIdxs[vi].length = nPolys;
            }
        }


    }
}
