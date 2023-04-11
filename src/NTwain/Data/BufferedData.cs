using System;
using System.Buffers;

namespace NTwain.Data
{
  /// <summary>
  /// Simple thing with shared bytes buffer and the valid data length.
  /// </summary>
  public class BufferedData : IDisposable
  {
    // experiment using array pool for things transferred in memory.
    // this can pool up to a "normal" max of legal size paper in 24 bit at 300 dpi (~31MB)
    // so the array max is made with 32 MB. Typical usage should be a lot less.
    internal static readonly ArrayPool<byte> MemPool = ArrayPool<byte>.Create(32 * 1024 * 1024, 8);

    public BufferedData(int size)
    {
      _buffer = MemPool.Rent(size);
      _length = size;
      _fromPool = true;
    }

    internal BufferedData(byte[] data, int size, bool fromPool)
    {
      _buffer = data;
      _length = size;
      _fromPool = fromPool;
    }

    bool _fromPool;

    /// <summary>
    /// Bytes buffer. This may be bigger than the data size
    /// and contain invalid data.
    /// </summary>
    byte[]? _buffer;
    public byte[]? Buffer { get; }

    /// <summary>
    /// Actual usable data length in the buffer.
    /// </summary>
    int _length;

    /// <summary>
    /// As a span of usable data.
    /// </summary>
    /// <returns></returns>
    public ReadOnlySpan<byte> AsSpan()
    {
      if (_buffer != null) return _buffer.AsSpan(0, _length);
      return Span<byte>.Empty;
    }

    public void Dispose()
    {
      if (_fromPool && _buffer != null)
      {
        MemPool.Return(_buffer);
        _buffer = null;
      }
    }
  }
}
