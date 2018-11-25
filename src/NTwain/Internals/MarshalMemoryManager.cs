using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Internals
{
    // probably wrong

    class MarshalMemoryManager : IMemoryManager
    {
        public IntPtr Allocate(uint size)
        {
            return Marshal.AllocHGlobal((int)size);
        }

        public void Free(IntPtr handle)
        {
            Marshal.FreeHGlobal(handle);
        }

        public IntPtr Lock(IntPtr handle)
        {
            // no op
            return handle;
        }

        public void Unlock(IntPtr handle)
        {
            // no op
        }

    }
}
