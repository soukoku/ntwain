using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTwain.Triplets;
using NTwain.Data;
using NTwain.Values;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Diagnostics;
using System.Security.Permissions;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// Provides a session for working with TWAIN api in an application.
    /// </summary>
    public class TwainSession : ITwainStateInternal, IMessageFilter, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSession" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSession(TWIdentity appId)
        {
            if (appId == null) { throw new ArgumentNullException("appId"); }
            AppId = appId;
            State = 1;
            EnforceState = true;
        }

        #region properties

        object _callbackObj;
        SynchronizationContext _syncer;


        /// <summary>
        /// Gets the app id used for the session.
        /// </summary>
        /// <value>The app id.</value>
        public TWIdentity AppId { get; private set; }

        /// <summary>
        /// Gets the source id used for the session.
        /// </summary>
        /// <value>The source id.</value>
        public TWIdentity SourceId { get; private set; }

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>The state.</value>
        public int State { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether callback is used parts of source communication
        /// if supported. May be required if things don't work. This does not take effect if
        /// the source is already open.
        /// </summary>
        /// <value>
        ///   <c>true</c> to disable callback; otherwise, <c>false</c>.
        /// </value>
        public bool DisableCallback { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        public bool EnforceState { get; set; }

        DGAudio _dgAudio;
        /// <summary>
        /// Gets the triplet operations defined for audio data group.
        /// </summary>
        /// <value>The DG audio.</value>
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
        /// <value>The DG control.</value>
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
        /// <value>The DG image.</value>
        public DGImage DGImage
        {
            get
            {
                if (_dgImage == null) { _dgImage = new DGImage(this); }
                return _dgImage;
            }
        }

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
                return _supportedCaps ?? new CapabilityId[0];
            }
            private set
            {
                _supportedCaps = value;
                RaisePropertyChanged("SupportedCaps");
            }
        }


        #endregion

        #region state transition calls

        void ITwainStateInternal.ChangeState(int newState, bool notifyChange)
        {
            Debug.WriteLine("TWAIN State = " + newState);
            State = newState;
            if (notifyChange) { RaisePropertyChanged("State"); }
        }
        ICommitable ITwainStateInternal.GetPendingStateChanger(int newState)
        {
            return new TentativeStateCommitable(this, newState);
        }
        void ITwainStateInternal.ChangeSourceId(TWIdentity sourceId)
        {
            SourceId = sourceId;
            RaisePropertyChanged("SourceId");
        }


        HandleRef _parentHandle;

        /// <summary>
        /// Opens the data source manager.
        /// </summary>
        /// <param name="handle">The handle. On Windows = points to the window handle (hWnd) that will act as the Source’s
        /// "parent". On Macintosh = should be a NULL value.</param>
        /// <returns></returns>
        public ReturnCode OpenManager(HandleRef handle)
        {
            Debug.WriteLine(string.Format("Thread {0}: OpenManager.", Thread.CurrentThread.ManagedThreadId));

            _parentHandle = handle;
            var hand = handle.Handle;
            var rc = DGControl.Parent.OpenDsm(ref hand);
            if (rc == ReturnCode.Success)
            {
                // if twain2 then get mem management stuff
                if ((AppId.DataFunctionalities & DataFunctionalities.Dsm2) == DataFunctionalities.Dsm2)
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

            var hand = _parentHandle.Handle;
            var rc = DGControl.Parent.CloseDsm(ref hand);
            if (rc == ReturnCode.Success)
            {
                _parentHandle = default(HandleRef);
                MemoryManager.Instance.UpdateEntryPoint(null);
            }
            return rc;
        }

        /// <summary>
        /// Loads the specified source into main memory and causes its initialization.
        /// </summary>
        /// <param name="sourceProductName">Name of the source.</param>
        /// <returns></returns>
        public ReturnCode OpenSource(string sourceProductName)
        {
            var source = new TWIdentity();
            source.ProductName = sourceProductName;
            return OpenSource(source);
        }

        /// <summary>
        /// Loads the specified Source into main memory and causes its initialization.
        /// </summary>
        /// <param name="sourceId">The source id.</param>
        /// <returns></returns>
        public ReturnCode OpenSource(TWIdentity sourceId)
        {
            if (sourceId == null) { throw new ArgumentNullException("sourceId"); }

            Debug.WriteLine(string.Format("Thread {0}: OpenSource.", Thread.CurrentThread.ManagedThreadId));

            var rc = DGControl.Identity.OpenDS(sourceId);
            if (rc == ReturnCode.Success)
            {
                SourceId = sourceId;
                SupportedCaps = this.GetCapabilities();

                // TODO: does it work?
                _syncer = SynchronizationContext.Current ?? new SynchronizationContext();

                if (!DisableCallback)
                {
                    // app v2.2 or higher uses callback2
                    if (AppId.ProtocolMajor >= 2 && AppId.ProtocolMinor >= 2)
                    {
                        var cb = new TWCallback2(new CallbackDelegate(CallbackHandler));
                        var rc2 = DGControl.Callback2.RegisterCallback(cb);

                        if (rc2 == ReturnCode.Success)
                        {
                            Debug.WriteLine("Registered callback2.");
                            _callbackObj = cb;
                        }
                    }
                    else
                    {
                        var cb = new TWCallback(new CallbackDelegate(CallbackHandler));

                        var rc2 = DGControl.Callback.RegisterCallback(cb);

                        if (rc2 == ReturnCode.Success)
                        {
                            Debug.WriteLine("Registered callback.");
                            _callbackObj = cb;
                        }
                    }
                }
            }
            return rc;
        }

        ReturnCode CallbackHandler(TWIdentity origin, TWIdentity dest,
            DataGroups dg, DataArgumentType dat, Values.Message msg, IntPtr data)
        {
            if (origin != null && SourceId != null && origin.Id == SourceId.Id)
            {
                Debug.WriteLine(string.Format("Thread {0}: GOT TWAIN callback for msg {1}.", Thread.CurrentThread.ManagedThreadId, msg));
                // spec says should handle this on the thread that enabled the DS, 
                // but it's already the same and doesn't work (failure + seqError) w/o jumping to another thread and back.
                // My guess is the DS needs to see the Success first before letting transfer happen
                // so this is an artificial delay to make it happen.
                // TODO: find a better method.
                ThreadPool.QueueUserWorkItem(o =>
                {
                    _syncer.Send(blah =>
                    {
                        HandleSourceMsg(origin, dest, dg, dat, msg, data);
                    }, null);
                }, null);
                return ReturnCode.Success;
            }
            return ReturnCode.Failure;
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
                SourceId = null;
                _callbackObj = null;
                SupportedCaps = null;
            }
            return rc;
        }

        TWUserInterface _twui;
        /// <summary>
        /// Enables the source for data acquisition.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI will display as modal.</param>
        /// <param name="windowHandle">The window handle if modal.</param>
        /// <returns></returns>
        public ReturnCode EnableSource(SourceEnableMode mode, bool modal, HandleRef windowHandle)
        {
            Debug.WriteLine(string.Format("Thread {0}: EnableSource.", Thread.CurrentThread.ManagedThreadId));

            _twui = new TWUserInterface();
            _twui.ShowUI = mode == SourceEnableMode.ShowUI;
            _twui.ModalUI = modal;
            _twui.hParent = windowHandle.Handle;

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
        ReturnCode DisableSource()
        {
            Debug.WriteLine(string.Format("Thread {0}: DisableSource.", Thread.CurrentThread.ManagedThreadId));

            var rc = DGControl.UserInterface.DisableDS(_twui);
            if (rc == ReturnCode.Success)
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
            return rc;
        }

        #endregion

        #region consumer to handle

        /// <summary>
        /// Occurs when source has been disabled (back to state 4).
        /// </summary>
        public event EventHandler SourceDisabled;
        /// <summary>
        /// Occurs when a data transfer is ready.
        /// </summary>
        public event EventHandler<TransferReadyEventArgs> TransferReady;
        /// <summary>
        /// Occurs when the source has generated an event.
        /// </summary>
        public event EventHandler<DeviceEventArgs> DeviceEvent;
        /// <summary>
        /// Occurs when data has been transferred.
        /// </summary>
        public event EventHandler<DataTransferredEventArgs> DataTransferred;


        private void DoTransferRoutine()
        {
            TWPendingXfers pending = new TWPendingXfers();
            var rc = ReturnCode.Success;

            do
            {
                IList<FileFormat> formats = Enumerable.Empty<FileFormat>().ToList();
                IList<Compression> compressions = Enumerable.Empty<Compression>().ToList();
                bool canDoFileXfer = this.CapGetImageXferMech().Contains(XferMech.File);
                var curFormat = this.GetCurrentCap<FileFormat>(CapabilityId.ICapImageFileFormat);
                var curComp = this.GetCurrentCap<Compression>(CapabilityId.ICapCompression);
                TWImageInfo imgInfo;
                bool skip = false;
                if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                {
                    // bad!
                    skip = true;
                }

                try
                {
                    formats = this.CapGetImageFileFormat();
                }
                catch { }
                try
                {
                    compressions = this.CapGetCompression();
                }
                catch { }

                // ask consumer for cancel in case of non-ui multi-page transfers
                TransferReadyEventArgs args = new TransferReadyEventArgs(pending, formats, curFormat, compressions,
                    curComp, canDoFileXfer, imgInfo);
                args.CancelCurrent = skip;

                var hand = TransferReady;
                if (hand != null)
                {
                    try
                    {
                        hand(this, args);
                    }
                    catch { }
                }

                if (!args.CancelAll && !args.CancelCurrent)
                {
                    Values.XferMech mech = this.GetCurrentCap<XferMech>(CapabilityId.ICapXferMech);

                    if (args.CanDoFileXfer && !string.IsNullOrEmpty(args.OutputFile))
                    {
                        var setXferRC = DGControl.SetupFileXfer.Set(new TWSetupFileXfer
                        {
                            FileName = args.OutputFile,
                            Format = args.ImageFormat
                        });
                        if (setXferRC == ReturnCode.Success)
                        {
                            mech = XferMech.File;
                        }
                    }

                    // I don't know how this is supposed to work so it probably doesn't
                    //this.CapSetImageFormat(args.ImageFormat);
                    //this.CapSetImageCompression(args.ImageCompression);

                    #region do xfer

                    // TODO: expose all swallowed exceptions somehow later

                    IntPtr dataPtr = IntPtr.Zero;
                    IntPtr lockedPtr = IntPtr.Zero;
                    string file = null;
                    try
                    {
                        ReturnCode xrc = ReturnCode.Cancel;
                        switch (mech)
                        {
                            case Values.XferMech.Native:
                                xrc = DGImage.ImageNativeXfer.Get(ref dataPtr);
                                break;
                            case Values.XferMech.File:
                                xrc = DGImage.ImageFileXfer.Get();
                                if (File.Exists(args.OutputFile))
                                {
                                    file = args.OutputFile;
                                }
                                break;
                            case Values.XferMech.MemFile:
                                // not supported yet
                                //TWImageMemXfer memxfer = new TWImageMemXfer();
                                //xrc = DGImage.ImageMemXfer.Get(memxfer);
                                break;
                        }
                        if (xrc == ReturnCode.XferDone)
                        {
                            State = 7;
                            try
                            {
                                var dtHand = DataTransferred;
                                if (dtHand != null)
                                {
                                    if (dataPtr != IntPtr.Zero)
                                    {
                                        lockedPtr = MemoryManager.Instance.MemLock(dataPtr);
                                    }
                                    dtHand(this, new DataTransferredEventArgs(lockedPtr, file));
                                }
                            }
                            catch { }
                        }
                        //}
                        //else if (group == DataGroups.Audio)
                        //{
                        //	var xrc = DGAudio.AudioNativeXfer.Get(ref dataPtr);
                        //	if (xrc == ReturnCode.XferDone)
                        //	{
                        //		State = 7;
                        //		try
                        //		{
                        //			var dtHand = DataTransferred;
                        //			if (dtHand != null)
                        //			{
                        //				lockedPtr = MemoryManager.Instance.MemLock(dataPtr);
                        //				dtHand(this, new DataTransferredEventArgs(lockedPtr));
                        //			}
                        //		}
                        //		catch { }
                        //	}
                        //}
                    }
                    finally
                    {
                        State = 6;
                        // data here is allocated by source so needs to use shared mem calls
                        if (lockedPtr != IntPtr.Zero)
                        {
                            MemoryManager.Instance.MemUnlock(lockedPtr);
                            lockedPtr = IntPtr.Zero;
                        }
                        if (dataPtr != IntPtr.Zero)
                        {
                            MemoryManager.Instance.MemFree(dataPtr);
                            dataPtr = IntPtr.Zero;
                        }
                    }
                    #endregion
                }

                if (args.CancelAll)
                {
                    rc = DGControl.PendingXfers.Reset(pending);
                    if (rc == ReturnCode.Success)
                    {
                        // if audio exit here
                        //if (group == DataGroups.Audio)
                        //{
                        //	//???
                        //	return;
                        //}

                    }
                }
                else
                {
                    rc = DGControl.PendingXfers.EndXfer(pending);
                }
            } while (rc == ReturnCode.Success && pending.Count != 0);

            State = 5;
            DisableSource();

        }

        #endregion

        #region messaging use

        ReturnCode HandleSourceMsg(TWIdentity origin, TWIdentity destination,
            DataGroups dg, DataArgumentType dat, NTwain.Values.Message msg, IntPtr data)
        {
            Debug.WriteLine(string.Format("Thread {0}: HandleSourceMsg at state {1} with DG={2} DAT={3} MSG={4}.", Thread.CurrentThread.ManagedThreadId, State, dg, dat, msg));

            ReturnCode rc = ReturnCode.Success;

            switch (msg)
            {
                case Values.Message.XferReady:
                    if (State < 6)
                        State = 6;
                    // this is the meat of all twain stuff
                    DoTransferRoutine();
                    break;
                case Values.Message.DeviceEvent:
                    TWDeviceEvent de;
                    rc = DGControl.DeviceEvent.Get(out de);
                    if (rc == ReturnCode.Success)
                    {
                        var hand = this.DeviceEvent;
                        if (hand != null)
                        {
                            try
                            {
                                hand(this, new DeviceEventArgs(de));
                            }
                            catch { }
                        }
                    }
                    break;
                case Values.Message.CloseDSReq:
                case Values.Message.CloseDSOK:
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

            return rc;
        }

        /// <summary>
        /// Forces the stepping down of an opened source ignoring return values.
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

        /// <summary>
        /// Handles the message from a message loop.
        /// </summary>
        /// <param name="msgPtr">Pointer to message structure.</param>
        /// <returns>True if handled by TWAIN.</returns>
        bool HandleLoopMsgEvent(ref IntPtr msgPtr)
        {
            TWEvent evt = new TWEvent();
            evt.pEvent = msgPtr;
            var rc = DGControl.Event.ProcessEvent(evt);
            HandleSourceMsg(null, null, DataGroups.Control, DataArgumentType.Null, evt.TWMessage, IntPtr.Zero);
            return rc == ReturnCode.DSEvent;
        }

        /// <summary>
        /// Message loop processor for winform. 
        /// Use this by adding the <see cref="TwainSession"/> as an <see cref="IMessageFilter "/>.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>
        /// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.
        /// </returns>
        //[EnvironmentPermissionAttribute(SecurityAction.LinkDemand)]
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        bool IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (State > 3)
            {
                MSG winmsg = default(MSG);
                winmsg.hwnd = m.HWnd;
                winmsg.lParam = m.LParam;
                winmsg.message = m.Msg;
                winmsg.wParam = m.WParam;

                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    // no need to lock for marshal alloc
                    msgPtr = MemoryManager.Instance.MemAllocate((uint)Marshal.SizeOf(winmsg));
                    Marshal.StructureToPtr(winmsg, msgPtr, false);
                    return HandleLoopMsgEvent(ref msgPtr);
                }
                finally
                {
                    if (msgPtr != IntPtr.Zero)
                        MemoryManager.Instance.MemFree(msgPtr);
                }
            }
            return false;
        }

        /// <summary>
        /// Message loop processor for wpf.
        /// Use this as the target of <see cref="HwndSourceHook"/> delegate.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message ID.</param>
        /// <param name="wParam">The message's wParam value.</param>
        /// <param name="lParam">The message's lParam value.</param>
        /// <param name="handled">A value that indicates whether the message was handled. Set the value to true if the message was handled; otherwise, false.</param>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand)]
        public IntPtr PreFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // always pass message since it works whether there's a callback or not?
            if (State > 3)// && _callbackObj == null)
            {
                MSG winmsg = default(MSG);
                winmsg.hwnd = hwnd;
                winmsg.lParam = lParam;
                winmsg.message = msg;
                winmsg.wParam = wParam;

                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    // no need to lock for marshal alloc
                    msgPtr = MemoryManager.Instance.MemAllocate((uint)Marshal.SizeOf(winmsg));
                    Marshal.StructureToPtr(winmsg, msgPtr, false);
                    handled = HandleLoopMsgEvent(ref msgPtr);
                }
                finally
                {
                    if (msgPtr != IntPtr.Zero)
                        MemoryManager.Instance.MemFree(msgPtr);
                }
            }
            return IntPtr.Zero;
        }
        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        void RaisePropertyChanged(string property)
        {
            var hand = PropertyChanged;
            if (hand != null) { hand(this, new PropertyChangedEventArgs(property)); }
        }

        #endregion

    }
}
