using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
	sealed class DeviceEvent : BaseTriplet
	{
		internal DeviceEvent(TwainSession session) : base(session) { }

        public ReturnCode Get(ref TW_DEVICEEVENT sourceDeviceEvent)
        {
            if (Use32BitData)
            {
                return NativeMethods.Dsm32(Session.Config.App32, Session.CurrentSource.Identity,
                    DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get, ref sourceDeviceEvent);
            }
            return ReturnCode.Failure;
        }
	}
}