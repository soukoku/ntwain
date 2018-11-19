using NTwain.Data;
using NTwain.Triplets;
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
    /// Manages a TWAIN session.
    /// </summary>
    public partial class TwainSession
    {
        internal readonly TwainConfig Config;

        private IntPtr _hWnd;
        // cache generated twain sources so if you get same source from same session it'll return the same object
        readonly Dictionary<string, DataSource> _ownedSources = new Dictionary<string, DataSource>();
        // need to keep delegate around to prevent GC?
        readonly Callback32 _callback32Delegate;


        /// <summary>
        /// Constructs a new <see cref="TwainSession"/>.
        /// </summary>
        /// <param name="config"></param>
        public TwainSession(TwainConfig config)
        {
            Config = config;
            switch (config.Platform)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                default:
                    _callback32Delegate = new Callback32(Handle32BitCallback);
                    break;
            }
        }

        /// <summary>
        /// Opens the TWAIN session.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public ReturnCode Open(IntPtr hWnd)
        {
            _hWnd = hWnd;
            return DGControl.Parent.OpenDSM(hWnd);
        }

        /// <summary>
        /// Steps down to the target session state.
        /// </summary>
        /// <param name="targetState"></param>
        /// <returns></returns>
        public ReturnCode StepDown(TwainState targetState)
        {
            var rc = ReturnCode.Failure;
            while (State > targetState)
            {
                switch (State)
                {
                    case TwainState.DsmOpened:
                        rc = DGControl.Parent.CloseDSM(_hWnd);
                        if (rc != ReturnCode.Success) return rc;
                        break;
                    case TwainState.SourceOpened:
                        rc = DGControl.Identity.CloseDS(CurrentSource.Identity32);
                        if (rc != ReturnCode.Success) return rc;
                        break;
                }
            }
            return rc;
        }

        /// <summary>
        /// Gets the manager status. Useful after getting a non-success return code.
        /// </summary>
        /// <returns></returns>
        public TW_STATUS GetStatus()
        {
            TW_STATUS stat = default;
            var rc = DGControl.Status.Get(ref stat, null);
            return stat;
        }

        /// <summary>
        /// Gets the translated string for a <see cref="TW_STATUS"/>.
        /// </summary>
        /// <param name="status"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public string GetLocalizedStatus(ref TW_STATUS status, DataSource source = null)
        {
            var rc = DGControl.StatusUtf8.Get(ref status, source, out string message);
            return message;
        }

        internal void RegisterCallback()
        {
            // TODO: support other platforms
            var callbackPtr = Marshal.GetFunctionPointerForDelegate(_callback32Delegate);

            // try new callback first
            var cb2 = new TW_CALLBACK2 { CallBackProc = callbackPtr };
            var rc = DGControl.Callback2.RegisterCallback(ref cb2);
            if (rc == ReturnCode.Success) Debug.WriteLine("Registed Callback2 success.");
            else
            {
                var status = GetStatus();
                Debug.WriteLine($"Register Callback2 failed with condition code: {status.ConditionCode}.");
            }


            if (rc != ReturnCode.Success)
            {
                // always register old callback
                var cb = new TW_CALLBACK { CallBackProc = callbackPtr };

                rc = DGControl.Callback.RegisterCallback(ref cb);

                if (rc == ReturnCode.Success) Debug.WriteLine("Registed Callback success.");
                else
                {
                    var status = GetStatus();
                    Debug.WriteLine($"Register Callback failed with {status.ConditionCode}.");
                }
            }
        }


        /// <summary>
        /// Enumerate list of sources available on the machine.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataSource> GetSources()
        {
            var rc = DGControl.Identity.GetFirst(out TW_IDENTITY srcId);
            while (rc == ReturnCode.Success)
            {
                yield return GetSourceSingleton(srcId);
                rc = DGControl.Identity.GetNext(out srcId);
            }
        }

        /// <summary>
        /// Gets/sets the default data source. Setting to null is not supported.
        /// </summary>
        public DataSource DefaultSource
        {
            get
            {
                if (DGControl.Identity.GetDefault(out TW_IDENTITY src) == ReturnCode.Success)
                {
                    return GetSourceSingleton(src);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    if (value.Session != this)
                    {
                        throw new InvalidOperationException("Source is not from this session.");
                    }
                    var rc = DGControl.Identity.Set(value);
                    RaisePropertyChanged(nameof(DefaultSource));
                }
            }
        }

        /// <summary>
        /// Tries to show the built-in source selector dialog and return the selected source.
        /// </summary>
        /// <returns></returns>
        public DataSource ShowSourceSelector()
        {
            if (DGControl.Identity.UserSelect(out TW_IDENTITY id) == ReturnCode.Success)
            {
                return GetSourceSingleton(id);
            }
            return null;
        }

        private DataSource _currentSource;

        /// <summary>
        /// Gets the currently open data source.
        /// </summary>
        public DataSource CurrentSource
        {
            get { return _currentSource; }
            internal set
            {
                var old = _currentSource;
                _currentSource = value;

                RaisePropertyChanged(nameof(CurrentSource));
                old?.RaisePropertyChanged(nameof(DataSource.IsOpen));
                value?.RaisePropertyChanged(nameof(DataSource.IsOpen));
            }
        }


        internal DataSource GetSourceSingleton(TW_IDENTITY sourceId)
        {
            DataSource source = null;
            var key = $"{sourceId.Id}|{sourceId.Manufacturer}|{sourceId.ProductFamily}|{sourceId.ProductName}";
            if (_ownedSources.ContainsKey(key))
            {
                source = _ownedSources[key];
            }
            else
            {
                _ownedSources[key] = source = new DataSource(this, sourceId);
            }
            return source;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"Session: {State}";
        }
    }
}
