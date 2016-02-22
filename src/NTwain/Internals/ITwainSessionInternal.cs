using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NTwain.Internals
{
    /// <summary>
    /// Extends <see cref="ITwainSession"/> with extra stuff for internal use.
    /// </summary>
    interface ITwainSessionInternal : ITwainSession, ITripletControl
    {
        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <returns></returns>
        TWIdentity AppId { get; }

        MessageLoopHook MessageLoopHook { get; }

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
        ICommittable GetPendingStateChanger(int newState);

        void ChangeCurrentSource(DataSource source);

        void UpdateCallback();

        ReturnCode DisableSource();

        void SafeSyncableRaiseEvent(DataTransferredEventArgs e);
        void SafeSyncableRaiseEvent(TransferErrorEventArgs e);
        void SafeSyncableRaiseEvent(TransferReadyEventArgs e);

        ReturnCode EnableSource(SourceEnableMode mode, bool modal, IntPtr windowHandle);

        bool CloseDSRequested { get; }

        /// <summary>
        /// Gets the triplet operations defined for audio data group.
        /// </summary>
        DGAudio DGAudio { get; }
    }
}
