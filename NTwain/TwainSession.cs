using NTwain.Data;
using NTwain.Triplets;
using NTwain.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// Basic class for interfacing with TWAIN.
    /// </summary>
    public class TwainSession : ITwainStateInternal, ITwainOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSessionOld" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSession(TWIdentity appId)
        {
            if (appId == null) { throw new ArgumentNullException("appId"); }
            _appId = appId;
            ((ITwainStateInternal)this).ChangeState(1, false);
            EnforceState = true;
        }

        TWIdentity _appId;
        IntPtr _appHandle;
        SynchronizationContext _syncer;
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


        #region ITwainStateInternal Members

        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <value>The app id.</value>
        TWIdentity ITwainStateInternal.GetAppId() { return _appId; }

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
                OnStateChanged();
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
            OnSourceChanged();
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
                    OnStateChanged();
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
            var hand = PropertyChanged;
            if (hand != null) { hand(this, new PropertyChangedEventArgs(propertyName)); }
        }

        #endregion

        #region privileged calls that causes state change in TWAIN

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by <see cref="CloseManager"/> when done.
        /// </summary>
        /// <param name="appHandle">On Windows = points to the window handle (hWnd) that will act as the Source’s
        /// "parent". On Macintosh = should be a NULL value.</param>
        /// <returns></returns>
        public ReturnCode OpenManager(IntPtr appHandle)
        {
            Debug.WriteLine(string.Format("Thread {0}: OpenManager.", Thread.CurrentThread.ManagedThreadId));

            var rc = DGControl.Parent.OpenDsm(appHandle);
            if (rc == ReturnCode.Success)
            {
                _appHandle = appHandle;
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
            return rc;
        }

        /// <summary>
        /// Closes the data source manager.
        /// </summary>
        /// <returns></returns>
        public ReturnCode CloseManager()
        {
            Debug.WriteLine(string.Format("Thread {0}: CloseManager.", Thread.CurrentThread.ManagedThreadId));

            var rc = DGControl.Parent.CloseDsm(_appHandle);
            if (rc == ReturnCode.Success)
            {
                _appHandle = IntPtr.Zero;
            }
            return rc;
        }

        /// <summary>
        /// Loads the specified source into main memory and causes its initialization.
        /// Calls to this must be followed by 
        /// <see cref="CloseSource" /> when done.
        /// </summary>
        /// <param name="sourceProductName">Name of the source.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Source name is required.;sourceProductName</exception>
        public ReturnCode OpenSource(string sourceProductName)
        {
            if (string.IsNullOrEmpty(sourceProductName)) { throw new ArgumentException("Source name is required.", "sourceProductName"); }

            Debug.WriteLine(string.Format("Thread {0}: OpenSource.", Thread.CurrentThread.ManagedThreadId));

            var source = new TWIdentity();
            source.ProductName = sourceProductName;

            var rc = DGControl.Identity.OpenDS(source);
            if (rc == ReturnCode.Success)
            {
            }
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
            Debug.WriteLine(string.Format("Thread {0}: CloseSource.", Thread.CurrentThread.ManagedThreadId));

            var rc = DGControl.Identity.CloseDS();
            if (rc == ReturnCode.Success)
            {
                _callbackObj = null;
                SupportedCaps = null;
            }
            return rc;
        }

        /// <summary>
        /// Enables the source.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI will display as modal.</param>
        /// <param name="windowHandle">The window handle if modal.</param>
        /// <param name="context">The
        /// Windows only. 
        /// <see cref="SynchronizationContext" /> that is required for certain operations.
        /// It is recommended you call this method in an UI thread and pass in
        /// <see cref="SynchronizationContext.Current" />
        /// if you do not have a custom one setup.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">context</exception>
        public ReturnCode EnableSource(SourceEnableMode mode, bool modal, IntPtr windowHandle, SynchronizationContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }

            Debug.WriteLine(string.Format("Thread {0}: EnableSource.", Thread.CurrentThread.ManagedThreadId));

            _syncer = context;

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
                return DGControl.UserInterface.EnableDSUIOnly(_twui);
            }
            else
            {
                return DGControl.UserInterface.EnableDS(_twui);
            }

        }

        /// <summary>
        /// Disables the source to end data acquisition.
        /// </summary>
        /// <returns></returns>
        protected ReturnCode DisableSource()
        {
            Debug.WriteLine(string.Format("Thread {0}: DisableSource.", Thread.CurrentThread.ManagedThreadId));

            var rc = DGControl.UserInterface.DisableDS(_twui);
            if (rc == ReturnCode.Success)
            {
                OnSourceDisabled();
            }
            return rc;
        }

        /// <summary>
        /// Forces the stepping down of an opened source when things gets out of control.
        /// Used when session state and source state become out of sync.
        /// This should be called on the Thread that originally called the <see cref="EnableSource"/>
        /// method, if applicable.
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
        /// Called when <see cref="State"/> changed
        /// and raises the <see cref="StateChanged" /> event.
        /// </summary>
        protected virtual void OnStateChanged()
        {
            var hand = StateChanged;
            if (hand != null)
            {
                try
                {
                    hand(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        /// <summary>
        /// Called when <see cref="SourceId"/> changed
        /// and raises the <see cref="SourceChanged" /> event.
        /// </summary>
        protected virtual void OnSourceChanged()
        {
            var hand = SourceChanged;
            if (hand != null)
            {
                try
                {
                    hand(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        /// <summary>
        /// Called when source has been disabled (back to state 4)
        /// and raises the <see cref="SourceDisabled" /> event.
        /// </summary>
        protected virtual void OnSourceDisabled()
        {
            var hand = SourceDisabled;
            if (hand != null)
            {
                try
                {
                    hand(this, EventArgs.Empty);
                }
                catch { }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DeviceEvent" /> event.
        /// </summary>
        /// <param name="e">The <see cref="DeviceEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDeviceEvent(DeviceEventArgs e)
        {
            var hand = DeviceEvent;
            if (hand != null)
            {
                try
                {
                    hand(this, e);
                }
                catch { }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:TransferReady" /> event.
        /// </summary>
        /// <param name="e">The <see cref="TransferReadyEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTransferReady(TransferReadyEventArgs e)
        {
            var hand = TransferReady;
            if (hand != null)
            {
                try
                {
                    hand(this, e);
                }
                catch { }
            }
        }

        /// <summary>
        /// Raises the <see cref="E:DataTransferred" /> event.
        /// </summary>
        /// <param name="e">The <see cref="DataTransferredEventArgs"/> instance containing the event data.</param>
        protected virtual void OnDataTransferred(DataTransferredEventArgs e)
        {
            var hand = DataTransferred;
            if (hand != null)
            {
                try
                {
                    hand(this, e);
                }
                catch { }
            }
        }

        #endregion

        #region TWAIN logic during xfer work

        /// <summary>
        /// Handles the message from a typical WndProc message loop and check if it's from the TWAIN source.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>True if handled by TWAIN.</returns>
        protected bool HandleWndProcMessage(ref MESSAGE message)
        {
            var handled = false;
            if (State >= 4) // technically we should only handle on state >= 5 but there might be missed msgs if we wait until state changes after enabling ds
            {
                // transform it into a pointer for twain
                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    // no need to do another lock call when using marshal alloc
                    msgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(message));
                    Marshal.StructureToPtr(message, msgPtr, false);

                    TWEvent evt = new TWEvent();
                    evt.pEvent = msgPtr;
                    if (handled = DGControl.Event.ProcessEvent(evt) == ReturnCode.DSEvent)
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
            return handled;
        }

        ReturnCode HandleCallback(TWIdentity origin, TWIdentity destination, DataGroups dg, DataArgumentType dat, Message msg, IntPtr data)
        {
            if (origin != null && SourceId != null && origin.Id == SourceId.Id)
            {
                Debug.WriteLine(string.Format("Thread {0}: CallbackHandler at state {1} with MSG={2}.", Thread.CurrentThread.ManagedThreadId, State, msg));
                // spec says we must handle this on the thread that enabled the DS, 
                // but it's usually already the same thread and doesn't work (failure + seqError) w/o jumping to another thread and back.
                // My guess is the DS needs to see the Success return code first before letting transfer happen
                // so this is an hack to make it happen.

                // TODO: find a better method without needing a SynchronizationContext.
                ThreadPool.QueueUserWorkItem(o =>
                {
                    var ctx = o as SynchronizationContext;
                    if (ctx != null)
                    {
                        _syncer.Post(blah =>
                        {
                            HandleSourceMsg(msg);
                        }, null);
                    }
                    else
                    {
                        // no context? better hope for the best!
                        HandleSourceMsg(msg);
                    }
                }, _syncer);
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
                        OnDeviceEvent(new DeviceEventArgs(de));
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

                OnTransferReady(preXferArgs);

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
                    OnDataTransferred(new DataTransferredEventArgs { NativeData = lockedPtr });
                }
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
                OnDataTransferred(new DataTransferredEventArgs { FilePath = filePath });
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
                    if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                    {
                        imgInfo = null;
                    }
                    if (dataPtr != IntPtr.Zero)
                    {
                        lockedPtr = MemoryManager.Instance.Lock(dataPtr);
                    }
                    OnDataTransferred(new DataTransferredEventArgs { NativeData = lockedPtr, FinalImageInfo = imgInfo });
                }
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
                if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                {
                    imgInfo = null;
                }
                OnDataTransferred(new DataTransferredEventArgs { FilePath = filePath, FinalImageInfo = imgInfo });
            }
        }

        private void DoImageMemoryXfer()
        {
            throw new NotImplementedException();

            TWSetupMemXfer memInfo;
            if (DGControl.SetupMemXfer.Get(out memInfo) == ReturnCode.Success)
            {
                TWImageMemXfer xferInfo = new TWImageMemXfer();
                try
                {
                    xferInfo.Memory = new TWMemory
                    {
                        Flags = MemoryFlags.AppOwns | MemoryFlags.Pointer,
                        Length = memInfo.Preferred,
                        TheMem = MemoryManager.Instance.Allocate(memInfo.Preferred)
                    };

                    var xrc = ReturnCode.Success;
                    do
                    {
                        xrc = DGImage.ImageMemFileXfer.Get(xferInfo);

                        if (xrc == ReturnCode.Success ||
                            xrc == ReturnCode.XferDone)
                        {
                            State = 7;
                            byte[] buffer = new byte[(int)xferInfo.BytesWritten];
                            // todo: need lock before use?
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
                            // now what?

                        }
                    } while (xrc == ReturnCode.Success);

                    if (xrc == ReturnCode.XferDone)
                    {
                        TWImageInfo imgInfo;
                        //TWExtImageInfo extInfo;
                        //if (SupportedCaps.Contains(CapabilityId.ICapExtImageInfo))
                        //{
                        //    if (DGImage.ExtImageInfo.Get(out extInfo) != ReturnCode.Success)
                        //    {
                        //        extInfo = null;
                        //    }
                        //}
                        if (DGImage.ImageInfo.Get(out imgInfo) == ReturnCode.Success)
                        {
                            //OnDataTransferred(new DataTransferredEventArgs(IntPtr.Zero, null));
                        }
                        else
                        {
                            Trace.TraceError("Failed to get image info after ImageMemXfer.");
                            imgInfo = null;
                        }
                    }
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
                                // todo: need lock before use?
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
                    if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                    {
                        imgInfo = null;
                    }
                    OnDataTransferred(new DataTransferredEventArgs { FilePath = finalFile, FinalImageInfo = imgInfo });
                }
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// The MSG structure in Windows for TWAIN use.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        protected struct MESSAGE
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="MESSAGE"/> struct.
            /// </summary>
            /// <param name="hwnd">The HWND.</param>
            /// <param name="message">The message.</param>
            /// <param name="wParam">The w parameter.</param>
            /// <param name="lParam">The l parameter.</param>
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
