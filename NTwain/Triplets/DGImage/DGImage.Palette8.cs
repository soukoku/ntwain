using NTwain.Data;
using NTwain.Values;
using System;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.Palette8"/>.
    /// </summary>
	public sealed class Palette8 : OpBase
	{
		internal Palette8(ITwainSessionInternal session) : base(session) { }

		/// <summary>
		/// This operation causes the Source to report its current palette information.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode Get(out TWPalette8 palette)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.Palette8, Message.Get);
			palette = new TWPalette8();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Get, palette);
		}

		/// <summary>
		/// This operation causes the Source to report its power-on default palette.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode GetDefault(out TWPalette8 palette)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.Palette8, Message.GetDefault);
			palette = new TWPalette8();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.GetDefault, palette);
		}

		/// <summary>
		/// This operation causes the Source to dispose of any current palette it has and to use its default
		/// palette for the next palette transfer.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode Reset(out TWPalette8 palette)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.Palette8, Message.Reset);
			palette = new TWPalette8();
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, palette);
		}

		/// <summary>
		/// This operation requests that the Source adopt the specified palette for use with all subsequent
		/// palette transfers. The application should be careful to supply a palette that matches the bit
		/// depth of the Source. The Source is not required to adopt this palette. The application should be
		/// careful to check the return value from this operation.
		/// </summary>
		/// <param name="palette">The palette.</param>
		/// <returns></returns>
		public ReturnCode Set(TWPalette8 palette)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.Palette8, Message.Set);
			return PInvoke.DsmEntry(Session.AppId, Session.SourceId, Message.Set, palette);
		}
	}
}