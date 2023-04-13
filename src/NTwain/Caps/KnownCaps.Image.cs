using NTwain.Data;

namespace NTwain.Caps
{
  partial class KnownCaps
  {

    CapWriter<TW_BOOL>? _icap_autobright;
    public CapWriter<TW_BOOL> ICAP_AUTOBRIGHT => _icap_autobright ??= new(_twain, CAP.ICAP_AUTOBRIGHT, 1);


    CapWriter<TW_BOOL>? _icap_autodiscardblankpages;
    public CapWriter<TW_BOOL> ICAP_AUTODISCARDBLANKPAGES => _icap_autodiscardblankpages ??= new(_twain, CAP.ICAP_AUTODISCARDBLANKPAGES, 2);


    CapWriter<TW_BOOL>? _icap_automaticborderdetection;
    public CapWriter<TW_BOOL> ICAP_AUTOMATICBORDERDETECTION => _icap_automaticborderdetection ??= new(_twain, CAP.ICAP_AUTOMATICBORDERDETECTION, 1.8f);


    CapWriter<TW_BOOL>? _icap_automaticcolorenabled;
    public CapWriter<TW_BOOL> ICAP_AUTOMATICCOLORENABLED => _icap_automaticcolorenabled ??= new(_twain, CAP.ICAP_AUTOMATICCOLORENABLED, 2.1f);


    CapWriter<TWPT>? _icap_automaticcolornoncolorpixeltype;
    public CapWriter<TWPT> ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE => _icap_automaticcolornoncolorpixeltype ??= new(_twain, CAP.ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE, 2.1f);


    CapReader<TW_BOOL>? _icap_automaticcropusesframe;
    public CapReader<TW_BOOL> ICAP_AUTOMATICCROPUSESFRAME => _icap_automaticcropusesframe ??= new(_twain, CAP.ICAP_AUTOMATICCROPUSESFRAME, 2.1f);


    CapWriter<TW_BOOL>? _icap_automaticdeskew;
    public CapWriter<TW_BOOL> ICAP_AUTOMATICDESKEW => _icap_automaticdeskew ??= new(_twain, CAP.ICAP_AUTOMATICDESKEW, 1.8f);


    CapWriter<TW_BOOL>? _icap_automaticlengthdetection;
    public CapWriter<TW_BOOL> ICAP_AUTOMATICLENGTHDETECTION => _icap_automaticlengthdetection ??= new(_twain, CAP.ICAP_AUTOMATICLENGTHDETECTION, 2.1f);


    CapWriter<TW_BOOL>? _icap_automaticrotate;
    public CapWriter<TW_BOOL> ICAP_AUTOMATICROTATE => _icap_automaticrotate ??= new(_twain, CAP.ICAP_AUTOMATICROTATE, 1.8f);


    CapWriter<TW_BOOL>? _icap_autosize;
    public CapWriter<TW_BOOL> ICAP_AUTOSIZE => _icap_autosize ??= new(_twain, CAP.ICAP_AUTOSIZE, 2);


    CapWriter<TW_BOOL>? _icap_barcodedetectionenabled;
    public CapWriter<TW_BOOL> ICAP_BARCODEDETECTIONENABLED => _icap_barcodedetectionenabled ??= new(_twain, CAP.ICAP_BARCODEDETECTIONENABLED, 1.8f);


    CapWriter<uint>? _icap_barcodemaxretries;
    public CapWriter<uint> ICAP_BARCODEMAXRETRIES => _icap_barcodemaxretries ??= new(_twain, CAP.ICAP_BARCODEMAXRETRIES, 1.8f);


    CapWriter<uint>? _icap_barcodemaxsearchpriorities;
    public CapWriter<uint> ICAP_BARCODEMAXSEARCHPRIORITIES => _icap_barcodemaxsearchpriorities ??= new(_twain, CAP.ICAP_BARCODEMAXSEARCHPRIORITIES, 1.8f);


    CapWriter<TWBD>? _icap_barcodesearchmode;
    public CapWriter<TWBD> ICAP_BARCODESEARCHMODE => _icap_barcodesearchmode ??= new(_twain, CAP.ICAP_BARCODESEARCHMODE, 1.8f);


    CapWriter<TWBT>? _icap_barcodesearchpriorities;
    public CapWriter<TWBT> ICAP_BARCODESEARCHPRIORITIES => _icap_barcodesearchpriorities ??= new(_twain, CAP.ICAP_BARCODESEARCHPRIORITIES, 1.8f);


