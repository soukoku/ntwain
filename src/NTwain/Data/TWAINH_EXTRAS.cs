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
  public static class TwainPlatform
  {
    static TwainPlatform()
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
  public static class TwainConst
  {
    /// <summary>
    /// Don't care values...
    /// </summary>
    public const byte TWON_DONTCARE8 = 0xff;
    public const ushort TWON_DONTCARE16 = 0xffff;
    public const uint TWON_DONTCARE32 = 0xffffffff;
    /// <summary>
    /// We're departing from a strict translation of H so that
    /// we can achieve a unified status return type.  
    /// </summary>
    public const int STSCC = 0x10000; // get us past the custom space

  }

  /// <summary>
  /// TWAIN's boolean values.
  /// </summary>
  public enum BoolType : ushort
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
      if (!(MinValue is IConvertible))
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
      return $"{Left},{Top},{Right},{Bottom}";
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
      ConType = (TWON)TwainConst.TWON_DONTCARE16;
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

  //partial struct TW_DEVICEEVENT
  //{
  //    public TWDE Event { get { return (TWDE)_event; } }
  //    public TWFL FlashUsed2 { get { return (TWFL)_flashUsed2; } }
  //}
}
