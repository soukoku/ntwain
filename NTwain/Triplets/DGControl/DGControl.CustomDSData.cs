using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
	public sealed class CustomDSData : OpBase
	{
		internal CustomDSData(ITwainSessionInternal session) : base(session) { }
		/// <summary>
		/// This operation is used by the application to query the data source for its current settings, e.g.
        /// DPI, paper size, color format. The actual format of the data is data source dependent and not
		/// defined by TWAIN.
		/// </summary>
		/// <param name="customData">The custom data.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWCustomDSData customData)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.CustomDSData, Message.Get);
			customData = new TWCustomDSData();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, customData);
		}

		/// <summary>
		/// This operation is used by the application to set the current settings for a data source to a
		/// previous state as defined by the data contained in the customData data structure. The actual
		/// format of the data is data source dependent and not defined by TWAIN.
		/// </summary>
		/// <param name="customData">The custom data.</param>
		/// <returns></returns>
		public ReturnCode Set(TWCustomDSData customData)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.CustomDSData, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, customData);
		}
	}
}