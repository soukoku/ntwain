using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
	sealed class DeviceEvent : BaseTriplet
	{
		internal DeviceEvent(TwainSession session) : base(session) { }
    
        public ReturnCode Get(ref TW_DEVICEEVENT sourceDeviceEvent)
		{
			return NativeMethods.DsmWin32(Session.Config.AppWin32, Session.CurrentSource.Identity, 
                DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref sourceDeviceEvent);
		}
	}
}