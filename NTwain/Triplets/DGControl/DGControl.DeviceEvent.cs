using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.DeviceEvent"/>.
    /// </summary>
	sealed class DeviceEvent : TripletBase
	{
		internal DeviceEvent(ITwainSessionInternal session) : base(session) { }
    
        public ReturnCode Get(out TWDeviceEvent sourceDeviceEvent)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get);
			sourceDeviceEvent = new TWDeviceEvent();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, sourceDeviceEvent);
		}
	}
}