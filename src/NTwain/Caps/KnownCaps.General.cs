using NTwain.Data;

namespace NTwain.Caps
{
  partial class KnownCaps
  {

    CapWriter<TWAL>? _cap_alarms;
    public CapWriter<TWAL> CAP_ALARMS => _cap_alarms ??= new(_twain, CAP.CAP_ALARMS, 1.8f);


    CapWriter<int>? _cap_alarmvolume;
    public CapWriter<int> CAP_ALARMVOLUME => _cap_alarmvolume ??= new(_twain, CAP.CAP_ALARMVOLUME, 1.8f);


    CapWriter<TW_STR128>? _cap_author;
    public CapWriter<TW_STR128> CAP_AUTHOR => _cap_author ??= new(_twain, CAP.CAP_AUTHOR, 1);


    CapWriter<TW_BOOL>? _cap_autofeed;
    public CapWriter<TW_BOOL> CAP_AUTOFEED => _cap_autofeed ??= new(_twain, CAP.CAP_AUTOFEED, 1);


    CapWriter<int>? _cap_automaticcapture;
    public CapWriter<int> CAP_AUTOMATICCAPTURE => _cap_automaticcapture ??= new(_twain, CAP.CAP_AUTOMATICCAPTURE, 1.8f);


    CapWriter<TW_BOOL>? _cap_automaticsensemedium;
    public CapWriter<TW_BOOL> CAP_AUTOMATICSENSEMEDIUM => _cap_automaticsensemedium ??= new(_twain, CAP.CAP_AUTOMATICSENSEMEDIUM, 2.1f);


    CapWriter<TW_BOOL>? _cap_autoscan;
    public CapWriter<TW_BOOL> CAP_AUTOSCAN => _cap_autoscan ??= new(_twain, CAP.CAP_AUTOSCAN, 1.6f);


    CapReader<int>? _cap_batteryminutes;
    public CapReader<int> CAP_BATTERYMINUTES => _cap_batteryminutes ??= new(_twain, CAP.CAP_BATTERYMINUTES, 1.8f);


    CapReader<short>? _cap_batterypercentage;
    public CapReader<short> CAP_BATTERYPERCENTAGE => _cap_batterypercentage ??= new(_twain, CAP.CAP_BATTERYPERCENTAGE, 1.8f);


    CapWriter<TW_BOOL>? _cap_cameraenabled;
    public CapWriter<TW_BOOL> CAP_CAMERAENABLED => _cap_cameraenabled ??= new(_twain, CAP.CAP_CAMERAENABLED, 2);


    CapWriter<ushort>? _cap_cameraorder;
    public CapWriter<ushort> CAP_CAMERAORDER => _cap_cameraorder ??= new(_twain, CAP.CAP_CAMERAORDER, 2);


    CapReader<TW_BOOL>? _cap_camerapreviewui;
    public CapReader<TW_BOOL> CAP_CAMERAPREVIEWUI => _cap_camerapreviewui ??= new(_twain, CAP.CAP_CAMERAPREVIEWUI, 1.8f);


    CapWriter<TWCS>? _cap_cameraside;
    public CapWriter<TWCS> CAP_CAMERASIDE => _cap_cameraside ??= new(_twain, CAP.CAP_CAMERASIDE, 1.91f);


    CapWriter<TW_STR255>? _cap_caption;
    public CapWriter<TW_STR255> CAP_CAPTION => _cap_caption ??= new(_twain, CAP.CAP_CAPTION, 1);


    CapWriter<TW_BOOL>? _cap_clearpage;
    public CapWriter<TW_BOOL> CAP_CLEARPAGE => _cap_clearpage ??= new(_twain, CAP.CAP_CLEARPAGE, 1);


    CapReader<TW_BOOL>? _cap_customdsdata;
    public CapReader<TW_BOOL> CAP_CUSTOMDSDATA => _cap_customdsdata ??= new(_twain, CAP.CAP_CUSTOMDSDATA, 1.7f);


    CapReader<TW_STR255>? _cap_custominterfaceguid;
    public CapReader<TW_STR255> CAP_CUSTOMINTERFACEGUID => _cap_custominterfaceguid ??= new(_twain, CAP.CAP_CUSTOMINTERFACEGUID, 2.1f);


