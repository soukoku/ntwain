using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    /// <summary>
    /// Represents <see cref="DataArgumentType.Capability"/>.
    /// </summary>
	public sealed class Capability : BaseTriplet
	{
		internal Capability(TwainSession session) : base(session) { }

        /// <summary>
        /// Capability triplet call with custom DAT and message.
        /// </summary>
        /// <param name="DAT"></param>
        /// <param name="message"></param>
        /// <param name="capability"></param>
        /// <returns></returns>
        public ReturnCode Custom(DataArgumentType DAT, Message message, ref TW_CAPABILITY capability)
        {
            if (Is32Bit)
            {
                if (IsWin)
                    return NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DAT, message, ref capability);
                if (IsLinux)
                    return NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DAT, message, ref capability);
                if (IsMac)
                    return NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DAT, message, ref capability);
            }

            if (IsWin)
                return NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DAT, message, ref capability);
            if (IsLinux)
                return NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DAT, message, ref capability);
            if (IsMac)
                return NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                    DataGroups.Control, DAT, message, ref capability);

            return ReturnCode.Failure;
        }


        /// <summary>
        /// Returns the Source’s Current, Default and Available Values for a specified capability.
        /// </summary>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode Get(ref TW_CAPABILITY capability)
		{
            return Custom(DataArgumentType.Capability, Message.Get, ref capability);
		}

		/// <summary>
		/// Returns the Source’s Current Value for the specified capability.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode GetCurrent(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.GetCurrent, ref capability);
        }

		/// <summary>
		/// Returns the Source’s Default Value. This is the Source’s preferred default value.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode GetDefault(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.GetDefault, ref capability);
        }

		/// <summary>
		/// Returns help text suitable for use in a GUI; for instance: "Specify the amount of detail in an
		/// image. Higher values result in more detail." for ICapXRESOLUTION.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode GetHelp(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.GetHelp, ref capability);
        }

		/// <summary>
		/// Returns a label suitable for use in a GUI, for instance "Resolution:"
		/// for ICapXRESOLUTION.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode GetLabel(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.GetLabel, ref capability);
        }

		/// <summary>
		/// Return all of the labels for a capability of type TW_ARRAY or TW_ENUMERATION, for example
		/// "US Letter" for ICapSupportedSizes’ TWSS_USLETTER.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode GetLabelEnum(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.GetLabelEnum, ref capability);
        }

		/// <summary>
		/// Returns the Source’s support status of this capability.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode QuerySupport(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.QuerySupport, ref capability);
        }

		/// <summary>
		/// Change the Current Value of the specified capability back to its power-on value and return the
		/// new Current Value.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode Reset(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.Reset, ref capability);
        }

		/// <summary>
		/// This command resets all of the current values and constraints to their defaults for all of the
		/// negotiable capabilities supported by the driver.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode ResetAll(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.ResetAll, ref capability);
        }

		/// <summary>
		/// Changes the Current Value(s) and Available Values of the specified capability to those specified
		/// by the application. As of TWAIN 2.2 this only modifies the Current Value of the specified capability, constraints cannot be
        /// changed with this.
		/// Current Values are set when the container is a TW_ONEVALUE or TW_ARRAY. Available and
		/// Current Values are set when the container is a TW_ENUMERATION or TW_RANGE.
		/// </summary>
		/// <param name="capability">The capability.</param>
		/// <returns></returns>
		public ReturnCode Set(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.Set, ref capability);
        }

        /// <summary>
        /// Changes the Current Value(s) and Available Value(s) of the specified capability to those specified
        /// by the application.
        /// </summary>
        /// Current Values are set when the container is a TW_ONEVALUE or TW_ARRAY. Available and
        /// Current Values are set when the container is a TW_ENUMERATION or TW_RANGE.
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode SetConstraint(ref TW_CAPABILITY capability)
        {
            return Custom(DataArgumentType.Capability, Message.SetConstraint, ref capability);
        }
	}
}