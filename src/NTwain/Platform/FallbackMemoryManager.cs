using System;
using System.Runtime.InteropServices;

namespace NTwain.Platform;

/// <summary>
/// Manages memory allocation, locking, and freeing operations for TWAIN sessions.
/// </summary>
internal class FallbackMemoryManager : IMemoryManager
{
    /// <inheritdoc/>
    public IntPtr Alloc(uint size)
    {
        return Marshal.AllocHGlobal((int)size);
    }

    /// <inheritdoc/>
    public void Free(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return;

        Marshal.FreeHGlobal(handle);
    }

    /// <inheritdoc/>
    public IntPtr Lock(IntPtr handle)
    {
        return handle;
    }

    /// <inheritdoc/>
    public void Unlock(IntPtr handle)
    {
    }
}
