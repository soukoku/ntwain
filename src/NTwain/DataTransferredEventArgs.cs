using NTwain.Data;
using System;

namespace NTwain
{
  // TODO: maybe a 2-level "dispose" with end of event being 1
  // and manual dispose 2 for perf if this is not good enough.

  public class DataTransferredEventArgs : EventArgs
  {
    public DataTransferredEventArgs(TW_AUDIOINFO info, TW_SETUPFILEXFER xfer)
    {
      AudioInfo = info;
      File = xfer;
      IsFile = true;
    }
    public DataTransferredEventArgs(TW_AUDIOINFO info, byte[] data)
    {
      AudioInfo = info;
      Data = data;
    }

    public DataTransferredEventArgs(TW_IMAGEINFO info, TW_SETUPFILEXFER xfer)
    {
      ImageInfo = info;
      File = xfer;
      IsImage = true;
      IsFile = true;
    }
    public DataTransferredEventArgs(TW_IMAGEINFO info, byte[] data)
    {
      ImageInfo = info;
      Data = data;
      IsImage = true;
    }

    /// <summary>
    /// Whether transferred data is an image or audio.
    /// </summary>
    public bool IsImage { get; }

    /// <summary>
    /// Whether transfer was a file or memory data.
    /// </summary>
    public bool IsFile { get; }


    /// <summary>
    /// The complete file data if <see cref="IsFile"/> is false. 
    /// IMPORTANT: The data held
    /// in this array will no longer be valid once
    /// the event handler ends.
    /// </summary>
    public byte[]? Data { get; }

    /// <summary>
    /// The file info if <see cref="IsFile"/> is true.
    /// </summary>
    public TW_SETUPFILEXFER? File { get; }


    /// <summary>
    /// Gets the final image information if <see cref="IsImage"/> is true.
    /// </summary>
    public TW_IMAGEINFO ImageInfo { get; }


    /// <summary>
    /// Gets the final audio information if <see cref="IsImage"/> is false.
    /// </summary>
    public TW_AUDIOINFO AudioInfo { get; }

  }
}