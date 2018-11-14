using NTwain.Data;
using NTwain.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Contains settings and platform info for a <see cref="TwainSession"/>.
    /// </summary>
    public class TwainConfig
    {
        internal TwainConfig() { }

        /// <summary>
        /// Gets whether the app is running in 64bit.
        /// </summary>
        public bool Is64Bit { get; internal set; }

        /// <summary>
        /// Gets the platform the app is running on.
        /// </summary>
        public PlatformID Platform { get; internal set; }

        //public bool PreferLegacyDsm { get; internal set; }

        internal TW_IDENTITY AppWin32 { get; set; }

        internal TW_IDENTITY SrcWin32 { get; set; }

        /// <summary>
        /// Gets memory manager associated with a <see cref="TwainSession"/>.
        /// </summary>
        public IMemoryManager MemoryManager { get; internal set; }
    }
}
