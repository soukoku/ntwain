using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Data
{
    /// <summary>
    /// The logical state of a TWAIN session.
    /// </summary>
    public enum TwainState
    {
        /// <summary>
        /// Just a default value.
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// The starting state, nothing loaded.
        /// </summary>
        S1 = 1,
        /// <summary>
        /// The DSM library has been loaded.
        /// </summary>
        S2 = 2,
        /// <summary>
        /// The DSM has been opened.
        /// </summary>
        S3 = 3,
        /// <summary>
        /// A data source has been opened, ready for configuration.
        /// </summary>
        S4 = 4,
        /// <summary>
        /// A data source has been enabled, GUI up or waiting to transfer first image.
        /// </summary>
        S5 = 5,
        /// <summary>
        /// Data is ready for transfer from the source.
        /// </summary>
        S6 = 6,
        /// <summary>
        /// Data is being transferred.
        /// </summary>
        S7 = 7,

        /// <summary>
        /// The starting state, nothing loaded.
        /// </summary>
        DsmUnloaded = S1,
        /// <summary>
        /// The DSM library has been loaded.
        /// </summary>
        DsmLoaded = S2,
        /// <summary>
        /// The DSM has been opened.
        /// </summary>
        DsmOpened = S3,
        /// <summary>
        /// A data source has been opened, ready for configuration.
        /// </summary>
        SourceOpened = S4,
        /// <summary>
        /// A data source has been enabled, GUI up or waiting to transfer first image.
        /// </summary>
        SourceEnabled = S5,
        /// <summary>
        /// Data is ready for transfer from the source.
        /// </summary>
        TransferReady = S6,
        /// <summary>
        /// Data is being transferred.
        /// </summary>
        Transferring = S7
    }

}
