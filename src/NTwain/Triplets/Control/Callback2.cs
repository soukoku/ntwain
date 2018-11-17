using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    sealed class Callback2 : BaseTriplet
    {
        internal Callback2(TwainSession session) : base(session) { }

        public ReturnCode RegisterCallback(ref TW_CALLBACK2 callback2)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, ref callback2);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, ref callback2);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, ref callback2);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, ref callback2);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, ref callback2);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, ref callback2);

            return ReturnCode.Failure;
        }
    }
}