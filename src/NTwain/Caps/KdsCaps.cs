using NTwain.Data;
using NTwain.Data.Kds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Caps;

/// <summary>
/// Provides reader/writer wrapper of known <see cref="NTwain.Data.Kds.KDS_CAP"/>s.
/// </summary>
public partial class KdsCaps
{
    protected readonly TwainAppSession _twain;

    public KdsCaps(TwainAppSession twain)
    {
        _twain = twain;
    }


    CapWriter<TWBM>? _CAP_BLANKPAGEMODE;
    public CapWriter<TWBM> CAP_BLANKPAGEMODE =>
        _CAP_BLANKPAGEMODE ??= new(_twain, (CAP)KDS_CAP.CAP_BLANKPAGEMODE, 2);


    CapWriter<uint>? _CAP_BLANKPAGECONTENT;
    public CapWriter<uint> CAP_BLANKPAGECONTENT =>
        _CAP_BLANKPAGECONTENT ??= new(_twain, (CAP)KDS_CAP.CAP_BLANKPAGECONTENT, 2);


    CapWriter<uint>? _CAP_BLANKPAGECOMPSIZEBW;
    public CapWriter<uint> CAP_BLANKPAGECOMPSIZEBW =>
        _CAP_BLANKPAGECOMPSIZEBW ??= new(_twain, (CAP)KDS_CAP.CAP_BLANKPAGECOMPSIZEBW, 2);


    CapWriter<uint>? _CAP_BLANKPAGECOMPSIZEGRAY;
    public CapWriter<uint> CAP_BLANKPAGECOMPSIZEGRAY =>
        _CAP_BLANKPAGECOMPSIZEGRAY ??= new(_twain, (CAP)KDS_CAP.CAP_BLANKPAGECOMPSIZEGRAY, 2);


    CapWriter<uint>? _CAP_BLANKPAGECOMPSIZERGB;
    public CapWriter<uint> CAP_BLANKPAGECOMPSIZERGB =>
        _CAP_BLANKPAGECOMPSIZERGB ??= new(_twain, (CAP)KDS_CAP.CAP_BLANKPAGECOMPSIZERGB, 2);



    CapWriter<TWBK>? _CAP_BACKGROUND;
    public CapWriter<TWBK> CAP_BACKGROUND =>
        _CAP_BACKGROUND ??= new(_twain, (CAP)KDS_CAP.CAP_BACKGROUND, 2);


    CapWriter<TWBK>? _CAP_BACKGROUNDFRONT;
    public CapWriter<TWBK> CAP_BACKGROUNDFRONT =>
        _CAP_BACKGROUNDFRONT ??= new(_twain, (CAP)KDS_CAP.CAP_BACKGROUNDFRONT, 2);


    CapWriter<TWBK>? _CAP_BACKGROUNDREAR;
    public CapWriter<TWBK> CAP_BACKGROUNDREAR =>
        _CAP_BACKGROUNDREAR ??= new(_twain, (CAP)KDS_CAP.CAP_BACKGROUNDREAR, 2);


    CapWriter<TWBK>? _CAP_BACKGROUNDPLATEN;
    public CapWriter<TWBK> CAP_BACKGROUNDPLATEN =>
        _CAP_BACKGROUNDPLATEN ??= new(_twain, (CAP)KDS_CAP.CAP_BACKGROUNDPLATEN, 2);


    CapWriter<TW_FIX32>? _CAP_BATCHCOUNT;
    public CapWriter<TW_FIX32> CAP_BATCHCOUNT =>
        _CAP_BATCHCOUNT ??= new(_twain, (CAP)KDS_CAP.CAP_BATCHCOUNT, 2);

    // TODO: verify data types below


    CapWriter<TW_BOOL>? _CAP_BINARIZATION;
    public CapWriter<TW_BOOL> CAP_BINARIZATION =>
        _CAP_BINARIZATION ??= new(_twain, (CAP)KDS_CAP.CAP_BINARIZATION, 2);


    CapWriter<TW_BOOL>? _CAP_CHECKDIGIT;
    public CapWriter<TW_BOOL> CAP_CHECKDIGIT =>
        _CAP_CHECKDIGIT ??= new(_twain, (CAP)KDS_CAP.CAP_CHECKDIGIT, 2);


