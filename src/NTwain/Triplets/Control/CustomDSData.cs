using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
	sealed class CustomDSData : BaseTriplet
	{
		internal CustomDSData(TwainSession session) : base(session) { }

        ReturnCode DoIt(Message msg, ref TW_CUSTOMDSDATA data)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.CustomDSData, msg, ref data);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.CustomDSData, msg, ref data);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.CustomDSData, msg, ref data);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.CustomDSData, msg, ref data);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.CustomDSData, msg, ref data);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.CustomDSData, msg, ref data);

            return ReturnCode.Failure;
        }

        public ReturnCode Get(ref TW_CUSTOMDSDATA data)
        {
            return DoIt(Message.Get, ref data);
		}

		public ReturnCode Set(ref TW_CUSTOMDSDATA data)
        {
            return DoIt(Message.Set, ref data);
        }
	}
}