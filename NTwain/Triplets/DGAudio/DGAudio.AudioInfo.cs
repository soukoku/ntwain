using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.AudioInfo"/>.
    /// </summary>
	public sealed class AudioInfo : OpBase
	{
		internal AudioInfo(ITwainStateInternal session) : base(session) { }
		/// <summary>
		/// Used to get the information of the current audio data ready to transfer.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWAudioInfo info)
		{
			Session.VerifyState(6, 7, DataGroups.Audio, DataArgumentType.AudioInfo, Message.Get);
			info = new TWAudioInfo();
			return PInvoke.DsmEntry(Session.GetAppId(), Session.SourceId, Message.Get, info);
		}
	}
}