using NTwain.Data;
using NTwain.Triplets;
using System;

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
        /// Loads the specified source into main memory and causes its initialization.
        /// Calls to this must be followed by
        /// <see cref="CloseSource" /> when not using it anymore.
        /// </summary>
        /// <param name="sourceProductName">Name of the source.</param>
        /// <returns></returns>
        ReturnCode OpenSource(string sourceProductName);

        /// <summary>
        /// When an application is finished with a Source, it must formally close the session between them
        /// using this operation. This is necessary in case the Source only supports connection with a single
        /// application (many desktop scanners will behave this way). A Source such as this cannot be
        /// accessed by other applications until its current session is terminated
        /// </summary>
        /// <returns></returns>
        ReturnCode CloseSource();

        /// <summary>
        /// Enables the source to start transferring.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI will display as modal.</param>
        /// <param name="windowHandle">The window handle if modal.</param>
        /// <returns></returns>
        ReturnCode EnableSource(SourceEnableMode mode, bool modal, IntPtr windowHandle);

        /// <summary>
        /// Forces the stepping down of an opened source when things gets out of control.
        /// Used when session state and source state become out of sync.
        /// </summary>
        /// <param name="targetState">State of the target.</param>
        void ForceStepDown(int targetState);
    }
}
