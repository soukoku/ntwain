using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.SetupFileXfer"/>.
    /// </summary>
	public sealed class SetupFileXfer : OpBase
	{
		internal SetupFileXfer(ITwainStateInternal session) : base(session) { }
		/// <summary>
		/// Returns information about the file into which the Source has or will put the acquired image
		/// or audio data.
		/// </summary>
		/// <param name="setupFileXfer">The setup file xfer.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWSetupFileXfer setupFileXfer)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.SetupFileXfer, Message.Get);
			setupFileXfer = new TWSetupFileXfer();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, setupFileXfer);
		}

		/// <summary>
		/// Returns information for the default image or audio file.
		/// </summary>
		/// <param name="setupFileXfer">The setup file xfer.</param>
		/// <returns></returns>
		public ReturnCode GetDefault(out TWSetupFileXfer setupFileXfer)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.SetupFileXfer, Message.GetDefault);
			setupFileXfer = new TWSetupFileXfer();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.GetDefault, setupFileXfer);
		}

		/// <summary>
		/// Resets the current file information to the image or audio default file information and
		/// returns that default information.
		/// </summary>
		/// <param name="setupFileXfer">The setup file xfer.</param>
		/// <returns></returns>
		public ReturnCode Reset(out TWSetupFileXfer setupFileXfer)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.SetupFileXfer, Message.Reset);
			setupFileXfer = new TWSetupFileXfer();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, setupFileXfer);
		}

		/// <summary>
		/// Sets the file transfer information for the next file transfer. The application is responsible for
		/// verifying that the specified file name is valid and that the file either does not currently exist (in
		/// which case, the Source is to create the file), or that the existing file is available for opening and
		/// read/write operations. The application should also assure that the file format it is requesting
		/// can be provided by the Source
		/// </summary>
		/// <param name="setupFileXfer">The setup file xfer.</param>
		/// <returns></returns>
		public ReturnCode Set(TWSetupFileXfer setupFileXfer)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.SetupFileXfer, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, setupFileXfer);
		}

	}
}