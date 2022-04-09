using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KimeraCS
{

    using Defines;

    using static OpenGL32;
    using static GDI32;

    public class FF7TEXTexture
    {
        public struct TEX
        {
            public string TEXfileName;
            public uint texID;
            public IntPtr HDC;
            public IntPtr HBMP;

            // TEX file format by Mirex and Aali
            // http://wiki.qhimm.com/FF7/TEX_format
            public int version;                // Must be 1 for FF7 to load it
            public int unk1;
            public int ColorKeyFlag;           // Set to 1 to enable the transparent color
            public int unk2;
            public int unk3;
            public int minimumBitsPerColor;    // D3D driver uses these to determine which texture
                                               // format to convert to on load
            public int maximumBitsPerColor;
            public int minimumAlphaBits;
            public int maximumAlphaBits;
            public int minimumBitsPerPixel;
            public int maximumBitsPerPixel;
            public int unk4;
            public int numPalettes;
            public int numColorsPerPalette;
            public int bitDepth;
            public int width;
            public int height;
            public int bytesPerRow;            //  Rarelly used. Usually assumed to be BytesperPixel*Width
            public int unk5;
            public int paletteFlag;
            public int bitsPerIndex;
            public int indexedTo8BitsFlag;     //  Never used in FF7
            public int paletteSize;            //  Must be NumPalletes*NumColorsPerPallete
            public int numColorsPerPalette2;   //  Can be the same or 0. Doesn't really matter
            public int runtimeData;            //  Placeholder for some information. Doesn't matter
            public int bitsPerPixel;
            public int bytesPerPixel;          //  Should be trusted over BitsPerPixel
            ////////////////////////////////////////////////////////////////////////////////////
            //Pixel format (all 0 for palletized images)
            public int numRedBits;
            public int numGreenBits;
            public int numBlueBits;
            public int numAlphaBits;
            public int redBitMask;
            public int greenBitMask;
            public int blueBitMask;
            public int alphaBitMask;
            public int redShift;
            public int greenShift;
            public int blueShift;
            public int alphaShift;
            //  The components values are computed by the following expresion:
            //  (((value & mask) >> shift) * 255) / max
            public int red8;       // Always 8
            public int green8;     // Always 8
            public int blue8;      // Always 8
            public int alpha8;     // Always 8
            public int redMax;
            public int greenMax;
            public int blueMax;
            public int alphaMax;
            //  End of Pixel format
            ////////////////////////////////////////////////////////////////////////////////////
            public int colorKeyArrayFlag;
            public int runtimeData2;
            public int referenceAlpha;
            public int unk6;
            public int unk7;
            public int runtimeDataPaletteIndex;
            public int runtimeData3;
            public int runtimeData4;
            public int unk8;
            public int unk9;
            public int unk10;
            public int unk11;
            public byte[] palette;             //  Always in 32-bit BGRA format
            public byte[] pixelData;           //  Width * Height * BytesPerPixel. Either indices or raw
                                               //  data following the specified format
            public byte[] colorKeyData;        //  NumPalletes * 1 bytes
        }

        public static int ReadTEXTexture(ref TEX inTEXTexture, string inTEXfile)
        {
            int iReadTEXTextureResult = -1;
            byte[] fileBuffer;

            inTEXTexture.texID = 0xFFFFFFFF;

            try
            {
                if (File.Exists(inTEXfile))
                {
                    // Read All TEX Texture file into memory
                    fileBuffer = File.ReadAllBytes(inTEXfile);

                    inTEXTexture.TEXfileName = Path.GetFileName(inTEXfile);

                    using (var fileMemory = new MemoryStream(fileBuffer))
                    {
                        using (var memReader = new BinaryReader(fileMemory))
                        {
                            // Read memory data into structure
                            inTEXTexture.version = memReader.ReadInt32();
                            inTEXTexture.unk1 = memReader.ReadInt32();
                            inTEXTexture.ColorKeyFlag = memReader.ReadInt32();
                            inTEXTexture.unk2 = memReader.ReadInt32();
                            inTEXTexture.unk3 = memReader.ReadInt32();
                            inTEXTexture.minimumBitsPerColor = memReader.ReadInt32();
                            inTEXTexture.maximumBitsPerColor = memReader.ReadInt32(); ;
                            inTEXTexture.minimumAlphaBits = memReader.ReadInt32();
                            inTEXTexture.maximumAlphaBits = memReader.ReadInt32();
                            inTEXTexture.minimumBitsPerPixel = memReader.ReadInt32();
                            inTEXTexture.maximumBitsPerPixel = memReader.ReadInt32();
                            inTEXTexture.unk4 = memReader.ReadInt32();
                            inTEXTexture.numPalettes = memReader.ReadInt32();
                            inTEXTexture.numColorsPerPalette = memReader.ReadInt32();
                            inTEXTexture.bitDepth = memReader.ReadInt32();
                            inTEXTexture.width = memReader.ReadInt32();
                            inTEXTexture.height = memReader.ReadInt32();
                            inTEXTexture.bytesPerRow = memReader.ReadInt32();
                            inTEXTexture.unk5 = memReader.ReadInt32();
                            inTEXTexture.paletteFlag = memReader.ReadInt32();
                            inTEXTexture.bitsPerIndex = memReader.ReadInt32();
                            inTEXTexture.indexedTo8BitsFlag = memReader.ReadInt32();
                            inTEXTexture.paletteSize = memReader.ReadInt32();
                            inTEXTexture.numColorsPerPalette2 = memReader.ReadInt32();
                            inTEXTexture.runtimeData = memReader.ReadInt32();
                            inTEXTexture.bitsPerPixel = memReader.ReadInt32();
                            inTEXTexture.bytesPerPixel = memReader.ReadInt32();
                            inTEXTexture.numRedBits = memReader.ReadInt32();
                            inTEXTexture.numGreenBits = memReader.ReadInt32();
                            inTEXTexture.numBlueBits = memReader.ReadInt32();
                            inTEXTexture.numAlphaBits = memReader.ReadInt32();
                            inTEXTexture.redBitMask = memReader.ReadInt32();
                            inTEXTexture.greenBitMask = memReader.ReadInt32();
                            inTEXTexture.blueBitMask = memReader.ReadInt32();
                            inTEXTexture.alphaBitMask = memReader.ReadInt32();
                            inTEXTexture.redShift = memReader.ReadInt32();
                            inTEXTexture.greenShift = memReader.ReadInt32();
                            inTEXTexture.blueShift = memReader.ReadInt32();
                            inTEXTexture.alphaShift = memReader.ReadInt32();
                            inTEXTexture.red8 = memReader.ReadInt32();
                            inTEXTexture.green8 = memReader.ReadInt32();
                            inTEXTexture.blue8 = memReader.ReadInt32();
                            inTEXTexture.alpha8 = memReader.ReadInt32();
                            inTEXTexture.redMax = memReader.ReadInt32();
                            inTEXTexture.greenMax = memReader.ReadInt32();
                            inTEXTexture.blueMax = memReader.ReadInt32();
                            inTEXTexture.alphaMax = memReader.ReadInt32();
                            inTEXTexture.colorKeyArrayFlag = memReader.ReadInt32();
                            inTEXTexture.runtimeData2 = memReader.ReadInt32();
                            inTEXTexture.referenceAlpha = memReader.ReadInt32();
                            inTEXTexture.unk6 = memReader.ReadInt32();
                            inTEXTexture.unk7 = memReader.ReadInt32();
                            inTEXTexture.runtimeDataPaletteIndex = memReader.ReadInt32();
                            inTEXTexture.runtimeData3 = memReader.ReadInt32();
                            inTEXTexture.runtimeData4 = memReader.ReadInt32();
                            inTEXTexture.unk8 = memReader.ReadInt32();
                            inTEXTexture.unk9 = memReader.ReadInt32();
                            inTEXTexture.unk10 = memReader.ReadInt32();
                            inTEXTexture.unk11 = memReader.ReadInt32();

                            if (inTEXTexture.paletteFlag == 1)
                            {
                                inTEXTexture.palette = new byte[inTEXTexture.paletteSize * 4];
                                inTEXTexture.palette = memReader.ReadBytes(inTEXTexture.paletteSize * 4);
                            }

                            inTEXTexture.pixelData = new byte[inTEXTexture.width * inTEXTexture.height * inTEXTexture.bytesPerPixel];
                            inTEXTexture.pixelData = memReader.ReadBytes(inTEXTexture.width * inTEXTexture.height * inTEXTexture.bytesPerPixel);

                            if (inTEXTexture.colorKeyArrayFlag == 1)
                            {
                                inTEXTexture.colorKeyData = new byte[inTEXTexture.numPalettes];
                                inTEXTexture.colorKeyData = memReader.ReadBytes(inTEXTexture.numPalettes);
                            }
                        }
                    }

                    iReadTEXTextureResult = 0;
                }
                else
                {
                    MessageBox.Show("TEX texture file " + Path.GetFileName(inTEXfile) + " not found.",
                                    "Warning");
                }
            }
            catch
            {
                //  Debug.Print "TEX file not found!!!"
                MessageBox.Show("Error reading TEX texture file " + inTEXfile + ".",
                "Error");
            }

            return iReadTEXTextureResult;
        }

        //  Get a 32 bits BGRA (or BGR for 24 bits) version of the image
        public static void GetTEXTexturev(ref TEX inTEXTexture, ref byte[] textureImg)
        {
            int imageBytesSize, imageSize, offsetBit, ti, color_offset;
            int col16, b1, b2;

            offsetBit = 0;
            ti = 0;

            if (inTEXTexture.paletteFlag == 1)
            {
                textureImg = new byte[inTEXTexture.width * inTEXTexture.height * 4];

                imageBytesSize = inTEXTexture.width * inTEXTexture.height * inTEXTexture.bytesPerPixel;

                while (offsetBit < imageBytesSize)
                {
                    color_offset = 4 * inTEXTexture.pixelData[offsetBit];
                    offsetBit++;

                    textureImg[ti] = inTEXTexture.palette[color_offset];
                    textureImg[ti + 1] = inTEXTexture.palette[color_offset + 1];
                    textureImg[ti + 2] = inTEXTexture.palette[color_offset + 2];

                    if (color_offset == 0 && inTEXTexture.ColorKeyFlag == 1) textureImg[ti + 3] = 0;
                    else textureImg[ti + 3] = 255;

                    ti += 4;
                }
            }
            else 
            {
                if (inTEXTexture.bitDepth == 16)
                {
                    imageSize = 0;

                    textureImg = new byte[inTEXTexture.width * inTEXTexture.height * 4];

                    while (offsetBit < imageSize)
                    {
                        b1 = inTEXTexture.pixelData[offsetBit];
                        b2 = inTEXTexture.pixelData[offsetBit + 1];
                        col16 = (b1 & 255) | ((b2 & 255) * 256);

                        textureImg[ti + 2] = Convert.ToByte((col16 & 31) * 255 / 31);
                        textureImg[ti + 1] = Convert.ToByte(((int)Math.Pow(col16 / 2, 5) & 31) * 255 / 31);
                        textureImg[ti] = Convert.ToByte(((int)Math.Pow(col16 / 2, 10) & 31) * 255 / 31);

                        if (textureImg[ti] == 0 && textureImg[ti + 1] == 0 && textureImg[ti + 2] == 0 && inTEXTexture.ColorKeyFlag == 1)
                            textureImg[ti + 3] = 0;
                        else
                            textureImg[ti + 3] = 255;

                        ti += 4;
                        offsetBit += 2;
                    }
                }
                else textureImg = inTEXTexture.pixelData;
            }
        }

        //  Create the OpenGL Texture object
        public static void LoadTEXTexture(ref TEX inTEXTexture)
        {
            IntPtr hTIPtr;
            byte[] textureImg = null;

            glPixelFormat format = 0x0;
            glInternalFormat internalformat = 0x0;

            glGenTextures(1, ref inTEXTexture.texID);
            glBindTexture(glTextureTarget.GL_TEXTURE_2D, inTEXTexture.texID);

            glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MAG_FILTER, (float)glTextureMagFilter.GL_LINEAR);
            glTexParameterf(glTextureTarget.GL_TEXTURE_2D, glTextureParameter.GL_TEXTURE_MIN_FILTER, (float)glTextureMagFilter.GL_LINEAR);

            switch (inTEXTexture.bitDepth)
            {
                case 1:
                    format = glPixelFormat.GL_BGRA;
                    internalformat = glInternalFormat.GL_RGBA;
                    break;

                case 2:
                    format = glPixelFormat.GL_BGRA;
                    internalformat = glInternalFormat.GL_RGBA;
                    break;

                case 4:
                    format = glPixelFormat.GL_BGRA;
                    internalformat = glInternalFormat.GL_RGBA;
                    break;

                case 8:
                    format = glPixelFormat.GL_BGRA;
                    internalformat = glInternalFormat.GL_RGBA;
                    break;

                case 16:
                    format = glPixelFormat.GL_RGBA;
                    internalformat = glInternalFormat.GL_RGB5;
                    break;

                case 24:
                    format = glPixelFormat.GL_BGR;
                    internalformat = glInternalFormat.GL_RGB;
                    break;

                case 32:
                    format = glPixelFormat.GL_BGRA;
                    internalformat = glInternalFormat.GL_RGBA;
                    break;

            }

            glPixelStorei(glPixelStoreParameter.GL_UNPACK_ALIGNMENT, 1);

            GetTEXTexturev(ref inTEXTexture, ref textureImg);

            // We need to create the IntPtr of textureImg because the glTexImag2D OPENGL function
            //glTexImage2D(glTexture2DProxyTarget.GL_TEXTURE_2D, 0, internalformat,
            //             inTEXTexture.width, inTEXTexture.height, 0, format, glPixelDataType.GL_UNSIGNED_BYTE, textureImg);

            GCHandle pinnedArray = GCHandle.Alloc(textureImg, GCHandleType.Pinned);
            hTIPtr = pinnedArray.AddrOfPinnedObject();

            glTexImage2D(glTexture2DProxyTarget.GL_TEXTURE_2D, 0, internalformat,
                                    inTEXTexture.width, inTEXTexture.height, 0, format, glPixelDataType.GL_UNSIGNED_BYTE, hTIPtr);

            pinnedArray.Free();
        }


        public static void LoadBitmapFromTEXTexture(ref TEX inTEXTexture)
        {
            long li, si, ti, pi;
            byte aux_val;
            long lineLength, lineLengthBytes, linePad, linePadUseful, linePadBytes, line_end;
            short shift, parts, parts_left, i;
            long bmpSizeBytes;

            BitmapInfo pictureInfo = new BitmapInfo();
            byte[] pictureData;

            try
            {
                pictureInfo.bmiHeader.biSize = 40;
                pictureInfo.bmiHeader.biWidth = inTEXTexture.width;
                pictureInfo.bmiHeader.biHeight = inTEXTexture.height;
                pictureInfo.bmiHeader.biPlanes = 1;

                if (inTEXTexture.paletteFlag == 1)
                    pictureInfo.bmiHeader.biBitCount = (BitCount)(byte)(Math.Log(inTEXTexture.paletteSize) / Math.Log(2));
                else
                    pictureInfo.bmiHeader.biBitCount = (BitCount)(byte)inTEXTexture.bitDepth;

                pictureInfo.bmiHeader.biCompression = BitmapCompression.BI_RGB;

                lineLength = pictureInfo.bmiHeader.biWidth * (int)pictureInfo.bmiHeader.biBitCount;

                if (lineLength % 32 == 0) linePad = 0;
                else linePad = 32 * ((lineLength / 32) + 1) - 8 * (lineLength / 8);

                if (linePad == 0) linePadUseful = 0;
                else linePadUseful = lineLength - 8 * (lineLength / 8);

                if (linePad > 0 && linePad < 8) linePadBytes = 1;
                else linePadBytes = linePad / 8;

                lineLengthBytes = lineLength / 8 + linePadBytes;

                bmpSizeBytes = (uint)(lineLengthBytes * pictureInfo.bmiHeader.biHeight);

                pictureInfo.bmiHeader.biSizeImage = (uint)bmpSizeBytes;
                pictureInfo.bmiHeader.biXPelsPerMeter = 0;
                pictureInfo.bmiHeader.biYPelsPerMeter = 0;

                if (inTEXTexture.ColorKeyFlag == 1) pictureInfo.bmiHeader.biClrUsed = (uint)inTEXTexture.paletteSize;
                else pictureInfo.bmiHeader.biClrUsed = 0;

                pictureInfo.bmiHeader.biClrImportant = pictureInfo.bmiHeader.biClrUsed;

                if (pictureInfo.bmiHeader.biBitCount == BitCount.BitPerPixel1BPP ||
                    pictureInfo.bmiHeader.biBitCount == BitCount.BitPerPixel4BPP ||
                    pictureInfo.bmiHeader.biBitCount == BitCount.BitPerPixel8BPP)
                {
                    pictureInfo.bmiColors = new RGBQuad[256];

                    for (i = 0; i < inTEXTexture.paletteSize; i++)
                    {
                        pictureInfo.bmiColors[i].rgbRed = inTEXTexture.palette[(4 * i) + 2];
                        pictureInfo.bmiColors[i].rgbGreen = inTEXTexture.palette[(4 * i) + 1];
                        pictureInfo.bmiColors[i].rgbBlue = inTEXTexture.palette[(4 * i)];
                    }
                }

                pictureData = new byte[bmpSizeBytes];

                if (pictureInfo.bmiHeader.biBitCount == BitCount.BitPerPixel1BPP ||
                    pictureInfo.bmiHeader.biBitCount == BitCount.BitPerPixel4BPP)
                {
                    si = 0;
                    shift = (short)Math.Pow(2, (double)pictureInfo.bmiHeader.biBitCount);
                    parts = (short)(8 / (int)pictureInfo.bmiHeader.biBitCount);
                    parts_left = (short)(linePadUseful / (int)pictureInfo.bmiHeader.biBitCount);

                    for (li = pictureInfo.bmiHeader.biHeight - 2; li >= 0; li--)
                    {
                        line_end = (li + 1) * lineLengthBytes - linePadBytes;

                        for (ti = li * lineLengthBytes; ti < line_end; ti++)
                        {
                            aux_val = 0;

                            for (pi = 0; pi < parts; pi++)
                            {
                                aux_val = (byte)((aux_val * shift) | inTEXTexture.pixelData[si]);
                                si++;
                            }

                            pictureData[ti] = aux_val;
                        }

                        if (linePad > 0)
                        {
                            aux_val = 0;

                            for (pi = 0; pi < parts_left; pi++)
                            {
                                aux_val = (byte)((aux_val * shift) | inTEXTexture.pixelData[si]);
                            }

                            if (parts_left > -1) si = -1;
                            else si = 0;

                            ti = ti + linePadBytes;
                        }
                    }
                }
                else
                {
                    long lineLengthDIV = lineLength / 8;

                    for (li = 0; li < pictureInfo.bmiHeader.biHeight; li++)
                    {
                        for (i = 0; i < lineLengthDIV; i++)
                        {
                            pictureData[((pictureInfo.bmiHeader.biHeight - 1 - li) * lineLengthBytes) + i] =
                                inTEXTexture.pixelData[(li * lineLengthDIV) + i];
                        }
                    }
                }

                IntPtr ppvBits;
                IntPtr sdc = User32.GetDC(IntPtr.Zero);

                inTEXTexture.HDC = CreateCompatibleDC(sdc);
                inTEXTexture.HBMP = CreateDIBSection(inTEXTexture.HDC, ref pictureInfo, DIBColorTable.DIB_RGB_COLORS, out ppvBits, IntPtr.Zero, 0);

                SelectObject(inTEXTexture.HDC, inTEXTexture.HBMP);
                SetDIBits(inTEXTexture.HDC, inTEXTexture.HBMP, 0, (uint)pictureInfo.bmiHeader.biHeight, pictureData, ref pictureInfo, DIBColorTable.DIB_RGB_COLORS);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error loading Bitmap from TEXTexture: " + e.Message + ".", "Error");
            }
        }

        public static void LoadImageAsTexTexture(string fileName, ref TEX tex)
        {
            Bitmap bmpTexture;

            if (Path.GetExtension(fileName).ToUpper() == ".TEX" || Path.GetExtension(fileName).Length <= 0)
            {
                // This is a tex texture file
                ReadTEXTexture(ref tex, fileName);
                LoadBitmapFromTEXTexture(ref tex);
            }
            else
            {
                // This is another format image texture file
                bmpTexture = new Bitmap(fileName);

                IntPtr tmpIntPtrGetTEX = IntPtr.Zero;
                GetTEXTextureFromBitmap(ref tex, ref tmpIntPtrGetTEX, bmpTexture.GetHbitmap());
            }

            LoadTEXTexture(ref tex);
            LoadBitmapFromTEXTexture(ref tex);
        }

        public static void GetTEXTextureFromBitmap(ref TEX outTEX, ref IntPtr hDC, IntPtr hBMP)
        {
            int li, si, ti, pi, palSize, texBitmapSize, i;
            long lineLength, lineLengthBytes, linePad, linePadUseful, linePadBytes, line_end;
            short shift, mask, parts, parts_left, bits;
            byte aux_val;

            BitmapInfo pictureInfo = new BitmapInfo();
            byte[] pictureData = null;

            GetAllBitmapData(hDC, hBMP, pictureData, ref pictureInfo);

            bits = (short)pictureInfo.bmiHeader.biBitCount;
            palSize = 0;
            if (bits <= 8) palSize = (int)Math.Pow(2, bits);

            outTEX.version = 1;
            outTEX.unk1 = 0;
            outTEX.ColorKeyFlag = 1;
            outTEX.unk2 = 1;
            outTEX.unk3 = 5;
            outTEX.minimumBitsPerColor = bits;
            outTEX.maximumBitsPerColor = 8;
            outTEX.minimumAlphaBits = 0;
            outTEX.maximumAlphaBits = 8;
            outTEX.minimumBitsPerPixel = 8;
            outTEX.maximumBitsPerPixel = 32;
            outTEX.unk4 = 0;

            if (palSize > 0) outTEX.numPalettes = 1;
            else outTEX.numPalettes = 0;

            outTEX.numColorsPerPalette = palSize;
            outTEX.bitDepth = bits;
            outTEX.width = pictureInfo.bmiHeader.biWidth;
            outTEX.height = pictureInfo.bmiHeader.biHeight;
            if (bits < 8) outTEX.bytesPerRow = outTEX.width;
            else outTEX.bytesPerRow = (bits * outTEX.width) / 8;

            outTEX.unk5 = 0;

            if (bits <= 8)
            {
                outTEX.paletteFlag = 1;
                outTEX.bitsPerIndex = 8;

            }
            else
            {
                outTEX.paletteFlag = 0;
                outTEX.bitsPerIndex = 0;
            }

            outTEX.indexedTo8BitsFlag = 0;
            outTEX.paletteSize = palSize;
            outTEX.numColorsPerPalette2 = palSize;
            outTEX.runtimeData = 19752016;
            outTEX.bitsPerPixel = bits;

            if (bits < 8) outTEX.bytesPerPixel = 1;
            outTEX.bytesPerPixel = bits / 8;

            outTEX.red8 = 8;
            outTEX.green8 = 8;
            outTEX.blue8 = 8;
            outTEX.alpha8 = 8;

            switch (bits)
            {
                case 16:
                    outTEX.numRedBits = 5;
                    outTEX.numGreenBits = 5;
                    outTEX.numBlueBits = 5;
                    outTEX.numAlphaBits = 0;
                    outTEX.redBitMask = 0x7E00;
                    outTEX.greenBitMask = 0x3E0;
                    outTEX.blueBitMask = 0x1F;
                    outTEX.alphaBitMask = 0;
                    outTEX.redShift = 10;
                    outTEX.greenShift = 5;
                    outTEX.blueShift = 0;
                    outTEX.alphaShift = 0;
                    break;

                case 24:
                    outTEX.numRedBits = 8;
                    outTEX.numGreenBits = 8;
                    outTEX.numBlueBits = 8;
                    outTEX.numAlphaBits = 0;
                    outTEX.redBitMask = 0xFF0000;
                    outTEX.greenBitMask = 0xFF00;
                    outTEX.blueBitMask = 0xFF;
                    outTEX.alphaBitMask = 0;
                    outTEX.redShift = 16;
                    outTEX.greenShift = 8;
                    outTEX.blueShift = 0;
                    outTEX.alphaShift = 0;
                    break;

                case 32:
                    outTEX.numRedBits = 8;
                    outTEX.numGreenBits = 8;
                    outTEX.numBlueBits = 8;
                    outTEX.numAlphaBits = 8;
                    outTEX.redBitMask = 0xFF0000;
                    outTEX.greenBitMask = 0xFF00;
                    outTEX.blueBitMask = 0xFF;
                    outTEX.alphaBitMask = -16777216;
                    outTEX.redShift = 16;
                    outTEX.greenShift = 8;
                    outTEX.blueShift = 0;
                    outTEX.alphaShift = 24;
                    break;
            }

            outTEX.redMax = (int)Math.Pow(2, outTEX.numRedBits - 1);
            outTEX.greenMax = (int)Math.Pow(2, outTEX.numGreenBits - 1);
            outTEX.blueMax = (int)Math.Pow(2, outTEX.numBlueBits - 1);
            outTEX.alphaMax = (int)Math.Pow(2, outTEX.numAlphaBits - 1);

            outTEX.colorKeyArrayFlag = 0;
            outTEX.runtimeData2 = 0;
            outTEX.referenceAlpha = 255;
            outTEX.unk6 = 4;
            outTEX.unk7 = 1;
            outTEX.runtimeDataPaletteIndex = 0;
            outTEX.runtimeData3 = 34546076;
            outTEX.runtimeData4 = 0;
            outTEX.unk8 = 0;
            outTEX.unk9 = 480;
            outTEX.unk10 = 320;
            outTEX.unk11 = 512;

            lineLength = outTEX.width * outTEX.bitsPerPixel;

            if (lineLength % 32 == 0) linePad = 0;
            else linePad = 32 * ((lineLength / 32) + 1) - 8 * (lineLength / 8);

            if (linePad == 0) linePadUseful = 0;
            else linePadUseful = lineLength - 8 * (lineLength / 8);

            if (linePad > 0 && linePad < 8) linePadBytes = 1;
            else linePadBytes = linePad / 8;

            lineLengthBytes = lineLength / 8 + linePadBytes;

            texBitmapSize = outTEX.width * outTEX.height * outTEX.bytesPerPixel - 1;
            outTEX.pixelData = new byte[texBitmapSize];

            if (bits == 1 || bits == 4)
            {
                ti = 0;
                shift = (short)Math.Pow(2, bits);
                mask = (short)(shift - 1);
                parts = (short)((8 / bits) - 1);
                parts_left = (short)((linePadUseful / bits) - 1);

                for (li = outTEX.height - 2; li >= 0; li--)
                {
                    line_end = (li + 1) * lineLengthBytes - linePadBytes - 1;

                    for (si = (int)(li * lineLengthBytes); si <= line_end; si ++)
                    {
                        aux_val = pictureData[si];

                        for (pi = 0; pi <= parts; pi++)
                        {
                            outTEX.pixelData[ti + parts - pi] = (byte)(aux_val & mask);
                            aux_val = (byte)(aux_val / shift);
                        }

                        ti = ti + parts + 1;
                    }

                    if (linePad > 0)
                    {
                        aux_val = pictureData[si];

                        for (pi = 0; pi <= parts_left; pi++)
                        {
                            outTEX.pixelData[ti] = (byte)(aux_val & mask);
                            aux_val = (byte)(aux_val / shift);
                        }

                        ti = ti + parts_left + 1;
                    }
                }
            }
            else
            {
                long lineLengthDIV = lineLength / 8;

                for (li = 0; li < outTEX.height; li++)
                {
                    for (i = 0; i < lineLengthDIV; i++)
                    {
                        pictureData[(li * lineLengthDIV) + i] = pictureData[(((outTEX.height - 1) - li) * lineLengthBytes) + i];
                    }
                }
            }

            if (outTEX.paletteFlag == 1)
            {
                outTEX.palette = new byte[4 * outTEX.numColorsPerPalette];

                for (i = 0; i < outTEX.numColorsPerPalette; i++)
                {
                    outTEX.palette[(4 * i) + 2] = pictureInfo.bmiColors[i].rgbRed;
                    outTEX.palette[(4 * i) + 1] = pictureInfo.bmiColors[i].rgbGreen;
                    outTEX.palette[(4 * i)] = pictureInfo.bmiColors[i].rgbBlue;
                }
            }

        }

        public static void UnloadTexture(ref TEX inTEX)
        {
            uint[] lstTEX = new uint[1];
            lstTEX[0] = inTEX.texID;

            glDeleteTextures(1, lstTEX);
            DeleteDC(inTEX.HDC);
            DeleteObject(inTEX.HBMP);
        }

 

        //  ---------------------------------------------------------------------------------------------------
        //  ============================================= SAVING ==============================================
        //  ---------------------------------------------------------------------------------------------------
        public static void WriteTEXTexture(TEX Texture, string fileName)
        {
            try
            {
                if (Texture.texID == 0xFFFFFFFF) return;

                FileStream writeStream;

                using (writeStream = new FileStream(fileName, FileMode.Create))
                {
                    using (BinaryWriter fileWriter = new BinaryWriter(writeStream))
                    {
                        fileWriter.Write(Texture.version);
                        fileWriter.Write(Texture.unk1);
                        fileWriter.Write(Texture.ColorKeyFlag);
                        fileWriter.Write(Texture.unk2);
                        fileWriter.Write(Texture.unk3);
                        fileWriter.Write(Texture.minimumBitsPerColor);
                        fileWriter.Write(Texture.maximumBitsPerColor);
                        fileWriter.Write(Texture.minimumAlphaBits);
                        fileWriter.Write(Texture.maximumAlphaBits);
                        fileWriter.Write(Texture.minimumBitsPerPixel);
                        fileWriter.Write(Texture.maximumBitsPerPixel);
                        fileWriter.Write(Texture.unk4);
                        fileWriter.Write(Texture.numPalettes);
                        fileWriter.Write(Texture.numColorsPerPalette);
                        fileWriter.Write(Texture.bitDepth);
                        fileWriter.Write(Texture.width);
                        fileWriter.Write(Texture.height);
                        fileWriter.Write(Texture.bytesPerRow);
                        fileWriter.Write(Texture.unk5);
                        fileWriter.Write(Texture.paletteFlag);
                        fileWriter.Write(Texture.bitsPerIndex);
                        fileWriter.Write(Texture.indexedTo8BitsFlag);
                        fileWriter.Write(Texture.paletteSize);
                        fileWriter.Write(Texture.numColorsPerPalette2);
                        fileWriter.Write(Texture.runtimeData);
                        fileWriter.Write(Texture.bitsPerPixel);
                        fileWriter.Write(Texture.bytesPerPixel);

                        fileWriter.Write(Texture.numRedBits);
                        fileWriter.Write(Texture.numGreenBits);
                        fileWriter.Write(Texture.numBlueBits);
                        fileWriter.Write(Texture.numAlphaBits);
                        fileWriter.Write(Texture.redBitMask);
                        fileWriter.Write(Texture.greenBitMask);
                        fileWriter.Write(Texture.blueBitMask);
                        fileWriter.Write(Texture.alphaBitMask);
                        fileWriter.Write(Texture.redShift);
                        fileWriter.Write(Texture.greenShift);
                        fileWriter.Write(Texture.blueShift);
                        fileWriter.Write(Texture.alphaShift);
                        fileWriter.Write(Texture.red8);
                        fileWriter.Write(Texture.green8);
                        fileWriter.Write(Texture.blue8);
                        fileWriter.Write(Texture.alpha8);
                        fileWriter.Write(Texture.redMax);
                        fileWriter.Write(Texture.greenMax);
                        fileWriter.Write(Texture.blueMax);
                        fileWriter.Write(Texture.alphaMax);

                        fileWriter.Write(Texture.colorKeyArrayFlag);
                        fileWriter.Write(Texture.runtimeData2);
                        fileWriter.Write(Texture.referenceAlpha);
                        fileWriter.Write(Texture.unk6);
                        fileWriter.Write(Texture.unk7);
                        fileWriter.Write(Texture.runtimeDataPaletteIndex);
                        fileWriter.Write(Texture.runtimeData3);
                        fileWriter.Write(Texture.runtimeData4);
                        fileWriter.Write(Texture.unk8);
                        fileWriter.Write(Texture.unk9);
                        fileWriter.Write(Texture.unk10);
                        fileWriter.Write(Texture.unk11);

                        if (Texture.paletteFlag == 1) fileWriter.Write(Texture.palette);

                        fileWriter.Write(Texture.pixelData);

                        if (Texture.colorKeyArrayFlag == 1) fileWriter.Write(Texture.colorKeyData);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error writing TEX Texture " + fileName + ".", "Error", MessageBoxButtons.OK);
            }
        }



    }
}
