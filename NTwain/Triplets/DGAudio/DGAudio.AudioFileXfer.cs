using System;
using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
	sealed class AudioFileXfer : OpBase
	{
		internal AudioFileXfer(TwainSession session) : base(session) { }
		/// <summary>
		/// This operation is used to initiate the transfer of audio from the Source to the application via the
		/// disk-file transfer mechanism. It causes the transfer to begin.
		/// </summary>
		/// <returns></returns>
		public ReturnCode Get()
		{
			Session.VerifyState(6, 6, DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get);
			IntPtr z = IntPtr.Zero;
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref z);
		}
	}
}