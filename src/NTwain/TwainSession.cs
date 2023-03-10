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
    public partial class TwainSession
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
            _ownedSources = new Dictionary<string, DataSource>();
            if (PlatformInfo.Current.IsSupported)
            {
                ((ITwainSessionInternal)this).ChangeState(2, false);
            }
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
        readonly Dictionary<string, DataSource> _ownedSources;

        DataSource GetSourceInstance(ITwainSessionInternal session, TWIdentity sourceId)
        {
            DataSource source = null;
            PlatformInfo.Current.Log.Debug("Source id = {0}", sourceId.Id);
            var key = string.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}", sourceId.Id, sourceId.Manufacturer, sourceId.ProductFamily, sourceId.ProductName);
            if (_ownedSources.ContainsKey(key))
            {
                source = _ownedSources[key];
            }
            else
            {
                _ownedSources[key] = source = new DataSource(session, sourceId);
            }
            return source;
        }

        #region ITwainSession Members


        DGCustom _dgCustom;
        DGCustom ITripletControl.DGCustom { get { return DGCustom; } }
        public DGCustom DGCustom
        {
            get
            {
                if (_dgCustom == null) { _dgCustom = new DGCustom(this); }
                return _dgCustom;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether calls to triplets will verify the current twain session state.
        /// </summary>
        /// <value>
        ///   <c>true</c> if state value is enforced; otherwise, <c>false</c>.
        /// </value>
        public bool EnforceState { get; set; }

        /// <summary>
        /// [Experimental] Gets or sets the optional synchronization context when not specifying a <see cref="MessageLoopHook"/> on <see cref="Open()"/>.
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
        public DataSource CurrentSource { get; private set; }

        /// <summary>
        /// Gets or sets the default source for this application.
        /// While this can be get as long as the session is open,
        /// it can only be set at State 3.
        /// </summary>
        /// <value>
        /// The default source.
        /// </value>
        public DataSource DefaultSource
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
        public DataSource ShowSourceSelector()
        {
            TWIdentity id;
            if (((ITwainSessionInternal)this).DGControl.Identity.UserSelect(out id) == ReturnCode.Success)
            {
                return GetSourceInstance(this, id);
            }
            return null;
        }

        int _state = 1;
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
        /// Gets the named state value as defined by the TWAIN spec.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public State StateEx
        {
            get
            {
                return (State)_state;
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
        /// Whether to stop the transfer process when transfer error is encountered.
        /// May be required on some sources.
        /// </summary>
        /// <value>
        /// <c>true</c> to stop on transfer error; otherwise, <c>false</c>.
        /// </value>
        public bool StopOnTransferError { get; set; }

        /// <summary>
        /// Gets the reason a source was disabled (dropped from state 5) if it's due to user action.
        /// Mostly only <see cref="Message.CloseDSOK" /> or <see cref="Message.CloseDSReq" />.
        /// </summary>
        /// <value>
        /// The dialog result.
        /// </value>
        public Message DisableReason { get; private set; }

        /// <summary>
        /// Opens the data source manager. This must be the first method used
        /// before using other TWAIN functions. Calls to this must be followed by
        /// <see cref="Close" /> when done with a TWAIN session.
        /// </summary>
        /// <returns></returns>
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
                PlatformInfo.Current.Log.Debug("Thread {0}: OpenManager.", Thread.CurrentThread.ManagedThreadId);

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
                            PlatformInfo.InternalCurrent.MemoryManager = entry;
                            PlatformInfo.Current.Log.Debug("Using TWAIN2 memory functions.");
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
            _msgLoopHook?.Invoke(() =>
            {
                PlatformInfo.Current.Log.Debug("Thread {0}: CloseManager.", Thread.CurrentThread.ManagedThreadId);

                rc = ((ITwainSessionInternal)this).DGControl.Parent.CloseDsm(_msgLoopHook.Handle);
                if (rc == ReturnCode.Success)
                {
                    PlatformInfo.InternalCurrent.MemoryManager = null;
                    _msgLoopHook.Stop();
                    _msgLoopHook = null;
                }
            });
            return rc;
        }


        /// <summary>
        /// Gets list of sources available in the system.
        /// Only call this at state 2 or higher.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataSource> GetSources()
        {
            return this;
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

            var hit = this.Where(s => string.Equals(s.Name, sourceName)).FirstOrDefault();
            if (hit != null)
            {
                return hit.Open();
            }
            return ReturnCode.Failure;
        }

        /// <summary>
        /// Quick shortcut to open a source.
        /// </summary>
        /// <param name="sourceId">Id of the source.</param>
        /// <returns></returns>
        public ReturnCode OpenSource(int sourceId)
        {
            var curSrc = CurrentSource;
            if (curSrc != null)
            {
                // TODO: close any open sources first

            }

            var hit = this.Where(s => s.Id == sourceId).FirstOrDefault();
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
        /// Gets the manager status string. Only call this at state 3 or higher.
        /// </summary>
        /// <param name="status">Status from previous calls.</param>
        /// <returns></returns>
        public TWStatusUtf8 GetStatusUtf8(TWStatus status)
        {
            TWStatusUtf8 stat;
            ((ITwainSessionInternal)this).DGControl.StatusUtf8.GetManager(status, out stat);
            return stat;
        }

        /// <summary>
        /// Forces the stepping down of an opened source when things gets out of control.
        /// Used when session state and source state become out of sync.
        /// </summary>
        /// <param name="targetState">State of the target.</param>
        public void ForceStepDown(int targetState)
        {
            PlatformInfo.Current.Log.Debug("Thread {0}: ForceStepDown.", Thread.CurrentThread.ManagedThreadId);

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

            _msgLoopHook?.Invoke(() =>
            {
                if (targetState < 7 && CurrentSource != null)
                {
                    // not sure if really necessary but can't hurt to pin it
                    var pending = new TWPendingXfers();
                    var handle = GCHandle.Alloc(pending, GCHandleType.Pinned);
                    try
                    {
                        ((ITwainSessionInternal)this).DGControl.PendingXfers.EndXfer(pending);
                    }
                    finally
                    {
                        handle.Free();
                    }
                }
                if (targetState < 6 && CurrentSource != null)
                {
                    var pending = new TWPendingXfers();
                    var handle = GCHandle.Alloc(pending, GCHandleType.Pinned);
                    try
                    {
                        ((ITwainSessionInternal)this).DGControl.PendingXfers.Reset(pending);
                    }
                    finally
                    {
                        handle.Free();
                    }
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
        /// Occurs when a transfer was canceled.
        /// </summary>
        public event EventHandler<TransferCanceledEventArgs> TransferCanceled;
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
                catch (Exception ex)
                {
                    PlatformInfo.Current.Log.Error("PropertyChanged event error.", ex);
                }
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
                    catch (Exception ex)
                    {
                        PlatformInfo.Current.Log.Error("PropertyChanged event error.", ex);
                    }
                }, null);
            }
        }

        #endregion


        #region IEnumerable<DataSource> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<DataSource> GetEnumerator()
        {
            TWIdentity srcId;
            var rc = ((ITwainSessionInternal)this).DGControl.Identity.GetFirst(out srcId);
            while (rc == ReturnCode.Success)
            {
                yield return GetSourceInstance(this, srcId);
                rc = ((ITwainSessionInternal)this).DGControl.Identity.GetNext(out srcId);
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
                catch (Exception ex)
                {
                    PlatformInfo.Current.Log.Error("{0} event error.", ex, handler.Method.Name);
                }
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
                    catch (Exception ex)
                    {
                        PlatformInfo.Current.Log.Error("{0} event error.", ex, handler.Method.Name);
                    }
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
                PlatformInfo.Current.Log.Debug("Trying to raise event {0} on thread {1} without sync.", e.GetType().Name, Thread.CurrentThread.ManagedThreadId);

                try
                {
                    onEventFunc(e);
                    if (handler != null) { handler(this, e); }
                }
                catch (Exception ex)
                {
                    PlatformInfo.Current.Log.Error("{0} event error.", ex, handler.Method.Name);
                }
            }
            else
            {
                PlatformInfo.Current.Log.Debug("Trying to raise event {0} on thread {1} with sync.", e.GetType().Name, Thread.CurrentThread.ManagedThreadId);
                // on some consumer desktop scanner with poor drivers this can frequently hang. there's nothing I can do here.
                syncer.Send(o =>
                {
                    try
                    {
                        onEventFunc(e);
                        if (handler != null) { handler(this, e); }
                    }
                    catch (Exception ex)
                    {
                        PlatformInfo.Current.Log.Error("{0} event error.", ex, handler.Method.Name);
                    }
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
        /// Called when a transfer was canceled.
        /// </summary>
        /// <param name="e">The <see cref="TransferCanceledEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTransferCanceled(TransferCanceledEventArgs e) { }

        /// <summary>
        /// Called when an error has been encountered during transfer.
        /// </summary>
        /// <param name="e">The <see cref="TransferErrorEventArgs"/> instance containing the event data.</param>
        protected virtual void OnTransferError(TransferErrorEventArgs e) { }

        #endregion
    }
}
