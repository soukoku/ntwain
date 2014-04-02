using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	sealed class EntryPoint : OpBase
	{
		internal EntryPoint(TwainSession session) : base(session) { }
		/// <summary>
		/// Gets the function entry points for twain 2.0 or higher.
		/// </summary>
		/// <param name="entryPoint">The entry point.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWEntryPoint entryPoint)
		{
			Session.VerifyState(3, 3, DataGroups.Control, DataArgumentType.EntryPoint, Message.Get);
			entryPoint = new TWEntryPoint();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, entryPoint);
		}
	}
}