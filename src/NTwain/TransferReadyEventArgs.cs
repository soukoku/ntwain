using NTwain.Data;
using System;

namespace NTwain
{
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
        /// <param name="endOfJob">if set to <c>true</c> then it's the end of job.</param>
        public TransferReadyEventArgs(DataSource source, int pendingCount, bool endOfJob)
        {
            DataSource = source;
            PendingTransferCount = pendingCount;
            EndOfJob = endOfJob;
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
        public bool EndOfJob { get; private set; }

        /// <summary>
        /// Gets the known pending transfer count. This may not be appilicable 
        /// for certain scanning modes.
        /// </summary>
        /// <value>The pending count.</value>
        public int PendingTransferCount { get; private set; }

        TWImageInfo _imgInfo;
        /// <summary>
        /// Gets the tentative image information for the current transfer if applicable.
        /// This may differ from the final image depending on the transfer mode used (mostly when doing mem xfer).
        /// </summary>
        /// <value>
        /// The image info.
        /// </value>
        public TWImageInfo PendingImageInfo
        {
            get
            {
                if (_imgInfo == null)
                {
                    if (DataSource.DGImage.ImageInfo.Get(out _imgInfo) != ReturnCode.Success)
                    {
                        _imgInfo = null;
                    }
                }
                return _imgInfo;
            }
        }

        TWAudioInfo _audInfo;
        /// <summary>
        /// Gets the audio information for the current transfer if applicable.
        /// </summary>
        /// <value>
        /// The audio information.
        /// </value>
        public TWAudioInfo AudioInfo
        {
            get
            {
                if (_audInfo == null)
                {
                    if (DataSource.DGAudio.AudioInfo.Get(out _audInfo) != ReturnCode.Success)
                    {
                        _audInfo = null;
                    }
                }
                return _audInfo;
            }
        }

    }
}
