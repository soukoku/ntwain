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

        const string WinDll = "twaindsm.dll";
        const string LinuxDll = "/usr/local/lib/libtwaindsm.so";
        const string Mac32Dll = "/System/Library/Frameworks/TWAIN.framework/TWAIN";
        const string Mac64Dll = "/Library/Frameworks/TWAINDSM.framework/TWAINDSM";

    }
}
