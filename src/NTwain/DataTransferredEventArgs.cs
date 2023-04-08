using NTwain.Data;
using System;

namespace NTwain
{
  // TODO: a 2-level dispose with end of event method
  // and manual dispose for perf if this is not good enough.

  public class DataTransferredEventArgs : EventArgs
  {
    readonly TwainAppSession _twain;
    readonly bool _isImage;

    /// <summary>
    /// Ctor for array data;
    /// </summary>
    /// <param name="twain"></param>
    /// <param name="isImage"></param>
    /// <param name="data"></param>
    internal DataTransferredEventArgs(TwainAppSession twain, bool isImage, byte[] data)
    {
      _twain = twain;
      _isImage = isImage;
      Data = data;
    }


    /// <summary>
    /// The complete file data if the transfer was done
    /// through memory. IMPORTANT: The data held
    /// in this array will no longer be valid once
    /// the event handler ends.
    /// </summary>
    public byte[]? Data { get; }


    /// <summary>
    /// Gets the final image information if transfer was an image.
    /// </summary>
    public TW_IMAGEINFO? GetImageInfo()
    {
      if (_isImage && _twain.GetImageInfo(out TW_IMAGEINFO info).RC == TWRC.SUCCESS)
      {
        return info;
      }
      return null;

    }
  }
}