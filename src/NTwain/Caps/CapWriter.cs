using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Caps
{
  public class CapWriter<TValue> : CapReader<TValue> where TValue : struct
  {
    public CapWriter(TwainAppSession twain, CAP cap) : base(twain, cap)
    {
    }
  }
}
