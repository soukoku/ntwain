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

        public ReturnCode OpenDSM(ref IntPtr hWnd)
        {
            var rc = NativeMethods.DsmWin32(session.Config.AppWin32, null, 
                DataGroups.Control, DataArgumentType.Parent, Message.OpenDSM, ref hWnd);
            if (rc == ReturnCode.Success)
            {
                session.State = TwainState.DsmOpened;

                // if twain2 then get memory management functions
                if ((session.Config.AppWin32.DataFunctionalities & DataFunctionalities.Dsm2) == DataFunctionalities.Dsm2)
                {
                    TW_ENTRYPOINT entry;
                    rc = session.DGControl.EntryPoint.Get(out entry);
                    if (rc == ReturnCode.Success)
                    {
                        session.Config.MemoryManager = entry;
                    }
                    else
                    {
                        rc = CloseDSM(ref hWnd);
                    }
                }
            }
            return rc;
        }

        public ReturnCode CloseDSM(ref IntPtr hWnd)
        {
            var rc = NativeMethods.DsmWin32(session.Config.AppWin32, null, DataGroups.Control, 
                DataArgumentType.Parent, Message.CloseDSM, ref hWnd);
            if (rc == ReturnCode.Success) session.State = TwainState.DsmLoaded;
            return rc;
        }
    }
}
