using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KimeraCS
{
    public class DirectBitmap : IDisposable
    {
        public const float fDefaultDPI = 96.0f;

        public Bitmap Bitmap { get; set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public int BitsPerPixel { get; private set; }
        public int Stride { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);

            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());

            Rectangle rect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                Bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                Bitmap.PixelFormat);

            Stride = bmpData.Stride;

            switch(bmpData.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    BitsPerPixel = 1;
                    break;

                case PixelFormat.Format4bppIndexed:
                    BitsPerPixel = 4;
                    break;

                case PixelFormat.Format8bppIndexed:
                    BitsPerPixel = 8;
                    break;

                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    BitsPerPixel = 16;
                    break;

                case PixelFormat.Format24bppRgb:
                    BitsPerPixel = 24;
                    break;

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format32bppPArgb:
                    BitsPerPixel = 32;
                    break;

                default:
                    BitsPerPixel = 32;
                    break;
            }

            Bitmap.UnlockBits(bmpData);
        }

        public DirectBitmap(string filename)
        {
            Bitmap tmpBmp = new Bitmap(filename);

            // Normalize DPI resolution of loaded image (default 96 dpi)
            // I detected some .png files have different DPI than the created default bitmaps
            // for .tex textures. Those .png have sometimes a DPI value of 72, then, when used
            // in pictureboxes, the image is distorted.
            // Normalizing the DPI value -horizontal & vertical- seems to fix that
            tmpBmp.SetResolution(fDefaultDPI, fDefaultDPI);

            // We rotate the image because .tex uses different x/y coordiantes
            tmpBmp.RotateFlip(RotateFlipType.Rotate180FlipX);

            Width = tmpBmp.Width;
            Height = tmpBmp.Height;
            Bits = new Int32[tmpBmp.Width * tmpBmp.Height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);

            Bitmap = new Bitmap(tmpBmp.Width, tmpBmp.Height, tmpBmp.Width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());

            Rectangle rect = new Rectangle(0, 0, Bitmap.Width, Bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                Bitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                Bitmap.PixelFormat);

            Stride = bmpData.Stride;

            switch (bmpData.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    BitsPerPixel = 1;
                    break;

                case PixelFormat.Format4bppIndexed:
                    BitsPerPixel = 4;
                    break;

                case PixelFormat.Format8bppIndexed:
                    BitsPerPixel = 8;
                    break;

                case PixelFormat.Format16bppGrayScale:
                case PixelFormat.Format16bppArgb1555:
                case PixelFormat.Format16bppRgb555:
                case PixelFormat.Format16bppRgb565:
                    BitsPerPixel = 16;
                    break;

                case PixelFormat.Format24bppRgb:
                    BitsPerPixel = 24;
                    break;

                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppRgb:
                case PixelFormat.Format32bppPArgb:
                    BitsPerPixel = 32;
                    break;

                default:
                    BitsPerPixel = 32;
                    break;
            }

            Bitmap.UnlockBits(bmpData);

            using (Graphics g = Graphics.FromImage(Bitmap))
            {
                g.DrawImage(tmpBmp, 0, 0);
            }

            tmpBmp.Dispose();
        }

        public void SetPixel(int x, int y, Color colour)
        {
            int index = x + (y * Width);
            int col = colour.ToArgb();

            Bits[index] = col;
        }

        public Color GetPixel(int x, int y)
        {
            int index = x + (y * Width);
            int col = Bits[index];
            Color result = Color.FromArgb(col);

            return result;
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }

    }
}