    CapWriter<TW_BOOL>? _CAP_DOUBLEFEEDENDJOB;
    public CapWriter<TW_BOOL> CAP_DOUBLEFEEDENDJOB =>
        _CAP_DOUBLEFEEDENDJOB ??= new(_twain, (CAP)KDS_CAP.CAP_DOUBLEFEEDENDJOB, 2);


    CapWriter<TW_BOOL>? _CAP_DOUBLEFEEDSTOP;
    public CapWriter<TW_BOOL> CAP_DOUBLEFEEDSTOP =>
        _CAP_DOUBLEFEEDSTOP ??= new(_twain, (CAP)KDS_CAP.CAP_DOUBLEFEEDSTOP, 2);


    CapWriter<TW_BOOL>? _CAP_EASYSTACKING;
    public CapWriter<TW_BOOL> CAP_EASYSTACKING =>
        _CAP_EASYSTACKING ??= new(_twain, (CAP)KDS_CAP.CAP_EASYSTACKING, 2);


    CapWriter<TW_BOOL>? _CAP_ENABLECOLORPATCHCODE;
    public CapWriter<TW_BOOL> CAP_ENABLECOLORPATCHCODE =>
        _CAP_ENABLECOLORPATCHCODE ??= new(_twain, (CAP)KDS_CAP.CAP_ENABLECOLORPATCHCODE, 2);


    CapWriter<int>? _CAP_ENERGYSTAR;
    public CapWriter<int> CAP_ENERGYSTAR =>
        _CAP_ENERGYSTAR ??= new(_twain, (CAP)KDS_CAP.CAP_ENERGYSTAR, 2);


    CapWriter<TW_BOOL>? _CAP_FEEDERKEEPALIVE;
    public CapWriter<TW_BOOL> CAP_FEEDERKEEPALIVE =>
        _CAP_FEEDERKEEPALIVE ??= new(_twain, (CAP)KDS_CAP.CAP_FEEDERKEEPALIVE, 2);


    CapWriter<ushort>? _CAP_FEEDERMODE;
    public CapWriter<ushort> CAP_FEEDERMODE =>
        _CAP_FEEDERMODE ??= new(_twain, (CAP)KDS_CAP.CAP_FEEDERMODE, 2);


    CapWriter<TW_BOOL>? _CAP_FIXEDDOCUMENTSIZE;
    public CapWriter<TW_BOOL> CAP_FIXEDDOCUMENTSIZE =>
        _CAP_FIXEDDOCUMENTSIZE ??= new(_twain, (CAP)KDS_CAP.CAP_FIXEDDOCUMENTSIZE, 2);


    CapWriter<ushort>? _CAP_FOLDEDCORNER;
    public CapWriter<ushort> CAP_FOLDEDCORNER =>
        _CAP_FOLDEDCORNER ??= new(_twain, (CAP)KDS_CAP.CAP_FOLDEDCORNER, 2);


    CapWriter<ushort>? _CAP_FOLDEDCORNERSENSITIVITY;
    public CapWriter<ushort> CAP_FOLDEDCORNERSENSITIVITY =>
        _CAP_FOLDEDCORNERSENSITIVITY ??= new(_twain, (CAP)KDS_CAP.CAP_FOLDEDCORNERSENSITIVITY, 2);


    CapWriter<TW_BOOL>? _CAP_INDICATORSWARMUP;
    public CapWriter<TW_BOOL> CAP_INDICATORSWARMUP =>
        _CAP_INDICATORSWARMUP ??= new(_twain, (CAP)KDS_CAP.CAP_INDICATORSWARMUP, 2);


    CapWriter<int>? _CAP_MULTIFEEDCOUNT;
    public CapWriter<int> CAP_MULTIFEEDCOUNT =>
        _CAP_MULTIFEEDCOUNT ??= new(_twain, (CAP)KDS_CAP.CAP_MULTIFEEDCOUNT, 2);


    CapWriter<ushort>? _CAP_MULTIFEEDRESPONSE;
    public CapWriter<ushort> CAP_MULTIFEEDRESPONSE =>
        _CAP_MULTIFEEDRESPONSE ??= new(_twain, (CAP)KDS_CAP.CAP_MULTIFEEDRESPONSE, 2);


    CapWriter<TW_STR255>? _CAP_MULTIFEEDSOUND;
    public CapWriter<TW_STR255> CAP_MULTIFEEDSOUND =>
        _CAP_MULTIFEEDSOUND ??= new(_twain, (CAP)KDS_CAP.CAP_MULTIFEEDSOUND, 2);


