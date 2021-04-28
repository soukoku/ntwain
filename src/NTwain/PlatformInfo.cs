using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTwain
{
    /// <summary>
    /// Contains info on current platform, process, and TWAIN stuff.
    /// </summary>
    public static class PlatformInfo
    {
        /// <summary>
        /// Whether the current OS is windows.
        /// </summary>
        public static bool IsWindows { get; }

        /// <summary>
        /// Whether the current OS is linux.
        /// </summary>
        public static bool IsLinux { get; }

        /// <summary>
        /// Whether the current OS is MacOSX.
        /// </summary>
        public static bool IsMacOSX { get; }

        /// <summary>
        /// Whether the application is running in 64-bit.
        /// </summary>
        public static bool IsApp64Bit { get; set; }

        static PlatformInfo()
        {
            var platform = Environment.OSVersion.Platform;
            IsWindows = platform == PlatformID.Win32NT;
            IsLinux = platform == PlatformID.Unix;
            IsMacOSX = platform == PlatformID.MacOSX;
            IsApp64Bit = IntPtr.Size == 8;
        }
    }
}
