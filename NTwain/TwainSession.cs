using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
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

            _twain = new TWAIN(info.CompanyName, info.ProductName, info.ProductName,
                (ushort)TWON_PROTOCOL.MAJOR, (ushort)TWON_PROTOCOL.MINOR,
               (uint)(DG.APP2 | DG.IMAGE),
               country, "", language, 2, 4, false, true, HandleDeviceEvent, HandleScanEvent, HandleUIThreadAction, hWnd);

            _threadMarshaller = threadMarshaller ?? new NoParticularMarshaller();
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
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Gets the low-level twain object.
        /// </summary>
        public TWAIN TWAIN { get { return _twain; } }

        #region event callbacks

        private void HandleUIThreadAction(Action action)
        {
            _threadMarshaller.Invoke(action);
        }

        private STS HandleScanEvent(bool a_blClosing)
        {
            _threadMarshaller.BeginInvoke(new Action<bool>(RaiseScanEvent), a_blClosing);
            return STS.SUCCESS;
        }
        void RaiseScanEvent(bool closing)
        {
            OnScanEvent(closing);
            ScanEvent?.Invoke(this, closing);
        }

        protected virtual void OnScanEvent(bool closing) { }
        public event EventHandler<bool> ScanEvent;

        private STS HandleDeviceEvent()
        {
            STS sts;
            TW_DEVICEEVENT twdeviceevent;

            // Drain the event queue...
            while (true)
            {
                // Try to get an event...
                twdeviceevent = default;
                sts = _twain.DatDeviceevent(DG.CONTROL, MSG.GET, ref twdeviceevent);
                if (sts != STS.SUCCESS)
                {
                    break;
                }
                else
                {
                    RaiseDeviceEvent(twdeviceevent);
                }
            }

            // Return a status, in case we ever need it for anything...
            return STS.SUCCESS;
        }

        void RaiseDeviceEvent(TW_DEVICEEVENT twdeviceevent)
        {
            OnDeviceEvent(twdeviceevent);
            DeviceEvent?.Invoke(this, twdeviceevent);
        }

        protected virtual void OnDeviceEvent(TW_DEVICEEVENT twdeviceevent) { }

        public event EventHandler<TW_DEVICEEVENT> DeviceEvent;

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
        /// Gets the manager status. Useful after getting a non-success return code.
        /// </summary>
        /// <returns></returns>
        public TW_STATUS GetStatus()
        {
            TW_STATUS stat = default;
            var sts = _twain.DatStatus(DG.CONTROL, MSG.GET, ref stat);
            return stat;
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
        /// This is called when <see cref="Dispose"/> is invoked.
        /// </summary>
        public void Close()
        {
            StepDown(STATE.S2);
        }

        /// <summary>
        /// Gets list of TWAIN devices.
        /// </summary>
        /// <returns></returns>
        public IList<TW_IDENTITY> GetDevices()
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
        /// Gets or sets the default device.
        /// </summary>
        public TW_IDENTITY? DefaultDevice
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
        /// Gets or sets the currently open device.
        /// Setting it will try to open it.
        /// </summary>
        public TW_IDENTITY? CurrentDevice
        {
            get
            {
                if (State > STATE.S3)
                {
                    TW_IDENTITY twidentity = default;
                    if (CsvSerializer.CsvToIdentity(ref twidentity, _twain.GetDsIdentity()))
                    {
                        return twidentity;
                    }
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
        /// <param name="state"></param>
        public void StepDown(STATE state)
        {
            TW_PENDINGXFERS twpendingxfers = default;

            // Make sure we have something to work with...
            if (_twain == null)
            {
                return;
            }

            // Walk the states, we don't care about the status returns.  Basically,
            // these need to work, or we're guaranteed to hang...

            // 7 --> 6
            if ((_twain.GetState() == STATE.S7) && (state < STATE.S7))
            {
                _twain.DatPendingxfers(DG.CONTROL, MSG.ENDXFER, ref twpendingxfers);
            }

            // 6 --> 5
            if ((_twain.GetState() == STATE.S6) && (state < STATE.S6))
            {
                _twain.DatPendingxfers(DG.CONTROL, MSG.RESET, ref twpendingxfers);
            }

            // 5 --> 4
            if ((_twain.GetState() == STATE.S5) && (state < STATE.S5))
            {
                TW_USERINTERFACE twuserinterface = default;
                _twain.DatUserinterface(DG.CONTROL, MSG.DISABLEDS, ref twuserinterface);
            }

            // 4 --> 3
            if ((_twain.GetState() == STATE.S4) && (state < STATE.S4))
            {
                _caps = null;
                TW_IDENTITY twidentity = default;
                CsvSerializer.CsvToIdentity(ref twidentity, _twain.GetDsIdentity());
                _twain.DatIdentity(DG.CONTROL, MSG.CLOSEDS, ref twidentity);
            }

            // 3 --> 2
            if ((_twain.GetState() == STATE.S3) && (state < STATE.S3))
            {
                _twain.DatParent(DG.CONTROL, MSG.CLOSEDSM, ref _hWnd);
            }
        }

        private Capabilities _caps;

        /// <summary>
        /// Get current device's capabilities. Will be null if no device is open.
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
        /// Attempts to show the current device's settings dialog if supported.
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
        /// Begins the capture process on the current device.
        /// </summary>
        /// <param name="showUI">Whether to display settings UI. Not all devices support this.</param>
        /// <returns></returns>
        public STS StartCapture(bool showUI)
        {
            TW_USERINTERFACE ui = default;
            ui.hParent = _hWnd;
            ui.ShowUI = (ushort)(showUI ? 1 : 0);
            return _twain.DatUserinterface(DG.CONTROL, MSG.ENABLEDS, ref ui);
        }

        #endregion
    }
}
