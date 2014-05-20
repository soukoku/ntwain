using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;

namespace NTwain
{
    /// <summary>
    /// Interface for TWAIN triplet operations. 
    /// </summary>
    public interface ITwainOperation
    {
        /// <summary>
        /// Gets the triplet operations defined for audio data group.
        /// </summary>
        DGAudio DGAudio { get; }

        /// <summary>
        /// Gets the triplet operations defined for control data group.
        /// </summary>
        DGControl DGControl { get; }

        /// <summary>
        /// Gets the triplet operations defined for image data group.
        /// </summary>
        DGImage DGImage { get; }

        /// <summary>
        /// Gets the direct triplet operation entry for custom values.
        /// </summary>
        DGCustom DGCustom { get; }

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by <see cref="CloseManager"/> when done with a TWAIN session.
        /// </summary>
        /// <returns></returns>
        ReturnCode OpenManager();

        /// <summary>
        /// Closes the data source manager.
        /// </summary>
        /// <returns></returns>
        ReturnCode CloseManager();

        /// <summary>
        /// Forces the stepping down of an opened source when things gets out of control.
        /// Used when session state and source state become out of sync.
        /// </summary>
        /// <param name="targetState">State of the target.</param>
        void ForceStepDown(int targetState);

        /// <summary>
        /// Gets list of sources available in the system.
        /// </summary>
        /// <returns></returns>
        IList<TwainSource> GetSources();

        /// <summary>
        /// Gets the manager status. Only call this at state 2 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        TWStatus GetStatus();
    }
}
