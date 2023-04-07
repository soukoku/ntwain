using NTwain.Data;
using System;

namespace NTwain
{
  public class DataTransferredEventArgs : EventArgs
  {
    readonly TwainAppSession _twain;
    readonly bool _isImage;

    /// <summary>
    /// Ctor for array data;
    /// </summary>
    /// <param name="twain"></param>
    /// <param name="data"></param>
    internal DataTransferredEventArgs(TwainAppSession twain, byte[] data, bool isImage)
    {
      this._twain = twain;
      Data = data;
      this._isImage = isImage;
    }


    /// <summary>
    /// The complete file data if the transfer was done
    /// through memory. IMPORTANT: The data held
    /// in this array will no longer be valid once
    /// the event handler ends.
    /// </summary>
    public byte[]? Data { get; }


    TW_IMAGEINFO? _imgInfo;

    /// <summary>
    /// Gets the final image information if applicable.
    /// </summary>
    public TW_IMAGEINFO? ImageInfo
    {
      get
      {
        if (_isImage && _imgInfo == null)
        {
          if (_twain.GetImageInfo(out TW_IMAGEINFO info).RC == TWRC.SUCCESS)
          {
            _imgInfo = info;
          }
        }
        return _imgInfo;
      }
    }

  }
}
