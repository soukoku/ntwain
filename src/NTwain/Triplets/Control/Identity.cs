using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets.Control
{
    sealed class Identity : BaseTriplet
    {
        internal Identity(TwainSession session) : base(session) { }

        public ReturnCode CloseDS(TW_IDENTITY source)
        {
            var rc = ReturnCode.Failure;

            if (Is32Bit)
            {
                if (IsWin)
                    rc = NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
                else if (IsLinux)
                    rc = NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
                else if (IsMac)
                    rc = NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
            }
            else
            {
                if (IsWin)
                    rc = NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
                else if (IsLinux)
                    rc = NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
                else if (IsMac)
                    rc = NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
            }

            if (rc == ReturnCode.Success)
            {
                Session.State = TwainState.S3;
                Session.CurrentSource = null;
            }
            return rc;
        }

        public ReturnCode OpenDS(TW_IDENTITY source)
        {
            Session.StepDown(TwainState.DsmOpened);
            var rc = ReturnCode.Failure;

            if (Is32Bit)
            {
                if (IsWin)
                    rc = NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.OpenDS, source);
                else if (IsLinux)
                    rc = NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.OpenDS, source);
                else if (IsMac)
                    rc = NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.OpenDS, source);
            }
            else
            {
                if (IsWin)
                    rc = NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.OpenDS, source);
                else if (IsLinux)
                    rc = NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.OpenDS, source);
                else if (IsMac)
                    rc = NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.OpenDS, source);
            }

            if (rc == ReturnCode.Success)
            {
                Session.CurrentSource = Session.GetSourceSingleton(source);
                Session.State = TwainState.S4;
                Session.RegisterCallback();
            }
            return rc;
        }

        public ReturnCode GetDefault(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);

            return ReturnCode.Failure;
        }
        
        public ReturnCode GetFirst(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);

            return ReturnCode.Failure;
        }

        public ReturnCode GetNext(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);

            return ReturnCode.Failure;
        }

        public ReturnCode Set(DataSource source)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity32);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity32);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity32);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity32);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity32);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity32);

            return ReturnCode.Failure;
        }

        public ReturnCode UserSelect(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);

            return ReturnCode.Failure;
        }
    }
}