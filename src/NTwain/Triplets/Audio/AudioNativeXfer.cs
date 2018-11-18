using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets.Audio
{
	sealed class AudioNativeXfer : BaseTriplet
    {
		internal AudioNativeXfer(TwainSession session) : base(session) { }

		public ReturnCode Get(ref IntPtr handle)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Audio, DataArgumentType.AudioNativeXfer, Message.Get, ref handle);

            return ReturnCode.Failure;
        }
	}
}