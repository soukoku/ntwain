using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.CieColor"/>.
    /// </summary>
	public sealed class CieColor : OpBase
	{
		internal CieColor(ITwainStateInternal session) : base(session) { }

		/// <summary>
		/// This operation causes the Source to report the currently active parameters to be used in
		/// converting acquired color data into CIE XYZ.
		/// </summary>
		/// <param name="cieColor">Color data.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWCieColor cieColor)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.CieColor, Message.Get);
			cieColor = new TWCieColor();
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Get, cieColor);
		}
	}
}