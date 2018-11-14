using NTwain.Data;
using NTwain.Internals;
using System;

namespace NTwain.Triplets
{
    sealed class Identity : BaseTriplet
    {
        internal Identity(TwainSession session) : base(session) { }

        public ReturnCode CloseDS(TW_IDENTITY source)
        {
            var rc = NativeMethods.DsmWin32(Session.Config.AppWin32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
            if (rc == ReturnCode.Success)
            {
                Session.State = TwainState.S3;
                Session.CurrentSource = null;
            }
            return rc;
        }

        public ReturnCode GetDefault(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return NativeMethods.DsmWin32(Session.Config.AppWin32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);
        }


        public ReturnCode GetFirst(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return NativeMethods.DsmWin32(Session.Config.AppWin32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);
        }

        public ReturnCode GetNext(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return NativeMethods.DsmWin32(Session.Config.AppWin32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);
        }

        public ReturnCode OpenDS(TW_IDENTITY source)
        {
            var rc = NativeMethods.DsmWin32(Session.Config.AppWin32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.OpenDS, source);
            if (rc == ReturnCode.Success)
            {
                Session.CurrentSource = Session.GetSourceSingleton(source);
                Session.State = TwainState.S4;
            }
            return rc;
        }

        public ReturnCode Set(DataSource source)
        {
            return NativeMethods.DsmWin32(Session.Config.AppWin32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity);
        }

        public ReturnCode UserSelect(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return NativeMethods.DsmWin32(Session.Config.AppWin32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);
        }
    }
}