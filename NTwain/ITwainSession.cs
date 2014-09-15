using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// General interface for a TWAIN session.
    /// </summary>
    public interface ITwainSession : IEnumerable<DataSource>, INotifyPropertyChanged
    {

        /// <summary>
        /// [Experimental] Gets or sets the optional synchronization context when not specifying a <see cref="MessageLoopHook"/> on <see cref="Open()"/>.
        /// This allows events to be raised on the thread associated with the context. This is experimental is not recommended for use.
        /// </summary>
        /// <value>
        /// The synchronization context.
        /// </value>
        SynchronizationContext SynchronizationContext { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        bool EnforceState { get; set; }


        /// <summary>
        /// Gets the currently open source.
        /// </summary>
        /// <value>
        /// The current source.
        /// </value>
        DataSource CurrentSource { get; }

        /// <summary>
        /// Gets or sets the default source for this application.
        /// While this can be get as long as the session is open,
        /// it can only be set at State 3.
        /// </summary>
        /// <value>
        /// The default source.
        /// </value>
        DataSource DefaultSource { get; set; }

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>The state.</value>
        int State { get; }


        /// <summary>
        /// Quick flag to check if the DSM has been opened.
        /// </summary>
        bool IsDsmOpen { get; }

        /// <summary>
        /// Quick flag to check if a source has been opened.
        /// </summary>
        bool IsSourceOpen { get; }

        /// <summary>
        /// Quick flag to check if a source has been enabled.
        /// </summary>
        bool IsSourceEnabled { get; }

        /// <summary>
        /// Quick flag to check if a source is in the transferring state.
        /// </summary>
        bool IsTransferring { get; }

        /// <summary>
        /// Try to show the built-in source selector dialog and return the selected source.
        /// This is not recommended and is only included for completeness.
        /// </summary>
        /// <returns></returns>
        DataSource ShowSourceSelector();


        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by 
        /// <see cref="Close" /> when done with a TWAIN session.
        /// </summary>
        /// <returns></returns>
        ReturnCode Open();

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by 
        /// <see cref="Close" /> when done with a TWAIN session.
        /// </summary>
        /// <param name="messageLoopHook">The message loop hook.</param>
        /// <returns></returns>
        ReturnCode Open(MessageLoopHook messageLoopHook);

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
        IEnumerable<DataSource> GetSources();

        /// <summary>
        /// Quick shortcut to open a source.
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        ReturnCode OpenSource(string sourceName);

        /// <summary>
        /// Gets the manager status. Only call this at state 2 or higher.
        /// </summary>
        /// <returns></returns>
        TWStatus GetStatus();

        /// <summary>
        /// Gets the manager status. Only call this at state 3 or higher.
        /// </summary>
        /// <returns></returns>
        TWStatusUtf8 GetStatusUtf8();


        /// <summary>
        /// Occurs when <see cref="State"/> has changed.
        /// </summary>
        event EventHandler StateChanged;
        /// <summary>
        /// Occurs when <see cref="CurrentSource"/> has changed.
        /// </summary>
        event EventHandler SourceChanged;
        /// <summary>
        /// Occurs when source has been disabled (back to state 4).
        /// </summary>
        event EventHandler SourceDisabled;
        /// <summary>
        /// Occurs when the source has generated an event.
        /// </summary>
        event EventHandler<DeviceEventArgs> DeviceEvent;
        /// <summary>
        /// Occurs when a data transfer is ready.
        /// </summary>
        event EventHandler<TransferReadyEventArgs> TransferReady;
        /// <summary>
        /// Occurs when data has been transferred.
        /// </summary>
        event EventHandler<DataTransferredEventArgs> DataTransferred;
        /// <summary>
        /// Occurs when an error has been encountered during transfer.
        /// </summary>
        event EventHandler<TransferErrorEventArgs> TransferError;

    }
}
