using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
    public class TwainSession : IDisposable
    {
        private TWAIN _twain;
        private bool _disposed;
        private readonly IThreadMarshaller _threadMarshaller;
        private IntPtr _hWnd;

        public TwainSession(Assembly application,
            IThreadMarshaller threadMarshaller, IntPtr hWnd,
            TWLG language = TWLG.ENGLISH_USA, TWCY country = TWCY.USA)
        {
            var info = FileVersionInfo.GetVersionInfo(application.Location);

            _twain = new TWAIN(
                info.CompanyName, info.ProductName, info.ProductName,
                (ushort)TWON_PROTOCOL.MAJOR, (ushort)TWON_PROTOCOL.MINOR,
                (uint)(DG.APP2 | DG.IMAGE),
                country, "", language, 2, 4, false, true,
                HandleDeviceEvent,
                HandleScanEvent,
                HandleUIThreadAction,
                hWnd);

            _threadMarshaller = threadMarshaller ?? new ThreadPoolMarshaller();
            _hWnd = hWnd;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_twain != null)
                    {
                        Close();
                        _twain.Dispose();
                        _twain = null;
                    }
                    Log.Close();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the low-level twain object.
        /// Only use if you know what you're doing.
        /// </summary>
        public TWAIN TWAIN { get { return _twain; } }

        #region event callbacks

        /// <summary>
        /// Raised when data source has encountered some hardwar event.
        /// </summary>
        public event EventHandler<TW_DEVICEEVENT> DeviceEvent;

        /// <summary>
        /// Raised when data source comes down to state 4 from higher.
        /// </summary>
        public event EventHandler SourceDisabled;

        /// <summary>
        /// Raised when there's some error during transfer.
        /// </summary>
        public event EventHandler<TransferErrorEventArgs> TransferError;

        /// <summary>
        /// Raised when there's a pending transfer. Can be used to cancel transfers.
        /// </summary>
        public event EventHandler<TransferReadyEventArgs> TransferReady;

        private void HandleUIThreadAction(Action action)
        {
            DebugThreadInfo("begin");

            _threadMarshaller.Invoke(action);
        }

        private STS HandleDeviceEvent()
        {
            STS sts;
            TW_DEVICEEVENT twdeviceevent;

            // Drain the event queue...
            while (true)
            {
                DebugThreadInfo("in loop");

                // Try to get an event...
                twdeviceevent = default;
                sts = _twain.DatDeviceevent(DG.CONTROL, MSG.GET, ref twdeviceevent);
                if (sts != STS.SUCCESS)
                {
                    break;
                }
                else
                {
                    try
                    {
                        DeviceEvent?.Invoke(this, twdeviceevent);
                    }
                    catch { }
                }
            }

            // Return a status, in case we ever need it for anything...
            return STS.SUCCESS;
        }

        private STS HandleScanEvent(bool closing)
        {
            DebugThreadInfo("begin");

            // the scan event needs to return asap since it can come from msg loop
            // so fire off the handling work to another thread
            _threadMarshaller.BeginInvoke(new Action<bool>(HandleScanEventReal), closing);
            return STS.SUCCESS;
        }

        void HandleScanEventReal(bool closing)
        {
            DebugThreadInfo("begin");

            if (_twain == null || State <= STATE.S4 || closing) return;

            if (_twain.IsMsgCloseDsReq() || _twain.IsMsgCloseDsOk())
            {
                StepDown(STATE.S4);
                return;
            }

            // all except mem xfer will run this once and raise event.
            // mem xfer will run this multiple times until complete image is assembled
            if (_twain.IsMsgXferReady())
            {
                TW_PENDINGXFERS pending = default;
                var sts = _twain.DatPendingxfers(DG.CONTROL, MSG.GET, ref pending);
                if (sts != STS.SUCCESS)
                {
                    try
                    {
                        TransferError?.Invoke(this, new TransferErrorEventArgs(sts));
                    }
                    catch { }
                    return; // do more?
                }

                var xferMech = Capabilities.ICAP_XFERMECH.GetCurrent();

                var readyArgs = new TransferReadyEventArgs(_twain, pending.Count, (TWEJ)pending.EOJ);
                try
                {
                    TransferReady?.Invoke(this, readyArgs);
                }
                catch { }

                if (readyArgs.CancelCapture == CancelType.Immediate)
                {
                    sts = _twain.DatPendingxfers(DG.CONTROL, MSG.RESET, ref pending);
                }
                else
                {
                    if (readyArgs.CancelCapture == CancelType.Graceful) StopCapture();

                    if (!readyArgs.SkipCurrent)
                    {
                        switch (xferMech)
                        {
                            case TWSX.NATIVE:
                                RunImageNativeXfer();
                                break;
                            case TWSX.MEMFILE:
                                RunImageMemFileXfer();
                                break;
                            case TWSX.FILE:
                                RunImageFileXfer();
                                break;
                            case TWSX.MEMORY:
                                RunImageMemoryXfer();
                                break;
                        }
                    }
                    sts = _twain.DatPendingxfers(DG.CONTROL, MSG.ENDXFER, ref pending);
                }

                // TODO: may be wrong for now
                if (pending.Count == 0 || sts == STS.CANCEL || sts == STS.XFERDONE)
                {
                    StepDown(STATE.S4);
                }
                else
                {
                    HandleScanEvent(State <= STATE.S3);
                }
            }

        }

        [Conditional("DEBUG")]
        private void DebugThreadInfo(string description, [CallerMemberName] string callerName = "")
        {
            var tid = Thread.CurrentThread.ManagedThreadId;
            Debug.WriteLine($"[Thread {tid}] {callerName}() {description}");
        }

        private void RunImageMemoryXfer()
        {
            throw new NotImplementedException();

            //// Handle DAT_NULL/MSG_XFERREADY...
            //if (_twain.IsMsgXferReady() && !_xferReadySent)
            //{
            //    _xferReadySent = true;

            //    // Get the amount of memory needed...
            //    TW_SETUPMEMXFER m_twsetupmemxfer = default;
            //    var sts = _twain.DatSetupmemxfer(DG.CONTROL, MSG.GET, ref m_twsetupmemxfer);
            //    if ((sts != STS.SUCCESS) || (m_twsetupmemxfer.Preferred == 0))
            //    {
            //        _xferReadySent = false;
            //        if (!_disableDsSent)
            //        {
            //            _disableDsSent = true;
            //            StepDown(STATE.S4);
            //        }
            //    }

            //    // Allocate the transfer memory (with a little extra to protect ourselves)...
            //    var m_intptrXfer = Marshal.AllocHGlobal((int)m_twsetupmemxfer.Preferred + 65536);
            //    if (m_intptrXfer == IntPtr.Zero)
            //    {
            //        _disableDsSent = true;
            //        StepDown(STATE.S4);
            //    }
            //}

            //// This is where the statemachine runs that transfers and optionally
            //// saves the images to disk (it also displays them).  It'll go back
            //// and forth between states 6 and 7 until an error occurs, or until
            //// we run out of images...
            //if (_xferReadySent && !_disableDsSent)
            //{
            //    CaptureImages();
            //}
        }

        private void RunImageFileXfer()
        {
            throw new NotImplementedException();
        }

        private void RunImageMemFileXfer()
        {
            throw new NotImplementedException();
        }

        private void RunImageNativeXfer()
        {

        }

        //protected virtual void OnScanEvent(bool closing) { }
        //public event EventHandler<bool> ScanEvent;

        #endregion

        #region TWAIN operations


        /// <summary>
        /// Gets the current TWAIN state.
        /// </summary>
        public STATE State
        {
            get { return _twain.GetState(); }
        }

        /// <summary>
        /// Opens the TWAIN data source manager.
        /// This needs to be done before anything else.
        /// </summary>
        /// <returns></returns>
        public STS Open()
        {
            var sts = _twain.DatParent(DG.CONTROL, MSG.OPENDSM, ref _hWnd);
            return sts;
        }

        /// <summary>
        /// Closes the TWAIN data source manager.
        /// This is called when <see cref="Dispose()"/> is invoked.
        /// </summary>
        public void Close()
        {
            StepDown(STATE.S2);
        }

        /// <summary>
        /// Gets list of TWAIN data sources.
        /// </summary>
        /// <returns></returns>
        public IList<TW_IDENTITY> GetDataSources()
        {
            var list = new List<TW_IDENTITY>();
            if (State > STATE.S2)
            {
                TW_IDENTITY twidentity = default;
                STS sts;

                for (sts = _twain.DatIdentity(DG.CONTROL, MSG.GETFIRST, ref twidentity);
                     sts != STS.ENDOFLIST;
                     sts = _twain.DatIdentity(DG.CONTROL, MSG.GETNEXT, ref twidentity))
                {
                    list.Add(twidentity);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets or sets the default data source.
        /// </summary>
        public TW_IDENTITY? DefaultDataSource
        {
            get
            {
                TW_IDENTITY twidentity = default;
                var sts = _twain.DatIdentity(DG.CONTROL, MSG.GETDEFAULT, ref twidentity);
                if (sts == STS.SUCCESS) return twidentity;
                return null;
            }
            set
            {
                // Make it the default, we don't care if this succeeds...
                if (value.HasValue)
                {
                    var twidentity = value.Value;
                    _twain.DatIdentity(DG.CONTROL, MSG.SET, ref twidentity);
                }
            }
        }

        /// <summary>
        /// Gets or sets the currently open data source.
        /// Setting it will try to open it.
        /// </summary>
        public TW_IDENTITY? CurrentDataSource
        {
            get
            {
                if (State > STATE.S3)
                {
                    return _twain.m_twidentityDs;
                }
                return null;
            }
            set
            {
                StepDown(STATE.S3);
                if (value.HasValue)
                {
                    var twidentity = value.Value;
                    _twain.DatIdentity(DG.CONTROL, MSG.OPENDS, ref twidentity);
                }
            }
        }

        /// <summary>
        /// Steps down the TWAIN state to the specified state.
        /// </summary>
        /// <param name="target"></param>
        public void StepDown(STATE target)
        {
            // Make sure we have something to work with...
            if (_twain == null) return;

            // Walk the states, we don't care about the status returns.  Basically,
            // these need to work, or we're guaranteed to hang...

            // 7 --> 6
            if ((State == STATE.S7) && (target < STATE.S7))
            {
                TW_PENDINGXFERS twpendingxfers = default;
                _twain.DatPendingxfers(DG.CONTROL, MSG.ENDXFER, ref twpendingxfers);
            }

            // 6 --> 5
            if ((State == STATE.S6) && (target < STATE.S6))
            {
                TW_PENDINGXFERS twpendingxfers = default;
                _twain.DatPendingxfers(DG.CONTROL, MSG.RESET, ref twpendingxfers);
            }

            // 5 --> 4
            if ((State == STATE.S5) && (target < STATE.S5))
            {
                TW_USERINTERFACE twuserinterface = default;
                _twain.DatUserinterface(DG.CONTROL, MSG.DISABLEDS, ref twuserinterface);
                SourceDisabled?.Invoke(this, EventArgs.Empty);
            }

            // 4 --> 3
            if ((State == STATE.S4) && (target < STATE.S4))
            {
                _caps = null;
                _twain.DatIdentity(DG.CONTROL, MSG.CLOSEDS, ref _twain.m_twidentityDs);
            }

            // 3 --> 2
            if ((State == STATE.S3) && (target < STATE.S3))
            {
                _twain.DatParent(DG.CONTROL, MSG.CLOSEDSM, ref _hWnd);
            }
        }

        private Capabilities _caps;

        /// <summary>
        /// Get current data source's capabilities. Will be null if no data source is open.
        /// </summary>
        /// <returns></returns>
        public Capabilities Capabilities
        {
            get
            {
                if (State >= STATE.S4)
                {
                    return _caps ?? (_caps = new Capabilities(_twain));
                }
                return null;
            }
        }

        /// <summary>
        /// Gets/sets the current source's settings as opaque data.
        /// Returns null if not supported.
        /// </summary>
        public byte[] CustomDsData
        {
            get
            {
                TW_CUSTOMDSDATA data = default;
                var sts = _twain.DatCustomdsdata(DG.CONTROL, MSG.GET, ref data);
                if (sts == STS.SUCCESS)
                {
                    if (data.hData != IntPtr.Zero && data.InfoLength > 0)
                    {
                        try
                        {
                            var lockedPtr = _twain.DsmMemLock(data.hData);
                            var bytes = new byte[data.InfoLength];
                            Marshal.Copy(lockedPtr, bytes, 0, bytes.Length);
                        }
                        finally
                        {
                            _twain.DsmMemUnlock(data.hData);
                            _twain.DsmMemFree(ref data.hData);
                        }
                    }
                    return EmptyArray<byte>.Value;
                }
                return null;
            }
            set
            {
                if (value == null || value.Length == 0) return;

                TW_CUSTOMDSDATA data = default;
                data.InfoLength = (uint)value.Length;
                data.hData = _twain.DsmMemAlloc(data.InfoLength);
                try
                {
                    var lockedPtr = _twain.DsmMemLock(data.hData);
                    Marshal.Copy(value, 0, lockedPtr, value.Length);
                    _twain.DsmMemUnlock(data.hData);
                    var sts = _twain.DatCustomdsdata(DG.CONTROL, MSG.SET, ref data);
                }
                finally
                {
                    // should be freed already if no error but just in case
                    if (data.hData != IntPtr.Zero) _twain.DsmMemFree(ref data.hData);
                }
            }
        }


        /// <summary>
        /// Attempts to show the current data source's settings dialog if supported.
        /// </summary>
        /// <returns></returns>
        public STS ShowSettings()
        {
            TW_USERINTERFACE ui = default;
            ui.hParent = _hWnd;
            ui.ShowUI = 1;
            return _twain.DatUserinterface(DG.CONTROL, MSG.ENABLEDSUIONLY, ref ui);
        }

        /// <summary>
        /// Begins the capture process on the current data source.
        /// </summary>
        /// <param name="showUI">Whether to display settings UI. Not all data sources support this.</param>
        /// <returns></returns>
        public STS StartCapture(bool showUI)
        {
            TW_USERINTERFACE ui = default;
            ui.hParent = _hWnd;
            ui.ShowUI = (ushort)(showUI ? 1 : 0);
            return _twain.DatUserinterface(DG.CONTROL, MSG.ENABLEDS, ref ui);
        }

        /// <summary>
        /// Stops the data source's automated feeder
        /// if <see cref="Capabilities.CAP_AUTOSCAN"/> is set to true.
        /// </summary>
        /// <returns></returns>
        public STS StopCapture()
        {
            TW_PENDINGXFERS pending = default;
            return _twain.DatPendingxfers(DG.CONTROL, MSG.STOPFEEDER, ref pending);
        }

        /// <summary>
        /// Reads information relating to the last capture run.
        /// Only valid on state 4 after a capture.
        /// </summary>
        public Metrics GetMetrics()
        {
            TW_METRICS twmetrics = default;
            twmetrics.SizeOf = (uint)Marshal.SizeOf(twmetrics);
            var sts = _twain.DatMetrics(DG.CONTROL, MSG.GET, ref twmetrics);
            if (sts == STS.SUCCESS)
            {
                return new Metrics
                {
                    ReturnCode = sts,
                    Images = (int)twmetrics.ImageCount,
                    Sheets = (int)twmetrics.SheetCount
                };
            }
            return new Metrics { ReturnCode = sts };
        }

        /// <summary>
        /// Sends a TWAIN Direct task from the application to the driver.
        /// </summary>
        /// <param name="taskJson">The TWAIN Direct task in JSON.</param>
        /// <param name="communicationManager">The current system being used to connect the application to the scanner.</param>
        /// <returns></returns>
        public TwainDirectTaskResult SetTwainDirectTask(string taskJson, ushort communicationManager = 0)
        {
            var result = new TwainDirectTaskResult { ReturnCode = STS.FAILURE };
            TW_TWAINDIRECT task = default;
            try
            {
                task.SizeOf = (uint)Marshal.SizeOf(typeof(TW_TWAINDIRECT));
                task.CommunicationManager = communicationManager;
                task.Send = ValueWriter.StringToPtrUTF8(_twain, taskJson, out int length);
                task.SendSize = (uint)length;

                result.ReturnCode = _twain.DatTwaindirect(DG.CONTROL, MSG.SETTASK, ref task);
                if (result.ReturnCode == STS.SUCCESS && task.ReceiveSize > 0 && task.Receive != IntPtr.Zero)
                {
                    result.ResponseJson = ValueReader.PtrToStringUTF8(task.Receive, (int)task.ReceiveSize);
                }
            }
            finally
            {
                if (task.Send != IntPtr.Zero) _twain.DsmMemFree(ref task.Send); // just in case
                if (task.Receive != IntPtr.Zero) _twain.DsmMemFree(ref task.Receive);
            }
            return result;
        }

        #endregion
    }
}
