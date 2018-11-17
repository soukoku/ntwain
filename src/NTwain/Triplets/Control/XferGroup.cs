using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.XferGroup"/>.
    /// </summary>
    public sealed class XferGroup : BaseTriplet
	{
		internal XferGroup(TwainSession session) : base(session) { }

		/// <summary>
		/// Returns the Data Group (the type of data) for the upcoming transfer. The Source is required to
		/// only supply one of the DGs specified in the SupportedGroups field of origin.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
        public ReturnCode Get(ref DataGroups value)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.XferGroup, Message.Get, ref value);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.XferGroup, Message.Get, ref value);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.XferGroup, Message.Get, ref value);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.XferGroup, Message.Get, ref value);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.XferGroup, Message.Get, ref value);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.XferGroup, Message.Get, ref value);

            return ReturnCode.Failure;
		}

        /// <summary>
        /// The transfer group determines the kind of data being passed from the Source to the Application.
        /// By default a TWAIN Source must default to DG_IMAGE. Currently the only other data group
        /// supported is DG_AUDIO, which is a feature supported by some digital cameras.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public ReturnCode Set(DataGroups value)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.XferGroup, Message.Set, ref value);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.XferGroup, Message.Set, ref value);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.XferGroup, Message.Set, ref value);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.XferGroup, Message.Set, ref value);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.XferGroup, Message.Set, ref value);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.XferGroup, Message.Set, ref value);

            return ReturnCode.Failure;
        }
	}
}