using NTwain.Data;
using NTwain.Internals;
using System.Runtime.InteropServices;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.Metrics"/>.
    /// </summary>
    public sealed class Metrics : BaseTriplet
    {
        internal Metrics(TwainSession session) : base(session) { }

        /// <summary>
        /// Reads information relating to the last time DG_CONTROL / DAT_USERINTERFACE / MSG_ENABLEDS was sent.
        /// An application calls this to get final counts after scanning. This is
        /// necessary because some metrics cannot be detected during scanning, such as blank images
        /// discarded at the very end of a session.
        /// </summary>
        /// <param name="metrics"></param>
        /// <returns></returns>
        public ReturnCode Get(ref TW_METRICS metrics)
        {
            metrics.SizeOf = (uint)Marshal.SizeOf(typeof(TW_METRICS));

            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.Metrics, Message.Get, ref metrics);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.Metrics, Message.Get, ref metrics);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.Metrics, Message.Get, ref metrics);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.Metrics, Message.Get, ref metrics);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.Metrics, Message.Get, ref metrics);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DataArgumentType.Metrics, Message.Get, ref metrics);

            return ReturnCode.Failure;
        }
    }
}