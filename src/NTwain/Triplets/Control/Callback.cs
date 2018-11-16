using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
	sealed class Callback : BaseTriplet
	{
		internal Callback(TwainSession session) : base(session) { }

		public ReturnCode RegisterCallback(ref TW_CALLBACK callback)
        {
            return NativeMethods.DsmWin32(Session.Config.AppWin32, Session.CurrentSource.Identity,
                DataGroups.Control, DataArgumentType.Callback, Message.RegisterCallback, ref callback);
        }
	}
}