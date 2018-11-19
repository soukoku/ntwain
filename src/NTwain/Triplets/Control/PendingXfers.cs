using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
	sealed class PendingXfers : BaseTriplet
	{
		internal PendingXfers(TwainSession session) : base(session) { }

        ReturnCode DoIt(Message msg, ref TW_PENDINGXFERS pendingXfers)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.PendingXfers, msg, ref pendingXfers);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.PendingXfers, msg, ref pendingXfers);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.PendingXfers, msg, ref pendingXfers);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.PendingXfers, msg, ref pendingXfers);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.PendingXfers, msg, ref pendingXfers);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.PendingXfers, msg, ref pendingXfers);

            return ReturnCode.Failure;
        }

        /// <summary>
        /// This triplet is used to cancel or terminate a transfer. Issued in state 6, this triplet cancels the next
        /// pending transfer, discards the transfer data, and decrements the pending transfers count. In
        /// state 7, this triplet terminates the current transfer. If any data has not been transferred (this is
        /// only possible during a memory transfer) that data is discarded.
        /// </summary>
        /// <param name="pendingXfers">The pending xfers.</param>
        /// <returns></returns>
        internal ReturnCode EndXfer(ref TW_PENDINGXFERS pendingXfers)
        {
            return DoIt(Message.EndXfer, ref pendingXfers);
        }

		/// <summary>
		/// Returns the number of transfers the Source is ready to supply to the application, upon demand.
		/// If DAT_XFERGROUP is set to DG_Image, this is the number of images. If DAT_XFERGROUP is set
		/// to DG_AUDIO, this is the number of audio snippets for the current image. If there is no current
		/// image, this call must return Failure / SeqError.
		/// </summary>
		/// <param name="pendingXfers">The pending xfers.</param>
		/// <returns></returns>
		public ReturnCode Get(ref TW_PENDINGXFERS pendingXfers)
        {
            return DoIt(Message.Get, ref pendingXfers);
        }

		/// <summary>
		/// Sets the number of pending transfers in the Source to zero.
		/// </summary>
		/// <param name="pendingXfers">The pending xfers.</param>
		/// <returns></returns>
		internal ReturnCode Reset(ref TW_PENDINGXFERS pendingXfers)
        {
            return DoIt(Message.Reset, ref pendingXfers);
        }

		/// <summary>
		/// If CapAutoScan is TRUE, this command will stop the operation of the scanner’s automatic
		/// feeder. No other action is taken.
		/// </summary>
		/// <param name="pendingXfers">The pending xfers.</param>
		/// <returns></returns>
		public ReturnCode StopFeeder(ref TW_PENDINGXFERS pendingXfers)
        {
            return DoIt(Message.StopFeeder, ref pendingXfers);
        }
	}
}