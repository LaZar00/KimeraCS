// Info of this comes from:
// https://wiki.xentax.com/index.php/Playstation_TMD

//Specific info about some fields of the structures:

// U_CHAR FLAG OPTIONS FOR PRIMITIVE
// {
//    bit   0;   -LGT.Light source calculation
//                       0 - On.Light source calculation is carried out
//                       1 - Off.Light source calculation is not carried out
//    bit   1;   -FCE.Determines if primitive is one or two faced
//                       0 - Primitive has one face
//                       1 - Primitive has two faces
//    bit   2;   -GRD.Determines single or gradient pigment
//                       0 - Primitive has one solid pigment
//                       1 - Primitive has different pigments at each vertex
//    bits  3-7; -ignored
// }

// U_CHAR MODE OPTIONS FOR PRIMITIVE
// {
//    bit   0;   -TGE.Brightness calculation at time of calculation
//                       0 - On.Calculates light
//                       1 - Off.Draws texture as is (without lighting)
//    bit   1;   -ABE.Activates translucency when rendered
//                       0 - Off.Primitive is rendered solid.
//                       1 - On.Primitive is rendered semitranslucent.
//    bit   2;   -TME.Sets whether a texture is used or not
//                       0 - Off.No texture is rendered.
//                       1 - On.A texture is rendered.
//    bit   3;   -displays whether a 3 or 4 sided polygon
//                       0 - 3 sided polygon
//                       1 - 4 sided polygon
//    bit   4;   -IIP.Shading mode
//                       0 - Flat shading
//                       1 - Gouraud shading
//    bits  5-7; -Code.Defines which sort of entity to draw.
//                       (5)0 - Polygon(3 sided or 4 sided)
//                       (6)1 - Straight line
//                       (7)2 - Sprite
// }

// U_SHORT CBA - position of CLUT in VRAM for primitives with textures 
// {
//    bits  0-5;    -upper 6 bits of 10 bits of X coordinate value
//                   for CLUT in VRAM
//    bits  6-14;   -9 bits of Y coordinate value for CLUT in VRAM
//    bit 15;       -ignored
// }


// U_SHORT TSB - information about image for primitives with textures 
//{
//    bits  0-4;    -texture page number of texture
//    bits  5-6;    -ABR.Semitransparency method(see below)
//    bits  7-8;    -TPF.Colour mode of the texture(see below)
//    bits  9-15;    -ignored   
// }

// ABR Values
// 0 - 50  % background + 50  % polygon
// 1 - 100 % background + 100 % polygon
// 2 - 100 % background - 100 % polygon
// 3 - 100 % background + 25  % polygon

// TPF Values
// 0     - 4 bit texture and CLUT
// 1     - 8 bit texture and CLUT
// 2     - 15 bit texture with no CLUT



