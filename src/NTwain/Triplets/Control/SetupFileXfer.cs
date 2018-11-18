using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.SetupFileXfer"/>.
    /// </summary>
    public sealed class SetupFileXfer : BaseTriplet
	{
		internal SetupFileXfer(TwainSession session) : base(session) { }

        ReturnCode DoIt(Message msg, ref TW_SETUPFILEXFER xfer)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.SetupFileXfer, msg, ref xfer);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.SetupFileXfer, msg, ref xfer);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.SetupFileXfer, msg, ref xfer);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.SetupFileXfer, msg, ref xfer);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.SetupFileXfer, msg, ref xfer);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.SetupFileXfer, msg, ref xfer);

            return ReturnCode.Failure;
        }

        /// <summary>
        /// Returns information about the file into which the Source has or will put the acquired image
        /// or audio data.
        /// </summary>
        /// <param name="setupFileXfer">The setup file xfer.</param>
        /// <returns></returns>
        public ReturnCode Get(ref TW_SETUPFILEXFER setupFileXfer)
		{
            return DoIt(Message.Get, ref setupFileXfer);
		}

		/// <summary>
		/// Returns information for the default image or audio file.
		/// </summary>
		/// <param name="setupFileXfer">The setup file xfer.</param>
		/// <returns></returns>
        public ReturnCode GetDefault(ref TW_SETUPFILEXFER setupFileXfer)
        {
            return DoIt(Message.GetDefault, ref setupFileXfer);
        }

		/// <summary>
		/// Resets the current file information to the image or audio default file information and
		/// returns that default information.
		/// </summary>
		/// <param name="setupFileXfer">The setup file xfer.</param>
		/// <returns></returns>
        public ReturnCode Reset(ref TW_SETUPFILEXFER setupFileXfer)
        {
            return DoIt(Message.Reset, ref setupFileXfer);
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
        public ReturnCode Set(ref TW_SETUPFILEXFER setupFileXfer)
        {
            return DoIt(Message.Set, ref setupFileXfer);
        }

	}
}