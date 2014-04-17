using NTwain.Data;
using NTwain.Values;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ImageLayout"/>.
    /// </summary>
	public sealed class ImageLayout : OpBase
	{
		internal ImageLayout(ITwainStateInternal session) : base(session) { }

		public ReturnCode Get(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.Get);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Get, layout);
		}

		public ReturnCode GetDefault(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.GetDefault);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.GetDefault, layout);
		}

		public ReturnCode Reset(out TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Reset);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Reset, layout);
		}

		public ReturnCode Set(TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Set);
			return Dsm.DsmEntry(Session.AppId, Session.SourceId, Message.Set, layout);
		}
	}
}