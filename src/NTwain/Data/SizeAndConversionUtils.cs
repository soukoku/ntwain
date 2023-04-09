using System;
using System.Collections.Generic;

namespace NTwain.Data
{

  public static class SizeAndConversionUtils
  {
    /// <summary>
    /// Maps <see cref="CAP"/> id with its enum 
    /// value type when applicable 
    /// (e.g. <see cref="CAP.ICAP_XFERMECH"/> to <see cref="TWSX"/>).
    /// </summary>
    /// <param name="cap"></param>
    /// <returns></returns>
    public static Type? GetEnumType(CAP cap)
    {
      if (__map.ContainsKey(cap)) return __map[cap];
      return null;
    }
        static readonly Dictionary<CAP, Type> __map = new()
    {
      { CAP.ACAP_XFERMECH, typeof(TWSX) },
      { CAP.CAP_ALARMS, typeof(TWAL) },
      { CAP.CAP_AUTOFEED, typeof(TW_BOOL) },
      { CAP.CAP_AUTOMATICSENSEMEDIUM, typeof(TW_BOOL) },
      { CAP.CAP_AUTOSCAN, typeof(TW_BOOL) },
      { CAP.CAP_CAMERAENABLED, typeof(TW_BOOL) },
      { CAP.CAP_CAMERAPREVIEWUI, typeof(TW_BOOL) },
      { CAP.CAP_CAMERASIDE, typeof(TWCS) },

      { CAP.CAP_CLEARPAGE, typeof(TW_BOOL) },
      { CAP.CAP_CUSTOMDSDATA, typeof(TW_BOOL) },
      { CAP.CAP_DEVICEEVENT, typeof(TWDE) },
      { CAP.CAP_DEVICEONLINE, typeof(TW_BOOL) },

      { CAP.CAP_DOUBLEFEEDDETECTION, typeof(TWDF) },
      { CAP.CAP_DOUBLEFEEDDETECTIONRESPONSE, typeof(TWDP) },
      { CAP.CAP_DOUBLEFEEDDETECTIONSENSITIVITY, typeof(TWUS) },

      { CAP.CAP_DUPLEX, typeof(TWDX) },
      { CAP.CAP_DUPLEXENABLED, typeof(TW_BOOL) },
      { CAP.CAP_ENABLEDSUIONLY, typeof(TW_BOOL) },
      { CAP.CAP_EXTENDEDCAPS, typeof(CAP) },

      { CAP.CAP_FEEDERALIGNMENT, typeof(TWFA) },
      { CAP.CAP_FEEDERENABLED, typeof(TW_BOOL) },
      { CAP.CAP_FEEDERLOADED, typeof(TW_BOOL) },
      { CAP.CAP_FEEDERORDER, typeof(TWFO) },
      { CAP.CAP_FEEDERPOCKET, typeof(TWFP) },
      { CAP.CAP_FEEDERPREP, typeof(TW_BOOL) },
      { CAP.CAP_FEEDPAGE, typeof(TW_BOOL) },

      { CAP.CAP_IAFIELDA_LEVEL, typeof(TWIA) },
      { CAP.CAP_IAFIELDB_LEVEL, typeof(TWIA) },
      { CAP.CAP_IAFIELDC_LEVEL, typeof(TWIA) },
      { CAP.CAP_IAFIELDD_LEVEL, typeof(TWIA) },
      { CAP.CAP_IAFIELDE_LEVEL, typeof(TWIA) },


      { CAP.CAP_IMAGEADDRESSENABLED, typeof(TW_BOOL) },
      { CAP.CAP_INDICATORS, typeof(TW_BOOL) },
      { CAP.CAP_INDICATORSMODE, typeof(TWCI) },
      { CAP.CAP_JOBCONTROL, typeof(TWJC) },
      { CAP.CAP_LANGUAGE, typeof(TWLG) },

      { CAP.CAP_MICRENABLED, typeof(TW_BOOL) },
      { CAP.CAP_PAPERDETECTABLE, typeof(TW_BOOL) },
      { CAP.CAP_PAPERHANDLING, typeof(TWPH) },
      { CAP.CAP_POWERSUPPLY, typeof(TWPS) },

      { CAP.CAP_PRINTER, typeof(TWPR) },
      { CAP.CAP_PRINTERENABLED, typeof(TW_BOOL) },
      { CAP.CAP_PRINTERFONTSTYLE, typeof(TWPF) },
      { CAP.CAP_PRINTERINDEXTRIGGER, typeof(TWCT) },
      { CAP.CAP_PRINTERMODE, typeof(TWPM) },

      { CAP.CAP_REACQUIREALLOWED, typeof(TW_BOOL) },
      { CAP.CAP_REWINDPAGE, typeof(TW_BOOL) },
      { CAP.CAP_SEGMENTED, typeof(TWSG) },

      { CAP.CAP_SUPPORTEDCAPS, typeof(CAP) },
      { CAP.CAP_SUPPORTEDCAPSSEGMENTUNIQUE, typeof(CAP) },
      { CAP.CAP_SUPPORTEDDATS, typeof(DAT) },

      { CAP.CAP_THUMBNAILSENABLED, typeof(TW_BOOL) },

      { CAP.CAP_UICONTROLLABLE, typeof(TW_BOOL) },

      { CAP.ICAP_AUTOBRIGHT, typeof(TW_BOOL) },
      { CAP.ICAP_AUTODISCARDBLANKPAGES, typeof(TWBP) },
      { CAP.ICAP_AUTOMATICBORDERDETECTION, typeof(TW_BOOL) },
      { CAP.ICAP_AUTOMATICCOLORENABLED, typeof(TW_BOOL) },
      { CAP.ICAP_AUTOMATICCOLORNONCOLORPIXELTYPE, typeof(TWPT) },
      { CAP.ICAP_AUTOMATICCROPUSESFRAME, typeof(TW_BOOL) },
      { CAP.ICAP_AUTOMATICDESKEW, typeof(TW_BOOL) },
      { CAP.ICAP_AUTOMATICLENGTHDETECTION, typeof(TW_BOOL) },
      { CAP.ICAP_AUTOMATICROTATE, typeof(TW_BOOL) },
      { CAP.ICAP_AUTOSIZE, typeof(TWAS) },

      { CAP.ICAP_BARCODEDETECTIONENABLED, typeof(TW_BOOL) },
      { CAP.ICAP_BARCODESEARCHMODE, typeof(TWBD) },
      { CAP.ICAP_BARCODESEARCHPRIORITIES, typeof(TWBT) },

      { CAP.ICAP_BITDEPTHREDUCTION, typeof(TWBR) },
      { CAP.ICAP_BITORDER, typeof(TWBO) },
      { CAP.ICAP_BITORDERCODES, typeof(TWBO) },

      { CAP.ICAP_COLORMANAGEMENTENABLED, typeof(TW_BOOL) },
      { CAP.ICAP_COMPRESSION, typeof(TWCP) },

      { CAP.ICAP_EXTIMAGEINFO, typeof(TW_BOOL) },
      { CAP.ICAP_FEEDERTYPE, typeof(TWFE) },
      { CAP.ICAP_FILMTYPE, typeof(TWFM) },
      { CAP.ICAP_FILTER, typeof(TWFT) },
      { CAP.ICAP_FLASHUSED2, typeof(TWFL) },
      { CAP.ICAP_FLIPROTATION, typeof(TWFR) },
      { CAP.ICAP_ICCPROFILE, typeof(TWIC) },

      { CAP.ICAP_IMAGEFILEFORMAT, typeof(TWFF) },
      { CAP.ICAP_IMAGEFILTER, typeof(TWIF) },
      { CAP.ICAP_IMAGEMERGE, typeof(TWIM) },

      { CAP.ICAP_JPEGPIXELTYPE, typeof(TWPT) },
      { CAP.ICAP_JPEGQUALITY, typeof(TWJQ) },
      { CAP.ICAP_JPEGSUBSAMPLING, typeof(TWJS) },

      { CAP.ICAP_LAMPSTATE, typeof(TW_BOOL) },
      { CAP.ICAP_LIGHTPATH, typeof(TWLP) },
      { CAP.ICAP_LIGHTSOURCE, typeof(TWLS) },

      { CAP.ICAP_MIRROR, typeof(TWMR) },
      { CAP.ICAP_NOISEFILTER, typeof(TWNF) },
      { CAP.ICAP_ORIENTATION, typeof(TWOR) },
      { CAP.ICAP_OVERSCAN, typeof(TWOV) },

      { CAP.ICAP_PATCHCODEDETECTIONENABLED, typeof(TW_BOOL) },
      { CAP.ICAP_PATCHCODESEARCHMODE, typeof(TWBD) },
      { CAP.ICAP_PATCHCODESEARCHPRIORITIES, typeof(TWPCH) },

      { CAP.ICAP_PIXELFLAVOR, typeof(TWPF) },
      { CAP.ICAP_PIXELFLAVORCODES, typeof(TWPF) },
      { CAP.ICAP_PIXELTYPE, typeof(TWPT) },
      { CAP.ICAP_PLANARCHUNKY, typeof(TWPC) },

      { CAP.ICAP_SUPPORTEDBARCODETYPES, typeof(TWBT) },
      { CAP.ICAP_SUPPORTEDEXTIMAGEINFO, typeof(TWEI) },
      { CAP.ICAP_SUPPORTEDPATCHCODETYPES, typeof(TWPCH) },
      { CAP.ICAP_SUPPORTEDSIZES, typeof(TWSS) },

      { CAP.ICAP_TILES, typeof(TW_BOOL) },
      { CAP.ICAP_UNDEFINEDIMAGESIZE, typeof(TW_BOOL) },
      { CAP.ICAP_UNITS, typeof(TWUN) },
      { CAP.ICAP_XFERMECH, typeof(TWSX) },
    };


    /// <summary>
    /// Gets the byte size of the TWAIN item value type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static int GetItemTypeSize(this TWTY type)
    {
      if (__sizes.TryGetValue(type, out int size))
      {
        return size;
      }
      return 0;
    }
    static readonly IDictionary<TWTY, int> __sizes = new Dictionary<TWTY, int>
    {
      {TWTY.INT8, 1},
      {TWTY.UINT8, 1},
      {TWTY.INT16, 2},
      {TWTY.UINT16, 2},
      {TWTY.BOOL, 2},
      {TWTY.INT32, 4},
      {TWTY.UINT32, 4},
      {TWTY.FIX32, 4},
      {TWTY.FRAME, 16},
      {TWTY.STR32, TW_STR32.Size},
      {TWTY.STR64, TW_STR64.Size},
      {TWTY.STR128, TW_STR128.Size},
      {TWTY.STR255, TW_STR255.Size},
      // is it fixed 4 bytes or intptr size?
      {TWTY.HANDLE, IntPtr.Size},
    };
  }
}
