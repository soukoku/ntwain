using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.AudioFileXfer"/>.
    /// </summary>
	sealed class AudioFileXfer : OpBase
	{
        internal AudioFileXfer(ITwainSessionInternal session) : base(session) { }
		/// <summary>
		/// This operation is used to initiate the transfer of audio from the Source to the application via the
		/// disk-file transfer mechanism. It causes the transfer to begin.
		/// </summary>
		/// <returns></returns>
		public ReturnCode Get()
		{
			Session.VerifyState(6, 6, DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get);
			IntPtr z = IntPtr.Zero;
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref z);
		}
	}
}