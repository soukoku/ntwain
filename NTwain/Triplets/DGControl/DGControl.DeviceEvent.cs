using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.DeviceEvent"/>.
    /// </summary>
	public sealed class DeviceEvent : OpBase
	{
		internal DeviceEvent(ITwainStateInternal session) : base(session) { }
		public ReturnCode Get(out TWDeviceEvent sourceDeviceEvent)
		{
			Session.VerifyState(4, 7, DataGroups.Control, DataArgumentType.DeviceEvent, Message.Get);
			sourceDeviceEvent = new TWDeviceEvent();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, sourceDeviceEvent);
		}
	}
}