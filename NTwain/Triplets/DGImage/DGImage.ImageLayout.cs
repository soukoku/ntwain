using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.ImageLayout"/>.
    /// </summary>
	public sealed class ImageLayout : TripletBase
	{
		internal ImageLayout(ITwainSessionInternal session) : base(session) { }

        /// <summary>
        /// Gets both the size and placement of the image on the scanner. The
        /// coordinates on the scanner and the extents of the image are expressed in the unit of measure
        /// currently negotiated for ICAP_UNITS (default is inches).
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Get(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.Get);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Get, layout);
		}

        /// <summary>
        /// returns the default information on the layout of an image. This is the size and
        /// position of the image that will be acquired from the Source if the acquisition is started with the
        /// Source (and the device it is controlling) in its power-on state (for instance, most flatbed scanners
        /// will capture the entire bed).
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode GetDefault(out TWImageLayout layout)
		{
			Session.VerifyState(4, 6, DataGroups.Image, DataArgumentType.ImageLayout, Message.GetDefault);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.GetDefault, layout);
		}

        /// <summary>
        /// This operation sets the image layout information for the next transfer to its default settings.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#")]
        public ReturnCode Reset(out TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Reset);
			layout = new TWImageLayout();
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Reset, layout);
		}

        /// <summary>
        /// This operation sets the layout for the next image transfer. This allows the application to specify
        /// the physical area to be acquired during the next image transfer (for instance, a frame-based
        /// application would pass to the Source the size of the frame the user selected within the
        /// application—the helpful Source would present a selection region already sized to match the
        /// layout frame size).
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <returns></returns>
		public ReturnCode Set(TWImageLayout layout)
		{
			Session.VerifyState(4, 4, DataGroups.Image, DataArgumentType.ImageLayout, Message.Set);
			return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, Message.Set, layout);
		}
	}
}