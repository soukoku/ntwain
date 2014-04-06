using System;
namespace NTwain
{
    /// <summary>
    /// Interface that provides the correct methods for managing memory on data exchanged with TWAIN sources.
    /// </summary>
    public interface IMemoryManager
    {
        /// <summary>
        /// Function to allocate memory. Calls to this must be coupled with <see cref="Free"/> later.
        /// </summary>
        /// <param name="size">The size in bytes.</param>
        /// <returns>Handle to the allocated memory.</returns>
        IntPtr Allocate(uint size);

        /// <summary>
        /// Function to free memory. 
        /// </summary>
        /// <param name="handle">The handle from <see cref="Allocate"/>.</param>
        void Free(IntPtr handle);

        /// <summary>
        /// Function to lock some memory. Calls to this must be coupled with <see cref="Unlock"/> later.
        /// </summary>
        /// <param name="handle">The handle to allocated memory.</param>
        /// <returns>Handle to the lock.</returns>
        IntPtr Lock(IntPtr handle);

        /// <summary>
        /// Function to unlock a previously locked memory region.
        /// </summary>
        /// <param name="handle">The handle from <see cref="Lock"/>.</param>
        void Unlock(IntPtr handle);
    }
}
