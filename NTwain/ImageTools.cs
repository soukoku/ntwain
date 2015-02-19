using Microsoft.Win32.SafeHandles;
using NTwain.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Media.Imaging;

namespace NTwain
{
    public static class ImageTools
    {
        internal static bool IsDib(IntPtr data)
        {
            // a quick check not guaranteed correct,
            // compare first 2 bytes to size of struct (which is also the first field)
            var test = Marshal.ReadInt16(data);
            // should be 40
            return test == BITMAPINFOHEADER.GetByteSize();
        }
        internal static bool IsTiff(IntPtr data)
        {
            var test = Marshal.ReadInt16(data);
            // should be II
            return test == 0x4949;
        }
        internal static Bitmap ReadBitmapImage(IntPtr data)
        {
            Bitmap finalImg = null;
            Bitmap tempImg = null;
            try
            {
                var header = (BITMAPINFOHEADER)Marshal.PtrToStructure(data, typeof(BITMAPINFOHEADER));

                if (header.Validate())
                {
                    PixelFormat format = header.GetDrawingPixelFormat();
                    tempImg = new Bitmap(header.biWidth, Math.Abs(header.biHeight), header.GetStride(), format, header.GetScan0(data));
                    ColorPalette pal = header.GetDrawingPalette(data);
                    if (pal != null)
                    {
                        tempImg.Palette = pal;
                    }
                    float xdpi = header.GetXDpi();
                    float ydpi = header.GetYDpi();
                    if (xdpi != 0 && ydpi == 0)
                    {
                        ydpi = xdpi;
                    }
                    else if (ydpi != 0 && xdpi == 0)
                    {
                        xdpi = ydpi;
                    }
                    if (xdpi != 0)
                    {
                        tempImg.SetResolution(xdpi, ydpi);
                    }
                    if (header.IsBottomUpImage)
                    {
                        tempImg.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }
                    finalImg = tempImg;
                    tempImg = null;
                }
            }
            finally
            {
                if (tempImg != null)
                {
                    tempImg.Dispose();
                }
            }
            return finalImg;
        }

        internal static Image ReadTiffImage(IntPtr data)
        {
            // this is modified from twain cs sample
            // http://sourceforge.net/projects/twainforcsharp/?source=typ_redirect


            // Find the size of the image so we can turn it into a memory stream...
            var headerSize = Marshal.SizeOf(typeof(TIFFHEADER));
            var tagSize = Marshal.SizeOf(typeof(TIFFTAG));
            var tiffSize = 0;
            var header = (TIFFHEADER)Marshal.PtrToStructure(data, typeof(TIFFHEADER));
            var tagPtr = data.ToInt64() + headerSize;
            for (int i = 0; i < 999; i++)
            {
                tagPtr += (tagSize * i);
                var tag = (TIFFTAG)Marshal.PtrToStructure((IntPtr)tagPtr, typeof(TIFFTAG));

                switch (tag.u16Tag)
                {
                    case 273: // StripOffsets...
                    case 279: // StripByteCounts...
                        tiffSize += (int)tag.u32Value;
                        break;
                }
            }

            if (tiffSize > 0)
            {
                var dataCopy = new byte[tiffSize];
                Marshal.Copy(data, dataCopy, 0, tiffSize);

                return Image.FromStream(new MemoryStream(dataCopy));
            }
            return null;
        }

        /// <summary>
        /// Converts an <see cref="Image"/> to WPF <see cref="BitmapSource"/> if the image
        /// is a <see cref="Bitmap"/>.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns></returns>
        public static BitmapSource ConvertToWpfBitmap(this Image image)
        {
            var bmp = image as Bitmap;
            if (bmp != null)
            {
                using (var hbm = new SafeHBitmapHandle(bmp.GetHbitmap(), true))
                {
                    return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                       hbm.DangerousGetHandle(),
                       IntPtr.Zero,
                       System.Windows.Int32Rect.Empty,
                       BitmapSizeOptions.FromEmptyOptions());
                }
            }
            return null;
        }

        class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
        {
            [SecurityCritical]
            public SafeHBitmapHandle(IntPtr preexistingHandle, bool ownsHandle)
                : base(ownsHandle)
            {
                SetHandle(preexistingHandle);
            }

            protected override bool ReleaseHandle()
            {
                return NativeMethods.DeleteObject(handle);
            }
        }
    }
}
