using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// General interface for a TWAIN session.
    /// </summary>
    public interface ITwainSession : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the currently open source.
        /// </summary>
        /// <value>
        /// The current source.
        /// </value>
        TwainSource CurrentSource { get; }

        /// <summary>
        /// Gets or sets the default source for this application.
        /// While this can be get as long as the session is open,
        /// it can only be set at State 3.
        /// </summary>
        /// <value>
        /// The default source.
        /// </value>
        TwainSource DefaultSource { get; set; }

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>The state.</value>
        int State { get; }

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
        /// Try to show the built-in source selector dialog and return the selected source.
        /// This is not recommended and is only included for completeness.
        /// </summary>
        /// <returns></returns>
        TwainSource ShowSourceSelector();

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by <see cref="Close"/> when done with a TWAIN session.
        /// </summary>
        /// <returns></returns>
        ReturnCode Open();

        /// <summary>
        /// Closes the data source manager.
        /// </summary>
        /// <returns></returns>
        ReturnCode Close();

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
        IEnumerable<TwainSource> GetSources();

        /// <summary>
        /// Gets the manager status. Only call this at state 2 or higher.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <returns></returns>
        TWStatus GetStatus();
    }
}
