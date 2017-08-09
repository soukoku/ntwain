using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Exposes capabilities of a data source as properties.
    /// </summary>
    public class Capabilities : ICapabilities
    {
        IDataSource _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        public Capabilities(IDataSource source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            _source = source;
        }


        #region non-wrapped cap calls

        /// <summary>
        /// Gets the actual supported operations for a capability. This is not supported by all sources.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        public QuerySupports? QuerySupport(CapabilityId capabilityId)
        {
            QuerySupports? retVal = null;
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                cap.ContainerType = ContainerType.OneValue;
                var rc = _source.DGControl.Capability.QuerySupport(cap);
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
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        public object GetCurrent(CapabilityId capabilityId)
        {
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = _source.DGControl.Capability.GetCurrent(cap);
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
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        public object GetDefault(CapabilityId capabilityId)
        {
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = _source.DGControl.Capability.GetDefault(cap);
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
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        public IEnumerable<object> GetValues(CapabilityId capabilityId)
        {
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = _source.DGControl.Capability.Get(cap);
                if (rc == ReturnCode.Success)
                {
                    return CapabilityReader.ReadValue(cap).EnumerateCapValues();
                }
            }
            return Enumerable.Empty<object>();
        }

        /// <summary>
        /// Gets all the possible values of this capability without expanding.
        /// This may be required to work with large range values that cannot be safely enumerated
        /// with <see cref="GetValues"/>.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        public CapabilityReader GetValuesRaw(CapabilityId capabilityId)
        {
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = _source.DGControl.Capability.Get(cap);
                if (rc == ReturnCode.Success)
                {
                    return CapabilityReader.ReadValue(cap);
                }
            }
            return new CapabilityReader();
        }

        /// <summary>
        /// Resets all values and constraint to power-on defaults.
        /// </summary>
        /// <returns></returns>
        public ReturnCode ResetAll()
        {
            using (TWCapability cap = new TWCapability(CapabilityId.CapSupportedCaps))
            {
                var rc = _source.DGControl.Capability.ResetAll(cap);
                return rc;
            }
        }

        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        public ReturnCode Reset(CapabilityId capabilityId)
        {
            using (TWCapability cap = new TWCapability(capabilityId))
            {
                var rc = _source.DGControl.Capability.Reset(cap);
                return rc;
            }
        }

        #endregion

        #region audio caps

        private CapWrapper<XferMech> _audXferMech;

        /// <summary>
        /// Gets the property to work with audio <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The audio xfer mech.
        /// </value>
        public ICapWrapper<XferMech> ACapXferMech
        {
            get
            {
                return _audXferMech ?? (_audXferMech = new CapWrapper<XferMech>(_source, CapabilityId.ACapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        #endregion

        #region img caps

        #region mandatory

        private CapWrapper<CompressionType> _compression;

        /// <summary>
        /// Gets the property to work with image <see cref="CompressionType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image compression.
        /// </value>
        public ICapWrapper<CompressionType> ICapCompression
        {
            get
            {
                return _compression ?? (_compression = new CapWrapper<CompressionType>(_source, CapabilityId.ICapCompression, ValueExtensions.ConvertToEnum<CompressionType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }


        private CapWrapper<PixelType> _pixelType;

        /// <summary>
        /// Gets the property to work with image <see cref="PixelType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel type.
        /// </value>
        public ICapWrapper<PixelType> ICapPixelType
        {
            get
            {
                return _pixelType ?? (_pixelType = new CapWrapper<PixelType>(_source, CapabilityId.ICapPixelType, ValueExtensions.ConvertToEnum<PixelType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<Unit> _imgUnits;

        /// <summary>
        /// Gets the property to work with image <see cref="Unit"/> for the current source.
        /// </summary>
        /// <value>
        /// The image unit of measure.
        /// </value>
        public ICapWrapper<Unit> ICapUnits
        {
            get
            {
                return _imgUnits ?? (_imgUnits = new CapWrapper<Unit>(_source, CapabilityId.ICapUnits, ValueExtensions.ConvertToEnum<Unit>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<XferMech> _imgXferMech;

        /// <summary>
        /// Gets the property to work with image <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The image xfer mech.
        /// </value>
        public ICapWrapper<XferMech> ICapXferMech
        {
            get
            {
                return _imgXferMech ?? (_imgXferMech = new CapWrapper<XferMech>(_source, CapabilityId.ICapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        #endregion

        private CapWrapper<BoolType> _autoBright;

        /// <summary>
        /// Gets the property to work with image auto brightness flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto brightness flag.
        /// </value>
        public ICapWrapper<BoolType> ICapAutoBright
        {
            get
            {
                return _autoBright ?? (_autoBright = new CapWrapper<BoolType>(_source, CapabilityId.ICapAutoBright, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<TWFix32> _brightness;

        /// <summary>
        /// Gets the property to work with image brightness for the current source.
        /// </summary>
        /// <value>
        /// The image brightness.
        /// </value>
        public ICapWrapper<TWFix32> ICapBrightness
        {
            get
            {
                return _brightness ?? (_brightness = new CapWrapper<TWFix32>(_source, CapabilityId.ICapBrightness, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<TWFix32> _contrast;

        /// <summary>
        /// Gets the property to work with image contrast for the current source.
        /// </summary>
        /// <value>
        /// The image contrast.
        /// </value>
        public ICapWrapper<TWFix32> ICapContrast
        {
            get
            {
                return _contrast ?? (_contrast = new CapWrapper<TWFix32>(_source, CapabilityId.ICapContrast, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<byte> _custHalftone;

        /// <summary>
        /// Gets the property to work with image square-cell halftone for the current source.
        /// </summary>
        /// <value>
        /// The image square-cell halftone.
        /// </value>
        public ICapWrapper<byte> ICapCustHalftone
        {
            get
            {
                return _custHalftone ?? (_custHalftone = new CapWrapper<byte>(_source, CapabilityId.ICapCustHalftone, ValueExtensions.ConvertToEnum<byte>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt8
                        }));
            }
        }

        private CapWrapper<TWFix32> _exposureTime;

        /// <summary>
        /// Gets the property to work with image exposure time (in seconds) for the current source.
        /// </summary>
        /// <value>
        /// The image exposure time.
        /// </value>
        public ICapWrapper<TWFix32> ICapExposureTime
        {
            get
            {
                return _exposureTime ?? (_exposureTime = new CapWrapper<TWFix32>(_source, CapabilityId.ICapExposureTime, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<FilterType> _filter;

        /// <summary>
        /// Gets the property to work with image color filter for the current source.
        /// </summary>
        /// <value>
        /// The image color filter type.
        /// </value>
        public ICapWrapper<FilterType> ICapFilter
        {
            get
            {
                return _filter ?? (_filter = new CapWrapper<FilterType>(_source, CapabilityId.ICapFilter, ValueExtensions.ConvertToEnum<FilterType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<TWFix32> _gamma;

        /// <summary>
        /// Gets the property to work with image gamma value for the current source.
        /// </summary>
        /// <value>
        /// The image gamma.
        /// </value>
        public ICapWrapper<TWFix32> ICapGamma
        {
            get
            {
                return _gamma ?? (_gamma = new CapWrapper<TWFix32>(_source, CapabilityId.ICapGamma, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<string> _halftones;

        /// <summary>
        /// Gets the property to work with image halftone patterns for the current source.
        /// </summary>
        /// <value>
        /// The image halftone patterns.
        /// </value>
        public ICapWrapper<string> ICapHalftones
        {
            get
            {
                return _halftones ?? (_halftones = new CapWrapper<string>(_source, CapabilityId.ICapHalftones, ValueExtensions.ConvertToString, false));
            }
        }

        private CapWrapper<TWFix32> _highlight;

        /// <summary>
        /// Gets the property to work with image highlight value for the current source.
        /// </summary>
        /// <value>
        /// The image highlight.
        /// </value>
        public ICapWrapper<TWFix32> ICapHighlight
        {
            get
            {
                return _highlight ?? (_highlight = new CapWrapper<TWFix32>(_source, CapabilityId.ICapHighlight, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<FileFormat> _fileFormat;

        /// <summary>
        /// Gets the property to work with image <see cref="FileFormat"/> for the current source.
        /// </summary>
        /// <value>
        /// The image file format.
        /// </value>
        public ICapWrapper<FileFormat> ICapImageFileFormat
        {
            get
            {
                return _fileFormat ?? (_fileFormat = new CapWrapper<FileFormat>(_source, CapabilityId.ICapImageFileFormat, ValueExtensions.ConvertToEnum<FileFormat>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }


        private CapWrapper<BoolType> _lampState;

        /// <summary>
        /// Gets the property to work with image lamp state flag for the current source.
        /// </summary>
        /// <value>
        /// The image lamp state flag.
        /// </value>
        public ICapWrapper<BoolType> ICapLampState
        {
            get
            {
                return _lampState ?? (_lampState = new CapWrapper<BoolType>(_source, CapabilityId.ICapLampState, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<LightSource> _lightSource;

        /// <summary>
        /// Gets the property to work with image light source for the current source.
        /// </summary>
        /// <value>
        /// The image light source.
        /// </value>
        public ICapWrapper<LightSource> ICapLightSource
        {
            get
            {
                return _lightSource ?? (_lightSource = new CapWrapper<LightSource>(_source, CapabilityId.ICapLightSource, ValueExtensions.ConvertToEnum<LightSource>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<OrientationType> _orientation;

        /// <summary>
        /// Gets the property to work with image orientation for the current source.
        /// </summary>
        /// <value>
        /// The image orientation.
        /// </value>
        public ICapWrapper<OrientationType> ICapOrientation
        {
            get
            {
                return _orientation ?? (_orientation = new CapWrapper<OrientationType>(_source, CapabilityId.ICapOrientation, ValueExtensions.ConvertToEnum<OrientationType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<TWFix32> _physicalWidth;

        /// <summary>
        /// Gets the property to work with image physical width for the current source.
        /// </summary>
        /// <value>
        /// The image physical width.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> ICapPhysicalWidth
        {
            get
            {
                return _physicalWidth ?? (_physicalWidth = new CapWrapper<TWFix32>(_source, CapabilityId.ICapPhysicalWidth, ValueExtensions.ConvertToFix32, true));
            }
        }

        private CapWrapper<TWFix32> _physicalHeight;

        /// <summary>
        /// Gets the property to work with image physical height for the current source.
        /// </summary>
        /// <value>
        /// The image physical height.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> ICapPhysicalHeight
        {
            get
            {
                return _physicalHeight ?? (_physicalHeight = new CapWrapper<TWFix32>(_source, CapabilityId.ICapPhysicalHeight, ValueExtensions.ConvertToFix32, true));
            }
        }

        private CapWrapper<TWFix32> _shadow;

        /// <summary>
        /// Gets the property to work with image shadow value for the current source.
        /// </summary>
        /// <value>
        /// The image shadow.
        /// </value>
        public ICapWrapper<TWFix32> ICapShadow
        {
            get
            {
                return _shadow ?? (_shadow = new CapWrapper<TWFix32>(_source, CapabilityId.ICapShadow, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<TWFrame> _frames;

        /// <summary>
        /// Gets the property to work with the list of frames the source will acquire on each page.
        /// </summary>
        /// <value>
        /// The capture frames.
        /// </value>
        public ICapWrapper<TWFrame> ICapFrames
        {
            get
            {
                return _frames ?? (_frames = new CapWrapper<TWFrame>(_source, CapabilityId.ICapFrames, ValueExtensions.ConvertToFrame,
                        value =>
                        {
                            using (var cap = new TWCapability(CapabilityId.ICapFrames, value))
                            {
                                return _source.DGControl.Capability.Set(cap);
                            }
                        }));
            }
        }

        private CapWrapper<TWFix32> _nativeXRes;

        /// <summary>
        /// Gets the property to work with image's native x-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image's native x-axis resolution.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> ICapXNativeResolution
        {
            get
            {
                return _nativeXRes ?? (_nativeXRes = new CapWrapper<TWFix32>(_source, CapabilityId.ICapXNativeResolution, ValueExtensions.ConvertToFix32, true));
            }
        }

        private CapWrapper<TWFix32> _nativeYRes;

        /// <summary>
        /// Gets the property to work with image's native y-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image's native y-axis resolution.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> ICapYNativeResolution
        {
            get
            {
                return _nativeYRes ?? (_nativeYRes = new CapWrapper<TWFix32>(_source, CapabilityId.ICapYNativeResolution, ValueExtensions.ConvertToFix32, true));
            }
        }

        private CapWrapper<TWFix32> _xResolution;

        /// <summary>
        /// Gets the property to work with image x-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image x-axis resolution.
        /// </value>
        public ICapWrapper<TWFix32> ICapXResolution
        {
            get
            {
                return _xResolution ?? (_xResolution = new CapWrapper<TWFix32>(_source, CapabilityId.ICapXResolution, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }


        private CapWrapper<TWFix32> _yResolution;

        /// <summary>
        /// Gets the property to work with image y-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image y-axis resolution.
        /// </value>
        public ICapWrapper<TWFix32> ICapYResolution
        {
            get
            {
                return _yResolution ?? (_yResolution = new CapWrapper<TWFix32>(_source, CapabilityId.ICapYResolution, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<int> _maxFrames;

        /// <summary>
        /// Gets the property to work with image max frames for the current source.
        /// </summary>
        /// <value>
        /// The image max frames.
        /// </value>
        public ICapWrapper<int> ICapMaxFrames
        {
            get
            {
                return _maxFrames ?? (_maxFrames = new CapWrapper<int>(_source, CapabilityId.ICapMaxFrames, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _tiles;

        /// <summary>
        /// Gets the property to work with image tiles flag for the current source.
        /// </summary>
        /// <value>
        /// The image tiles flag.
        /// </value>
        public ICapWrapper<BoolType> ICapTiles
        {
            get
            {
                return _tiles ?? (_tiles = new CapWrapper<BoolType>(_source, CapabilityId.ICapTiles, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BitOrder> _bitOrder;

        /// <summary>
        /// Gets the property to work with image <see cref="BitOrder"/> for the current source.
        /// </summary>
        /// <value>
        /// The image bit order.
        /// </value>
        public ICapWrapper<BitOrder> ICapBitOrder
        {
            get
            {
                return _bitOrder ?? (_bitOrder = new CapWrapper<BitOrder>(_source, CapabilityId.ICapBitOrder, ValueExtensions.ConvertToEnum<BitOrder>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<int> _ccittKFactor;

        /// <summary>
        /// Gets the property to work with image CCITT K factor for the current source.
        /// </summary>
        /// <value>
        /// The image CCITT K factor.
        /// </value>
        public ICapWrapper<int> ICapCCITTKFactor
        {
            get
            {
                return _ccittKFactor ?? (_ccittKFactor = new CapWrapper<int>(_source, CapabilityId.ICapCCITTKFactor, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<LightPath> _lightPath;

        /// <summary>
        /// Gets the property to work with image light path for the current source.
        /// </summary>
        /// <value>
        /// The image light path.
        /// </value>
        public ICapWrapper<LightPath> ICapLightPath
        {
            get
            {
                return _lightPath ?? (_lightPath = new CapWrapper<LightPath>(_source, CapabilityId.ICapLightPath, ValueExtensions.ConvertToEnum<LightPath>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<PixelFlavor> _pixelFlavor;

        /// <summary>
        /// Gets the property to work with image pixel flavor for the current source.
        /// </summary>
        /// <value>
        /// The image pixel flavor.
        /// </value>
        public ICapWrapper<PixelFlavor> ICapPixelFlavor
        {
            get
            {
                return _pixelFlavor ?? (_pixelFlavor = new CapWrapper<PixelFlavor>(_source, CapabilityId.ICapPixelFlavor, ValueExtensions.ConvertToEnum<PixelFlavor>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<PlanarChunky> _planarChunky;

        /// <summary>
        /// Gets the property to work with image color format for the current source.
        /// </summary>
        /// <value>
        /// The image color format.
        /// </value>
        public ICapWrapper<PlanarChunky> ICapPlanarChunky
        {
            get
            {
                return _planarChunky ?? (_planarChunky = new CapWrapper<PlanarChunky>(_source, CapabilityId.ICapPlanarChunky, ValueExtensions.ConvertToEnum<PlanarChunky>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<TWFix32> _rotation;

        /// <summary>
        /// Gets the property to work with image rotation for the current source.
        /// </summary>
        /// <value>
        /// The image rotation.
        /// </value>
        public ICapWrapper<TWFix32> ICapRotation
        {
            get
            {
                return _rotation ?? (_rotation = new CapWrapper<TWFix32>(_source, CapabilityId.ICapRotation, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<SupportedSize> _supportSize;

        /// <summary>
        /// Gets the property to work with image <see cref="SupportedSize"/> for the current source.
        /// </summary>
        /// <value>
        /// The image supported size.
        /// </value>
        public ICapWrapper<SupportedSize> ICapSupportedSizes
        {
            get
            {
                return _supportSize ?? (_supportSize = new CapWrapper<SupportedSize>(_source, CapabilityId.ICapSupportedSizes, ValueExtensions.ConvertToEnum<SupportedSize>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<TWFix32> _threshold;

        /// <summary>
        /// Gets the property to work with image threshold for the current source.
        /// </summary>
        /// <value>
        /// The image threshold.
        /// </value>
        public ICapWrapper<TWFix32> ICapThreshold
        {
            get
            {
                return _threshold ?? (_threshold = new CapWrapper<TWFix32>(_source, CapabilityId.ICapThreshold, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<TWFix32> _xscaling;

        /// <summary>
        /// Gets the property to work with image x-axis scaling for the current source.
        /// </summary>
        /// <value>
        /// The image x-axis scaling.
        /// </value>
        public ICapWrapper<TWFix32> ICapXScaling
        {
            get
            {
                return _xscaling ?? (_xscaling = new CapWrapper<TWFix32>(_source, CapabilityId.ICapXScaling, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<TWFix32> _yscaling;

        /// <summary>
        /// Gets the property to work with image y-axis scaling for the current source.
        /// </summary>
        /// <value>
        /// The image y-axis scaling.
        /// </value>
        public ICapWrapper<TWFix32> ICapYScaling
        {
            get
            {
                return _yscaling ?? (_yscaling = new CapWrapper<TWFix32>(_source, CapabilityId.ICapYScaling, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<BitOrder> _bitorderCodes;

        /// <summary>
        /// Gets the property to work with image CCITT compression <see cref="BitOrder"/> for the current source.
        /// </summary>
        /// <value>
        /// The image bit order for CCITT compression.
        /// </value>
        public ICapWrapper<BitOrder> ICapBitOrderCodes
        {
            get
            {
                return _bitorderCodes ?? (_bitorderCodes = new CapWrapper<BitOrder>(_source, CapabilityId.ICapBitOrderCodes, ValueExtensions.ConvertToEnum<BitOrder>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<PixelFlavor> _pixelFlavorCodes;

        /// <summary>
        /// Gets the property to work with image CCITT compression <see cref="PixelFlavor"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel flavor for CCITT compression.
        /// </value>
        public ICapWrapper<PixelFlavor> ICapPixelFlavorCodes
        {
            get
            {
                return _pixelFlavorCodes ?? (_pixelFlavorCodes = new CapWrapper<PixelFlavor>(_source, CapabilityId.ICapPixelFlavorCodes, ValueExtensions.ConvertToEnum<PixelFlavor>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<PixelType> _jpegPixelType;

        /// <summary>
        /// Gets the property to work with image jpeg compression <see cref="PixelType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel type for jpeg compression.
        /// </value>
        public ICapWrapper<PixelType> ICapJpegPixelType
        {
            get
            {
                return _jpegPixelType ?? (_jpegPixelType = new CapWrapper<PixelType>(_source, CapabilityId.ICapJpegPixelType, ValueExtensions.ConvertToEnum<PixelType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<int> _timeFill;

        /// <summary>
        /// Gets the property to work with image CCITT time fill for the current source.
        /// </summary>
        /// <value>
        /// The image CCITT time fill.
        /// </value>
        public ICapWrapper<int> ICapTimeFill
        {
            get
            {
                return _timeFill ?? (_timeFill = new CapWrapper<int>(_source, CapabilityId.ICapTimeFill, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<int> _bitDepth;

        /// <summary>
        /// Gets the property to work with image bit depth for the current source.
        /// </summary>
        /// <value>
        /// The image bit depth.
        /// </value>
        public ICapWrapper<int> ICapBitDepth
        {
            get
            {
                return _bitDepth ?? (_bitDepth = new CapWrapper<int>(_source, CapabilityId.ICapBitDepth, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BitDepthReduction> _bitDepthReduction;

        /// <summary>
        /// Gets the property to work with image bit depth reduction method for the current source.
        /// </summary>
        /// <value>
        /// The image bit depth reduction method.
        /// </value>
        public ICapWrapper<BitDepthReduction> ICapBitDepthReduction
        {
            get
            {
                return _bitDepthReduction ?? (_bitDepthReduction = new CapWrapper<BitDepthReduction>(_source, CapabilityId.ICapBitDepthReduction, ValueExtensions.ConvertToEnum<BitDepthReduction>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _undefinedImgSize;

        /// <summary>
        /// Gets the property to work with image undefined size flag for the current source.
        /// </summary>
        /// <value>
        /// The image undefined size flag.
        /// </value>
        public ICapWrapper<BoolType> ICapUndefinedImageSize
        {
            get
            {
                return _undefinedImgSize ?? (_undefinedImgSize = new CapWrapper<BoolType>(_source, CapabilityId.ICapUndefinedImageSize, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<uint> _imgDataSet;

        /// <summary>
        /// Gets or sets the image indices that will be delivered during the standard image transfer done in
        /// States 6 and 7.
        /// </summary>
        /// <value>
        /// The image indicies.
        /// </value>
        public ICapWrapper<uint> ICapImageDataSet
        {
            get
            {
                return _imgDataSet ?? (_imgDataSet = new CapWrapper<uint>(_source, CapabilityId.ICapImageDataSet, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<BoolType> _extImgInfo;

        /// <summary>
        /// Gets the property to work with ext image info flag for the current source.
        /// </summary>
        /// <value>
        /// The ext image info flag.
        /// </value>
        public ICapWrapper<BoolType> ICapExtImageInfo
        {
            get
            {
                return _extImgInfo ?? (_extImgInfo = new CapWrapper<BoolType>(_source, CapabilityId.ICapExtImageInfo, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<TWFix32> _minHeight;

        /// <summary>
        /// Gets the property to work with image minimum height for the current source.
        /// </summary>
        /// <value>
        /// The image minimumm height.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> ICapMinimumHeight
        {
            get
            {
                return _minHeight ?? (_minHeight = new CapWrapper<TWFix32>(_source, CapabilityId.ICapMinimumHeight, ValueExtensions.ConvertToFix32, true));
            }
        }

        private CapWrapper<TWFix32> _minWidth;

        /// <summary>
        /// Gets the property to work with image minimum width for the current source.
        /// </summary>
        /// <value>
        /// The image minimumm width.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> ICapMinimumWidth
        {
            get
            {
                return _minWidth ?? (_minWidth = new CapWrapper<TWFix32>(_source, CapabilityId.ICapMinimumWidth, ValueExtensions.ConvertToFix32, true));
            }
        }

        private CapWrapper<BlankPage> _autoDiscBlankPg;

        /// <summary>
        /// Gets the property to work with image blank page behavior for the current source.
        /// </summary>
        /// <value>
        /// The image blank page behavior.
        /// </value>
        public ICapWrapper<BlankPage> ICapAutoDiscardBlankPages
        {
            get
            {
                return _autoDiscBlankPg ?? (_autoDiscBlankPg = new CapWrapper<BlankPage>(_source, CapabilityId.ICapAutoDiscardBlankPages, ValueExtensions.ConvertToEnum<BlankPage>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        private CapWrapper<FlipRotation> _flipRotation;

        /// <summary>
        /// Gets the property to work with image flip-rotation behavior for the current source.
        /// </summary>
        /// <value>
        /// The image flip-rotation behavior.
        /// </value>
        public ICapWrapper<FlipRotation> ICapFlipRotation
        {
            get
            {
                return _flipRotation ?? (_flipRotation = new CapWrapper<FlipRotation>(_source, CapabilityId.ICapFlipRotation, ValueExtensions.ConvertToEnum<FlipRotation>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        private CapWrapper<BoolType> _barcodeDetectEnabled;

        /// <summary>
        /// Gets the property to work with image barcode detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image barcode detection flag.
        /// </value>
        public ICapWrapper<BoolType> ICapBarcodeDetectionEnabled
        {
            get
            {
                return _barcodeDetectEnabled ?? (_barcodeDetectEnabled = new CapWrapper<BoolType>(_source, CapabilityId.ICapBarcodeDetectionEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BarcodeType> _barcodeType;

        /// <summary>
        /// Gets the property to work with image barcode types for the current source.
        /// </summary>
        /// <value>
        /// The image barcode types.
        /// </value>
        public IReadOnlyCapWrapper<BarcodeType> ICapSupportedBarcodeTypes
        {
            get
            {
                return _barcodeType ?? (_barcodeType = new CapWrapper<BarcodeType>(_source, CapabilityId.ICapSupportedBarcodeTypes, ValueExtensions.ConvertToEnum<BarcodeType>, true));
            }
        }

        private CapWrapper<uint> _barcodeMaxPriority;

        /// <summary>
        /// Gets the property to work with image barcode max search priorities for the current source.
        /// </summary>
        /// <value>
        /// The image barcode max search priorities.
        /// </value>
        public ICapWrapper<uint> ICapBarcodeMaxSearchPriorities
        {
            get
            {
                return _barcodeMaxPriority ?? (_barcodeMaxPriority = new CapWrapper<uint>(_source, CapabilityId.ICapBarcodeMaxSearchPriorities, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<BarcodeType> _barcodeSearchPriority;

        /// <summary>
        /// Gets the property to work with image barcode search priority for the current source.
        /// </summary>
        /// <value>
        /// The image barcode search priority.
        /// </value>
        public ICapWrapper<BarcodeType> ICapBarcodeSearchPriorities
        {
            get
            {
                return _barcodeSearchPriority ?? (_barcodeSearchPriority = new CapWrapper<BarcodeType>(_source, CapabilityId.ICapBarcodeSearchPriorities, ValueExtensions.ConvertToEnum<BarcodeType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<BarcodeDirection> _barcodeSearchMode;

        /// <summary>
        /// Gets the property to work with image barcode search direction for the current source.
        /// </summary>
        /// <value>
        /// The image barcode search direction.
        /// </value>
        public ICapWrapper<BarcodeDirection> ICapBarcodeSearchMode
        {
            get
            {
                return _barcodeSearchMode ?? (_barcodeSearchMode = new CapWrapper<BarcodeDirection>(_source, CapabilityId.ICapBarcodeSearchMode, ValueExtensions.ConvertToEnum<BarcodeDirection>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<uint> _barcodeMaxRetries;

        /// <summary>
        /// Gets the property to work with image barcode max search retries for the current source.
        /// </summary>
        /// <value>
        /// The image barcode max search retries.
        /// </value>
        public ICapWrapper<uint> ICapBarcodeMaxRetries
        {
            get
            {
                return _barcodeMaxRetries ?? (_barcodeMaxRetries = new CapWrapper<uint>(_source, CapabilityId.ICapBarcodeMaxRetries, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<uint> _barcodeTimeout;

        /// <summary>
        /// Gets the property to work with image barcode max search timeout for the current source.
        /// </summary>
        /// <value>
        /// The image barcode max search timeout.
        /// </value>
        public ICapWrapper<uint> ICapBarcodeTimeout
        {
            get
            {
                return _barcodeTimeout ?? (_barcodeTimeout = new CapWrapper<uint>(_source, CapabilityId.ICapBarcodeTimeout, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<int> _zoomFactor;

        /// <summary>
        /// Gets the property to work with image zoom factor for the current source.
        /// </summary>
        /// <value>
        /// The image zoom factor.
        /// </value>
        public ICapWrapper<int> ICapZoomFactor
        {
            get
            {
                return _zoomFactor ?? (_zoomFactor = new CapWrapper<int>(_source, CapabilityId.ICapZoomFactor, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _patchcodeDetectEnabled;

        /// <summary>
        /// Gets the property to work with image patch code detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image patch code detection flag.
        /// </value>
        public ICapWrapper<BoolType> ICapPatchCodeDetectionEnabled
        {
            get
            {
                return _patchcodeDetectEnabled ?? (_patchcodeDetectEnabled = new CapWrapper<BoolType>(_source, CapabilityId.ICapPatchCodeDetectionEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<PatchCode> _patchcodeType;

        /// <summary>
        /// Gets the property to work with image patch code types for the current source.
        /// </summary>
        /// <value>
        /// The image patch code types.
        /// </value>
        public IReadOnlyCapWrapper<PatchCode> ICapSupportedPatchCodeTypes
        {
            get
            {
                return _patchcodeType ?? (_patchcodeType = new CapWrapper<PatchCode>(_source, CapabilityId.ICapSupportedPatchCodeTypes, ValueExtensions.ConvertToEnum<PatchCode>, true));
            }
        }

        private CapWrapper<uint> _patchcodeMaxPriority;

        /// <summary>
        /// Gets the property to work with image patch code max search priorities for the current source.
        /// </summary>
        /// <value>
        /// The image patch code max search priorities.
        /// </value>
        public ICapWrapper<uint> ICapPatchCodeMaxSearchPriorities
        {
            get
            {
                return _patchcodeMaxPriority ?? (_patchcodeMaxPriority = new CapWrapper<uint>(_source, CapabilityId.ICapPatchCodeMaxSearchPriorities, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<PatchCode> _patchcodeSearchPriority;

        /// <summary>
        /// Gets the property to work with image patch code search priority for the current source.
        /// </summary>
        /// <value>
        /// The image patch code search priority.
        /// </value>
        public ICapWrapper<PatchCode> ICapPatchCodeSearchPriorities
        {
            get
            {
                return _patchcodeSearchPriority ?? (_patchcodeSearchPriority = new CapWrapper<PatchCode>(_source, CapabilityId.ICapPatchCodeSearchPriorities, ValueExtensions.ConvertToEnum<PatchCode>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<BarcodeDirection> _patchcodeSearchMode;

        /// <summary>
        /// Gets the property to work with image patch code search direction for the current source.
        /// </summary>
        /// <value>
        /// The image patch code search direction.
        /// </value>
        public ICapWrapper<BarcodeDirection> ICapPatchCodeSearchMode
        {
            get
            {
                return _patchcodeSearchMode ?? (_patchcodeSearchMode = new CapWrapper<BarcodeDirection>(_source, CapabilityId.ICapPatchCodeSearchMode, ValueExtensions.ConvertToEnum<BarcodeDirection>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<uint> _patchCodeMaxRetries;

        /// <summary>
        /// Gets the property to work with image patch code max search retries for the current source.
        /// </summary>
        /// <value>
        /// The image patch code max search retries.
        /// </value>
        public ICapWrapper<uint> ICapPatchCodeMaxRetries
        {
            get
            {
                return _patchCodeMaxRetries ?? (_patchCodeMaxRetries = new CapWrapper<uint>(_source, CapabilityId.ICapPatchCodeMaxRetries, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<uint> _patchCodeTimeout;

        /// <summary>
        /// Gets the property to work with image patch code max search timeout for the current source.
        /// </summary>
        /// <value>
        /// The image patch code max search timeout.
        /// </value>
        public ICapWrapper<uint> ICapPatchCodeTimeout
        {
            get
            {
                return _patchCodeTimeout ?? (_patchCodeTimeout = new CapWrapper<uint>(_source, CapabilityId.ICapPatchCodeTimeout, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<FlashedUsed> _flashUsed2;

        /// <summary>
        /// Gets the property to work with flash option for the current source.
        /// </summary>
        /// <value>
        /// The flash option.
        /// </value>
        public ICapWrapper<FlashedUsed> ICapFlashUsed2
        {
            get
            {
                return _flashUsed2 ?? (_flashUsed2 = new CapWrapper<FlashedUsed>(_source, CapabilityId.ICapFlashUsed2, ValueExtensions.ConvertToEnum<FlashedUsed>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<ImageFilter> _imgFilter;

        /// <summary>
        /// Gets the property to work with image enhancement filter for the current source.
        /// </summary>
        /// <value>
        /// The image enhancement filter.
        /// </value>
        public ICapWrapper<ImageFilter> ICapImageFilter
        {
            get
            {
                return _imgFilter ?? (_imgFilter = new CapWrapper<ImageFilter>(_source, CapabilityId.ICapImageFilter, ValueExtensions.ConvertToEnum<ImageFilter>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<NoiseFilter> _noiseFilter;

        /// <summary>
        /// Gets the property to work with image noise filter for the current source.
        /// </summary>
        /// <value>
        /// The image noise filter.
        /// </value>
        public ICapWrapper<NoiseFilter> ICapNoiseFilter
        {
            get
            {
                return _noiseFilter ?? (_noiseFilter = new CapWrapper<NoiseFilter>(_source, CapabilityId.ICapNoiseFilter, ValueExtensions.ConvertToEnum<NoiseFilter>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<OverScan> _overscan;

        /// <summary>
        /// Gets the property to work with image overscan option for the current source.
        /// </summary>
        /// <value>
        /// The image overscan option.
        /// </value>
        public ICapWrapper<OverScan> ICapOverScan
        {
            get
            {
                return _overscan ?? (_overscan = new CapWrapper<OverScan>(_source, CapabilityId.ICapOverScan, ValueExtensions.ConvertToEnum<OverScan>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _borderDetect;

        /// <summary>
        /// Gets the property to work with auto border detection flag for the current source.
        /// </summary>
        /// <value>
        /// The auto border detection flag.
        /// </value>
        public ICapWrapper<BoolType> ICapAutomaticBorderDetection
        {
            get
            {
                return _borderDetect ?? (_borderDetect = new CapWrapper<BoolType>(_source, CapabilityId.ICapAutomaticBorderDetection, ValueExtensions.ConvertToEnum<BoolType>,
                    value =>
                    {
                        var rc = ReturnCode.Failure;

                        var one = new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        };

                        // this needs to also set undefined size optino
                        rc = ICapUndefinedImageSize.SetValue(value);
                        using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, one))
                        {
                            rc = _source.DGControl.Capability.Set(capValue);
                        }

                        return rc;
                    }));
            }
        }

        private CapWrapper<BoolType> _autoDeskew;

        /// <summary>
        /// Gets the property to work with image auto deskew flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto deskew flag.
        /// </value>
        public ICapWrapper<BoolType> ICapAutomaticDeskew
        {
            get
            {
                return _autoDeskew ?? (_autoDeskew = new CapWrapper<BoolType>(_source, CapabilityId.ICapAutomaticDeskew, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BoolType> _autoRotate;

        /// <summary>
        /// Gets the property to work with image auto rotate flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto rotate flag.
        /// </value>
        public ICapWrapper<BoolType> ICapAutomaticRotate
        {
            get
            {
                return _autoRotate ?? (_autoRotate = new CapWrapper<BoolType>(_source, CapabilityId.ICapAutomaticRotate, ValueExtensions.ConvertToEnum<BoolType>,
                         value => new TWOneValue
                         {
                             Item = (uint)value,
                             ItemType = ItemType.Bool
                         }));
            }
        }

        private CapWrapper<JpegQuality> _jpegQuality;

        /// <summary>
        /// Gets the property to work with image jpeg quality for the current source.
        /// </summary>
        /// <value>
        /// The image jpeg quality.
        /// </value>
        public ICapWrapper<JpegQuality> ICapJpegQuality
        {
            get
            {
                //TODO: verify
                return _jpegQuality ?? (_jpegQuality = new CapWrapper<JpegQuality>(_source, CapabilityId.ICapJpegQuality, ValueExtensions.ConvertToEnum<JpegQuality>,
                         value => new TWOneValue
                         {
                             Item = (uint)value,
                             ItemType = ItemType.Int16
                         }));
            }
        }

        private CapWrapper<FeederType> _feederType;

        /// <summary>
        /// Gets the property to work with feeder type for the current source.
        /// </summary>
        /// <value>
        /// The feeder type.
        /// </value>
        public ICapWrapper<FeederType> ICapFeederType
        {
            get
            {
                return _feederType ?? (_feederType = new CapWrapper<FeederType>(_source, CapabilityId.ICapFeederType, ValueExtensions.ConvertToEnum<FeederType>,
                         value => new TWOneValue
                         {
                             Item = (uint)value,
                             ItemType = ItemType.UInt16
                         }));
            }
        }

        private CapWrapper<IccProfile> _iccProfile;

        /// <summary>
        /// Gets the property to work with image icc profile for the current source.
        /// </summary>
        /// <value>
        /// The image icc profile.
        /// </value>
        public ICapWrapper<IccProfile> ICapICCProfile
        {
            get
            {
                return _iccProfile ?? (_iccProfile = new CapWrapper<IccProfile>(_source, CapabilityId.ICapICCProfile, ValueExtensions.ConvertToEnum<IccProfile>,
                         value => new TWOneValue
                         {
                             Item = (uint)value,
                             ItemType = ItemType.UInt16
                         }));
            }
        }

        private CapWrapper<AutoSize> _autoSize;

        /// <summary>
        /// Gets the property to work with image auto size option for the current source.
        /// </summary>
        /// <value>
        /// The image auto size option.
        /// </value>
        public ICapWrapper<AutoSize> ICapAutoSize
        {
            get
            {
                return _autoSize ?? (_autoSize = new CapWrapper<AutoSize>(_source, CapabilityId.ICapAutoSize, ValueExtensions.ConvertToEnum<AutoSize>,
                         value => new TWOneValue
                         {
                             Item = (uint)value,
                             ItemType = ItemType.UInt16
                         }));
            }
        }

        private CapWrapper<BoolType> _cropUseFrame;

        /// <summary>
        /// Gets the property to work with image auto crop flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto crop flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> ICapAutomaticCropUsesFrame
        {
            get
            {
                return _cropUseFrame ?? (_cropUseFrame = new CapWrapper<BoolType>(_source, CapabilityId.ICapAutomaticCropUsesFrame, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<BoolType> _lengthDetect;

        /// <summary>
        /// Gets the property to work with image auto length detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto length detection flag.
        /// </value>
        public ICapWrapper<BoolType> ICapAutomaticLengthDetection
        {
            get
            {
                return _lengthDetect ?? (_lengthDetect = new CapWrapper<BoolType>(_source, CapabilityId.ICapAutomaticLengthDetection, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<BoolType> _autoColor;

        /// <summary>
        /// Gets the property to work with image auto color detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto color detection flag.
        /// </value>
        public ICapWrapper<BoolType> ICapAutomaticColorEnabled
        {
            get
            {
                return _autoColor ?? (_autoColor = new CapWrapper<BoolType>(_source, CapabilityId.ICapAutomaticColorEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<PixelType> _autoColorNonPixel;

        /// <summary>
        /// Gets the property to work with image auto non-color pixel type for the current source.
        /// </summary>
        /// <value>
        /// The image auto non-color pixel type.
        /// </value>
        public ICapWrapper<PixelType> ICapAutomaticColorNonColorPixelType
        {
            get
            {
                return _autoColorNonPixel ?? (_autoColorNonPixel = new CapWrapper<PixelType>(_source, CapabilityId.ICapAutomaticColorNonColorPixelType, ValueExtensions.ConvertToEnum<PixelType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<BoolType> _colorMgmt;

        /// <summary>
        /// Gets the property to work with image color management flag for the current source.
        /// </summary>
        /// <value>
        /// The image color management flag.
        /// </value>
        public ICapWrapper<BoolType> ICapColorManagementEnabled
        {
            get
            {
                return _colorMgmt ?? (_colorMgmt = new CapWrapper<BoolType>(_source, CapabilityId.ICapColorManagementEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<ImageMerge> _imgMerge;

        /// <summary>
        /// Gets the property to work with image merge option for the current source.
        /// </summary>
        /// <value>
        /// The image merge option.
        /// </value>
        public ICapWrapper<ImageMerge> ICapImageMerge
        {
            get
            {
                return _imgMerge ?? (_imgMerge = new CapWrapper<ImageMerge>(_source, CapabilityId.ICapImageMerge, ValueExtensions.ConvertToEnum<ImageMerge>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<TWFix32> _mergeHeight;

        /// <summary>
        /// Gets the property to work with image merge height threshold for the current source.
        /// </summary>
        /// <value>
        /// The image merge height threshold.
        /// </value>
        public ICapWrapper<TWFix32> ICapImageMergeHeightThreshold
        {
            get
            {
                return _mergeHeight ?? (_mergeHeight = new CapWrapper<TWFix32>(_source, CapabilityId.ICapImageMergeHeightThreshold, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<ExtendedImageInfo> _supportedExtInfo;

        /// <summary>
        /// Gets the property to get supported ext image info for the current source.
        /// </summary>
        /// <value>
        /// The supported ext image info.
        /// </value>
        public IReadOnlyCapWrapper<ExtendedImageInfo> ICapSupportedExtImageInfo
        {
            get
            {
                return _supportedExtInfo ?? (_supportedExtInfo = new CapWrapper<ExtendedImageInfo>(_source, CapabilityId.ICapSupportedExtImageInfo, ValueExtensions.ConvertToEnum<ExtendedImageInfo>, true));
            }
        }

        private CapWrapper<FilmType> _filmType;

        /// <summary>
        /// Gets the property to work with image film type for the current source.
        /// </summary>
        /// <value>
        /// The image film type.
        /// </value>
        public ICapWrapper<FilmType> ICapFilmType
        {
            get
            {
                return _filmType ?? (_filmType = new CapWrapper<FilmType>(_source, CapabilityId.ICapFilmType, ValueExtensions.ConvertToEnum<FilmType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<Mirror> _mirror;

        /// <summary>
        /// Gets the property to work with image mirror option for the current source.
        /// </summary>
        /// <value>
        /// The image mirror option.
        /// </value>
        public ICapWrapper<Mirror> ICapMirror
        {
            get
            {
                return _mirror ?? (_mirror = new CapWrapper<Mirror>(_source, CapabilityId.ICapMirror, ValueExtensions.ConvertToEnum<Mirror>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<JpegSubsampling> _jpegSubSampling;

        /// <summary>
        /// Gets the property to work with image jpeg sub sampling for the current source.
        /// </summary>
        /// <value>
        /// The image jpeg sub sampling.
        /// </value>
        public ICapWrapper<JpegSubsampling> ICapJpegSubsampling
        {
            get
            {
                return _jpegSubSampling ?? (_jpegSubSampling = new CapWrapper<JpegSubsampling>(_source, CapabilityId.ICapJpegSubsampling, ValueExtensions.ConvertToEnum<JpegSubsampling>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        #endregion

        #region general caps

        #region mandatory

        private CapWrapper<int> _xferCount;

        /// <summary>
        /// Gets the property to work with xfer count for the current source.
        /// </summary>
        /// <value>
        /// The xfer count.
        /// </value>
        public ICapWrapper<int> CapXferCount
        {
            get
            {
                return _xferCount ?? (_xferCount = new CapWrapper<int>(_source, CapabilityId.CapXferCount, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = value > 0 ? (uint)value : uint.MaxValue,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        #endregion

        private CapWrapper<string> _author;

        /// <summary>
        /// Gets the property to work with the name or other identifying information about the 
        /// Author of the image. It may include a copyright string.
        /// </summary>
        /// <value>
        /// The author string.
        /// </value>
        public ICapWrapper<string> CapAuthor
        {
            get
            {
                return _author ?? (_author = new CapWrapper<string>(_source, CapabilityId.CapAuthor, ValueExtensions.ConvertToString,
                    value =>
                    {
                        using (var cap = new TWCapability(CapabilityId.CapAuthor, value, ItemType.String128))
                        {
                            return _source.DGControl.Capability.Set(cap);
                        }
                    }));
            }
        }


        private CapWrapper<string> _caption;

        /// <summary>
        /// Gets the property to work with the general note about the acquired image.
        /// </summary>
        /// <value>
        /// The general note string.
        /// </value>
        public ICapWrapper<string> CapCaption
        {
            get
            {
                return _caption ?? (_caption = new CapWrapper<string>(_source, CapabilityId.CapCaption, ValueExtensions.ConvertToString,
                    value =>
                    {
                        using (var cap = new TWCapability(CapabilityId.CapCaption, value, ItemType.String255))
                        {
                            return _source.DGControl.Capability.Set(cap);
                        }
                    }));
            }
        }

        private CapWrapper<BoolType> _feederEnabled;

        /// <summary>
        /// Gets the property to work with feeder enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The feeder enabled flag.
        /// </value>
        public ICapWrapper<BoolType> CapFeederEnabled
        {
            get
            {
                return _feederEnabled ?? (_feederEnabled = new CapWrapper<BoolType>(_source, CapabilityId.CapFeederEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                    value =>
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
                                rc = _source.DGControl.Capability.Set(enabled);
                            }
                        }
                        // to really use feeder we must also set autofeed or autoscan, but only
                        // for one of them since setting autoscan also sets autofeed
                        if (CapAutoScan.CanSet)
                        {
                            rc = CapAutoScan.SetValue(value);
                        }
                        else if (CapAutoFeed.CanSet)
                        {
                            rc = CapAutoFeed.SetValue(value);
                        }

                        return rc;
                    }));
            }
        }

        private CapWrapper<BoolType> _feederLoaded;

        /// <summary>
        /// Gets the property to work with feeder loaded flag for the current source.
        /// </summary>
        /// <value>
        /// The feeder loaded flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapFeederLoaded
        {
            get
            {
                return _feederLoaded ?? (_feederLoaded = new CapWrapper<BoolType>(_source, CapabilityId.CapFeederLoaded, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<string> _timedate;

        /// <summary>
        /// Gets the property to get the image acquired time and date.
        /// </summary>
        /// <value>
        /// The time and date string.
        /// </value>
        public IReadOnlyCapWrapper<string> CapTimeDate
        {
            get
            {
                return _timedate ?? (_timedate = new CapWrapper<string>(_source, CapabilityId.CapTimeDate, ValueExtensions.ConvertToString, true));
            }
        }

        private CapWrapper<CapabilityId> _supportedCaps;

        /// <summary>
        /// Gets the supported caps for the current source. This is not supported by all sources.
        /// </summary>
        /// <value>
        /// The supported caps.
        /// </value>
        public IReadOnlyCapWrapper<CapabilityId> CapSupportedCaps
        {
            get
            {
                return _supportedCaps ?? (_supportedCaps = new CapWrapper<CapabilityId>(_source, CapabilityId.CapSupportedCaps, value => value.ConvertToEnum<CapabilityId>(false), true));
            }
        }

        private CapWrapper<CapabilityId> _extendedCaps;

        /// <summary>
        /// Gets the extended caps for the current source.
        /// </summary>
        /// <value>
        /// The extended caps.
        /// </value>
        public ICapWrapper<CapabilityId> CapExtendedCaps
        {
            get
            {
                return _extendedCaps ?? (_extendedCaps = new CapWrapper<CapabilityId>(_source, CapabilityId.CapExtendedCaps, value => value.ConvertToEnum<CapabilityId>(false),
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.UInt16
                    }));
            }
        }

        private CapWrapper<BoolType> _autoFeed;

        /// <summary>
        /// Gets the property to work with auto feed page flag for the current source.
        /// </summary>
        /// <value>
        /// The auto feed flag.
        /// </value>
        public ICapWrapper<BoolType> CapAutoFeed
        {
            get
            {
                return _autoFeed ?? (_autoFeed = new CapWrapper<BoolType>(_source, CapabilityId.CapAutoFeed, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
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
        public ICapWrapper<BoolType> CapClearPage
        {
            get
            {
                return _clearPage ?? (_clearPage = new CapWrapper<BoolType>(_source, CapabilityId.CapClearPage, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BoolType> _feedPage;

        /// <summary>
        /// Gets the property to work with feed page flag for the current source.
        /// </summary>
        /// <value>
        /// The feed page flag.
        /// </value>
        public ICapWrapper<BoolType> CapFeedPage
        {
            get
            {
                return _feedPage ?? (_feedPage = new CapWrapper<BoolType>(_source, CapabilityId.CapFeedPage, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BoolType> _rewindPage;

        /// <summary>
        /// Gets the property to work with rewind page flag for the current source.
        /// </summary>
        /// <value>
        /// The rewind page flag.
        /// </value>
        public ICapWrapper<BoolType> CapRewindPage
        {
            get
            {
                return _rewindPage ?? (_rewindPage = new CapWrapper<BoolType>(_source, CapabilityId.CapRewindPage, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BoolType> _indicators;

        /// <summary>
        /// Gets the property to work with indicators flag for the current source.
        /// </summary>
        /// <value>
        /// The indicators flag.
        /// </value>
        public ICapWrapper<BoolType> CapIndicators
        {
            get
            {
                return _indicators ?? (_indicators = new CapWrapper<BoolType>(_source, CapabilityId.CapIndicators, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BoolType> _paperDetectable;

        /// <summary>
        /// Gets the property to work with paper sensor flag for the current source.
        /// </summary>
        /// <value>
        /// The paper sensor flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapPaperDetectable
        {
            get
            {
                return _paperDetectable ?? (_paperDetectable = new CapWrapper<BoolType>(_source, CapabilityId.CapPaperDetectable, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<BoolType> _uiControllable;

        /// <summary>
        /// Gets the property to work with UI controllable flag for the current source.
        /// </summary>
        /// <value>
        /// The UI controllable flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapUIControllable
        {
            get
            {
                return _uiControllable ?? (_uiControllable = new CapWrapper<BoolType>(_source, CapabilityId.CapUIControllable, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<BoolType> _devOnline;

        /// <summary>
        /// Gets the property to work with devince online flag for the current source.
        /// </summary>
        /// <value>
        /// The devince online flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapDeviceOnline
        {
            get
            {
                return _devOnline ?? (_devOnline = new CapWrapper<BoolType>(_source, CapabilityId.CapDeviceOnline, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<BoolType> _autoScan;

        /// <summary>
        /// Gets the property to work with auto scan page flag for the current source.
        /// </summary>
        /// <value>
        /// The auto scan flag.
        /// </value>
        public ICapWrapper<BoolType> CapAutoScan
        {
            get
            {
                return _autoScan ?? (_autoScan = new CapWrapper<BoolType>(_source, CapabilityId.CapAutoScan, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BoolType> _thumbsEnabled;

        /// <summary>
        /// Gets the property to work with thumbnails enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The thumbnails enabled flag.
        /// </value>
        public ICapWrapper<BoolType> CapThumbnailsEnabled
        {
            get
            {
                return _thumbsEnabled ?? (_thumbsEnabled = new CapWrapper<BoolType>(_source, CapabilityId.CapThumbnailsEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<Duplex> _duplex;

        /// <summary>
        /// Gets the property to see what's the duplex mode for the current source.
        /// </summary>
        /// <value>
        /// The duplex mode.
        /// </value>
        public IReadOnlyCapWrapper<Duplex> CapDuplex
        {
            get
            {
                return _duplex ?? (_duplex = new CapWrapper<Duplex>(_source, CapabilityId.CapDuplex, ValueExtensions.ConvertToEnum<Duplex>, true));
            }
        }

        private CapWrapper<BoolType> _duplexEnabled;

        /// <summary>
        /// Gets the property to work with duplex enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The duplex enabled flag.
        /// </value>
        public ICapWrapper<BoolType> CapDuplexEnabled
        {
            get
            {
                return _duplexEnabled ?? (_duplexEnabled = new CapWrapper<BoolType>(_source, CapabilityId.CapDuplexEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        private CapWrapper<BoolType> _dsUIonly;

        /// <summary>
        /// Gets the property to see whether device supports UI only flag (no transfer).
        /// </summary>
        /// <value>
        /// The UI only flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapEnableDSUIOnly
        {
            get
            {
                return _dsUIonly ?? (_dsUIonly = new CapWrapper<BoolType>(_source, CapabilityId.CapEnableDSUIOnly, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<BoolType> _dsData;

        /// <summary>
        /// Gets the property to see whether device supports custom data triplets.
        /// </summary>
        /// <value>
        /// The custom data flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapCustomDSData
        {
            get
            {
                return _dsData ?? (_dsData = new CapWrapper<BoolType>(_source, CapabilityId.CapCustomDSData, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<uint> _endorser;

        /// <summary>
        /// Gets the property to work with endorser for the current source.
        /// </summary>
        /// <value>
        /// The endorser option.
        /// </value>
        public ICapWrapper<uint> CapEndorser
        {
            get
            {
                return _endorser ?? (_endorser = new CapWrapper<uint>(_source, CapabilityId.CapEndorser, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<JobControl> _jobControl;

        /// <summary>
        /// Gets the property to work with job control option for the current source.
        /// </summary>
        /// <value>
        /// The job control option.
        /// </value>
        public ICapWrapper<JobControl> CapJobControl
        {
            get
            {
                return _jobControl ?? (_jobControl = new CapWrapper<JobControl>(_source, CapabilityId.CapJobControl, ValueExtensions.ConvertToEnum<JobControl>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<AlarmType> _alarms;

        /// <summary>
        /// Gets the property to work with alarms for the current source.
        /// </summary>
        /// <value>
        /// The alarms.
        /// </value>
        public ICapWrapper<AlarmType> CapAlarms
        {
            get
            {
                return _alarms ?? (_alarms = new CapWrapper<AlarmType>(_source, CapabilityId.CapAlarms, ValueExtensions.ConvertToEnum<AlarmType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<int> _alarmVolume;

        /// <summary>
        /// Gets the property to work with alarm volume for the current source.
        /// </summary>
        /// <value>
        /// The alarm volume.
        /// </value>
        public ICapWrapper<int> CapAlarmVolume
        {
            get
            {
                return _alarmVolume ?? (_alarmVolume = new CapWrapper<int>(_source, CapabilityId.CapAlarmVolume, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        private CapWrapper<int> _autoCapture;

        /// <summary>
        /// Gets the property to work with auto capture count for the current source.
        /// </summary>
        /// <value>
        /// The auto capture count.
        /// </value>
        public ICapWrapper<int> CapAutomaticCapture
        {
            get
            {
                return _autoCapture ?? (_autoCapture = new CapWrapper<int>(_source, CapabilityId.CapAutomaticCapture, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        private CapWrapper<int> _timeBeforeCap;

        /// <summary>
        /// Gets the property to work with the time before first capture (milliseconds) for the current source.
        /// </summary>
        /// <value>
        /// The time before first capture.
        /// </value>
        public ICapWrapper<int> CapTimeBeforeFirstCapture
        {
            get
            {
                return _timeBeforeCap ?? (_timeBeforeCap = new CapWrapper<int>(_source, CapabilityId.CapTimeBeforeFirstCapture, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        private CapWrapper<int> _timeBetweenCap;

        /// <summary>
        /// Gets the property to work with the time between captures (milliseconds) for the current source.
        /// </summary>
        /// <value>
        /// The time between captures.
        /// </value>
        public ICapWrapper<int> CapTimeBetweenCaptures
        {
            get
            {
                return _timeBetweenCap ?? (_timeBetweenCap = new CapWrapper<int>(_source, CapabilityId.CapTimeBetweenCaptures, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        private CapWrapper<ClearBuffer> _clearBuff;

        /// <summary>
        /// Gets the property to work with the clear buffers option for the current source.
        /// </summary>
        /// <value>
        /// The clear buffers option.
        /// </value>
        public ICapWrapper<ClearBuffer> CapClearBuffers
        {
            get
            {
                return _clearBuff ?? (_clearBuff = new CapWrapper<ClearBuffer>(_source, CapabilityId.CapClearBuffers, ValueExtensions.ConvertToEnum<ClearBuffer>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<uint> _maxBatchBuff;

        /// <summary>
        /// Gets the property to work with the max buffered pages for the current source.
        /// </summary>
        /// <value>
        /// The max batch buffered pages.
        /// </value>
        public ICapWrapper<uint> CapMaxBatchBuffers
        {
            get
            {
                return _maxBatchBuff ?? (_maxBatchBuff = new CapWrapper<uint>(_source, CapabilityId.CapMaxBatchBuffers, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<string> _devTimeDate;

        /// <summary>
        /// Gets the property to work with the device's time and date.
        /// </summary>
        /// <value>
        /// The device time and date.
        /// </value>
        public ICapWrapper<string> CapDeviceTimeDate
        {
            get
            {
                return _devTimeDate ?? (_devTimeDate = new CapWrapper<string>(_source, CapabilityId.CapDeviceTimeDate, ValueExtensions.ConvertToString,
                    value =>
                    {
                        using (var cap = new TWCapability(CapabilityId.CapDeviceTimeDate, value, ItemType.String32))
                        {
                            return _source.DGControl.Capability.Set(cap);
                        }
                    }));
            }
        }

        private CapWrapper<PowerSupply> _powerSup;

        /// <summary>
        /// Gets the property to see current device's power supply.
        /// </summary>
        /// <value>
        /// The power supply indicator.
        /// </value>
        public IReadOnlyCapWrapper<PowerSupply> CapPowerSupply
        {
            get
            {
                return _powerSup ?? (_powerSup = new CapWrapper<PowerSupply>(_source, CapabilityId.CapPowerSupply, ValueExtensions.ConvertToEnum<PowerSupply>, true));
            }
        }

        private CapWrapper<BoolType> _camPreviewUI;

        /// <summary>
        /// Gets the property to see whether device supports camera preview UI flag.
        /// </summary>
        /// <value>
        /// The camera preview UI flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapCameraPreviewUI
        {
            get
            {
                return _camPreviewUI ?? (_camPreviewUI = new CapWrapper<BoolType>(_source, CapabilityId.CapCameraPreviewUI, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<DeviceEvent> _devEvent;

        /// <summary>
        /// Gets the property to work with the reported device events for the current source.
        /// </summary>
        /// <value>
        /// The reported device events.
        /// </value>
        public ICapWrapper<DeviceEvent> CapDeviceEvent
        {
            get
            {
                return _devEvent ?? (_devEvent = new CapWrapper<DeviceEvent>(_source, CapabilityId.CapDeviceEvent, ValueExtensions.ConvertToEnum<DeviceEvent>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<string> _serialNo;

        /// <summary>
        /// Gets the property for device serial number.
        /// </summary>
        /// <value>
        /// The device serial number.
        /// </value>
        public IReadOnlyCapWrapper<string> CapSerialNumber
        {
            get
            {
                return _serialNo ?? (_serialNo = new CapWrapper<string>(_source, CapabilityId.CapSerialNumber, ValueExtensions.ConvertToString, true));
            }
        }

        private CapWrapper<Printer> _printer;

        /// <summary>
        /// Gets the property to work with printer list for the current source.
        /// </summary>
        /// <value>
        /// The printer list.
        /// </value>
        public ICapWrapper<Printer> CapPrinter
        {
            get
            {
                return _printer ?? (_printer = new CapWrapper<Printer>(_source, CapabilityId.CapPrinter, ValueExtensions.ConvertToEnum<Printer>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _printerEnabled;

        /// <summary>
        /// Gets the property to work with printer enabled flag.
        /// </summary>
        /// <value>
        /// The printer enabled flag.
        /// </value>
        public ICapWrapper<BoolType> CapPrinterEnabled
        {
            get
            {
                return _printerEnabled ?? (_printerEnabled = new CapWrapper<BoolType>(_source, CapabilityId.CapPrinterEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<int> _printerIndex;

        /// <summary>
        /// Gets the property to work with the starting printer index for the current source.
        /// </summary>
        /// <value>
        /// The printer index.
        /// </value>
        public ICapWrapper<int> CapPrinterIndex
        {
            get
            {
                return _printerIndex ?? (_printerIndex = new CapWrapper<int>(_source, CapabilityId.CapPrinterIndex, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<PrinterMode> _printerMode;

        /// <summary>
        /// Gets the property to work with printer mode for the current source.
        /// </summary>
        /// <value>
        /// The printer mode.
        /// </value>
        public ICapWrapper<PrinterMode> CapPrinterMode
        {
            get
            {
                return _printerMode ?? (_printerMode = new CapWrapper<PrinterMode>(_source, CapabilityId.CapPrinterMode, ValueExtensions.ConvertToEnum<PrinterMode>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<string> _printerString;

        /// <summary>
        /// Specifies the string(s) that are to be used in the string component when the current <see cref="CapPrinter"/>
        /// device is enabled.
        /// </summary>
        /// <value>
        /// The printer string.
        /// </value>
        public ICapWrapper<string> CapPrinterString
        {
            get
            {
                return _printerString ?? (_printerString = new CapWrapper<string>(_source, CapabilityId.CapPrinterString, ValueExtensions.ConvertToString,
                    value =>
                    {
                        using (var cap = new TWCapability(CapabilityId.CapPrinterString, value, ItemType.String255))
                        {
                            return _source.DGControl.Capability.Set(cap);
                        }
                    }));
            }
        }

        private CapWrapper<string> _printerSuffix;

        /// <summary>
        /// Specifies the string that shall be used as the current <see cref="CapPrinter"/> device’s suffix.
        /// </summary>
        /// <value>
        /// The printer suffix string.
        /// </value>
        public ICapWrapper<string> CapPrinterSuffix
        {
            get
            {
                return _printerSuffix ?? (_printerSuffix = new CapWrapper<string>(_source, CapabilityId.CapPrinterSuffix, ValueExtensions.ConvertToString,
                    value =>
                    {
                        using (var cap = new TWCapability(CapabilityId.CapPrinterSuffix, value, ItemType.String255))
                        {
                            return _source.DGControl.Capability.Set(cap);
                        }
                    }));
            }
        }

        private CapWrapper<Language> _language;

        /// <summary>
        /// Gets the property to work with string data language for the current source.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        public ICapWrapper<Language> CapLanguage
        {
            get
            {
                return _language ?? (_language = new CapWrapper<Language>(_source, CapabilityId.CapLanguage, ValueExtensions.ConvertToEnum<Language>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<FeederAlignment> _feedAlign;

        /// <summary>
        /// Gets the property to work with feeder alignment for the current source.
        /// </summary>
        /// <value>
        /// The feeder alignment.
        /// </value>
        public ICapWrapper<FeederAlignment> CapFeederAlignment
        {
            get
            {
                return _feedAlign ?? (_feedAlign = new CapWrapper<FeederAlignment>(_source, CapabilityId.CapFeederAlignment, ValueExtensions.ConvertToEnum<FeederAlignment>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<FeederOrder> _feedOrder;

        /// <summary>
        /// Gets the property to work with feeder order for the current source.
        /// </summary>
        /// <value>
        /// The feeder order.
        /// </value>
        public ICapWrapper<FeederOrder> CapFeederOrder
        {
            get
            {
                return _feedOrder ?? (_feedOrder = new CapWrapper<FeederOrder>(_source, CapabilityId.CapFeederOrder, ValueExtensions.ConvertToEnum<FeederOrder>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _reacuireAllow;

        /// <summary>
        /// Gets the property to see whether device supports reacquire flag.
        /// </summary>
        /// <value>
        /// The reacquire flag.
        /// </value>
        public IReadOnlyCapWrapper<BoolType> CapReacquireAllowed
        {
            get
            {
                return _reacuireAllow ?? (_reacuireAllow = new CapWrapper<BoolType>(_source, CapabilityId.CapReacquireAllowed, ValueExtensions.ConvertToEnum<BoolType>, true));
            }
        }

        private CapWrapper<int> _battMinutes;

        /// <summary>
        /// Gets the property to see the remaining battery power for the device.
        /// </summary>
        /// <value>
        /// The battery minutes.
        /// </value>
        public IReadOnlyCapWrapper<int> CapBatteryMinutes
        {
            get
            {
                return _battMinutes ?? (_battMinutes = new CapWrapper<int>(_source, CapabilityId.CapBatteryMinutes, ValueExtensions.ConvertToEnum<int>, true));
            }
        }

        private CapWrapper<int> _battPercent;

        /// <summary>
        /// Gets the property to see the remaining battery percentage for the device.
        /// </summary>
        /// <value>
        /// The battery percentage.
        /// </value>
        public IReadOnlyCapWrapper<int> CapBatteryPercentage
        {
            get
            {
                return _battPercent ?? (_battPercent = new CapWrapper<int>(_source, CapabilityId.CapBatteryPercentage, ValueExtensions.ConvertToEnum<int>, true));
            }
        }

        private CapWrapper<CameraSide> _camSide;

        /// <summary>
        /// Gets the property to work with camera side for the current source.
        /// </summary>
        /// <value>
        /// The camera side.
        /// </value>
        public ICapWrapper<CameraSide> CapCameraSide
        {
            get
            {
                return _camSide ?? (_camSide = new CapWrapper<CameraSide>(_source, CapabilityId.CapCameraSide, ValueExtensions.ConvertToEnum<CameraSide>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<Segmented> _segmented;

        /// <summary>
        /// Gets the property to work with segmentation setting for the current source.
        /// </summary>
        /// <value>
        /// The segmentation setting.
        /// </value>
        public ICapWrapper<Segmented> CapSegmented
        {
            get
            {
                return _segmented ?? (_segmented = new CapWrapper<Segmented>(_source, CapabilityId.CapSegmented, ValueExtensions.ConvertToEnum<Segmented>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _camEnabled;

        /// <summary>
        /// Gets the property to work with camera enabled flag.
        /// </summary>
        /// <value>
        /// The camera enabled flag.
        /// </value>
        public ICapWrapper<BoolType> CapCameraEnabled
        {
            get
            {
                return _camEnabled ?? (_camEnabled = new CapWrapper<BoolType>(_source, CapabilityId.CapCameraEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<PixelType> _camOrder;

        /// <summary>
        /// Gets the property to work with camera order for the current source.
        /// </summary>
        /// <value>
        /// The camera order setting.
        /// </value>
        public ICapWrapper<PixelType> CapCameraOrder
        {
            get
            {
                return _camOrder ?? (_camOrder = new CapWrapper<PixelType>(_source, CapabilityId.CapCameraOrder, ValueExtensions.ConvertToEnum<PixelType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _micrEnabled;

        /// <summary>
        /// Gets the property to work with check scanning support flag.
        /// </summary>
        /// <value>
        /// The check scanning support flag.
        /// </value>
        public ICapWrapper<BoolType> CapMicrEnabled
        {
            get
            {
                return _micrEnabled ?? (_micrEnabled = new CapWrapper<BoolType>(_source, CapabilityId.CapMicrEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<BoolType> _feederPrep;

        /// <summary>
        /// Gets the property to work with feeder prep flag.
        /// </summary>
        /// <value>
        /// The feeder prep flag.
        /// </value>
        public ICapWrapper<BoolType> CapFeederPrep
        {
            get
            {
                return _feederPrep ?? (_feederPrep = new CapWrapper<BoolType>(_source, CapabilityId.CapFeederPrep, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<FeederPocket> _feedPocket;

        /// <summary>
        /// Gets the property to work with feeder pocket for the current source.
        /// </summary>
        /// <value>
        /// The feeder pocket setting.
        /// </value>
        public ICapWrapper<FeederPocket> CapFeederPocket
        {
            get
            {
                return _feedPocket ?? (_feedPocket = new CapWrapper<FeederPocket>(_source, CapabilityId.CapFeederPocket, ValueExtensions.ConvertToEnum<FeederPocket>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<BoolType> _autoMedium;

        /// <summary>
        /// Gets the property to work with auto-sense medium (paper source) flag.
        /// </summary>
        /// <value>
        /// The auto-sense medium flag.
        /// </value>
        public ICapWrapper<BoolType> CapAutomaticSenseMedium
        {
            get
            {
                return _autoMedium ?? (_autoMedium = new CapWrapper<BoolType>(_source, CapabilityId.CapAutomaticSenseMedium, ValueExtensions.ConvertToEnum<BoolType>,
                    value => new TWOneValue
                    {
                        Item = (uint)value,
                        ItemType = ItemType.Bool
                    }));
            }
        }

        private CapWrapper<string> _custGuid;

        /// <summary>
        /// Gets the property for device interface guid.
        /// </summary>
        /// <value>
        /// The device interface guid.
        /// </value>
        public IReadOnlyCapWrapper<string> CapCustomInterfaceGuid
        {
            get
            {
                return _custGuid ?? (_custGuid = new CapWrapper<string>(_source, CapabilityId.CapCustomInterfaceGuid, ValueExtensions.ConvertToString, true));
            }
        }

        private CapWrapper<CapabilityId> _supportedCapsUnique;

        /// <summary>
        /// Gets the supported caps for unique segments for the current source.
        /// </summary>
        /// <value>
        /// The supported caps for unique segments.
        /// </value>
        public IReadOnlyCapWrapper<CapabilityId> CapSupportedCapsSegmentUnique
        {
            get
            {
                return _supportedCapsUnique ?? (_supportedCapsUnique = new CapWrapper<CapabilityId>(_source, CapabilityId.CapSupportedCapsSegmentUnique, value => value.ConvertToEnum<CapabilityId>(false), true));
            }
        }

        private CapWrapper<uint> _supportedDat;

        /// <summary>
        /// Gets the supported caps for supported DATs.
        /// </summary>
        /// <value>
        /// The supported DATs.
        /// </value>
        public IReadOnlyCapWrapper<uint> CapSupportedDATs
        {
            get
            {
                return _supportedDat ?? (_supportedDat = new CapWrapper<uint>(_source, CapabilityId.CapSupportedDATs, ValueExtensions.ConvertToEnum<uint>, true));
            }
        }

        private CapWrapper<DoubleFeedDetection> _dblFeedDetect;

        /// <summary>
        /// Gets the property to work with double feed detection option for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection option.
        /// </value>
        public ICapWrapper<DoubleFeedDetection> CapDoubleFeedDetection
        {
            get
            {
                return _dblFeedDetect ?? (_dblFeedDetect = new CapWrapper<DoubleFeedDetection>(_source, CapabilityId.CapDoubleFeedDetection, ValueExtensions.ConvertToEnum<DoubleFeedDetection>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<TWFix32> _dblFeedLength;

        /// <summary>
        /// Gets the property to work with double feed detection length for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection length.
        /// </value>
        public ICapWrapper<TWFix32> CapDoubleFeedDetectionLength
        {
            get
            {
                return _dblFeedLength ?? (_dblFeedLength = new CapWrapper<TWFix32>(_source, CapabilityId.CapDoubleFeedDetectionLength, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<DoubleFeedDetectionSensitivity> _dblFeedSensitivity;

        /// <summary>
        /// Gets the property to work with double feed detection sensitivity for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection sensitivity.
        /// </value>
        public ICapWrapper<DoubleFeedDetectionSensitivity> CapDoubleFeedDetectionSensitivity
        {
            get
            {
                return _dblFeedSensitivity ?? (_dblFeedSensitivity = new CapWrapper<DoubleFeedDetectionSensitivity>(_source, CapabilityId.CapDoubleFeedDetectionSensitivity, ValueExtensions.ConvertToEnum<DoubleFeedDetectionSensitivity>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<DoubleFeedDetectionResponse> _dblFeedResp;

        /// <summary>
        /// Gets the property to work with double feed detection response for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection response.
        /// </value>
        public ICapWrapper<DoubleFeedDetectionResponse> CapDoubleFeedDetectionResponse
        {
            get
            {
                return _dblFeedResp ?? (_dblFeedResp = new CapWrapper<DoubleFeedDetectionResponse>(_source, CapabilityId.CapDoubleFeedDetectionResponse, ValueExtensions.ConvertToEnum<DoubleFeedDetectionResponse>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<PaperHandling> _paperHandling;

        /// <summary>
        /// Gets the property to work with paper handling option for the current source.
        /// </summary>
        /// <value>
        /// The paper handling option.
        /// </value>
        public ICapWrapper<PaperHandling> CapPaperHandling
        {
            get
            {
                return _paperHandling ?? (_paperHandling = new CapWrapper<PaperHandling>(_source, CapabilityId.CapPaperHandling, ValueExtensions.ConvertToEnum<PaperHandling>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<IndicatorsMode> _indicatorMode;

        /// <summary>
        /// Gets the property to work with diplayed indicators for the current source.
        /// </summary>
        /// <value>
        /// The diplayed indicators.
        /// </value>
        public ICapWrapper<IndicatorsMode> CapIndicatorsMode
        {
            get
            {
                return _indicatorMode ?? (_indicatorMode = new CapWrapper<IndicatorsMode>(_source, CapabilityId.CapIndicatorsMode, ValueExtensions.ConvertToEnum<IndicatorsMode>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        private CapWrapper<TWFix32> _printVOffset;

        /// <summary>
        /// Gets the property to work with printer y-offset for the current source.
        /// </summary>
        /// <value>
        /// The printer y-offset.
        /// </value>
        public ICapWrapper<TWFix32> CapPrinterVerticalOffset
        {
            get
            {
                return _printVOffset ?? (_printVOffset = new CapWrapper<TWFix32>(_source, CapabilityId.CapPrinterVerticalOffset, ValueExtensions.ConvertToFix32, value => value.ToOneValue()));
            }
        }

        private CapWrapper<int> _powerSaveTime;

        /// <summary>
        /// Gets the property to work with camera power down time (seconds) for the current source.
        /// </summary>
        /// <value>
        /// The camera power down time.
        /// </value>
        public ICapWrapper<int> CapPowerSaveTime
        {
            get
            {
                return _powerSaveTime ?? (_powerSaveTime = new CapWrapper<int>(_source, CapabilityId.CapPowerSaveTime, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        private CapWrapper<int> _printCharRot;

        /// <summary>
        /// Gets the property to work with printer character rotation for the current source.
        /// </summary>
        /// <value>
        /// The printer character rotation.
        /// </value>
        public ICapWrapper<int> CapPrinterCharRotation
        {
            get
            {
                return _printCharRot ?? (_printCharRot = new CapWrapper<int>(_source, CapabilityId.CapPrinterCharRotation, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<PrinterFontStyle> _printFontStyle;

        /// <summary>
        /// Gets the property to work with printer font style for the current source.
        /// </summary>
        /// <value>
        /// The printer font style.
        /// </value>
        public ICapWrapper<PrinterFontStyle> CapPrinterFontStyle
        {
            get
            {
                return _printFontStyle ?? (_printFontStyle = new CapWrapper<PrinterFontStyle>(_source, CapabilityId.CapPrinterFontStyle, ValueExtensions.ConvertToEnum<PrinterFontStyle>, false));
            }
        }

        private CapWrapper<string> _printerIdxLeadChar;

        /// <summary>
        /// Set the character to be used for filling the leading digits before the counter value if the
        /// counter digits are fewer than <see cref="CapPrinterIndexNumDigits"/>.
        /// </summary>
        /// <value>
        /// The printer leading string.
        /// </value>
        public ICapWrapper<string> CapPrinterIndexLeadChar
        {
            get
            {
                return _printerIdxLeadChar ?? (_printerIdxLeadChar = new CapWrapper<string>(_source, CapabilityId.CapPrinterIndexLeadChar, ValueExtensions.ConvertToString,
                    value =>
                    {
                        using (var cap = new TWCapability(CapabilityId.CapPrinterIndexLeadChar, value, ItemType.String32))
                        {
                            return _source.DGControl.Capability.Set(cap);
                        }
                    }));
            }
        }

        private CapWrapper<uint> _printIdxMax;

        /// <summary>
        /// Gets the property to work with printer index max value for the current source.
        /// </summary>
        /// <value>
        /// The printer index max value.
        /// </value>
        public ICapWrapper<uint> CapPrinterIndexMaxValue
        {
            get
            {
                return _printIdxMax ?? (_printIdxMax = new CapWrapper<uint>(_source, CapabilityId.CapPrinterIndexMaxValue, ValueExtensions.ConvertToEnum<uint>,
                        value => new TWOneValue
                        {
                            Item = value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<int> _printNumDigit;

        /// <summary>
        /// Gets the property to work with printer number digits value for the current source.
        /// </summary>
        /// <value>
        /// The printer number digits value.
        /// </value>
        public ICapWrapper<int> CapPrinterIndexNumDigits
        {
            get
            {
                return _printNumDigit ?? (_printNumDigit = new CapWrapper<int>(_source, CapabilityId.CapPrinterIndexNumDigits, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<int> _printIdxStep;

        /// <summary>
        /// Gets the property to work with printer index step value for the current source.
        /// </summary>
        /// <value>
        /// The printer index step value.
        /// </value>
        public ICapWrapper<int> CapPrinterIndexStep
        {
            get
            {
                return _printIdxStep ?? (_printIdxStep = new CapWrapper<int>(_source, CapabilityId.CapPrinterIndexStep, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt32
                        }));
            }
        }

        private CapWrapper<PrinterIndexTrigger> _printIdxTrig;

        /// <summary>
        /// Gets the property to work with printer index trigger for the current source.
        /// </summary>
        /// <value>
        /// The printer index trigger.
        /// </value>
        public ICapWrapper<PrinterIndexTrigger> CapPrinterIndexTrigger
        {
            get
            {
                return _printIdxTrig ?? (_printIdxTrig = new CapWrapper<PrinterIndexTrigger>(_source, CapabilityId.CapPrinterIndexTrigger, ValueExtensions.ConvertToEnum<PrinterIndexTrigger>, false));
            }
        }

        private CapWrapper<string> _printPreview;

        /// <summary>
        /// Gets the next print values.
        /// </summary>
        /// <value>
        /// The next print values.
        /// </value>
        public IReadOnlyCapWrapper<string> CapPrinterStringPreview
        {
            get
            {
                return _printPreview ?? (_printPreview = new CapWrapper<string>(_source, CapabilityId.CapPrinterStringPreview, ValueExtensions.ConvertToString, true));
            }
        }

        #endregion

    }
}
