using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    sealed class Status : BaseTriplet
    {
        internal Status(TwainSession session) : base(session) { }

        public ReturnCode GetManagerStatus(ref TW_STATUS status)
        {
            return NativeMethods.DsmWin32(Session.Config.AppWin32, null,
                DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
        }

        public ReturnCode GetSourceStatus(ref TW_STATUS status)
        {
            return NativeMethods.DsmWin32(Session.Config.AppWin32, Session.CurrentSource.Identity,
                DataGroups.Control, DataArgumentType.Status, Message.Get, ref status);
        }
    }
}