    CapWriter<TW_BOOL>? _CAP_MULTIFEEDTHICKNESSDETECTION;
    public CapWriter<TW_BOOL> CAP_MULTIFEEDTHICKNESSDETECTION =>
        _CAP_MULTIFEEDTHICKNESSDETECTION ??= new(_twain, (CAP)KDS_CAP.CAP_MULTIFEEDTHICKNESSDETECTION, 2);


    CapWriter<TW_BOOL>? _CAP_NOWAIT;
    public CapWriter<TW_BOOL> CAP_NOWAIT =>
        _CAP_NOWAIT ??= new(_twain, (CAP)KDS_CAP.CAP_NOWAIT, 2);


    CapWriter<TW_FIX32>? _CAP_PAGECOUNT;
    public CapWriter<TW_FIX32> CAP_PAGECOUNT =>
        _CAP_PAGECOUNT ??= new(_twain, (CAP)KDS_CAP.CAP_PAGECOUNT, 2);


    CapWriter<TW_FIX32>? _CAP_PAGESIZELIMIT;
    public CapWriter<TW_FIX32> CAP_PAGESIZELIMIT =>
        _CAP_PAGESIZELIMIT ??= new(_twain, (CAP)KDS_CAP.CAP_PAGESIZELIMIT, 2);


    CapWriter<ushort>? _CAP_PAPERDESTINATION;
    public CapWriter<ushort> CAP_PAPERDESTINATION =>
        _CAP_PAPERDESTINATION ??= new(_twain, (CAP)KDS_CAP.CAP_PAPERDESTINATION, 2);


    CapWriter<ushort>? _CAP_PAPERJAMRESPONSE;
    public CapWriter<ushort> CAP_PAPERJAMRESPONSE =>
        _CAP_PAPERJAMRESPONSE ??= new(_twain, (CAP)KDS_CAP.CAP_PAPERJAMRESPONSE, 2);


    CapWriter<ushort>? _CAP_PAPERSOURCE;
    public CapWriter<ushort> CAP_PAPERSOURCE =>
        _CAP_PAPERSOURCE ??= new(_twain, (CAP)KDS_CAP.CAP_PAPERSOURCE, 2);


    CapWriter<int>? _CAP_PATCHCOUNT;
    public CapWriter<int> CAP_PATCHCOUNT =>
        _CAP_PATCHCOUNT ??= new(_twain, (CAP)KDS_CAP.CAP_PATCHCOUNT, 2);


    CapWriter<TW_BOOL>? _CAP_PATCHHEAD1;
    public CapWriter<TW_BOOL> CAP_PATCHHEAD1 =>
        _CAP_PATCHHEAD1 ??= new(_twain, (CAP)KDS_CAP.CAP_PATCHHEAD1, 2);


    CapWriter<TW_BOOL>? _CAP_PATCHHEAD2;
    public CapWriter<TW_BOOL> CAP_PATCHHEAD2 =>
        _CAP_PATCHHEAD2 ??= new(_twain, (CAP)KDS_CAP.CAP_PATCHHEAD2, 2);


    CapWriter<TW_BOOL>? _CAP_PATCHHEAD3;
    public CapWriter<TW_BOOL> CAP_PATCHHEAD3 =>
        _CAP_PATCHHEAD3 ??= new(_twain, (CAP)KDS_CAP.CAP_PATCHHEAD3, 2);


    CapWriter<TW_BOOL>? _CAP_PATCHHEAD4;
    public CapWriter<TW_BOOL> CAP_PATCHHEAD4 =>
        _CAP_PATCHHEAD4 ??= new(_twain, (CAP)KDS_CAP.CAP_PATCHHEAD4, 2);


    CapWriter<int>? _CAP_POWEROFFTIMEOUT;
    public CapWriter<int> CAP_POWEROFFTIMEOUT =>
        _CAP_POWEROFFTIMEOUT ??= new(_twain, (CAP)KDS_CAP.CAP_POWEROFFTIMEOUT, 2);


    CapWriter<TW_BOOL>? _CAP_POWEROFFTIMEOUTENABLED;
    public CapWriter<TW_BOOL> CAP_POWEROFFTIMEOUTENABLED =>
        _CAP_POWEROFFTIMEOUTENABLED ??= new(_twain, (CAP)KDS_CAP.CAP_POWEROFFTIMEOUTENABLED, 2);


