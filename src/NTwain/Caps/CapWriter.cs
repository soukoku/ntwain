using NTwain.Data;
using System.Collections.Generic;

namespace NTwain.Caps
{
  public class CapWriter<TValue> : CapReader<TValue> where TValue : struct
  {
    public CapWriter(TwainAppSession twain, CAP cap, float introducedVersion = 1)
      : base(twain, cap, introducedVersion)
    {
    }

    /// <summary>
    /// Sets current value using one-value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS Set(TValue value)
    {
      return LastSTS = _twain.SetCap(Cap, value);
    }

    /// <summary>
    /// Sets current value using array.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS Set(IList<TValue> values)
    {
      var twcap = ValueWriter.CreateArrayCap(Cap, _twain, values);
      return LastSTS = _twain.SetCap(ref twcap);
    }

    /// <summary>
    /// Sets current value using enumeration.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS Set(Enumeration<TValue> values)
    {
      var twcap = ValueWriter.CreateEnumCap(Cap, _twain, values);
      return LastSTS = _twain.SetCap(ref twcap);
    }

    /// <summary>
    /// Sets current value using range.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS Set(Range<TValue> values)
    {
      var twcap = ValueWriter.CreateRangeCap(Cap, _twain, values);
      return LastSTS = _twain.SetCap(ref twcap);
    }


    /// <summary>
    /// Sets constraint using one-value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetConstraint(TValue value)
    {
      var twcap = ValueWriter.CreateOneValueCap(Cap, _twain, value);
      return LastSTS = _twain.SetConstraint(ref twcap);
    }

    /// <summary>
    /// Sets constraint using array.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS SetConstraint(IList<TValue> values)
    {
      var twcap = ValueWriter.CreateArrayCap(Cap, _twain, values);
      return LastSTS = _twain.SetConstraint(ref twcap);
    }

    /// <summary>
    /// Sets constraint using enumeration.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS SetConstraint(Enumeration<TValue> values)
    {
      var twcap = ValueWriter.CreateEnumCap(Cap, _twain, values);
      return LastSTS = _twain.SetConstraint(ref twcap);
    }

    /// <summary>
    /// Sets constraint using range.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS SetConstraint(Range<TValue> values)
    {
      var twcap = ValueWriter.CreateRangeCap(Cap, _twain, values);
      return LastSTS = _twain.SetConstraint(ref twcap);
    }

    /// <summary>
    /// Resets this cap to power-on default.
    /// </summary>
    /// <returns></returns>
    public STS Reset()
    {
      LastSTS = _twain.ResetCap(Cap, out TW_CAPABILITY twcap);
      twcap.Free(_twain);
      return LastSTS;
    }
  }
}
