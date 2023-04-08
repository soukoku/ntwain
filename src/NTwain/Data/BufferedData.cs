using System;

namespace NTwain.Data
{
  /// <summary>
  /// Simple struct with bytes buffer and the valid data length.
  /// </summary>
  public struct BufferedData
  {
    /// <summary>
    /// Bytes buffer. This may be bigger than the data size
    /// and contain invalid data.
    /// </summary>
    public byte[]? Buffer;

    /// <summary>
    /// Actual usable data length in the buffer.
    /// </summary>
    public int Length;

    /// <summary>
    /// As a span of usable data.
    /// </summary>
    /// <returns></returns>
    public ReadOnlySpan<byte> AsSpan()
    {
      if (Buffer != null) return Buffer.AsSpan(0, Length);
      return Span<byte>.Empty;
    }
  }
}
