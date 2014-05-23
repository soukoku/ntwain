using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.CustomDSData"/>.
    /// </summary>
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Get(out TWCustomDSData customData)
		{
			Session.VerifyState(4, 4, DataGroups.Control, DataArgumentType.CustomDSData, Message.Get);
			customData = new TWCustomDSData();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, customData);
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
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Set, customData);
		}
	}
}