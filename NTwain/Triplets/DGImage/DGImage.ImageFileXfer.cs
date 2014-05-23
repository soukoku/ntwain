using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ImageFileXfer"/>.
    /// </summary>
	sealed class ImageFileXfer : OpBase
	{
		internal ImageFileXfer(ITwainSessionInternal session) : base(session) { }

		/// <summary>
		/// This operation is used to initiate the transfer of an image from the Source to the application via
		/// the disk-file transfer mechanism. It causes the transfer to begin.
		/// </summary>
		/// <returns></returns>
		public ReturnCode Get()
		{
			Session.VerifyState(6, 6, DataGroups.Image, DataArgumentType.ImageFileXfer, Message.Get);
			IntPtr z = IntPtr.Zero;
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, DataGroups.Image, DataArgumentType.ImageFileXfer, Message.Get, ref z);
		}
	}
}