using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets.Control
{
    sealed class Identity : BaseTriplet
    {
        internal Identity(TwainSession session) : base(session) { }

        ReturnCode DoIt(Message msg, TW_IDENTITY source)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, msg, source);
                else if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, msg, source);
                else if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, msg, source);
            }
            else
            {
                if (IsWin)
                    return NativeMethods.DsmWin64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, msg, source);
                else if (IsLinux)
                    return NativeMethods.DsmLinux64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, msg, source);
                else if (IsMac)
                    return NativeMethods.DsmMac64(Session.Config.App32, IntPtr.Zero,
                        DataGroups.Control, DataArgumentType.Identity, msg, source);
            }

            return ReturnCode.Failure;
        }

        public ReturnCode CloseDS(TW_IDENTITY source)
        {
            var rc = DoIt(Message.CloseDS, source);

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
            var rc = DoIt(Message.OpenDS, source);
            
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
            return DoIt(Message.GetDefault, source);
        }
        
        public ReturnCode GetFirst(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return DoIt(Message.GetFirst, source);
        }

        public ReturnCode GetNext(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return DoIt(Message.GetNext, source);
        }

        public ReturnCode Set(DataSource source)
        {
            // TODO: platform handling
            return DoIt(Message.Set, source?.Identity32);
        }

        public ReturnCode UserSelect(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return DoIt(Message.UserSelect, source);
        }
    }
}