    CapWriter<TW_BOOL>? _CAP_ENHANCEDSEPARATION;
    public CapWriter<TW_BOOL> CAP_ENHANCEDSEPARATION =>
        _CAP_ENHANCEDSEPARATION ??= new(_twain, (CAP)KDS_CAP.CAP_ENHANCEDSEPARATION, 2);


    CapWriter<TW_STR255>? _CAP_PRINTERDATE;
    public CapWriter<TW_STR255> CAP_PRINTERDATE =>
        _CAP_PRINTERDATE ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERDATE, 2);


    CapWriter<ushort>? _CAP_PRINTERDATEDELIMITER;
    public CapWriter<ushort> CAP_PRINTERDATEDELIMITER =>
        _CAP_PRINTERDATEDELIMITER ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERDATEDELIMITER, 2);


    CapWriter<ushort>? _CAP_PRINTERDATEFORMAT;
    public CapWriter<ushort> CAP_PRINTERDATEFORMAT =>
        _CAP_PRINTERDATEFORMAT ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERDATEFORMAT, 2);


    CapWriter<ushort>? _CAP_PRINTERFONT;
    public CapWriter<ushort> CAP_PRINTERFONT =>
        _CAP_PRINTERFONT ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERFONT, 2);


    CapWriter<ushort>? _CAP_PRINTERFONTFORMAT;
    public CapWriter<ushort> CAP_PRINTERFONTFORMAT =>
        _CAP_PRINTERFONTFORMAT ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERFONTFORMAT, 2);


    CapWriter<int>? _CAP_PRINTERFONTRESIZE;
    public CapWriter<int> CAP_PRINTERFONTRESIZE =>
        _CAP_PRINTERFONTRESIZE ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERFONTRESIZE, 2);


    CapWriter<TW_FIX32>? _CAP_PRINTERPOSITION;
    public CapWriter<TW_FIX32> CAP_PRINTERPOSITION =>
        _CAP_PRINTERPOSITION ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERPOSITION, 2);


    CapWriter<TW_STR255>? _CAP_PRINTERTIME;
    public CapWriter<TW_STR255> CAP_PRINTERTIME =>
        _CAP_PRINTERTIME ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERTIME, 2);


    CapWriter<ushort>? _CAP_PRINTERTIMEFORMAT;
    public CapWriter<ushort> CAP_PRINTERTIMEFORMAT =>
        _CAP_PRINTERTIMEFORMAT ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTERTIMEFORMAT, 2);


    CapWriter<TW_BOOL>? _CAP_PRINTONIMAGEFRONT;
    public CapWriter<TW_BOOL> CAP_PRINTONIMAGEFRONT =>
        _CAP_PRINTONIMAGEFRONT ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTONIMAGEFRONT, 2);


    CapWriter<TW_BOOL>? _CAP_PRINTONIMAGEREAR;
    public CapWriter<TW_BOOL> CAP_PRINTONIMAGEREAR =>
        _CAP_PRINTONIMAGEREAR ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTONIMAGEREAR, 2);


    CapWriter<TW_FIX32>? _CAP_PRINTONIMAGEPOSITIONX;
    public CapWriter<TW_FIX32> CAP_PRINTONIMAGEPOSITIONX =>
        _CAP_PRINTONIMAGEPOSITIONX ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTONIMAGEPOSITIONX, 2);


    CapWriter<TW_FIX32>? _CAP_PRINTONIMAGEPOSITIONY;
    public CapWriter<TW_FIX32> CAP_PRINTONIMAGEPOSITIONY =>
        _CAP_PRINTONIMAGEPOSITIONY ??= new(_twain, (CAP)KDS_CAP.CAP_PRINTONIMAGEPOSITIONY, 2);


    CapWriter<TW_BOOL>? _CAP_SIDESDIFFERENT;
    public CapWriter<TW_BOOL> CAP_SIDESDIFFERENT =>
        _CAP_SIDESDIFFERENT ??= new(_twain, (CAP)KDS_CAP.CAP_SIDESDIFFERENT, 2);


    CapWriter<TW_BOOL>? _CAP_SIMULATING;
    public CapWriter<TW_BOOL> CAP_SIMULATING =>
        _CAP_SIMULATING ??= new(_twain, (CAP)KDS_CAP.CAP_SIMULATING, 2);


    CapWriter<ushort>? _CAP_TOGGLEPATCH;
    public CapWriter<ushort> CAP_TOGGLEPATCH =>
        _CAP_TOGGLEPATCH ??= new(_twain, (CAP)KDS_CAP.CAP_TOGGLEPATCH, 2);


    CapWriter<TW_BOOL>? _CAP_TRANSPORTAUTOSTART;
    public CapWriter<TW_BOOL> CAP_TRANSPORTAUTOSTART =>
        _CAP_TRANSPORTAUTOSTART ??= new(_twain, (CAP)KDS_CAP.CAP_TRANSPORTAUTOSTART, 2);


    CapWriter<int>? _CAP_TRANSPORTTIMEOUT;
    public CapWriter<int> CAP_TRANSPORTTIMEOUT =>
        _CAP_TRANSPORTTIMEOUT ??= new(_twain, (CAP)KDS_CAP.CAP_TRANSPORTTIMEOUT, 2);


    CapWriter<ushort>? _CAP_TRANSPORTTIMEOUTRESPONSE;
    public CapWriter<ushort> CAP_TRANSPORTTIMEOUTRESPONSE =>
        _CAP_TRANSPORTTIMEOUTRESPONSE ??= new(_twain, (CAP)KDS_CAP.CAP_TRANSPORTTIMEOUTRESPONSE, 2);


    CapWriter<ushort>? _CAP_ULTRASONICSENSITIVITY;
    public CapWriter<ushort> CAP_ULTRASONICSENSITIVITY =>
        _CAP_ULTRASONICSENSITIVITY ??= new(_twain, (CAP)KDS_CAP.CAP_ULTRASONICSENSITIVITY, 2);


    CapWriter<ushort>? _CAP_ULTRASONICSENSORCENTER;
    public CapWriter<ushort> CAP_ULTRASONICSENSORCENTER =>
        _CAP_ULTRASONICSENSORCENTER ??= new(_twain, (CAP)KDS_CAP.CAP_ULTRASONICSENSORCENTER, 2);


    CapWriter<ushort>? _CAP_ULTRASONICSENSORLEFT;
    public CapWriter<ushort> CAP_ULTRASONICSENSORLEFT =>
        _CAP_ULTRASONICSENSORLEFT ??= new(_twain, (CAP)KDS_CAP.CAP_ULTRASONICSENSORLEFT, 2);


    CapWriter<ushort>? _CAP_ULTRASONICSENSORRIGHT;
    public CapWriter<ushort> CAP_ULTRASONICSENSORRIGHT =>
        _CAP_ULTRASONICSENSORRIGHT ??= new(_twain, (CAP)KDS_CAP.CAP_ULTRASONICSENSORRIGHT, 2);


    CapWriter<ushort>? _CAP_ULTRASONICSENSORLEFTCENTER;
    public CapWriter<ushort> CAP_ULTRASONICSENSORLEFTCENTER =>
        _CAP_ULTRASONICSENSORLEFTCENTER ??= new(_twain, (CAP)KDS_CAP.CAP_ULTRASONICSENSORLEFTCENTER, 2);


    CapWriter<ushort>? _CAP_ULTRASONICSENSORRIGHTCENTER;
    public CapWriter<ushort> CAP_ULTRASONICSENSORRIGHTCENTER =>
        _CAP_ULTRASONICSENSORRIGHTCENTER ??= new(_twain, (CAP)KDS_CAP.CAP_ULTRASONICSENSORRIGHTCENTER, 2);


    CapWriter<TW_FIX32>? _CAP_ULTRASONICSENSORZONEHEIGHT;
    public CapWriter<TW_FIX32> CAP_ULTRASONICSENSORZONEHEIGHT =>
        _CAP_ULTRASONICSENSORZONEHEIGHT ??= new(_twain, (CAP)KDS_CAP.CAP_ULTRASONICSENSORZONEHEIGHT, 2);


    CapWriter<ushort>? _CAP_WINDOWPOSITION;
    public CapWriter<ushort> CAP_WINDOWPOSITION =>
        _CAP_WINDOWPOSITION ??= new(_twain, (CAP)KDS_CAP.CAP_WINDOWPOSITION, 2);
}
