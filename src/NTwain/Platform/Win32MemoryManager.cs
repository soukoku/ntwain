using System;
using System.Runtime.Versioning;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;

namespace NTwain.Platform;

/// <summary>
/// Manages memory allocation, locking, and freeing operations for TWAIN sessions.
/// </summary>
#if !NETFRAMEWORK
[SupportedOSPlatform("windows5.1.2600")]
#endif
internal class Win32MemoryManager : IMemoryManager
{
    /// <inheritdoc/>
    public IntPtr Alloc(uint size)
    {
        return PInvoke.GlobalAlloc(GLOBAL_ALLOC_FLAGS.GHND, size);
    }

    /// <inheritdoc/>
    public void Free(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return;

        PInvoke.GlobalFree((HGLOBAL)handle);
    }

    /// <inheritdoc/>
    public IntPtr Lock(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return IntPtr.Zero;

        unsafe
        {
            return (IntPtr)PInvoke.GlobalLock((HGLOBAL)handle);
        }
    }

    /// <inheritdoc/>
    public void Unlock(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return;

        PInvoke.GlobalUnlock((HGLOBAL)handle);
    }
}
