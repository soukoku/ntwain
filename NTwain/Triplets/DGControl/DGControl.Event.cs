using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
	sealed class Event : OpBase
	{
		internal Event(ITwainSessionInternal session) : base(session) { }

		/// <summary>
		/// This operation supports the distribution of events from the application to Sources so that the
		/// Source can maintain its user interface and return messages to the application. Once the
		/// application has enabled the Source, it must immediately begin sending to the Source all events
		/// that enter the application’s main event loop. This allows the Source to update its user interface
		/// in real-time and to return messages to the application which cause state transitions. Even if the
		/// application overrides the Source’s user interface, it must forward all events once the Source has
		/// been enabled. The Source will tell the application whether or not each event belongs to the
		/// Source.
		/// </summary>
		/// <param name="theEvent">The event.</param>
		/// <returns></returns>
		public ReturnCode ProcessEvent(TWEvent theEvent)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.Event, Message.ProcessEvent);
			return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.ProcessEvent, theEvent);
		}
	}
}