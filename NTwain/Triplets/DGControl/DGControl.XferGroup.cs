using System;
using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
	public sealed class XferGroup : OpBase
	{
		internal XferGroup(ITwainSessionInternal session) : base(session) { }
		/// <summary>
		/// Returns the Data Group (the type of data) for the upcoming transfer. The Source is required to
		/// only supply one of the DGs specified in the SupportedGroups field of origin.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public ReturnCode Get(out uint value)
		{
			Session.VerifyState(4, 6, DataGroups.Control, DataArgumentType.XferGroup, Message.Get);
			throw new NotImplementedException();

			// TODO: I have no idea if this even works, just something that looks logical.
			// Does memory from pointer need to be released once we got the value?

			//IntPtr ptr = IntPtr.Zero;
			//var rc = Custom.DsmEntry(Session.AppId, Session.SourceId, DataGroup.Control, DataArgumentType.XferGroup, Message.Get, ref ptr);
			//unsafe
			//{
			//    uint* realPtr = (uint*)ptr.ToPointer();
			//    value = (*realPtr);
			//}
			//return rc;
		}

		public ReturnCode Set(uint value)
		{
			Session.VerifyState(6, 6, DataGroups.Control, DataArgumentType.XferGroup, Message.Set);
			throw new NotImplementedException();
		}
	}
}