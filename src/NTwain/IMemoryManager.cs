using System;

namespace NTwain;

/// <summary>
/// Something that can do the 4 memory mgmt
/// things required by TWAIN.
/// </summary>
public interface IMemoryManager
{
    /// <summary>
    /// Allocates a block of memory of the specified size.
    /// </summary>
    /// <param name="size">The size in bytes to allocate.</param>
    /// <returns>A handle to the allocated memory block.</returns>
    IntPtr Alloc(uint size);

    /// <summary>
    /// Frees a previously allocated memory block.
    /// </summary>
    /// <param name="handle">The handle returned from <see cref="Alloc(uint)"/>.</param>
    void Free(IntPtr handle);

    /// <summary>
    /// Locks a memory block and returns a pointer to access its contents.
    /// </summary>
    /// <param name="handle">The handle returned from <see cref="Alloc(uint)"/>.</param>
    /// <returns>A pointer to the locked memory block.</returns>
    IntPtr Lock(IntPtr handle);

    /// <summary>
    /// Unlocks a previously locked memory block.
    /// </summary>
    /// <param name="handle">The handle returned from <see cref="Alloc(uint)"/>.</param>
    void Unlock(IntPtr handle);
}