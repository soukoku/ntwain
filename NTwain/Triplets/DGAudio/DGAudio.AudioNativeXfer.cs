using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.AudioNativeXfer"/>.
    /// </summary>
	sealed class AudioNativeXfer : OpBase
	{
		internal AudioNativeXfer(ITwainSessionInternal session) : base(session) { }
		/// <summary>
		/// Causes the transfer of an audio data from the Source to the application, via the Native
		/// transfer mechanism, to begin. The resulting data is stored in main memory in a single block.
		/// The data is stored in AIFF format on the Macintosh and as a WAV format under Microsoft
		/// Windows. The size of the audio snippet that can be transferred is limited to the size of the
		/// memory block that can be allocated by the Source.
		/// </summary>
		/// <param name="handle">The handle.</param>
		/// <returns></returns>
		public ReturnCode Get(ref IntPtr handle)
		{
			Session.VerifyState(6, 6, DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);
		}
	}
}