    CapWriter<TWDE>? _cap_deviceevent;
    public CapWriter<TWDE> CAP_DEVICEEVENT => _cap_deviceevent ??= new(_twain, CAP.CAP_DEVICEEVENT, 1.8f);


    CapReader<TW_BOOL>? _cap_deviceonline;
    public CapReader<TW_BOOL> CAP_DEVICEONLINE => _cap_deviceonline ??= new(_twain, CAP.CAP_DEVICEONLINE, 1.6f);


    CapWriter<TW_STR32>? _cap_devicetimedate;
    public CapWriter<TW_STR32> CAP_DEVICETIMEDATE => _cap_devicetimedate ??= new(_twain, CAP.CAP_DEVICETIMEDATE, 1.8f);


    CapWriter<TWDF>? _cap_doublefeeddetection;
    public CapWriter<TWDF> CAP_DOUBLEFEEDDETECTION => _cap_doublefeeddetection ??= new(_twain, CAP.CAP_DOUBLEFEEDDETECTION, 2.2f);


    CapWriter<TW_FIX32>? _cap_doublefeeddetectionlength;
    public CapWriter<TW_FIX32> CAP_DOUBLEFEEDDETECTIONLENGTH => _cap_doublefeeddetectionlength ??= new(_twain, CAP.CAP_DOUBLEFEEDDETECTIONLENGTH, 2.2f);


    CapWriter<TWDP>? _cap_doublefeeddetectionresponse;
    public CapWriter<TWDP> CAP_DOUBLEFEEDDETECTIONRESPONSE => _cap_doublefeeddetectionresponse ??= new(_twain, CAP.CAP_DOUBLEFEEDDETECTIONRESPONSE, 2.2f);


    CapWriter<TWUS>? _cap_doublefeeddetectionsensitivity;
    public CapWriter<TWUS> CAP_DOUBLEFEEDDETECTIONSENSITIVITY => _cap_doublefeeddetectionsensitivity ??= new(_twain, CAP.CAP_DOUBLEFEEDDETECTIONSENSITIVITY, 2.2f);


    CapReader<TWDX>? _cap_duplex;
    public CapReader<TWDX> CAP_DUPLEX => _cap_duplex ??= new(_twain, CAP.CAP_DUPLEX, 1.7f);


    CapWriter<TW_BOOL>? _cap_duplexenabled;
    public CapWriter<TW_BOOL> CAP_DUPLEXENABLED => _cap_duplexenabled ??= new(_twain, CAP.CAP_DUPLEXENABLED, 1.7f);


    CapReader<TW_BOOL>? _cap_enabledsuionly;
    public CapReader<TW_BOOL> CAP_ENABLEDSUIONLY => _cap_enabledsuionly ??= new(_twain, CAP.CAP_ENABLEDSUIONLY, 1.7f);


    CapWriter<uint>? _cap_endorser;
    public CapWriter<uint> CAP_ENDORSER => _cap_endorser ??= new(_twain, CAP.CAP_ENDORSER, 1.7f);


    CapWriter<CAP>? _cap_extendedcaps;
    public CapWriter<CAP> CAP_EXTENDEDCAPS => _cap_extendedcaps ??= new(_twain, CAP.CAP_EXTENDEDCAPS, 1);


    CapWriter<TWFA>? _cap_feederalignment;
    public CapWriter<TWFA> CAP_FEEDERALIGNMENT => _cap_feederalignment ??= new(_twain, CAP.CAP_FEEDERALIGNMENT, 1.8f);


    CapWriter<TW_BOOL>? _cap_feederenabled;
    public CapWriter<TW_BOOL> CAP_FEEDERENABLED => _cap_feederenabled ??= new(_twain, CAP.CAP_FEEDERENABLED, 1);


    CapReader<TW_BOOL>? _cap_feederloaded;
    public CapReader<TW_BOOL> CAP_FEEDERLOADED => _cap_feederloaded ??= new(_twain, CAP.CAP_FEEDERLOADED, 1);


