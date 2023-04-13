using NTwain.Data;

namespace NTwain.Caps
{
  /// <summary>
  /// Provides reader/writer wrapper of known <see cref="CAP"/>s.
  /// </summary>
  public partial class KnownCaps
  {
    protected readonly TwainAppSession _twain;

    public KnownCaps(TwainAppSession twain)
    {
      _twain = twain;
    }

  }
}