    CapWriter<uint>? _icap_barcodetimeout;
    public CapWriter<uint> ICAP_BARCODETIMEOUT => _icap_barcodetimeout ??= new(_twain, CAP.ICAP_BARCODETIMEOUT, 1.8f);


    CapWriter<ushort>? _icap_bitdepth;
    public CapWriter<ushort> ICAP_BITDEPTH => _icap_bitdepth ??= new(_twain, CAP.ICAP_BITDEPTH, 1);


    CapWriter<TWBR>? _icap_bitdepthreduction;
    public CapWriter<TWBR> ICAP_BITDEPTHREDUCTION => _icap_bitdepthreduction ??= new(_twain, CAP.ICAP_BITDEPTHREDUCTION, 1.5f);


    CapWriter<TWBO>? _icap_bitorder;
    public CapWriter<TWBO> ICAP_BITORDER => _icap_bitorder ??= new(_twain, CAP.ICAP_BITORDER, 1);


    CapWriter<TWBO>? _icap_bitordercodes;
    public CapWriter<TWBO> ICAP_BITORDERCODES => _icap_bitordercodes ??= new(_twain, CAP.ICAP_BITORDERCODES, 1);


    CapWriter<TW_FIX32>? _icap_brightness;
    public CapWriter<TW_FIX32> ICAP_BRIGHTNESS => _icap_brightness ??= new(_twain, CAP.ICAP_BRIGHTNESS, 1);


    CapWriter<ushort>? _icap_ccittkfactor;
    public CapWriter<ushort> ICAP_CCITTKFACTOR => _icap_ccittkfactor ??= new(_twain, CAP.ICAP_CCITTKFACTOR, 1);


    CapWriter<TW_BOOL>? _icap_colormanagementenabled;
    public CapWriter<TW_BOOL> ICAP_COLORMANAGEMENTENABLED => _icap_colormanagementenabled ??= new(_twain, CAP.ICAP_COLORMANAGEMENTENABLED, 2.1f);


    CapWriter<TWCP>? _icap_compression;
    public CapWriter<TWCP> ICAP_COMPRESSION => _icap_compression ??= new(_twain, CAP.ICAP_COMPRESSION, 1);


    CapWriter<TW_FIX32>? _icap_contrast;
    public CapWriter<TW_FIX32> ICAP_CONTRAST => _icap_contrast ??= new(_twain, CAP.ICAP_CONTRAST, 1);


    CapWriter<byte>? _icap_custhalftone;
    public CapWriter<byte> ICAP_CUSTHALFTONE => _icap_custhalftone ??= new(_twain, CAP.ICAP_CUSTHALFTONE, 1);


    CapWriter<TW_FIX32>? _icap_exposuretime;
    public CapWriter<TW_FIX32> ICAP_EXPOSURETIME => _icap_exposuretime ??= new(_twain, CAP.ICAP_EXPOSURETIME, 1);


    CapWriter<TW_BOOL>? _icap_extimageinfo;
    public CapWriter<TW_BOOL> ICAP_EXTIMAGEINFO => _icap_extimageinfo ??= new(_twain, CAP.ICAP_EXTIMAGEINFO, 1.7f);


    CapWriter<TWFE>? _icap_feedertype;
    public CapWriter<TWFE> ICAP_FEEDERTYPE => _icap_feedertype ??= new(_twain, CAP.ICAP_FEEDERTYPE, 1.91f);


    CapWriter<TWFM>? _icap_filmtype;
    public CapWriter<TWFM> ICAP_FILMTYPE => _icap_filmtype ??= new(_twain, CAP.ICAP_FILMTYPE, 2.2f);


    CapWriter<TWFT>? _icap_filter;
    public CapWriter<TWFT> ICAP_FILTER => _icap_filter ??= new(_twain, CAP.ICAP_FILTER, 1);


    //CapWriter<TWFL>? _icap_flashused;
    //public CapWriter<TWFL> ICAP_FLASHUSED => _icap_flashused ??= new(_twain, CAP.ICAP_FLASHUSED, 1);


    CapWriter<TWFL>? _icap_flashused2;
    public CapWriter<TWFL> ICAP_FLASHUSED2 => _icap_flashused2 ??= new(_twain, CAP.ICAP_FLASHUSED2, 1.8f);


    CapWriter<TWFR>? _icap_fliprotation;
    public CapWriter<TWFR> ICAP_FLIPROTATION => _icap_fliprotation ??= new(_twain, CAP.ICAP_FLIPROTATION, 1.8f);


