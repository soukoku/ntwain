using System;
namespace NTwain
{
    /// <summary>
    /// Interface that provides the correct methods for managing memory on data exchanged with TWAIN sources.
    /// </summary>
    public interface IMemoryManager
    {
        /// <summary>
        /// Function to allocate memory. Calls to this must be coupled with <see cref="MemFree"/> later.
        /// </summary>
        /// <param name="size">The size in bytes.</param>
        /// <returns>Handle to the allocated memory.</returns>
        IntPtr MemAllocate(uint size);

        /// <summary>
        /// Function to free memory. 
        /// </summary>
        /// <param name="handle">The handle from <see cref="MemAllocate"/>.</param>
        void MemFree(IntPtr handle);

        /// <summary>
        /// Function to lock some memory. Calls to this must be coupled with <see cref="MemUnlock"/> later.
        /// </summary>
        /// <param name="handle">The handle to allocated memory.</param>
        /// <returns>Handle to the lock.</returns>
        IntPtr MemLock(IntPtr handle);

        /// <summary>
        /// Function to unlock a previously locked memory region.
        /// </summary>
        /// <param name="handle">The handle from <see cref="MemLock"/>.</param>
        void MemUnlock(IntPtr handle);
    }
}
