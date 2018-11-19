using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets.Control
{
    sealed class EntryPoint : BaseTriplet
    {
        internal EntryPoint(TwainSession session) : base(session) { }

        public ReturnCode Get(out TW_ENTRYPOINT entry)
        {
            entry = new TW_ENTRYPOINT();
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entry);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entry);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entry);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entry);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entry);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entry);

            return ReturnCode.Failure;
        }
    }
}
