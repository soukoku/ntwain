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
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// Basic class for interfacing with TWAIN. You should only have one of this per application process.
    /// </summary>
    public partial class TwainSession : ITwainSessionInternal, IWinMessageFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSession"/> class.
        /// </summary>
        /// <param name="supportedGroups">The supported groups.</param>
        public TwainSession(DataGroups supportedGroups)
            : this(TWIdentity.CreateFromAssembly(supportedGroups, Assembly.GetEntryAssembly()))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSession" /> class.
        /// </summary>
        /// <param name="appId">The app id that represents calling application.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSession(TWIdentity appId)
        {
            if (appId == null) { throw new ArgumentNullException("appId"); }

            _appId = appId;
            _ownedSources = new Dictionary<string, TwainSource>();
            ((ITwainSessionInternal)this).ChangeState(1, false);
#if DEBUG
            // defaults to false on release since it's only useful during dev
            EnforceState = true;
#endif
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        object _callbackObj; // kept around so it doesn't get gc'ed
        TWIdentity _appId;
        TWUserInterface _twui;

        // cache generated twain sources so if you get same source from one session it'll return the same object
        readonly Dictionary<string, TwainSource> _ownedSources;

        TwainSource GetSourceInstance(ITwainSessionInternal session, TWIdentity sourceId)
        {
            TwainSource source = null;
            Debug.WriteLine("Source id = " + sourceId.Id);
            var key = string.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}", sourceId.Id, sourceId.Manufacturer, sourceId.ProductFamily, sourceId.ProductName);
            if (_ownedSources.ContainsKey(key))
            {
                source = _ownedSources[key];
            }
            else
            {
                _ownedSources[key] = source = new TwainSource(session, sourceId);
            }
            return source;
        }
        
        #region ITwainSession Members


        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        public bool EnforceState { get; set; }

        /// <summary>
        /// [Experimental] Gets or sets the optional synchronization context when not specifying a <see cref="MessageLoopHook"/> on <see cref="Open"/>.
        /// This allows events to be raised on the thread associated with the context. This is experimental is not recommended for use.
        /// </summary>
        /// <value>
        /// The synchronization context.
        /// </value>
        public SynchronizationContext SynchronizationContext { get; set; }


        /// <summary>
        /// Gets the currently open source.
        /// </summary>
        /// <value>
        /// The current source.
        /// </value>
        public TwainSource CurrentSource { get; private set; }

        /// <summary>
        /// Gets or sets the default source for this application.
        /// While this can be get as long as the session is open,
        /// it can only be set at State 3.
        /// </summary>
        /// <value>
        /// The default source.
        /// </value>
        public TwainSource DefaultSource
        {
            get
            {
                TWIdentity id;
                if (((ITwainSessionInternal)this).DGControl.Identity.GetDefault(out id) == ReturnCode.Success)
                {
                    return GetSourceInstance(this, id);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    ((ITwainSessionInternal)this).DGControl.Identity.Set(value.Identity);
                }
            }
        }

        /// <summary>
        /// Try to show the built-in source selector dialog and return the selected source.
        /// This is not recommended and is only included for completeness.
        /// </summary>
        /// <returns></returns>
        public TwainSource ShowSourceSelector()
        {
            TWIdentity id;
            if (((ITwainSessionInternal)this).DGControl.Identity.UserSelect(out id) == ReturnCode.Success)
            {
                return GetSourceInstance(this, id);
            }
            return null;
        }

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

        /// <summary>
        /// Quick flag to check if the DSM has been opened.
        /// </summary>
        public bool IsDsmOpen { get { return State > 2; } }

        /// <summary>
        /// Quick flag to check if a source has been opened.
        /// </summary>
        public bool IsSourceOpen { get { return State > 3; } }

        /// <summary>
        /// Quick flag to check if a source has been enabled.
        /// </summary>
        public bool IsSourceEnabled { get { return State > 4; } }

        /// <summary>
        /// Quick flag to check if a source is in the transferring state.
        /// </summary>
        public bool IsTransferring { get { return State > 5; } }

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by 
        /// <see cref="Close" /> when done with a TWAIN session.
        /// </summary>
        /// <returns></returns
        public ReturnCode Open()
        {
            return Open(new InternalMessageLoopHook());
        }

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by
        /// <see cref="Close" /> when done with a TWAIN session.
        /// </summary>
        /// <param name="messageLoopHook">The message loop hook.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">messageLoopHook</exception>
        public ReturnCode Open(MessageLoopHook messageLoopHook)
        {
            if (messageLoopHook == null) { throw new ArgumentNullException("messageLoopHook"); }

            _msgLoopHook = messageLoopHook;
            _msgLoopHook.Start(this);
            var rc = ReturnCode.Failure;
            _msgLoopHook.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: OpenManager.", Thread.CurrentThread.ManagedThreadId));

                rc = ((ITwainSessionInternal)this).DGControl.Parent.OpenDsm(_msgLoopHook.Handle);
                if (rc == ReturnCode.Success)
                {
                    // if twain2 then get memory management functions
                    if ((_appId.DataFunctionalities & DataFunctionalities.Dsm2) == DataFunctionalities.Dsm2)
                    {
                        TWEntryPoint entry;
                        rc = ((ITwainSessionInternal)this).DGControl.EntryPoint.Get(out entry);
                        if (rc == ReturnCode.Success)
                        {
                            Platform.MemoryManager = entry;
                            Debug.WriteLine("Using TWAIN2 memory functions.");
                        }
                        else
                        {
                            Close();
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
        public ReturnCode Close()
        {
            var rc = ReturnCode.Failure;
            _msgLoopHook.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: CloseManager.", Thread.CurrentThread.ManagedThreadId));

                rc = ((ITwainSessionInternal)this).DGControl.Parent.CloseDsm(_msgLoopHook.Handle);
                if (rc == ReturnCode.Success)
                {
                    Platform.MemoryManager = null;
                    _msgLoopHook.Stop();
                }
            });
            return rc;
        }


        /// <summary>
        /// Gets list of sources available in the system.
        /// Only call this at state 2 or higher.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TwainSource> GetSources()
        {
            TWIdentity srcId;
            var rc = ((ITwainSessionInternal)this).DGControl.Identity.GetFirst(out srcId);
            while (rc == ReturnCode.Success)
            {
                yield return GetSourceInstance(this, srcId);
                rc = ((ITwainSessionInternal)this).DGControl.Identity.GetNext(out srcId);
            }
        }

        /// <summary>
        /// Quick shortcut to open a source.
        /// </summary>
        /// <param name="sourceName">Name of the source.</param>
        /// <returns></returns>
        public ReturnCode OpenSource(string sourceName)
        {
            var curSrc = CurrentSource;
            if (curSrc != null)
            {
                // TODO: close any open sources first

            }

            var hit = GetSources().Where(s => string.Equals(s.Name, sourceName)).FirstOrDefault();
            if (hit != null)
            {
                return hit.Open();
            }
            return ReturnCode.Failure;
        }

        /// <summary>
        /// Gets the manager status. Only call this at state 2 or higher.
        /// </summary>
        /// <returns></returns>
        public TWStatus GetStatus()
        {
            TWStatus stat;
            ((ITwainSessionInternal)this).DGControl.Status.GetManager(out stat);
            return stat;
        }

        /// <summary>
        /// Gets the manager status. Only call this at state 3 or higher.
        /// </summary>
        /// <returns></returns>
        public TWStatusUtf8 GetStatusUtf8()
        {
            TWStatusUtf8 stat;
            ((ITwainSessionInternal)this).DGControl.StatusUtf8.GetManager(out stat);
            return stat;
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

            _msgLoopHook.Invoke(() =>
            {
                if (targetState < 7 && CurrentSource != null)
                {
                    ((ITwainSessionInternal)this).DGControl.PendingXfers.EndXfer(new TWPendingXfers());
                }
                if (targetState < 6 && CurrentSource != null)
                {
                    ((ITwainSessionInternal)this).DGControl.PendingXfers.Reset(new TWPendingXfers());
                }
                if (targetState < 5 && CurrentSource != null)
                {
                    ((ITwainSessionInternal)this).DisableSource();
                }
                if (targetState < 4 && CurrentSource != null)
                {
                    CurrentSource.Close();
                }
                if (targetState < 3)
                {
                    Close();
                }
            });
            EnforceState = origFlag;
        }


        /// <summary>
        /// Occurs when <see cref="State"/> has changed.
        /// </summary>
        public event EventHandler StateChanged;
        /// <summary>
        /// Occurs when <see cref="CurrentSource"/> has changed.
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

        #region events overridables

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
        /// Called when <see cref="CurrentSource"/> changed.
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


        #region IWinMessageFilter Members

        /// <summary>
        /// Checks and handle the message if it's a TWAIN message.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>
        /// true if handled internally.
        /// </returns>
        public bool IsTwainMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam)
        {
            bool handled = false;
            // this handles the message from a typical WndProc message loop and check if it's from the TWAIN source.
            if (_state >= 5)
            {
                // transform it into a pointer for twain
                IntPtr msgPtr = IntPtr.Zero;
                try
                {
                    var winMsg = new NTwain.Internals.MESSAGE(hwnd, msg, wParam, lParam);

                    // no need to do another lock call when using marshal alloc
                    msgPtr = Marshal.AllocHGlobal(Marshal.SizeOf(winMsg));
                    Marshal.StructureToPtr(winMsg, msgPtr, false);

                    var evt = new TWEvent();
                    evt.pEvent = msgPtr;
                    if (handled = (((ITwainSessionInternal)this).DGControl.Event.ProcessEvent(evt) == ReturnCode.DSEvent))
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
            return handled;
        }

        #endregion

        ReturnCode HandleCallback(TWIdentity origin, TWIdentity destination, DataGroups dg, DataArgumentType dat, Message msg, IntPtr data)
        {
            if (origin != null && CurrentSource != null && origin.Id == CurrentSource.Identity.Id && _state >= 5)
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: CallbackHandler at state {1} with MSG={2}.", Thread.CurrentThread.ManagedThreadId, State, msg));
                // spec says we must handle this on the thread that enabled the DS.
                // by using the internal dispatcher this will be the case.

                _msgLoopHook.BeginInvoke(() =>
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
                    var rc = ((ITwainSessionInternal)this).DGControl.DeviceEvent.Get(out de);
                    if (rc == ReturnCode.Success)
                    {
                        SafeSyncableRaiseOnEvent(OnDeviceEvent, DeviceEvent, new DeviceEventArgs(de));
                    }
                    break;
                case Message.CloseDSReq:
                case Message.CloseDSOK:
                    Debug.WriteLine("Got msg " + msg);
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
