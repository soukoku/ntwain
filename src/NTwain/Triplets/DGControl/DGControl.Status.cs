using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.Status"/>.
    /// </summary>
	sealed class Status : TripletBase
	{
		internal Status(ITwainSessionInternal session) : base(session) { }
		/// <summary>
		/// Returns the current Condition Code for the Source Manager.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns></returns>
        public ReturnCode GetManager(out TWStatus status)
		{
			Session.VerifyState(2, 7, DataGroups.Control, DataArgumentType.Status, Message.Get);
			status = new TWStatus();
			return Dsm.DsmEntry(Session.AppId, null, Message.Get, status);
		}

		/// <summary>
		/// Returns the current Condition Code for the specified Source.
		/// </summary>
		/// <param name="status">The status.</param>
		/// <returns></returns>
        public ReturnCode GetSource(out TWStatus status)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.Status, Message.Get);
			status = new TWStatus();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, status);
		}
	}
}