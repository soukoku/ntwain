using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    // this contains all cap-related methods prefixed with Cap


    partial class DataSource
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
        public ReturnCode ResetAll(CapabilityId capabilityId)
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
        public ReturnCode Reset(CapabilityId capabilityId)
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

        #region high-level caps

        #region audio caps

        private CapabilityControl<XferMech> _audXferMech;

        /// <summary>
        /// Gets the property to work with audio <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The audio xfer mech.
        /// </value>
        public CapabilityControl<XferMech> CapAudioXferMech
        {
            get
            {
                if (_audXferMech == null)
                {
                    _audXferMech = new CapabilityControl<XferMech>(this, CapabilityId.ACapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
                        value => new TWCapability(CapabilityId.ACapXferMech, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _audXferMech;
            }
        }

        #endregion

        #region img caps

        private CapabilityControl<XferMech> _imgXferMech;

        /// <summary>
        /// Gets the property to work with image <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The image xfer mech.
        /// </value>
        public CapabilityControl<XferMech> CapImageXferMech
        {
            get
            {
                if (_imgXferMech == null)
                {
                    _imgXferMech = new CapabilityControl<XferMech>(this, CapabilityId.ICapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
                        value => new TWCapability(CapabilityId.ICapXferMech, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _imgXferMech;
            }
        }


        private CapabilityControl<CompressionType> _compression;

        /// <summary>
        /// Gets the property to work with image <see cref="CompressionType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image compression.
        /// </value>
        public CapabilityControl<CompressionType> CapImageCompression
        {
            get
            {
                if (_compression == null)
                {
                    _compression = new CapabilityControl<CompressionType>(this, CapabilityId.ICapCompression, ValueExtensions.ConvertToEnum<CompressionType>,
                        value => new TWCapability(CapabilityId.ICapCompression, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _compression;
            }
        }


        private CapabilityControl<FileFormat> _fileFormat;

        /// <summary>
        /// Gets the property to work with image <see cref="FileFormat"/> for the current source.
        /// </summary>
        /// <value>
        /// The image file format.
        /// </value>
        public CapabilityControl<FileFormat> CapImageFileFormat
        {
            get
            {
                if (_fileFormat == null)
                {
                    _fileFormat = new CapabilityControl<FileFormat>(this, CapabilityId.ICapImageFileFormat, ValueExtensions.ConvertToEnum<FileFormat>,
                        value => new TWCapability(CapabilityId.ICapImageFileFormat, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _fileFormat;
            }
        }


        private CapabilityControl<PixelType> _pixelType;

        /// <summary>
        /// Gets the property to work with image <see cref="PixelType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel type.
        /// </value>
        public CapabilityControl<PixelType> CapImagePixelType
        {
            get
            {
                if (_pixelType == null)
                {
                    _pixelType = new CapabilityControl<PixelType>(this, CapabilityId.ICapPixelType, ValueExtensions.ConvertToEnum<PixelType>,
                        value => new TWCapability(CapabilityId.ICapPixelType, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _pixelType;
            }
        }


        private CapabilityControl<SupportedSize> _supportSize;

        /// <summary>
        /// Gets the property to work with image <see cref="SupportedSize"/> for the current source.
        /// </summary>
        /// <value>
        /// The image supported size.
        /// </value>
        public CapabilityControl<SupportedSize> CapImageSupportedSize
        {
            get
            {
                if (_supportSize == null)
                {
                    _supportSize = new CapabilityControl<SupportedSize>(this, CapabilityId.ICapSupportedSizes, ValueExtensions.ConvertToEnum<SupportedSize>,
                        value => new TWCapability(CapabilityId.ICapSupportedSizes, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _supportSize;
            }
        }


        private CapabilityControl<BoolType> _autoDeskew;

        /// <summary>
        /// Gets the property to work with image auto deskew flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto deskew flag.
        /// </value>
        public CapabilityControl<BoolType> CapImageAutoDeskew
        {
            get
            {
                if (_autoDeskew == null)
                {
                    _autoDeskew = new CapabilityControl<BoolType>(this, CapabilityId.ICapAutomaticDeskew, ValueExtensions.ConvertToEnum<BoolType>, value =>
                    {
                        if (Identity.ProtocolMajor >= 2)
                        {
                            // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                            TWEnumeration en = new TWEnumeration();
                            en.ItemList = new object[] { (uint)value };
                            en.ItemType = ItemType.Bool;

                            return new TWCapability(CapabilityId.ICapAutomaticDeskew, en);
                        }
                        else
                        {
                            TWOneValue one = new TWOneValue();
                            one.Item = (uint)value;
                            one.ItemType = ItemType.Bool;

                            return new TWCapability(CapabilityId.ICapAutomaticDeskew, one);
                        }
                    });
                }
                return _autoDeskew;
            }
        }


        private CapabilityControl<BoolType> _autoRotate;

        /// <summary>
        /// Gets the property to work with image auto rotate flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto rotate flag.
        /// </value>
        public CapabilityControl<BoolType> CapImageAutoRotate
        {
            get
            {
                if (_autoRotate == null)
                {
                    _autoRotate = new CapabilityControl<BoolType>(this, CapabilityId.ICapAutomaticRotate, ValueExtensions.ConvertToEnum<BoolType>, value =>
                    {
                        if (Identity.ProtocolMajor >= 2)
                        {
                            // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                            TWEnumeration en = new TWEnumeration();
                            en.ItemList = new object[] { (uint)value };
                            en.ItemType = ItemType.Bool;

                            return new TWCapability(CapabilityId.ICapAutomaticRotate, en);
                        }
                        else
                        {
                            TWOneValue one = new TWOneValue();
                            one.Item = (uint)value;
                            one.ItemType = ItemType.Bool;

                            return new TWCapability(CapabilityId.ICapAutomaticRotate, one);
                        }
                    });
                }
                return _autoRotate;
            }
        }


        private CapabilityControl<TWFix32> _xResolution;

        /// <summary>
        /// Gets the property to work with image horizontal resolution for the current source.
        /// </summary>
        /// <value>
        /// The image horizontal resolution.
        /// </value>
        public CapabilityControl<TWFix32> CapImageXResolution
        {
            get
            {
                if (_xResolution == null)
                {
                    _xResolution = new CapabilityControl<TWFix32>(this, CapabilityId.ICapXResolution, ValueExtensions.ConvertToFix32,
                        value => new TWCapability(CapabilityId.ICapXResolution, new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
                }
                return _xResolution;
            }
        }


        private CapabilityControl<TWFix32> _yResolution;

        /// <summary>
        /// Gets the property to work with image vertical resolution for the current source.
        /// </summary>
        /// <value>
        /// The image vertical resolution.
        /// </value>
        public CapabilityControl<TWFix32> CapImageYResolution
        {
            get
            {
                if (_yResolution == null)
                {
                    _yResolution = new CapabilityControl<TWFix32>(this, CapabilityId.ICapYResolution, ValueExtensions.ConvertToFix32,
                        value => new TWCapability(CapabilityId.ICapYResolution, new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
                }
                return _yResolution;
            }
        }

        #endregion

        #region other caps

        private CapabilityControl<BoolType> _duplexEnabled;

        /// <summary>
        /// Gets the property to work with duplex enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The duplex enabled flag.
        /// </value>
        public CapabilityControl<BoolType> CapDuplexEnabled
        {
            get
            {
                if (_duplexEnabled == null)
                {
                    _duplexEnabled = new CapabilityControl<BoolType>(this, CapabilityId.CapDuplexEnabled, ValueExtensions.ConvertToEnum<BoolType>, value =>
                    {
                        if (Identity.ProtocolMajor >= 2)
                        {
                            // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                            TWEnumeration en = new TWEnumeration();
                            en.ItemList = new object[] { (uint)value };
                            en.ItemType = ItemType.Bool;

                            return new TWCapability(CapabilityId.CapDuplexEnabled, en);
                        }
                        else
                        {
                            TWOneValue one = new TWOneValue();
                            one.Item = (uint)value;
                            one.ItemType = ItemType.Bool;

                            return new TWCapability(CapabilityId.CapDuplexEnabled, one);
                        }
                    });
                }
                return _duplexEnabled;
            }
        }

        #endregion

        #endregion

        #region onesie flags


        /// <summary>
        /// Change the auto border detection flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetBorderDetection(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.ICapAutomaticBorderDetection))
            {
                // this goes along with undefinedimagesize so that also
                // needs to be set
                if (Identity.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapUndefinedImageSize, en))
                    {
                        rc = _session.DGControl.Capability.Set(dx);
                    }
                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, en))
                    {
                        rc = _session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapUndefinedImageSize, one))
                    {
                        rc = _session.DGControl.Capability.Set(capValue);
                    }
                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, one))
                    {
                        rc = _session.DGControl.Capability.Set(capValue);
                    }
                }
            }
            return rc;
        }

        /// <summary>
        /// Change the use feeder flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetFeeder(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.CapFeederEnabled))
            {
                if (Identity.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (ushort)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;



                    // we will never set feeder off, only autofeed and autoscan
                    // but if it is to SET then enable feeder needs to be set first
                    if (useIt)
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapFeederEnabled, en))
                        {
                            rc = _session.DGControl.Capability.Set(dx);
                        }
                    }

                    // to really use feeder we must also set autofeed or autoscan, but only
                    // for one of them since setting autoscan also sets autofeed
                    if (SupportedCaps.Contains(CapabilityId.CapAutoScan))
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapAutoScan, en))
                        {
                            rc = _session.DGControl.Capability.Set(dx);
                        }
                    }
                    else if (SupportedCaps.Contains(CapabilityId.CapAutoFeed))
                    {
                        using (TWCapability dx = new TWCapability(CapabilityId.CapAutoFeed, en))
                        {
                            rc = _session.DGControl.Capability.Set(dx);
                        }
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    if (useIt)
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
                }
            }
            return rc;
        }

        #endregion
    }
}
