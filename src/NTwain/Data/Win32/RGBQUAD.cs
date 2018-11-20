using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Data.Win32
{
    /// <summary>
    /// Describes a color consisting of relative intensities of red, green, and blue.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    struct RGBQUAD
    {
        /// <summary>
        /// The intensity of blue in the color.
        /// </summary>
        public byte rgbBlue;
        /// <summary>
        /// The intensity of green in the color.
        /// </summary>
        public byte rgbGreen;
        /// <summary>
        /// The intensity of red in the color.
        /// </summary>
        public byte rgbRed;
        /// <summary>
        /// This member is reserved and must be zero.
        /// </summary>
        byte rgbReserved;
    };
}
