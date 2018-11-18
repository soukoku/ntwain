using NTwain.Data;
using NTwain.Triplets.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataGroups.Audio"/>.
	/// </summary>
    partial class DGAudio : BaseTriplet
    {
        internal DGAudio(TwainSession session) : base(session) { }

        AudioFileXfer _fileXfer;
        internal AudioFileXfer AudioFileXfer => _fileXfer ?? (_fileXfer = new AudioFileXfer(Session));

        AudioInfo _info;
        internal AudioInfo AudioInfo => _info ?? (_info = new AudioInfo(Session));

        AudioNativeXfer _natXfer;
        internal AudioNativeXfer AudioNativeXfer => _natXfer ?? (_natXfer = new AudioNativeXfer(Session));
    }
}
