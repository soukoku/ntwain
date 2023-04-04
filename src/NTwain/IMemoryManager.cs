using System;

namespace NTwain
{
  /// <summary>
  /// Something that can do the 4 memory mgmt
  /// things required by TWAIN.
  /// </summary>
  public interface IMemoryManager
  {
    IntPtr Alloc(uint size);
    void Free(IntPtr handle);
    IntPtr Lock(IntPtr handle);
    void Unlock(IntPtr handle);
  }
}