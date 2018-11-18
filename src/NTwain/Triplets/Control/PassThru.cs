using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.PassThru"/>.
    /// </summary>
	public sealed class PassThru : BaseTriplet
	{
		internal PassThru(TwainSession session) : base(session) { }

        /// <summary>
        /// PASSTHRU is intended for the use of Source writers writing diagnostic applications. It allows
        /// raw communication with the currently selected device in the Source.
        /// </summary>
        /// <param name="data">The pass thru data.</param>
        /// <returns></returns>
        public ReturnCode PassThrough(ref TW_PASSTHRU data)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.PassThru, Message.PassThru, ref data);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.PassThru, Message.PassThru, ref data);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.PassThru, Message.PassThru, ref data);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.PassThru, Message.PassThru, ref data);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.PassThru, Message.PassThru, ref data);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.PassThru, Message.PassThru, ref data);

            return ReturnCode.Failure;
		}
	}
}