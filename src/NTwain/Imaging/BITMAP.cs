using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Windows.Win32.Graphics.Gdi;

// this is a good read
// http://atlc.sourceforge.net/bmp.html


partial struct BITMAPINFOHEADER
{
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
        return (uint)Marshal.SizeOf<BITMAPINFOHEADER>();
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


    ///// <summary>
    ///// Gets the System.Drawing pixel format of current structure.
    ///// </summary>
    ///// <returns></returns>
    //public PixelFormat GetDrawingPixelFormat()
    //{
    //  switch (biBitCount)
    //  {
    //    case 1:
    //      return PixelFormat.Format1bppIndexed;
    //    case 4:
    //      return PixelFormat.Format4bppIndexed;
    //    case 8:
    //      return PixelFormat.Format8bppIndexed;
    //    case 16:
    //      return PixelFormat.Format16bppRgb565;
    //    case 24:
    //      return PixelFormat.Format24bppRgb;
    //    case 32:
    //      return PixelFormat.Format32bppRgb;
    //    case 48:
    //      return PixelFormat.Format48bppRgb;
    //  }
    //  return PixelFormat.DontCare;
    //}

    ///// <summary>
    ///// Gets the color palette that's contained in the header.
    ///// Note not all images will have palette, so check if the return value
    ///// is null before using it.
    ///// </summary>
    ///// <returns></returns>
    //public ColorPalette? GetDrawingPalette(IntPtr headerPtr)
    //{
    //  //if (format == PixelFormat.Format8bppIndexed)
    //  //{
    //  //    // update color palette to grayscale version
    //  //    ColorPalette grayPallet = bitmap.Palette;
    //  //    for (int i = 0; i < grayPallet.Entries.Length; i++)
    //  //    {
    //  //        grayPallet.Entries[i] = Color.FromArgb(i, i, i);
    //  //    }
    //  //    bitmap.Palette = grayPallet; // this is what makes the gray pallet take effect
    //  //}

    //  if (biClrUsed > 0)
    //  {
    //    byte[] data = new byte[biClrUsed * 4];
    //    Marshal.Copy(new IntPtr(headerPtr.ToInt32() + biSize), data, 0, data.Length);
    //    var dummy = new System.Drawing.Bitmap(1, 1, GetDrawingPixelFormat());
    //    ColorPalette pal = dummy.Palette;
    //    dummy.Dispose();
    //    int index = 0;
    //    int setCount = data.Length / 4;
    //    for (int i = 0; i < setCount; i++)
    //    {
    //      index = i * 4;
    //      pal.Entries[i] = Color.FromArgb(data[index + 2], data[index + 1], data[index]);
    //    }
    //    return pal;
    //  }
    //  return null;
    //}

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

    /// <inheritdoc/>
    public override string ToString()
    {
        return new StringBuilder().Append("BitmapInfoHeader:")
            .Append("\r\n\tSize = " + biSize)
            .Append("\r\n\tWidth = " + biWidth)
            .Append("\r\n\tHeight = " + biHeight)
            .Append("\r\n\tPlanes = " + biPlanes)
            .Append("\r\n\tBitCount = " + biBitCount)
            .Append("\r\n\tCompression = " + Compression)
            .Append("\r\n\tSizeImage = " + biSizeImage)
            .Append("\r\n\tXPixelsPerMeter = " + biXPelsPerMeter)
            .Append("\r\n\tYPixelsPerMeter = " + biYPelsPerMeter)
            .Append("\r\n\tColorUsed = " + biClrUsed)
            .Append("\r\n\tColorImportant = " + biClrImportant).ToString();
    }

    /// <summary>
    /// Gets the bitmap compression type.
    /// </summary>
    public CompressionType Compression { get { return (CompressionType)biCompression; } }

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
}

//[StructLayout(LayoutKind.Sequential, Pack = 1)]
//struct BITMAPFILEHEADER
//{
//    public ushort bfType;
//    public uint bfSize;
//    public ushort bfReserved1;
//    public ushort bfReserved2;
//    public uint bfOffBits;
//}