    CapWriter<TW_FRAME>? _icap_frames;
    public CapWriter<TW_FRAME> ICAP_FRAMES => _icap_frames ??= new(_twain, CAP.ICAP_FRAMES, 1);


    CapWriter<TW_FIX32>? _icap_gamma;
    public CapWriter<TW_FIX32> ICAP_GAMMA => _icap_gamma ??= new(_twain, CAP.ICAP_GAMMA, 1);


    CapWriter<TW_STR32>? _icap_halftones;
    public CapWriter<TW_STR32> ICAP_HALFTONES => _icap_halftones ??= new(_twain, CAP.ICAP_HALFTONES, 1);


    CapWriter<TW_FIX32>? _icap_highlight;
    public CapWriter<TW_FIX32> ICAP_HIGHLIGHT => _icap_highlight ??= new(_twain, CAP.ICAP_HIGHLIGHT, 1);


    CapWriter<TWIC>? _icap_iccprofile;
    public CapWriter<TWIC> ICAP_ICCPROFILE => _icap_iccprofile ??= new(_twain, CAP.ICAP_ICCPROFILE, 1.91f);


    CapWriter<uint>? _icap_imagedataset;
    public CapWriter<uint> ICAP_IMAGEDATASET => _icap_imagedataset ??= new(_twain, CAP.ICAP_IMAGEDATASET, 1.7f);


    CapWriter<TWFF>? _icap_imagefileformat;
    public CapWriter<TWFF> ICAP_IMAGEFILEFORMAT => _icap_imagefileformat ??= new(_twain, CAP.ICAP_IMAGEFILEFORMAT, 1);


    CapWriter<TWIF>? _icap_imagefilter;
    public CapWriter<TWIF> ICAP_IMAGEFILTER => _icap_imagefilter ??= new(_twain, CAP.ICAP_IMAGEFILTER, 1.8f);


    CapWriter<TWIM>? _icap_imagemerge;
    public CapWriter<TWIM> ICAP_IMAGEMERGE => _icap_imagemerge ??= new(_twain, CAP.ICAP_IMAGEMERGE, 2.1f);


    CapWriter<TW_FIX32>? _icap_imagemergeheightthreshold;
    public CapWriter<TW_FIX32> ICAP_IMAGEMERGEHEIGHTTHRESHOLD => _icap_imagemergeheightthreshold ??= new(_twain, CAP.ICAP_IMAGEMERGEHEIGHTTHRESHOLD, 2.1f);


    CapWriter<TWPT>? _icap_jpegpixeltype;
    public CapWriter<TWPT> ICAP_JPEGPIXELTYPE => _icap_jpegpixeltype ??= new(_twain, CAP.ICAP_JPEGPIXELTYPE, 1);


    CapWriter<TWJQ>? _icap_jpegquality;
    public CapWriter<TWJQ> ICAP_JPEGQUALITY => _icap_jpegquality ??= new(_twain, CAP.ICAP_JPEGQUALITY, 1.9f);


    CapWriter<TWJS>? _icap_jpegsubsampling;
    public CapWriter<TWJS> ICAP_JPEGSUBSAMPLING => _icap_jpegsubsampling ??= new(_twain, CAP.ICAP_JPEGSUBSAMPLING, 2.2f);


    CapWriter<TW_BOOL>? _icap_lampstate;
    public CapWriter<TW_BOOL> ICAP_LAMPSTATE => _icap_lampstate ??= new(_twain, CAP.ICAP_LAMPSTATE, 1);


    CapWriter<TWLP>? _icap_lightpath;
    public CapWriter<TWLP> ICAP_LIGHTPATH => _icap_lightpath ??= new(_twain, CAP.ICAP_LIGHTPATH, 1);


    CapWriter<TWLS>? _icap_lightsource;
    public CapWriter<TWLS> ICAP_LIGHTSOURCE => _icap_lightsource ??= new(_twain, CAP.ICAP_LIGHTSOURCE, 1);


    CapWriter<ushort>? _icap_maxframes;
    public CapWriter<ushort> ICAP_MAXFRAMES => _icap_maxframes ??= new(_twain, CAP.ICAP_MAXFRAMES, 1);


    CapReader<TW_FIX32>? _icap_minimumheight;
    public CapReader<TW_FIX32> ICAP_MINIMUMHEIGHT => _icap_minimumheight ??= new(_twain, CAP.ICAP_MINIMUMHEIGHT, 1.7f);


