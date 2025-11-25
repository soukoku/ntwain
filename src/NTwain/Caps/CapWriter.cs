using NTwain.Data;
using System.Collections.Generic;

namespace NTwain.Caps;

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
        return LastSTS = _twain.SetCap(Cap, values);
    }

    /// <summary>
    /// Sets current value using enumeration.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS Set(Enumeration<TValue> values)
    {
        return LastSTS = _twain.SetCap(Cap, values);
    }

    /// <summary>
    /// Sets current value using range.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS Set(Range<TValue> values)
    {
        return LastSTS = _twain.SetCap(Cap, values);
    }


    /// <summary>
    /// Sets constraint using one-value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public STS SetConstraint(TValue value)
    {
        return LastSTS = _twain.SetConstraint(Cap, value);
    }

    /// <summary>
    /// Sets constraint using array.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS SetConstraint(IList<TValue> values)
    {
        return LastSTS = _twain.SetConstraint(Cap, values);
    }

    /// <summary>
    /// Sets constraint using enumeration.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS SetConstraint(Enumeration<TValue> values)
    {
        return LastSTS = _twain.SetConstraint(Cap, values);
    }

    /// <summary>
    /// Sets constraint using range.
    /// </summary>
    /// <param name="values"></param>
    /// <returns></returns>
    public STS SetConstraint(Range<TValue> values)
    {
        return LastSTS = _twain.SetConstraint(Cap, values);
    }

    /// <summary>
    /// Resets this cap to power-on default.
    /// </summary>
    /// <param name="value">The current value after reset.</param>
    /// <returns></returns>
    public STS Reset(out List<TValue> value)
    {
        LastSTS = _twain.ResetCap(Cap, out value);
        return LastSTS;
    }
}
