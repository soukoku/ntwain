using NTwain.Data;
using System;

namespace NTwain
{
  /// <summary>
  /// Contains event data when a data transfer is ready to be processed.
  /// </summary>
  public class TransferReadyEventArgs : EventArgs
  {
    public TransferReadyEventArgs(TwainAppSession twain, int pendingCount, TWEJ endOfJobFlag)
    {
      _twain = twain;
      PendingCount = pendingCount;
      EndOfJobFlag = endOfJobFlag;
    }


    /// <summary>
    /// Gets or sets whether the current transfer should be skipped
    /// and continue next transfer if there are more data.
    /// </summary>
    public bool SkipCurrent { get; set; }

    /// <summary>
    /// Gets or sets whether to cancel the capture phase.
    /// </summary>
    public CancelType Cancel { get; set; }

    /// <summary>
    /// Gets the end of job flag value for this transfer if job control is enabled.
    /// </summary>
    public TWEJ EndOfJobFlag { get; private set; }

    /// <summary>
    /// Gets the known pending transfer count. This may not be appilicable 
    /// for certain scanning modes.
    /// </summary>
    public int PendingCount { get; private set; }

    TW_IMAGEINFO? _imgInfo;
    private readonly TwainAppSession _twain;

    /// <summary>
    /// Gets the tentative image information for the current transfer if applicable.
    /// This may differ from the final image depending on the transfer mode used (mostly when doing mem xfer).
    /// </summary>
    public TW_IMAGEINFO? PendingImageInfo
    {
      get
      {
        // only get it if requested since it could be slow
        if (!_imgInfo.HasValue)
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

  public enum CancelType
  {
    /// <summary>
    /// No cancel.
    /// </summary>
    None,
    /// <summary>
    /// Skips current transfer.
    /// </summary>
    SkipCurrent,
    /// <summary>
    /// Stops feeder but continue receiving already scanned images in the app.
    /// </summary>
    Graceful,
    /// <summary>
    /// Stops feeder and discard any pending images.
    /// </summary>
    EndNow
  }
}
