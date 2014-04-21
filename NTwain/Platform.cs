using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Class for checking various platform requirements and conditions.
    /// </summary>
    public static class Platform
    {
        // Change pinvoke base on where running in 64bit mode.
        // Theoretically [DllImport("twaindsm", EntryPoint = "#1")] 
        // works on both 32 and 64 bit
        // but it's not installed on either system by default.
        // A proper 64 bit twain driver would've installed it so  
        // in essence it only exists in 64 bit systems and thus
        // the 2 sets of identical pinvokes for windows :(

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Platform()
        {
            IsApp64bit = IntPtr.Size == 8;
            NewWinDsmExists = File.Exists(Path.Combine(Environment.SystemDirectory, "twaindsm.dll"));

            UseNewDSM = IsApp64bit || NewWinDsmExists;

            IsOnMono = Type.GetType("Mono.Runtime") != null;
            IsWin = Environment.OSVersion.Platform == PlatformID.Win32NT;
            IsLinux = Environment.OSVersion.Platform == PlatformID.Unix;

            _defaultMemManager = new WinMemoryManager();
        }

        internal static readonly bool UseNewDSM;

        internal static readonly bool IsApp64bit;
        internal static readonly bool NewWinDsmExists;

        internal static readonly bool IsOnMono;

        internal static readonly bool IsWin;
        internal static readonly bool IsLinux;

        /// <summary>
        /// Gets a value indicating whether this library is supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this library is supported; otherwise, <c>false</c>.
        /// </value>
        public static bool IsSupported
        {
            get
            {
                if (IsWin)
                {
                    if (IsApp64bit) { return NewWinDsmExists; }
                    return true;
                }
                return IsOnMono && IsLinux;
            }
        }


        static readonly WinMemoryManager _defaultMemManager;
        static IMemoryManager _specifiedMemManager;

        /// <summary>
        /// Gets the <see cref="IMemoryManager"/> for communicating with data sources.
        /// </summary>
        /// <value>
        /// The memory manager.
        /// </value>
        public static IMemoryManager MemoryManager
        {
            get
            {
                if (_specifiedMemManager == null) { return _defaultMemManager; }
                return _specifiedMemManager;
            }
            internal set
            {
                _specifiedMemManager = value;
            }
        }
    }
}
