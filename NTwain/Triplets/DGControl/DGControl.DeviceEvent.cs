using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.DeviceEvent"/>.
    /// </summary>
	public sealed class DeviceEvent : OpBase
	{
		internal DeviceEvent(ITwainSessionInternal session) : base(session) { }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Get(out TWDeviceEvent sourceDeviceEvent)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get);
			sourceDeviceEvent = new TWDeviceEvent();
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Get, sourceDeviceEvent);
		}
	}
}