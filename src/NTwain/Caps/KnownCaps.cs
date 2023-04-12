using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Caps
{
  /// <summary>
  /// Provides reader/writer wrapper of known <see cref="CAP"/>s.
  /// </summary>
  public partial class KnownCaps
  {
    private readonly TwainAppSession _twain;

    public KnownCaps(TwainAppSession twain)
    {
      _twain = twain;
    }

  }
}
