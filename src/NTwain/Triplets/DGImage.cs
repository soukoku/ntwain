using NTwain.Data;
using NTwain.Triplets.Image;
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

        ImageInfo _info;
        internal ImageInfo ImageInfo => _info ?? (_info = new ImageInfo(Session));
    }
}
