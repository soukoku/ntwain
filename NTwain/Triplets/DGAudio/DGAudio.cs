using NTwain.Values;
using System;
namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataGroups.Audio"/>.
	/// </summary>
	public sealed class DGAudio
	{
        ITwainSessionInternal _session;
        internal DGAudio(ITwainSessionInternal session)
		{
			if (session == null) { throw new ArgumentNullException("session"); }
			_session = session;
		}


		AudioFileXfer _audioFileXfer;
		internal AudioFileXfer AudioFileXfer
		{
			get
			{
				if (_audioFileXfer == null) { _audioFileXfer = new AudioFileXfer(_session); }
				return _audioFileXfer;
			}
		}

		AudioInfo _audioInfo;
		public AudioInfo AudioInfo
		{
			get
			{
				if (_audioInfo == null) { _audioInfo = new AudioInfo(_session); }
				return _audioInfo;
			}
		}

		AudioNativeXfer _audioNativeXfer;
		internal AudioNativeXfer AudioNativeXfer
		{
			get
			{
				if (_audioNativeXfer == null) { _audioNativeXfer = new AudioNativeXfer(_session); }
				return _audioNativeXfer;
			}
		}
	}
}
