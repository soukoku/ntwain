using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
	sealed class Callback : BaseTriplet
	{
		internal Callback(TwainSession session) : base(session) { }

		public ReturnCode RegisterCallback(ref TW_CALLBACK callback)
        {
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, Session.CurrentSource.Identity,
                    DataGroups.Control, DataArgumentType.Callback, Message.RegisterCallback, ref callback);
            }
            return ReturnCode.Failure;
        }
	}
}