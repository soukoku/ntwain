using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.CieColor"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Cie")]
    public sealed class CieColor : OpBase
	{
		internal CieColor(ITwainSessionInternal session) : base(session) { }

		/// <summary>
		/// This operation causes the Source to report the currently active parameters to be used in
		/// converting acquired color data into CIE XYZ.
		/// </summary>
		/// <param name="cieColor">Color data.</param>
		/// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "cie"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Get(out TWCieColor cieColor)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.CieColor, Message.Get);
			cieColor = new TWCieColor();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, cieColor);
		}
	}
}