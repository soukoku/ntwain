using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
	sealed class ImageMemFileXfer : OpBase
	{
		internal ImageMemFileXfer(ITwainSessionInternal session) : base(session) { }

		/// <summary>
		/// This operation is used to initiate the transfer of an image from the Source to the application via
		/// the Memory-File transfer mechanism.
		/// </summary>
		/// <param name="xfer">The xfer.</param>
		/// <returns></returns>
		public ReturnCode Get(TWImageMemXfer xfer)
		{
			Session.VerifyState(6, 7, DataGroups.Image, DataArgumentType.ImageMemFileXfer, Message.Get);
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, xfer);
		}

	}
}