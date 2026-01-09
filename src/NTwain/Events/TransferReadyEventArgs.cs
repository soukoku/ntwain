using NTwain.Data;
using NTwain.Triplets;
using System;

namespace NTwain.Events;

/// <summary>
/// Contains event data when a data transfer is ready to be processed.
/// </summary>
public class TransferReadyEventArgs : EventArgs
{
    public TransferReadyEventArgs(TwainAppSession twain, TWSX imageXferMech, TWSX audioXferMech)
    {
        _twain = twain;
        XferType = XferType.Image;
        ImgXferMech = imageXferMech;
        AudXferMech = audioXferMech;
    }

    /// <summary>
    /// Update pending info and reset cancel flag.
    /// </summary>
    /// <param name="pending"></param>
    public void UpdatePending(ref TW_PENDINGXFERS pending)
    {
        Cancel = CancelType.None;
        PendingCount = pending.Count;
        EndOfJobFlag = (TWEJ)pending.EOJ;
    }

    /// <summary>
    /// Gets or sets whether to cancel the transfer.
    /// </summary>
    public CancelType Cancel { get; set; }

    /// <summary>
    /// Gets the end of job flag value for this transfer if job control is enabled.
    /// </summary>
    public TWEJ EndOfJobFlag { get; private set; }

    /// <summary>
    /// Choose the type of transfer to use with the current device.
    /// </summary>
    public XferType XferType { get; set; }

    /// <summary>
    /// Gets the current transfer mech if working with images.
    /// </summary>
    public TWSX ImgXferMech { get; }

    /// <summary>
    /// Gets the current transfer mech if working with audio.
    /// </summary>
    public TWSX AudXferMech { get; }

    /// <summary>
    /// Gets the known pending transfer count. This may not be appilicable 
    /// for certain scanning modes.
    /// </summary>
    public int PendingCount { get; private set; }

    private readonly TwainAppSession _twain;

    /// <summary>
    /// If the transfer mech is file-related,
    /// setup the file transfer options here.
    /// </summary>
    /// <param name="fileXfer"></param>
    /// <returns></returns>
    public STS SetupFileTransfer(ref TW_SETUPFILEXFER fileXfer)
    {
        if (_twain.CurrentSource == null) return default;

        return _twain.WrapInSTS(DGControl.SetupFileXfer.Set(_twain.AppIdentity, _twain.CurrentSource, ref fileXfer));
    }

    /// <summary>
    /// Gets the ext image info at this state for Kodak devices. Use any utility methods on it 
    /// to read the data. Remember to call <see cref="TW_EXTIMAGEINFO.Free(IMemoryManager)"/>
    /// when done.
    /// </summary>
    /// <param name="container">Container to query. Can be created with <see cref="TW_EXTIMAGEINFO.CreateRequest(TWEI[])"/></param>
    /// <returns></returns>
    public STS GetExtendedImageInfo(ref TW_EXTIMAGEINFO container)
    {
        if (_twain.CurrentSource == null) return default;

        return _twain.WrapInSTS(DGImage.ExtImageInfo.GetSpecial(_twain.AppIdentity, _twain.CurrentSource, ref container));
    }

    //TW_IMAGEINFO? _imgInfo;
    ///// <summary>
    ///// Gets the tentative image information for the current transfer if applicable.
    ///// This may differ from the final image depending on the transfer mode used (mostly when doing mem xfer).
    ///// </summary>
    //public TW_IMAGEINFO? PendingImageInfo
    //{
    //  get
    //  {
    //    // only get it if requested since it could be slow
    //    if (!_imgInfo.HasValue)
    //    {
    //      if (_twain.GetImageInfo(out TW_IMAGEINFO info).RC == TWRC.SUCCESS)
    //      {
    //        _imgInfo = info;
    //      }
    //    }
    //    return _imgInfo;
    //  }
    //}

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

public enum XferType
{
    Image,
    Audio
}

