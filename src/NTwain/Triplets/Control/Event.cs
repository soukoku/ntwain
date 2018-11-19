using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    sealed class Event : BaseTriplet
    {
        internal Event(TwainSession session) : base(session) { }

        public ReturnCode ProcessEvent(ref TW_EVENT evt)
        {
            if (Is32Bit)
            {
                return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.Event, Message.ProcessEvent, ref evt);
            }

            return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                DataGroups.Control, DataArgumentType.Event, Message.ProcessEvent, ref evt);
        }
    }
}