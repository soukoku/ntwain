using System;
namespace NTwain
{
    /// <summary>
    /// Contains various platform requirements and conditions for TWAIN.
    /// </summary>
    public interface IPlatformInfo
    {
        /// <summary>
        /// Gets a value indicating whether the applicable TWAIN DSM library exists in the operating system.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the TWAIN DSM; otherwise, <c>false</c>.
        /// </value>
        bool DsmExists { get; }

        /// <summary>
        /// Gets the expected TWAIN DSM dll path.
        /// </summary>
        /// <value>
        /// The expected DSM path.
        /// </value>
        string ExpectedDsmPath { get; }

        /// <summary>
        /// Gets a value indicating whether the application is running in 64-bit.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application is 64-bit; otherwise, <c>false</c>.
        /// </value>
        bool IsApp64Bit { get; }

        /// <summary>
        /// Gets a value indicating whether this library is supported on current OS.
        /// Check the other platform properties to determine the reason if this is false.
        /// </summary>
        /// <value>
        /// <c>true</c> if this library is supported; otherwise, <c>false</c>.
        /// </value>
        bool IsSupported { get; }

        /// <summary>
        /// Gets the <see cref="IMemoryManager"/> for communicating with data sources.
        /// This should only be used when a <see cref="TwainSession"/> is open.
        /// </summary>
        /// <value>
        /// The memory manager.
        /// </value>
        IMemoryManager MemoryManager { get; }
    }
}
