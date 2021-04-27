///////////////////////////////////////////////////////////////////////////////////////
//
//  TwainWorkingGroup.TWAIN
//
//  This is a wrapper class for basic TWAIN functionality.  It establishes
//  behavior that every application should adhere to.  It also hides OS
//  specific details, so that toolkits or applications can use one unified
//  interface to TWAIN.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            TWAIN       Comment
//  M.McLaughlin    13-Mar-2019     2.4.0.3     Add language code page support for strings
//  M.McLaughlin    13-Nov-2015     2.4.0.0     Updated to latest spec
//  M.McLaughlin    13-Sep-2015     2.3.1.2     DsmMem bug fixes
//  M.McLaughlin    26-Aug-2015     2.3.1.1     Log fix and sync with TWAIN Direct
//  M.McLaughlin    13-Mar-2015     2.3.1.0     Numerous fixes
//  M.McLaughlin    13-Oct-2014     2.3.0.4     Added logging
//  M.McLaughlin    24-Jun-2014     2.3.0.3     Stability fixes
//  M.McLaughlin    21-May-2014     2.3.0.2     64-Bit Linux
//  M.McLaughlin    27-Feb-2014     2.3.0.1     AnyCPU support
//  M.McLaughlin    21-Oct-2013     2.3.0.0     Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2013-2020 Kodak Alaris Inc.
//
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TWAINWorkingGroup
{
    /// <summary>
    /// The header for a Bitmap file.
    /// Needed for supporting DAT.IMAGENATIVEXFER...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
    }

    /// <summary>
    /// The header for a Device Independent Bitmap (DIB).
    /// Needed for supporting DAT.IMAGENATIVEXFER...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public void Init()
        {
            biSize = (uint)Marshal.SizeOf(this);
        }
    }

    /// <summary>
    /// The TIFF file header.
    /// Needed for supporting DAT.IMAGENATIVEXFER...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TIFFHEADER
    {
        public ushort u8ByteOrder;
        public ushort u16Version;
        public uint u32OffsetFirstIFD;
        public ushort u16u16IFD;
    }

    /// <summary>
    /// An individual TIFF Tag.
    /// Needed for supporting DAT.IMAGENATIVEXFER...
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TIFFTAG
    {
        public ushort u16Tag;
        public ushort u16Type;
        public uint u32Count;
        public uint u32Value;
    }

    ///////////////////////////////////////////////////////////////////////////////
    // Private Definitions (TIFF): this stuff should have been here all along to
    // make it easier to share.  It's only needed when writing out files from
    // DAT_IMAGEMEMXFER data.
    ///////////////////////////////////////////////////////////////////////////////

    // A TIFF header is composed of tags...
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TiffTag
    {
        public TiffTag(ushort a_u16Tag, ushort a_u16Type, uint a_u32Count, uint a_u32Value)
        {
            u16Tag = a_u16Tag;
            u16Type = a_u16Type;
            u32Count = a_u32Count;
            u32Value = a_u32Value;
        }

        public ushort u16Tag;
        public ushort u16Type;
        public uint u32Count;
        public uint u32Value;
    }

    // TIFF header for Uncompressed BITONAL images...
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TiffBitonalUncompressed
    {
        // Constructor...
        public TiffBitonalUncompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
        {
            // Header...
            u16ByteOrder = 0x4949;
            u16Version = 42;
            u32OffsetFirstIFD = 8;

            // First IFD...
            u16IFD = 16;

            // Tags...
            tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
            tifftagSubFileType = new TiffTag(255, 3, 1, 1);
            tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
            tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
            tifftagBitsPerSample = new TiffTag(258, 3, 1, 1);
            tifftagCompression = new TiffTag(259, 3, 1, 1);
            tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 1);
            tifftagFillOrder = new TiffTag(266, 3, 1, 1);
            tifftagStripOffsets = new TiffTag(273, 4, 1, 222);
            tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 1);
            tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
            tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
            tifftagXResolution = new TiffTag(282, 5, 1, 206);
            tifftagYResolution = new TiffTag(283, 5, 1, 214);
            tifftagT4T6Options = new TiffTag(292, 4, 1, 0);
            tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

            // Footer...
            u32NextIFD = 0;
            u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
        }

        // Header...
        public ushort u16ByteOrder;
        public ushort u16Version;
        public uint u32OffsetFirstIFD;

        // First IFD...
        public ushort u16IFD;

        // Tags...
        public TiffTag tifftagNewSubFileType;
        public TiffTag tifftagSubFileType;
        public TiffTag tifftagImageWidth;
        public TiffTag tifftagImageLength;
        public TiffTag tifftagBitsPerSample;
        public TiffTag tifftagCompression;
        public TiffTag tifftagPhotometricInterpretation;
        public TiffTag tifftagFillOrder;
        public TiffTag tifftagStripOffsets;
        public TiffTag tifftagSamplesPerPixel;
        public TiffTag tifftagRowsPerStrip;
        public TiffTag tifftagStripByteCounts;
        public TiffTag tifftagXResolution;
        public TiffTag tifftagYResolution;
        public TiffTag tifftagT4T6Options;
        public TiffTag tifftagResolutionUnit;

        // Footer...
        public uint u32NextIFD;
        public ulong u64XResolution;
        public ulong u64YResolution;
    }

    // TIFF header for Group4 BITONAL images...
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TiffBitonalG4
    {
        // Constructor...
        public TiffBitonalG4(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
        {
            // Header...
            u16ByteOrder = 0x4949;
            u16Version = 42;
            u32OffsetFirstIFD = 8;

            // First IFD...
            u16IFD = 16;

            // Tags...
            tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
            tifftagSubFileType = new TiffTag(255, 3, 1, 1);
            tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
            tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
            tifftagBitsPerSample = new TiffTag(258, 3, 1, 1);
            tifftagCompression = new TiffTag(259, 3, 1, 4);
            tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 0);
            tifftagFillOrder = new TiffTag(266, 3, 1, 1);
            tifftagStripOffsets = new TiffTag(273, 4, 1, 222);
            tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 1);
            tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
            tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
            tifftagXResolution = new TiffTag(282, 5, 1, 206);
            tifftagYResolution = new TiffTag(283, 5, 1, 214);
            tifftagT4T6Options = new TiffTag(293, 4, 1, 0);
            tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

            // Footer...
            u32NextIFD = 0;
            u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
        }

        // Header...
        public ushort u16ByteOrder;
        public ushort u16Version;
        public uint u32OffsetFirstIFD;

        // First IFD...
        public ushort u16IFD;

        // Tags...
        public TiffTag tifftagNewSubFileType;
        public TiffTag tifftagSubFileType;
        public TiffTag tifftagImageWidth;
        public TiffTag tifftagImageLength;
        public TiffTag tifftagBitsPerSample;
        public TiffTag tifftagCompression;
        public TiffTag tifftagPhotometricInterpretation;
        public TiffTag tifftagFillOrder;
        public TiffTag tifftagStripOffsets;
        public TiffTag tifftagSamplesPerPixel;
        public TiffTag tifftagRowsPerStrip;
        public TiffTag tifftagStripByteCounts;
        public TiffTag tifftagXResolution;
        public TiffTag tifftagYResolution;
        public TiffTag tifftagT4T6Options;
        public TiffTag tifftagResolutionUnit;

        // Footer...
        public uint u32NextIFD;
        public ulong u64XResolution;
        public ulong u64YResolution;
    }

    // TIFF header for Uncompressed GRAYSCALE images...
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TiffGrayscaleUncompressed
    {
        // Constructor...
        public TiffGrayscaleUncompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
        {
            // Header...
            u16ByteOrder = 0x4949;
            u16Version = 42;
            u32OffsetFirstIFD = 8;

            // First IFD...
            u16IFD = 14;

            // Tags...
            tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
            tifftagSubFileType = new TiffTag(255, 3, 1, 1);
            tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
            tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
            tifftagBitsPerSample = new TiffTag(258, 3, 1, 8);
            tifftagCompression = new TiffTag(259, 3, 1, 1);
            tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 1);
            tifftagStripOffsets = new TiffTag(273, 4, 1, 198);
            tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 1);
            tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
            tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
            tifftagXResolution = new TiffTag(282, 5, 1, 182);
            tifftagYResolution = new TiffTag(283, 5, 1, 190);
            tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

            // Footer...
            u32NextIFD = 0;
            u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
        }

        // Header...
        public ushort u16ByteOrder;
        public ushort u16Version;
        public uint u32OffsetFirstIFD;

        // First IFD...
        public ushort u16IFD;

        // Tags...
        public TiffTag tifftagNewSubFileType;
        public TiffTag tifftagSubFileType;
        public TiffTag tifftagImageWidth;
        public TiffTag tifftagImageLength;
        public TiffTag tifftagBitsPerSample;
        public TiffTag tifftagCompression;
        public TiffTag tifftagPhotometricInterpretation;
        public TiffTag tifftagStripOffsets;
        public TiffTag tifftagSamplesPerPixel;
        public TiffTag tifftagRowsPerStrip;
        public TiffTag tifftagStripByteCounts;
        public TiffTag tifftagXResolution;
        public TiffTag tifftagYResolution;
        public TiffTag tifftagResolutionUnit;

        // Footer...
        public uint u32NextIFD;
        public ulong u64XResolution;
        public ulong u64YResolution;
    }

    // TIFF header for Uncompressed COLOR images...
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
    public struct TiffColorUncompressed
    {
        // Constructor...
        public TiffColorUncompressed(uint a_u32Width, uint a_u32Height, uint a_u32Resolution, uint a_u32Size)
        {
            // Header...
            u16ByteOrder = 0x4949;
            u16Version = 42;
            u32OffsetFirstIFD = 8;

            // First IFD...
            u16IFD = 14;

            // Tags...
            tifftagNewSubFileType = new TiffTag(254, 4, 1, 0);
            tifftagSubFileType = new TiffTag(255, 3, 1, 1);
            tifftagImageWidth = new TiffTag(256, 4, 1, a_u32Width);
            tifftagImageLength = new TiffTag(257, 4, 1, a_u32Height);
            tifftagBitsPerSample = new TiffTag(258, 3, 3, 182);
            tifftagCompression = new TiffTag(259, 3, 1, 1);
            tifftagPhotometricInterpretation = new TiffTag(262, 3, 1, 2);
            tifftagStripOffsets = new TiffTag(273, 4, 1, 204);
            tifftagSamplesPerPixel = new TiffTag(277, 3, 1, 3);
            tifftagRowsPerStrip = new TiffTag(278, 4, 1, a_u32Height);
            tifftagStripByteCounts = new TiffTag(279, 4, 1, a_u32Size);
            tifftagXResolution = new TiffTag(282, 5, 1, 188);
            tifftagYResolution = new TiffTag(283, 5, 1, 196);
            tifftagResolutionUnit = new TiffTag(296, 3, 1, 2);

            // Footer...
            u32NextIFD = 0;
            u16XBitsPerSample1 = 8;
            u16XBitsPerSample2 = 8;
            u16XBitsPerSample3 = 8;
            u64XResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
            u64YResolution = (ulong)0x100000000 + (ulong)a_u32Resolution;
        }

        // Header...
        public ushort u16ByteOrder;
        public ushort u16Version;
        public uint u32OffsetFirstIFD;

        // First IFD...
        public ushort u16IFD;

        // Tags...
        public TiffTag tifftagNewSubFileType;
        public TiffTag tifftagSubFileType;
        public TiffTag tifftagImageWidth;
        public TiffTag tifftagImageLength;
        public TiffTag tifftagBitsPerSample;
        public TiffTag tifftagCompression;
        public TiffTag tifftagPhotometricInterpretation;
        public TiffTag tifftagStripOffsets;
        public TiffTag tifftagSamplesPerPixel;
        public TiffTag tifftagRowsPerStrip;
        public TiffTag tifftagStripByteCounts;
        public TiffTag tifftagXResolution;
        public TiffTag tifftagYResolution;
        public TiffTag tifftagResolutionUnit;

        // Footer...
        public uint u32NextIFD;
        public ushort u16XBitsPerSample1;
        public ushort u16XBitsPerSample2;
        public ushort u16XBitsPerSample3;
        public ulong u64XResolution;
        public ulong u64YResolution;
    }

}
