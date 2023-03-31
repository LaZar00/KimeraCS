using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace KimeraCS
{

    using static FF7Skeleton;

    class GDI32
    {
        internal const string DLLName = "GDI32.DLL";

        /// <summary>
        /// Generic interface for the different version of BITMAPINFOHEADER structures
        /// </summary>
        public interface IBitmapInfoHeader
        {
        }

        //public struct POINTAPI
        //{
        //    int x;
        //    int y;

        //    public POINTAPI(int in_x, int in_y)
        //    {
        //        x = in_x;
        //        y = in_y;
        //    }
        //}

        //public struct LOGBRUSH
        //{
        //    int lbStyle;
        //    int lbColor;
        //    int lbHatch;
        //}

        [Flags]
        public enum BitCount : ushort
        {
            BitPerPixel0BPP = 0,
            BitPerPixel1BPP = 1,
            BitPerPixel4BPP = 4,
            BitPerPixel8BPP = 8,
            BitPerPixel16BPP = 16,
            BitPerPixel24BPP = 24,
            BitPerPixel32BPP = 32
        }

        [Flags]
        public enum BitmapCompression : uint
        {
            BI_RGB = 0,
            BI_RLE8 = 1,
            BI_RLE4 = 2,
            BI_BITFIELDS = 3,
            BI_JPEG = 4,
            BI_PNG = 5,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BitmapInfoHeader : IBitmapInfoHeader
        {
            public uint biSize;
            public int biWidth;
            public int biHeight;
            public ushort biPlanes;
            public BitCount biBitCount;
            public BitmapCompression biCompression;
            public uint biSizeImage;
            public int biXPelsPerMeter;
            public int biYPelsPerMeter;
            public uint biClrUsed;
            public uint biClrImportant;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RgbQuad
        {
            public byte rgbBlue;
            public byte rgbGreen;
            public byte rgbRed;
            public byte rgbReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BitmapInfo
        {
            public BitmapInfoHeader bmiHeader;
            [MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = 256, ArraySubType = UnmanagedType.Struct)]
            public RgbQuad[] bmiColors;
        }

        [Flags]
        public enum DIBColorTable : uint
        {
            DIB_PAL_INDICES = 2,
            DIB_PAL_COLORS = 1,
            DIB_RGB_COLORS = 0
        }

        [Flags]
        public enum PenStyle : int
        {
            PS_SOLID = 0, //The pen is solid.
            PS_DASH = 1, //The pen is dashed.
            PS_DOT = 2, //The pen is dotted.
            PS_DASHDOT = 3, //The pen has alternating dashes and dots.
            PS_DASHDOTDOT = 4, //The pen has alternating dashes and double dots.
            PS_NULL = 5, //The pen is invisible.
            PS_INSIDEFRAME = 6,// Normally when the edge is drawn, it’s centred on the outer edge meaning that half the width of the pen is drawn
                               // outside the shape’s edge, half is inside the shape’s edge. When PS_INSIDEFRAME is specified the edge is drawn
                               //completely inside the outer edge of the shape.
            PS_USERSTYLE = 7,
            PS_ALTERNATE = 8,
            PS_STYLE_MASK = 0x0000000F,

            PS_ENDCAP_ROUND = 0x00000000,
            PS_ENDCAP_SQUARE = 0x00000100,
            PS_ENDCAP_FLAT = 0x00000200,
            PS_ENDCAP_MASK = 0x00000F00,

            PS_JOIN_ROUND = 0x00000000,
            PS_JOIN_BEVEL = 0x00001000,
            PS_JOIN_MITER = 0x00002000,
            PS_JOIN_MASK = 0x0000F000,

            PS_COSMETIC = 0x00000000,
            PS_GEOMETRIC = 0x00010000,
            PS_TYPE_MASK = 0x000F0000
        };


        #region Flags Win32
        [Flags()]
        public enum PFD_FLAGS : uint
        {
            PFD_DOUBLEBUFFER = 0x00000001,
            PFD_STEREO = 0x00000002,
            PFD_DRAW_TO_WINDOW = 0x00000004,
            PFD_DRAW_TO_BITMAP = 0x00000008,
            PFD_SUPPORT_GDI = 0x00000010,
            PFD_SUPPORT_OPENGL = 0x00000020,
            PFD_GENERIC_FORMAT = 0x00000040,
            PFD_NEED_PALETTE = 0x00000080,
            PFD_NEED_SYSTEM_PALETTE = 0x00000100,
            PFD_SWAP_EXCHANGE = 0x00000200,
            PFD_SWAP_COPY = 0x00000400,
            PFD_SWAP_LAYER_BUFFERS = 0x00000800,
            PFD_GENERIC_ACCELERATED = 0x00001000,
            PFD_SUPPORT_DIRECTDRAW = 0x00002000,
            PFD_DIRECT3D_ACCELERATED = 0x00004000,
            PFD_SUPPORT_COMPOSITION = 0x00008000,
            PFD_DEPTH_DONTCARE = 0x20000000,
            PFD_DOUBLEBUFFER_DONTCARE = 0x40000000,
            PFD_STEREO_DONTCARE = 0x80000000
        }

        [Flags()]
        public enum PFD_LAYER_TYPES : byte
        {
            PFD_MAIN_PLANE = 0,
            PFD_OVERLAY_PLANE = 1,
            PFD_UNDERLAY_PLANE = 255
        }

        [Flags()]
        public enum PFD_PIXEL_TYPE : byte
        {
            PFD_TYPE_RGBA = 0,
            PFD_TYPE_COLORINDEX = 1
        }
        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct PIXELFORMATDESCRIPTOR
        {

            #region Fields
            public byte bReserved;
            public byte cAccumAlphaBits;
            public byte cAccumBits;
            public byte cAccumBlueBits;
            public byte cAccumGreenBits;
            public byte cAccumRedBits;
            public byte cAlphaBits;
            public byte cAlphaShift;
            public byte cAuxBuffers;
            public byte cBlueBits;
            public byte cBlueShift;
            public byte cColorBits;
            public byte cDepthBits;
            public byte cGreenBits;
            public byte cGreenShift;
            public byte cRedBits;
            public byte cRedShift;
            public byte cStencilBits;
            public Int32 dwDamageMask;
            public PFD_FLAGS dwFlags;
            public Int32 dwLayerMask;
            public Int32 dwVisibleMask;
            public PFD_LAYER_TYPES iLayerType;
            public PFD_PIXEL_TYPE iPixelType;
            public Int16 nSize;
            public Int16 nVersion;
            #endregion Fields

            public void Init()
            {
                nSize = 40;
                nVersion = 1;
                dwFlags = PFD_FLAGS.PFD_DRAW_TO_WINDOW | PFD_FLAGS.PFD_SUPPORT_OPENGL | PFD_FLAGS.PFD_DOUBLEBUFFER;
                iPixelType = PFD_PIXEL_TYPE.PFD_TYPE_RGBA;
                cColorBits = 24;
                cRedBits = cRedShift = cGreenBits = cGreenShift = cBlueBits = cBlueShift = 0;
                cAlphaBits = cAlphaShift = 0;
                cAccumBits = cAccumRedBits = cAccumGreenBits = cAccumBlueBits = cAccumAlphaBits = 0;
                cDepthBits = 32;
                cStencilBits = cAuxBuffers = 0;
                iLayerType = PFD_LAYER_TYPES.PFD_MAIN_PLANE;
                bReserved = 0;
                dwLayerMask = dwVisibleMask = dwDamageMask = 0;
            }
        }


        ///////////////////////////////////////
        // gdi32.dll
        public enum StretchMode : uint
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }

        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        public enum TernaryRasterOperations : uint
        {
            SRCCOPY = 0x00CC0020,
            SRCPAINT = 0x00EE0086,
            SRCAND = 0x008800C6,
            SRCINVERT = 0x00660046,
            SRCERASE = 0x00440328,
            NOTSRCCOPY = 0x00330008,
            NOTSRCERASE = 0x001100A6,
            MERGECOPY = 0x00C000CA,
            MERGEPAINT = 0x00BB0226,
            PATCOPY = 0x00F00021,
            PATPAINT = 0x00FB0A09,
            PATINVERT = 0x005A0049,
            DSTINVERT = 0x00550009,
            BLACKNESS = 0x00000042,
            WHITENESS = 0x00FF0062,
            CAPTUREBLT = 0x40000000
        }

        [DllImport(DLLName, EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport(DLLName, EntryPoint = "ChoosePixelFormat")]
        public static extern int ChoosePixelFormat(IntPtr hdc, [In] ref PIXELFORMATDESCRIPTOR ppfd);

//        [DllImport(DLLName, EntryPoint = "CreateBrushIndirect")]
//        public static extern IntPtr CreateBrushIndirect([In] ref LOGBRUSH lplb);

        [DllImport(DLLName, EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport(DLLName, EntryPoint = "CreateCompatibleDC")]
        public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

        [DllImport(DLLName, EntryPoint = "CreateDIBSection")]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, ref BitmapInfo bmi, DIBColorTable Usage, out IntPtr bits, IntPtr hSection, uint dwOffset);

//        [DllImport(DLLName, EntryPoint = "CreatePen")]
//        public static extern IntPtr CreatePen(PenStyle fnPenStyle, int nWidth, uint crColor);

//        [DllImport(DLLName, EntryPoint = "CreateSolidBrush")]
//        public static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport(DLLName, EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);

        [DllImport(DLLName, EntryPoint = "DeleteObject")]
        public static extern bool DeleteObject([In] IntPtr hObject);

//        [DllImport(DLLName)]
//        public static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport(DLLName, EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

//        [DllImport(DLLName, EntryPoint = "SetPixel")]
//        public static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);

        [DllImport(DLLName, EntryPoint = "SetDIBits", SetLastError = true)]
        public static extern int SetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan, uint cScanLines,
                                           byte[] lpvBits, [In] ref BitmapInfo lpbmi, DIBColorTable uUsage);

        [DllImport(DLLName, EntryPoint = "GetDIBits")]
        public static extern int GetDIBits([In] IntPtr hdc, [In] IntPtr hbmp, uint uStartScan, uint cScanLines,
                                           [Out] byte[] lpvBits, ref BitmapInfo lpbi, DIBColorTable uUsage);

        [DllImport(DLLName, EntryPoint = "StretchBlt")]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest,
                                             IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, TernaryRasterOperations dwRop);

        [DllImport(DLLName, EntryPoint = "SetStretchBltMode")]
        public static extern bool SetStretchBltMode(IntPtr hDC, StretchMode iStretchMode);

        //[DllImport(DLLName, EntryPoint = "SetBrushOrgEx", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        //public static extern int SetBrushOrgEx(IntPtr hDC, int x, int y, ref POINTAPI p);

        [DllImport(DLLName, EntryPoint = "SetPixelFormat")]
        public static extern bool SetPixelFormat(IntPtr hdc, int iPixelFormat, ref PIXELFORMATDESCRIPTOR ppfd);

        [DllImport(DLLName, EntryPoint = "SwapBuffers")]
        public static extern int SwapBuffers(IntPtr hDC);



        ///////////////////////////////////////
        // Global functions
        //public static uint RGBTOINT(byte r, byte g, byte b, byte a)
        public static uint RGBTOINT(byte r, byte g, byte b)
        {
            //return (uint)((a << 24) | (b << 16) | (g << 8) | (r << 0));
            return (uint)((b << 16) | (g << 8) | r );
            //return (uint)((a * 0x1000000) + (b * 0x10000) + (g * 0x100) + r);
            //return (uint)((a * 0x1000000) + (b * 0x10000) + (g * 0x100) + r);
        }

        public static void INTTORGB(byte[] inRGB, int idxInRGB, uint color)
        {
            inRGB[idxInRGB * 4] = (byte)(color & 0xFF);
            inRGB[idxInRGB * 4 + 1] = (byte)((color >> 8) & 0xFF);
            inRGB[idxInRGB * 4 + 2] = (byte)((color >> 16) & 0xFF);
            inRGB[idxInRGB * 4 + 3] = 0xFF;
        }

        public static void GetHeaderBitmapInfo(IntPtr hDC, IntPtr hBMP, ref BitmapInfo bmpInfo)
        {
            byte[] lpvBits = null;

            bmpInfo.bmiHeader.biSize = 40;

            GetDIBits(hDC, hBMP, 0, 0, lpvBits, ref bmpInfo, DIBColorTable.DIB_RGB_COLORS);
        }

 
        //  Uses the 3-call method to GetDIBits for retriving the bitmap data (including original pallete)
        public static void GetAllBitmapData(IntPtr hDC, IntPtr hBMP,
                                            DirectBitmap bmpTexture, ref BitmapInfo bmpInfo, out byte[] pictureData)
        {
            if (bmpInfo.bmiHeader.biBitCount == BitCount.BitPerPixel1BPP ||
                bmpInfo.bmiHeader.biBitCount == BitCount.BitPerPixel4BPP ||
                bmpInfo.bmiHeader.biBitCount == BitCount.BitPerPixel8BPP)
            {
                uint oldUsed = bmpInfo.bmiHeader.biClrUsed;  //Read bitmap palette

                GetDIBits(hDC, hBMP, 0, 0, null, ref bmpInfo, 0);
                bmpInfo.bmiHeader.biClrUsed = oldUsed;
            }

            // Declare an array to hold the bytes of the bitmap.         
            pictureData = new byte[bmpTexture.Stride * bmpTexture.Height];

            // We put each color to the TEX structure pictureData
            int bmpSize = bmpTexture.Width * bmpTexture.Height;
            for (int i = 0; i < bmpSize; i++)
            {
                pictureData[i * 4] = (byte)(bmpTexture.Bits[i] & 0xFF);
                pictureData[i * 4 + 1] = (byte)(bmpTexture.Bits[i] >> 8 & 0xFF);
                pictureData[i * 4 + 2] = (byte)(bmpTexture.Bits[i] >> 16 & 0xFF);
                pictureData[i * 4 + 3] = (byte)(bmpTexture.Bits[i] >> 24 & 0xFF);
            }
        }


        //  This function fits a bitmap to a picturebox scaling/shrinking it if necessary with NearestNeighbor interpolate option
        public static Bitmap FitBitmapToPictureBox(PictureBox pbIn, int iImgWidth, int iImgHeight, IntPtr hBMP)
        {
            Bitmap tmpBMP;
            float fAspectRatio = (float)iImgWidth / (float)iImgHeight;

            // Get the size available
            float fWidth = pbIn.ClientSize.Width;
            float fHeight = pbIn.ClientSize.Height;

            //  Adjust the wid/hgt ratio to match aspect_src
            if (fWidth / fHeight > fAspectRatio)
            {
                //  The area is too short and wide.
                //  Make it narrower.
                fWidth = fAspectRatio * fHeight;
            }
            else
            {
                //  The area is too tall and thin.
                //  Make it shorter.
                fHeight = fWidth / fAspectRatio;
            }

            //  Create image at the correct size.
            if (iImgWidth < pbIn.ClientSize.Width && iImgHeight < pbIn.ClientSize.Height)
                tmpBMP = new Bitmap(pbIn.ClientSize.Width, pbIn.ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            else
                tmpBMP = new Bitmap(iImgWidth, iImgHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(tmpBMP))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                g.DrawImage(Image.FromHbitmap(hBMP), 0, 0, fWidth, fHeight);
            }

            return tmpBMP;
        }

        //  This function fits a bitmap to a picturebox scaling/shrinking it if necessary with NearestNeighbor interpolate option
        //  Variant that allows pass as reference the pbIn
        public static Bitmap PutDIBIntoBitmap(int iImgWidth, int iImgHeight, IntPtr hBMP)
        {
            Bitmap tmpBMP;
            tmpBMP = new Bitmap(iImgWidth, iImgHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(tmpBMP))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;

                g.DrawImage(Image.FromHbitmap(hBMP), 0, 0, iImgWidth, iImgHeight);
            }

            return tmpBMP;
        }


        public enum ProgressBarTextLocation
        {
            Left,
            Centered
        }

        public static void SetProgressBarText
        (
        System.Windows.Forms.ProgressBar Target, //The target progress bar
        string Text, //The text to show in the progress bar
        ProgressBarTextLocation Location, //Where the text is to be placed
        System.Drawing.Color TextColor, //The color the text is to be drawn in
        System.Drawing.Font TextFont //The font we use to draw the text
        )
        {

            //Make sure we didn't get a null progress bar
            if (Target == null) { throw new ArgumentException("Null Target"); }

            //Now we can add in the text
            Target.Refresh();

            //gr will be the graphics object we use to draw on Target
            using (Graphics gr = Target.CreateGraphics())
            {
                gr.DrawString(Text,
                    TextFont, //The font we will draw it it (TextFont)
                    new SolidBrush(TextColor), //The brush we will use to draw it

                    //Where we will draw it
                    new PointF(
                        // X location (Center or Left)
                        Location == ProgressBarTextLocation.Left ?
                        5 : //Left side
                        Target.Width / 2 - (gr.MeasureString(Text, //Centered
                        TextFont).Width / 2.0F),
                        // Y Location (This is the same regardless of Location)
                        Target.Height / 2 - (gr.MeasureString(Text,
                        TextFont).Height / 2.0F)));           
            }

        }

        public static IntPtr GenerateDC(IntPtr hDCIn, int x, int y)
        {
            IntPtr hBMP, hDC;
            string errorTitle = "GDI graphics error";

            //  type = 16;   Icon MessageBox, error (only for KimeraVB6)

            do
            {
                hDC = CreateCompatibleDC(hDCIn);
            } while (hDC == null);

            // -- Commented in KimeraVB6
            //if (hDC != null)
            //{
            //    MessageBox.Show("The device context can not be created.", type, errorTitle);
            //    hDC = IntPtr.Zero;
            //    return hDC;
            //}

            hBMP = CreateCompatibleBitmap(hDCIn, x, y);
            if (hBMP == IntPtr.Zero)
            {
                MessageBox.Show("The bitmap can not be created.", errorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DeleteDC(hDC);
                return hDC;
            }

            SelectObject(hDC, hBMP);
            DeleteObject(hBMP);
            BitBlt(hDC, 0, 0, x, y, hDC, 0, 0, TernaryRasterOperations.BLACKNESS);

            return hDC;
        }






    }
}