    CapWriter<TWFO>? _cap_feederorder;
    public CapWriter<TWFO> CAP_FEEDERORDER => _cap_feederorder ??= new(_twain, CAP.CAP_FEEDERORDER, 1.8f);


    CapWriter<TWFP>? _cap_feederpocket;
    public CapWriter<TWFP> CAP_FEEDERPOCKET => _cap_feederpocket ??= new(_twain, CAP.CAP_FEEDERPOCKET, 2);


    CapWriter<TW_BOOL>? _cap_feederprep;
    public CapWriter<TW_BOOL> CAP_FEEDERPREP => _cap_feederprep ??= new(_twain, CAP.CAP_FEEDERPREP, 2);


    CapWriter<TW_BOOL>? _cap_feedpage;
    public CapWriter<TW_BOOL> CAP_FEEDPAGE => _cap_feedpage ??= new(_twain, CAP.CAP_FEEDPAGE, 1);


    CapReader<TW_STR32>? _cap_iafielda_lastpage;
    public CapReader<TW_STR32> CAP_IAFIELDA_LASTPAGE => _cap_iafielda_lastpage ??= new(_twain, CAP.CAP_IAFIELDA_LASTPAGE, 2.5f);


    CapWriter<TWIA>? _cap_iafielda_level;
    public CapWriter<TWIA> CAP_IAFIELDA_LEVEL => _cap_iafielda_level ??= new(_twain, CAP.CAP_IAFIELDA_LEVEL, 2.5f);


    CapWriter<TW_STR32>? _cap_iafielda_printformat;
    public CapWriter<TW_STR32> CAP_IAFIELDA_PRINTFORMAT => _cap_iafielda_printformat ??= new(_twain, CAP.CAP_IAFIELDA_PRINTFORMAT, 2.5f);


    CapWriter<TW_STR32>? _cap_iafielda_value;
    public CapWriter<TW_STR32> CAP_IAFIELDA_VALUE => _cap_iafielda_value ??= new(_twain, CAP.CAP_IAFIELDA_VALUE, 2.5f);


    CapReader<TW_STR32>? _cap_iafieldb_lastpage;
    public CapReader<TW_STR32> CAP_IAFIELDB_LASTPAGE => _cap_iafieldb_lastpage ??= new(_twain, CAP.CAP_IAFIELDB_LASTPAGE, 2.5f);


    CapWriter<TWIA>? _cap_iafieldb_level;
    public CapWriter<TWIA> CAP_IAFIELDB_LEVEL => _cap_iafieldb_level ??= new(_twain, CAP.CAP_IAFIELDB_LEVEL, 2.5f);


    CapWriter<TW_STR32>? _cap_iafieldb_printformat;
    public CapWriter<TW_STR32> CAP_IAFIELDB_PRINTFORMAT => _cap_iafieldb_printformat ??= new(_twain, CAP.CAP_IAFIELDB_PRINTFORMAT, 2.5f);


    CapWriter<TW_STR32>? _cap_iafieldb_value;
    public CapWriter<TW_STR32> CAP_IAFIELDB_VALUE => _cap_iafieldb_value ??= new(_twain, CAP.CAP_IAFIELDB_VALUE, 2.5f);


    CapReader<TW_STR32>? _cap_iafieldc_lastpage;
    public CapReader<TW_STR32> CAP_IAFIELDC_LASTPAGE => _cap_iafieldc_lastpage ??= new(_twain, CAP.CAP_IAFIELDC_LASTPAGE, 2.5f);


    CapWriter<TWIA>? _cap_iafieldc_level;
    public CapWriter<TWIA> CAP_IAFIELDC_LEVEL => _cap_iafieldc_level ??= new(_twain, CAP.CAP_IAFIELDC_LEVEL, 2.5f);


    CapWriter<TW_STR32>? _cap_iafieldc_printformat;
    public CapWriter<TW_STR32> CAP_IAFIELDC_PRINTFORMAT => _cap_iafieldc_printformat ??= new(_twain, CAP.CAP_IAFIELDC_PRINTFORMAT, 2.5f);


    CapWriter<TW_STR32>? _cap_iafieldc_value;
    public CapWriter<TW_STR32> CAP_IAFIELDC_VALUE => _cap_iafieldc_value ??= new(_twain, CAP.CAP_IAFIELDC_VALUE, 2.5f);


