using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets.Audio
{
	sealed class AudioFileXfer : BaseTriplet
    {
        internal AudioFileXfer(TwainSession session) : base(session) { }

		public ReturnCode Get()
		{
			IntPtr zero = IntPtr.Zero;
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref zero);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref zero);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref zero);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref zero);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref zero);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioFileXfer, Message.Get, ref zero);

            return ReturnCode.Failure;
        }
	}
}