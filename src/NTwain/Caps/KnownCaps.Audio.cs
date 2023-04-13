using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTwain.Caps
{
  partial class KnownCaps
  {

    CapWriter<TWSX>? _ACAP_XFERMECH;
    public CapWriter<TWSX> ACAP_XFERMECH => _ACAP_XFERMECH ??= new(_twain, CAP.ACAP_XFERMECH, 1.8f);
  }
}
