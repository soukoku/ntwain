using Microsoft.Win32.SafeHandles;
using NTwain.Interop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Windows.Media.Imaging;

namespace NTwain
{
    public static class ImageTools
    {
        internal static Bitmap ReadBitmapImage(IntPtr dibBitmap)
        {
            Bitmap finalImg = null;
            Bitmap tempImg = null;
            if (IsDib(dibBitmap))
            {
                try
                {
                    var header = (BITMAPINFOHEADER)Marshal.PtrToStructure(dibBitmap, typeof(BITMAPINFOHEADER));

                    if (header.Validate())
                    {
                        PixelFormat format = header.GetDrawingPixelFormat();
                        tempImg = new Bitmap(header.biWidth, Math.Abs(header.biHeight), header.GetStride(), format, header.GetScan0(dibBitmap));
                        ColorPalette pal = header.GetDrawingPalette(dibBitmap);
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
            }
            return finalImg;
        }

        static bool IsDib(IntPtr dibBitmap)
        {
            // a quick check not guaranteed correct,
            // compare first byte to size of struct (which is also the first field)
            var test = Marshal.ReadInt32(dibBitmap);
            // should be 40
            return test == BITMAPINFOHEADER.GetByteSize();
        }

        /// <summary>
        /// Converts a <see cref="Bitmap"/> to WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="image">The image to convert.</param>
        /// <returns></returns>
        public static BitmapSource ConvertToWpfBitmap(this Bitmap image)
        {
            if (image != null)
            {
                using (var hbm = new SafeHBitmapHandle(image.GetHbitmap(), true))
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

        internal static Bitmap ReadTiffImage(IntPtr data)
        {
            return null;
        }
    }
}
