using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Audio
{
    sealed class AudioInfo : BaseTriplet
    {
        internal AudioInfo(TwainSession session) : base(session) { }

        public ReturnCode Get(ref TW_AUDIOINFO info)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioInfo, Message.Get, ref info);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioInfo, Message.Get, ref info);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioInfo, Message.Get, ref info);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioInfo, Message.Get, ref info);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioInfo, Message.Get, ref info);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioInfo, Message.Get, ref info);

            return ReturnCode.Failure;
        }
    }
}