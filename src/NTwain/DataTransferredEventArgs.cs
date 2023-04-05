using System;
using System.Buffers;

namespace NTwain
{
  public class DataTransferredEventArgs : EventArgs, IDisposable
  {
    /// <summary>
    /// Ctor for pooled data and the pool to clean it up.
    /// </summary>
    /// <param name="pool"></param>
    /// <param name="data"></param>
    internal DataTransferredEventArgs(ArrayPool<byte>? pool, byte[]? data)
    {
      _pool = pool;
      Data = data;
    }


    bool _disposed;
    private readonly ArrayPool<byte>? _pool;

    /// <summary>
    /// The complete file data if the transfer was done
    /// through memory. IMPORTANT: The data held
    /// in this array will no longer be valid once
    /// this event arg has been disposed.
    /// </summary>
    public byte[]? Data { get; private set; }



    public void Dispose()
    {
      if (!_disposed)
      {
        if (_pool != null && Data != null)
        {
          _pool.Return(Data);
          Data = null;
        }
        _disposed = true;
      }
    }
  }
}
