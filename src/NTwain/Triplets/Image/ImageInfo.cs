using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Image
{
    sealed class ImageInfo : BaseTriplet
    {
        internal ImageInfo(TwainSession session) : base(session) { }

        public ReturnCode Get(ref TW_IMAGEINFO info)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Image, DataArgumentType.ImageInfo, Message.Get, ref info);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Image, DataArgumentType.ImageInfo, Message.Get, ref info);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Image, DataArgumentType.ImageInfo, Message.Get, ref info);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Image, DataArgumentType.ImageInfo, Message.Get, ref info);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Image, DataArgumentType.ImageInfo, Message.Get, ref info);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Image, DataArgumentType.ImageInfo, Message.Get, ref info);

            return ReturnCode.Failure;
        }
    }
}