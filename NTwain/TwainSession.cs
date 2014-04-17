using NTwain.Data;
using NTwain.Properties;
using NTwain.Triplets;
using NTwain.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace NTwain
{
    /// <summary>
    /// Basic class for interfacing with TWAIN.
    /// </summary>
    public class TwainSession : ITwainStateInternal, ITwainOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSession" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSession(TWIdentity appId)
        {
            if (appId == null) { throw new ArgumentNullException("appId"); }
            _appId = appId;
            ((ITwainStateInternal)this).ChangeState(1, false);
            EnforceState = true;

            MessageLoop.Instance.EnsureStarted();
        }

        TWIdentity _appId;
        object _callbackObj; // kept around so it doesn't get gc'ed
        TWUserInterface _twui;

        static readonly CapabilityId[] _emptyCapList = new CapabilityId[0];

        private IList<CapabilityId> _supportedCaps;
        /// <summary>
        /// Gets the supported caps for the current source.
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
                RaisePropertyChanged("SupportedCaps");
            }
        }

        /// <summary>
        /// EXPERIMENTAL. Gets or sets the optional synchronization context.
        /// This allows events to be raised on the thread
        /// associated with the context.
        /// </summary>
        /// <value>
        /// The synchronization context.
        /// </value>
        public SynchronizationContext SynchronizationContext { get; set; }


        #region ITwainStateInternal Members

        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <value>The app id.</value>
        TWIdentity ITwainStateInternal.AppId { get { return _appId; } }

        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        public bool EnforceState { get; set; }

        void ITwainStateInternal.ChangeState(int newState, bool notifyChange)
        {
            _state = newState;
            if (notifyChange)
            {
                RaisePropertyChanged("State");
                SafeAsyncSyncableRaiseOnEvent(OnStateChanged, StateChanged);
            }
        }

        ICommitable ITwainStateInternal.GetPendingStateChanger(int newState)
        {
            return new TentativeStateCommitable(this, newState);
        }

        void ITwainStateInternal.ChangeSourceId(TWIdentity sourceId)
        {
            SourceId = sourceId;
            RaisePropertyChanged("SourceId");
            SafeAsyncSyncableRaiseOnEvent(OnSourceChanged, SourceChanged);
        }

        #endregion

        #region ITwainState Members

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
            protected set
            {
                if (value > 0 && value < 8)
                {
                    _state = value;
                    RaisePropertyChanged("State");
                    SafeAsyncSyncableRaiseOnEvent(OnStateChanged, StateChanged);
                }
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
        protected void RaisePropertyChanged(string propertyName)
        {
            if (SynchronizationContext == null)
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
                SynchronizationContext.Post(o =>
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
            ReturnCode rc = ReturnCode.Success;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format("Thread {0}: OpenManager.", Thread.CurrentThread.ManagedThreadId));

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
                            MemoryManager.Instance.UpdateEntryPoint(entry);
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
            ReturnCode rc = ReturnCode.Success;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format("Thread {0}: CloseManager.", Thread.CurrentThread.ManagedThreadId));

                rc = DGControl.Parent.CloseDsm(MessageLoop.Instance.LoopHandle);
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
        /// <exception cref="ArgumentException">Source name is required.;sourceProductName</exception>
        public ReturnCode OpenSource(string sourceProductName)
        {
            if (string.IsNullOrEmpty(sourceProductName)) { throw new ArgumentException(Resources.SourceRequired, "sourceProductName"); }

            ReturnCode rc = ReturnCode.Success;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format("Thread {0}: OpenSource.", Thread.CurrentThread.ManagedThreadId));

                var source = new TWIdentity();
                source.ProductName = sourceProductName;

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
            ReturnCode rc = ReturnCode.Success;
            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format("Thread {0}: CloseSource.", Thread.CurrentThread.ManagedThreadId));

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
        /// <exception cref="ArgumentNullException">context</exception>
        public ReturnCode EnableSource(SourceEnableMode mode, bool modal, IntPtr windowHandle)
        {
            ReturnCode rc = ReturnCode.Success;

            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format("Thread {0}: EnableSource.", Thread.CurrentThread.ManagedThreadId));

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

                if (_callbackObj == null)
                {
                    // must use msg loop if callback is not available
                    MessageLoop.Instance.AddHook(HandleWndProcMessage);
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
            });
            return rc;
        }

        /// <summary>
        /// Disables the source to end data acquisition.
        /// </summary>
        /// <returns></returns>
        protected ReturnCode DisableSource()
        {
            ReturnCode rc = ReturnCode.Success;

            MessageLoop.Instance.Invoke(() =>
            {
                Debug.WriteLine(string.Format("Thread {0}: DisableSource.", Thread.CurrentThread.ManagedThreadId));

                rc = DGControl.UserInterface.DisableDS(_twui);
                if (rc == ReturnCode.Success)
                {
                    if (_callbackObj == null)
                    {
                        MessageLoop.Instance.RemoveHook(HandleWndProcMessage);
                    }
                    else
                    {
                        _callbackObj = null;
                    }
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
            Debug.WriteLine(string.Format("Thread {0}: ForceStepDown.", Thread.CurrentThread.ManagedThreadId));

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
                    DisableSource();
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
        /// Raises event and if applicable marshal it synchronously to the <see cref="SynchronizationContext" /> thread.
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
                try
                {
                    onEventFunc(e);
                    if (handler != null) { handler(this, e); }
                }
                catch { }
            }
            else
            {
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
        /// Raises event and if applicable marshal it asynchronously to the <see cref="SynchronizationContext"/> thread.
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

        #region TWAIN logic during xfer work

        //[EnvironmentPermissionAttribute(SecurityAction.LinkDemand)]
        IntPtr HandleWndProcMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // this handles the message from a typical WndProc message loop and check if it's from the TWAIN source.
            if (State >= 5)
            {
                // transform it into a pointer for twain
                var winmsg = new MESSAGE(hwnd, msg, wParam, lParam);
                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    // no need to do another lock call when using marshal alloc
                    msgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winmsg));
                    Marshal.StructureToPtr(winmsg, msgPtr, false);

                    var evt = new TWEvent();
                    evt.pEvent = msgPtr;
                    if (handled = (DGControl.Event.ProcessEvent(evt) == ReturnCode.DSEvent))
                    {
                        Debug.WriteLine(string.Format("Thread {0}: HandleWndProcMessage at state {1} with MSG={2}.", Thread.CurrentThread.ManagedThreadId, State, evt.TWMessage));

                        HandleSourceMsg(evt.TWMessage);
                    }
                }
                finally
                {
                    if (msgPtr != IntPtr.Zero) { Marshal.FreeHGlobal(msgPtr); }
                }
            }

            return IntPtr.Zero;
        }

        ReturnCode HandleCallback(TWIdentity origin, TWIdentity destination, DataGroups dg, DataArgumentType dat, Message msg, IntPtr data)
        {
            if (origin != null && SourceId != null && origin.Id == SourceId.Id)
            {
                Debug.WriteLine(string.Format("Thread {0}: CallbackHandler at state {1} with MSG={2}.", Thread.CurrentThread.ManagedThreadId, State, msg));
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

        // method that handles msg from the source, whether it's from wndproc or callbacks
        void HandleSourceMsg(Message msg)
        {
            switch (msg)
            {
                case Message.XferReady:
                    if (State < 6)
                    {
                        State = 6;
                    }
                    DoTransferRoutine();
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
                        DisableSource();
                    }
                    break;
            }
        }

        /// <summary>
        /// Performs the TWAIN transfer routine at state 6. 
        /// </summary>
        protected virtual void DoTransferRoutine()
        {
            var pending = new TWPendingXfers();
            var rc = ReturnCode.Success;

            do
            {
                #region build and raise xfer ready

                TWAudioInfo audInfo;
                if (DGAudio.AudioInfo.Get(out audInfo) != ReturnCode.Success)
                {
                    audInfo = null;
                }

                TWImageInfo imgInfo;
                if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                {
                    imgInfo = null;
                }

                // ask consumer for xfer details
                var preXferArgs = new TransferReadyEventArgs
                {
                    AudioInfo = audInfo,
                    PendingImageInfo = imgInfo,
                    PendingTransferCount = pending.Count,
                    EndOfJob = pending.EndOfJob == 0
                };

                SafeSyncableRaiseOnEvent(OnTransferReady, TransferReady, preXferArgs);

                #endregion

                #region actually handle xfer

                if (preXferArgs.CancelAll)
                {
                    rc = DGControl.PendingXfers.Reset(pending);
                }
                else if (!preXferArgs.CancelCurrent)
                {
                    DataGroups xferGroup = DataGroups.None;

                    if (DGControl.XferGroup.Get(ref xferGroup) != ReturnCode.Success)
                    {
                        xferGroup = DataGroups.None;
                    }

                    if ((xferGroup & DataGroups.Image) == DataGroups.Image)
                    {
                        var mech = this.GetCurrentCap(CapabilityId.ICapXferMech).ConvertToEnum<XferMech>();
                        switch (mech)
                        {
                            case XferMech.Native:
                                DoImageNativeXfer();
                                break;
                            case XferMech.Memory:
                                DoImageMemoryXfer();
                                break;
                            case XferMech.File:
                                DoImageFileXfer();
                                break;
                            case XferMech.MemFile:
                                DoImageMemoryFileXfer();
                                break;
                        }
                    }
                    if ((xferGroup & DataGroups.Audio) == DataGroups.Audio)
                    {
                        var mech = this.GetCurrentCap(CapabilityId.ACapXferMech).ConvertToEnum<XferMech>();
                        switch (mech)
                        {
                            case XferMech.Native:
                                DoAudioNativeXfer();
                                break;
                            case XferMech.File:
                                DoAudioFileXfer();
                                break;
                        }
                    }
                }
                rc = DGControl.PendingXfers.EndXfer(pending);

                #endregion

            } while (rc == ReturnCode.Success && pending.Count != 0);

            State = 5;
            DisableSource();

        }

        #region audio xfers

        private void DoAudioNativeXfer()
        {
            IntPtr dataPtr = IntPtr.Zero;
            IntPtr lockedPtr = IntPtr.Zero;
            try
            {
                var xrc = DGAudio.AudioNativeXfer.Get(ref dataPtr);
                if (xrc == ReturnCode.XferDone)
                {
                    State = 7;
                    if (dataPtr != IntPtr.Zero)
                    {
                        lockedPtr = MemoryManager.Instance.Lock(dataPtr);
                    }

                    SafeSyncableRaiseOnEvent(OnDataTransferred, DataTransferred, new DataTransferredEventArgs { NativeData = lockedPtr });
                }
                else
                {
                    SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { ReturnCode = xrc, SourceStatus = this.GetSourceStatus() });
                }
            }
            catch (Exception ex)
            {
                SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { Exception = ex });
            }
            finally
            {
                State = 6;
                // data here is allocated by source so needs to use shared mem calls
                if (lockedPtr != IntPtr.Zero)
                {
                    MemoryManager.Instance.Unlock(lockedPtr);
                    lockedPtr = IntPtr.Zero;
                }
                if (dataPtr != IntPtr.Zero)
                {
                    MemoryManager.Instance.Free(dataPtr);
                    dataPtr = IntPtr.Zero;
                }
            }
        }

        private void DoAudioFileXfer()
        {
            string filePath = null;
            TWSetupFileXfer setupInfo;
            if (DGControl.SetupFileXfer.Get(out setupInfo) == ReturnCode.Success)
            {
                filePath = setupInfo.FileName;
            }

            var xrc = DGAudio.AudioFileXfer.Get();
            if (xrc == ReturnCode.XferDone)
            {
                SafeSyncableRaiseOnEvent(OnDataTransferred, DataTransferred, new DataTransferredEventArgs { FileDataPath = filePath });
            }
            else
            {
                SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { ReturnCode = xrc, SourceStatus = this.GetSourceStatus() });
            }
        }

        #endregion

        #region image xfers

        private void DoImageNativeXfer()
        {
            IntPtr dataPtr = IntPtr.Zero;
            IntPtr lockedPtr = IntPtr.Zero;
            try
            {
                var xrc = DGImage.ImageNativeXfer.Get(ref dataPtr);
                if (xrc == ReturnCode.XferDone)
                {
                    State = 7;
                    TWImageInfo imgInfo;
                    TWExtImageInfo extInfo = null;
                    if (SupportedCaps.Contains(CapabilityId.ICapExtImageInfo))
                    {
                        if (DGImage.ExtImageInfo.Get(out extInfo) != ReturnCode.Success)
                        {
                            extInfo = null;
                        }
                    }
                    if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                    {
                        imgInfo = null;
                    }
                    if (dataPtr != IntPtr.Zero)
                    {
                        lockedPtr = MemoryManager.Instance.Lock(dataPtr);
                    }
                    SafeSyncableRaiseOnEvent(OnDataTransferred, DataTransferred, new DataTransferredEventArgs
                    {
                        NativeData = lockedPtr,
                        ImageInfo = imgInfo,
                        ExImageInfo = extInfo
                    });
                    if (extInfo != null) { extInfo.Dispose(); }
                }
                else
                {
                    SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { ReturnCode = xrc, SourceStatus = this.GetSourceStatus() });
                }
            }
            catch (Exception ex)
            {
                SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { Exception = ex });
            }
            finally
            {
                State = 6;
                // data here is allocated by source so needs to use shared mem calls
                if (lockedPtr != IntPtr.Zero)
                {
                    MemoryManager.Instance.Unlock(lockedPtr);
                    lockedPtr = IntPtr.Zero;
                }
                if (dataPtr != IntPtr.Zero)
                {
                    MemoryManager.Instance.Free(dataPtr);
                    dataPtr = IntPtr.Zero;
                }
            }
        }

        private void DoImageFileXfer()
        {
            string filePath = null;
            TWSetupFileXfer setupInfo;
            if (DGControl.SetupFileXfer.Get(out setupInfo) == ReturnCode.Success)
            {
                filePath = setupInfo.FileName;
            }

            var xrc = DGImage.ImageFileXfer.Get();
            if (xrc == ReturnCode.XferDone)
            {
                TWImageInfo imgInfo;
                TWExtImageInfo extInfo = null;
                if (SupportedCaps.Contains(CapabilityId.ICapExtImageInfo))
                {
                    if (DGImage.ExtImageInfo.Get(out extInfo) != ReturnCode.Success)
                    {
                        extInfo = null;
                    }
                }
                if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                {
                    imgInfo = null;
                }
                SafeSyncableRaiseOnEvent(OnDataTransferred, DataTransferred, new DataTransferredEventArgs
                {
                    FileDataPath = filePath,
                    ImageInfo = imgInfo,
                    ExImageInfo = extInfo
                });
                if (extInfo != null) { extInfo.Dispose(); }
            }
            else
            {
                SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { ReturnCode = xrc, SourceStatus = this.GetSourceStatus() });
            }
        }

        private void DoImageMemoryXfer()
        {
            TWSetupMemXfer memInfo;
            if (DGControl.SetupMemXfer.Get(out memInfo) == ReturnCode.Success)
            {
                TWImageMemXfer xferInfo = new TWImageMemXfer();
                try
                {
                    // how to tell if going to xfer in strip vs tile?
                    // if tile don't allocate memory in app?

                    xferInfo.Memory = new TWMemory
                    {
                        Flags = MemoryFlags.AppOwns | MemoryFlags.Pointer,
                        Length = memInfo.Preferred,
                        TheMem = MemoryManager.Instance.Allocate(memInfo.Preferred)
                    };

                    // do the unthinkable and keep all xferred batches in memory, 
                    // possibly defeating the purpose of mem xfer
                    // unless compression is used.
                    // todo: use array instead of memory stream?
                    using (MemoryStream xferredData = new MemoryStream())
                    {
                        var xrc = ReturnCode.Success;
                        do
                        {
                            xrc = DGImage.ImageMemFileXfer.Get(xferInfo);

                            if (xrc == ReturnCode.Success ||
                                xrc == ReturnCode.XferDone)
                            {
                                State = 7;
                                // optimize and allocate buffer only once instead of inside the loop?
                                byte[] buffer = new byte[(int)xferInfo.BytesWritten];

                                IntPtr lockPtr = IntPtr.Zero;
                                try
                                {
                                    lockPtr = MemoryManager.Instance.Lock(xferInfo.Memory.TheMem);
                                    Marshal.Copy(lockPtr, buffer, 0, buffer.Length);
                                    xferredData.Write(buffer, 0, buffer.Length);
                                }
                                finally
                                {
                                    if (lockPtr != IntPtr.Zero)
                                    {
                                        MemoryManager.Instance.Unlock(lockPtr);
                                    }
                                }
                            }
                        } while (xrc == ReturnCode.Success);

                        if (xrc == ReturnCode.XferDone)
                        {
                            TWImageInfo imgInfo;
                            TWExtImageInfo extInfo = null;
                            if (SupportedCaps.Contains(CapabilityId.ICapExtImageInfo))
                            {
                                if (DGImage.ExtImageInfo.Get(out extInfo) != ReturnCode.Success)
                                {
                                    extInfo = null;
                                }
                            }
                            if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                            {
                                imgInfo = null;
                            }

                            SafeSyncableRaiseOnEvent(OnDataTransferred, DataTransferred, new DataTransferredEventArgs
                            {
                                MemData = xferredData.ToArray(),
                                ImageInfo = imgInfo,
                                ExImageInfo = extInfo
                            });
                            if (extInfo != null) { extInfo.Dispose(); }
                        }
                        else
                        {
                            SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { ReturnCode = xrc, SourceStatus = this.GetSourceStatus() });
                        }
                    }
                }
                catch (Exception ex)
                {
                    SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { Exception = ex });
                }
                finally
                {
                    State = 6;
                    if (xferInfo.Memory.TheMem != IntPtr.Zero)
                    {
                        MemoryManager.Instance.Free(xferInfo.Memory.TheMem);
                    }
                }

            }
        }

        private void DoImageMemoryFileXfer()
        {
            // since it's memory-file xfer need info from both (maybe)
            TWSetupMemXfer memInfo;
            TWSetupFileXfer fileInfo;
            if (DGControl.SetupMemXfer.Get(out memInfo) == ReturnCode.Success &&
                DGControl.SetupFileXfer.Get(out fileInfo) == ReturnCode.Success)
            {
                TWImageMemXfer xferInfo = new TWImageMemXfer();
                var tempFile = Path.GetTempFileName();
                string finalFile = null;
                try
                {
                    // no strip or tile here, just chunks
                    xferInfo.Memory = new TWMemory
                    {
                        Flags = MemoryFlags.AppOwns | MemoryFlags.Pointer,
                        Length = memInfo.Preferred,
                        TheMem = MemoryManager.Instance.Allocate(memInfo.Preferred)
                    };

                    var xrc = ReturnCode.Success;
                    using (var outStream = File.OpenWrite(tempFile))
                    {
                        do
                        {
                            xrc = DGImage.ImageMemFileXfer.Get(xferInfo);

                            if (xrc == ReturnCode.Success ||
                                xrc == ReturnCode.XferDone)
                            {
                                State = 7;
                                byte[] buffer = new byte[(int)xferInfo.BytesWritten];

                                IntPtr lockPtr = IntPtr.Zero;
                                try
                                {
                                    lockPtr = MemoryManager.Instance.Lock(xferInfo.Memory.TheMem);
                                    Marshal.Copy(lockPtr, buffer, 0, buffer.Length);
                                }
                                finally
                                {
                                    if (lockPtr != IntPtr.Zero)
                                    {
                                        MemoryManager.Instance.Unlock(lockPtr);
                                    }
                                }
                                outStream.Write(buffer, 0, buffer.Length);
                            }
                        } while (xrc == ReturnCode.Success);
                    }

                    if (xrc == ReturnCode.XferDone)
                    {
                        switch (fileInfo.Format)
                        {
                            case FileFormat.Bmp:
                                finalFile = Path.ChangeExtension(tempFile, ".bmp");
                                break;
                            case FileFormat.Dejavu:
                                finalFile = Path.ChangeExtension(tempFile, ".dejavu");
                                break;
                            case FileFormat.Exif:
                                finalFile = Path.ChangeExtension(tempFile, ".exit");
                                break;
                            case FileFormat.Fpx:
                                finalFile = Path.ChangeExtension(tempFile, ".fpx");
                                break;
                            case FileFormat.Jfif:
                                finalFile = Path.ChangeExtension(tempFile, ".jpg");
                                break;
                            case FileFormat.Jp2:
                                finalFile = Path.ChangeExtension(tempFile, ".jp2");
                                break;
                            case FileFormat.Jpx:
                                finalFile = Path.ChangeExtension(tempFile, ".jpx");
                                break;
                            case FileFormat.Pdf:
                            case FileFormat.PdfA:
                            case FileFormat.PdfA2:
                                finalFile = Path.ChangeExtension(tempFile, ".pdf");
                                break;
                            case FileFormat.Pict:
                                finalFile = Path.ChangeExtension(tempFile, ".pict");
                                break;
                            case FileFormat.Png:
                                finalFile = Path.ChangeExtension(tempFile, ".png");
                                break;
                            case FileFormat.Spiff:
                                finalFile = Path.ChangeExtension(tempFile, ".spiff");
                                break;
                            case FileFormat.Tiff:
                            case FileFormat.TiffMulti:
                                finalFile = Path.ChangeExtension(tempFile, ".tif");
                                break;
                            case FileFormat.Xbm:
                                finalFile = Path.ChangeExtension(tempFile, ".xbm");
                                break;
                            default:
                                finalFile = Path.ChangeExtension(tempFile, ".unknown");
                                break;
                        }
                        File.Move(tempFile, finalFile);
                    }
                    else
                    {
                        SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { ReturnCode = xrc, SourceStatus = this.GetSourceStatus() });
                    }
                }
                catch (Exception ex)
                {
                    SafeSyncableRaiseOnEvent(OnTransferError, TransferError, new TransferErrorEventArgs { Exception = ex });
                }
                finally
                {
                    State = 6;
                    if (xferInfo.Memory.TheMem != IntPtr.Zero)
                    {
                        MemoryManager.Instance.Free(xferInfo.Memory.TheMem);
                    }
                    if (File.Exists(tempFile))
                    {
                        File.Delete(tempFile);
                    }
                }

                if (File.Exists(finalFile))
                {
                    TWImageInfo imgInfo;
                    TWExtImageInfo extInfo = null;
                    if (SupportedCaps.Contains(CapabilityId.ICapExtImageInfo))
                    {
                        if (DGImage.ExtImageInfo.Get(out extInfo) != ReturnCode.Success)
                        {
                            extInfo = null;
                        }
                    }
                    if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                    {
                        imgInfo = null;
                    }
                    SafeSyncableRaiseOnEvent(OnDataTransferred, DataTransferred, new DataTransferredEventArgs
                    {
                        FileDataPath = finalFile,
                        ImageInfo = imgInfo,
                        ExImageInfo = extInfo
                    });
                    if (extInfo != null) { extInfo.Dispose(); }
                }
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// The MSG structure in Windows for TWAIN use.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        struct MESSAGE
        {
            public MESSAGE(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam)
            {
                _hwnd = hwnd;
                _message = (uint)message;
                _wParam = wParam;
                _lParam = lParam;
                _time = 0;
                _x = 0;
                _y = 0;
            }

            IntPtr _hwnd;
            uint _message;
            IntPtr _wParam;
            IntPtr _lParam;
            uint _time;
            int _x;
            int _y;
        }
    }
}
