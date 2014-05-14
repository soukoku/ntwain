using NTwain.Data;
using NTwain.Internals;
using NTwain.Properties;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// Basic class for interfacing with TWAIN. You should only have one of this per application process.
    /// </summary>
    public class TwainSession : ITwainSessionInternal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSession" /> class.
        /// </summary>
        /// <param name="appId">The app id that represents calling application.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSession(TWIdentity appId)
        {
            if (appId == null) { throw new ArgumentNullException("appId"); }

            _appId = appId;
            ((ITwainSessionInternal)this).ChangeState(1, false);
            EnforceState = true;

            MessageLoop.Instance.EnsureStarted(HandleWndProcMessage);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        object _callbackObj; // kept around so it doesn't get gc'ed
        TWIdentity _appId;
        TWUserInterface _twui;

        /// <summary>
        /// Gets or sets the optional synchronization context.
        /// This allows events to be raised on the thread
        /// associated with the context.
        /// </summary>
        /// <value>
        /// The synchronization context.
        /// </value>
        public SynchronizationContext SynchronizationContext { get; set; }


        #region ITwainSessionInternal Members

        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <value>The app id.</value>
        TWIdentity ITwainSessionInternal.AppId { get { return _appId; } }

        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        public bool EnforceState { get; set; }

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

        void ITwainSessionInternal.ChangeSourceId(TWIdentity sourceId)
        {
            SourceId = sourceId;
            OnPropertyChanged("SourceId");
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

        #endregion

        #region ITwainSession Members

        /// <summary>
        /// Gets the source id used for the session.
        /// </summary>
        /// <value>
        /// The source id.
        /// </value>
        public TWIdentity SourceId { get; private set; }

        int _state;
        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public int State
        {
            get { return _state; }
            private set
            {
                if (value > 0 && value < 8)
                {
                    _state = value;
                    OnPropertyChanged("State");
                    SafeAsyncSyncableRaiseOnEvent(OnStateChanged, StateChanged);
                }
            }
        }


        static readonly CapabilityId[] _emptyCapList = new CapabilityId[0];

        private IList<CapabilityId> _supportedCaps;
        /// <summary>
        /// Gets the supported caps for the currently open source.
        /// </summary>
        /// <value>
        /// The supported caps.
        /// </value>
        public IList<CapabilityId> SupportedCaps
        {
            get
            {
                if (_supportedCaps == null && State > 3)
                {
                    _supportedCaps = this.GetCapabilities();
                }
                return _supportedCaps ?? _emptyCapList;
            }
            private set
            {
                _supportedCaps = value;
                OnPropertyChanged("SupportedCaps");
            }
        }

        #endregion

        #region ITwainOperation Members

        DGAudio _dgAudio;
        /// <summary>
        /// Gets the triplet operations defined for audio data group.
        /// </summary>
        public DGAudio DGAudio
        {
            get
            {
                if (_dgAudio == null) { _dgAudio = new DGAudio(this); }
                return _dgAudio;
            }
        }

        DGControl _dgControl;
        /// <summary>
        /// Gets the triplet operations defined for control data group.
        /// </summary>
        public DGControl DGControl
        {
            get
            {
                if (_dgControl == null) { _dgControl = new DGControl(this); }
                return _dgControl;
            }
        }

        DGImage _dgImage;
        /// <summary>
        /// Gets the triplet operations defined for image data group.
        /// </summary>
        public DGImage DGImage
        {
            get
            {
                if (_dgImage == null) { _dgImage = new DGImage(this); }
                return _dgImage;
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var syncer = SynchronizationContext;
            if (syncer == null)
            {
                try
                {
                    var hand = PropertyChanged;
                    if (hand != null) { hand(this, new PropertyChangedEventArgs(propertyName)); }
                }
                catch { }
            }
            else
            {
                syncer.Post(o =>
                {
                    try
                    {
                        var hand = PropertyChanged;
                        if (hand != null) { hand(this, new PropertyChangedEventArgs(propertyName)); }
                    }
                    catch { }
                }, null);
            }
        }

        #endregion

        #region privileged calls that causes state change in TWAIN

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by <see cref="CloseManager"/> when done with a TWAIN session.
        /// </summary>
        /// <returns></returns>
        public ReturnCode OpenManager()
        {
            var rc = ReturnCode.Failure;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: OpenManager.", Thread.CurrentThread.ManagedThreadId));

                rc = DGControl.Parent.OpenDsm(MessageLoop.Instance.LoopHandle);
                if (rc == ReturnCode.Success)
                {
                    // if twain2 then get memory management functions
                    if ((_appId.DataFunctionalities & DataFunctionalities.Dsm2) == DataFunctionalities.Dsm2)
                    {
                        TWEntryPoint entry;
                        rc = DGControl.EntryPoint.Get(out entry);
                        if (rc == ReturnCode.Success)
                        {
                            Platform.MemoryManager = entry;
                            Debug.WriteLine("Using TWAIN2 memory functions.");
                        }
                        else
                        {
                            CloseManager();
                        }
                    }
                }
            });
            return rc;
        }

        /// <summary>
        /// Closes the data source manager.
        /// </summary>
        /// <returns></returns>
        public ReturnCode CloseManager()
        {
            var rc = ReturnCode.Failure;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: CloseManager.", Thread.CurrentThread.ManagedThreadId));

                rc = DGControl.Parent.CloseDsm(MessageLoop.Instance.LoopHandle);
                if (rc == ReturnCode.Success)
                {
                    Platform.MemoryManager = null;
                }
            });
            return rc;
        }

        /// <summary>
        /// Loads the specified source into main memory and causes its initialization.
        /// Calls to this must be followed by
        /// <see cref="CloseSource" /> when not using it anymore.
        /// </summary>
        /// <param name="sourceProductName">Name of the source.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">sourceProductName</exception>
        public ReturnCode OpenSource(string sourceProductName)
        {
            if (string.IsNullOrEmpty(sourceProductName)) { throw new ArgumentException(Resources.SourceRequired, "sourceProductName"); }

            var rc = ReturnCode.Failure;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: OpenSource.", Thread.CurrentThread.ManagedThreadId));

                var source = new TWIdentity
                {
                    ProductName = sourceProductName
                };

                rc = DGControl.Identity.OpenDS(source);
            });
            return rc;
        }

        /// <summary>
        /// When an application is finished with a Source, it must formally close the session between them
        /// using this operation. This is necessary in case the Source only supports connection with a single
        /// application (many desktop scanners will behave this way). A Source such as this cannot be
        /// accessed by other applications until its current session is terminated
        /// </summary>
        /// <returns></returns>
        public ReturnCode CloseSource()
        {
            var rc = ReturnCode.Failure;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: CloseSource.", Thread.CurrentThread.ManagedThreadId));

                rc = DGControl.Identity.CloseDS();
                if (rc == ReturnCode.Success)
                {
                    SupportedCaps = null;
                }
            });
            return rc;
        }

        /// <summary>
        /// Enables the source to start transferring.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI will display as modal.</param>
        /// <param name="windowHandle">The window handle if modal.</param>
        /// <returns></returns>
        public ReturnCode EnableSource(SourceEnableMode mode, bool modal, IntPtr windowHandle)
        {
            var rc = ReturnCode.Failure;

            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: EnableSource.", Thread.CurrentThread.ManagedThreadId));

                // app v2.2 or higher uses callback2
                if (_appId.ProtocolMajor >= 2 && _appId.ProtocolMinor >= 2)
                {
                    var cb = new TWCallback2(HandleCallback);
                    var rc2 = DGControl.Callback2.RegisterCallback(cb);

                    if (rc2 == ReturnCode.Success)
                    {
                        Debug.WriteLine("Registered callback2 OK.");
                        _callbackObj = cb;
                    }
                }
                else
                {
                    var cb = new TWCallback(HandleCallback);

                    var rc2 = DGControl.Callback.RegisterCallback(cb);

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
                    rc = DGControl.UserInterface.EnableDSUIOnly(_twui);
                }
                else
                {
                    rc = DGControl.UserInterface.EnableDS(_twui);
                }

                if (rc != ReturnCode.Success)
                {
                    _callbackObj = null;
                }
            });
            return rc;
        }

        ReturnCode ITwainSessionInternal.DisableSource()
        {
            var rc = ReturnCode.Failure;

            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: DisableSource.", Thread.CurrentThread.ManagedThreadId));

                rc = DGControl.UserInterface.DisableDS(_twui);
                if (rc == ReturnCode.Success)
                {
                    _callbackObj = null;
                    SafeAsyncSyncableRaiseOnEvent(OnSourceDisabled, SourceDisabled);
                }
            });
            return rc;
        }

        /// <summary>
        /// Forces the stepping down of an opened source when things gets out of control.
        /// Used when session state and source state become out of sync.
        /// </summary>
        /// <param name="targetState">State of the target.</param>
        public void ForceStepDown(int targetState)
        {
            Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: ForceStepDown.", Thread.CurrentThread.ManagedThreadId));

            bool origFlag = EnforceState;
            EnforceState = false;

            // From the twain spec
            // Stepping Back Down the States
            // DG_CONTROL / DAT_PENDINGXFERS / MSG_ENDXFER → state 7 to 6
            // DG_CONTROL / DAT_PENDINGXFERS / MSG_RESET → state 6 to 5
            // DG_CONTROL / DAT_USERINTERFACE / MSG_DISABLEDS → state 5 to 4
            // DG_CONTROL / DAT_IDENTITY / MSG_CLOSEDS → state 4 to 3
            // Ignore the status returns from the calls prior to the one yielding the desired state. For instance, if a
            // call during scanning returns TWCC_SEQERROR and the desire is to return to state 5, then use the
            // following commands.
            // DG_CONTROL / DAT_PENDINGXFERS / MSG_ENDXFER → state 7 to 6
            // DG_CONTROL / DAT_PENDINGXFERS / MSG_RESET → state 6 to 5
            // Being sure to confirm that DG_CONTROL / DAT_PENDINGXFERS / MSG_RESET returned
            // success, the return status from DG_CONTROL / DAT_PENDINGXFERS / MSG_ENDXFER may
            // be ignored.

            MessageLoop.Instance.Invoke(() =>
            {
                if (targetState < 7)
                {
                    DGControl.PendingXfers.EndXfer(new TWPendingXfers());
                }
                if (targetState < 6)
                {
                    DGControl.PendingXfers.Reset(new TWPendingXfers());
                }
                if (targetState < 5)
                {
                    ((ITwainSessionInternal)this).DisableSource();
                }
                if (targetState < 4)
                {
                    CloseSource();
                }
                if (targetState < 3)
                {
                    CloseManager();
                }
            });
            EnforceState = origFlag;
        }

        #endregion

        #region custom events and overridables

        /// <summary>
        /// Occurs when <see cref="State"/> has changed.
        /// </summary>
        public event EventHandler StateChanged;
        /// <summary>
        /// Occurs when <see cref="SourceId"/> has changed.
        /// </summary>
        public event EventHandler SourceChanged;
        /// <summary>
        /// Occurs when source has been disabled (back to state 4).
        /// </summary>
        public event EventHandler SourceDisabled;
        /// <summary>
        /// Occurs when the source has generated an event.
        /// </summary>
        public event EventHandler<DeviceEventArgs> DeviceEvent;
        /// <summary>
        /// Occurs when a data transfer is ready.
        /// </summary>
        public event EventHandler<TransferReadyEventArgs> TransferReady;
        /// <summary>
        /// Occurs when data has been transferred.
        /// </summary>
        public event EventHandler<DataTransferredEventArgs> DataTransferred;
        /// <summary>
        /// Occurs when an error has been encountered during transfer.
        /// </summary>
        public event EventHandler<TransferErrorEventArgs> TransferError;


        /// <summary>
        /// Raises event and if applicable marshal it asynchronously to the <see cref="SynchronizationContext"/> thread
        /// without exceptions.
        /// </summary>
        /// <param name="onEventFunc">The on event function.</param>
        /// <param name="handler">The handler.</param>
        void SafeAsyncSyncableRaiseOnEvent(Action onEventFunc, EventHandler handler)
        {
            var syncer = SynchronizationContext;
            if (syncer == null)
            {
                try
                {
                    onEventFunc();
                    if (handler != null) { handler(this, EventArgs.Empty); }
                }
                catch { }
            }
            else
            {
                syncer.Post(o =>
                {
                    try
                    {
                        onEventFunc();
                        if (handler != null) { handler(this, EventArgs.Empty); }
                    }
                    catch { }
                }, null);
            }
        }

        /// <summary>
        /// Raises event and if applicable marshal it synchronously to the <see cref="SynchronizationContext" /> thread
        /// without exceptions.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="onEventFunc">The on event function.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="e">The TEventArgs instance containing the event data.</param>
        void SafeSyncableRaiseOnEvent<TEventArgs>(Action<TEventArgs> onEventFunc, EventHandler<TEventArgs> handler, TEventArgs e) where TEventArgs : EventArgs
        {
            var syncer = SynchronizationContext;
            if (syncer == null)
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Trying to raise event {0} on thread {1} without sync.", e.GetType().Name, Thread.CurrentThread.ManagedThreadId));

                try
                {
                    onEventFunc(e);
                    if (handler != null) { handler(this, e); }
                }
                catch { }
            }
            else
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Trying to raise event {0} on thread {1} with sync.", e.GetType().Name, Thread.CurrentThread.ManagedThreadId));
                // on some consumer desktop scanner with poor drivers this can frequently hang. there's nothing I can do here.
                syncer.Send(o =>
                {
                    try
                    {
                        onEventFunc(e);
                        if (handler != null) { handler(this, e); }
                    }
                    catch { }
                }, null);
            }
        }

        /// <summary>
        /// Called when <see cref="State"/> changed.
        /// </summary>
        protected virtual void OnStateChanged() { }

        /// <summary>
        /// Called when <see cref="SourceId"/> changed.
        /// </summary>
        protected virtual void OnSourceChanged() { }

        /// <summary>
        /// Called when source has been disabled (back to state 4).
        /// </summary>
        protected virtual void OnSourceDisabled() { }

        /// <summary>
        /// Called when the source has generated an event.
        /// </summary>
        /// <param name="e">The <see cref="DeviceEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeviceEvent(DeviceEventArgs e) { }

        /// <summary>
        /// Called when a data transfer is ready.
        /// </summary>
        /// <param name="e">The <see cref="TransferReadyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTransferReady(TransferReadyEventArgs e) { }

        /// <summary>
        /// Called when data has been transferred.
        /// </summary>
        /// <param name="e">The <see cref="DataTransferredEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDataTransferred(DataTransferredEventArgs e) { }

        /// <summary>
        /// Called when an error has been encountered during transfer.
        /// </summary>
        /// <param name="e">The <see cref="TransferErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTransferError(TransferErrorEventArgs e) { }

        #endregion

        #region handle twain ds message

        void HandleWndProcMessage(ref WindowsHook.MESSAGE winMsg, ref bool handled)
        {
            // this handles the message from a typical WndProc message loop and check if it's from the TWAIN source.
            if (_state >= 5)
            {
                // transform it into a pointer for twain
                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    // no need to do another lock call when using marshal alloc
                    msgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winMsg));
                    Marshal.StructureToPtr(winMsg, msgPtr, false);

                    var evt = new TWEvent();
                    evt.pEvent = msgPtr;
                    if (handled = (DGControl.Event.ProcessEvent(evt) == ReturnCode.DSEvent))
                    {
                        Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: HandleWndProcMessage at state {1} with MSG={2}.", Thread.CurrentThread.ManagedThreadId, State, evt.TWMessage));

                        HandleSourceMsg(evt.TWMessage);
                    }
                }
                finally
                {
                    if (msgPtr != IntPtr.Zero) { Marshal.FreeHGlobal(msgPtr); }
                }
            }
        }

        ReturnCode HandleCallback(TWIdentity origin, TWIdentity destination, DataGroups dg, DataArgumentType dat, Message msg, IntPtr data)
        {
            if (origin != null && SourceId != null && origin.Id == SourceId.Id && _state >= 5)
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: CallbackHandler at state {1} with MSG={2}.", Thread.CurrentThread.ManagedThreadId, State, msg));
                // spec says we must handle this on the thread that enabled the DS.
                // by using the internal dispatcher this will be the case.

                MessageLoop.Instance.BeginInvoke(() =>
                {
                    HandleSourceMsg(msg);
                });
                return ReturnCode.Success;
            }
            return ReturnCode.Failure;
        }

        // final method that handles msg from the source, whether it's from wndproc or callbacks
        void HandleSourceMsg(Message msg)
        {
            switch (msg)
            {
                case Message.XferReady:
                    if (State < 6)
                    {
                        State = 6;
                    }
                    TransferLogic.DoTransferRoutine(this);
                    break;
                case Message.DeviceEvent:
                    TWDeviceEvent de;
                    var rc = DGControl.DeviceEvent.Get(out de);
                    if (rc == ReturnCode.Success)
                    {
                        SafeSyncableRaiseOnEvent(OnDeviceEvent, DeviceEvent, new DeviceEventArgs(de));
                    }
                    break;
                case Message.CloseDSReq:
                case Message.CloseDSOK:
                    // even though it says closeDS it's really disable.
                    // dsok is sent if source is enabled with uionly

                    // some sources send this at other states so do a step down
                    if (State > 5)
                    {
                        ForceStepDown(4);
                    }
                    else if (State == 5)
                    {
                        // needs this state check since some source sends this more than once
                        ((ITwainSessionInternal)this).DisableSource();
                    }
                    break;
            }
        }

        #endregion
    }
}
