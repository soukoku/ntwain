using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataGroups.Audio"/>.
	/// </summary>
    public partial class DGAudio : BaseTriplet
    {
        internal DGAudio(TwainSession session) : base(session) { }

    }
}
