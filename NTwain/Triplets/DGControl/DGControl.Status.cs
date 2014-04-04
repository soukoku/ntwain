using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	public sealed class Status : OpBase
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
			return PInvoke.DsmEntry(Session.AppId, null, Message.Get, status);
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
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, status);
		}
	}
}