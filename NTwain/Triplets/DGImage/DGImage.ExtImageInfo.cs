using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ExtImageInfo"/>.
    /// </summary>
	public sealed class ExtImageInfo : OpBase
	{
		internal ExtImageInfo(ITwainSessionInternal session) : base(session) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Get(out TWExtImageInfo info)
		{
			Session.VerifyState(7, 7, DataGroups.Image, DataArgumentType.ExtImageInfo, Message.Get);
            info = new TWExtImageInfo();
            return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Get, info);
		}
	}
}