using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace KimeraCS
{

    using Defines;

    using static FF7Skeleton;
    using static FF7PModel;

    using static Utils;
    using static FileTools;

    public class FF7TMDModel
    {
        public const int TMD_PADDING = 12;


        //////////////////////////////////////////////////////////////////////////////////////////
        // TMD Packet Struct types

        // 3 SIDED, FLAT SHADING, FLAT PIGMENT 
        // olen = 0x04;   ilen =0x03;   flag =0x00;    mode =0x20;
        public struct TMD_3_FS_FP
        {
            public byte R, G, B;               // pigment of polygon
            public byte mode2;                 // same as mode in primitive
            public ushort Normal0;             // index value of normal element
            public ushort Vertex0;             // index value of vertex element
            public ushort Vertex1;
            public ushort Vertex2;
        }

        // 3 SIDED, GOURAUD SHADING, FLAT PIGMENT 
        // olen = 0x06;   ilen =0x04;   flag =0x00;    mode =0x30;
        public struct TMD_3_GS_FP
        {
            public byte R, G, B;               // pigment of polygon
            public byte mode2;                 // same as mode in primitive
            public ushort Normal0;             // index value of normal element
            public ushort Vertex0;             // index value of vertex element
            public ushort Normal1;
            public ushort Vertex1;
            public ushort Normal2;
            public ushort Vertex2;
        }

        // 3 SIDED, FLAT SHADING, GRADIENT PIGMENT 
        // olen = 0x06;   ilen =0x05;   flag =0x04;    mode =0x20;
        public struct TMD_3_FS_GP
        {
            public byte R0, G0, B0;            // pigment of polygon at Vertex0
            public byte mode2;                 // same as mode in primitive
            public byte R1, G1, B1;            // pigment of polygon at Vertex1
            public byte pad1;                  // ignored
            public byte R2, G2, B2;            // pigment of polygon at Vertex2
            public byte pad2;                  // ignored
            public ushort Normal0;             // index value of normal element
            public ushort Vertex0;             // index value of vertex element
            public ushort Vertex1;
            public ushort Vertex2;
        }

        // 3 SIDED, GOURAUD SHADING, GRADIENT PIGMENT 
        // olen = 0x06;   ilen =0x06;   flag =0x04;    mode =0x30;
        public struct TMD_3_GS_GP
        {
            public byte R0, G0, B0;            // pigment of polygon at Vertex0
            public byte mode2;                 // same as mode in primitive
            public byte R1, G1, B1;            // pigment of polygon at Vertex1
            public byte pad1;                  // ignored
            public byte R2, G2, B2;            // pigment of polygon at Vertex2
            public byte pad2;                  // ignored
            public ushort Normal0;             // index value of normal element
            public ushort Vertex0;             // index value of vertex element
            public ushort Normal1;
            public ushort Vertex1;
            public ushort Normal2;
            public ushort Vertex2;
        }

        // 3 SIDED, TEXTURED, FLAT SHADING, NO PIGMENT
        // olen = 0x07;   ilen =0x05;   flag =0x00;    mode =0x24;
        public struct TMD_3_TX_FS_NP
        {
            public byte U0, V0;                // X and Y coordinate of texture for Vertex0
            public ushort CBA;                 // position of CLUT for texture in VRAM(see earlier)
            public byte U1, V1;                // X and Y coordinate of texture for Vertex1
            public ushort TSB;                 // information about texture in VRAM(see earlier)
            public byte U2, V2;                // X and Y coordinate of texture for Vertex2
            public ushort pad;                 // ignored
            public ushort Normal;              // index value of normal element
            public ushort Vertex0;             // index value of vertex element
            public ushort Vertex1;
            public ushort Vertex2;
        }

        // 3 SIDED, TEXTURED, GOURAUD SHADING, NO PIGMENT 
        // olen = 0x9;   ilen =0x06;   flag =0x00;    mode =0x34;
        public struct TMD_3_TX_GS_NP
        {
            public byte U0, V0;                // X and Y coordinate of texture for Vertex0
            public ushort CBA;                 // position of CLUT for texture in VRAM(see earlier)
            public byte U1, V1;                // X and Y coordinate of texture for Vertex1
            public ushort TSB;                 // information about texture in VRAM(see earlier)
            public byte U2, V2;                // X and Y coordinate of texture for Vertex2
            public ushort pad;                 // ignored
            public ushort Normal;              // index value of normal element
            public ushort Vertex0;             // index value of vertex element
            public ushort Normal1;
            public ushort Vertex1;
            public ushort Normal2;
            public ushort Vertex2;
        }

        // 3 SIDED, NO SHADING, FLAT PIGMENT 
        // olen = 0x04;   ilen =0x03;   flag =0x01;    mode =0x21;
        public struct TMD_3_NS_FP
        {
            public byte R, G, B;               // pigment of polygon
            public byte mode2;                 // same as mode in primitive
            public ushort Vertex0;             // index value of vertex element
            public ushort Vertex1;
            public ushort Vertex2;
            public ushort pad;                 // ignored
        }

        // 3 SIDED, NO SHADING, GRADIENT PIGMENT 
        // olen = 0x06;   ilen =0x05;   flag =0x01;    mode =0x35;
        // olen = 0x06;   ilen =0x05;   flag =0x01;    mode =0x31;          FF7 Vertex Colored Version
        public struct TMD_3_NS_GP
        {
            public byte R0, G0, B0;            // pigment of polygon at Vertex0
            public byte mode2;                 // same as mode in primitive
            public byte R1, G1, B1;            // pigment of polygon at Vertex1
            public byte pad1;                  // ignored
            public byte R2, G2, B2;            // pigment of polygon at Vertex2
            public byte pad2;                  // ignored
            public ushort Vertex0;             // index value of vertex element
            public ushort Vertex1;
            public ushort Vertex2;
            public ushort pad;                 // ignored
        }

        // 3 SIDED, TEXTURED, NO SHADING, FLAT PIGMENT 
        // olen = 0x07;   ilen =0x06;   flag =0x01;    mode =0x25;          FF7 Texture Version
        public struct TMD_3_TX_NS_FP
        {
            public byte U0, V0;                // X and Y coordinate of texture for Vertex0
            public ushort CBA;                 // position of CLUT for texture in VRAM(see earlier)
            public byte U1, V1;                // X and Y coordinate of texture for Vertex1
            public ushort TSB;                 // information about texture in VRAM(see earlier)
            public byte U2, V2;                // X and Y coordinate of texture for Vertex2
            public ushort pad1;                // ignored
            public byte R, G, B;               // pigment of polygon
            public byte pad2;
            public ushort Vertex0;             // index value of vertex element
            public ushort Vertex1;
            public ushort Vertex2;
            public ushort pad;                 // ignored
        }

        // 3 SIDED, TEXTURED, NO SHADING, GRADIENT PIGMENT
        // olen = 0x9;   ilen =0x08;   flag =0x01;    mode =0x35;
        public struct TMD_3_TX_NS_GP
        {
            public byte U0, V0;                // X and Y coordinate of texture for Vertex0
            public ushort CBA;                 // position of CLUT for texture in VRAM(see earlier)
            public byte U1, V1;                // X and Y coordinate of texture for Vertex1
            public ushort TSB;                 // information about texture in VRAM(see earlier)
            public byte U2, V2;                // X and Y coordinate of texture for Vertex2
            public ushort pad1;                // ignored
            public byte R0, G0, B0;            // pigment of polygon at Vertex0
            public byte pad2;                  // ignored
            public byte R1, G1, B1;            // pigment of polygon at Vertex1
            public byte pad3;                  // ignored
            public byte R2, G2, B2;            // pigment of polygon at Vertex2
            public byte pad4;                  // ignored
            public ushort Vertex0;             // index value of vertex element
            public ushort Vertex1;
            public ushort Vertex2;
            public ushort pad;                 // ignored
        }

        //////////////////////////////////////////////////////////////////////////////////////////


        // TMD Main structs
        public struct TMD_HEADER
        {
            public int version;                 //  version of TMD. Always 0x00000041
            public int flags;              //  Indicates when addresses are relative or explicit
            public int nObjects;           //  number of objects in the TMD
        }

        public struct TMD_PRIMITIVE_HEADER
        {
            public byte olen;              //  word length of the drawing primitive created by GPU
            public byte ilen;              //  word length of the packet data section
            public byte flag;              //  options when rendering
            public byte mode;              //  indicates type of primitive
        }

        public struct TMD_VERTEX
        {
            public short vx;               //  x value of the vertex
            public short vy;               //  y value of the vertex
            public short vz;               //  z value of the vertex
            public short pad;              //  ignore

            public float fvx;              //  x value (float) of the vertex
            public float fvy;              //  y value (float) of the vertex
            public float fvz;              //  z value (float) of the vertex
            public float fpad;             //  ignore
        }

        public struct TMD_NORMAL
        {
            public short nx;               //  x value of the normal
            public short ny;               //  y value of the normal
            public short nz;               //  z value of the normal
            public short pad;              //  ignore
        }

        public struct TMD_PRIMITIVE_PACKET
        {
            public TMD_3_FS_FP tmd3fsfp;
            public TMD_3_GS_FP tmd3gsfp;
            public TMD_3_FS_GP tmd3fsgp;
            public TMD_3_GS_GP tmd3gsgp;
            public TMD_3_TX_FS_NP tmd3txfsnp;
            public TMD_3_TX_GS_NP tmd3txgsnp;
            public TMD_3_NS_FP tmd3nsfp;
            public TMD_3_NS_GP tmd3nsgp;
            public TMD_3_TX_NS_FP tmd3txnsfp;
            public TMD_3_TX_NS_GP tmd3txnsgp;
        }

        public struct TMD_OBJECT
        {
            public int offsetVerts;        //  start address of vertex list
            public int nVerts;             //  number of vertices in the object
            public int offsetNormals;      //  start address of normal list
            public int nNormals;           //  number of normals in the object
            public int offsetPrimitives;   //  start address of primitive list
            public int nPrimitives;        //  number of primitives in the object
            public int scale;              //  Ignored. For FF7 normally 0.

            public TMD_PRIMITIVE_HEADER[] TMDPrimitiveList;
            public TMD_PRIMITIVE_PACKET[] TMDPrimitiveListPacket;
            public TMD_VERTEX[] TMDVertexList;
            public TMD_NORMAL[] TMDNormalList;
        }

        public struct TMDModel
        {
            public TMD_HEADER TMDHeader;
            public TMD_OBJECT[] TMDObjectList;
        }

        public static bool bConverted2Float;

        public static void LoadTMDModel(ref TMDModel mTMDModel, string strTMDFolder, string strTMDFileName)
        {
            byte[] fileBuffer;
            long fileBufferPos = 0, fileBufferPosItems = 0, iCountObj, iCountItem;
            string strTMDFullFileName = strTMDFolder + "\\" + strTMDFileName;

            // Let's read TMD file into memory.
            // First check if exists
            if (!File.Exists(strTMDFullFileName))
            {
                //  Debug.Print fileName
                throw new FileNotFoundException("Error opening .TMD Model " + strTMDFileName + " file.");
            }

            // Read raw data of TMD Model file into memory
            fileBuffer = File.ReadAllBytes(strTMDFullFileName);


            //// Read TMD Model structure.
            // Header
            mTMDModel.TMDHeader = new TMD_HEADER();
            ReadTMDHeader(fileBuffer, ref fileBufferPos, ref mTMDModel.TMDHeader, strTMDFullFileName);

            // Objects
            mTMDModel.TMDObjectList = new TMD_OBJECT[mTMDModel.TMDHeader.nObjects];
            for (iCountObj = 0; iCountObj < mTMDModel.TMDHeader.nObjects; iCountObj++)
            {
                // Read Object
                ReadTMDObject(fileBuffer, ref fileBufferPos, ref mTMDModel.TMDObjectList[iCountObj]);

                // Read Object Data
                // Read Primitives
                mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveList =
                        new TMD_PRIMITIVE_HEADER[mTMDModel.TMDObjectList[iCountObj].nPrimitives];

                mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket =
                        new TMD_PRIMITIVE_PACKET[mTMDModel.TMDObjectList[iCountObj].nPrimitives];

                fileBufferPosItems = mTMDModel.TMDObjectList[iCountObj].offsetPrimitives + TMD_PADDING;

                for (iCountItem = 0; iCountItem < mTMDModel.TMDObjectList[iCountObj].nPrimitives; iCountItem++)
                {
                    // Read Primitive
                    ReadTMDPrimitive(fileBuffer, ref fileBufferPosItems,
                                     ref mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveList[iCountItem]);

                    // Read Primitive Packet
                    ReadTMDPrimitivePacket(fileBuffer, ref fileBufferPosItems, 
                                           mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveList[iCountItem].mode,
                                           ref mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountItem]);
                }


                // Read Vertices
                mTMDModel.TMDObjectList[iCountObj].TMDVertexList =
                        new TMD_VERTEX[mTMDModel.TMDObjectList[iCountObj].nVerts];

                fileBufferPosItems = mTMDModel.TMDObjectList[iCountObj].offsetVerts + TMD_PADDING;

                ReadTMDVertices(fileBuffer, fileBufferPosItems, mTMDModel.TMDObjectList[iCountObj].nVerts,                                         
                                ref mTMDModel.TMDObjectList[iCountObj].TMDVertexList);


                // Read Normals
                mTMDModel.TMDObjectList[iCountObj].TMDNormalList =
                        new TMD_NORMAL[mTMDModel.TMDObjectList[iCountObj].nNormals];

                fileBufferPosItems = mTMDModel.TMDObjectList[iCountObj].offsetNormals + TMD_PADDING;

                ReadTMDNormals(fileBuffer, fileBufferPosItems, mTMDModel.TMDObjectList[iCountObj].nNormals,
                               ref mTMDModel.TMDObjectList[iCountObj].TMDNormalList);
            }

        }



        /////////////////////////////////////////////////////////////////////////////////////////////
        // Load TMD functions
        public static int ReadTMDHeader(byte[] fileBuffer, ref long pos, ref TMD_HEADER TMDHeader, string fileName)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    TMDHeader.version = memReader.ReadInt32();

                    // Let's use for our own purposes the version variable:
                    // 0x41 - vanilla model (with vertex coordinates as shorts)
                    // 0xFF - converted 2 float model (with vertex coordinates as floats)
                    if (TMDHeader.version != 0x41 && TMDHeader.version != 0xFF)
                    {
                        MessageBox.Show("The file header of the TMD file " + fileName + " is not correct.",
                                        "Error");
                        return 0;
                    }

                    if (TMDHeader.version == 0xFF) bConverted2Float = true;
                    else bConverted2Float = false;

                    TMDHeader.flags = memReader.ReadInt32();
                    TMDHeader.nObjects = memReader.ReadInt32();

                    pos = memReader.BaseStream.Position;
                }
            }

            return 1;
        }

        public static void ReadTMDObject(byte[] fileBuffer, ref long pos, ref TMD_OBJECT TMDObject)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    TMDObject.offsetVerts = memReader.ReadInt32();
                    TMDObject.nVerts = memReader.ReadInt32();
                    TMDObject.offsetNormals = memReader.ReadInt32();
                    TMDObject.nNormals = memReader.ReadInt32();
                    TMDObject.offsetPrimitives = memReader.ReadInt32();
                    TMDObject.nPrimitives = memReader.ReadInt32();
                    TMDObject.scale = memReader.ReadInt32();

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadTMDPrimitive(byte[] fileBuffer, ref long pos, 
                                            ref TMD_PRIMITIVE_HEADER TMDPrimitive)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    TMDPrimitive.olen = memReader.ReadByte();
                    TMDPrimitive.ilen = memReader.ReadByte();
                    TMDPrimitive.flag = memReader.ReadByte();
                    TMDPrimitive.mode = memReader.ReadByte();

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadTMDPrimitivePacket(byte[] fileBuffer, ref long pos, int mode,
                                                  ref TMD_PRIMITIVE_PACKET TMDPrimitivePacket)
        {
            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    switch (mode)
                    {
                        case 0x25:
                            TMDPrimitivePacket.tmd3txnsfp = new TMD_3_TX_NS_FP()
                            {
                                U0 = memReader.ReadByte(),
                                V0 = memReader.ReadByte(),
                                CBA = memReader.ReadUInt16(),
                                U1 = memReader.ReadByte(),
                                V1 = memReader.ReadByte(),
                                TSB = memReader.ReadUInt16(),
                                U2 = memReader.ReadByte(),
                                V2 = memReader.ReadByte(),
                                pad1 = memReader.ReadUInt16(),
                                R = memReader.ReadByte(),
                                G = memReader.ReadByte(),
                                B = memReader.ReadByte(),
                                pad2 = memReader.ReadByte(),
                                Vertex0 = memReader.ReadUInt16(),
                                Vertex1 = memReader.ReadUInt16(),
                                Vertex2 = memReader.ReadUInt16(),
                                pad = memReader.ReadUInt16(),
                            };

                            break;

                        case 0x31:
                            TMDPrimitivePacket.tmd3nsgp = new TMD_3_NS_GP()
                            {
                                R0 = memReader.ReadByte(),
                                G0 = memReader.ReadByte(),
                                B0 = memReader.ReadByte(),
                                mode2 = memReader.ReadByte(),
                                R1 = memReader.ReadByte(),
                                G1 = memReader.ReadByte(),
                                B1 = memReader.ReadByte(),
                                pad1 = memReader.ReadByte(),
                                R2 = memReader.ReadByte(),
                                G2 = memReader.ReadByte(),
                                B2 = memReader.ReadByte(),
                                pad2 = memReader.ReadByte(),
                                Vertex0 = memReader.ReadUInt16(),
                                Vertex1 = memReader.ReadUInt16(),
                                Vertex2 = memReader.ReadUInt16(),
                                pad = memReader.ReadUInt16(),
                            };

                            break;

                        default:

                            break;
                    }

                    pos = memReader.BaseStream.Position;
                }
            }
        }

        public static void ReadTMDVertices(byte[] fileBuffer, long pos, int numVerts,
                                           ref TMD_VERTEX[] TMDVertexList)
        {
            int i;

            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    if (bConverted2Float)
                    {
                        for (i = 0; i < numVerts; i++)
                        {
                            TMDVertexList[i].fvx = memReader.ReadSingle();
                            TMDVertexList[i].fvy = memReader.ReadSingle();
                            TMDVertexList[i].fvz = memReader.ReadSingle();
                            TMDVertexList[i].fpad = memReader.ReadSingle();

                            TMDVertexList[i].vx = (short)TMDVertexList[i].fvx;
                            TMDVertexList[i].vy = (short)TMDVertexList[i].fvy;
                            TMDVertexList[i].vz = (short)TMDVertexList[i].fvz;
                            TMDVertexList[i].pad = (short)TMDVertexList[i].fpad;
                        }
                    }
                    else
                    {
                        for (i = 0; i < numVerts; i++)
                        {
                            TMDVertexList[i].vx = memReader.ReadInt16();
                            TMDVertexList[i].vy = memReader.ReadInt16();
                            TMDVertexList[i].vz = memReader.ReadInt16();
                            TMDVertexList[i].pad = memReader.ReadInt16();

                            TMDVertexList[i].fvx = (float)Math.Round((double)TMDVertexList[i].vx, 6);
                            TMDVertexList[i].fvy = (float)Math.Round((double)TMDVertexList[i].vy, 6);
                            TMDVertexList[i].fvz = (float)Math.Round((double)TMDVertexList[i].vz, 6);
                            TMDVertexList[i].fpad = (float)Math.Round((double)TMDVertexList[i].pad, 6);
                        }
                    }
                }
            }
        }

        public static void ReadTMDNormals(byte[] fileBuffer, long pos, int numNormals,
                                          ref TMD_NORMAL[] TMDNormalList)
        {
            int i;

            if (numNormals == 0) return;

            using (var fileMemory = new MemoryStream(fileBuffer))
            {
                using (var memReader = new BinaryReader(fileMemory))
                {
                    memReader.BaseStream.Position = pos;

                    for (i = 0; i < numNormals; i++)
                    {
                        TMDNormalList[i].nx = memReader.ReadInt16();
                        TMDNormalList[i].ny = memReader.ReadInt16();
                        TMDNormalList[i].nz = memReader.ReadInt16();
                        TMDNormalList[i].pad = memReader.ReadInt16();
                    }
                }
            }
        }



        //  -------------------------------------------------------------------------------------------------
        //  ===================================== CONVERSION FUNCTIONS ======================================
        //  -------------------------------------------------------------------------------------------------
        private static int FindVertexIdxByColorVArray(byte inR, byte inG, byte inB, Color[] vcolorsV,
                                                      short inX, short inY, short inZ,
                                                      Point3D[] vertsV, int mode)
        {
            int iVertexIdx = -1, iCountColor;
            bool bFound = false;

            // We have to find the vertex + RGB Index
            iCountColor = 0;
            while (iCountColor < vcolorsV.Length && !bFound)
            {
                switch (mode)
                {
                    case 0x31:
                        if (inR == vcolorsV[iCountColor].R &&
                            inG == vcolorsV[iCountColor].G &&
                            inB == vcolorsV[iCountColor].B &&
                            vertsV[iCountColor].x == inX &&
                            vertsV[iCountColor].y == inY &&
                            vertsV[iCountColor].z == inZ)
                        {
                            bFound = true;
                            iVertexIdx = iCountColor;
                        }
                        else iCountColor++;

                        break;

                    default:

                        break;
                }
            }

            return iVertexIdx;
        }

        private static int FindVertexIdxByColorVArrayf(byte inR, byte inG, byte inB, Color[] vcolorsV,
                                                       float inX, float inY, float inZ,
                                                       Point3D[] vertsV, int mode)
        {
            int iVertexIdx = -1, iCountColor;
            bool bFound = false;

            // We have to find the vertex + RGB Index
            iCountColor = 0;
            while (iCountColor < vcolorsV.Length && !bFound)
            {
                switch (mode)
                {
                    case 0x31:
                        if (inR == vcolorsV[iCountColor].R &&
                            inG == vcolorsV[iCountColor].G &&
                            inB == vcolorsV[iCountColor].B &&
                            vertsV[iCountColor].x == inX &&
                            vertsV[iCountColor].y == inY &&
                            vertsV[iCountColor].z == inZ)
                        {
                            bFound = true;
                            iVertexIdx = iCountColor;
                        }
                        else iCountColor++;

                        break;

                    default:

                        break;
                }
            }

            return iVertexIdx;
        }

        public static bool TMDHasTextureUVs(TMD_PRIMITIVE_HEADER inPrimHdr)
        {
            bool bHasTexture = false;

            switch (inPrimHdr.mode)
            {
                case 0x25:
                    bHasTexture = true;
                    break;
            }

            return bHasTexture;
        }

        private static void PopulatePModel(TMD_VERTEX[] TMDVertices,
                                           TMD_PRIMITIVE_HEADER[] TMDPrimitiveHeaders,
                                           TMD_PRIMITIVE_PACKET[] TMDPrimitivePackets,
                                           out Point3D[] vertsV, out PPolygon[] facesV,
                                           out Color[] vcolorsV, out Color[] pcolorsV, 
                                           out Point2D[] TexCoords)
        {
            int iPolyIdx, iVertIdx, iVColorIdx, iFoundColorVArray;
            ushort usVertex;

            // Let's populate vertices
            vertsV = new Point3D[TMDVertices.Length];
            vcolorsV = new Color[TMDVertices.Length];

            if (bConverted2Float)
            {
                for (iVertIdx = 0; iVertIdx < TMDVertices.Length; iVertIdx++)
                {
                    vertsV[iVertIdx].x = TMDVertices[iVertIdx].fvx;
                    vertsV[iVertIdx].y = TMDVertices[iVertIdx].fvy;
                    vertsV[iVertIdx].z = TMDVertices[iVertIdx].fvz;
                }
            }
            else
            {
                for (iVertIdx = 0; iVertIdx < TMDVertices.Length; iVertIdx++)
                {
                    vertsV[iVertIdx].x = TMDVertices[iVertIdx].vx;
                    vertsV[iVertIdx].y = TMDVertices[iVertIdx].vy;
                    vertsV[iVertIdx].z = TMDVertices[iVertIdx].vz;
                }
            }


            for (iVColorIdx = 0; iVColorIdx < TMDVertices.Length; iVColorIdx++)
                vcolorsV[iVColorIdx] = Color.FromArgb(128, 255, 255, 255);


            // Let's populate faces
            facesV = new PPolygon[TMDPrimitiveHeaders.Length];
            pcolorsV = new Color[TMDPrimitiveHeaders.Length];


            // Let's prepare for TextureCoordinates if it has any
            if (TMDHasTextureUVs(TMDPrimitiveHeaders[0]))
            {
                TexCoords = new Point2D[TMDVertices.Length];
            }
            else TexCoords = null;


            for (iPolyIdx = 0; iPolyIdx < TMDPrimitiveHeaders.Length; iPolyIdx++)
            {
                facesV[iPolyIdx].tag1 = 0;
                facesV[iPolyIdx].tag2 = PPOLY_TAG2;

                // Prepare PPolygon
                facesV[iPolyIdx].Verts = new ushort[3];
                facesV[iPolyIdx].Normals = new ushort[3];
                facesV[iPolyIdx].Edges = new ushort[3];

                switch (TMDPrimitiveHeaders[iPolyIdx].mode)
                {
                    case 0x25:          // Texturized
                        facesV[iPolyIdx].Verts[0] = 
                            (ushort)TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex0;
                        facesV[iPolyIdx].Verts[1] = 
                            (ushort)TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex1;
                        facesV[iPolyIdx].Verts[2] = 
                            (ushort)TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex2;

                        pcolorsV[iPolyIdx] = Color.FromArgb(255,
                                                      TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.R,
                                                      TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.G,
                                                      TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.B);

                        vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex0] =
                            Color.FromArgb(255,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.R,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.G,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.B);
                        vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex1] =
                            Color.FromArgb(255,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.R,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.G,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.B);
                        vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex2] =
                            Color.FromArgb(255,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.R,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.G,
                                           TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.B);

                        TexCoords[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex0].x =
                            TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.U0 / 64f;
                        TexCoords[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex0].y =
                            TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.V0 / 64f;

                        TexCoords[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex1].x =
                            TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.U1 / 64f;
                        TexCoords[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex1].y =
                            TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.V1 / 64f;

                        TexCoords[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex2].x =
                            TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.U2 / 64f;
                        TexCoords[TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.Vertex2].y =
                            TMDPrimitivePackets[iPolyIdx].tmd3txnsfp.V2 / 64f;

                        break;

                    case 0x31:          // Vertex colored
                        // Vertex 0
                        // Check if exists the RGB color in vcolorsV array

                        if (vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].A == 128)
                        {
                            usVertex = TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0;

                            vcolorsV[usVertex] = Color.FromArgb(255,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0);
                        }
                        else
                        {
                            if (vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].R == 
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0 &&
                                vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].G == 
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0 &&
                                vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].B == 
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0)
                            {
                                usVertex = TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0;
                            }
                            else
                            {
                                if (bConverted2Float)
                                {
                                    iFoundColorVArray =
                                        FindVertexIdxByColorVArrayf(TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0,
                                                                    vcolorsV,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].fvx,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].fvy,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].fvz,
                                                                    vertsV,
                                                                    TMDPrimitiveHeaders[iPolyIdx].mode);
                                }
                                else
                                {
                                    iFoundColorVArray =
                                        FindVertexIdxByColorVArray(TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                                   TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                                   TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0,
                                                                   vcolorsV,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].vx,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].vy,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].vz,
                                                                   vertsV,
                                                                   TMDPrimitiveHeaders[iPolyIdx].mode);
                                }


                                if (iFoundColorVArray == -1)
                                {
                                    // We need to add a new vertex/colorV and reassign the poly vertex index of the poly.
                                    usVertex = (ushort)vertsV.Length;

                                    // Add vertex to P Model vertex array
                                    Array.Resize(ref vertsV, vertsV.Length + 1);

                                    if (bConverted2Float)
                                    {
                                        vertsV[vertsV.Length - 1].x =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].fvx;
                                        vertsV[vertsV.Length - 1].y =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].fvy;
                                        vertsV[vertsV.Length - 1].z =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].fvz;
                                    }
                                    else
                                    {
                                        vertsV[vertsV.Length - 1].x =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].vx;
                                        vertsV[vertsV.Length - 1].y =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].vy;
                                        vertsV[vertsV.Length - 1].z =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex0].vz;
                                    }

                                    // Add color to P Model vertices color array
                                    Array.Resize(ref vcolorsV, vcolorsV.Length + 1);
                                    vcolorsV[vcolorsV.Length - 1] = Color.FromArgb(255,
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0);
                                }
                                else
                                {
                                    usVertex = (ushort)iFoundColorVArray;
                                }
                            }
                        }

                        facesV[iPolyIdx].Verts[0] = usVertex;


                        // Vertex 1
                        // Check if exists the RGB color in vcolorsV array
                        if (vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].A == 128)
                        {
                            usVertex = TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1;

                            vcolorsV[usVertex] = Color.FromArgb(255,
                                                        TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R1,
                                                        TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G1,
                                                        TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B1);
                        }
                        else
                        {
                            if (vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].R == 
                                                        TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R1 &&
                                vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].G == 
                                                        TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G1 &&
                                vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].B == 
                                                        TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B1)
                            {
                                usVertex = TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1;
                            }
                            else
                            {
                                if (bConverted2Float)
                                {
                                    iFoundColorVArray =
                                        FindVertexIdxByColorVArrayf(TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0,
                                                                    vcolorsV,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].fvx,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].fvy,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].fvz,
                                                                    vertsV,
                                                                    TMDPrimitiveHeaders[iPolyIdx].mode);
                                }
                                else
                                {
                                    iFoundColorVArray =
                                        FindVertexIdxByColorVArray(TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                                   TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                                   TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0,
                                                                   vcolorsV,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].vx,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].vy,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].vz,
                                                                   vertsV,
                                                                   TMDPrimitiveHeaders[iPolyIdx].mode);
                                }


                                if (iFoundColorVArray == -1)
                                {
                                    // We need to add a new vertex/colorV and reassign the poly vertex index of the poly.
                                    usVertex = (ushort)vertsV.Length;

                                    // Add vertex to P Model vertex array
                                    Array.Resize(ref vertsV, vertsV.Length + 1);
                                    if (bConverted2Float)
                                    {
                                        vertsV[vertsV.Length - 1].x =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].fvx;
                                        vertsV[vertsV.Length - 1].y =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].fvy;
                                        vertsV[vertsV.Length - 1].z =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].fvz;
                                    }
                                    else
                                    {
                                        vertsV[vertsV.Length - 1].x =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].vx;
                                        vertsV[vertsV.Length - 1].y =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].vy;
                                        vertsV[vertsV.Length - 1].z =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex1].vz;
                                    }


                                    // Add color to P Model vertices color array
                                    Array.Resize(ref vcolorsV, vcolorsV.Length + 1);
                                    vcolorsV[vcolorsV.Length - 1] = Color.FromArgb(255,
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R1,
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G1,
                                                            TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B1);
                                }
                                else
                                {
                                    usVertex = (ushort)iFoundColorVArray;
                                }
                            }
                        }

                        facesV[iPolyIdx].Verts[1] = usVertex;


                        // Vertex 2
                        // Check if exists the RGB color in vcolorsV array
                        if (vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].A == 128)
                        {
                            usVertex = TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2;

                            vcolorsV[usVertex] = Color.FromArgb(255,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R2,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G2,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B2);
                        }
                        else
                        {
                            if (vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].R == 
                                                         TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R2 &&
                                vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].G == 
                                                         TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G2 &&
                                vcolorsV[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].B == 
                                                         TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B2)
                            {
                                usVertex = TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2;
                            }
                            else
                            {
                                if (bConverted2Float)
                                {
                                    iFoundColorVArray =
                                        FindVertexIdxByColorVArrayf(TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0,
                                                                    vcolorsV,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].fvx,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].fvy,
                                                                    TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].fvz,
                                                                    vertsV,
                                                                    TMDPrimitiveHeaders[iPolyIdx].mode);
                                }
                                else
                                {
                                    iFoundColorVArray =
                                        FindVertexIdxByColorVArray(TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0,
                                                                   TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0,
                                                                   TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0,
                                                                   vcolorsV,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].vx,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].vy,
                                                                   TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].vz,
                                                                   vertsV,
                                                                   TMDPrimitiveHeaders[iPolyIdx].mode);
                                }

                                if (iFoundColorVArray == -1)
                                {
                                    // We need to add a new vertex/colorV and reassign the poly vertex index of the poly.
                                    usVertex = (ushort)vertsV.Length;

                                    // Add vertex to P Model vertex array
                                    Array.Resize(ref vertsV, vertsV.Length + 1);
                                    if (bConverted2Float)
                                    {
                                        vertsV[vertsV.Length - 1].x =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].fvx;
                                        vertsV[vertsV.Length - 1].y =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].fvy;
                                        vertsV[vertsV.Length - 1].z =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].fvz;
                                    }
                                    else
                                    {
                                        vertsV[vertsV.Length - 1].x =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].vx;
                                        vertsV[vertsV.Length - 1].y =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].vy;
                                        vertsV[vertsV.Length - 1].z =
                                            TMDVertices[TMDPrimitivePackets[iPolyIdx].tmd3nsgp.Vertex2].vz;
                                    }


                                    // Add color to P Model vertices color array
                                    Array.Resize(ref vcolorsV, vcolorsV.Length + 1);
                                    vcolorsV[vcolorsV.Length - 1] = Color.FromArgb(255,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R2,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G2,
                                                    TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B2);
                                }
                                else
                                {
                                    usVertex = (ushort)iFoundColorVArray;
                                }
                            }
                        }

                        facesV[iPolyIdx].Verts[2] = usVertex;

                        pcolorsV[iPolyIdx] = Color.FromArgb(255,
                                                      (TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R0 + 
                                                       TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R1 + 
                                                       TMDPrimitivePackets[iPolyIdx].tmd3nsgp.R2) / 3,
                                                      (TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G0 + 
                                                       TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G1 + 
                                                       TMDPrimitivePackets[iPolyIdx].tmd3nsgp.G2) / 3,
                                                      (TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B0 + 
                                                       TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B1 + 
                                                       TMDPrimitivePackets[iPolyIdx].tmd3nsgp.B2) / 3);


                        break;

                    default:

                        break;
                }
            }
        }

        public static void ConvertTMD2PModel(ref PModel outPModel, TMDModel inTMDModel, int iModelIdx)
        {
            outPModel.Header.version = 1;
            outPModel.Header.off04 = 1;

            // Instantiate Header.unknown (in VB6 this is done directly, here we need to do it manually or when creating the PModel.
            // In the other P Models (Field, Battle or Magic) we will have this unknown data, but not when converting a 3DS to PModel.s
            outPModel.Header.unknown = new int[16];

            PopulatePModel(inTMDModel.TMDObjectList[iModelIdx].TMDVertexList,
                           inTMDModel.TMDObjectList[iModelIdx].TMDPrimitiveList,
                           inTMDModel.TMDObjectList[iModelIdx].TMDPrimitiveListPacket,
                           out Point3D[] vertsV, out PPolygon[] facesV, out Color[] vcolorsV, 
                           out Color[] pcolorsV, out Point2D[]  texcoordsV);

            //texcoordsV = null;
            //texcoordsV = new Point2D[vertsV.Length];
            //GetTexCoords(mesh, out texcoordsV);

            AddGroup(ref outPModel, vertsV, facesV, texcoordsV, vcolorsV, pcolorsV, 0);          

            // Kimera own working vars of the model
            outPModel.resizeX = 1;
            outPModel.resizeY = 1;
            outPModel.resizeZ = 1;
            outPModel.repositionX = 0;
            outPModel.repositionY = 0;
            outPModel.repositionZ = 0;
            outPModel.rotateAlpha = 0;
            outPModel.rotateBeta = 0;
            outPModel.rotateGamma = 0;
            outPModel.rotationQuaternion.x = 0;
            outPModel.rotationQuaternion.y = 0;
            outPModel.rotationQuaternion.z = 0;
            outPModel.rotationQuaternion.w = 1;

            ComputeNormals(ref outPModel);
            ComputeBoundingBox(ref outPModel);
            ComputeEdges(ref outPModel);
        }

        public static int IsVertexDuplicated(Point3D tmpP3D, TMD_VERTEX[] inVertex)
        {
            bool bIsVertexDuplicated = false;
            int iCounter = 0;

            if (bConverted2Float)
            {
                while (iCounter < inVertex.Length && !bIsVertexDuplicated)
                {
                    if (inVertex[iCounter].fvx == tmpP3D.x &&
                        inVertex[iCounter].fvy == tmpP3D.y &&
                        inVertex[iCounter].fvz == tmpP3D.z)
                        bIsVertexDuplicated = true;
                    else iCounter++;
                }
            }
            else
            {
                while (iCounter < inVertex.Length && !bIsVertexDuplicated)
                {
                    if (inVertex[iCounter].vx == (short)Math.Round(tmpP3D.x) &&
                        inVertex[iCounter].vy == (short)Math.Round(tmpP3D.y) &&
                        inVertex[iCounter].vz == (short)Math.Round(tmpP3D.z))
                        bIsVertexDuplicated = true;
                    else iCounter++;
                }
            }

            if (!bIsVertexDuplicated) iCounter = -1;
            return iCounter;
        }

        public static ushort GetVertexIdx(TMD_VERTEX[] inVertex, Point3D inP3D)
        {
            ushort iGetVertexIndexResult = 0;
            bool bFound = false;

            if (bConverted2Float)
            {
                while (iGetVertexIndexResult < inVertex.Length && !bFound)
                {
                    if (inVertex[iGetVertexIndexResult].fvx == inP3D.x &&
                        inVertex[iGetVertexIndexResult].fvy == inP3D.y &&
                        inVertex[iGetVertexIndexResult].fvz == inP3D.z)
                        bFound = true;
                    else iGetVertexIndexResult++;
                }
            }
            else
            {
                while (iGetVertexIndexResult < inVertex.Length && !bFound)
                {
                    if (inVertex[iGetVertexIndexResult].vx == (short)Math.Round(inP3D.x) &&
                        inVertex[iGetVertexIndexResult].vy == (short)Math.Round(inP3D.y) &&
                        inVertex[iGetVertexIndexResult].vz == (short)Math.Round(inP3D.z))
                        bFound = true;
                    else iGetVertexIndexResult++;
                }
            }


            // This should not happen
            if (iGetVertexIndexResult >= inVertex.Length)
                MessageBox.Show("This should not happen. The Vertex searched is not found!", "Info");

            return iGetVertexIndexResult;
        }


        public static void ConvertPModel2TMD(PModel inPModel, int iModelIdx)
        {
            int iCounter, iSizeAccum, iVertexIndexDuplicated;

            TMD_OBJECT newTMDObj = new TMD_OBJECT();

            // Before create the TMD Object, as it does not uses Groups,
            // we need to merge all the groups of the PModel we want to convert into
            // one unique Group if needed.
            if (inPModel.Header.numGroups > 1)
            {
                MergeGroupsIntoOne(inPModel, out PModel tmpPModel, true);

                inPModel = tmpPModel;
            }

            // First we will create the new TMD Object in a new struct.
            newTMDObj.offsetVerts = 0;
            newTMDObj.nVerts = 0;

            newTMDObj.offsetNormals = 0;
            newTMDObj.nNormals = 0;

            newTMDObj.offsetPrimitives = 0;          
            newTMDObj.nPrimitives = inPModel.Polys.Length;

            newTMDObj.scale = 0;

            // Create Vertex List
            // We need to check if there are duplicates
            iCounter = 0;

            newTMDObj.TMDVertexList = new TMD_VERTEX[0];
            foreach (Point3D itmP3D in inPModel.Verts)
            {
                iVertexIndexDuplicated = IsVertexDuplicated(itmP3D, newTMDObj.TMDVertexList);

                if (iVertexIndexDuplicated == -1)
                {
                    Array.Resize(ref newTMDObj.TMDVertexList, newTMDObj.TMDVertexList.Length + 1);

                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].vx = (short)Math.Round(itmP3D.x);
                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].vy = (short)Math.Round(itmP3D.y);
                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].vz = (short)Math.Round(itmP3D.z);                   
                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].pad = 0;

                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].fvx = itmP3D.x;
                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].fvy = itmP3D.y;
                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].fvz = itmP3D.z;
                    newTMDObj.TMDVertexList[newTMDObj.TMDVertexList.Length - 1].fpad = 0;
                }
                //else
                //{
                //    // We need to update in Polys list this vertex index
                //    for (int i = 0; i < inPModel.Polys.Length; i++)
                //    {
                //        if (inPModel.Polys[i].Verts[0] == iCounter)
                //            inPModel.Polys[i].Verts[0] = (short)iVertexIndexDuplicated;

                //        if (inPModel.Polys[i].Verts[1] == iCounter)
                //            inPModel.Polys[i].Verts[1] = (short)iVertexIndexDuplicated;

                //        if (inPModel.Polys[i].Verts[2] == iCounter)
                //            inPModel.Polys[i].Verts[2] = (short)iVertexIndexDuplicated;
                //    }
                //}

                iCounter++;
            }
            // Finally we will resize the vertex array to the max counted
            newTMDObj.nVerts = newTMDObj.TMDVertexList.Length;

            // Create Normal List           Normally the models (even texturized) has not normal vectors.
            newTMDObj.TMDNormalList = new TMD_NORMAL[0];

            // Create Primitive List
            iCounter = 0;

            newTMDObj.TMDPrimitiveList = new TMD_PRIMITIVE_HEADER[newTMDObj.nPrimitives];
            newTMDObj.TMDPrimitiveListPacket = new TMD_PRIMITIVE_PACKET[newTMDObj.nPrimitives];
            foreach (PPolygon itmPPoly in inPModel.Polys)
            {
                switch (inPModel.Groups[0].texFlag)
                {
                    case 0:
                        // It HAS NOT texture (mode 0x31)
                        newTMDObj.TMDPrimitiveList[iCounter].olen = 0x06;
                        newTMDObj.TMDPrimitiveList[iCounter].ilen = 0x05;
                        newTMDObj.TMDPrimitiveList[iCounter].flag = 0x01;
                        newTMDObj.TMDPrimitiveList[iCounter].mode = 0x31;

                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.R0 = inPModel.Vcolors[itmPPoly.Verts[0]].R;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.G0 = inPModel.Vcolors[itmPPoly.Verts[0]].G;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.B0 = inPModel.Vcolors[itmPPoly.Verts[0]].B;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.mode2 = 0x31;

                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.R1 = inPModel.Vcolors[itmPPoly.Verts[1]].R;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.G1 = inPModel.Vcolors[itmPPoly.Verts[1]].G;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.B1 = inPModel.Vcolors[itmPPoly.Verts[1]].B;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.pad1 = 0;

                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.R2 = inPModel.Vcolors[itmPPoly.Verts[2]].R;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.G2 = inPModel.Vcolors[itmPPoly.Verts[2]].G;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.B2 = inPModel.Vcolors[itmPPoly.Verts[2]].B;
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.pad2 = 0;


                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.Vertex0 = GetVertexIdx(newTMDObj.TMDVertexList,
                                                                                                   inPModel.Verts[itmPPoly.Verts[0]]);
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.Vertex1 = GetVertexIdx(newTMDObj.TMDVertexList,
                                                                                                   inPModel.Verts[itmPPoly.Verts[1]]);
                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.Vertex2 = GetVertexIdx(newTMDObj.TMDVertexList,
                                                                                                   inPModel.Verts[itmPPoly.Verts[2]]);

                        newTMDObj.TMDPrimitiveListPacket[iCounter].tmd3nsgp.pad = 0;

                        break;

                    case 1:
                        // It HAS texture. (mode 0x25)
                        newTMDObj.TMDPrimitiveList[iCounter].olen = 0x07;
                        newTMDObj.TMDPrimitiveList[iCounter].ilen = 0x06;
                        newTMDObj.TMDPrimitiveList[iCounter].flag = 0x01;
                        newTMDObj.TMDPrimitiveList[iCounter].mode = 0x25;

                        break;

                    default:

                        break;
                }

                iCounter++;
            }

            // Now we have to assign the new TMD Object to the TMD Object List
            mTMDModel.TMDObjectList[iModelIdx] = newTMDObj;

            // Finally we recalculate offsets with the new data.
            // Recalculate Primitives offsets
            iSizeAccum = mTMDModel.TMDHeader.nObjects * 28;      // Offset for: Objects Table

            for (iCounter = 0; iCounter < mTMDModel.TMDHeader.nObjects; iCounter++)
            {
                mTMDModel.TMDObjectList[iCounter].offsetPrimitives = iSizeAccum;

                // Accumulate the Primitive Header + Packet sizes for next Primitive
                iSizeAccum += mTMDModel.TMDObjectList[iCounter].TMDPrimitiveList.Length * 4;

                switch (mTMDModel.TMDObjectList[iCounter].TMDPrimitiveList[0].mode)
                {
                    case 0x25:
                        iSizeAccum += mTMDModel.TMDObjectList[iCounter].TMDPrimitiveListPacket.Length * 24;
                        break;

                    case 0x31:
                        iSizeAccum += mTMDModel.TMDObjectList[iCounter].TMDPrimitiveListPacket.Length * 20;
                        break;
                }

            }

            // Recalculate Vertex offsets
            for (iCounter = 0; iCounter < mTMDModel.TMDHeader.nObjects; iCounter++)
            {
                mTMDModel.TMDObjectList[iCounter].offsetVerts = iSizeAccum;

                // Accumulate the Primitive Header + Packet sizes for next Primitive
                if (bConverted2Float)
                    iSizeAccum += mTMDModel.TMDObjectList[iCounter].nVerts * 16;
                else
                    iSizeAccum += mTMDModel.TMDObjectList[iCounter].nVerts * 8;
            }

            // Recalculate Normals offsets
            for (iCounter = 0; iCounter < mTMDModel.TMDHeader.nObjects; iCounter++)
            {
                mTMDModel.TMDObjectList[iCounter].offsetNormals = iSizeAccum;

                // Accumulate the Primitive Header + Packet sizes for next Primitive
                iSizeAccum += mTMDModel.TMDObjectList[iCounter].nNormals * 8;
            }

            // Check vertex winding of the object polys
            MessageBox.Show("The P Model has been converted to TMD Object.", "Info");
        }

        public static void RecalculateOffsets()
        {
            int iCounter, iSizeAccum;

            // Recalculate Primitives offsets
            iSizeAccum = mTMDModel.TMDHeader.nObjects * 28;      // Offset for: Objects Table

            for (iCounter = 0; iCounter < mTMDModel.TMDHeader.nObjects; iCounter++)
            {

                mTMDModel.TMDObjectList[iCounter].offsetPrimitives = iSizeAccum;

                iSizeAccum += mTMDModel.TMDObjectList[iCounter].TMDPrimitiveList.Length * 4;

                switch (mTMDModel.TMDObjectList[iCounter].TMDPrimitiveList[0].mode)
                {
                    case 0x25:
                        iSizeAccum += mTMDModel.TMDObjectList[iCounter].TMDPrimitiveListPacket.Length * 24;
                        break;

                    case 0x31:
                        iSizeAccum += mTMDModel.TMDObjectList[iCounter].TMDPrimitiveListPacket.Length * 20;
                        break;
                }

            }

            // Recalculate Vertex offsets
            for (iCounter = 0; iCounter < mTMDModel.TMDHeader.nObjects; iCounter++)
            {
                mTMDModel.TMDObjectList[iCounter].offsetVerts = iSizeAccum;

                // Accumulate the Primitive Header + Packet sizes for next Primitive
                if (bConverted2Float)
                    iSizeAccum += mTMDModel.TMDObjectList[iCounter].nVerts * 16;
                else
                    iSizeAccum += mTMDModel.TMDObjectList[iCounter].nVerts * 8;
            }

            // Recalculate Normals offsets
            for (iCounter = 0; iCounter < mTMDModel.TMDHeader.nObjects; iCounter++)
            {
                mTMDModel.TMDObjectList[iCounter].offsetNormals = iSizeAccum;

                // Accumulate the Primitive Header + Packet sizes for next Primitive
                iSizeAccum += mTMDModel.TMDObjectList[iCounter].nNormals * 8;
            }
        }



        /////////////////////////////////////////////////////////////////////////////////////////////
        // SAVING TMD
        // Write TMD functions
        public static int WriteTMD(string strFileName)
        {
            int iWriteTMDResult = 1, iCountPrim, iStreamSize = 0;
            byte[] fileBuffer = new byte[5000000];
            MemoryStream memWriteStream;

            try
            {
                using (memWriteStream = new MemoryStream(fileBuffer))
                {
                    using (BinaryWriter binWriter = new BinaryWriter(memWriteStream))
                    {
                        // Write Header
                        binWriter.Write(mTMDModel.TMDHeader.version);
                        binWriter.Write(mTMDModel.TMDHeader.flags);
                        binWriter.Write(mTMDModel.TMDHeader.nObjects);

                        iStreamSize += 12;

                        // Write Object List
                        foreach (TMD_OBJECT itmTMDObj in mTMDModel.TMDObjectList)
                        {
                            binWriter.Write(itmTMDObj.offsetVerts);
                            binWriter.Write(itmTMDObj.nVerts);
                            binWriter.Write(itmTMDObj.offsetNormals);
                            binWriter.Write(itmTMDObj.nNormals);
                            binWriter.Write(itmTMDObj.offsetPrimitives);
                            binWriter.Write(itmTMDObj.nPrimitives);
                            binWriter.Write(itmTMDObj.scale);

                            iStreamSize += 28;
                        }

                        // Write Header & Packet of Primitives List
                        foreach (TMD_OBJECT itmTMDObj in mTMDModel.TMDObjectList)
                        {

                            iCountPrim = 0;

                            while (iCountPrim < itmTMDObj.TMDPrimitiveList.Length)
                            {
                                // Header
                                binWriter.Write(itmTMDObj.TMDPrimitiveList[iCountPrim].olen);
                                binWriter.Write(itmTMDObj.TMDPrimitiveList[iCountPrim].ilen);
                                binWriter.Write(itmTMDObj.TMDPrimitiveList[iCountPrim].flag);
                                binWriter.Write(itmTMDObj.TMDPrimitiveList[iCountPrim].mode);

                                iStreamSize += 4;

                                // Packet
                                switch (itmTMDObj.TMDPrimitiveList[iCountPrim].mode)
                                {
                                    case 0x25:
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.U0);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.V0);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.CBA);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.U1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.V1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.TSB);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.U2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.V2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.pad1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.R);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.G);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.B);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.pad2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.pad);

                                        iStreamSize += 24;
                                        break;

                                    case 0x31:
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.R0);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.G0);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.B0);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.mode2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.R1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.G1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.B1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.pad1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.R2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.G2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.B2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.pad2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2);
                                        binWriter.Write(itmTMDObj.TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.pad);

                                        iStreamSize += 20;
                                        break;

                                    default:

                                        break;
                                }

                                iCountPrim++;
                            }
                        }

                        // Write Vertex List
                        foreach (TMD_OBJECT itmTMDObj in mTMDModel.TMDObjectList)
                        {
                            if (itmTMDObj.TMDVertexList.Length > 0)
                            {
                                if (bConverted2Float)
                                {
                                    foreach (TMD_VERTEX itmTMDVert in itmTMDObj.TMDVertexList)
                                    {
                                        binWriter.Write(itmTMDVert.fvx);
                                        binWriter.Write(itmTMDVert.fvy);
                                        binWriter.Write(itmTMDVert.fvz);
                                        binWriter.Write(itmTMDVert.fpad);

                                        iStreamSize += 16;
                                    }
                                }
                                else
                                {
                                    foreach (TMD_VERTEX itmTMDVert in itmTMDObj.TMDVertexList)
                                    {
                                        binWriter.Write(itmTMDVert.vx);
                                        binWriter.Write(itmTMDVert.vy);
                                        binWriter.Write(itmTMDVert.vz);
                                        binWriter.Write(itmTMDVert.pad);

                                        iStreamSize += 8;
                                    }
                                }
                            }
                        }

                        // Write Normals List if any
                        foreach (TMD_OBJECT itmTMDObj in mTMDModel.TMDObjectList)
                        {
                            if (itmTMDObj.TMDNormalList.Length > 0)
                            {
                                foreach (TMD_NORMAL itmTMDNorm in itmTMDObj.TMDNormalList)
                                {
                                    binWriter.Write(itmTMDNorm.nx);
                                    binWriter.Write(itmTMDNorm.ny);
                                    binWriter.Write(itmTMDNorm.nz);
                                    binWriter.Write(itmTMDNorm.pad);

                                    iStreamSize += 8;
                                }
                            }
                        }
                    }
                }

                Array.Resize(ref fileBuffer, iStreamSize);
                File.WriteAllBytes(strFileName, fileBuffer);

            }
            catch (Exception ex)
            {
                strGlobalExceptionMessage = ex.Message;

                iWriteTMDResult = -1;
            }

            return iWriteTMDResult;
        }



        /////////////////////////////////////////////////////////////////////////////////////////////
        // LOGGING PROCEDURES


        public static void WriteTMDLOG()
        {
            int iCountObj = 0, iCountPrim;
            StringBuilder strTMDLOG = new StringBuilder();

            try
            {
                // Write TMD header
                strTMDLOG.AppendLine("Version: 0x" + String.Format("{0:X}", mTMDModel.TMDHeader.version) + "    " +
                                     "Flag: " + mTMDModel.TMDHeader.flags.ToString() + "    " +
                                     "Num. Objects: " + mTMDModel.TMDHeader.nObjects.ToString("000"));
                strTMDLOG.AppendLine("");

                // For each TMD object we will write each primitive data
                foreach (TMD_OBJECT itmObj in mTMDModel.TMDObjectList)
                {
                    // Write Object info
                    strTMDLOG.AppendLine("──────────────────────────────────────────────────────────────────────");
                    strTMDLOG.AppendLine("Object: " + (iCountObj + 1).ToString("000"));
                    strTMDLOG.AppendLine("──────────────────────────────────────────────────────────────────────");

                    strTMDLOG.AppendLine(String.Format("{0,-19}{1,10}{2,12}{3,-19}{4,4}",
                                         "Offset Vertices:",
                                         "0x" + String.Format("{0:X}", mTMDModel.TMDObjectList[iCountObj].offsetVerts),
                                         "(" + mTMDModel.TMDObjectList[iCountObj].offsetVerts.ToString() + ")",
                                         "   Num. Vertices:",
                                         mTMDModel.TMDObjectList[iCountObj].nVerts.ToString("000")));
                    strTMDLOG.AppendLine(String.Format("{0,-19}{1,10}{2,12}{3,-19}{4,4}",
                                         "Offset Normals:",
                                         "0x" + String.Format("{0:X}", mTMDModel.TMDObjectList[iCountObj].offsetNormals),
                                         "(" + mTMDModel.TMDObjectList[iCountObj].offsetNormals.ToString() + ")",
                                         "   Num. Normals:",
                                         mTMDModel.TMDObjectList[iCountObj].nNormals.ToString("000")));
                    strTMDLOG.AppendLine(String.Format("{0,-19}{1,10}{2,12}{3,-19}{4,4}",
                                         "Offset Primitives:",
                                         "0x" + String.Format("{0:X}", mTMDModel.TMDObjectList[iCountObj].offsetPrimitives),
                                         "(" + mTMDModel.TMDObjectList[iCountObj].offsetPrimitives.ToString() + ")",
                                         "   Num. Primitives:",
                                         mTMDModel.TMDObjectList[iCountObj].nPrimitives.ToString("000")));
                    strTMDLOG.AppendLine("Scale: " + mTMDModel.TMDObjectList[iCountObj].scale);

                    strTMDLOG.AppendLine("");


                    // Write each primitive data
                    iCountPrim = 0;
                    foreach (TMD_PRIMITIVE_HEADER itmPrimHead in mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveList)
                    {
                        strTMDLOG.AppendLine("Primitive: " + (iCountPrim + 1).ToString("000"));
                        strTMDLOG.AppendLine("Mode: " + itmPrimHead.mode.ToString("000") + "   " +
                                             "Flag: " + itmPrimHead.flag.ToString("000") + "   " +
                                             "Ilen: " + itmPrimHead.ilen.ToString("00000") + "   " +
                                             "Olen: " + itmPrimHead.olen.ToString("00000"));

                        if ((itmPrimHead.mode >> 5) == 1)
                            strTMDLOG.AppendLine("Type: Polygon");
                        else if ((itmPrimHead.mode >> 6) == 1)
                            strTMDLOG.AppendLine("Type: Straight Line");
                        else
                            strTMDLOG.AppendLine("Type: Sprite");

                        strTMDLOG.AppendLine("Options(Mode)-> Bright(TGE): " + ((itmPrimHead.mode & (1 << 0)) == 0 ? "Off" : "On") + "   " +
                                             "Translucency(ABE): " + ((itmPrimHead.mode & (1 << 1)) == 0 ? "Off" : "On").ToString() + "   " +
                                             "Texture(TME): " + ((itmPrimHead.mode & (1 << 2)) == 0 ? "No" : "Yes").ToString() + "   " +
                                             "3-4 Sided Polygon: " + ((itmPrimHead.mode & (1 << 3)) == 0 ? "3" : "4").ToString() + "   " +
                                             "Shading Mode(IIP): " + ((itmPrimHead.mode & (1 << 4)) == 0 ? "Off" : "On").ToString());

                        strTMDLOG.AppendLine("Options(Flag)-> Light(LGT): " + ((itmPrimHead.flag & (1 << 0)) == 0 ? "On" : "Off") + "   " +
                                             "1-2 Primitive Faces(FCE): " + ((itmPrimHead.flag & (1 << 1)) == 0 ? "1" : "2").ToString() + "   " +
                                             "Gradient Pigment(GRD): " + ((itmPrimHead.flag & (1 << 2)) == 0 ? "No" : "Yes").ToString());

                        switch (itmPrimHead.mode)
                        {
                            case 0x25:
                                strTMDLOG.AppendLine("CBA: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.CBA.ToString() + "   " +
                                                     "TSB: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.TSB.ToString());

                                strTMDLOG.AppendLine("Options(TSB)-->   Texture Page(TPG): " + (mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.TSB & 0b11111).ToString() + "   " +
                                                     "Semitransparency(ABR): " + (mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.TSB & 0b1100000).ToString() + "   " +
                                                     "Texture Color Mode(TPF): " + (mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.TSB & 0b110000000).ToString());

                                strTMDLOG.AppendLine("R: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.R.ToString("000") + "   " +
                                                     "G: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.G.ToString("000") + "   " +
                                                     "B: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.B.ToString("000"));

                                strTMDLOG.AppendLine("V0: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0.ToString("00000") + "   " +
                                                     "V1: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1.ToString("00000") + "   " +
                                                     "V2: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2.ToString("00000"));

                                if (bConverted2Float)
                                {
                                    strTMDLOG.AppendLine("Vertex0->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0].fvx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0].fvy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0].fvz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex1->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1].fvx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1].fvy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1].fvz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex2->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2].fvx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2].fvy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2].fvz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                }
                                else
                                {
                                    strTMDLOG.AppendLine("Vertex0->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0].vx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0].vy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex0].vz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex1->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1].vx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1].vy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex1].vz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex2->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2].vx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2].vy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.Vertex2].vz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                }

                                strTMDLOG.AppendLine("TexCoord0->   U: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.U0.ToString("00000") + "   " +
                                                     "V: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.V0.ToString("00000"));
                                strTMDLOG.AppendLine("TexCoord1->   U: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.U1.ToString("00000") + "   " +
                                                     "V: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.V1.ToString("00000"));
                                strTMDLOG.AppendLine("TexCoord2->   U: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.U2.ToString("00000") + "   " +
                                                     "V: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3txnsfp.V2.ToString("00000"));

                                break;

                            case 0x31:
                                strTMDLOG.AppendLine("Mode2: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.mode2.ToString("000"));

                                strTMDLOG.AppendLine("R0: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.R0.ToString("000") + "   " +
                                                     "G0: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.G0.ToString("000") + "   " +
                                                     "B0: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.B0.ToString("000"));
                                strTMDLOG.AppendLine("R1: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.R1.ToString("000") + "   " +
                                                     "G1: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.G1.ToString("000") + "   " +
                                                     "B1: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.B1.ToString("000"));
                                strTMDLOG.AppendLine("R2: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.R2.ToString("000") + "   " +
                                                     "G2: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.G2.ToString("000") + "   " +
                                                     "B2: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.B2.ToString("000"));

                                strTMDLOG.AppendLine("V0: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0.ToString("00000") + "   " +
                                                     "V1: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1.ToString("00000") + "   " +
                                                     "V2: " + mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2.ToString("00000"));

                                if (bConverted2Float) 
                                {
                                    strTMDLOG.AppendLine("Vertex0->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0].fvx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0].fvy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0].fvz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex1->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1].fvx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1].fvy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1].fvz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex2->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2].fvx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2].fvy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2].fvz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                }
                                else
                                {
                                    strTMDLOG.AppendLine("Vertex0->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0].vx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0].vy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex0].vz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex1->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1].vx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1].vy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex1].vz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                    strTMDLOG.AppendLine("Vertex2->   X: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2].vx.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Y: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2].vy.ToString(CultureInfo.InvariantCulture.NumberFormat) + "   " +
                                                         "Z: " + mTMDModel.TMDObjectList[iCountObj].TMDVertexList[mTMDModel.TMDObjectList[iCountObj].TMDPrimitiveListPacket[iCountPrim].tmd3nsgp.Vertex2].vz.ToString(CultureInfo.InvariantCulture.NumberFormat));
                                }

                                break;

                            default:

                                break;
                        }


                        strTMDLOG.AppendLine("");
                        iCountPrim++;
                    }

                    strTMDLOG.AppendLine("");
                    iCountObj++;
                }

                File.WriteAllText(strGlobalPathTMDModelFolder + "\\" +
                                  Path.GetFileNameWithoutExtension(strGlobalTMDModelName).ToUpper() + ".log",
                                  strTMDLOG.ToString());

                MessageBox.Show("Log for TMD file " + strGlobalTMDModelName + " saved correctly.", "Info", MessageBoxButtons.OK);
            }
            catch
            {
                MessageBox.Show("Error saving the log for TMD file " + strGlobalTMDModelName + ".", "Error", MessageBoxButtons.OK);
            }
        }




    }
}
