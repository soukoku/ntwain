using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    // This contains all cap-related methods prefixed with Cap.
    // It will attempt to have all known cap abilities defined 
    // with a wrapper unless it can only exist as part of another cap
    // or it's lame & nobody uses it.


    partial class DataSource : ICapControl
    {
        #region low-level cap stuff

        /// <summary>
        /// Gets the actual supported operations for a capability.
        /// </summary>
        /// <param name="capId">The cap identifier.</param>
        /// <returns></returns>
        public QuerySupports CapQuerySupport(CapabilityId capId)
        {
            QuerySupports retVal = QuerySupports.None;
            using (TWCapability cap = new TWCapability(capId))
            {
                var rc = _session.DGControl.Capability.QuerySupport(cap);
                if (rc == ReturnCode.Success)
                {
                    var read = CapabilityReader.ReadValue(cap);

                    if (read.ContainerType == ContainerType.OneValue)
                    {
                        retVal = read.OneValue.ConvertToEnum<QuerySupports>();
                    }
                }
            }
            return retVal;
        }

        /// <summary>
        /// Gets the current value for a capability.
        /// </summary>
        /// <param name="capId">The cap id.</param>
        /// <returns></returns>
        public object CapGetCurrent(CapabilityId capId)
        {
            using (TWCapability cap = new TWCapability(capId))
            {
                var rc = _session.DGControl.Capability.GetCurrent(cap);
                if (rc == ReturnCode.Success)
                {
                    var read = CapabilityReader.ReadValue(cap);

                    switch (read.ContainerType)
                    {
                        case ContainerType.Enum:
                            if (read.CollectionValues != null && read.CollectionValues.Count > read.EnumCurrentIndex)
                            {
                                return read.CollectionValues[read.EnumCurrentIndex];
                            }
                            break;
                        case ContainerType.OneValue:
                            return read.OneValue;
                        case ContainerType.Range:
                            return read.RangeCurrentValue;
                        case ContainerType.Array:
                            // no source should ever return an array but anyway
                            if (read.CollectionValues != null)
                            {
                                return read.CollectionValues.FirstOrDefault();
                            }
                            break;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the default value for a capability.
        /// </summary>
        /// <param name="capId">The cap id.</param>
        /// <returns></returns>
        public object CapGetDefault(CapabilityId capId)
        {
            using (TWCapability cap = new TWCapability(capId))
            {
                var rc = _session.DGControl.Capability.GetDefault(cap);
                if (rc == ReturnCode.Success)
                {
                    var read = CapabilityReader.ReadValue(cap);

                    switch (read.ContainerType)
                    {
                        case ContainerType.Enum:
                            if (read.CollectionValues != null && read.CollectionValues.Count > read.EnumDefaultIndex)
                            {
                                return read.CollectionValues[read.EnumDefaultIndex];
                            }
                            break;
                        case ContainerType.OneValue:
                            return read.OneValue;
                        case ContainerType.Range:
                            return read.RangeDefaultValue;
                        case ContainerType.Array:
                            // no source should ever return an array but anyway
                            if (read.CollectionValues != null)
                            {
                                return read.CollectionValues.FirstOrDefault();
                            }
                            break;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// A general method that tries to get capability values from current <see cref="DataSource" />.
        /// </summary>
        /// <param name="capabilityId">The capability unique identifier.</param>
        /// <returns></returns>
        public IList<object> CapGet(CapabilityId capabilityId)
        {
            var list = new List<object>();
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = _session.DGControl.Capability.Get(cap);
                if (rc == ReturnCode.Success)
                {
                    cap.ReadMultiCapValues(list);
                }
            }
            return list;
        }

        /// <summary>
        /// Resets all values and constraint to power-on defaults.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        public ReturnCode CapResetAll(CapabilityId capabilityId)
        {
            using (TWCapability cap = new TWCapability(capabilityId)
            {
                ContainerType = ContainerType.DoNotCare
            })
            {
                var rc = DGControl.Capability.ResetAll(cap);
                return rc;
            }
        }

        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <param name="capabilityId">The capability identifier.</param>
        /// <returns></returns>
        public ReturnCode CapReset(CapabilityId capabilityId)
        {
            using (TWCapability cap = new TWCapability(capabilityId)
            {
                ContainerType = ContainerType.DoNotCare
            })
            {
                var rc = DGControl.Capability.Reset(cap);
                return rc;
            }
        }

        #endregion

        #region high-level caps stuff

        #region audio caps

        private CapWrapper<XferMech> _audXferMech;

        /// <summary>
        /// Gets the property to work with audio <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The audio xfer mech.
        /// </value>
        public CapWrapper<XferMech> CapAudioXferMech
        {
            get
            {
                return _audXferMech ?? (_audXferMech = new CapWrapper<XferMech>(this, CapabilityId.ACapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
                        value => new TWCapability(CapabilityId.ACapXferMech, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }

        #endregion

        #region img caps

        private CapWrapper<Unit> _imgUnits;

        /// <summary>
        /// Gets the property to work with image <see cref="Unit"/> for the current source.
        /// </summary>
        /// <value>
        /// The image unit of measure.
        /// </value>
        public CapWrapper<Unit> CapImageUnits
        {
            get
            {
                return _imgUnits ?? (_imgUnits = new CapWrapper<Unit>(this, CapabilityId.ICapUnits, ValueExtensions.ConvertToEnum<Unit>,
                        value => new TWCapability(CapabilityId.ICapUnits, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }

        private CapWrapper<XferMech> _imgXferMech;

        /// <summary>
        /// Gets the property to work with image <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The image xfer mech.
        /// </value>
        public CapWrapper<XferMech> CapImageXferMech
        {
            get
            {
                return _imgXferMech ?? (_imgXferMech = new CapWrapper<XferMech>(this, CapabilityId.ICapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
                        value => new TWCapability(CapabilityId.ICapXferMech, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }


        private CapWrapper<CompressionType> _compression;

        /// <summary>
        /// Gets the property to work with image <see cref="CompressionType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image compression.
        /// </value>
        public CapWrapper<CompressionType> CapImageCompression
        {
            get
            {
                return _compression ?? (_compression = new CapWrapper<CompressionType>(this, CapabilityId.ICapCompression, ValueExtensions.ConvertToEnum<CompressionType>,
                        value => new TWCapability(CapabilityId.ICapCompression, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }


        private CapWrapper<FileFormat> _fileFormat;

        /// <summary>
        /// Gets the property to work with image <see cref="FileFormat"/> for the current source.
        /// </summary>
        /// <value>
        /// The image file format.
        /// </value>
        public CapWrapper<FileFormat> CapImageFileFormat
        {
            get
            {
                return _fileFormat ?? (_fileFormat = new CapWrapper<FileFormat>(this, CapabilityId.ICapImageFileFormat, ValueExtensions.ConvertToEnum<FileFormat>,
                        value => new TWCapability(CapabilityId.ICapImageFileFormat, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }


        private CapWrapper<PixelType> _pixelType;

        /// <summary>
        /// Gets the property to work with image <see cref="PixelType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel type.
        /// </value>
        public CapWrapper<PixelType> CapImagePixelType
        {
            get
            {
                return _pixelType ?? (_pixelType = new CapWrapper<PixelType>(this, CapabilityId.ICapPixelType, ValueExtensions.ConvertToEnum<PixelType>,
                        value => new TWCapability(CapabilityId.ICapPixelType, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }


        private CapWrapper<SupportedSize> _supportSize;

        /// <summary>
        /// Gets the property to work with image <see cref="SupportedSize"/> for the current source.
        /// </summary>
        /// <value>
        /// The image supported size.
        /// </value>
        public CapWrapper<SupportedSize> CapImageSupportedSize
        {
            get
            {
                return _supportSize ?? (_supportSize = new CapWrapper<SupportedSize>(this, CapabilityId.ICapSupportedSizes, ValueExtensions.ConvertToEnum<SupportedSize>,
                        value => new TWCapability(CapabilityId.ICapSupportedSizes, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }


        private CapWrapper<BoolType> _autoDeskew;

        /// <summary>
        /// Gets the property to work with image auto deskew flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto deskew flag.
        /// </value>
        public CapWrapper<BoolType> CapImageAutoDeskew
        {
            get
            {
                return _autoDeskew ?? (_autoDeskew = new CapWrapper<BoolType>(this, CapabilityId.ICapAutomaticDeskew, ValueExtensions.ConvertToEnum<BoolType>, value =>
                        new TWCapability(CapabilityId.ICapAutomaticDeskew, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        })));
            }
        }


        private CapWrapper<BoolType> _autoRotate;

        /// <summary>
        /// Gets the property to work with image auto rotate flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto rotate flag.
        /// </value>
        public CapWrapper<BoolType> CapImageAutoRotate
        {
            get
            {
                return _autoRotate ?? (_autoRotate = new CapWrapper<BoolType>(this, CapabilityId.ICapAutomaticRotate, ValueExtensions.ConvertToEnum<BoolType>, value =>
                         new TWCapability(CapabilityId.ICapAutomaticRotate, new TWOneValue
                         {
                             Item = (uint)value,
                             ItemType = ItemType.Bool
                         })));
            }
        }


        private CapWrapper<TWFix32> _xResolution;

        /// <summary>
        /// Gets the property to work with image horizontal resolution for the current source.
        /// </summary>
        /// <value>
        /// The image horizontal resolution.
        /// </value>
        public CapWrapper<TWFix32> CapImageXResolution
        {
            get
            {
                return _xResolution ?? (_xResolution = new CapWrapper<TWFix32>(this, CapabilityId.ICapXResolution, ValueExtensions.ConvertToFix32,
                        value => new TWCapability(CapabilityId.ICapXResolution, new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        })));
            }
        }


        private CapWrapper<TWFix32> _yResolution;

        /// <summary>
        /// Gets the property to work with image vertical resolution for the current source.
        /// </summary>
        /// <value>
        /// The image vertical resolution.
        /// </value>
        public CapWrapper<TWFix32> CapImageYResolution
        {
            get
            {
                return _yResolution ?? (_yResolution = new CapWrapper<TWFix32>(this, CapabilityId.ICapYResolution, ValueExtensions.ConvertToFix32,
                        value => new TWCapability(CapabilityId.ICapYResolution, new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        })));
            }
        }


        private CapWrapper<BoolType> _borderDetect;

        /// <summary>
        /// Gets the property to work with auto border detection flag for the current source.
        /// </summary>
        /// <value>
        /// The auto border detection flag.
        /// </value>
        public CapWrapper<BoolType> CapImageAutomaticBorderDetection
        {
            get
            {
                return _borderDetect ?? (_borderDetect = new CapWrapper<BoolType>(this, CapabilityId.ICapAutomaticBorderDetection, ValueExtensions.ConvertToEnum<BoolType>, value =>
                    {
                        var rc = ReturnCode.Failure;

                        var one = new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        };

                        using (TWCapability capValue = new TWCapability(CapabilityId.ICapUndefinedImageSize, one))
                        {
                            rc = _session.DGControl.Capability.Set(capValue);
                        }
                        using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, one))
                        {
                            rc = _session.DGControl.Capability.Set(capValue);
                        }

                        return rc;
                    }));
            }
        }

        #endregion

        #region other caps

        private CapWrapper<Duplex> _duplex;

        /// <summary>
        /// Gets the property to see what's the duplex mode for the current source.
        /// </summary>
        /// <value>
        /// The duplex mode.
        /// </value>
        public CapWrapper<Duplex> CapDuplex
        {
            get
            {
                return _duplex ?? (_duplex = new CapWrapper<Duplex>(this, CapabilityId.CapDuplex, ValueExtensions.ConvertToEnum<Duplex>));
            }
        }

        private CapWrapper<BoolType> _duplexEnabled;

        /// <summary>
        /// Gets the property to work with duplex enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The duplex enabled flag.
        /// </value>
        public CapWrapper<BoolType> CapDuplexEnabled
        {
            get
            {
                return _duplexEnabled ?? (_duplexEnabled = new CapWrapper<BoolType>(this, CapabilityId.CapDuplexEnabled, ValueExtensions.ConvertToEnum<BoolType>, value =>
                        new TWCapability(CapabilityId.CapDuplexEnabled, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        })));
            }
        }


        private CapWrapper<int> _xferCount;

        /// <summary>
        /// Gets the property to work with xfer count for the current source.
        /// </summary>
        /// <value>
        /// The xfer count.
        /// </value>
        public CapWrapper<int> CapXferCount
        {
            get
            {
                return _xferCount ?? (_xferCount = new CapWrapper<int>(this, CapabilityId.CapXferCount, ValueExtensions.ConvertToEnum<int>, value =>
                        new TWCapability(CapabilityId.CapXferCount, new TWOneValue
                        {
                            Item = value > 0 ? (uint)value : uint.MaxValue,
                            ItemType = ItemType.UInt16
                        })));
            }
        }

        private CapWrapper<BoolType> _feederLoaded;

        /// <summary>
        /// Gets the property to work with feeder loaded flag for the current source.
        /// </summary>
        /// <value>
        /// The feeder loaded flag.
        /// </value>
        public CapWrapper<BoolType> CapFeederLoaded
        {
            get
            {
                return _feederLoaded ?? (_feederLoaded = new CapWrapper<BoolType>(this, CapabilityId.CapFeederLoaded, ValueExtensions.ConvertToEnum<BoolType>));
            }
        }

        private CapWrapper<BoolType> _feederEnabled;

        /// <summary>
        /// Gets the property to work with feeder enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The feeder enabled flag.
        /// </value>
        public CapWrapper<BoolType> CapFeederEnabled
        {
            get
            {
                return _feederEnabled ?? (_feederEnabled = new CapWrapper<BoolType>(this, CapabilityId.CapFeederEnabled, ValueExtensions.ConvertToEnum<BoolType>, value =>
                    {
                        var rc = ReturnCode.Failure;

                        var one = new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        };

                        // we will never set feeder off, only autofeed and autoscan
                        // but if it is true then enable feeder needs to be set first
                        if (value == BoolType.True)
                        {
                            using (TWCapability enabled = new TWCapability(CapabilityId.CapFeederEnabled, one))
                            {
                                rc = _session.DGControl.Capability.Set(enabled);
                            }
                        }
                        // to really use feeder we must also set autofeed or autoscan, but only
                        // for one of them since setting autoscan also sets autofeed
                        if (SupportedCaps.Contains(CapabilityId.CapAutoScan))
                        {
                            using (TWCapability autoScan = new TWCapability(CapabilityId.CapAutoScan, one))
                            {
                                rc = _session.DGControl.Capability.Set(autoScan);
                            }
                        }
                        else if (SupportedCaps.Contains(CapabilityId.CapAutoFeed))
                        {
                            using (TWCapability autoScan = new TWCapability(CapabilityId.CapAutoFeed, one))
                            {
                                rc = _session.DGControl.Capability.Set(autoScan);
                            }
                        }

                        return rc;
                    }));
            }
        }

        private CapWrapper<BoolType> _clearPage;

        /// <summary>
        /// Gets the property to work with clear page flag for the current source.
        /// </summary>
        /// <value>
        /// The clear page flag.
        /// </value>
        public CapWrapper<BoolType> CapClearPage
        {
            get
            {
                return _clearPage ?? (_clearPage = new CapWrapper<BoolType>(this, CapabilityId.CapClearPage, ValueExtensions.ConvertToEnum<BoolType>, value =>
                        new TWCapability(CapabilityId.CapClearPage, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        })));
            }
        }

        private CapWrapper<BoolType> _feedPage;

        /// <summary>
        /// Gets the property to work with feed page flag for the current source.
        /// </summary>
        /// <value>
        /// The feed page flag.
        /// </value>
        public CapWrapper<BoolType> CapFeedPage
        {
            get
            {
                return _feedPage ?? (_feedPage = new CapWrapper<BoolType>(this, CapabilityId.CapFeedPage, ValueExtensions.ConvertToEnum<BoolType>, value =>
                        new TWCapability(CapabilityId.CapFeedPage, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        })));
            }
        }

        private CapWrapper<BoolType> _rewindPage;

        /// <summary>
        /// Gets the property to work with rewind page flag for the current source.
        /// </summary>
        /// <value>
        /// The rewind page flag.
        /// </value>
        public CapWrapper<BoolType> CapRewindPage
        {
            get
            {
                return _rewindPage ?? (_rewindPage = new CapWrapper<BoolType>(this, CapabilityId.CapRewindPage, ValueExtensions.ConvertToEnum<BoolType>, value =>
                        new TWCapability(CapabilityId.CapRewindPage, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        })));
            }
        }

        private CapWrapper<BoolType> _indicators;

        /// <summary>
        /// Gets the property to work with indicators flag for the current source.
        /// </summary>
        /// <value>
        /// The indicators flag.
        /// </value>
        public CapWrapper<BoolType> CapIndicators
        {
            get
            {
                return _indicators ?? (_indicators = new CapWrapper<BoolType>(this, CapabilityId.CapIndicators, ValueExtensions.ConvertToEnum<BoolType>, value =>
                        new TWCapability(CapabilityId.CapIndicators, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        })));
            }
        }

        private CapWrapper<BoolType> _paperDetectable;

        /// <summary>
        /// Gets the property to work with paper sensor flag for the current source.
        /// </summary>
        /// <value>
        /// The paper sensor flag.
        /// </value>
        public CapWrapper<BoolType> CapPaperDetectable
        {
            get
            {
                return _paperDetectable ?? (_paperDetectable = new CapWrapper<BoolType>(this, CapabilityId.CapPaperDetectable, ValueExtensions.ConvertToEnum<BoolType>));
            }
        }

        private CapWrapper<BoolType> _uiControllable;

        /// <summary>
        /// Gets the property to work with UI controllable flag for the current source.
        /// </summary>
        /// <value>
        /// The UI controllable flag.
        /// </value>
        public CapWrapper<BoolType> CapUIControllable
        {
            get
            {
                return _uiControllable ?? (_uiControllable = new CapWrapper<BoolType>(this, CapabilityId.CapUIControllable, ValueExtensions.ConvertToEnum<BoolType>));
            }
        }

        private CapWrapper<BoolType> _devOnline;

        /// <summary>
        /// Gets the property to work with devince online flag for the current source.
        /// </summary>
        /// <value>
        /// The devince online flag.
        /// </value>
        public CapWrapper<BoolType> CapDeviceOnline
        {
            get
            {
                return _devOnline ?? (_devOnline = new CapWrapper<BoolType>(this, CapabilityId.CapDeviceOnline, ValueExtensions.ConvertToEnum<BoolType>));
            }
        }

        private CapWrapper<BoolType> _thumbsEnabled;

        /// <summary>
        /// Gets the property to work with thumbnails enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The thumbnails enabled flag.
        /// </value>
        public CapWrapper<BoolType> CapThumbnailsEnabled
        {
            get
            {
                return _thumbsEnabled ?? (_thumbsEnabled = new CapWrapper<BoolType>(this, CapabilityId.CapThumbnailsEnabled, ValueExtensions.ConvertToEnum<BoolType>, value =>
                        new TWCapability(CapabilityId.CapThumbnailsEnabled, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        })));
            }
        }

        private CapWrapper<BoolType> _dsUIonly;

        /// <summary>
        /// Gets the property to see whether device supports UI only flag (no transfer).
        /// </summary>
        /// <value>
        /// The UI only flag.
        /// </value>
        public CapWrapper<BoolType> CapEnableDSUIOnly
        {
            get
            {
                return _dsUIonly ?? (_dsUIonly = new CapWrapper<BoolType>(this, CapabilityId.CapEnableDSUIOnly, ValueExtensions.ConvertToEnum<BoolType>));
            }
        }

        private CapWrapper<BoolType> _dsData;

        /// <summary>
        /// Gets the property to see whether device supports custom data triplets.
        /// </summary>
        /// <value>
        /// The custom data flag.
        /// </value>
        public CapWrapper<BoolType> CapCustomDSData
        {
            get
            {
                return _dsData ?? (_dsData = new CapWrapper<BoolType>(this, CapabilityId.CapCustomDSData, ValueExtensions.ConvertToEnum<BoolType>));
            }
        }

        private CapWrapper<JobControl> _jobControl;

        /// <summary>
        /// Gets the property to work with job control option for the current source.
        /// </summary>
        /// <value>
        /// The job control option.
        /// </value>
        public CapWrapper<JobControl> CapJobControl
        {
            get
            {
                return _jobControl ?? (_jobControl = new CapWrapper<JobControl>(this, CapabilityId.CapJobControl, ValueExtensions.ConvertToEnum<JobControl>, value =>
                        new TWCapability(CapabilityId.CapJobControl, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        })));
            }
        }

        private CapWrapper<int> _alarmVolume;

        /// <summary>
        /// Gets the property to work with alarm volume for the current source.
        /// </summary>
        /// <value>
        /// The alarm volume.
        /// </value>
        public CapWrapper<int> CapAlarmVolume
        {
            get
            {
                return _alarmVolume ?? (_alarmVolume = new CapWrapper<int>(this, CapabilityId.CapAlarmVolume, ValueExtensions.ConvertToEnum<int>, value =>
                        new TWCapability(CapabilityId.CapAlarmVolume, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        })));
            }
        }

        //private CapWrapper<int> _autoCapture;

        ///// <summary>
        ///// Gets the property to work with auto capture count for the current source.
        ///// </summary>
        ///// <value>
        ///// The auto capture count.
        ///// </value>
        //public CapWrapper<int> CapAutomaticCapture
        //{
        //    get
        //    {
        //        return _autoCapture ?? (_autoCapture = new CapWrapper<int>(this, CapabilityId.CapAutomaticCapture, ValueExtensions.ConvertToEnum<int>, value =>
        //                new TWCapability(CapabilityId.CapAutomaticCapture, new TWOneValue
        //                {
        //                    Item = (uint)value,
        //                    ItemType = ItemType.Int32
        //                })));
        //    }
        //}


        #endregion

        #endregion

    }
}
