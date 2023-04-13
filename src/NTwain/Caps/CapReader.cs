using NTwain.Data;
using System;
using System.Collections.Generic;

namespace NTwain.Caps
{
  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="TValue"></typeparam>
  public class CapReader<TValue> where TValue : struct
  {
    protected readonly TwainAppSession _twain;

    public CapReader(TwainAppSession twain, CAP cap, float introducedVersion = 1)
    {
      _twain = twain;
      Cap = cap;
      Introduced = introducedVersion;
    }

    public CAP Cap { get; }

    /// <summary>
    /// When this was introduced in TWAIN.
    /// </summary>
    public float Introduced { get; }

    /// <summary>
    /// The STS result from the most recent call with this cap wrapper.
    /// </summary>
    public STS LastSTS { get; protected set; }

    TWQC? _qc;
    public TWQC Supports
    {
      get
      {
        if (!_qc.HasValue) _qc = _twain.QueryCapSupport(Cap);
        return _qc.Value;
      }
    }

    public IList<TValue> Get()
    {
      LastSTS = _twain.GetCapValues(Cap, out IList<TValue> values);
      if (LastSTS.IsSuccess)
      {
        return values;
      };
      return Array.Empty<TValue>();
    }

    public IList<TValue> GetCurrent()
    {
      LastSTS = _twain.GetCapCurrent(Cap, out List<TValue> value);
      if (LastSTS.IsSuccess)
      {
        return value;
      };
      return Array.Empty<TValue>();
    }

    public IList<TValue> GetDefault()
    {
      LastSTS = _twain.GetCapDefault(Cap, out List<TValue> value);
      if (LastSTS.IsSuccess)
      {
        return value;
      };
      return Array.Empty<TValue>();
    }

    public string? GetLabel()
    {
      LastSTS = _twain.GetCapLabel(Cap, out string? value);
      if (LastSTS.IsSuccess)
      {
        return value;
      };
      return default;
    }

    public string? GetHelp()
    {
      LastSTS = _twain.GetCapHelp(Cap, out string? value);
      if (LastSTS.IsSuccess)
      {
        return value;
      };
      return default;
    }

    public IList<string> GetLabelEnum()
    {
      LastSTS = _twain.GetCapLabelEnum(Cap, out IList<string> value);
      if (LastSTS.IsSuccess)
      {
        return value;
      };
      return Array.Empty<string>();
    }
  }
}
