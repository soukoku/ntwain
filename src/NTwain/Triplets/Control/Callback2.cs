using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    sealed class Callback2 : BaseTriplet
    {
        internal Callback2(TwainSession session) : base(session) { }

        public ReturnCode RegisterCallback(ref TW_CALLBACK2 callback)
        {
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, Session.CurrentSource.Identity,
                    DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, ref callback);
            }
            return ReturnCode.Failure;
        }
    }
}