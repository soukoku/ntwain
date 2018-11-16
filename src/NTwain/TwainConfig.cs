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

        /// <summary>
        /// 32bit version of the app identity.
        /// </summary>
        internal TW_IDENTITY App32 { get; set; }

        //internal TW_IDENTITY Src32 { get; set; }

        IMemoryManager _memMgr;
        /// <summary>
        /// Gets memory manager associated with a <see cref="TwainSession"/>.
        /// </summary>
        public IMemoryManager MemoryManager
        {
            get
            {
                return _memMgr ?? DefaultMemoryManager;
            }
            internal set { _memMgr = value; }
        }

        internal IMemoryManager DefaultMemoryManager { get; set; }
    }
}
