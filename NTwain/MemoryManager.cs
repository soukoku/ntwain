using NTwain.Data;
using NTwain.Properties;
using System;

namespace NTwain
{
    /// <summary>
    /// Provides methods for managing memory on data exchanged with twain sources.
    /// This should only be used after the DSM has been opened.
    /// </summary>
    public class MemoryManager : IMemoryManager
    {
        static readonly MemoryManager _instance = new MemoryManager();
        /// <summary>
        /// Gets the singleton <see cref="MemoryManager"/> instance.
        /// </summary>
        public static MemoryManager Instance { get { return _instance; } }

        private MemoryManager() { }

        /// <summary>
        /// Updates the entry point used by TWAIN.
        /// </summary>
        /// <param name="entryPoint">The entry point.</param>
        internal void UpdateEntryPoint(TWEntryPoint entryPoint)
        {
            _twain2Entry = entryPoint;
        }

        TWEntryPoint _twain2Entry;

        /// <summary>
        /// Function to allocate memory. Calls to this must be coupled with <see cref="Free"/> later.
        /// </summary>
        /// <param name="size">The size in bytes.</param>
        /// <returns>Handle to the allocated memory.</returns>
        public IntPtr Allocate(uint size)
        {
            IntPtr retVal = IntPtr.Zero;

            if (_twain2Entry != null && _twain2Entry.AllocateFunction != null)
            {
                retVal = _twain2Entry.AllocateFunction(size);
            }
            else
            {
                // 0x0040 is GPTR
                retVal = NativeMethods.WinGlobalAlloc(0x0040, new UIntPtr(size));
            }

            if (retVal == IntPtr.Zero)
            {
                throw new OutOfMemoryException();
            }
            return retVal;
        }

        /// <summary>
        /// Function to free memory. 
        /// </summary>
        /// <param name="handle">The handle from <see cref="Allocate"/>.</param>
        public void Free(IntPtr handle)
        {
            if (_twain2Entry != null && _twain2Entry.FreeFunction != null)
            {
                _twain2Entry.FreeFunction(handle);
            }
            else
            {
                NativeMethods.WinGlobalFree(handle);
            }
        }

        /// <summary>
        /// Function to lock some memory. Calls to this must be coupled with <see cref="Unlock"/> later.
        /// </summary>
        /// <param name="handle">The handle to allocated memory.</param>
        /// <returns>Handle to the lock.</returns>
        public IntPtr Lock(IntPtr handle)
        {
            if (_twain2Entry != null && _twain2Entry.LockFunction != null)
            {
                return _twain2Entry.LockFunction(handle);
            }
            else
            {
                return NativeMethods.WinGlobalLock(handle);
            }
        }

        /// <summary>
        /// Function to unlock a previously locked memory region.
        /// </summary>
        /// <param name="handle">The handle from <see cref="Lock"/>.</param>
        public void Unlock(IntPtr handle)
        {
            if (_twain2Entry != null && _twain2Entry.UnlockFunction != null)
            {
                _twain2Entry.UnlockFunction(handle);
            }
            else
            {
                NativeMethods.WinGlobalUnlock(handle);
            }
        }
    }
}
