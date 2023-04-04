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
    /// <param name="status">Additional status info from TWAIN.</param>
    public TransferErrorEventArgs(STS code, string? info)
    {
      Code = code;
      Info = info;
    }

    /// <summary>
    /// Gets the return code or condition code from the transfer.
    /// </summary>
    public STS Code { get; private set; }

    /// <summary>
    /// Gets the additional status info from TWAIN.
    /// </summary>
    public string? Info { get; private set; }

    /// <summary>
    /// Gets the exception if the error is from some exception
    /// and not from TWAIN.
    /// </summary>
    public Exception? Exception { get; private set; }
  }
}
