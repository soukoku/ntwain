using NTwain.Data;
using NTwain.Triplets;
using System;
using System.ComponentModel;
namespace NTwain
{
    /// <summary>
    /// Interface for keeping track of current TWAIN state with current app and source ids.
    /// </summary>
    public interface ITwainState : INotifyPropertyChanged
    {   
        /// <summary>
        /// Gets the source id used for the session.
        /// </summary>
        /// <value>The source id.</value>
        TWIdentity SourceId { get; }

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>The state.</value>
        int State { get; }
    }


    /// <summary>
    /// Internal interface for state management.
    /// </summary>
    interface ITwainStateInternal : ITwainState
    {
        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <returns></returns>
        TWIdentity GetAppId();

        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        bool EnforceState { get; set; }

        /// <summary>
        /// Changes the state right away.
        /// </summary>
        /// <param name="newState">The new state.</param>
        /// <param name="notifyChange">if set to <c>true</c> to notify change.</param>
        void ChangeState(int newState, bool notifyChange);

        /// <summary>
        /// Gets the pending state changer and tentatively changes the session state to the specified value.
        /// Value will only stick if committed.
        /// </summary>
        /// <param name="newState">The new state.</param>
        /// <returns></returns>
        ICommitable GetPendingStateChanger(int newState);

        void ChangeSourceId(TWIdentity sourceId);
    }

    interface ICommitable : IDisposable
    {
        void Commit();
    }
}
