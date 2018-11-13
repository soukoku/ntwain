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

        public bool PreferLegacyDsm { get; internal set; }

        public ITW_IDENTITY App { get; internal set; }

        internal IMemoryManager MemoryManager { get; set; }
    }
}
