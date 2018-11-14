using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    sealed partial class Parent : BaseTriplet
    {
        internal Parent(TwainSession session) : base(session) { }

        public ReturnCode OpenDSM(IntPtr hWnd)
        {
            var rc = NativeMethods.DsmWin32(Session.Config.AppWin32, null, 
                DataGroups.Control, DataArgumentType.Parent, Message.OpenDSM, ref hWnd);
            if (rc == ReturnCode.Success)
            {
                Session.State = TwainState.DsmOpened;

                // if twain2 then get memory management functions
                if ((Session.Config.AppWin32.DataFunctionalities & DataFunctionalities.Dsm2) == DataFunctionalities.Dsm2)
                {
                    TW_ENTRYPOINT entry;
                    rc = Session.DGControl.EntryPoint.Get(out entry);
                    if (rc == ReturnCode.Success)
                    {
                        Session.Config.MemoryManager = entry;
                    }
                    else
                    {
                        rc = CloseDSM(hWnd);
                    }
                }
            }
            return rc;
        }

        public ReturnCode CloseDSM(IntPtr hWnd)
        {
            var rc = NativeMethods.DsmWin32(Session.Config.AppWin32, null, DataGroups.Control, 
                DataArgumentType.Parent, Message.CloseDSM, ref hWnd);
            if (rc == ReturnCode.Success) Session.State = TwainState.DsmLoaded;
            return rc;
        }
    }
}
