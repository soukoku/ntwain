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
                    _imgXferMech = new CapabilityControl<XferMech>(this, CapabilityId.ICapXferMech, CapRoutines.EnumRoutine<XferMech>,
                        value => new TWCapability(CapabilityId.ICapXferMech, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _imgXferMech;
            }
        }


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
                    _audXferMech = new CapabilityControl<XferMech>(this, CapabilityId.ACapXferMech, CapRoutines.EnumRoutine<XferMech>,
                        value => new TWCapability(CapabilityId.ACapXferMech, new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
                }
                return _audXferMech;
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
                    _compression = new CapabilityControl<CompressionType>(this, CapabilityId.ICapCompression, CapRoutines.EnumRoutine<CompressionType>,
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
                    _fileFormat = new CapabilityControl<FileFormat>(this, CapabilityId.ICapImageFileFormat, CapRoutines.EnumRoutine<FileFormat>,
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
                    _pixelType = new CapabilityControl<PixelType>(this, CapabilityId.ICapPixelType, CapRoutines.EnumRoutine<PixelType>,
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
                    _supportSize = new CapabilityControl<SupportedSize>(this, CapabilityId.ICapSupportedSizes, CapRoutines.EnumRoutine<SupportedSize>,
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
        /// The image supported size.
        /// </value>
        public CapabilityControl<BoolType> CapImageAutoDeskew
        {
            get
            {
                if (_autoDeskew == null)
                {
                    _autoDeskew = new CapabilityControl<BoolType>(this, CapabilityId.ICapAutomaticDeskew, CapRoutines.EnumRoutine<BoolType>, null);
                }
                return _autoDeskew;
            }
        }

        #endregion

        #region dpi

        /// <summary>
        /// Gets the supported DPI values for the current source.
        /// Only call this at state 4 or higher.
        /// </summary>
        /// <returns></returns>
        public IList<TWFix32> CapGetDPIs()
        {
            var list = CapGet(CapabilityId.ICapXResolution);
            return list.Select(o => o.ConvertToFix32()).ToList();
        }

        /// <summary>
        /// Change the DPI value for the current source.
        /// </summary>
        /// <param name="dpi">The DPI.</param>
        /// <returns></returns>
        public ReturnCode CapSetDPI(TWFix32 dpi)
        {
            return CapSetDPI(dpi, dpi);
        }

        /// <summary>
        /// Change the DPI value for the current source.
        /// </summary>
        /// <param name="xDPI">The x DPI.</param>
        /// <param name="yDPI">The y DPI.</param>
        /// <returns></returns>
        public ReturnCode CapSetDPI(TWFix32 xDPI, TWFix32 yDPI)
        {
            TWOneValue one = new TWOneValue();
            one.Item = (uint)xDPI;// ((uint)dpi) << 16;
            one.ItemType = ItemType.Fix32;

            using (TWCapability xres = new TWCapability(CapabilityId.ICapXResolution, one))
            {
                var rc = _session.DGControl.Capability.Set(xres);
                if (rc == ReturnCode.Success)
                {
                    one.Item = (uint)yDPI;
                    using (TWCapability yres = new TWCapability(CapabilityId.ICapYResolution, one))
                    {
                        rc = _session.DGControl.Capability.Set(yres);
                    }
                }
                return rc;
            }
        }

        #endregion

        #region onesie flags

        /// <summary>
        /// Change the auto deskew flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetAutoDeskew(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.ICapAutomaticDeskew))
            {

                if (Identity.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticDeskew, en))
                    {
                        rc = _session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticDeskew, one))
                    {
                        rc = _session.DGControl.Capability.Set(capValue);
                    }
                }
            }

            return rc;
        }

        /// <summary>
        /// Change the auto rotate flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetAutoRotate(bool useIt)
        {
            var rc = ReturnCode.Failure;
            if (SupportedCaps.Contains(CapabilityId.ICapAutomaticRotate))
            {
                if (Identity.ProtocolMajor >= 2)
                {
                    // if using twain 2.0 will need to use enum instead of onevalue (yuck)
                    TWEnumeration en = new TWEnumeration();
                    en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                    en.ItemType = ItemType.Bool;

                    using (TWCapability dx = new TWCapability(CapabilityId.ICapAutomaticRotate, en))
                    {
                        rc = _session.DGControl.Capability.Set(dx);
                    }
                }
                else
                {
                    TWOneValue one = new TWOneValue();
                    one.Item = (uint)(useIt ? 1 : 0);
                    one.ItemType = ItemType.Bool;

                    using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticRotate, one))
                    {
                        rc = _session.DGControl.Capability.Set(capValue);
                    }
                }
            }
            return rc;
        }

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
        /// Change the duplex flag for the current source.
        /// </summary>
        /// <param name="useIt">if set to <c>true</c> to use it.</param>
        /// <returns></returns>
        public ReturnCode CapSetDuplex(bool useIt)
        {
            if (Identity.ProtocolMajor >= 2)
            {
                // twain 2 likes to use enum :(

                TWEnumeration en = new TWEnumeration();
                en.ItemList = new object[] { (uint)(useIt ? 1 : 0) };
                en.ItemType = ItemType.Bool;

                using (TWCapability dx = new TWCapability(CapabilityId.CapDuplexEnabled, en))
                {
                    return _session.DGControl.Capability.Set(dx);
                }
            }
            else
            {
                TWOneValue one = new TWOneValue();
                one.Item = (uint)(useIt ? 1 : 0);
                one.ItemType = ItemType.Bool;

                using (TWCapability dx = new TWCapability(CapabilityId.CapDuplexEnabled, one))
                {
                    return _session.DGControl.Capability.Set(dx);
                }
            }
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
