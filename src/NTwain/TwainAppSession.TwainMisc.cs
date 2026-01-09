using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NTwain;

// this file contains wrapped twain calls that are
// complicated to use.

partial class TwainAppSession
{
    /// <summary>
    /// Gets/sets the current source's settings as opaque data.
    /// Returns null if not supported. This is only valid in <see cref="STATE.S4"/>.
    /// </summary>
    public byte[]? CustomDsData
    {
        get
        {
            if (_currentDs == null) return null;

            var rc = DGControl.CustomDsData.Get(_appIdentity, _currentDs, out TW_CUSTOMDSDATA data);
            if (rc == TWRC.SUCCESS)
            {
                if (data.hData != IntPtr.Zero && data.InfoLength > 0)
                {
                    try
                    {
                        var lockedPtr = MemoryManager.Lock(data.hData);
                        var bytes = new byte[data.InfoLength];
                        Marshal.Copy(lockedPtr, bytes, 0, bytes.Length);
                        return bytes;
                    }
                    finally
                    {
                        MemoryManager.Unlock(data.hData);
                        MemoryManager.Free(data.hData);
                    }
                }
            }
            return null;
        }
        set
        {
            if (value == null || value.Length == 0 || _currentDs == null) return;

            TW_CUSTOMDSDATA data = default;
            data.InfoLength = (uint)value.Length;
            data.hData = MemoryManager.Alloc(data.InfoLength);
            try
            {
                var lockedPtr = MemoryManager.Lock(data.hData);
                Marshal.Copy(value, 0, lockedPtr, value.Length);
                MemoryManager.Unlock(data.hData);
                var rc = DGControl.CustomDsData.Set(_appIdentity, _currentDs, ref data);
            }
            finally
            {
                // should be freed already if no error but just in case
                if (data.hData != IntPtr.Zero) MemoryManager.Free(data.hData);
            }
        }
    }


    /// <summary>
    /// Enumerate file system items at current level.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<TW_FILESYSTEM> GetFileSystemItems()
    {
        if (_currentDs == null) yield break;

        TW_FILESYSTEM fs = default;
        for (var rc = DGControl.FileSystem.GetFirstFile(_appIdentity, _currentDs, ref fs);
            rc == TWRC.SUCCESS;
            rc = DGControl.FileSystem.GetNextFile(_appIdentity, _currentDs, ref fs))
        {
            yield return fs;
        }
    }

    /// <summary>
    /// Try to change to a different directory.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public STS ChangeFileSystemDirectory(TWFY type, string path)
    {
        if (_currentDs == null) return STS.SequenceError();

        return InvokeTriplet(() =>
        {
            TW_FILESYSTEM fs = new() { FileType = (int)type, InputName = path };
            return WrapInSTS(DGControl.FileSystem.ChangeDirectory(_appIdentity, _currentDs, ref fs));
        });
    }
}
