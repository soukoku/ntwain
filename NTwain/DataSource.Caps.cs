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
        /// <returns></returns>
        public ReturnCode CapResetAll()
        {
            using (TWCapability cap = new TWCapability(CapabilityId.CapSupportedCaps))
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
            using (TWCapability cap = new TWCapability(capabilityId))
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
        public ICapWrapper<XferMech> CapAudioXferMech
        {
            get
            {
                return _audXferMech ?? (_audXferMech = new CapWrapper<XferMech>(this, CapabilityId.ACapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
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
        public ICapWrapper<CompressionType> CapImageCompression
        {
            get
            {
                return _compression ?? (_compression = new CapWrapper<CompressionType>(this, CapabilityId.ICapCompression, ValueExtensions.ConvertToEnum<CompressionType>,
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
        public ICapWrapper<PixelType> CapImagePixelType
        {
            get
            {
                return _pixelType ?? (_pixelType = new CapWrapper<PixelType>(this, CapabilityId.ICapPixelType, ValueExtensions.ConvertToEnum<PixelType>,
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
        public ICapWrapper<Unit> CapImageUnits
        {
            get
            {
                return _imgUnits ?? (_imgUnits = new CapWrapper<Unit>(this, CapabilityId.ICapUnits, ValueExtensions.ConvertToEnum<Unit>,
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
        public ICapWrapper<XferMech> CapImageXferMech
        {
            get
            {
                return _imgXferMech ?? (_imgXferMech = new CapWrapper<XferMech>(this, CapabilityId.ICapXferMech, ValueExtensions.ConvertToEnum<XferMech>,
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
        public ICapWrapper<BoolType> CapImageAutoBright
        {
            get
            {
                return _autoBright ?? (_autoBright = new CapWrapper<BoolType>(this, CapabilityId.ICapAutoBright, ValueExtensions.ConvertToEnum<BoolType>,
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
        public ICapWrapper<TWFix32> CapImageBrightness
        {
            get
            {
                return _brightness ?? (_brightness = new CapWrapper<TWFix32>(this, CapabilityId.ICapBrightness, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }

        private CapWrapper<TWFix32> _contrast;

        /// <summary>
        /// Gets the property to work with image contrast for the current source.
        /// </summary>
        /// <value>
        /// The image contrast.
        /// </value>
        public ICapWrapper<TWFix32> CapImageContrast
        {
            get
            {
                return _contrast ?? (_contrast = new CapWrapper<TWFix32>(this, CapabilityId.ICapContrast, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }

        // TODO: add ICapCustHalftone

        private CapWrapper<TWFix32> _exposureTime;

        /// <summary>
        /// Gets the property to work with image exposure time (in seconds) for the current source.
        /// </summary>
        /// <value>
        /// The image exposure time.
        /// </value>
        public ICapWrapper<TWFix32> CapImageExposureTime
        {
            get
            {
                return _exposureTime ?? (_exposureTime = new CapWrapper<TWFix32>(this, CapabilityId.ICapExposureTime, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }
        
        private CapWrapper<ImageFilter> _imgFilter;

        /// <summary>
        /// Gets the property to work with image <see cref="FilterType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image filter type.
        /// </value>
        public ICapWrapper<ImageFilter> CapImageFilter
        {
            get
            {
                return _imgFilter ?? (_imgFilter = new CapWrapper<ImageFilter>(this, CapabilityId.ICapFilter, ValueExtensions.ConvertToEnum<ImageFilter>,
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
        public ICapWrapper<TWFix32> CapImageGamma
        {
            get
            {
                return _gamma ?? (_gamma = new CapWrapper<TWFix32>(this, CapabilityId.ICapGamma, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }

        // TODO: add ICapHalftones

        private CapWrapper<TWFix32> _highlight;

        /// <summary>
        /// Gets the property to work with image highlight value for the current source.
        /// </summary>
        /// <value>
        /// The image highlight.
        /// </value>
        public ICapWrapper<TWFix32> CapImageHighlight
        {
            get
            {
                return _highlight ?? (_highlight = new CapWrapper<TWFix32>(this, CapabilityId.ICapHighlight, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }
        
        private CapWrapper<FileFormat> _fileFormat;

        /// <summary>
        /// Gets the property to work with image <see cref="FileFormat"/> for the current source.
        /// </summary>
        /// <value>
        /// The image file format.
        /// </value>
        public ICapWrapper<FileFormat> CapImageFileFormat
        {
            get
            {
                return _fileFormat ?? (_fileFormat = new CapWrapper<FileFormat>(this, CapabilityId.ICapImageFileFormat, ValueExtensions.ConvertToEnum<FileFormat>,
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
        public ICapWrapper<BoolType> CapImageLameState
        {
            get
            {
                return _lampState ?? (_lampState = new CapWrapper<BoolType>(this, CapabilityId.ICapLampState, ValueExtensions.ConvertToEnum<BoolType>,
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
        public ICapWrapper<LightSource> CapImageLightSource
        {
            get
            {
                return _lightSource ?? (_lightSource = new CapWrapper<LightSource>(this, CapabilityId.ICapLightSource, ValueExtensions.ConvertToEnum<LightSource>,
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
        public ICapWrapper<OrientationType> CapImageOrientation
        {
            get
            {
                return _orientation ?? (_orientation = new CapWrapper<OrientationType>(this, CapabilityId.ICapOrientation, ValueExtensions.ConvertToEnum<OrientationType>,
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
        public IReadOnlyCapWrapper<TWFix32> CapImagePhysicalWidth
        {
            get
            {
                return _physicalWidth ?? (_physicalWidth = new CapWrapper<TWFix32>(this, CapabilityId.ICapPhysicalWidth, ValueExtensions.ConvertToFix32));
            }
        }

        private CapWrapper<TWFix32> _physicalHeight;

        /// <summary>
        /// Gets the property to work with image physical height for the current source.
        /// </summary>
        /// <value>
        /// The image physical height.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> CapImagePhysicalHeight
        {
            get
            {
                return _physicalHeight ?? (_physicalHeight = new CapWrapper<TWFix32>(this, CapabilityId.ICapPhysicalHeight, ValueExtensions.ConvertToFix32));
            }
        }

        private CapWrapper<TWFix32> _shadow;

        /// <summary>
        /// Gets the property to work with image shadow value for the current source.
        /// </summary>
        /// <value>
        /// The image shadow.
        /// </value>
        public ICapWrapper<TWFix32> CapImageShadow
        {
            get
            {
                return _shadow ?? (_shadow = new CapWrapper<TWFix32>(this, CapabilityId.ICapShadow, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }

        // TODO: add ICapFrames

        private CapWrapper<TWFix32> _nativeXRes;

        /// <summary>
        /// Gets the property to work with image's native x-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image's native x-axis resolution.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> CapImageXNativeResolution
        {
            get
            {
                return _nativeXRes ?? (_nativeXRes = new CapWrapper<TWFix32>(this, CapabilityId.ICapXNativeResolution, ValueExtensions.ConvertToFix32));
            }
        }

        private CapWrapper<TWFix32> _nativeYRes;

        /// <summary>
        /// Gets the property to work with image's native y-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image's native y-axis resolution.
        /// </value>
        public IReadOnlyCapWrapper<TWFix32> CapImageYNativeResolution
        {
            get
            {
                return _nativeYRes ?? (_nativeYRes = new CapWrapper<TWFix32>(this, CapabilityId.ICapYNativeResolution, ValueExtensions.ConvertToFix32));
            }
        }
        
        private CapWrapper<TWFix32> _xResolution;

        /// <summary>
        /// Gets the property to work with image x-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image x-axis resolution.
        /// </value>
        public ICapWrapper<TWFix32> CapImageXResolution
        {
            get
            {
                return _xResolution ?? (_xResolution = new CapWrapper<TWFix32>(this, CapabilityId.ICapXResolution, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }


        private CapWrapper<TWFix32> _yResolution;

        /// <summary>
        /// Gets the property to work with image y-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image y-axis resolution.
        /// </value>
        public ICapWrapper<TWFix32> CapImageYResolution
        {
            get
            {
                return _yResolution ?? (_yResolution = new CapWrapper<TWFix32>(this, CapabilityId.ICapYResolution, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }
        
        private CapWrapper<int> _maxFrames;

        /// <summary>
        /// Gets the property to work with image max frames for the current source.
        /// </summary>
        /// <value>
        /// The image max frames.
        /// </value>
        public ICapWrapper<int> CapImageMaxFrames
        {
            get
            {
                return _maxFrames ?? (_maxFrames = new CapWrapper<int>(this, CapabilityId.ICapMaxFrames, ValueExtensions.ConvertToEnum<int>,
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
        public ICapWrapper<BoolType> CapImageTiles
        {
            get
            {
                return _tiles ?? (_tiles = new CapWrapper<BoolType>(this, CapabilityId.ICapTiles, ValueExtensions.ConvertToEnum<BoolType>,
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
        public ICapWrapper<BitOrder> CapImageBitOrder
        {
            get
            {
                return _bitOrder ?? (_bitOrder = new CapWrapper<BitOrder>(this, CapabilityId.ICapBitOrder, ValueExtensions.ConvertToEnum<BitOrder>,
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
        public ICapWrapper<int> CapImageCCITTKFactor
        {
            get
            {
                return _ccittKFactor ?? (_ccittKFactor = new CapWrapper<int>(this, CapabilityId.ICapCCITTKFactor, ValueExtensions.ConvertToEnum<int>,
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
        public ICapWrapper<LightPath> CapImageLightPath
        {
            get
            {
                return _lightPath ?? (_lightPath = new CapWrapper<LightPath>(this, CapabilityId.ICapLightPath, ValueExtensions.ConvertToEnum<LightPath>,
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
        public ICapWrapper<PixelFlavor> CapImagePixelFlavor
        {
            get
            {
                return _pixelFlavor ?? (_pixelFlavor = new CapWrapper<PixelFlavor>(this, CapabilityId.ICapPixelFlavor, ValueExtensions.ConvertToEnum<PixelFlavor>,
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
        public ICapWrapper<PlanarChunky> CapImagePlanarChunky
        {
            get
            {
                return _planarChunky ?? (_planarChunky = new CapWrapper<PlanarChunky>(this, CapabilityId.ICapPlanarChunky, ValueExtensions.ConvertToEnum<PlanarChunky>,
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
        public ICapWrapper<TWFix32> CapImageRotation
        {
            get
            {
                return _rotation ?? (_rotation = new CapWrapper<TWFix32>(this, CapabilityId.ICapRotation, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
            }
        }


        private CapWrapper<SupportedSize> _supportSize;

        /// <summary>
        /// Gets the property to work with image <see cref="SupportedSize"/> for the current source.
        /// </summary>
        /// <value>
        /// The image supported size.
        /// </value>
        public ICapWrapper<SupportedSize> CapImageSupportedSize
        {
            get
            {
                return _supportSize ?? (_supportSize = new CapWrapper<SupportedSize>(this, CapabilityId.ICapSupportedSizes, ValueExtensions.ConvertToEnum<SupportedSize>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.UInt16
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
        public ICapWrapper<BoolType> CapImageAutoDeskew
        {
            get
            {
                return _autoDeskew ?? (_autoDeskew = new CapWrapper<BoolType>(this, CapabilityId.ICapAutomaticDeskew, ValueExtensions.ConvertToEnum<BoolType>,
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
        public ICapWrapper<BoolType> CapImageAutoRotate
        {
            get
            {
                return _autoRotate ?? (_autoRotate = new CapWrapper<BoolType>(this, CapabilityId.ICapAutomaticRotate, ValueExtensions.ConvertToEnum<BoolType>,
                         value => new TWOneValue
                         {
                             Item = (uint)value,
                             ItemType = ItemType.Bool
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
        public ICapWrapper<BoolType> CapImageAutomaticBorderDetection
        {
            get
            {
                return _borderDetect ?? (_borderDetect = new CapWrapper<BoolType>(this, CapabilityId.ICapAutomaticBorderDetection, ValueExtensions.ConvertToEnum<BoolType>,
                    value =>
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
                return _xferCount ?? (_xferCount = new CapWrapper<int>(this, CapabilityId.CapXferCount, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = value > 0 ? (uint)value : uint.MaxValue,
                            ItemType = ItemType.UInt16
                        }));
            }
        }

        #endregion


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
        public ICapWrapper<BoolType> CapDuplexEnabled
        {
            get
            {
                return _duplexEnabled ?? (_duplexEnabled = new CapWrapper<BoolType>(this, CapabilityId.CapDuplexEnabled, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
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
        public ICapWrapper<BoolType> CapFeederEnabled
        {
            get
            {
                return _feederEnabled ?? (_feederEnabled = new CapWrapper<BoolType>(this, CapabilityId.CapFeederEnabled, ValueExtensions.ConvertToEnum<BoolType>,
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
        public ICapWrapper<BoolType> CapClearPage
        {
            get
            {
                return _clearPage ?? (_clearPage = new CapWrapper<BoolType>(this, CapabilityId.CapClearPage, ValueExtensions.ConvertToEnum<BoolType>,
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
                return _feedPage ?? (_feedPage = new CapWrapper<BoolType>(this, CapabilityId.CapFeedPage, ValueExtensions.ConvertToEnum<BoolType>,
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
                return _rewindPage ?? (_rewindPage = new CapWrapper<BoolType>(this, CapabilityId.CapRewindPage, ValueExtensions.ConvertToEnum<BoolType>,
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
                return _indicators ?? (_indicators = new CapWrapper<BoolType>(this, CapabilityId.CapIndicators, ValueExtensions.ConvertToEnum<BoolType>,
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
        public IReadOnlyCapWrapper<BoolType> CapUIControllable
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
        public IReadOnlyCapWrapper<BoolType> CapDeviceOnline
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
        public ICapWrapper<BoolType> CapThumbnailsEnabled
        {
            get
            {
                return _thumbsEnabled ?? (_thumbsEnabled = new CapWrapper<BoolType>(this, CapabilityId.CapThumbnailsEnabled, ValueExtensions.ConvertToEnum<BoolType>,
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
        public IReadOnlyCapWrapper<BoolType> CapCustomDSData
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
        public ICapWrapper<JobControl> CapJobControl
        {
            get
            {
                return _jobControl ?? (_jobControl = new CapWrapper<JobControl>(this, CapabilityId.CapJobControl, ValueExtensions.ConvertToEnum<JobControl>,
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
                return _alarmVolume ?? (_alarmVolume = new CapWrapper<int>(this, CapabilityId.CapAlarmVolume, ValueExtensions.ConvertToEnum<int>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Int32
                        }));
            }
        }

        //private CapWrapper<int> _autoCapture;

        ///// <summary>
        ///// Gets the property to work with auto capture count for the current source.
        ///// </summary>
        ///// <value>
        ///// The auto capture count.
        ///// </value>
        //public ICapWrapper<int> CapAutomaticCapture
        //{
        //    get
        //    {
        //        return _autoCapture ?? (_autoCapture = new CapWrapper<int>(this, CapabilityId.CapAutomaticCapture, ValueExtensions.ConvertToEnum<int>,
        //                new TWCapability(CapabilityId.CapAutomaticCapture, new TWOneValue
        //                {
        //                    Item = (uint)value,
        //                    ItemType = ItemType.Int32
        //                }));
        //    }
        //}


        #endregion

        #endregion

    }
}
