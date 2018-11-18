using NTwain.Data;
using NTwain.Internals;
using System.Runtime.InteropServices;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.TwainDirect"/>.
    /// </summary>
    public sealed class TwainDirect : BaseTriplet
    {
        internal TwainDirect(TwainSession session) : base(session) { }

        /// <summary>
        /// Sends a TWAIN Direct task from the application to the driver.
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public ReturnCode SetTask(ref TW_TWAINDIRECT task)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.TwainDirect, Message.SetTask, ref task);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.TwainDirect, Message.SetTask, ref task);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.TwainDirect, Message.SetTask, ref task);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.TwainDirect, Message.SetTask, ref task);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.TwainDirect, Message.SetTask, ref task);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.TwainDirect, Message.SetTask, ref task);

            return ReturnCode.Failure;
        }
    }
}