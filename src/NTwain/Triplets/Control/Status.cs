using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    sealed class Status : BaseTriplet
    {
        internal Status(TwainSession session) : base(session) { }

        public ReturnCode Get(ref TW_STATUS status, DataSource source)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, source?.Identity32,
                        DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, source?.Identity32,
                        DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, source?.Identity32,
                        DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
            }
            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, source?.Identity32,
                    DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, source?.Identity32,
                    DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, source?.Identity32,
                    DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);

            return ReturnCode.Failure;
        }
    }
}