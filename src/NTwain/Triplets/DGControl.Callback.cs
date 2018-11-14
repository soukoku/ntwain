using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
	sealed class Callback : BaseTriplet
	{
		internal Callback(TwainSession session) : base(session) { }

		public ReturnCode RegisterCallback(TW_CALLBACK callback)
        {
            return NativeMethods.DsmWin32(Session.Config.AppWin32, Session.CurrentSource.Identity,
                DataGroups.Control, DataArgumentType.Callback2, Message.RegisterCallback, callback);
        }
	}
}