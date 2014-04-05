using NTwain.Data;
using NTwain.Triplets;
using NTwain.Values;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// Base working class for interfacing with TWAIN.
    /// </summary>
    public class TwainSessionBase : ITwainStateInternal, ITwainOperationInternal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSession" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSessionBase(TWIdentity appId)
        {
            if (appId == null) { throw new ArgumentNullException("appId"); }
            _appId = appId;
            State = 1;
            EnforceState = true;
        }

        TWIdentity _appId;
        HandleRef _appHandle;
        SynchronizationContext _syncer;
        object _callbackObj; // kept around so it doesn't get gc'ed
        TWUserInterface _twui;

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
            State = newState;
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
            OnSourceIdChanged();
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

        /// <summary>
        /// Gets the current state number as defined by the TWAIN spec.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public int State { get; private set; }

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
        public ReturnCode OpenManager(HandleRef appHandle)
        {
            Debug.WriteLine(string.Format("Thread {0}: OpenManager.", Thread.CurrentThread.ManagedThreadId));

            var rc = DGControl.Parent.OpenDsm(appHandle.Handle);
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

            var rc = DGControl.Parent.CloseDsm(_appHandle.Handle);
            if (rc == ReturnCode.Success)
            {
                _appHandle = default(HandleRef);
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
        /// <see cref="SynchronizationContext" /> that is required for certain operations.
        /// It is recommended you call this method in a UI thread and pass in
        /// <see cref="SynchronizationContext.Current" />
        /// if you do not have a custom one setup.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">context</exception>
        public ReturnCode EnableSource(SourceEnableMode mode, bool modal, HandleRef windowHandle, SynchronizationContext context)
        {
            if (context == null) { throw new ArgumentNullException("context"); }
            Debug.WriteLine(string.Format("Thread {0}: EnableSource.", Thread.CurrentThread.ManagedThreadId));

            _syncer = context;

            // app v2.2 or higher uses callback2
            if (_appId.ProtocolMajor >= 2 && _appId.ProtocolMinor >= 2)
            {
                var cb = new TWCallback2(CallbackHandler);
                var rc2 = DGControl.Callback2.RegisterCallback(cb);

                if (rc2 == ReturnCode.Success)
                {
                    Debug.WriteLine("Registered callback2 OK.");
                    _callbackObj = cb;
                }
            }
            else
            {
                var cb = new TWCallback(CallbackHandler);

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
                OnSourceDisabled();
            }
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
        /// Called when <see cref="State"/> changed.
        /// </summary>
        protected virtual void OnStateChanged() { }

        /// <summary>
        /// Called when <see cref="SourceId"/> changed.
        /// </summary>
        protected virtual void OnSourceIdChanged() { }

        /// <summary>
        /// Called when source has been disabled (back to state 4).
        /// </summary>
        protected virtual void OnSourceDisabled() { }

        #endregion

        #region real TWAIN logic during xfer

        void HandleSourceMsg(TWIdentity origin, TWIdentity destination, DataGroups dg, DataArgumentType dat, Message msg, IntPtr data)
        {
            if (msg != Message.Null)
            {
                Debug.WriteLine(string.Format("Thread {0}: HandleSourceMsg at state {1} with DG={2} DAT={3} MSG={4}.", Thread.CurrentThread.ManagedThreadId, State, dg, dat, msg));
            }


        }

        ReturnCode CallbackHandler(TWIdentity origin, TWIdentity destination, DataGroups dg, DataArgumentType dat, Message msg, IntPtr data)
        {
            if (origin != null && SourceId != null && origin.Id == SourceId.Id)
            {
                Debug.WriteLine(string.Format("Thread {0}: CallbackHandler at state {1} with DG={2} DAT={3} MSG={4}.", Thread.CurrentThread.ManagedThreadId, State, dg, dat, msg));
                // spec says we must handle this on the thread that enabled the DS, 
                // but it's usually already the same thread and doesn't work (failure + seqError) w/o jumping to another thread and back.
                // My guess is the DS needs to see the Success return code first before letting transfer happen
                // so this is an hack to make it happen.
                // TODO: find a better method.
                ThreadPool.QueueUserWorkItem(o =>
                {
                    var ctx = o as SynchronizationContext;
                    if (ctx != null)
                    {
                        _syncer.Send(blah =>
                        {
                            HandleSourceMsg(origin, destination, dg, dat, msg, data);
                        }, null);
                    }
                    else
                    {
                        // no context? better hope for the best!
                        HandleSourceMsg(origin, destination, dg, dat, msg, data);
                    }
                }, _syncer);
                return ReturnCode.Success;
            }
            return ReturnCode.Failure;
        }


        #endregion
    }
}
