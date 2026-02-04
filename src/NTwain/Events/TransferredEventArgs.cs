using NTwain.Data;
using NTwain.Triplets;
using System;

namespace NTwain.Events;


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

    private BufferedData? _data;
    private bool _dataOwnershipTransferred;

    /// <summary>
    /// Gets the transferred data. 
    /// IMPORTANT: This data is only valid during the event handler execution.
    /// If you need to keep the data after the event handler returns, call <see cref="TakeDataOwnership"/> first.
    /// Otherwise, the data will be automatically disposed when the event completes.
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
        if (_twain == null || _twain.CurrentSource == null) return default;

        return _twain.WrapInSTS(DGImage.ExtImageInfo.Get(_twain.AppIdentity, _twain.CurrentSource, ref container));
    }

    /// <summary>
    /// Transfers ownership of the in-memory data to the caller, preventing automatic disposal.
    /// Use this when you need to process the data asynchronously or keep it beyond the event handler scope.
    /// After calling this, you MUST manually dispose the returned BufferedData when finished.
    /// </summary>
    /// <returns>
    /// The buffered data with transferred ownership, or null if no data exists or ownership was already transferred.
    /// </returns>
    /// <example>
    /// <code>
    /// private async void OnTransferred(TwainAppSession sender, TransferredEventArgs e)
    /// {
    ///     var data = e.TakeDataOwnership();  // Take ownership
    ///     if (data != null)
    ///     {
    ///         try
    ///         {
    ///             await ProcessDataAsync(data);  // Can use beyond event handler
    ///         }
    ///         finally
    ///         {
    ///             data.Dispose();  // Must dispose when done
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public BufferedData? TakeDataOwnership()
    {
        if (_dataOwnershipTransferred || _data == null)
            return null;

        _dataOwnershipTransferred = true;
        var data = _data;
        _data = null;
        return data;
    }

    public void Dispose()
    {
        // Only dispose if ownership wasn't transferred
        if (!_dataOwnershipTransferred && _data != null)
        {
            _data.Dispose();
            _data = null;
        }
    }
}