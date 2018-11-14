using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataGroups.Image"/>.
    /// </summary>
    public partial class DGImage : BaseTriplet
    {
        internal DGImage(TwainSession session) : base(session) { }

    }
}
