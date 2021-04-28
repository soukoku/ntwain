using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Contains event data when a data transfer is ready to be processed.
    /// </summary>
    public class TransferReadyEventArgs : EventArgs
    {
        public TransferReadyEventArgs(TWAIN twain, int pendingCount, TWEJ endOfJobFlag)
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
        public CancelType CancelCapture { get; set; }

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
        private readonly TWAIN _twain;

        /// <summary>
        /// Gets the tentative image information for the current transfer if applicable.
        /// This may differ from the final image depending on the transfer mode used (mostly when doing mem xfer).
        /// </summary>
        public TW_IMAGEINFO? PendingImageInfo
        {
            get
            {
                if (!_imgInfo.HasValue)
                {
                    TW_IMAGEINFO info = default;
                    if (_twain.DatImageinfo(DG.IMAGE, MSG.GET, ref info) == STS.SUCCESS)
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
        /// Stops feeder but continue receiving scanned images.
        /// </summary>
        Graceful,
        /// <summary>
        /// Stops feeder and discard pending images.
        /// </summary>
        Immediate
    }
}
