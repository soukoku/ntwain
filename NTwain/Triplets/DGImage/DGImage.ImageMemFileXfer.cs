using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	sealed class ImageMemFileXfer : OpBase
	{
		internal ImageMemFileXfer(TwainSession session) : base(session) { }

		/// <summary>
		/// This operation is used to initiate the transfer of an image from the Source to the application via
		/// the Memory-File transfer mechanism.
		/// </summary>
		/// <param name="xfer">The xfer.</param>
		/// <returns></returns>
		public ReturnCode Get(TWImageMemXfer xfer)
		{
			Session.VerifyState(6, 6, DataGroups.Image, DataArgumentType.ImageMemFileXfer, Message.Get);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, xfer);
		}

	}
}