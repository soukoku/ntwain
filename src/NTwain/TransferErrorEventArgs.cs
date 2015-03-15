using NTwain.Data;
using System;

namespace NTwain
{
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
        public TransferErrorEventArgs(ReturnCode code, TWStatus status)
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
        public TWStatus SourceStatus { get; private set; }

        /// <summary>
        /// Gets the exception if the error is from some exception.
        /// </summary>
        /// <value>
        /// The exception.
        /// </value>
        public Exception Exception { get; private set; }
    }
}
