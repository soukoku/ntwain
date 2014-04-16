using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	sealed class ImageMemXfer : OpBase
	{
		internal ImageMemXfer(ITwainStateInternal session) : base(session) { }

		/// <summary>
		/// This operation is used to initiate the transfer of an image from the Source to the application via
		/// the Buffered Memory transfer mechanism.
		/// </summary>
		/// <param name="xfer">The xfer.</param>
		/// <returns></returns>
		public ReturnCode Get(TWImageMemXfer xfer)
		{
			Session.VerifyState(6, 7, DataGroups.Image, DataArgumentType.ImageMemXfer, Message.Get);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Get, xfer);
		}
	}
}