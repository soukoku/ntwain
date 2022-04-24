using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;
using static TWAINWorkingGroup.TWAIN;

namespace NTwain
{
    /// <summary>
    /// Contains known capabilities for TWAIN 2.4. If a device has own cap or
    /// uses different type for the cap then create your own <see cref="CapWrapper{TValue}"/>
    /// for working with them.
    /// </summary>
    public class Capabilities
    {
        private readonly TWAIN _twain;

        public Capabilities(TWAIN twain)
        {
            _twain = twain;
        }


        /// <summary>
        /// Resets all cap values and constraint to power-on defaults.
        /// Not all sources will support this.
        /// </summary>
        /// <returns></returns>
        public STS ResetAll()
        {
            var twCap = new TW_CAPABILITY
            {
                Cap = CAP.CAP_SUPPORTEDCAPS
            };

            var sts = _twain.DatCapability(DG.CONTROL, MSG.RESETALL, ref twCap);
            return sts;
        }


        #region audio caps

        private CapWrapper<TWSX> _audXferMech;
        public CapWrapper<TWSX> ACAP_XFERMECH
        {
            get
            {
                return _audXferMech ?? (_audXferMech = new CapWrapper<TWSX>(_twain, CAP.ACAP_XFERMECH));
            }
        }

        #endregion

        #region img caps

        #region mandatory

        private CapWrapper<TWCP> _compression;
        public CapWrapper<TWCP> ICAP_COMPRESSION
        {
            get
            {
                return _compression ?? (_compression = new CapWrapper<TWCP>(_twain, CAP.ICAP_COMPRESSION));
            }
        }


        private CapWrapper<TWPT> _pixelType;
        public CapWrapper<TWPT> ICAP_PIXELTYPE
        {
            get
            {
                return _pixelType ?? (_pixelType = new CapWrapper<TWPT>(_twain, CAP.ICAP_PIXELTYPE));
            }
        }

        private CapWrapper<TWUN> _imgUnits;
        public CapWrapper<TWUN> ICAP_UNITS
        {
            get
            {
                return _imgUnits ?? (_imgUnits = new CapWrapper<TWUN>(_twain, CAP.ICAP_UNITS));
            }
        }

        private CapWrapper<TWSX> _imgXferMech;
        public CapWrapper<TWSX> ICAP_XFERMECH
        {
            get
            {
                return _imgXferMech ?? (_imgXferMech = new CapWrapper<TWSX>(_twain, CAP.ICAP_XFERMECH));
            }
        }

        #endregion

        private CapWrapper<BoolType> _autoBright;
        public CapWrapper<BoolType> ICAP_AUTOBRIGHT
        {
            get
            {
                return _autoBright ?? (_autoBright = new CapWrapper<BoolType>(_twain, CAP.ICAP_AUTOBRIGHT));
            }
        }

        private CapWrapper<TW_FIX32> _brightness;
        public CapWrapper<TW_FIX32> ICAP_BRIGHTNESS
        {
            get
            {
                return _brightness ?? (_brightness = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_BRIGHTNESS));
            }
        }