    CapReader<TW_STR32>? _cap_iafieldd_lastpage;
    public CapReader<TW_STR32> CAP_IAFIELDD_LASTPAGE => _cap_iafieldd_lastpage ??= new(_twain, CAP.CAP_IAFIELDD_LASTPAGE, 2.5f);


    CapWriter<TWIA>? _cap_iafieldd_level;
    public CapWriter<TWIA> CAP_IAFIELDD_LEVEL => _cap_iafieldd_level ??= new(_twain, CAP.CAP_IAFIELDD_LEVEL, 2.5f);


    CapWriter<TW_STR32>? _cap_iafieldd_printformat;
    public CapWriter<TW_STR32> CAP_IAFIELDD_PRINTFORMAT => _cap_iafieldd_printformat ??= new(_twain, CAP.CAP_IAFIELDD_PRINTFORMAT, 2.5f);


    CapWriter<TW_STR32>? _cap_iafieldd_value;
    public CapWriter<TW_STR32> CAP_IAFIELDD_VALUE => _cap_iafieldd_value ??= new(_twain, CAP.CAP_IAFIELDD_VALUE, 2.5f);


    CapReader<TW_STR32>? _cap_iafielde_lastpage;
    public CapReader<TW_STR32> CAP_IAFIELDE_LASTPAGE => _cap_iafielde_lastpage ??= new(_twain, CAP.CAP_IAFIELDE_LASTPAGE, 2.5f);


    CapWriter<TWIA>? _cap_iafielde_level;
    public CapWriter<TWIA> CAP_IAFIELDE_LEVEL => _cap_iafielde_level ??= new(_twain, CAP.CAP_IAFIELDE_LEVEL, 2.5f);


    CapWriter<TW_STR32>? _cap_iafielde_printformat;
    public CapWriter<TW_STR32> CAP_IAFIELDE_PRINTFORMAT => _cap_iafielde_printformat ??= new(_twain, CAP.CAP_IAFIELDE_PRINTFORMAT, 2.5f);


    CapWriter<TW_STR32>? _cap_iafielde_value;
    public CapWriter<TW_STR32> CAP_IAFIELDE_VALUE => _cap_iafielde_value ??= new(_twain, CAP.CAP_IAFIELDE_VALUE, 2.5f);


    CapWriter<TW_BOOL>? _cap_imageaddressenabled;
    public CapWriter<TW_BOOL> CAP_IMAGEADDRESSENABLED => _cap_imageaddressenabled ??= new(_twain, CAP.CAP_IMAGEADDRESSENABLED, 2.5f);


    CapWriter<TW_BOOL>? _cap_indicators;
    public CapWriter<TW_BOOL> CAP_INDICATORS => _cap_indicators ??= new(_twain, CAP.CAP_INDICATORS, 1.1f);


    CapWriter<TWCI>? _cap_indicatorsmode;
    public CapWriter<TWCI> CAP_INDICATORSMODE => _cap_indicatorsmode ??= new(_twain, CAP.CAP_INDICATORSMODE, 2.2f);


    CapWriter<TWJC>? _cap_jobcontrol;
    public CapWriter<TWJC> CAP_JOBCONTROL => _cap_jobcontrol ??= new(_twain, CAP.CAP_JOBCONTROL, 1.7f);


    CapWriter<TWLG>? _cap_language;
    public CapWriter<TWLG> CAP_LANGUAGE => _cap_language ??= new(_twain, CAP.CAP_LANGUAGE, 1.8f);


    CapWriter<uint>? _cap_maxbatchbuffers;
    public CapWriter<uint> CAP_MAXBATCHBUFFERS => _cap_maxbatchbuffers ??= new(_twain, CAP.CAP_MAXBATCHBUFFERS, 1.8f);


    CapWriter<TW_BOOL>? _cap_micrenabled;
    public CapWriter<TW_BOOL> CAP_MICRENABLED => _cap_micrenabled ??= new(_twain, CAP.CAP_MICRENABLED, 2);


