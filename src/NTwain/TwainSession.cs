using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

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

        /// <summary>
        /// Constructs a new <see cref="TwainSession"/>.
        /// </summary>
        /// <param name="config"></param>
        public TwainSession(TwainConfig config)
        {
            Config = config;
        }

        /// <summary>
        /// Opens the TWAIN session.
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public ReturnCode Open(ref IntPtr hWnd)
        {
            _hWnd = hWnd;
            return DGControl.Parent.OpenDSM(ref hWnd);
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
                        rc = DGControl.Parent.CloseDSM(ref _hWnd);
                        if (rc != ReturnCode.Success) return rc;
                        break;
                }
            }
            return rc;
        }

        /// <summary>
        /// Gets list of sources available on the machine.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DataSource> GetSources()
        {
            TW_IDENTITY srcId;
            var rc = DGControl.Identity.GetFirst(out srcId);
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
                TW_IDENTITY src;
                if (DGControl.Identity.GetDefault(out src) == ReturnCode.Success)
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
            TW_IDENTITY id;
            if (DGControl.Identity.UserSelect(out id) == ReturnCode.Success)
            {
                return GetSourceSingleton(id);
            }
            return null;
        }

        /// <summary>
        /// Gets the currently open data source.
        /// </summary>
        /// <value>
        /// The current source.
        /// </value>
        public DataSource CurrentSource { get; internal set; }

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
    }
}
