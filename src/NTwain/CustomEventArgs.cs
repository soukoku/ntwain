using NTwain.Data;
using System;

namespace NTwain
{
    /// <summary>
    /// Contains data for a TWAIN source event.
    /// </summary>
    public class DeviceEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the detailed device event.
        /// </summary>
        public TW_DEVICEEVENT Data { get; internal set; }
    }

    /// <summary>
    /// Contains data when data source came down from being enabled.
    /// </summary>
    public class SourceDisabledEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the affected source.
        /// </summary>
        public DataSource Source { get; internal set; }

        /// <summary>
        /// Whether the source was enabled with UI-only (no transfer).
        /// The app could do something different if this is <code>true</code>, such as
        /// getting the <see cref="TW_CUSTOMDSDATA"/>.
        /// </summary>
        public bool UIOnly { get; internal set; }
    }


    /// <summary>
    /// Contains event data when a data transfer is ready to be processed.
    /// </summary>
    public class TransferReadyEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferReadyEventArgs"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="pendingCount">The pending count.</param>
        /// <param name="endOfJobFlag"></param>
        public TransferReadyEventArgs(DataSource source, int pendingCount, EndXferJob endOfJobFlag)
        {
            DataSource = source;
            PendingTransferCount = pendingCount;
            EndOfJobFlag = endOfJobFlag;
        }

        /// <summary>
        /// Gets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        public DataSource DataSource { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current transfer should be canceled
        /// and continue next transfer if there are more data.
        /// </summary>
        /// <value><c>true</c> to cancel current transfer; otherwise, <c>false</c>.</value>
        public bool CancelCurrent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all transfers should be canceled.
        /// </summary>
        /// <value><c>true</c> to cancel all transfers; otherwise, <c>false</c>.</value>
        public bool CancelAll { get; set; }

        /// <summary>
        /// Gets a value indicating whether current transfer signifies an end of job in TWAIN world.
        /// </summary>
        /// <value><c>true</c> if transfer is end of job; otherwise, <c>false</c>.</value>
        public bool EndOfJob { get { return EndOfJobFlag != EndXferJob.None; } }

        /// <summary>
        /// Gets the end of job flag value for this transfer (if job control is enabled).
        /// </summary>
        /// <value>
        /// The end of job flag.
        /// </value>
        public EndXferJob EndOfJobFlag { get; private set; }

        /// <summary>
        /// Gets the known pending transfer count. This may not be appilicable 
        /// for certain scanning modes.
        /// </summary>
        /// <value>The pending count.</value>
        public int PendingTransferCount { get; private set; }

        TW_IMAGEINFO? _imgInfo;
        /// <summary>
        /// Gets the tentative image information for the current transfer if applicable.
        /// This may differ from the final image depending on the transfer mode used (mostly when doing mem xfer).
        /// </summary>
        /// <value>
        /// The image info.
        /// </value>
        public TW_IMAGEINFO? PendingImageInfo
        {
            get
            {
                if (!_imgInfo.HasValue)
                {
                    TW_IMAGEINFO img = default;
                    if (DataSource.Session.DGImage.ImageInfo.Get(ref img) == ReturnCode.Success)
                    {
                        _imgInfo = img;
                    }
                }
                return _imgInfo;
            }
        }

        TW_AUDIOINFO? _audInfo;
        /// <summary>
        /// Gets the audio information for the current transfer if applicable.
        /// </summary>
        /// <value>
        /// The audio information.
        /// </value>
        public TW_AUDIOINFO? AudioInfo
        {
            get
            {
                if (_audInfo == null)
                {
                    TW_AUDIOINFO aud = default;
                    if (DataSource.Session.DGAudio.AudioInfo.Get(ref aud) == ReturnCode.Success)
                    {
                        _audInfo = aud;
                    }
                }
                return _audInfo;
            }
        }

    }

    /// <summary>
    /// Contains TWAIN codes and source status when an error is encountered during transfer.
    /// </summary>
    public class TransferErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferErrorEventArgs"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public TransferErrorEventArgs(Exception error)
        {
            Exception = error;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferErrorEventArgs"/> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="status">The status.</param>
        public TransferErrorEventArgs(ReturnCode code, TW_STATUS status)
        {
            ReturnCode = code;
            SourceStatus = status;
        }

        /// <summary>
        /// Gets the return code from the transfer.
        /// </summary>
        /// <value>
        /// The return code.
        /// </value>
        public ReturnCode ReturnCode { get; private set; }

        /// <summary>
        /// Gets the source status.
        /// </summary>
        /// <value>
        /// The source status.
        /// </value>
        public TW_STATUS SourceStatus { get; private set; }

        /// <summary>
        /// Gets the exception if the error is from some exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; private set; }
    }
}