    CapReader<TW_BOOL>? _cap_paperdetectable;
    public CapReader<TW_BOOL> CAP_PAPERDETECTABLE => _cap_paperdetectable ??= new(_twain, CAP.CAP_PAPERDETECTABLE, 1.6f);


    CapWriter<TWPH>? _cap_paperhandling;
    public CapWriter<TWPH> CAP_PAPERHANDLING => _cap_paperhandling ??= new(_twain, CAP.CAP_PAPERHANDLING, 2.2f);


    CapWriter<int>? _cap_powersavetime;
    public CapWriter<int> CAP_POWERSAVETIME => _cap_powersavetime ??= new(_twain, CAP.CAP_POWERSAVETIME, 1.8f);


    CapReader<TWPS>? _cap_powersupply;
    public CapReader<TWPS> CAP_POWERSUPPLY => _cap_powersupply ??= new(_twain, CAP.CAP_POWERSUPPLY, 1.8f);


    CapWriter<TWPR>? _cap_printer;
    public CapWriter<TWPR> CAP_PRINTER => _cap_printer ??= new(_twain, CAP.CAP_PRINTER, 1.8f);


    CapWriter<uint>? _cap_printercharrotation;
    public CapWriter<uint> CAP_PRINTERCHARROTATION => _cap_printercharrotation ??= new(_twain, CAP.CAP_PRINTERCHARROTATION, 2.2f);


    CapWriter<TW_BOOL>? _cap_printerenabled;
    public CapWriter<TW_BOOL> CAP_PRINTERENABLED => _cap_printerenabled ??= new(_twain, CAP.CAP_PRINTERENABLED, 1.8f);


    CapWriter<TWPFS>? _cap_printerfontstyle;
    public CapWriter<TWPFS> CAP_PRINTERFONTSTYLE => _cap_printerfontstyle ??= new(_twain, CAP.CAP_PRINTERFONTSTYLE, 2.3f);


    CapWriter<uint>? _cap_printerindex;
    public CapWriter<uint> CAP_PRINTERINDEX => _cap_printerindex ??= new(_twain, CAP.CAP_PRINTERINDEX, 1.8f);


    CapWriter<TW_STR32>? _cap_printerindexleadchar;
    public CapWriter<TW_STR32> CAP_PRINTERINDEXLEADCHAR => _cap_printerindexleadchar ??= new(_twain, CAP.CAP_PRINTERINDEXLEADCHAR, 2.3f);


    CapWriter<uint>? _cap_printerindexmaxvalue;
    public CapWriter<uint> CAP_PRINTERINDEXMAXVALUE => _cap_printerindexmaxvalue ??= new(_twain, CAP.CAP_PRINTERINDEXMAXVALUE, 2.3f);


    CapWriter<uint>? _cap_printerindexnumdigits;
    public CapWriter<uint> CAP_PRINTERINDEXNUMDIGITS => _cap_printerindexnumdigits ??= new(_twain, CAP.CAP_PRINTERINDEXNUMDIGITS, 2.3f);


    CapWriter<uint>? _cap_printerindexstep;
    public CapWriter<uint> CAP_PRINTERINDEXSTEP => _cap_printerindexstep ??= new(_twain, CAP.CAP_PRINTERINDEXSTEP, 2.3f);


    CapWriter<TWCT>? _cap_printerindextrigger;
    public CapWriter<TWCT> CAP_PRINTERINDEXTRIGGER => _cap_printerindextrigger ??= new(_twain, CAP.CAP_PRINTERINDEXTRIGGER, 2.3f);


    CapWriter<TWPM>? _cap_printermode;
    public CapWriter<TWPM> CAP_PRINTERMODE => _cap_printermode ??= new(_twain, CAP.CAP_PRINTERMODE, 1.8f);


    CapWriter<TW_STR255>? _cap_printerstring;
    public CapWriter<TW_STR255> CAP_PRINTERSTRING => _cap_printerstring ??= new(_twain, CAP.CAP_PRINTERSTRING, 1.8f);


    CapReader<TW_STR255>? _cap_printerstringpreview;
    public CapReader<TW_STR255> CAP_PRINTERSTRINGPREVIEW => _cap_printerstringpreview ??= new(_twain, CAP.CAP_PRINTERSTRINGPREVIEW, 2.3f);


