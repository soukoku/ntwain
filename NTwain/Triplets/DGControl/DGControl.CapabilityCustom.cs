using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets
{
    /// <summary>
    /// This is to support custom DAT value for custom capability defined by some manufacturers.
    /// </summary>
    public sealed class CapabilityCustom : OpBase
    {
        internal CapabilityCustom(ITwainSessionInternal session) : base(session) { }

        /// <summary>
        /// Returns the Source’s Current, Default and Available Values for a specified capability.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode Get(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 7, DataGroups.Control, (DataArgumentType)customDAT, Message.Get);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.Get, capability);
        }

        /// <summary>
        /// Returns the Source’s Current Value for the specified capability.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode GetCurrent(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 7, DataGroups.Control, (DataArgumentType)customDAT, Message.GetCurrent);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.GetCurrent, capability);
        }

        /// <summary>
        /// Returns the Source’s Default Value. This is the Source’s preferred default value.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode GetDefault(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 7, DataGroups.Control, (DataArgumentType)customDAT, Message.GetDefault);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.GetDefault, capability);
        }

        /// <summary>
        /// Returns help text suitable for use in a GUI; for instance: "Specify the amount of detail in an
        /// image. Higher values result in more detail." for ICapXRESOLUTION.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode GetHelp(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 4, DataGroups.Control, (DataArgumentType)customDAT, Message.GetHelp);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.GetHelp, capability);
        }

        /// <summary>
        /// Returns a label suitable for use in a GUI, for instance "Resolution:"
        /// for ICapXRESOLUTION.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode GetLabel(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 4, DataGroups.Control, (DataArgumentType)customDAT, Message.GetLabel);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.GetLabel, capability);
        }

        /// <summary>
        /// Return all of the labels for a capability of type <see cref="TWArray"/> or <see cref="TWEnumeration"/>, for example
        /// "US Letter" for ICapSupportedSizes’ TWSS_USLETTER.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode GetLabelEnum(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 4, DataGroups.Control, (DataArgumentType)customDAT, Message.GetLabelEnum);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.GetLabelEnum, capability);
        }

        /// <summary>
        /// Returns the Source’s support status of this capability.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode QuerySupport(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 7, DataGroups.Control, (DataArgumentType)customDAT, Message.QuerySupport);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.QuerySupport, capability);
        }

        /// <summary>
        /// Change the Current Value of the specified capability back to its power-on value and return the
        /// new Current Value.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode Reset(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 4, DataGroups.Control, (DataArgumentType)customDAT, Message.Reset);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.Reset, capability);
        }

        /// <summary>
        /// This command resets all of the current values and constraints to their defaults for all of the
        /// negotiable capabilities supported by the driver.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode ResetAll(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 4, DataGroups.Control, (DataArgumentType)customDAT, Message.ResetAll);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.ResetAll, capability);
        }

        /// <summary>
        /// Changes the Current Value(s) and Available Values of the specified capability to those specified
        /// by the application. As of TWAIN 2.2 this only modifies the Current Value of the specified capability, constraints cannot be
        /// changed with this.
        /// Current Values are set when the container is a <see cref="TWOneValue"/> or <see cref="TWArray"/>. Available and
        /// Current Values are set when the container is a <see cref="TWEnumeration"/> or <see cref="TWRange"/>.
        /// </summary>
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode Set(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 6, DataGroups.Control, (DataArgumentType)customDAT, Message.Set);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.Set, capability);
        }

        /// <summary>
        /// Changes the Current Value(s) and Available Value(s) of the specified capability to those specified
        /// by the application.
        /// </summary>
        /// Current Values are set when the container is a <see cref="TWOneValue"/> or <see cref="TWArray"/>. Available and
        /// Current Values are set when the container is a <see cref="TWEnumeration"/> or <see cref="TWRange"/>.
        /// <param name="customDAT">The custom DAT_* value from manufacturer.</param>
        /// <param name="capability">The capability.</param>
        /// <returns></returns>
        public ReturnCode SetConstraint(ushort customDAT, TWCapability capability)
        {
            Session.VerifyState(4, 7, DataGroups.Control, (DataArgumentType)customDAT, Message.SetConstraint);
            return Dsm.DsmEntry(Session.AppId, Session.CurrentSource.Identity, (DataArgumentType)customDAT, Message.SetConstraint, capability);
        }
    }
}