using System.Runtime.InteropServices;

namespace NTwain.Data.Win32
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
    }
}
