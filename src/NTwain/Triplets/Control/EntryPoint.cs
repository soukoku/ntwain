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
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, null,
                DataGroups.Control, DataArgumentType.EntryPoint, Message.Get, entryPoint);
            }
            return ReturnCode.Failure;
        }
    }
}