    CapWriter<TW_STR255>? _cap_printersuffix;
    public CapWriter<TW_STR255> CAP_PRINTERSUFFIX => _cap_printersuffix ??= new(_twain, CAP.CAP_PRINTERSUFFIX, 1.8f);


    CapWriter<TW_FIX32>? _cap_printerverticaloffset;
    public CapWriter<TW_FIX32> CAP_PRINTERVERTICALOFFSET => _cap_printerverticaloffset ??= new(_twain, CAP.CAP_PRINTERVERTICALOFFSET, 2.2f);


    CapReader<TW_BOOL>? _cap_reacquireallowed;
    public CapReader<TW_BOOL> CAP_REACQUIREALLOWED => _cap_reacquireallowed ??= new(_twain, CAP.CAP_REACQUIREALLOWED, 1.8f);


    CapWriter<TW_BOOL>? _cap_rewindpage;
    public CapWriter<TW_BOOL> CAP_REWINDPAGE => _cap_rewindpage ??= new(_twain, CAP.CAP_REWINDPAGE, 1);


    CapWriter<TWSG>? _cap_segmented;
    public CapWriter<TWSG> CAP_SEGMENTED => _cap_segmented ??= new(_twain, CAP.CAP_SEGMENTED, 1.91f);


    CapReader<TW_STR255>? _cap_serialnumber;
    public CapReader<TW_STR255> CAP_SERIALNUMBER => _cap_serialnumber ??= new(_twain, CAP.CAP_SERIALNUMBER, 1.8f);


    CapWriter<uint>? _cap_sheetcount;
    public CapWriter<uint> CAP_SHEETCOUNT => _cap_sheetcount ??= new(_twain, CAP.CAP_SHEETCOUNT, 2.4f);


    CapReader<CAP>? _cap_supportedcaps;
    public CapReader<CAP> CAP_SUPPORTEDCAPS => _cap_supportedcaps ??= new(_twain, CAP.CAP_SUPPORTEDCAPS, 1);


    CapReader<CAP>? _cap_supportedcapssegmentunique;
    public CapReader<CAP> CAP_SUPPORTEDCAPSSEGMENTUNIQUE => _cap_supportedcapssegmentunique ??= new(_twain, CAP.CAP_SUPPORTEDCAPSSEGMENTUNIQUE, 2.2f);


    CapReader<DAT>? _cap_supporteddats;
    public CapReader<DAT> CAP_SUPPORTEDDATS => _cap_supporteddats ??= new(_twain, CAP.CAP_SUPPORTEDDATS, 2.2f);


    CapWriter<TW_BOOL>? _cap_thumbnailsenabled;
    public CapWriter<TW_BOOL> CAP_THUMBNAILSENABLED => _cap_thumbnailsenabled ??= new(_twain, CAP.CAP_THUMBNAILSENABLED, 1.7f);


    CapWriter<int>? _cap_timebeforefirstcapture;
    public CapWriter<int> CAP_TIMEBEFOREFIRSTCAPTURE => _cap_timebeforefirstcapture ??= new(_twain, CAP.CAP_TIMEBEFOREFIRSTCAPTURE, 1.8f);


    CapWriter<int>? _cap_timebetweencaptures;
    public CapWriter<int> CAP_TIMEBETWEENCAPTURES => _cap_timebetweencaptures ??= new(_twain, CAP.CAP_TIMEBETWEENCAPTURES, 1.8f);


    CapReader<TW_STR32>? _cap_timedate;
    public CapReader<TW_STR32> CAP_TIMEDATE => _cap_timedate ??= new(_twain, CAP.CAP_TIMEDATE, 1);


    CapReader<TW_BOOL>? _cap_uicontrollable;
    public CapReader<TW_BOOL> CAP_UICONTROLLABLE => _cap_uicontrollable ??= new(_twain, CAP.CAP_UICONTROLLABLE, 1.6f);


    CapWriter<short>? _cap_xfercount;
    public CapWriter<short> CAP_XFERCOUNT => _cap_xfercount ??= new(_twain, CAP.CAP_XFERCOUNT, 1);

  }
}
