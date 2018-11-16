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

        public ReturnCode Get(out TW_ENTRYPOINT entryPoint)
        {
            entryPoint = new TW_ENTRYPOINT();
            return NativeMethods.DsmWin32(Session.Config.AppWin32, null, 
                DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entryPoint);
        }
    }
}
