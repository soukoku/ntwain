﻿using NTwain.Data;
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
        public ICapWrapper<XferMech> ACapXferMech
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
        public ICapWrapper<CompressionType> ICapCompression
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
        public ICapWrapper<PixelType> ICapPixelType
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
        public ICapWrapper<Unit> ICapUnits
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
        public ICapWrapper<XferMech> ICapXferMech
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
        public ICapWrapper<BoolType> ICapAutoBright
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
        public ICapWrapper<TWFix32> ICapBrightness
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
        public ICapWrapper<TWFix32> ICapContrast
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
        public ICapWrapper<TWFix32> ICapExposureTime
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
                return _filter ?? (_filter = new CapWrapper<FilterType>(this, CapabilityId.ICapFilter, ValueExtensions.ConvertToEnum<FilterType>,
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
        public ICapWrapper<TWFix32> ICapHighlight
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
        public ICapWrapper<FileFormat> ICapImageFileFormat
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
        public ICapWrapper<BoolType> ICapLampState
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
        public ICapWrapper<LightSource> ICapLightSource
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
        public ICapWrapper<OrientationType> ICapOrientation
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
        public IReadOnlyCapWrapper<TWFix32> ICapPhysicalWidth
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
        public IReadOnlyCapWrapper<TWFix32> ICapPhysicalHeight
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
        public ICapWrapper<TWFix32> ICapShadow
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
        public IReadOnlyCapWrapper<TWFix32> ICapXNativeResolution
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
        public IReadOnlyCapWrapper<TWFix32> ICapYNativeResolution
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
        public ICapWrapper<TWFix32> ICapXResolution
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
        public ICapWrapper<TWFix32> ICapYResolution
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
        public ICapWrapper<int> ICapMaxFrames
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
        public ICapWrapper<BoolType> ICapTiles
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
        public ICapWrapper<BitOrder> ICapBitOrder
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
        public ICapWrapper<int> ICapCCITTKFactor
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
        public ICapWrapper<LightPath> ICapLightPath
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
        public ICapWrapper<PixelFlavor> ICapPixelFlavor
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
        public ICapWrapper<PlanarChunky> ICapPlanarChunky
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
        public ICapWrapper<TWFix32> ICapRotation
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
        public ICapWrapper<SupportedSize> ICapSupportedSizes
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
                return _threshold ?? (_threshold = new CapWrapper<TWFix32>(this, CapabilityId.ICapThreshold, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
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
                return _xscaling ?? (_xscaling = new CapWrapper<TWFix32>(this, CapabilityId.ICapXScaling, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
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
                return _yscaling ?? (_yscaling = new CapWrapper<TWFix32>(this, CapabilityId.ICapYScaling, ValueExtensions.ConvertToFix32,
                        value => new TWOneValue
                        {
                            Item = (uint)value,// ((uint)dpi) << 16;
                            ItemType = ItemType.Fix32
                        }));
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
                return _bitorderCodes ?? (_bitorderCodes = new CapWrapper<BitOrder>(this, CapabilityId.ICapBitOrderCodes, ValueExtensions.ConvertToEnum<BitOrder>,
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
                return _pixelFlavorCodes ?? (_pixelFlavorCodes = new CapWrapper<PixelFlavor>(this, CapabilityId.ICapPixelFlavorCodes, ValueExtensions.ConvertToEnum<PixelFlavor>,
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
                return _jpegPixelType ?? (_jpegPixelType = new CapWrapper<PixelType>(this, CapabilityId.ICapJpegPixelType, ValueExtensions.ConvertToEnum<PixelType>,
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
                return _timeFill ?? (_timeFill = new CapWrapper<int>(this, CapabilityId.ICapTimeFill, ValueExtensions.ConvertToEnum<int>,
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
                return _bitDepth ?? (_bitDepth = new CapWrapper<int>(this, CapabilityId.ICapBitDepth, ValueExtensions.ConvertToEnum<int>,
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
                return _bitDepthReduction ?? (_bitDepthReduction = new CapWrapper<BitDepthReduction>(this, CapabilityId.ICapBitDepthReduction, ValueExtensions.ConvertToEnum<BitDepthReduction>,
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
                return _undefinedImgSize ?? (_undefinedImgSize = new CapWrapper<BoolType>(this, CapabilityId.ICapUndefinedImageSize, ValueExtensions.ConvertToEnum<BoolType>,
                        value => new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        }));
            }
        }

        // TODO: add ICAP_IMAGEDATASET

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
                return _extImgInfo ?? (_extImgInfo = new CapWrapper<BoolType>(this, CapabilityId.ICapExtImageInfo, ValueExtensions.ConvertToEnum<BoolType>,
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
                return _minHeight ?? (_minHeight = new CapWrapper<TWFix32>(this, CapabilityId.ICapMinimumHeight, ValueExtensions.ConvertToFix32));
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
                return _minWidth ?? (_minWidth = new CapWrapper<TWFix32>(this, CapabilityId.ICapMinimumWidth, ValueExtensions.ConvertToFix32));
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
                return _autoDiscBlankPg ?? (_autoDiscBlankPg = new CapWrapper<BlankPage>(this, CapabilityId.ICapAutoDiscardBlankPages, ValueExtensions.ConvertToEnum<BlankPage>,
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
                return _flipRotation ?? (_flipRotation = new CapWrapper<FlipRotation>(this, CapabilityId.ICapFlipRotation, ValueExtensions.ConvertToEnum<FlipRotation>,
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
                return _barcodeDetectEnabled ?? (_barcodeDetectEnabled = new CapWrapper<BoolType>(this, CapabilityId.ICapBarcodeDetectionEnabled, ValueExtensions.ConvertToEnum<BoolType>,
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
                return _barcodeType ?? (_barcodeType = new CapWrapper<BarcodeType>(this, CapabilityId.ICapSupportedBarcodeTypes, ValueExtensions.ConvertToEnum<BarcodeType>));
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
                return _barcodeMaxPriority ?? (_barcodeMaxPriority = new CapWrapper<uint>(this, CapabilityId.ICapBarcodeMaxSearchPriorities, ValueExtensions.ConvertToEnum<uint>,
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
                return _barcodeSearchPriority ?? (_barcodeSearchPriority = new CapWrapper<BarcodeType>(this, CapabilityId.ICapBarcodeSearchPriorities, ValueExtensions.ConvertToEnum<BarcodeType>,
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
                return _barcodeSearchMode ?? (_barcodeSearchMode = new CapWrapper<BarcodeDirection>(this, CapabilityId.ICapBarcodeSearchMode, ValueExtensions.ConvertToEnum<BarcodeDirection>,
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
                return _barcodeMaxRetries ?? (_barcodeMaxRetries = new CapWrapper<uint>(this, CapabilityId.ICapBarcodeMaxRetries, ValueExtensions.ConvertToEnum<uint>,
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
                return _barcodeTimeout ?? (_barcodeTimeout = new CapWrapper<uint>(this, CapabilityId.ICapBarcodeTimeout, ValueExtensions.ConvertToEnum<uint>,
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
                return _zoomFactor ?? (_zoomFactor = new CapWrapper<int>(this, CapabilityId.ICapZoomFactor, ValueExtensions.ConvertToEnum<int>,
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
                return _patchcodeDetectEnabled ?? (_patchcodeDetectEnabled = new CapWrapper<BoolType>(this, CapabilityId.ICapPatchCodeDetectionEnabled, ValueExtensions.ConvertToEnum<BoolType>,
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
                return _patchcodeType ?? (_patchcodeType = new CapWrapper<PatchCode>(this, CapabilityId.ICapSupportedPatchCodeTypes, ValueExtensions.ConvertToEnum<PatchCode>));
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
                return _patchcodeMaxPriority ?? (_patchcodeMaxPriority = new CapWrapper<uint>(this, CapabilityId.ICapPatchCodeMaxSearchPriorities, ValueExtensions.ConvertToEnum<uint>,
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
                return _patchcodeSearchPriority ?? (_patchcodeSearchPriority = new CapWrapper<PatchCode>(this, CapabilityId.ICapPatchCodeSearchPriorities, ValueExtensions.ConvertToEnum<PatchCode>,
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
                return _patchcodeSearchMode ?? (_patchcodeSearchMode = new CapWrapper<BarcodeDirection>(this, CapabilityId.ICapPatchCodeSearchMode, ValueExtensions.ConvertToEnum<BarcodeDirection>,
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
                return _patchCodeMaxRetries ?? (_patchCodeMaxRetries = new CapWrapper<uint>(this, CapabilityId.ICapPatchCodeMaxRetries, ValueExtensions.ConvertToEnum<uint>,
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
                return _patchCodeTimeout ?? (_patchCodeTimeout = new CapWrapper<uint>(this, CapabilityId.ICapPatchCodeTimeout, ValueExtensions.ConvertToEnum<uint>,
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
                return _flashUsed2 ?? (_flashUsed2 = new CapWrapper<FlashedUsed>(this, CapabilityId.ICapFlashUsed2, ValueExtensions.ConvertToEnum<FlashedUsed>,
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
                return _imgFilter ?? (_imgFilter = new CapWrapper<ImageFilter>(this, CapabilityId.ICapImageFilter, ValueExtensions.ConvertToEnum<ImageFilter>,
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
                return _noiseFilter ?? (_noiseFilter = new CapWrapper<NoiseFilter>(this, CapabilityId.ICapNoiseFilter, ValueExtensions.ConvertToEnum<NoiseFilter>,
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
                return _overscan ?? (_overscan = new CapWrapper<OverScan>(this, CapabilityId.ICapOverScan, ValueExtensions.ConvertToEnum<OverScan>,
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
                return _borderDetect ?? (_borderDetect = new CapWrapper<BoolType>(this, CapabilityId.ICapAutomaticBorderDetection, ValueExtensions.ConvertToEnum<BoolType>,
                    value =>
                    {
                        var rc = ReturnCode.Failure;

                        var one = new TWOneValue
                        {
                            Item = (uint)value,
                            ItemType = ItemType.Bool
                        };

                        // this needs to also set undefined size optino
                        rc = ICapUndefinedImageSize.Set(value);
                        using (TWCapability capValue = new TWCapability(CapabilityId.ICapAutomaticBorderDetection, one))
                        {
                            rc = _session.DGControl.Capability.Set(capValue);
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
        public ICapWrapper<BoolType> ICapAutomaticRotate
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
                return _jpegQuality ?? (_jpegQuality = new CapWrapper<JpegQuality>(this, CapabilityId.ICapJpegQuality, ValueExtensions.ConvertToEnum<JpegQuality>,
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
                return _feederType ?? (_feederType = new CapWrapper<FeederType>(this, CapabilityId.ICapFeederType, ValueExtensions.ConvertToEnum<FeederType>,
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
                return _iccProfile ?? (_iccProfile = new CapWrapper<IccProfile>(this, CapabilityId.ICapICCProfile, ValueExtensions.ConvertToEnum<IccProfile>,
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
                return _autoSize ?? (_autoSize = new CapWrapper<AutoSize>(this, CapabilityId.ICapAutoSize, ValueExtensions.ConvertToEnum<AutoSize>,
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