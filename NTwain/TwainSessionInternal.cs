using NTwain.Data;
using NTwain.Internals;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace NTwain
{
    // for internal pieces since the main twain session file is getting too long

    partial class TwainSession
    {
        #region ITwainSessionInternal Members

        MessageLoopHook _msgLoopHook;
        MessageLoopHook ITwainSessionInternal.MessageLoopHook { get { return _msgLoopHook; } }

        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <value>The app id.</value>
        TWIdentity ITwainSessionInternal.AppId { get { return _appId; } }

        void ITwainSessionInternal.ChangeState(int newState, bool notifyChange)
        {
            _state = newState;
            if (notifyChange)
            {
                OnPropertyChanged("State");
                SafeAsyncSyncableRaiseOnEvent(OnStateChanged, StateChanged);
            }
        }

        ICommittable ITwainSessionInternal.GetPendingStateChanger(int newState)
        {
            return new TentativeStateCommitable(this, newState);
        }

        void ITwainSessionInternal.ChangeCurrentSource(TwainSource source)
        {
            CurrentSource = source;
            OnPropertyChanged("CurrentSource");
            SafeAsyncSyncableRaiseOnEvent(OnSourceChanged, SourceChanged);
        }

        void ITwainSessionInternal.SafeSyncableRaiseEvent(DataTransferredEventArgs e)
        {
            SafeSyncableRaiseOnEvent(OnDataTransferred, DataTransferred, e);
        }
        void ITwainSessionInternal.SafeSyncableRaiseEvent(TransferErrorEventArgs e)
        {
            SafeSyncableRaiseOnEvent(OnTransferError, TransferError, e);
        }
        void ITwainSessionInternal.SafeSyncableRaiseEvent(TransferReadyEventArgs e)
        {
            SafeSyncableRaiseOnEvent(OnTransferReady, TransferReady, e);
        }

        DGAudio _dgAudio;
        DGAudio ITwainSessionInternal.DGAudio
        {
            get
            {
                if (_dgAudio == null) { _dgAudio = new DGAudio(this); }
                return _dgAudio;
            }
        }

        DGControl _dgControl;
        DGControl ITwainSessionInternal.DGControl
        {
            get
            {
                if (_dgControl == null) { _dgControl = new DGControl(this); }
                return _dgControl;
            }
        }

        DGImage _dgImage;
        DGImage ITwainSessionInternal.DGImage
        {
            get
            {
                if (_dgImage == null) { _dgImage = new DGImage(this); }
                return _dgImage;
            }
        }

        DGCustom _dgCustom;
        DGCustom ITwainSessionInternal.DGCustom
        {
            get
            {
                if (_dgCustom == null) { _dgCustom = new DGCustom(this); }
                return _dgCustom;
            }
        }


        /// <summary>
        /// Enables the source to start transferring.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI will display as modal.</param>
        /// <param name="windowHandle">The window handle if modal.</param>
        /// <returns></returns>
        ReturnCode ITwainSessionInternal.EnableSource(SourceEnableMode mode, bool modal, IntPtr windowHandle)
        {
            var rc = ReturnCode.Failure;

            _msgLoopHook.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: EnableSource with {1}.", Thread.CurrentThread.ManagedThreadId, mode));

                // app v2.2 or higher uses callback2
                if (_appId.ProtocolMajor >= 2 && _appId.ProtocolMinor >= 2)
                {
                    var cb = new TWCallback2(HandleCallback);
                    var rc2 = ((ITwainSessionInternal)this).DGControl.Callback2.RegisterCallback(cb);

                    if (rc2 == ReturnCode.Success)
                    {
                        Debug.WriteLine("Registered callback2 OK.");
                        _callbackObj = cb;
                    }
                }
                else
                {
                    var cb = new TWCallback(HandleCallback);

                    var rc2 = ((ITwainSessionInternal)this).DGControl.Callback.RegisterCallback(cb);

                    if (rc2 == ReturnCode.Success)
                    {
                        Debug.WriteLine("Registered callback OK.");
                        _callbackObj = cb;
                    }
                }

                _twui = new TWUserInterface();
                _twui.ShowUI = mode == SourceEnableMode.ShowUI;
                _twui.ModalUI = modal;
                _twui.hParent = windowHandle;

                if (mode == SourceEnableMode.ShowUIOnly)
                {
                    rc = ((ITwainSessionInternal)this).DGControl.UserInterface.EnableDSUIOnly(_twui);
                }
                else
                {
                    rc = ((ITwainSessionInternal)this).DGControl.UserInterface.EnableDS(_twui);
                }

                if (rc != ReturnCode.Success)
                {
                    _callbackObj = null;
                }
            });
            return rc;
        }

        bool _disabling;
        ReturnCode ITwainSessionInternal.DisableSource()
        {
            var rc = ReturnCode.Failure;
            if (!_disabling) // temp hack as a workaround to this being called from multiple threads (xfer logic & closedsreq msg)
            {
                _disabling = true;
                try
                {
                    _msgLoopHook.Invoke(() =>
                    {
                        Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: DisableSource.", Thread.CurrentThread.ManagedThreadId));

                        rc = ((ITwainSessionInternal)this).DGControl.UserInterface.DisableDS(_twui);
                        if (rc == ReturnCode.Success)
                        {
                            _callbackObj = null;
                            SafeAsyncSyncableRaiseOnEvent(OnSourceDisabled, SourceDisabled);
                        }
                    });
                }
                finally
                {
                    _disabling = false;
                }
            }
            return rc;
        }


        #endregion

    }
}