    CapReader<TW_FIX32>? _icap_minimumwidth;
    public CapReader<TW_FIX32> ICAP_MINIMUMWIDTH => _icap_minimumwidth ??= new(_twain, CAP.ICAP_MINIMUMWIDTH, 1.7f);


    CapWriter<TWMR>? _icap_mirror;
    public CapWriter<TWMR> ICAP_MIRROR => _icap_mirror ??= new(_twain, CAP.ICAP_MIRROR, 2.2f);


    CapWriter<TWNF>? _icap_noisefilter;
    public CapWriter<TWNF> ICAP_NOISEFILTER => _icap_noisefilter ??= new(_twain, CAP.ICAP_NOISEFILTER, 1.8f);


    CapWriter<TWOR>? _icap_orientation;
    public CapWriter<TWOR> ICAP_ORIENTATION => _icap_orientation ??= new(_twain, CAP.ICAP_ORIENTATION, 1);


    CapWriter<TWOV>? _icap_overscan;
    public CapWriter<TWOV> ICAP_OVERSCAN => _icap_overscan ??= new(_twain, CAP.ICAP_OVERSCAN, 1.8f);


    CapWriter<TW_BOOL>? _icap_patchcodedetectionenabled;
    public CapWriter<TW_BOOL> ICAP_PATCHCODEDETECTIONENABLED => _icap_patchcodedetectionenabled ??= new(_twain, CAP.ICAP_PATCHCODEDETECTIONENABLED, 1.8f);


    CapWriter<uint>? _icap_patchcodemaxretries;
    public CapWriter<uint> ICAP_PATCHCODEMAXRETRIES => _icap_patchcodemaxretries ??= new(_twain, CAP.ICAP_PATCHCODEMAXRETRIES, 1.8f);


    CapWriter<uint>? _icap_patchcodemaxsearchpriorities;
    public CapWriter<uint> ICAP_PATCHCODEMAXSEARCHPRIORITIES => _icap_patchcodemaxsearchpriorities ??= new(_twain, CAP.ICAP_PATCHCODEMAXSEARCHPRIORITIES, 1.8f);


    CapWriter<TWBD>? _icap_patchcodesearchmode;
    public CapWriter<TWBD> ICAP_PATCHCODESEARCHMODE => _icap_patchcodesearchmode ??= new(_twain, CAP.ICAP_PATCHCODESEARCHMODE, 1.8f);


    CapWriter<TWPCH>? _icap_patchcodesearchpriorities;
    public CapWriter<TWPCH> ICAP_PATCHCODESEARCHPRIORITIES => _icap_patchcodesearchpriorities ??= new(_twain, CAP.ICAP_PATCHCODESEARCHPRIORITIES, 1.8f);


    CapWriter<uint>? _icap_patchcodetimeout;
    public CapWriter<uint> ICAP_PATCHCODETIMEOUT => _icap_patchcodetimeout ??= new(_twain, CAP.ICAP_PATCHCODETIMEOUT, 1.8f);


    CapReader<TW_FIX32>? _icap_physicalheight;
    public CapReader<TW_FIX32> ICAP_PHYSICALHEIGHT => _icap_physicalheight ??= new(_twain, CAP.ICAP_PHYSICALHEIGHT, 1);


    CapReader<TW_FIX32>? _icap_physicalwidth;
    public CapReader<TW_FIX32> ICAP_PHYSICALWIDTH => _icap_physicalwidth ??= new(_twain, CAP.ICAP_PHYSICALWIDTH, 1);


    CapWriter<TWPF>? _icap_pixelflavor;
    public CapWriter<TWPF> ICAP_PIXELFLAVOR => _icap_pixelflavor ??= new(_twain, CAP.ICAP_PIXELFLAVOR, 1);


    CapWriter<TWPF>? _icap_pixelflavorcodes;
    public CapWriter<TWPF> ICAP_PIXELFLAVORCODES => _icap_pixelflavorcodes ??= new(_twain, CAP.ICAP_PIXELFLAVORCODES, 1);


    CapWriter<TWPT>? _icap_pixeltype;
    public CapWriter<TWPT> ICAP_PIXELTYPE => _icap_pixeltype ??= new(_twain, CAP.ICAP_PIXELTYPE, 1);


    CapWriter<TWPC>? _icap_planarchunky;
    public CapWriter<TWPC> ICAP_PLANARCHUNKY => _icap_planarchunky ??= new(_twain, CAP.ICAP_PLANARCHUNKY, 1);


    CapWriter<TW_FIX32>? _icap_rotation;
    public CapWriter<TW_FIX32> ICAP_ROTATION => _icap_rotation ??= new(_twain, CAP.ICAP_ROTATION, 1);


