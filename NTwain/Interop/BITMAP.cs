using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Interop
{
    // this is a good read
    // http://atlc.sourceforge.net/bmp.html

    /// <summary>
    /// Defines the dimensions and color information for a DIB.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct BITMAPINFO
    {
        /// <summary>
        /// Structure that contains information about the dimensions of color format.
        /// </summary>
        public BITMAPINFOHEADER bmiHeader;
        /// <summary>
        /// This contains one of the following:
        /// 1. An array of RGBQUAD. The elements of the array that make up the color table.
        /// 2. An array of 16-bit unsigned integers that specifies indexes into the currently realized logical palette. This use of bmiColors is allowed for functions that use DIBs.
        /// The number of entries in the array depends on the values of the biBitCount and biClrUsed members of the BITMAPINFOHEADER structure.
        /// </summary>
        public IntPtr bmiColors;

    };

    /// <summary>
    /// Structure that contains information about the dimensions and color format of a DIB.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct BITMAPINFOHEADER
    {
        #region fields
        /// <summary>
        /// The number of bytes required by the structure.
        /// </summary>
        public uint biSize;
        /// <summary>
        /// The width of the bitmap, in pixels.
        /// If Compression is JPEG or PNG, the Width member specifies the width of the decompressed 
        /// JPEG or PNG image file, respectively.
        /// </summary>
        public int biWidth;
        /// <summary>
        /// The height of the bitmap, in pixels. If Height is positive, 
        /// the bitmap is a bottom-up DIB and its origin is the lower-left corner. 
        /// If Height is negative, the bitmap is a top-down DIB and its origin is the upper-left corner.
        /// If Height is negative, indicating a top-down DIB, Compression must be either RGB or BITFIELDS. Top-down DIBs cannot be compressed.
        /// If Compression is JPEG or PNG, the Height member specifies the height of the decompressed JPEG or PNG image file, respectively.
        /// </summary>
        public int biHeight;
        /// <summary>
        /// The number of planes for the target device. This value must be set to 1.
        /// </summary>
        public ushort biPlanes;
        /// <summary>
        /// The number of bits-per-pixel. The BitCount member 
        /// determines the number of bits that define each pixel and the maximum number of colors in the bitmap. 
        /// </summary>
        public ushort biBitCount;
        /// <summary>
        /// The type of compression for a compressed bottom-up bitmap (top-down DIBs cannot be compressed).
        /// </summary>
        public CompressionType biCompression;
        /// <summary>
        /// The size, in bytes, of the image. This may be set to zero for RGB bitmaps.
        /// If Compression is JPEG or PNG, SizeImage indicates the size of the JPEG or PNG image buffer, respectively.
        /// </summary>
        public uint biSizeImage;
        /// <summary>
        /// The horizontal resolution, in pixels-per-meter, of the target device for the bitmap. 
        /// An application can use this value to select a bitmap from a resource group that 
        /// best matches the characteristics of the current device.
        /// </summary>
        public int biXPelsPerMeter;
        /// <summary>
        /// The vertical resolution, in pixels-per-meter, of the target device for the bitmap.
        /// </summary>
        public int biYPelsPerMeter;
        /// <summary>
        /// The number of color indexes in the color table that are actually used by the bitmap. 
        /// If this value is zero, the bitmap uses the maximum number of colors corresponding to 
        /// the value of the BitCount member for the compression mode specified by Compression.
        /// </summary>
        public uint biClrUsed;
        /// <summary>
        /// The number of color indexes that are required for displaying the bitmap. 
        /// If this value is zero, all colors are required.
        /// </summary>
        public uint biClrImportant;
        #endregion

        #region utilities

        const double METER_INCH_RATIO = 39.3700787;

        /// <summary>
        /// Gets the horizontal dpi of the bitmap.
        /// </summary>
        /// <returns></returns>
        public float GetXDpi()
        {
            return (float)Math.Round(biXPelsPerMeter / METER_INCH_RATIO, 0);
        }
        /// <summary>
        /// Gets the vertical dpi of the bitmap.
        /// </summary>
        /// <returns></returns>
        public float GetYDpi()
        {
            return (float)Math.Round(biYPelsPerMeter / METER_INCH_RATIO, 0);
        }
        /// <summary>
        /// Gets the size of the structure.
        /// </summary>
        /// <returns></returns>
        public static uint GetByteSize()
        {
            return (uint)Marshal.SizeOf(typeof(BITMAPINFOHEADER));
        }
        /// <summary>
        /// Checks to see if this structure contain valid data.
        /// It also fills in any missing pieces if possible.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            if (biHeight != 0 && biWidth != 0 && biBitCount != 0)
            {
                if (biSize == 0)
                {
                    biSize = GetByteSize();
                }
                if (biClrUsed == 0)
                {
                    switch (biBitCount)
                    {
                        case 1:
                            biClrUsed = 2;
                            break;
                        case 4:
                            biClrUsed = 16;
                            break;
                        case 8:
                            biClrUsed = 256;
                            break;
                    }
                }
                if (biSizeImage == 0)
                {
                    biSizeImage = (uint)((((
                        biWidth * biBitCount) + 31) & ~31) >> 3) * (uint)Math.Abs(biHeight);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the pointer to scan0 given the header pointer.
        /// </summary>
        /// <param name="headerPtr">The header PTR.</param>
        /// <returns></returns>
        public IntPtr GetScan0(IntPtr headerPtr)
        {
            int p = (int)biClrUsed;
            if ((p == 0) && (biBitCount <= 8))
            {
                p = 1 << biBitCount;
            }
            p = (p * 4) + (int)biSize + headerPtr.ToInt32();
            return new IntPtr(p);
        }

        /// <summary>
        /// Gets whether the bitmap is bottom-up or top-down format.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is bottom up image; otherwise, <c>false</c>.
        /// </value>
        /// <returns></returns>
        public bool IsBottomUpImage
        {
            get
            {
                return biHeight > 0;
            }
        }


        /// <summary>
        /// Gets the System.Drawing pixel format of current structure.
        /// </summary>
        /// <returns></returns>
        public PixelFormat GetDrawingPixelFormat()
        {
            switch (biBitCount)
            {
                case 1:
                    return PixelFormat.Format1bppIndexed;
                case 4:
                    return PixelFormat.Format4bppIndexed;
                case 8:
                    return PixelFormat.Format8bppIndexed;
                case 16:
                    return PixelFormat.Format16bppRgb565;
                case 24:
                    return PixelFormat.Format24bppRgb;
                case 32:
                    return PixelFormat.Format32bppRgb;
                case 48:
                    return PixelFormat.Format48bppRgb;
            }
            return PixelFormat.DontCare;
        }
        /// <summary>
        /// Gets the color palette that's contained in the header.
        /// Note not all images will have palette, so check if the return value
        /// is null before using it.
        /// </summary>
        /// <returns></returns>
        public ColorPalette GetDrawingPalette(IntPtr headerPtr)
        {
            //if (format == PixelFormat.Format8bppIndexed)
            //{
            //    // update color palette to grayscale version
            //    ColorPalette grayPallet = bitmap.Palette;
            //    for (int i = 0; i < grayPallet.Entries.Length; i++)
            //    {
            //        grayPallet.Entries[i] = Color.FromArgb(i, i, i);
            //    }
            //    bitmap.Palette = grayPallet; // this is what makes the gray pallet take effect
            //}

            if (biClrUsed > 0)
            {
                byte[] data = new byte[biClrUsed * 4];
                Marshal.Copy(new IntPtr(headerPtr.ToInt32() + biSize), data, 0, data.Length);
                var dummy = new System.Drawing.Bitmap(1, 1, GetDrawingPixelFormat());
                ColorPalette pal = dummy.Palette;
                dummy.Dispose();
                int index = 0;
                int setCount = data.Length / 4;
                for (int i = 0; i < setCount; i++)
                {
                    index = i * 4;
                    pal.Entries[i] = Color.FromArgb(data[index + 2], data[index + 1], data[index]);
                }
                return pal;
            }
            return null;
        }

        /// <summary>
        /// Gets the stride size of this bitmap.
        /// </summary>
        /// <returns></returns>
        public int GetStride()
        {
            int bitsPerRow = (biBitCount * biWidth);
            int strideTest = bitsPerRow / 8 + (bitsPerRow % 8 != 0 ? 1 : 0);
            int overage = strideTest % 4;
            if (overage > 0)
            {
                strideTest += (4 - overage);
            }
            return strideTest;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return new StringBuilder().Append("BitmapInfoHeader:")
                .Append("\r\n\tSize = " + biSize)
                .Append("\r\n\tWidth = " + biWidth)
                .Append("\r\n\tHeight = " + biHeight)
                .Append("\r\n\tPlanes = " + biPlanes)
                .Append("\r\n\tBitCount = " + biBitCount)
                .Append("\r\n\tCompression = " + biCompression)
                .Append("\r\n\tSizeImage = " + biSizeImage)
                .Append("\r\n\tXPixelsPerMeter = " + biXPelsPerMeter)
                .Append("\r\n\tYPixelsPerMeter = " + biYPelsPerMeter)
                .Append("\r\n\tColorUsed = " + biClrUsed)
                .Append("\r\n\tColorImportant = " + biClrImportant).ToString();
        }
        #endregion

        /// <summary>
        /// Indicates the bitmap compression of <seealso cref="BITMAPINFOHEADER"/>.
        /// </summary>
        public enum CompressionType : uint
        {
            /// <summary>
            /// An uncompressed format.
            /// </summary>
            BI_RGB = 0,
            /// <summary>
            ///  A run-length encoded (RLE) format for bitmaps with 8 bpp. The compression format is a 2-byte format consisting of a count byte followed by a byte containing a color index. For more information, see Bitmap Compression. 
            /// </summary>
            BI_RLE8 = 1,
            /// <summary>
            ///  An RLE, format for bitmaps with 4 bpp. The compression format is a 2-byte format consisting of a count byte followed by two word-length color indexes. For more information, see Bitmap Compression. 
            /// </summary>
            BI_RLE4 = 2,
            /// <summary>
            /// Specifies that the bitmap is not compressed and that the color table consists of three DWORD color masks that specify the red, green, and blue components of each pixel. 
            /// This is valid when used with 16- and 32-bpp bitmaps. 
            /// </summary>
            BI_BITFIELDS = 3,
            /// <summary>
            /// Indicates that the image is a JPEG image.
            /// </summary>
            BI_JPEG = 4,
            /// <summary>
            /// Indicates that the image is a PNG image.
            /// </summary>
            BI_PNG = 5
        }
    };

}
