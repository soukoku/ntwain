using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.RgbResponse"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rgb")]
    public sealed class RgbResponse : TripletBase
	{
		internal RgbResponse(ITwainSessionInternal session) : base(session) { }

		/// <summary>
		/// Causes the Source to use its "identity" response curves for future RGB transfers. The identity
		/// curve causes no change in the values of the captured data when it is applied. (Note that
		/// resetting the curves for RGB data does not reset any curves for other pixel types).
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Reset(out TWRgbResponse response)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.RgbResponse, Message.Reset);
			response = new TWRgbResponse();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Reset, response);
		}

		/// <summary>
		/// Causes the Source to transform any RGB data according to the response curves specified by the
		/// application.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <returns></returns>
		public ReturnCode Set(TWRgbResponse response)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.RgbResponse, Message.Set);
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Set, response);
		}
	}
}