using NTwain.Data;
using System;

namespace NTwain.Platform;

/// <summary>
/// Manages memory allocation, locking, and freeing operations for TWAIN sessions.
/// </summary>
internal class TwainMemoryManager : IMemoryManager
{
    private TW_ENTRYPOINT_DELEGATES _entryPoint;

    /// <summary>
    /// Initializes a new instance of the <see cref="TwainMemoryManager"/> class.
    /// </summary>
    /// <param name="entryPoint">The TWAIN entry point delegates for memory operations.</param>
    public TwainMemoryManager(TW_ENTRYPOINT_DELEGATES entryPoint)
    {
        _entryPoint = entryPoint;
    }

    /// <inheritdoc/>
    public IntPtr Alloc(uint size)
    {
        return _entryPoint.DSM_MemAllocate(size);
    }

    /// <inheritdoc/>
    public void Free(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return;

        _entryPoint.DSM_MemFree(handle);
    }

    /// <inheritdoc/>
    public IntPtr Lock(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return IntPtr.Zero;

        return _entryPoint.DSM_MemLock(handle);
    }

    /// <inheritdoc/>
    public void Unlock(IntPtr handle)
    {
        if (handle == IntPtr.Zero) return;

        _entryPoint.DSM_MemUnlock(handle);
    }
}