    CapWriter<TW_FIX32>? _icap_shadow;
    public CapWriter<TW_FIX32> ICAP_SHADOW => _icap_shadow ??= new(_twain, CAP.ICAP_SHADOW, 1);


    CapReader<TWBT>? _icap_supportedbarcodetypes;
    public CapReader<TWBT> ICAP_SUPPORTEDBARCODETYPES => _icap_supportedbarcodetypes ??= new(_twain, CAP.ICAP_SUPPORTEDBARCODETYPES, 1.8f);


    CapReader<TWEI>? _icap_supportedextimageinfo;
    public CapReader<TWEI> ICAP_SUPPORTEDEXTIMAGEINFO => _icap_supportedextimageinfo ??= new(_twain, CAP.ICAP_SUPPORTEDEXTIMAGEINFO, 2.1f);


    CapReader<TWPCH>? _icap_supportedpatchcodetypes;
    public CapReader<TWPCH> ICAP_SUPPORTEDPATCHCODETYPES => _icap_supportedpatchcodetypes ??= new(_twain, CAP.ICAP_SUPPORTEDPATCHCODETYPES, 1.8f);


    CapWriter<TWSS>? _icap_supportedsizes;
    public CapWriter<TWSS> ICAP_SUPPORTEDSIZES => _icap_supportedsizes ??= new(_twain, CAP.ICAP_SUPPORTEDSIZES, 1);


    CapWriter<TW_FIX32>? _icap_threshold;
    public CapWriter<TW_FIX32> ICAP_THRESHOLD => _icap_threshold ??= new(_twain, CAP.ICAP_THRESHOLD, 1);


    CapWriter<TW_BOOL>? _icap_tiles;
    public CapWriter<TW_BOOL> ICAP_TILES => _icap_tiles ??= new(_twain, CAP.ICAP_TILES, 1);


    CapWriter<ushort>? _icap_timefill;
    public CapWriter<ushort> ICAP_TIMEFILL => _icap_timefill ??= new(_twain, CAP.ICAP_TIMEFILL, 1);


    CapWriter<TW_BOOL>? _icap_undefinedimagesize;
    public CapWriter<TW_BOOL> ICAP_UNDEFINEDIMAGESIZE => _icap_undefinedimagesize ??= new(_twain, CAP.ICAP_UNDEFINEDIMAGESIZE, 1.6f);


    CapWriter<TWUN>? _icap_units;
    public CapWriter<TWUN> ICAP_UNITS => _icap_units ??= new(_twain, CAP.ICAP_UNITS, 1);


    CapWriter<TWSX>? _icap_xfermech;
    public CapWriter<TWSX> ICAP_XFERMECH => _icap_xfermech ??= new(_twain, CAP.ICAP_XFERMECH, 1);


    CapReader<TW_FIX32>? _icap_xnativeresolution;
    public CapReader<TW_FIX32> ICAP_XNATIVERESOLUTION => _icap_xnativeresolution ??= new(_twain, CAP.ICAP_XNATIVERESOLUTION, 1);


    CapWriter<TW_FIX32>? _icap_xresolution;
    public CapWriter<TW_FIX32> ICAP_XRESOLUTION => _icap_xresolution ??= new(_twain, CAP.ICAP_XRESOLUTION, 1);


    CapWriter<TW_FIX32>? _icap_xscaling;
    public CapWriter<TW_FIX32> ICAP_XSCALING => _icap_xscaling ??= new(_twain, CAP.ICAP_XSCALING, 1);


    CapReader<TW_FIX32>? _icap_ynativeresolution;
    public CapReader<TW_FIX32> ICAP_YNATIVERESOLUTION => _icap_ynativeresolution ??= new(_twain, CAP.ICAP_YNATIVERESOLUTION, 1);


    CapWriter<TW_FIX32>? _icap_yresolution;
    public CapWriter<TW_FIX32> ICAP_YRESOLUTION => _icap_yresolution ??= new(_twain, CAP.ICAP_YRESOLUTION, 1);


    CapWriter<TW_FIX32>? _icap_yscaling;
    public CapWriter<TW_FIX32> ICAP_YSCALING => _icap_yscaling ??= new(_twain, CAP.ICAP_YSCALING, 1);


    CapWriter<short>? _icap_zoomfactor;
    public CapWriter<short> ICAP_ZOOMFACTOR => _icap_zoomfactor ??= new(_twain, CAP.ICAP_ZOOMFACTOR, 1.8f);

  }
}
