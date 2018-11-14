using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    public partial class DGControl : BaseTriplet
    {
        internal DGControl(TwainSession session) : base(session) { }

        Parent _parent;
        internal Parent Parent => _parent ?? (_parent = new Parent(session));
    }
}
