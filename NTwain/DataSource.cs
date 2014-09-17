using NTwain.Data;
using NTwain.Internals;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// Represents a TWAIN data source.
    /// </summary>
    public partial class DataSource : IDataSource
    {
        ITwainSessionInternal _session;

        internal DataSource(ITwainSessionInternal session, TWIdentity sourceId)
        {
            _session = session;
            Identity = sourceId;
        }

        /// <summary>
        /// Opens the source for capability negotiation.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Open()
        {
            var rc = ReturnCode.Failure;
            _session.MessageLoopHook.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: OpenSource.", Thread.CurrentThread.ManagedThreadId));

                rc = _session.DGControl.Identity.OpenDS(this);
            });
            return rc;
        }

        /// <summary>
        /// Closes the source.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Close()
        {
            var rc = ReturnCode.Failure;
            _session.MessageLoopHook.Invoke(() =>
            {
                Debug.WriteLine(string.Format(CultureInfo.InvariantCulture, "Thread {0}: CloseSource.", Thread.CurrentThread.ManagedThreadId));

                rc = _session.DGControl.Identity.CloseDS();
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
        [Obsolete("Use Enable() instead.")]
        public ReturnCode StartTransfer(SourceEnableMode mode, bool modal, IntPtr windowHandle)
        {
            return Enable(mode, modal, windowHandle);
        }

        /// <summary>
        /// Enables the source to start transferring.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI will display as modal.</param>
        /// <param name="windowHandle">The window handle if modal.</param>
        /// <returns></returns>
        public ReturnCode Enable(SourceEnableMode mode, bool modal, IntPtr windowHandle)
        {
            return _session.EnableSource(mode, modal, windowHandle);
        }

        /// <summary>
        /// Gets the source status. Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public TWStatus GetStatus()
        {
            TWStatus stat;
            _session.DGControl.Status.GetSource(out stat);
            return stat;
        }
        /// <summary>
        /// Gets the source status. Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public TWStatusUtf8 GetStatusUtf8()
        {
            TWStatusUtf8 stat;
            _session.DGControl.StatusUtf8.GetSource(out stat);
            return stat;
        }


        #region properties

        internal TWIdentity Identity { get; private set; }

        /// <summary>
        /// Gets the source's product name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return Identity.ProductName; } }

        /// <summary>
        /// Gets the source's manufacturer name.
        /// </summary>
        /// <value>
        /// The manufacturer.
        /// </value>
        public string Manufacturer { get { return Identity.Manufacturer; } }

        /// <summary>
        /// Gets the source's product family.
        /// </summary>
        /// <value>
        /// The product family.
        /// </value>
        public string ProductFamily { get { return Identity.ProductFamily; } }

        /// <summary>
        /// Gets the version information.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public TWVersion Version { get { return Identity.Version; } }

        /// <summary>
        /// Gets the supported data group.
        /// </summary>
        /// <value>
        /// The data group.
        /// </value>
        public DataGroups DataGroup { get { return Identity.DataGroup; } }

        /// <summary>
        /// Gets the supported TWAIN protocol major number.
        /// </summary>
        /// <value>
        /// The protocol major number.
        /// </value>
        public int ProtocolMajor { get { return Identity.ProtocolMajor; } }

        /// <summary>
        /// Gets the supported TWAIN protocol minor number.
        /// </summary>
        /// <value>
        /// The protocol minor number.
        /// </value>
        public int ProtocolMinor { get { return Identity.ProtocolMinor; } }


        static readonly CapabilityId[] _emptyCapList = new CapabilityId[0];

        private IList<CapabilityId> _supportedCaps;
        /// <summary>
        /// Gets the supported caps for this source.
        /// </summary>
        /// <value>
        /// The supported caps.
        /// </value>
        public IList<CapabilityId> SupportedCaps
        {
            get
            {
                if (_supportedCaps == null && _session.State > 3)
                {
                    _supportedCaps = CapGet(CapabilityId.CapSupportedCaps).CastToEnum<CapabilityId>(false);
                }
                return _supportedCaps ?? _emptyCapList;
            }
            private set
            {
                _supportedCaps = value;
                //OnPropertyChanged("SupportedCaps");
            }
        }


        /// <summary>
        /// Gets the triplet operations defined for control data group.
        /// </summary>
        public DGControl DGControl
        {
            get
            {
                return _session.DGControl;
            }
        }

        /// <summary>
        /// Gets the triplet operations defined for image data group.
        /// </summary>
        public DGImage DGImage
        {
            get
            {
                return _session.DGImage;
            }
        }

        /// <summary>
        /// Gets the direct triplet operation entry for custom values.
        /// </summary>
        public DGCustom DGCustom
        {
            get
            {
                return _session.DGCustom;
            }
        }

        #endregion

        //#region INotifyPropertyChanged Members

        ///// <summary>
        ///// Occurs when a property value changes.
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        ///// <summary>
        ///// Raises the <see cref="PropertyChanged"/> event.
        ///// </summary>
        ///// <param name="propertyName">Name of the property.</param>
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    var syncer = Session.SynchronizationContext;
        //    if (syncer == null)
        //    {
        //        try
        //        {
        //            var hand = PropertyChanged;
        //            if (hand != null) { hand(this, new PropertyChangedEventArgs(propertyName)); }
        //        }
        //        catch { }
        //    }
        //    else
        //    {
        //        syncer.Post(o =>
        //        {
        //            try
        //            {
        //                var hand = PropertyChanged;
        //                if (hand != null) { hand(this, new PropertyChangedEventArgs(propertyName)); }
        //            }
        //            catch { }
        //        }, null);
        //    }
        //}

        //#endregion

        #region cameras

        /// <summary>
        /// [Experimental] Gets the cameras supported by the source.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetCameras()
        {
            TWFileSystem fs = new TWFileSystem();
            List<string> cams = new List<string>();
            var rc = DGControl.FileSystem.GetFirstFile(fs);
            while (rc == ReturnCode.Success)
            {
                switch (fs.FileType)
                {
                    case FileType.Camera:
                    case FileType.CameraBottom:
                    case FileType.CameraTop:
                    case FileType.CameraPreview:
                        cams.Add(fs.OutputName);
                        break;
                }
                rc = DGControl.FileSystem.GetNextFile(fs);
            }
            return cams;
        }

        /// <summary>
        /// [Experimental] Sets the target camera for cap negotiation that can be set per camera.
        /// </summary>
        /// <param name="cameraName"></param>
        /// <returns></returns>
        public ReturnCode SetCamera(string cameraName)
        {
            TWFileSystem fs = new TWFileSystem();
            fs.InputName = cameraName;
            return DGControl.FileSystem.ChangeDirectory(fs);
        }

        #endregion
    }
}
