using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain.Internals
{
    // probably wrong

    class LinuxMemoryManager : IMemoryManager
    {
        #region IMemoryManager Members

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
            return handle;
        }

        public void Unlock(IntPtr handle)
        {
            // no op
        }

        #endregion
    }
}
