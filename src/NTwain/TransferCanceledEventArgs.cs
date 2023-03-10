using System;

namespace NTwain
{
    /// <summary>
    /// Indicates a transfer cancellation, e.g. if the user pressed the "Cancel" button.
    /// </summary>
    public class TransferCanceledEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransferCanceledEventArgs"/> class.
        /// </summary>
        public TransferCanceledEventArgs()
        {
        }
    }
}
