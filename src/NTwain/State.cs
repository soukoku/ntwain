using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// The logical state of a TWAIN session.
    /// </summary>
    public enum State
    {
        /// <summary>
        /// Just a default value.
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// The starting state, corresponds to state 1.
        /// </summary>
        DsmUnloaded = 1,
        /// <summary>
        /// The DSM library has been loaded, corresponds to state 2.
        /// </summary>
        DsmLoaded = 2,
        /// <summary>
        /// The DSM has been opened, corresponds to state 3.
        /// </summary>
        DsmOpened = 3,
        /// <summary>
        /// A data source has been opened, corresponds to state 4.
        /// </summary>
        SourceOpened = 4,
        /// <summary>
        /// A data source has been enabled, corresponds to state 5.
        /// </summary>
        SourceEnabled = 5,
        /// <summary>
        /// Data is ready for transfer from the source, corresponds to state 6.
        /// </summary>
        TransferReady = 6,
        /// <summary>
        /// Data is being transferred, corresponds to state 7.
        /// </summary>
        Transferring = 7
    };

}
