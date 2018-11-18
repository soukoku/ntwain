using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.SetupMemXfer"/>.
    /// </summary>
    public sealed class SetupMemXfer : BaseTriplet
	{
		internal SetupMemXfer(TwainSession session) : base(session) { }

		/// <summary>
		/// Returns the Source’s preferred, minimum, and maximum allocation sizes for transfer memory
		/// buffers.
		/// </summary>
		/// <param name="setupMemXfer">The setup mem xfer.</param>
		/// <returns></returns>
        public ReturnCode Get(ref TW_SETUPMEMXFER setupMemXfer)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.SetupMemXfer, Message.Get, ref setupMemXfer);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.SetupMemXfer, Message.Get, ref setupMemXfer);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.SetupMemXfer, Message.Get, ref setupMemXfer);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.SetupMemXfer, Message.Get, ref setupMemXfer);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.SetupMemXfer, Message.Get, ref setupMemXfer);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.SetupMemXfer, Message.Get, ref setupMemXfer);

            return ReturnCode.Failure;
		}
	}
}