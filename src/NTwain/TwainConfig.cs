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
    public class TwainConfig
    {
        internal TwainConfig() { }

        public bool Is64Bit { get; internal set; }

        public PlatformID Platform { get; internal set; }

        //public bool PreferLegacyDsm { get; internal set; }

        internal TW_IDENTITY AppWin32 { get; set; }

        internal TW_IDENTITY SrcWin32 { get; set; }


        public IMemoryManager MemoryManager { get; internal set; }
    }
}
