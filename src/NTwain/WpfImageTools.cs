using System.IO;
using System.Windows.Media.Imaging;

namespace NTwain
{
    // this is in its own class to not depend on PresentationCore.dll on mono if it's not used.

    /// <summary>
    /// Contains extension methods for wpf images.
    /// </summary>
    public static class WpfImageTools
    {
        /// <summary>
        /// Loads a <see cref="Stream" /> into WPF <see cref="BitmapSource" />. The image created
        /// will be a copy so the stream can be disposed once this call returns.
        /// </summary>
        /// <param name="stream">The image stream.</param>
        /// <returns></returns>
        public static BitmapSource ConvertToWpfBitmap(this Stream stream)
        {
            return ConvertToWpfBitmap(stream, 0, 0);
        }

        /// <summary>
        /// Loads a <see cref="Stream" /> into WPF <see cref="BitmapSource" />. The image created
        /// will be a copy so the stream can be disposed once this call returns.
        /// </summary>
        /// <param name="stream">The image stream.</param>
        /// <param name="decodeWidth">Max width of the decoded image. Pass 0 to use default.</param>
        /// <param name="decodeHeight">Max height of the decoded image. Pass 0 to use default.</param>
        /// <returns></returns>
        public static BitmapSource ConvertToWpfBitmap(this Stream stream, int decodeWidth, int decodeHeight)
        {
            if (stream != null)
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.DecodePixelHeight = decodeHeight;
                image.DecodePixelWidth = decodeWidth;
                image.StreamSource = stream;
                image.EndInit();
                if (image.CanFreeze)
                {
                    image.Freeze();
                }
                return image;
            }
            return null;
        }


        ///// <summary>
        ///// Converts an <see cref="Image"/> to WPF <see cref="BitmapSource"/> if the image
        ///// is a <see cref="Bitmap"/>.
        ///// </summary>
        ///// <param name="image">The image to convert.</param>
        ///// <returns></returns>
        //public static BitmapSource ConvertToWpfBitmap(this Image image)
        //{
        //    var bmp = image as Bitmap;
        //    if (bmp != null)
        //    {
        //        using (var hbm = new SafeHBitmapHandle(bmp.GetHbitmap(), true))
        //        {
        //            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
        //               hbm.DangerousGetHandle(),
        //               IntPtr.Zero,
        //               System.Windows.Int32Rect.Empty,
        //               BitmapSizeOptions.FromEmptyOptions());
        //        }
        //    }
        //    return null;
        //}

        //class SafeHBitmapHandle : SafeHandleZeroOrMinusOneIsInvalid
        //{
        //    [SecurityCritical]
        //    public SafeHBitmapHandle(IntPtr preexistingHandle, bool ownsHandle)
        //        : base(ownsHandle)
        //    {
        //        SetHandle(preexistingHandle);
        //    }

        //    protected override bool ReleaseHandle()
        //    {
        //        return NativeMethods.DeleteObject(handle);
        //    }
        //}
    }
}
