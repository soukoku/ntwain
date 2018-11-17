using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
	sealed class DeviceEvent : BaseTriplet
	{
		internal DeviceEvent(TwainSession session) : base(session) { }

        public ReturnCode Get(ref TW_DEVICEEVENT evt)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref evt);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref evt);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref evt);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref evt);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref evt);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref evt);

            return ReturnCode.Failure;
        }
	}
}