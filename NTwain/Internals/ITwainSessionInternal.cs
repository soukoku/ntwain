using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Threading;

namespace NTwain.Internals
{
    interface ITwainSessionInternal : ITwainSession
    {
        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <returns></returns>
        TWIdentity AppId { get; }

        MessageLoopHook MessageLoopHook { get; }

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
        ICommittable GetPendingStateChanger(int newState);

        void ChangeSourceId(TwainSource source);

        ReturnCode DisableSource();

        void SafeSyncableRaiseEvent(DataTransferredEventArgs e);
        void SafeSyncableRaiseEvent(TransferErrorEventArgs e);
        void SafeSyncableRaiseEvent(TransferReadyEventArgs e);

        ReturnCode EnableSource(SourceEnableMode mode, bool modal, IntPtr windowHandle);
        SynchronizationContext SynchronizationContext { get; set; }

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
    }
}
