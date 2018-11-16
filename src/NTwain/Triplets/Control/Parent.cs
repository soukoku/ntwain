using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets.Control
{
    sealed partial class Parent : BaseTriplet
    {
        internal Parent(TwainSession session) : base(session) { }

        public ReturnCode OpenDSM(IntPtr hWnd)
        {
            var rc = ReturnCode.Failure;

            bool isDsm2 = false;

            if (Use32BitData)
            {
                rc = NativeMethods.Dsm32(Session.Config.App32, null,
                    DataGroups.Control, DataArgumentType.Parent, Message.OpenDSM, ref hWnd);

                isDsm2 = rc == ReturnCode.Success &&
                    (Session.Config.App32.DataFunctionalities & DataFunctionalities.Dsm2) == DataFunctionalities.Dsm2;
            }

            if (rc == ReturnCode.Success)
            {
                Session.State = TwainState.DsmOpened;

                if (isDsm2)
                {
                    rc = Session.DGControl.EntryPoint.Get(out TW_ENTRYPOINT entry);
                    if (rc == ReturnCode.Success)
                    {
                        Session.Config.MemoryManager = entry;
                    }
                }
            }
            return rc;
        }

        public ReturnCode CloseDSM(IntPtr hWnd)
        {
            var rc = ReturnCode.Failure;

            if (Use32BitData)
            {
                rc = NativeMethods.Dsm32(Session.Config.App32, null, DataGroups.Control,
                    DataArgumentType.Parent, Message.CloseDSM, ref hWnd);
            }

            if (rc == ReturnCode.Success)
            {
                Session.Config.MemoryManager = null;
                Session.State = TwainState.DsmLoaded;
            }
            return rc;
        }
    }
}
