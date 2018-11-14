using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets
{
    public partial class Parent : BaseTriplet
    {
        internal Parent(TwainSession session) : base(session) { }

        public ReturnCode OpenDSM(ref IntPtr hWnd)
        {
            var rc = NativeMethods.DsmWin32(session.Config.AppWin32, null, DataGroups.Control, DataArgumentType.Parent, Message.OpenDSM, ref hWnd);
            if (rc == ReturnCode.Success) session.State = TwainState.DsmOpened;
            return rc;
        }

        public ReturnCode CloseDSM(ref IntPtr hWnd)
        {
            var rc = NativeMethods.DsmWin32(session.Config.AppWin32, null, DataGroups.Control, DataArgumentType.Parent, Message.CloseDSM, ref hWnd);
            if (rc == ReturnCode.Success) session.State = TwainState.DsmLoaded;
            return rc;
        }
    }
}
