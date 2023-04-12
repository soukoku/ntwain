using NTwain.Data;
using System;
using System.Collections.Generic;

namespace NTwain.Caps
{
  public class CapReader<TValue> where TValue : struct
  {
    readonly TwainAppSession _twain;

    public CapReader(TwainAppSession twain, CAP cap)
    {
      _twain = twain;
      Cap = cap;
    }

    public CAP Cap { get; }

    public STS LastSTS { get; private set; }

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

    public TValue GetCurrent()
    {
      LastSTS = _twain.GetCapCurrent(Cap, out TValue value);
      if (LastSTS.IsSuccess)
      {
        return value;
      };
      return default;
    }

    public TValue GetDefault()
    {
      LastSTS = _twain.GetCapDefault(Cap, out TValue value);
      if (LastSTS.IsSuccess)
      {
        return value;
      };
      return default;
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
