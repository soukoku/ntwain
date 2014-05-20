using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ImageLayout"/>.
    /// </summary>
	public sealed class ImageLayout : OpBase
	{
		internal ImageLayout(ITwainSessionInternal session) : base(session) { }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Get(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.Get);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.Get, layout);
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode GetDefault(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.GetDefault);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.GetDefault, layout);
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Reset(out TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Reset);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.Reset, layout);
		}

		public ReturnCode Set(TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Set);
			return Dsm.DsmEntry(Session.AppId, Session.Source.Identity, Message.Set, layout);
		}
	}
}