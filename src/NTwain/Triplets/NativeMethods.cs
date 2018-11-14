using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    static partial class NativeMethods
    {
        const string EntryName = "DSM_Entry";

        const string WinDsmDll = "twaindsm.dll";
        const string LinuxDsmDll = "/usr/local/lib/libtwaindsm.so";
        const string MacDsmDll = "/Library/Frameworks/TWAINDSM.framework/TWAINDSM";

    }
}
