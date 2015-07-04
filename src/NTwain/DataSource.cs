using NTwain.Data;
using NTwain.Internals;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
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
            ProtocolVersion = new Version(sourceId.ProtocolMajor, sourceId.ProtocolMinor);
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
                PlatformInfo.Current.Log.Debug("Thread {0}: OpenSource.", Thread.CurrentThread.ManagedThreadId);

                rc = _session.DGControl.Identity.OpenDS(this);
                _session.UpdateCallback();
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
                PlatformInfo.Current.Log.Debug("Thread {0}: CloseSource.", Thread.CurrentThread.ManagedThreadId);

                rc = _session.DGControl.Identity.CloseDS();
                //if (rc == ReturnCode.Success)
                //{
                //    SupportedCaps = null;
                //}
                _session.UpdateCallback();
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
        /// Gets the id of the source.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get { return Identity.Id; } }

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
        /// Gets the source's version information.
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
        /// Gets the supported TWAIN protocol version.
        /// </summary>
        /// <value>
        /// The protocol version.
        /// </value>
        public Version ProtocolVersion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this data source has been opened.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this data source is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen { get { return _session.IsSourceOpen && _session.CurrentSource == this; } }

        /// <summary>
        /// Gets or sets the current settings (CustomDSData) of this source if supported.
        /// </summary>
        /// <value>
        /// The source settings.
        /// </value>
        public byte[] Settings
        {
            get
            {
                byte[] value = null;
                if (Capabilities.CapCustomDSData.GetCurrent() == BoolType.True)
                {
                    TWCustomDSData data;
                    if (DGControl.CustomDSData.Get(out data) == ReturnCode.Success && data.InfoLength > 0)
                    {
                        try
                        {
                            value = new byte[data.InfoLength];
                            var ptr = PlatformInfo.Current.MemoryManager.Lock(data.hData);
                            Marshal.Copy(ptr, value, 0, (int)data.InfoLength);
                        }
                        finally
                        {
                            PlatformInfo.Current.MemoryManager.Unlock(data.hData);
                            PlatformInfo.Current.MemoryManager.Free(data.hData);
                        }
                    }
                }
                return value;
            }
            set
            {
                if (value != null && value.Length > 0 &&
                    Capabilities.CapCustomDSData.GetCurrent() == BoolType.True)
                {
                    TWCustomDSData data = new TWCustomDSData
                    {
                        InfoLength = (uint)value.Length
                    };
                    try
                    {
                        data.hData = PlatformInfo.Current.MemoryManager.Allocate(data.InfoLength);
                        var ptr = PlatformInfo.Current.MemoryManager.Lock(data.hData);
                        Marshal.Copy(value, 0, ptr, value.Length);
                        var rc = DGControl.CustomDSData.Set(data);
                        if (rc != ReturnCode.Success)
                        {
                            // do something
                        }
                    }
                    finally
                    {
                        if (data.hData != IntPtr.Zero)
                        {
                            PlatformInfo.Current.MemoryManager.Unlock(data.hData);
                            PlatformInfo.Current.MemoryManager.Free(data.hData);
                        }
                    }
                }
            }
        }

        //static readonly CapabilityId[] _emptyCapList = new CapabilityId[0];

        //private IList<CapabilityId> _supportedCapsList;
        ///// <summary>
        ///// Gets the list of supported caps for this source. 
        ///// </summary>
        ///// <value>
        ///// The supported caps.
        ///// </value>
        //[Obsolete("Use CapSupportedCaps.Get() instead.")]
        //public IList<CapabilityId> SupportedCaps
        //{
        //    get
        //    {
        //        if (_supportedCapsList == null && _session.State > 3)
        //        {
        //            _supportedCapsList = CapSupportedCaps.GetValues();
        //        }
        //        return _supportedCapsList ?? _emptyCapList;
        //    }
        //    private set
        //    {
        //        _supportedCapsList = value;
        //        //OnPropertyChanged("SupportedCaps");
        //    }
        //}

        private Capabilities _caps;

        /// <summary>
        /// Gets the capabilities for this data source.
        /// </summary>
        /// <value>
        /// The capabilities.
        /// </value>
        public Capabilities Capabilities
        {
            get { return _caps ?? (_caps = new Capabilities(this)); }
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

        internal DGAudio DGAudio
        {
            get
            {
                return _session.DGAudio;
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
