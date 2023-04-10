using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace NTwain.Data
{
  // this contains my additions 
  // that makes some twain types easier to work with.

  /// <summary>
  /// Contains platform info for twain use.
  /// </summary>
  public static class TWPlatform
  {
    static TWPlatform()
    {
      Is32bit = IntPtr.Size == 4;

#if NETFRAMEWORK
      if (Environment.OSVersion.Platform == PlatformID.Win32NT)
      {
        IsWindows = true;
      }
      else if (System.IO.Directory.Exists("/Library/Application Support"))
      {
        IsMacOSX = true;
      }
      else if (Environment.OSVersion.Platform == PlatformID.Unix)
      {
        IsLinux = true;
      }

#else
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        IsWindows = true;
      }
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
      {
        IsMacOSX = true;
      }
      else
      {
        IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
      }
#endif
    }

    /// <summary>
    /// Whether the code is running under Linux.
    /// </summary>
    public static bool IsLinux { get; }

    /// <summary>
    /// Whether the code is running under MacOSX.
    /// </summary>
    public static bool IsMacOSX { get; }

    /// <summary>
    /// Whether the code is running under Windows.
    /// </summary>
    public static bool IsWindows { get; }

    /// <summary>
    /// Whether the code is running in 64bit or 32bit.
    /// </summary>
    public static bool Is32bit { get; }

    /// <summary>
    /// Whether to use the older DSM lib on Windows and Mac. 
    /// On Windows it only takes effect when running in 32bit. 
    /// Defaults to false.
    /// </summary>
    public static bool PreferLegacyDSM { get; set; }
  }

  /// <summary>
  /// Contains value that don't fit into enums nicely.
  /// </summary>
  public static class TWConst
  {
    /// <summary>
    /// Don't care values...
    /// </summary>
    public const byte TWON_DONTCARE8 = 0xff;
    public const ushort TWON_DONTCARE16 = 0xffff;
    public const uint TWON_DONTCARE32 = 0xffffffff;
  }

  /// <summary>
  /// TWAIN's boolean values.
  /// </summary>
  public enum TW_BOOL : ushort
  {
    /// <summary>
    /// The false value (0).
    /// </summary>
    False = 0,
    /// <summary>
    /// The true value (1).
    /// </summary>
    True = 1
  }

  ///// <summary>
  ///// A more dotnet-friendly representation of <see cref="TW_METRICS"/>.
  ///// </summary>
  //public struct Metrics
  //{
  //  /// <summary>
  //  /// Return code of querying the metrics.
  //  /// </summary>
  //  public STS ReturnCode;

  //  /// <summary>
  //  /// The number of sheets of paper processed by the scanner.
  //  /// </summary>
  //  public int Sheets;

  //  /// <summary>
  //  /// The number of images made available for transfer by the driver. This is not
  //  /// necessarily the same as the number of images actually transferred, since the
  //  /// application may opt to skip transfers or to end without transferring all images.
  //  /// </summary>
  //  public int Images;
  //}

  /// <summary>
  /// More dotnet friendly return type for twaindirect set result.
  /// </summary>
  public struct TwainDirectTaskResult
  {
    /// <summary>
    /// Return code of task.
    /// </summary>
    public TWRC ReturnCode;

    /// <summary>
    /// Status if code is failure.
    /// </summary>
    public TW_STATUS Status;

    /// <summary>
    /// The response of the task in JSON if successful.
    /// </summary>
    public string? ResponseJson;
  }

  public enum TWRC : ushort
  {
    // Custom base (same for TWRC and TWCC)...
    CUSTOMBASE = 0x8000,

    // Return codes...
    SUCCESS = 0,
    FAILURE = 1,
    CHECKSTATUS = 2,
    CANCEL = 3,
    DSEVENT = 4,
    NOTDSEVENT = 5,
    XFERDONE = 6,
    ENDOFLIST = 7,
    INFONOTSUPPORTED = 8,
    DATANOTAVAILABLE = 9,
    BUSY = 10,
    SCANNERLOCKED = 11,
  }
  public enum TWCC : ushort
  {
    // Condition codes (always associated with TWRC_FAILURE)...
    CUSTOMBASE = 0x8000,
    None = 0,
    BUMMER = 1,
    LOWMEMORY = 2,
    NODS = 3,
    MAXCONNECTIONS = 4,
    OPERATIONERROR = 5,
    BADCAP = 6,
    BADPROTOCOL = 9,
    BADVALUE = 10,
    SEQERROR = 11,
    BADDEST = 12,
    CAPUNSUPPORTED = 13,
    CAPBADOPERATION = 14,
    CAPSEQERROR = 15,
    DENIED = 16,
    FILEEXISTS = 17,
    FILENOTFOUND = 18,
    NOTEMPTY = 19,
    PAPERJAM = 20,
    PAPERDOUBLEFEED = 21,
    FILEWRITEERROR = 22,
    CHECKDEVICEONLINE = 23,
    INTERLOCK = 24,
    DAMAGEDCORNER = 25,
    FOCUSERROR = 26,
    DOCTOOLIGHT = 27,
    DOCTOODARK = 28,
    NOMEDIA = 29
  }

  //[StructLayout(LayoutKind.Sequential, Pack = 2)]

  /// <summary>
  /// Extended return type with original return code and additional
  /// status if RC is <see cref="TWRC.FAILURE"/>.
  /// </summary>
  public struct STS
  {
    /// <summary>
    /// The original return code.
    /// </summary>
    public TWRC RC;

    /// <summary>
    /// Additional status if RC is <see cref="TWRC.FAILURE"/>.
    /// </summary>
    public TW_STATUS STATUS;

    ///// <summary>
    ///// For easy conversion from DSM calls to the one we want
    ///// </summary>
    ///// <param name="rc"></param>
    //public static implicit operator STS(TWRC rc)
    //{
    //  return new STS { RC = rc };
    //}

    /// <summary>
    /// Quick check if the RC is success.
    /// </summary>
    public bool IsSuccess => RC == TWRC.SUCCESS;

    /// <summary>
    /// Quick access to condition code.
    /// </summary>
    public TWCC ConditionCode => STATUS.ConditionCode;

    public override string ToString()
    {
      return $"{RC} - {STATUS.ConditionCode}";
    }
  }

  partial struct TW_STATUS
  {
    public override string ToString()
    {
      return ConditionCode.ToString();
    }
  }

  /// <summary>
  /// A more dotnet-friendly representation of <see cref="TW_ENUMERATION"/>.
  /// </summary>
  /// <typeparam name="TValue"></typeparam>
  public class Enumeration<TValue> where TValue : struct
  {
    public int CurrentIndex;

    public int DefaultIndex;

    public TValue[]? Items;
  }

  /// <summary>
  /// A more dotnet-friendly representation of <see cref="TW_RANGE"/>.
  /// </summary>
  /// <typeparam name="TValue"></typeparam>
  public partial class Range<TValue> : IEnumerable<TValue> where TValue : struct
  {
    public TValue MinValue;
    public TValue MaxValue;
    public TValue StepSize;
    public TValue DefaultValue;
    public TValue CurrentValue;

    IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
    {
      if (MinValue is not IConvertible)
        throw new NotSupportedException($"The value type {typeof(TValue).Name} is not supported for range enumeration.");

      return new DynamicEnumerator(MinValue, MaxValue, StepSize);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return ((IEnumerable<TValue>)this).GetEnumerator();
    }

    // dynamic is a cheap hack to sidestep the compiler restrictions if I know TValue is numeric
    class DynamicEnumerator : IEnumerator<TValue>
    {
      private readonly TValue _min;
      private readonly TValue _max;
      private readonly TValue _step;
      private TValue _cur;
      bool started = false;

      public DynamicEnumerator(TValue min, TValue max, TValue step)
      {
        _min = min;
        _max = max;
        _step = step;
        _cur = min;
      }

      public TValue Current => _cur;

      object System.Collections.IEnumerator.Current => this.Current;

      public void Dispose() { }

      public bool MoveNext()
      {
        if (!started)
        {
          started = true;
          return true;
        }

        var next = _cur + (dynamic)_step;
        if (next == _cur || next < _min || next > _max) return false;

        _cur = next;
        return true;
      }

      public void Reset()
      {
        _cur = _min;
        started = false;
      }
    }
  }

  partial struct TW_FIX32 : IEquatable<TW_FIX32>, IConvertible
  {
    // this conversion logic is found in the twain spec.

    float ToFloat()
    {
      return Whole + Frac / 65536f;
    }
    double ToDouble()
    {
      return Whole + Frac / 65536.0;
    }
    public TW_FIX32(double value)
    {
      Whole = (short)value;
      Frac = (ushort)((value - Whole) * 65536.0);
    }
    public TW_FIX32(float value)
    {
      //int temp = (int)(value * 65536.0 + 0.5);
      //Whole = (short)(temp >> 16);
      //Fraction = (ushort)(temp & 0x0000ffff);

      // different version from twain faq
      bool sign = value < 0;
      int temp = (int)(value * 65536.0 + (sign ? (-0.5) : 0.5));
      Whole = (short)(temp >> 16);
      Frac = (ushort)(temp & 0x0000ffff);
    }

    public override string ToString()
    {
      return ToFloat().ToString();
    }

    public bool Equals(TW_FIX32 other)
    {
      return Whole == other.Whole && Frac == other.Frac;
    }
    public override bool Equals(object? obj)
    {
      if (obj is TW_FIX32 other)
      {
        return Equals(other);
      }
      return false;
    }
    public override int GetHashCode()
    {
      return Whole ^ Frac;
    }


    #region IConvertable

    TypeCode IConvertible.GetTypeCode()
    {
      return TypeCode.Single;
    }

    bool IConvertible.ToBoolean(IFormatProvider? provider)
    {
      return this != 0;
    }

    byte IConvertible.ToByte(IFormatProvider? provider)
    {
      return Convert.ToByte((float)this);
    }

    char IConvertible.ToChar(IFormatProvider? provider)
    {
      return Convert.ToChar((float)this);
    }

    DateTime IConvertible.ToDateTime(IFormatProvider? provider)
    {
      return Convert.ToDateTime((float)this);
    }

    decimal IConvertible.ToDecimal(IFormatProvider? provider)
    {
      return Convert.ToDecimal((float)this);
    }

    double IConvertible.ToDouble(IFormatProvider? provider)
    {
      return Convert.ToDouble((float)this);
    }

    short IConvertible.ToInt16(IFormatProvider? provider)
    {
      return Convert.ToInt16((float)this);
    }

    int IConvertible.ToInt32(IFormatProvider? provider)
    {
      return Convert.ToInt32((float)this);
    }

    long IConvertible.ToInt64(IFormatProvider? provider)
    {
      return Convert.ToInt64((float)this);
    }

    sbyte IConvertible.ToSByte(IFormatProvider? provider)
    {
      return Convert.ToSByte((float)this);
    }

    float IConvertible.ToSingle(IFormatProvider? provider)
    {
      return Convert.ToSingle((float)this);
    }

    string IConvertible.ToString(IFormatProvider? provider)
    {
      return this.ToString();
    }

    object IConvertible.ToType(Type conversionType, IFormatProvider? provider)
    {
      return Convert.ChangeType((float)this, conversionType, CultureInfo.InvariantCulture);
    }

    ushort IConvertible.ToUInt16(IFormatProvider? provider)
    {
      return Convert.ToUInt16((float)this);
    }

    uint IConvertible.ToUInt32(IFormatProvider? provider)
    {
      return Convert.ToUInt32((float)this);
    }

    ulong IConvertible.ToUInt64(IFormatProvider? provider)
    {
      return Convert.ToUInt64((float)this);
    }

    #endregion

    public static implicit operator float(TW_FIX32 value) => value.ToFloat();
    public static implicit operator TW_FIX32(float value) => new(value);

    public static implicit operator double(TW_FIX32 value) => value.ToDouble();
    public static implicit operator TW_FIX32(double value) => new((float)value);

    public static bool operator ==(TW_FIX32 value1, TW_FIX32 value2) => value1.Equals(value2);
    public static bool operator !=(TW_FIX32 value1, TW_FIX32 value2) => !value1.Equals(value2);
  }

  partial struct TW_FRAME : IEquatable<TW_FRAME>
  {
    /// <summary>
    /// Creates <see cref="TW_FRAME"/> from a string representation of it.
    /// </summary>
    /// <param name="value"></param>
    public TW_FRAME(string value) : this()
    {
      var parts = value.Split(',');
      if (parts.Length == 4)
      {
        Left = float.Parse(parts[0]);
        Top = float.Parse(parts[1]);
        Right = float.Parse(parts[2]);
        Bottom = float.Parse(parts[3]);
      }
      else
      {
        throw new ArgumentException($"Cannot create frame from \"{value}\".");
      }
    }

    /// <summary>
    /// String representation of Left,Top,Right,Bottom.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return $"({Left},{Top}) ({Right},{Bottom})";
    }

    public bool Equals(TW_FRAME other)
    {
      return Left == other.Left && Top == other.Top &&
          Right == other.Right && Bottom == other.Bottom;
    }

    public override bool Equals(object? obj)
    {
      if (obj is TW_FRAME other)
      {
        return Equals(other);
      }
      return false;
    }

    public override int GetHashCode()
    {
      return Left.GetHashCode() ^ Top.GetHashCode() ^
          Right.GetHashCode() ^ Bottom.GetHashCode();
    }


    public static bool operator ==(TW_FRAME value1, TW_FRAME value2)
    {
      return value1.Equals(value2);
    }
    public static bool operator !=(TW_FRAME value1, TW_FRAME value2)
    {
      return !value1.Equals(value2);
    }
  }

  partial struct TW_STR32
  {
    public const int Size = 34;

    public TW_STR32(string value) : this()
    {
      Set(value);
    }

    public override string ToString()
    {
      return Get();
    }

    public static implicit operator string(TW_STR32 value) => value.ToString();
    public static implicit operator TW_STR32(string value) => new(value);

  }

  partial struct TW_STR64
  {
    public const int Size = 66;

    public TW_STR64(string value) : this()
    {
      Set(value);
    }

    public override string ToString()
    {
      return Get();
    }

    public static implicit operator string(TW_STR64 value) => value.ToString();
    public static implicit operator TW_STR64(string value) => new(value);
  }

  partial struct TW_STR128
  {
    public const int Size = 130;

    public TW_STR128(string value) : this()
    {
      Set(value);
    }

    public override string ToString()
    {
      return Get();
    }

    public static implicit operator string(TW_STR128 value) => value.ToString();
    public static implicit operator TW_STR128(string value) => new(value);
  }

  partial struct TW_STR255
  {
    public const int Size = 256;

    public TW_STR255(string value) : this()
    {
      Set(value);
    }

    public override string ToString()
    {
      return Get();
    }

    public static implicit operator string(TW_STR255 value) => value.ToString();
    public static implicit operator TW_STR255(string value) => new(value);
  }

  partial struct TW_IDENTITY
  {
    public override string ToString()
    {
      return $"{Manufacturer} - {ProductFamily} - {ProductName} {Version} (TWAIN {ProtocolMajor}.{ProtocolMinor})";
    }
  }
  partial struct TW_IDENTITY_MACOSX
  {
    public override string ToString()
    {
      return $"{Manufacturer} - {ProductName} v{Version.MajorNum}.{Version.MinorNum} (TWAIN {ProtocolMajor}.{ProtocolMinor})";
    }
    public static implicit operator TW_IDENTITY_LEGACY(TW_IDENTITY_MACOSX value) => new()
    {
      Id = value.Id,
      Manufacturer = value.Manufacturer,
      ProductFamily = value.ProductFamily,
      ProductName = value.ProductName,
      ProtocolMajor = value.ProtocolMajor,
      ProtocolMinor = value.ProtocolMinor,
      SupportedGroups = value.SupportedGroups,
      Version = new()
      {
        Country = value.Version.Country,
        Info = value.Version.Info,
        Language = value.Version.Language,
        MajorNum = value.Version.MajorNum,
        MinorNum = value.Version.MinorNum,
      }
    };
  }
  partial struct TW_IDENTITY_LEGACY
  {
    /// <summary>
    /// A simplified check on whether this has valid data from DSM.
    /// </summary>
    public bool HasValue => Id == 0 && ProtocolMajor == 0 && ProtocolMinor == 0;

    public override string ToString()
    {
      return $"{Manufacturer} - {ProductName} v{Version.MajorNum}.{Version.MinorNum} (TWAIN {ProtocolMajor}.{ProtocolMinor})";
    }

    public static implicit operator TW_IDENTITY(TW_IDENTITY_LEGACY value) => new()
    {
      Id = value.Id,
      Manufacturer = value.Manufacturer,
      ProductFamily = value.ProductFamily,
      ProductName = value.ProductName,
      ProtocolMajor = value.ProtocolMajor,
      ProtocolMinor = value.ProtocolMinor,
      SupportedGroups = value.SupportedGroups,
      Version = new()
      {
        Country = value.Version.Country,
        Info = value.Version.Info,
        Language = value.Version.Language,
        MajorNum = value.Version.MajorNum,
        MinorNum = value.Version.MinorNum,
      }
    };
    public static implicit operator TW_IDENTITY_MACOSX(TW_IDENTITY_LEGACY value) => new()
    {
      Id = value.Id,
      Manufacturer = value.Manufacturer,
      ProductFamily = value.ProductFamily,
      ProductName = value.ProductName,
      ProtocolMajor = value.ProtocolMajor,
      ProtocolMinor = value.ProtocolMinor,
      SupportedGroups = value.SupportedGroups,
      Version = new()
      {
        Country = value.Version.Country,
        Info = value.Version.Info,
        Language = value.Version.Language,
        MajorNum = value.Version.MajorNum,
        MinorNum = value.Version.MinorNum,
      }
    };
  }

  partial struct TW_VERSION
  {
    public override string ToString()
    {
      return $"{MajorNum}.{MinorNum}";
    }
  }

  partial struct TW_STATUSUTF8
  {
    /// <summary>
    /// Frees the memory if necessary.
    /// </summary>
    /// <param name="mgr"></param>
    public void Free(IMemoryManager mgr)
    {
      // session already checks for zero
      mgr.Free(UTF8string);
      UTF8string = IntPtr.Zero;
    }

    /// <summary>
    /// Tries to read the text content and optionally frees the memory.
    /// </summary>
    /// <param name="mgr"></param>
    /// <param name="freeMemory">Whether to free the pointer after reads.</param>
    /// <returns></returns>
    public string? Read(IMemoryManager mgr, bool freeMemory = true)
    {
      string? val = null;
      if (UTF8string != IntPtr.Zero && Size > 0)
      {
        val = UTF8string.PtrToStringUTF8(mgr, (int)Size);
      }
      if (freeMemory) Free(mgr);
      return val;
    }
  }

  partial struct TW_CAPABILITY
  {
    public TW_CAPABILITY(CAP cap)
    {
      Cap = cap;
      ConType = (TWON)TWConst.TWON_DONTCARE16;
    }

    /// <summary>
    /// Frees the memory if necessary.
    /// </summary>
    /// <param name="mgr"></param>
    public void Free(IMemoryManager mgr)
    {
      // session already checks for zero
      mgr.Free(hContainer);
      hContainer = IntPtr.Zero;
    }
  }

  partial struct TW_IMAGEINFO
  {
    public override string ToString()
    {
      return $"{ImageWidth}x{ImageLength} {PixelType} {Compression} {BitsPerPixel}bpp";
    }
  }

  partial struct TW_SETUPMEMXFER
  {
    /// <summary>
    /// Determines the best buffer size from values
    /// specified by source
    /// </summary>
    /// <returns></returns>
    public uint DetermineBufferSize()
    {
      if (Preferred != TWConst.TWON_DONTCARE32) return Preferred;
      if (MaxBufSize != TWConst.TWON_DONTCARE32) return MaxBufSize;
      if (MinBufSize != TWConst.TWON_DONTCARE32) return MinBufSize;
      // default to 16 kb if source doesn't really want to say what it needs
      return 1024 * 16;
    }
  }

  partial struct TW_IMAGEMEMXFER
  {
    /// <summary>
    /// Get a don't care version for app use.
    /// </summary>
    /// <returns></returns>
    public static TW_IMAGEMEMXFER DONTCARE()
    {
      return new TW_IMAGEMEMXFER
      {
        BytesPerRow = TWConst.TWON_DONTCARE32,
        BytesWritten = TWConst.TWON_DONTCARE32,
        Columns = TWConst.TWON_DONTCARE32,
        Compression = TWConst.TWON_DONTCARE16,
        Rows = TWConst.TWON_DONTCARE32,
        XOffset = TWConst.TWON_DONTCARE32,
        YOffset = TWConst.TWON_DONTCARE32,
      };
    }
  }

  partial struct TW_IMAGEMEMXFER_MACOSX
  {
    /// <summary>
    /// Get a don't care version for app use.
    /// </summary>
    /// <returns></returns>
    public static TW_IMAGEMEMXFER_MACOSX DONTCARE()
    {
      return new TW_IMAGEMEMXFER_MACOSX
      {
        BytesPerRow = TWConst.TWON_DONTCARE32,
        BytesWritten = TWConst.TWON_DONTCARE32,
        Columns = TWConst.TWON_DONTCARE32,
        Compression = TWConst.TWON_DONTCARE32,
        Rows = TWConst.TWON_DONTCARE32,
        XOffset = TWConst.TWON_DONTCARE32,
        YOffset = TWConst.TWON_DONTCARE32,
      };
    }
  }

  partial struct TW_DEVICEEVENT
  {
    // provide casted versions over raw value

    public TWDE Event { get { return (TWDE)_Event; } }

    public TWFL FlashUsed2 { get { return (TWFL)_FlashUsed2; } }
  }


  /// <summary>
  /// Container for querying ext image info. After querying and done with
  /// the data you must call <see cref="Free(IMemoryManager)"/> to
  /// free the memory allocated.
  /// </summary>
  public partial struct TW_EXTIMAGEINFO
  {
    /// <summary>
    /// A quick way to create a query object with only <see cref="TWEI"/> values.
    /// Limit is 100 at this time.
    /// </summary>
    /// <param name="infoNames"></param>
    /// <returns></returns>
    public static TW_EXTIMAGEINFO CreateRequest(params TWEI[] infoNames)
    {
      if (infoNames == null || infoNames.Length == 0) return default;
      if (infoNames.Length > 100) throw new InvalidOperationException("Cannot query more than 100 TWEIs at this time.");

      TW_EXTIMAGEINFO container = new()
      {
        NumInfos = (uint)infoNames.Length,
      };

      for (var i = 0; i < infoNames.Length; i++)
      {
        TW_INFO info = new() { InfoId = infoNames[i] };
        container.Set(i, ref info);
      }
      return container;
    }

    /// <summary>
    /// Reads the info out of this as array.
    /// </summary>
    /// <returns></returns>
    public TW_INFO[] AsInfos()
    {
      if (NumInfos == 0) return Array.Empty<TW_INFO>();

      var arr = new TW_INFO[NumInfos];
      for (var i = 0; i < NumInfos; i++)
      {
        TW_INFO blah = default;
        Get(i, ref blah);
        arr[i] = blah;
      }
      return arr;
    }

    /// <summary>
    /// Frees all data contained here.
    /// </summary>
    /// <param name="memMgr"></param>
    public void Free(IMemoryManager memMgr)
    {
      #region don't open this
      Info_000.Free(memMgr);
      Info_001.Free(memMgr);
      Info_002.Free(memMgr);
      Info_003.Free(memMgr);
      Info_004.Free(memMgr);
      Info_005.Free(memMgr);
      Info_006.Free(memMgr);
      Info_007.Free(memMgr);
      Info_008.Free(memMgr);
      Info_009.Free(memMgr);
      Info_010.Free(memMgr);
      Info_011.Free(memMgr);
      Info_012.Free(memMgr);
      Info_013.Free(memMgr);
      Info_014.Free(memMgr);
      Info_015.Free(memMgr);
      Info_016.Free(memMgr);
      Info_017.Free(memMgr);
      Info_018.Free(memMgr);
      Info_019.Free(memMgr);
      Info_020.Free(memMgr);
      Info_021.Free(memMgr);
      Info_022.Free(memMgr);
      Info_023.Free(memMgr);
      Info_024.Free(memMgr);
      Info_025.Free(memMgr);
      Info_026.Free(memMgr);
      Info_027.Free(memMgr);
      Info_028.Free(memMgr);
      Info_029.Free(memMgr);
      Info_030.Free(memMgr);
      Info_031.Free(memMgr);
      Info_032.Free(memMgr);
      Info_033.Free(memMgr);
      Info_034.Free(memMgr);
      Info_035.Free(memMgr);
      Info_036.Free(memMgr);
      Info_037.Free(memMgr);
      Info_038.Free(memMgr);
      Info_039.Free(memMgr);
      Info_040.Free(memMgr);
      Info_041.Free(memMgr);
      Info_042.Free(memMgr);
      Info_043.Free(memMgr);
      Info_044.Free(memMgr);
      Info_045.Free(memMgr);
      Info_046.Free(memMgr);
      Info_047.Free(memMgr);
      Info_048.Free(memMgr);
      Info_049.Free(memMgr);
      Info_050.Free(memMgr);
      Info_051.Free(memMgr);
      Info_052.Free(memMgr);
      Info_053.Free(memMgr);
      Info_054.Free(memMgr);
      Info_055.Free(memMgr);
      Info_056.Free(memMgr);
      Info_057.Free(memMgr);
      Info_058.Free(memMgr);
      Info_059.Free(memMgr);
      Info_060.Free(memMgr);
      Info_061.Free(memMgr);
      Info_062.Free(memMgr);
      Info_063.Free(memMgr);
      Info_064.Free(memMgr);
      Info_065.Free(memMgr);
      Info_066.Free(memMgr);
      Info_067.Free(memMgr);
      Info_068.Free(memMgr);
      Info_069.Free(memMgr);
      Info_070.Free(memMgr);
      Info_071.Free(memMgr);
      Info_072.Free(memMgr);
      Info_073.Free(memMgr);
      Info_074.Free(memMgr);
      Info_075.Free(memMgr);
      Info_076.Free(memMgr);
      Info_077.Free(memMgr);
      Info_078.Free(memMgr);
      Info_079.Free(memMgr);
      Info_080.Free(memMgr);
      Info_081.Free(memMgr);
      Info_082.Free(memMgr);
      Info_083.Free(memMgr);
      Info_084.Free(memMgr);
      Info_085.Free(memMgr);
      Info_086.Free(memMgr);
      Info_087.Free(memMgr);
      Info_088.Free(memMgr);
      Info_089.Free(memMgr);
      Info_090.Free(memMgr);
      Info_091.Free(memMgr);
      Info_092.Free(memMgr);
      Info_093.Free(memMgr);
      Info_094.Free(memMgr);
      Info_095.Free(memMgr);
      Info_096.Free(memMgr);
      Info_097.Free(memMgr);
      Info_098.Free(memMgr);
      Info_099.Free(memMgr);
      #endregion
    }

  }

  partial struct TW_INFO
  {
    /// <summary>
    /// Quick check to see if the <see cref="Item"/> pointer is really
    /// a pointer or actual data (ugh).
    /// </summary>
    public bool IsDataAPointer =>
      ItemType == TWTY.HANDLE || (ItemType.GetItemTypeSize() * NumItems) > IntPtr.Size; // should it be intptr.size or just 4?

    /// <summary>
    /// Try to read out the item as the type specified in <see cref="ItemType"/>.
    /// This ONLY works if the data is not a pointer (see <see cref="IsDataAPointer"/>). 
    /// For pointers you'd read it yourself with 
    /// <see cref="ValueReader.ReadTWTYData{TValue}(IntPtr, TWTY, int)"/>.
    /// Unless it's a handle (<see cref="TWTY.HANDLE"/>) to non-twain-strings, then you'd use 
    /// <see cref="ReadHandleString(IMemoryManager, int)"/>.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <returns></returns>
    public unsafe TValue ReadNonPointerData<TValue>() where TValue : struct
    {
      if (ReturnCode != TWRC.SUCCESS || NumItems == 0 || IsDataAPointer) return default;

      // we can try a trick and make a pointer to this numeric data
      // and re-use our pointer reader. There's a good chance this is wrong in many ways.
      // TODO: test this idea in some unit test
      var value = TWPlatform.Is32bit ? Item.ToUInt32() : Item.ToUInt64(); // the value should be 32bit from the spec but not sure how it'll work in 64bit

      var fakePtr = (IntPtr)(&value);

      return fakePtr.ReadTWTYData<TValue>(ItemType, 0);
    }

    /// <summary>
    /// Try to read a null-terminated string from the item.
    /// </summary>
    /// <param name="memMgr"></param>
    /// <param name="index">If item is an array specify which string to read</param>
    /// <returns></returns>
    public unsafe string? ReadHandleString(IMemoryManager memMgr, int index = 0)
    {
      if (index < 0 || index >= NumItems || !IsDataAPointer) return default;

      // why is twain being difficult and not use TW_STR* like a normal person.
      // what even is the encoding for those things? Imma yolo it.
      string? value;
      var itemAsPtr = (IntPtr)Item.ToPointer(); // this is also iffy

      if (NumItems == 1)
      {
        // if 1, item is already the pointer to the string
        value = LockAndReadNullTerminatedString(memMgr, itemAsPtr);
      }
      else
      {
        // if more than 1, item points to an array of pointers that each points to their own string
        var lockPtr = memMgr.Lock(itemAsPtr);
        lockPtr += (IntPtr.Size * index);
        // is this even correct? I hope it is
        var subItemPtr = Marshal.PtrToStructure<IntPtr>(lockPtr);
        value = LockAndReadNullTerminatedString(memMgr, subItemPtr);
        memMgr.Unlock(itemAsPtr);
      }
      return value;
    }

    private string? LockAndReadNullTerminatedString(IMemoryManager memMgr, IntPtr data)
    {
      var lockPtr = memMgr.Lock(data);
      // yolo as ansi, should work in most cases
      var value = Marshal.PtrToStringAnsi(lockPtr);
      memMgr.Unlock(data);
      return value;
    }

    /// <summary>
    /// Frees all DS-allocated memory if necessary.
    /// </summary>
    /// <param name="memMgr"></param>
    internal unsafe void Free(IMemoryManager memMgr)
    {
      if (ReturnCode != TWRC.SUCCESS || !IsDataAPointer) return;

      var itemAsPtr = (IntPtr)Item.ToPointer(); // this is also iffy
      if (ItemType == TWTY.HANDLE && NumItems > 1)
      {
        // must go into each handle in the array and free them individually :(
        var lockPtr = memMgr.Lock(itemAsPtr);
        for (var i = 0; i < NumItems; i++)
        {
          // is this even correct? I hope it is
          var subItemPtr = Marshal.PtrToStructure<IntPtr>(lockPtr);
          memMgr.Free(subItemPtr);
          lockPtr += IntPtr.Size;
        }
      }
      memMgr.Free(itemAsPtr);
      Item = UIntPtr.Zero;
    }
  }
}
