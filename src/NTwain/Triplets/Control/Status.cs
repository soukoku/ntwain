using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    sealed class Status : BaseTriplet
    {
        internal Status(TwainSession session) : base(session) { }

        public ReturnCode GetManagerStatus(ref TW_STATUS status)
        {
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, null,
                    DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
            }
            return ReturnCode.Failure;
        }

        public ReturnCode GetSourceStatus(ref TW_STATUS status)
        {
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, Session.CurrentSource.Identity,
                    DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
            }
            return ReturnCode.Failure;
        }
    }
}