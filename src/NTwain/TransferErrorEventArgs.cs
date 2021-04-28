using System;
using TWAINWorkingGroup;

namespace NTwain
{
    /// <summary>
    /// Contains TWAIN codes and source status when an error is encountered during transfer.
    /// </summary>
    public class TransferErrorEventArgs : EventArgs
    {
        public TransferErrorEventArgs(STS code, Exception error = null)
        {
            Code = code;
            Exception = error;
        }

        /// <summary>
        /// Gets the error code from the transfer.
        /// </summary>
        public STS Code { get; }

        /// <summary>
        /// Gets the exception if the error is from some exception.
        /// </summary>
        public Exception Exception { get; }
    }
}
