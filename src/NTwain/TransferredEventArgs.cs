using NTwain.Data;
using System;

namespace NTwain
{

  public class TransferredEventArgs : EventArgs, IDisposable
  {
    public TransferredEventArgs(TW_AUDIOINFO info, TW_SETUPFILEXFER fileInfo)
    {
      AudioInfo = info;
      FileInfo = fileInfo;
    }
    public TransferredEventArgs(TW_AUDIOINFO info, BufferedData data)
    {
      AudioInfo = info;
      _data = data;
    }

    public TransferredEventArgs(TwainAppSession twain, TW_IMAGEINFO info, TW_SETUPFILEXFER? fileInfo, BufferedData? data)
    {
      ImageInfo = info;
      FileInfo = fileInfo;
      IsImage = true;
      _data = data;
      _twain = twain;
    }

    TwainAppSession? _twain;

    /// <summary>
    /// Whether transferred data is an image or audio.
    /// </summary>
    public bool IsImage { get; }

    private readonly BufferedData? _data;
    /// <summary>
    /// The complete file data if memory was involved in the transfer. 
    /// IMPORTANT: Content of this array will not be valid once
    /// this event arg has been disposed.
    /// </summary>
    public BufferedData? Data => _data;

    /// <summary>
    /// The file info if the transfer involved file information.
    /// </summary>
    public TW_SETUPFILEXFER? FileInfo { get; }


    /// <summary>
    /// Gets the final image information if <see cref="IsImage"/> is true.
    /// </summary>
    public TW_IMAGEINFO ImageInfo { get; }


    /// <summary>
    /// Gets the final audio information if <see cref="IsImage"/> is false.
    /// </summary>
    public TW_AUDIOINFO AudioInfo { get; }

    /// <summary>
    /// Gets the ext image info. Use any utility methods on it 
    /// to read the data. Remember to call <see cref="TW_EXTIMAGEINFO.Free(IMemoryManager)"/>
    /// when done.
    /// </summary>
    /// <param name="container">Container to query. Can be created with <see cref="TW_EXTIMAGEINFO.CreateRequest(TWEI[])"/></param>
    /// <returns></returns>
    public STS GetExtendedImageInfo(ref TW_EXTIMAGEINFO container)
    {
      if (_twain == null) return default;
      return _twain.GetExtendedImageInfo(ref container);
    }

    public void Dispose()
    {
      _data?.Dispose();
    }
  }
}