        private CapWrapper<TW_FIX32> _contrast;
        public CapWrapper<TW_FIX32> ICAP_CONTRAST
        {
            get
            {
                return _contrast ?? (_contrast = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_CONTRAST));
            }
        }

        private CapWrapper<byte> _custHalftone;
        public CapWrapper<byte> ICAP_CUSTHALFTONE
        {
            get
            {
                return _custHalftone ?? (_custHalftone = new CapWrapper<byte>(_twain, CAP.ICAP_CUSTHALFTONE));
            }
        }

        private CapWrapper<TW_FIX32> _exposureTime;
        /// <summary>
        /// Gets the property to work with image exposure time (in seconds) for the current source.
        /// </summary>
        /// <value>
        /// The image exposure time.
        /// </value>
        public CapWrapper<TW_FIX32> ICAP_EXPOSURETIME
        {
            get
            {
                return _exposureTime ?? (_exposureTime = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_EXPOSURETIME));
            }
        }

        private CapWrapper<TWFT> _filter;
        public CapWrapper<TWFT> ICAP_FILTER
        {
            get
            {
                return _filter ?? (_filter = new CapWrapper<TWFT>(_twain, CAP.ICAP_FILTER));
            }
        }

        private CapWrapper<TW_FIX32> _gamma;
        public CapWrapper<TW_FIX32> ICAP_GAMMA
        {
            get
            {
                return _gamma ?? (_gamma = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_GAMMA));
            }
        }

        private CapWrapper<TW_STR32> _halftones;
        public CapWrapper<TW_STR32> ICAP_HALFTONES
        {
            get
            {
                return _halftones ?? (_halftones = new CapWrapper<TW_STR32>(_twain, CAP.ICAP_HALFTONES));
            }
        }

        private CapWrapper<TW_FIX32> _highlight;
        public CapWrapper<TW_FIX32> ICAP_HIGHLIGHT
        {
            get
            {
                return _highlight ?? (_highlight = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_HIGHLIGHT));
            }
        }

        private CapWrapper<TWFF> _fileFormat;
        public CapWrapper<TWFF> ICAP_IMAGEFILEFORMAT
        {
            get
            {
                return _fileFormat ?? (_fileFormat = new CapWrapper<TWFF>(_twain, CAP.ICAP_IMAGEFILEFORMAT));
            }
        }


        private CapWrapper<BoolType> _lampState;
        public CapWrapper<BoolType> ICAP_LAMPSTATE
        {
            get
            {
                return _lampState ?? (_lampState = new CapWrapper<BoolType>(_twain, CAP.ICAP_LAMPSTATE));
            }
        }

        private CapWrapper<TWLS> _lightSource;
        public CapWrapper<TWLS> ICAP_LIGHTSOURCE
        {
            get
            {
                return _lightSource ?? (_lightSource = new CapWrapper<TWLS>(_twain, CAP.ICAP_LIGHTSOURCE));
            }
        }

        private CapWrapper<TWOR> _orientation;
        public CapWrapper<TWOR> ICAP_ORIENTATION
        {
            get
            {
                return _orientation ?? (_orientation = new CapWrapper<TWOR>(_twain, CAP.ICAP_ORIENTATION));
            }
        }

        private CapWrapper<TW_FIX32> _physicalWidth;
        public CapWrapper<TW_FIX32> ICAP_PHYSICALWIDTH
        {
            get
            {
                return _physicalWidth ?? (_physicalWidth = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_PHYSICALWIDTH));
            }
        }

        private CapWrapper<TW_FIX32> _physicalHeight;
        public CapWrapper<TW_FIX32> ICAP_PHYSICALHEIGHT
        {
            get
            {
                return _physicalHeight ?? (_physicalHeight = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_PHYSICALHEIGHT));
            }
        }

        private CapWrapper<TW_FIX32> _shadow;
        public CapWrapper<TW_FIX32> ICAP_SHADOW
        {
            get
            {
                return _shadow ?? (_shadow = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_SHADOW));
            }
        }

        private CapWrapper<TW_FRAME> _frames;
        public CapWrapper<TW_FRAME> ICAP_FRAMES
        {
            get
            {
                return _frames ?? (_frames = new CapWrapper<TW_FRAME>(_twain, CAP.ICAP_FRAMES));
            }
        }

        private CapWrapper<TW_FIX32> _nativeXRes;
        public CapWrapper<TW_FIX32> ICAP_XNATIVERESOLUTION
        {
            get
            {
                return _nativeXRes ?? (_nativeXRes = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_XNATIVERESOLUTION));
            }
        }

        private CapWrapper<TW_FIX32> _nativeYRes;
        public CapWrapper<TW_FIX32> ICAP_YNATIVERESOLUTION
        {
            get
            {
                return _nativeYRes ?? (_nativeYRes = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_YNATIVERESOLUTION));
            }
        }

        private CapWrapper<TW_FIX32> _xResolution;
        public CapWrapper<TW_FIX32> ICAP_XRESOLUTION
        {
            get
            {
                return _xResolution ?? (_xResolution = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_XRESOLUTION));
            }
        }


        private CapWrapper<TW_FIX32> _yResolution;
        public CapWrapper<TW_FIX32> ICAP_YRESOLUTION
        {
            get
            {
                return _yResolution ?? (_yResolution = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_YRESOLUTION));
            }
        }

        private CapWrapper<ushort> _maxFrames;
        public CapWrapper<ushort> ICAP_MAXFRAMES
        {
            get
            {
                return _maxFrames ?? (_maxFrames = new CapWrapper<ushort>(_twain, CAP.ICAP_MAXFRAMES));
            }
        }

        private CapWrapper<BoolType> _tiles;
        public CapWrapper<BoolType> ICAP_TILES
        {
            get
            {
                return _tiles ?? (_tiles = new CapWrapper<BoolType>(_twain, CAP.ICAP_TILES));
            }
        }

        private CapWrapper<TWBO> _bitOrder;
        public CapWrapper<TWBO> ICAP_BITORDER
        {
            get
            {
                return _bitOrder ?? (_bitOrder = new CapWrapper<TWBO>(_twain, CAP.ICAP_BITORDER));
            }
        }

        private CapWrapper<ushort> _ccittKFactor;
        public CapWrapper<ushort> ICAP_CCITTKFACTOR
        {
            get
            {
                return _ccittKFactor ?? (_ccittKFactor = new CapWrapper<ushort>(_twain, CAP.ICAP_CCITTKFACTOR));
            }
        }

        private CapWrapper<TWLP> _lightPath;
        public CapWrapper<TWLP> ICAP_LIGHTPATH
        {
            get
            {
                return _lightPath ?? (_lightPath = new CapWrapper<TWLP>(_twain, CAP.ICAP_LIGHTPATH));
            }
        }

        private CapWrapper<TWPF> _pixelFlavor;
        public CapWrapper<TWPF> ICAP_PIXELFLAVOR
        {
            get
            {
                return _pixelFlavor ?? (_pixelFlavor = new CapWrapper<TWPF>(_twain, CAP.ICAP_PIXELFLAVOR));
            }
        }

        private CapWrapper<TWPC> _planarChunky;
        public CapWrapper<TWPC> ICAP_PLANARCHUNKY
        {
            get
            {
                return _planarChunky ?? (_planarChunky = new CapWrapper<TWPC>(_twain, CAP.ICAP_PLANARCHUNKY));
            }
        }

        private CapWrapper<TW_FIX32> _rotation;
        public CapWrapper<TW_FIX32> ICAP_ROTATION
        {
            get
            {
                return _rotation ?? (_rotation = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_ROTATION));
            }
        }

        private CapWrapper<TWSS> _supportSize;
        public CapWrapper<TWSS> ICAP_SUPPORTEDSIZES
        {
            get
            {
                return _supportSize ?? (_supportSize = new CapWrapper<TWSS>(_twain, CAP.ICAP_SUPPORTEDSIZES));
            }
        }

        private CapWrapper<TW_FIX32> _threshold;
        public CapWrapper<TW_FIX32> ICAP_THRESHOLD
        {
            get
            {
                return _threshold ?? (_threshold = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_THRESHOLD));
            }
        }

        private CapWrapper<TW_FIX32> _xscaling;
        public CapWrapper<TW_FIX32> ICAP_XSCALING
        {
            get
            {
                return _xscaling ?? (_xscaling = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_XSCALING));
            }
        }

        private CapWrapper<TW_FIX32> _yscaling;
        public CapWrapper<TW_FIX32> ICAP_YSCALING
        {
            get
            {
                return _yscaling ?? (_yscaling = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_YSCALING));
            }
        }

        private CapWrapper<TWBO> _bitorderCodes;

        /// <summary>
        /// Gets the property to work with image CCITT compression <see cref="TWBO"/> for the current source.
        /// </summary>
        /// <value>
        /// The image bit order for CCITT compression.
        /// </value>
        public CapWrapper<TWBO> ICAP_BITORDERCODES
        {
            get
            {
                return _bitorderCodes ?? (_bitorderCodes = new CapWrapper<TWBO>(_twain, CAP.ICAP_BITORDERCODES));
            }
        }

        private CapWrapper<TWPF> _pixelFlavorCodes;

        /// <summary>
        /// Gets the property to work with image CCITT compression <see cref="TWPF"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel flavor for CCITT compression.
        /// </value>
        public CapWrapper<TWPF> ICAP_PIXELFLAVORCODES
        {
            get
            {
                return _pixelFlavorCodes ?? (_pixelFlavorCodes = new CapWrapper<TWPF>(_twain, CAP.ICAP_PIXELFLAVORCODES));
            }
        }

        private CapWrapper<TWPT> _jpegPixelType;

        /// <summary>
        /// Gets the property to work with image jpeg compression <see cref="TWPT"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel type for jpeg compression.
        /// </value>
        public CapWrapper<TWPT> ICAP_JPEGPIXELTYPE
        {
            get
            {
                return _jpegPixelType ?? (_jpegPixelType = new CapWrapper<TWPT>(_twain, CAP.ICAP_JPEGPIXELTYPE));
            }
        }

        private CapWrapper<ushort> _timeFill;

        /// <summary>
        /// Gets the property to work with image CCITT time fill for the current source.
        /// </summary>
        /// <value>
        /// The image CCITT time fill.
        /// </value>
        public CapWrapper<ushort> ICAP_TIMEFILL
        {
            get
            {
                return _timeFill ?? (_timeFill = new CapWrapper<ushort>(_twain, CAP.ICAP_TIMEFILL));
            }
        }

        private CapWrapper<ushort> _bitDepth;
        public CapWrapper<ushort> ICAP_BITDEPTH
        {
            get
            {
                return _bitDepth ?? (_bitDepth = new CapWrapper<ushort>(_twain, CAP.ICAP_BITDEPTH));
            }
        }

        private CapWrapper<TWBR> _bitDepthReduction;
        public CapWrapper<TWBR> ICAP_BITDEPTHREDUCTION
        {
            get
            {
                return _bitDepthReduction ?? (_bitDepthReduction = new CapWrapper<TWBR>(_twain, CAP.ICAP_BITDEPTHREDUCTION));
            }
        }

        private CapWrapper<BoolType> _undefinedImgSize;
        public CapWrapper<BoolType> ICAP_UNDEFINEDIMAGESIZE
        {
            get
            {
                return _undefinedImgSize ?? (_undefinedImgSize = new CapWrapper<BoolType>(_twain, CAP.ICAP_UNDEFINEDIMAGESIZE));
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
        public CapWrapper<uint> ICAP_IMAGEDATASET
        {
            get
            {
                return _imgDataSet ?? (_imgDataSet = new CapWrapper<uint>(_twain, CAP.ICAP_IMAGEDATASET));
            }
        }

        private CapWrapper<BoolType> _extImgInfo;
        public CapWrapper<BoolType> ICAP_EXTIMAGEINFO
        {
            get
            {
                return _extImgInfo ?? (_extImgInfo = new CapWrapper<BoolType>(_twain, CAP.ICAP_EXTIMAGEINFO));
            }
        }

        private CapWrapper<TW_FIX32> _minHeight;
        public CapWrapper<TW_FIX32> ICAP_MINIMUMHEIGHT
        {
            get
            {
                return _minHeight ?? (_minHeight = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_MINIMUMHEIGHT));
            }
        }

        private CapWrapper<TW_FIX32> _minWidth;
        public CapWrapper<TW_FIX32> ICAP_MINIMUMWIDTH
        {
            get
            {
                return _minWidth ?? (_minWidth = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_MINIMUMWIDTH));
            }
        }

        private CapWrapper<TWBP> _autoDiscBlankPg;
        public CapWrapper<TWBP> ICAP_AUTODISCARDBLANKPAGES
        {
            get
            {
                return _autoDiscBlankPg ?? (_autoDiscBlankPg = new CapWrapper<TWBP>(_twain, CAP.ICAP_AUTODISCARDBLANKPAGES));
            }
        }

        private CapWrapper<TWFR> _flipRotation;
        public CapWrapper<TWFR> ICAP_FLIPROTATION
        {
            get
            {
                return _flipRotation ?? (_flipRotation = new CapWrapper<TWFR>(_twain, CAP.ICAP_FLIPROTATION));
            }
        }

        private CapWrapper<BoolType> _barcodeDetectEnabled;
        public CapWrapper<BoolType> ICAP_BARCODEDETECTIONENABLED
        {
            get
            {
                return _barcodeDetectEnabled ?? (_barcodeDetectEnabled = new CapWrapper<BoolType>(_twain, CAP.ICAP_BARCODEDETECTIONENABLED));
            }
        }

        private CapWrapper<TWBT> _barcodeType;
        public CapWrapper<TWBT> ICAP_SUPPORTEDBARCODETYPES
        {
            get
            {
                return _barcodeType ?? (_barcodeType = new CapWrapper<TWBT>(_twain, CAP.ICAP_SUPPORTEDBARCODETYPES));
            }
        }

        private CapWrapper<uint> _barcodeMaxPriority;
        public CapWrapper<uint> ICAP_BARCODEMAXSEARCHPRIORITIES
        {
            get
            {
                return _barcodeMaxPriority ?? (_barcodeMaxPriority = new CapWrapper<uint>(_twain, CAP.ICAP_BARCODEMAXSEARCHPRIORITIES));
            }
        }

        private CapWrapper<TWBT> _barcodeSearchPriority;
        public CapWrapper<TWBT> ICAP_BARCODESEARCHPRIORITIES
        {
            get
            {
                return _barcodeSearchPriority ?? (_barcodeSearchPriority = new CapWrapper<TWBT>(_twain, CAP.ICAP_BARCODESEARCHPRIORITIES));
            }
        }

        private CapWrapper<TWBD> _barcodeSearchMode;
        public CapWrapper<TWBD> ICAP_BARCODESEARCHMODE
        {
            get
            {
                return _barcodeSearchMode ?? (_barcodeSearchMode = new CapWrapper<TWBD>(_twain, CAP.ICAP_BARCODESEARCHMODE));
            }
        }

        private CapWrapper<uint> _barcodeMaxRetries;
        public CapWrapper<uint> ICAP_BARCODEMAXRETRIES
        {
            get
            {
                return _barcodeMaxRetries ?? (_barcodeMaxRetries = new CapWrapper<uint>(_twain, CAP.ICAP_BARCODEMAXRETRIES));
            }
        }

        private CapWrapper<uint> _barcodeTimeout;

        /// <summary>
        /// Gets the property to work with image barcode max search timeout for the current source.
        /// </summary>
        /// <value>
        /// The image barcode max search timeout.
        /// </value>
        public CapWrapper<uint> ICAP_BARCODETIMEOUT
        {
            get
            {
                return _barcodeTimeout ?? (_barcodeTimeout = new CapWrapper<uint>(_twain, CAP.ICAP_BARCODETIMEOUT));
            }
        }

        private CapWrapper<short> _zoomFactor;
        public CapWrapper<short> ICAP_ZOOMFACTOR
        {
            get
            {
                return _zoomFactor ?? (_zoomFactor = new CapWrapper<short>(_twain, CAP.ICAP_ZOOMFACTOR));
            }
        }

        private CapWrapper<BoolType> _patchcodeDetectEnabled;
        public CapWrapper<BoolType> ICAP_PATCHCODEDETECTIONENABLED
        {
            get
            {
                return _patchcodeDetectEnabled ?? (_patchcodeDetectEnabled = new CapWrapper<BoolType>(_twain, CAP.ICAP_PATCHCODEDETECTIONENABLED));
            }
        }

        private CapWrapper<TWPCH> _patchcodeType;
        public CapWrapper<TWPCH> ICAP_SUPPORTEDPATCHCODETYPES
        {
            get
            {
                return _patchcodeType ?? (_patchcodeType = new CapWrapper<TWPCH>(_twain, CAP.ICAP_SUPPORTEDPATCHCODETYPES));
            }
        }

        private CapWrapper<uint> _patchcodeMaxPriority;
        public CapWrapper<uint> ICAP_PATCHCODEMAXSEARCHPRIORITIES
        {
            get
            {
                return _patchcodeMaxPriority ?? (_patchcodeMaxPriority = new CapWrapper<uint>(_twain, CAP.ICAP_PATCHCODEMAXSEARCHPRIORITIES));
            }
        }

        private CapWrapper<TWPCH> _patchcodeSearchPriority;
        public CapWrapper<TWPCH> ICAP_PATCHCODESEARCHPRIORITIES
        {
            get
            {
                return _patchcodeSearchPriority ?? (_patchcodeSearchPriority = new CapWrapper<TWPCH>(_twain, CAP.ICAP_PATCHCODESEARCHPRIORITIES));
            }
        }

        private CapWrapper<TWBD> _patchcodeSearchMode;
        public CapWrapper<TWBD> ICAP_PATCHCODESEARCHMODE
        {
            get
            {
                return _patchcodeSearchMode ?? (_patchcodeSearchMode = new CapWrapper<TWBD>(_twain, CAP.ICAP_PATCHCODESEARCHMODE));
            }
        }

        private CapWrapper<uint> _patchCodeMaxRetries;
        public CapWrapper<uint> ICAP_PATCHCODEMAXRETRIES
        {
            get
            {
                return _patchCodeMaxRetries ?? (_patchCodeMaxRetries = new CapWrapper<uint>(_twain, CAP.ICAP_PATCHCODEMAXRETRIES));
            }
        }

        private CapWrapper<uint> _patchCodeTimeout;
        public CapWrapper<uint> ICAP_PATCHCODETIMEOUT
        {
            get
            {
                return _patchCodeTimeout ?? (_patchCodeTimeout = new CapWrapper<uint>(_twain, CAP.ICAP_PATCHCODETIMEOUT));
            }
        }

        private CapWrapper<TWFL> _flashUsed2;
        public CapWrapper<TWFL> ICAP_FLASHUSED2
        {
            get
            {
                return _flashUsed2 ?? (_flashUsed2 = new CapWrapper<TWFL>(_twain, CAP.ICAP_FLASHUSED2));
            }
        }

        private CapWrapper<TWIF> _imgFilter;
        public CapWrapper<TWIF> ICAP_IMAGEFILTER
        {
            get
            {
                return _imgFilter ?? (_imgFilter = new CapWrapper<TWIF>(_twain, CAP.ICAP_IMAGEFILTER));
            }
        }

        private CapWrapper<TWNF> _noiseFilter;
        public CapWrapper<TWNF> ICAP_NOISEFILTER
        {
            get
            {
                return _noiseFilter ?? (_noiseFilter = new CapWrapper<TWNF>(_twain, CAP.ICAP_NOISEFILTER));
            }
        }

        private CapWrapper<TWOV> _overscan;
        public CapWrapper<TWOV> ICAP_OVERSCAN
        {
            get
            {
                return _overscan ?? (_overscan = new CapWrapper<TWOV>(_twain, CAP.ICAP_OVERSCAN));
            }
        }

        private CapWrapper<BoolType> _borderDetect;
        public CapWrapper<BoolType> ICAP_AUTOMATICBORDERDETECTION
        {
            get
            {
                return _borderDetect ?? (_borderDetect = new CapWrapper<BoolType>(_twain, CAP.ICAP_AUTOMATICBORDERDETECTION));

                // this needs to also set undefined size optino
            }
        }

        private CapWrapper<BoolType> _autoDeskew;
        public CapWrapper<BoolType> ICAP_AUTOMATICDESKEW
        {
            get
            {
                return _autoDeskew ?? (_autoDeskew = new CapWrapper<BoolType>(_twain, CAP.ICAP_AUTOMATICDESKEW));
            }
        }

        private CapWrapper<BoolType> _autoRotate;
        public CapWrapper<BoolType> ICAP_AUTOMATICROTATE
        {
            get
            {
                return _autoRotate ?? (_autoRotate = new CapWrapper<BoolType>(_twain, CAP.ICAP_AUTOMATICROTATE));
            }
        }

        private CapWrapper<TWJQ> _jpegQuality;
        public CapWrapper<TWJQ> ICAP_JPEGQUALITY
        {
            get
            {
                //TODO: verify
                return _jpegQuality ?? (_jpegQuality = new CapWrapper<TWJQ>(_twain, CAP.ICAP_JPEGQUALITY));
            }
        }

        private CapWrapper<TWFE> _feederType;
        public CapWrapper<TWFE> ICAP_FEEDERTYPE
        {
            get
            {
                return _feederType ?? (_feederType = new CapWrapper<TWFE>(_twain, CAP.ICAP_FEEDERTYPE));
            }
        }

        private CapWrapper<TWIC> _iccProfile;
        public CapWrapper<TWIC> ICAP_ICCPROFILE
        {
            get
            {
                return _iccProfile ?? (_iccProfile = new CapWrapper<TWIC>(_twain, CAP.ICAP_ICCPROFILE));
            }
        }

        private CapWrapper<TWAS> _autoSize;
        public CapWrapper<TWAS> ICAP_AUTOSIZE
        {
            get
            {
                return _autoSize ?? (_autoSize = new CapWrapper<TWAS>(_twain, CAP.ICAP_AUTOSIZE));
            }
        }

        private CapWrapper<BoolType> _cropUseFrame;
        public CapWrapper<BoolType> ICAP_AUTOMATICCROPUSESFRAME
        {
            get
            {
                return _cropUseFrame ?? (_cropUseFrame = new CapWrapper<BoolType>(_twain, CAP.ICAP_AUTOMATICCROPUSESFRAME));
            }
        }

        private CapWrapper<BoolType> _lengthDetect;
        public CapWrapper<BoolType> ICAP_AUTOMATICLENGTHDETECTION
        {
            get
            {
                return _lengthDetect ?? (_lengthDetect = new CapWrapper<BoolType>(_twain, CAP.ICAP_AUTOMATICLENGTHDETECTION));
            }
        }

        private CapWrapper<BoolType> _autoColor;
        public CapWrapper<BoolType> ICAP_AUTOMATICCOLORENABLED
        {
            get
            {
                return _autoColor ?? (_autoColor = new CapWrapper<BoolType>(_twain, CAP.ICAP_AUTOMATICCOLORENABLED));
            }
        }

        private CapWrapper<TWPT> _autoColorNonPixel;
        public CapWrapper<TWPT> ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE
        {
            get
            {
                return _autoColorNonPixel ?? (_autoColorNonPixel = new CapWrapper<TWPT>(_twain, CAP.ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE));
            }
        }

        private CapWrapper<BoolType> _colorMgmt;
        public CapWrapper<BoolType> ICAP_COLORMANAGEMENTENABLED
        {
            get
            {
                return _colorMgmt ?? (_colorMgmt = new CapWrapper<BoolType>(_twain, CAP.ICAP_COLORMANAGEMENTENABLED));
            }
        }

        private CapWrapper<TWIM> _imgMerge;
        public CapWrapper<TWIM> ICAP_IMAGEMERGE
        {
            get
            {
                return _imgMerge ?? (_imgMerge = new CapWrapper<TWIM>(_twain, CAP.ICAP_IMAGEMERGE));
            }
        }

        private CapWrapper<TW_FIX32> _mergeHeight;
        public CapWrapper<TW_FIX32> ICAP_IMAGEMERGEHEIGHTTHRESHOLD
        {
            get
            {
                return _mergeHeight ?? (_mergeHeight = new CapWrapper<TW_FIX32>(_twain, CAP.ICAP_IMAGEMERGEHEIGHTTHRESHOLD));
            }
        }

        private CapWrapper<TWEI> _supportedExtInfo;
        public CapWrapper<TWEI> ICAP_SUPPORTEDEXTIMAGEINFO
        {
            get
            {
                return _supportedExtInfo ?? (_supportedExtInfo = new CapWrapper<TWEI>(_twain, CAP.ICAP_SUPPORTEDEXTIMAGEINFO));
            }
        }

        private CapWrapper<TWFM> _filmType;
        public CapWrapper<TWFM> ICAP_FILMTYPE
        {
            get
            {
                return _filmType ?? (_filmType = new CapWrapper<TWFM>(_twain, CAP.ICAP_FILMTYPE));
            }
        }

        private CapWrapper<TWMR> _mirror;
        public CapWrapper<TWMR> ICAP_MIRROR
        {
            get
            {
                return _mirror ?? (_mirror = new CapWrapper<TWMR>(_twain, CAP.ICAP_MIRROR));
            }
        }

        private CapWrapper<TWJS> _jpegSubSampling;
        public CapWrapper<TWJS> ICAP_JPEGSUBSAMPLING
        {
            get
            {
                return _jpegSubSampling ?? (_jpegSubSampling = new CapWrapper<TWJS>(_twain, CAP.ICAP_JPEGSUBSAMPLING));
            }
        }

        #endregion

        #region general caps

        #region mandatory

        private CapWrapper<short> _xferCount;
        public CapWrapper<short> CAP_XFERCOUNT
        {
            get
            {
                return _xferCount ?? (_xferCount = new CapWrapper<short>(_twain, CAP.CAP_XFERCOUNT));
            }
        }

        #endregion

        private CapWrapper<TW_STR128> _author;
        public CapWrapper<TW_STR128> CAP_AUTHOR
        {
            get
            {
                return _author ?? (_author = new CapWrapper<TW_STR128>(_twain, CAP.CAP_AUTHOR));
            }
        }


        private CapWrapper<TW_STR255> _caption;
        public CapWrapper<TW_STR255> CAP_CAPTION
        {
            get
            {
                return _caption ?? (_caption = new CapWrapper<TW_STR255>(_twain, CAP.CAP_CAPTION));
            }
        }

        private CapWrapper<BoolType> _feederEnabled;
        public CapWrapper<BoolType> CAP_FEEDERENABLED
        {
            get
            {
                return _feederEnabled ?? (_feederEnabled = new CapWrapper<BoolType>(_twain, CAP.CAP_FEEDERENABLED));
                //value =>
                //    {
                //        var rc = ReturnCode.Failure;

                //        var one = new TWOneValue
                //        {
                //            Item = (uint)value,
                //            ItemType = ItemType.Bool
                //        };

                //        using (TWCapability enabled = new TWCapability(CAP.CapFeederEnabled, one))
                //        {
                //            rc = _source.DGControl.Capability.Set(enabled);
                //        }

                //        // to really use feeder we must also set autofeed or autoscan, but only
                //        // for one of them since setting autoscan also sets autofeed
                //        if (CapAutoScan.CanSet)
                //        {
                //            rc = CapAutoScan.SetValue(value);
                //        }
                //        else if (CapAutoFeed.CanSet)
                //        {
                //            rc = CapAutoFeed.SetValue(value);
                //        }

                //        return rc;
                //    }));
            }
        }

        private CapWrapper<BoolType> _feederLoaded;
        public CapWrapper<BoolType> CAP_FEEDERLOADED
        {
            get
            {
                return _feederLoaded ?? (_feederLoaded = new CapWrapper<BoolType>(_twain, CAP.CAP_FEEDERLOADED));
            }
        }

        private CapWrapper<TW_STR32> _timedate;
        public CapWrapper<TW_STR32> CAP_TIMEDATE
        {
            get
            {
                return _timedate ?? (_timedate = new CapWrapper<TW_STR32>(_twain, CAP.CAP_TIMEDATE));
            }
        }

        private CapWrapper<CAP> _supportedCaps;

        /// <summary>
        /// Gets the supported caps for the current source. This is not supported by all sources.
        /// </summary>
        /// <value>
        /// The supported caps.
        /// </value>
        public CapWrapper<CAP> CAP_SUPPORTEDCAPS
        {
            get
            {
                return _supportedCaps ?? (_supportedCaps = new CapWrapper<CAP>(_twain, CAP.CAP_SUPPORTEDCAPS));
            }
        }

        private CapWrapper<CAP> _extendedCaps;

        /// <summary>
        /// Gets the extended caps for the current source.
        /// </summary>
        /// <value>
        /// The extended caps.
        /// </value>
        public CapWrapper<CAP> CAP_EXTENDEDCAPS
        {
            get
            {
                return _extendedCaps ?? (_extendedCaps = new CapWrapper<CAP>(_twain, CAP.CAP_EXTENDEDCAPS));
            }
        }

        private CapWrapper<BoolType> _autoFeed;
        public CapWrapper<BoolType> CAP_AUTOFEED
        {
            get
            {
                return _autoFeed ?? (_autoFeed = new CapWrapper<BoolType>(_twain, CAP.CAP_AUTOFEED));
            }
        }

        private CapWrapper<BoolType> _clearPage;
        public CapWrapper<BoolType> CAP_CLEARPAGE
        {
            get
            {
                return _clearPage ?? (_clearPage = new CapWrapper<BoolType>(_twain, CAP.CAP_CLEARPAGE));
            }
        }

        private CapWrapper<BoolType> _feedPage;
        public CapWrapper<BoolType> CAP_FEEDPAGE
        {
            get
            {
                return _feedPage ?? (_feedPage = new CapWrapper<BoolType>(_twain, CAP.CAP_FEEDPAGE));
            }
        }

        private CapWrapper<BoolType> _rewindPage;
        public CapWrapper<BoolType> CAP_REWINDPAGE
        {
            get
            {
                return _rewindPage ?? (_rewindPage = new CapWrapper<BoolType>(_twain, CAP.CAP_REWINDPAGE));
            }
        }

        private CapWrapper<BoolType> _indicators;
        public CapWrapper<BoolType> CAP_INDICATORS
        {
            get
            {
                return _indicators ?? (_indicators = new CapWrapper<BoolType>(_twain, CAP.CAP_INDICATORS));
            }
        }

        private CapWrapper<BoolType> _paperDetectable;
        public CapWrapper<BoolType> CAP_PAPERDETECTABLE
        {
            get
            {
                return _paperDetectable ?? (_paperDetectable = new CapWrapper<BoolType>(_twain, CAP.CAP_PAPERDETECTABLE));
            }
        }

        private CapWrapper<BoolType> _uiControllable;
        public CapWrapper<BoolType> CAP_UICONTROLLABLE
        {
            get
            {
                return _uiControllable ?? (_uiControllable = new CapWrapper<BoolType>(_twain, CAP.CAP_UICONTROLLABLE));
            }
        }

        private CapWrapper<BoolType> _devOnline;
        public CapWrapper<BoolType> CAP_DEVICEONLINE
        {
            get
            {
                return _devOnline ?? (_devOnline = new CapWrapper<BoolType>(_twain, CAP.CAP_DEVICEONLINE));
            }
        }

        private CapWrapper<BoolType> _autoScan;
        public CapWrapper<BoolType> CAP_AUTOSCAN
        {
            get
            {
                return _autoScan ?? (_autoScan = new CapWrapper<BoolType>(_twain, CAP.CAP_AUTOSCAN));
            }
        }

        private CapWrapper<BoolType> _thumbsEnabled;
        public CapWrapper<BoolType> CAP_THUMBNAILSENABLED
        {
            get
            {
                return _thumbsEnabled ?? (_thumbsEnabled = new CapWrapper<BoolType>(_twain, CAP.CAP_THUMBNAILSENABLED));
            }
        }

        private CapWrapper<TWDX> _duplex;
        public CapWrapper<TWDX> CAP_DUPLEX
        {
            get
            {
                return _duplex ?? (_duplex = new CapWrapper<TWDX>(_twain, CAP.CAP_DUPLEX));
            }
        }

        private CapWrapper<BoolType> _duplexEnabled;
        public CapWrapper<BoolType> CAP_DUPLEXENABLED
        {
            get
            {
                return _duplexEnabled ?? (_duplexEnabled = new CapWrapper<BoolType>(_twain, CAP.CAP_DUPLEXENABLED));
            }
        }

        private CapWrapper<BoolType> _dsUIonly;

        /// <summary>
        /// Gets the property to see whether device supports UI only flag (no transfer).
        /// </summary>
        /// <value>
        /// The UI only flag.
        /// </value>
        public CapWrapper<BoolType> CAP_ENABLEDSUIONLY
        {
            get
            {
                return _dsUIonly ?? (_dsUIonly = new CapWrapper<BoolType>(_twain, CAP.CAP_ENABLEDSUIONLY));
            }
        }

        private CapWrapper<BoolType> _dsData;

        /// <summary>
        /// Gets the property to see whether device supports custom data triplets.
        /// </summary>
        /// <value>
        /// The custom data flag.
        /// </value>
        public CapWrapper<BoolType> CAP_CUSTOMDSDATA
        {
            get
            {
                return _dsData ?? (_dsData = new CapWrapper<BoolType>(_twain, CAP.CAP_CUSTOMDSDATA));
            }
        }

        private CapWrapper<uint> _endorser;
        public CapWrapper<uint> CAP_ENDORSER
        {
            get
            {
                return _endorser ?? (_endorser = new CapWrapper<uint>(_twain, CAP.CAP_ENDORSER));
            }
        }

        private CapWrapper<TWJC> _jobControl;
        public CapWrapper<TWJC> CAP_JOBCONTROL
        {
            get
            {
                return _jobControl ?? (_jobControl = new CapWrapper<TWJC>(_twain, CAP.CAP_JOBCONTROL));
            }
        }

        private CapWrapper<TWAL> _alarms;
        public CapWrapper<TWAL> CAP_ALARMS
        {
            get
            {
                return _alarms ?? (_alarms = new CapWrapper<TWAL>(_twain, CAP.CAP_ALARMS));
            }
        }

        private CapWrapper<int> _alarmVolume;
        public CapWrapper<int> CAP_ALARMVOLUME
        {
            get
            {
                return _alarmVolume ?? (_alarmVolume = new CapWrapper<int>(_twain, CAP.CAP_ALARMVOLUME));
            }
        }

        private CapWrapper<int> _autoCapture;
        public CapWrapper<int> CAP_AUTOMATICCAPTURE
        {
            get
            {
                return _autoCapture ?? (_autoCapture = new CapWrapper<int>(_twain, CAP.CAP_AUTOMATICCAPTURE));
            }
        }

        private CapWrapper<int> _timeBeforeCap;

        /// <summary>
        /// Gets the property to work with the time before first capture (milliseconds) for the current source.
        /// </summary>
        /// <value>
        /// The time before first capture.
        /// </value>
        public CapWrapper<int> CAP_TIMEBEFOREFIRSTCAPTURE
        {
            get
            {
                return _timeBeforeCap ?? (_timeBeforeCap = new CapWrapper<int>(_twain, CAP.CAP_TIMEBEFOREFIRSTCAPTURE));
            }
        }

        private CapWrapper<int> _timeBetweenCap;

        /// <summary>
        /// Gets the property to work with the time between captures (milliseconds) for the current source.
        /// </summary>
        /// <value>
        /// The time between captures.
        /// </value>
        public CapWrapper<int> CAP_TIMEBETWEENCAPTURES
        {
            get
            {
                return _timeBetweenCap ?? (_timeBetweenCap = new CapWrapper<int>(_twain, CAP.CAP_TIMEBETWEENCAPTURES));
            }
        }

        // deprecated
        //private CapWrapper<TWCB> _clearBuff;
        //public CapWrapper<TWCB> CAP_CLEARBUFFERS
        //{
        //    get
        //    {
        //        return _clearBuff ?? (_clearBuff = new CapWrapper<TWCB>(_twain, CAP.CAP_CLEARBUFFERS));
        //    }
        //}

        private CapWrapper<uint> _maxBatchBuff;

        /// <summary>
        /// Gets the property to work with the max buffered pages for the current source.
        /// </summary>
        /// <value>
        /// The max batch buffered pages.
        /// </value>
        public CapWrapper<uint> CAP_MAXBATCHBUFFERS
        {
            get
            {
                return _maxBatchBuff ?? (_maxBatchBuff = new CapWrapper<uint>(_twain, CAP.CAP_MAXBATCHBUFFERS));
            }
        }

        private CapWrapper<TW_STR32> _devTimeDate;
        public CapWrapper<TW_STR32> CAP_DEVICETIMEDATE
        {
            get
            {
                return _devTimeDate ?? (_devTimeDate = new CapWrapper<TW_STR32>(_twain, CAP.CAP_DEVICETIMEDATE));
            }
        }

        private CapWrapper<TWPS> _powerSup;
        public CapWrapper<TWPS> CAP_POWERSUPPLY
        {
            get
            {
                return _powerSup ?? (_powerSup = new CapWrapper<TWPS>(_twain, CAP.CAP_POWERSUPPLY));
            }
        }

        private CapWrapper<BoolType> _camPreviewUI;
        public CapWrapper<BoolType> CAP_CAMERAPREVIEWUI
        {
            get
            {
                return _camPreviewUI ?? (_camPreviewUI = new CapWrapper<BoolType>(_twain, CAP.CAP_CAMERAPREVIEWUI));
            }
        }

        private CapWrapper<TWDE> _devEvent;

        /// <summary>
        /// Gets the property to work with the reported device events for the current source.
        /// </summary>
        /// <value>
        /// The reported device events.
        /// </value>
        public CapWrapper<TWDE> CAP_DEVICEEVENT
        {
            get
            {
                return _devEvent ?? (_devEvent = new CapWrapper<TWDE>(_twain, CAP.CAP_DEVICEEVENT));
            }
        }

        private CapWrapper<TW_STR255> _serialNo;
        public CapWrapper<TW_STR255> CAP_SERIALNUMBER
        {
            get
            {
                return _serialNo ?? (_serialNo = new CapWrapper<TW_STR255>(_twain, CAP.CAP_SERIALNUMBER));
            }
        }

        private CapWrapper<TWPR> _printer;
        public CapWrapper<TWPR> CAP_PRINTER
        {
            get
            {
                return _printer ?? (_printer = new CapWrapper<TWPR>(_twain, CAP.CAP_PRINTER));
            }
        }

        private CapWrapper<BoolType> _printerEnabled;
        public CapWrapper<BoolType> CAP_PRINTERENABLED
        {
            get
            {
                return _printerEnabled ?? (_printerEnabled = new CapWrapper<BoolType>(_twain, CAP.CAP_PRINTERENABLED));
            }
        }

        private CapWrapper<uint> _printerIndex;
        public CapWrapper<uint> CAP_PRINTERINDEX
        {
            get
            {
                return _printerIndex ?? (_printerIndex = new CapWrapper<uint>(_twain, CAP.CAP_PRINTERINDEX));
            }
        }

        private CapWrapper<TWPM> _printerMode;
        public CapWrapper<TWPM> CAP_PRINTERMODE
        {
            get
            {
                return _printerMode ?? (_printerMode = new CapWrapper<TWPM>(_twain, CAP.CAP_PRINTERMODE));
            }
        }

        private CapWrapper<TW_STR255> _printerString;
        public CapWrapper<TW_STR255> CAP_PRINTERSTRING
        {
            get
            {
                return _printerString ?? (_printerString = new CapWrapper<TW_STR255>(_twain, CAP.CAP_PRINTERSTRING));
            }
        }

        private CapWrapper<TW_STR255> _printerSuffix;
        public CapWrapper<TW_STR255> CAP_PRINTERSUFFIX
        {
            get
            {
                return _printerSuffix ?? (_printerSuffix = new CapWrapper<TW_STR255>(_twain, CAP.CAP_PRINTERSUFFIX));
            }
        }

        private CapWrapper<TWLG> _language;
        public CapWrapper<TWLG> CAP_LANGUAGE
        {
            get
            {
                return _language ?? (_language = new CapWrapper<TWLG>(_twain, CAP.CAP_LANGUAGE));
            }
        }

        private CapWrapper<TWFA> _feedAlign;
        public CapWrapper<TWFA> CAP_FEEDERALIGNMENT
        {
            get
            {
                return _feedAlign ?? (_feedAlign = new CapWrapper<TWFA>(_twain, CAP.CAP_FEEDERALIGNMENT));
            }
        }

        private CapWrapper<TWFO> _feedOrder;
        public CapWrapper<TWFO> CAP_FEEDERORDER
        {
            get
            {
                return _feedOrder ?? (_feedOrder = new CapWrapper<TWFO>(_twain, CAP.CAP_FEEDERORDER));
            }
        }

        private CapWrapper<BoolType> _reacuireAllow;
        public CapWrapper<BoolType> CAP_REACQUIREALLOWED
        {
            get
            {
                return _reacuireAllow ?? (_reacuireAllow = new CapWrapper<BoolType>(_twain, CAP.CAP_REACQUIREALLOWED));
            }
        }

        private CapWrapper<int> _battMinutes;
        public CapWrapper<int> CAP_BATTERYMINUTES
        {
            get
            {
                return _battMinutes ?? (_battMinutes = new CapWrapper<int>(_twain, CAP.CAP_BATTERYMINUTES));
            }
        }

        private CapWrapper<short> _battPercent;
        public CapWrapper<short> CAP_BATTERYPERCENTAGE
        {
            get
            {
                return _battPercent ?? (_battPercent = new CapWrapper<short>(_twain, CAP.CAP_BATTERYPERCENTAGE));
            }
        }

        private CapWrapper<TWCS> _camSide;
        public CapWrapper<TWCS> CAP_CAMERASIDE
        {
            get
            {
                return _camSide ?? (_camSide = new CapWrapper<TWCS>(_twain, CAP.CAP_CAMERASIDE));
            }
        }

        private CapWrapper<TWSG> _segmented;
        public CapWrapper<TWSG> CAP_SEGMENTED
        {
            get
            {
                return _segmented ?? (_segmented = new CapWrapper<TWSG>(_twain, CAP.CAP_SEGMENTED));
            }
        }

        private CapWrapper<BoolType> _camEnabled;
        public CapWrapper<BoolType> CAP_CAMERAENABLED
        {
            get
            {
                return _camEnabled ?? (_camEnabled = new CapWrapper<BoolType>(_twain, CAP.CAP_CAMERAENABLED));
            }
        }

        private CapWrapper<TWPT> _camOrder;
        public CapWrapper<TWPT> CAP_CAMERAORDER
        {
            get
            {
                return _camOrder ?? (_camOrder = new CapWrapper<TWPT>(_twain, CAP.CAP_CAMERAORDER));
            }
        }

        private CapWrapper<BoolType> _micrEnabled;
        public CapWrapper<BoolType> CAP_MICRENABLED
        {
            get
            {
                return _micrEnabled ?? (_micrEnabled = new CapWrapper<BoolType>(_twain, CAP.CAP_MICRENABLED));
            }
        }

        private CapWrapper<BoolType> _feederPrep;
        public CapWrapper<BoolType> CAP_FEEDERPREP
        {
            get
            {
                return _feederPrep ?? (_feederPrep = new CapWrapper<BoolType>(_twain, CAP.CAP_FEEDERPREP));
            }
        }

        private CapWrapper<TWFP> _feedPocket;
        public CapWrapper<TWFP> CAP_FEEDERPOCKET
        {
            get
            {
                return _feedPocket ?? (_feedPocket = new CapWrapper<TWFP>(_twain, CAP.CAP_FEEDERPOCKET));
            }
        }

        private CapWrapper<BoolType> _autoMedium;
        public CapWrapper<BoolType> CAP_AUTOMATICSENSEMEDIUM
        {
            get
            {
                return _autoMedium ?? (_autoMedium = new CapWrapper<BoolType>(_twain, CAP.CAP_AUTOMATICSENSEMEDIUM));
            }
        }

        private CapWrapper<TW_STR255> _custGuid;
        public CapWrapper<TW_STR255> CAP_CUSTOMINTERFACEGUID
        {
            get
            {
                return _custGuid ?? (_custGuid = new CapWrapper<TW_STR255>(_twain, CAP.CAP_CUSTOMINTERFACEGUID));
            }
        }

        private CapWrapper<CAP> _supportedCapsUnique;
        public CapWrapper<CAP> CAP_SUPPORTEDCAPSSEGMENTUNIQUE
        {
            get
            {
                return _supportedCapsUnique ?? (_supportedCapsUnique = new CapWrapper<CAP>(_twain, CAP.CAP_SUPPORTEDCAPSSEGMENTUNIQUE));
            }
        }

        private CapWrapper<uint> _supportedDat;
        public CapWrapper<uint> CAP_SUPPORTEDDATS
        {
            get
            {
                return _supportedDat ?? (_supportedDat = new CapWrapper<uint>(_twain, CAP.CAP_SUPPORTEDDATS));
            }
        }

        private CapWrapper<TWDF> _dblFeedDetect;
        public CapWrapper<TWDF> CAP_DOUBLEFEEDDETECTION
        {
            get
            {
                return _dblFeedDetect ?? (_dblFeedDetect = new CapWrapper<TWDF>(_twain, CAP.CAP_DOUBLEFEEDDETECTION));
            }
        }

        private CapWrapper<TW_FIX32> _dblFeedLength;
        public CapWrapper<TW_FIX32> CAP_DOUBLEFEEDDETECTIONLENGTH
        {
            get
            {
                return _dblFeedLength ?? (_dblFeedLength = new CapWrapper<TW_FIX32>(_twain, CAP.CAP_DOUBLEFEEDDETECTIONLENGTH));
            }
        }

        private CapWrapper<TWUS> _dblFeedSensitivity;
        public CapWrapper<TWUS> CAP_DOUBLEFEEDDETECTIONSENSITIVITY
        {
            get
            {
                return _dblFeedSensitivity ?? (_dblFeedSensitivity = new CapWrapper<TWUS>(_twain, CAP.CAP_DOUBLEFEEDDETECTIONSENSITIVITY));
            }
        }

        private CapWrapper<TWDP> _dblFeedResp;
        public CapWrapper<TWDP> CAP_DOUBLEFEEDDETECTIONRESPONSE
        {
            get
            {
                return _dblFeedResp ?? (_dblFeedResp = new CapWrapper<TWDP>(_twain, CAP.CAP_DOUBLEFEEDDETECTIONRESPONSE));
            }
        }

        private CapWrapper<TWPH> _paperHandling;
        public CapWrapper<TWPH> CAP_PAPERHANDLING
        {
            get
            {
                return _paperHandling ?? (_paperHandling = new CapWrapper<TWPH>(_twain, CAP.CAP_PAPERHANDLING));
            }
        }

        private CapWrapper<TWCI> _indicatorMode;
        public CapWrapper<TWCI> CAP_INDICATORSMODE
        {
            get
            {
                return _indicatorMode ?? (_indicatorMode = new CapWrapper<TWCI>(_twain, CAP.CAP_INDICATORSMODE));
            }
        }

        private CapWrapper<TW_FIX32> _printVOffset;
        public CapWrapper<TW_FIX32> CAP_PRINTERVERTICALOFFSET
        {
            get
            {
                return _printVOffset ?? (_printVOffset = new CapWrapper<TW_FIX32>(_twain, CAP.CAP_PRINTERVERTICALOFFSET));
            }
        }

        private CapWrapper<int> _powerSaveTime;

        /// <summary>
        /// Gets the property to work with camera power down time (seconds) for the current source.
        /// </summary>
        /// <value>
        /// The camera power down time.
        /// </value>
        public CapWrapper<int> CAP_POWERSAVETIME
        {
            get
            {
                return _powerSaveTime ?? (_powerSaveTime = new CapWrapper<int>(_twain, CAP.CAP_POWERSAVETIME));
            }
        }

        private CapWrapper<uint> _printCharRot;
        public CapWrapper<uint> CAP_PRINTERCHARROTATION
        {
            get
            {
                return _printCharRot ?? (_printCharRot = new CapWrapper<uint>(_twain, CAP.CAP_PRINTERCHARROTATION));
            }
        }

        private CapWrapper<TWPFS> _printFontStyle;
        public CapWrapper<TWPFS> CAP_PRINTERFONTSTYLE
        {
            get
            {
                return _printFontStyle ?? (_printFontStyle = new CapWrapper<TWPFS>(_twain, CAP.CAP_PRINTERFONTSTYLE));
            }
        }

        private CapWrapper<TW_STR32> _printerIdxLeadChar;

        /// <summary>
        /// Set the character to be used for filling the leading digits before the counter value if the
        /// counter digits are fewer than <see cref="CAP_PRINTERINDEXNUMDIGITS"/>.
        /// </summary>
        /// <value>
        /// The printer leading string.
        /// </value>
        public CapWrapper<TW_STR32> CAP_PRINTERINDEXLEADCHAR
        {
            get
            {
                return _printerIdxLeadChar ?? (_printerIdxLeadChar = new CapWrapper<TW_STR32>(_twain, CAP.CAP_PRINTERINDEXLEADCHAR));
            }
        }

        private CapWrapper<uint> _printIdxMax;
        public CapWrapper<uint> CAP_PRINTERINDEXMAXVALUE
        {
            get
            {
                return _printIdxMax ?? (_printIdxMax = new CapWrapper<uint>(_twain, CAP.CAP_PRINTERINDEXMAXVALUE));
            }
        }

        private CapWrapper<uint> _printNumDigit;
        public CapWrapper<uint> CAP_PRINTERINDEXNUMDIGITS
        {
            get
            {
                return _printNumDigit ?? (_printNumDigit = new CapWrapper<uint>(_twain, CAP.CAP_PRINTERINDEXNUMDIGITS));
            }
        }

        private CapWrapper<uint> _printIdxStep;
        public CapWrapper<uint> CAP_PRINTERINDEXSTEP
        {
            get
            {
                return _printIdxStep ?? (_printIdxStep = new CapWrapper<uint>(_twain, CAP.CAP_PRINTERINDEXSTEP));
            }
        }

        private CapWrapper<TWCT> _printIdxTrig;
        public CapWrapper<TWCT> CAP_PRINTERINDEXTRIGGER
        {
            get
            {
                return _printIdxTrig ?? (_printIdxTrig = new CapWrapper<TWCT>(_twain, CAP.CAP_PRINTERINDEXTRIGGER));
            }
        }

        private CapWrapper<TW_STR255> _printPreview;
        public CapWrapper<TW_STR255> CAP_PRINTERSTRINGPREVIEW
        {
            get
            {
                return _printPreview ?? (_printPreview = new CapWrapper<TW_STR255>(_twain, CAP.CAP_PRINTERSTRINGPREVIEW));
            }
        }

        private CapWrapper<uint> _sheetCount;
        public CapWrapper<uint> CAP_SHEETCOUNT
        {
            get
            {
                return _sheetCount ?? (_sheetCount = new CapWrapper<uint>(_twain, CAP.CAP_SHEETCOUNT));
            }
        }


        #endregion
    }
}
