using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Provides methods for managing memory on data exchanged with twain sources.
    /// This should only be used after the DSM has been opened.
    /// </summary>
    public class MemoryManager : IMemoryManager
    {
        /// <summary>
        /// Gets the singleton <see cref="MemoryManager"/> instance.
        /// </summary>
        public static readonly MemoryManager Instance = new MemoryManager();

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
            if (_twain2Entry != null && _twain2Entry.AllocateFunction != null)
            {
                return _twain2Entry.AllocateFunction(size);
            }
            else
            {
                // 0x0040 is GPTR
                return WinGlobalAlloc(0x0040, new UIntPtr(size));
            }
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
                WinGlobalFree(handle);
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
                return WinGlobalLock(handle);
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
                WinGlobalUnlock(handle);
            }
        }

        #region old mem stuff for twain 1.x


        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalAlloc")]
        static extern IntPtr WinGlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalFree")]
        static extern IntPtr WinGlobalFree(IntPtr hMem);

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalLock")]
        static extern IntPtr WinGlobalLock(IntPtr handle);

        [DllImport("kernel32", SetLastError = true, EntryPoint = "GlobalUnlock")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool WinGlobalUnlock(IntPtr handle);

        #endregion
    }
}
