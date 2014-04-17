using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain
{
    /// <summary>
    /// Contains TWAIN codes and source status when an error is encountered during transfer.
    /// </summary>
    public class TransferErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the return code from the transfer.
        /// </summary>
        /// <value>
        /// The return code.
        /// </value>
        public ReturnCode ReturnCode { get; internal set; }

        /// <summary>
        /// Gets the source status.
        /// </summary>
        /// <value>
        /// The source status.
        /// </value>
        public TWStatus SourceStatus { get; internal set; }

        /// <summary>
        /// Gets the exception if the error is from some exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; internal set; }
    }
}
