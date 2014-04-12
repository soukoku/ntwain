using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.JpegCompression"/>.
    /// </summary>
	public sealed class JpegCompression : OpBase
	{
		internal JpegCompression(ITwainStateInternal session) : base(session) { }

		/// <summary>
		/// Causes the Source to return the parameters that will be used during the compression of data
		/// using the JPEG algorithms.
		/// </summary>
		/// <param name="compression">The compression.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWJpegCompression compression)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.JpegCompression, Message.Get);
			compression = new TWJpegCompression();
			return Dsm.DsmEntry(Session.GetAppId(), Session.SourceId, Message.Get, compression);
		}

		/// <summary>
		/// Causes the Source to return the power-on default values applied to JPEG-compressed data
		/// transfers.
		/// </summary>
		/// <param name="compression">The compression.</param>
		/// <returns></returns>
		public ReturnCode GetDefault(out TWJpegCompression compression)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.JpegCompression, Message.GetDefault);
			compression = new TWJpegCompression();
			return Dsm.DsmEntry(Session.GetAppId(), Session.SourceId, Message.GetDefault, compression);
		}

		/// <summary>
		/// Return the Source to using its power-on default values for JPEG-compressed transfers.
		/// </summary>
		/// <param name="compression">The compression.</param>
		/// <returns></returns>
		public ReturnCode Reset(out TWJpegCompression compression)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.JpegCompression, Message.Reset);
			compression = new TWJpegCompression();
			return Dsm.DsmEntry(Session.GetAppId(), Session.SourceId, Message.Reset, compression);
		}

		/// <summary>
		/// Allows the application to configure the compression parameters to be used on all future JPEGcompressed
		/// transfers during the current session. The application should have already
		/// established that the requested values are supported by the Source.
		/// </summary>
		/// <param name="compression">The compression.</param>
		/// <returns></returns>
		public ReturnCode Set(TWJpegCompression compression)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.JpegCompression, Message.Set);
			return Dsm.DsmEntry(Session.GetAppId(), Session.SourceId, Message.Set, compression);
		}
	}
}