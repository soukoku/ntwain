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

            if (Use32BitData)
            {
                rc = NativeMethods.Dsm32(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.CloseDS, source);
            }

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
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetDefault, source);
            }
            return ReturnCode.Failure;
        }


        public ReturnCode GetFirst(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.GetFirst, source);
            }
            return ReturnCode.Failure;
        }

        public ReturnCode GetNext(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            return NativeMethods.Dsm32(Session.Config.App32, IntPtr.Zero,
                DataGroups.Control, DataArgumentType.Identity, Message.GetNext, source);
        }

        public ReturnCode OpenDS(TW_IDENTITY source)
        {
            Session.StepDown(TwainState.DsmOpened);
            var rc = ReturnCode.Failure;

            if (Use32BitData)
            {
                rc = NativeMethods.Dsm32(Session.Config.App32, IntPtr.Zero,
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

        public ReturnCode Set(DataSource source)
        {
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.Set, source?.Identity);
            }
            return ReturnCode.Failure;
        }

        public ReturnCode UserSelect(out TW_IDENTITY source)
        {
            source = new TW_IDENTITY();
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, IntPtr.Zero,
                    DataGroups.Control, DataArgumentType.Identity, Message.UserSelect, source);
            }
            return ReturnCode.Failure;
        }
    }
}