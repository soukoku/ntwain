using NTwain.Data;
using System;

namespace NTwain.Events;

/// <summary>
/// Contains TWAIN codes and source status when an error is encountered during transfer.
/// </summary>
public class TransferErrorEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TransferErrorEventArgs"/> class.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <param name="phase"></param>
    public TransferErrorEventArgs(Exception error, string phase)
    {
        Exception = error;
        Phase = phase;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TransferErrorEventArgs"/> class.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="phase"></param>
    public TransferErrorEventArgs(STS code, string phase)
    {
        Code = code;
        Phase = phase;
    }

    /// <summary>
    /// Gets the transfer phase the error occurred in.
    /// </summary>
    public string Phase { get; private set; }

    /// <summary>
    /// Gets the return code or condition code if error is from DSM calls.
    /// </summary>
    public STS? Code { get; private set; }

    /// <summary>
    /// Gets the exception if the error is from some exception
    /// and not from TWAIN.
    /// </summary>
    public Exception? Exception { get; private set; }
}
