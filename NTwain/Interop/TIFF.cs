using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Interop
{
    // this is from twain cs sample
    // http://sourceforge.net/projects/twainforcsharp/?source=typ_redirect

    /// <summary>
    /// The TIFF file header.
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
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct TIFFTAG
    {
        public ushort u16Tag;
        public ushort u16Type;
        public uint u32Count;
        public uint u32Value;
    }
}
