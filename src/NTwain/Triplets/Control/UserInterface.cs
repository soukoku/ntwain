using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Triplets.Control
{
    sealed class UserInterface : BaseTriplet
    {
        internal UserInterface(TwainSession session) : base(session) { }

        public ReturnCode DisableDS(ref TW_USERINTERFACE ui, bool wasUIonly)
        {
            var rc = ReturnCode.Failure;

            if (Is32Bit)
            {
                if (IsWin)
                    rc = NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS, ref ui);
                else if (IsLinux)
                    rc = NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS, ref ui);
                else if (IsMac)
                    rc = NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS, ref ui);
            }
            else
            {
                if (IsWin)
                    rc = NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS, ref ui);
                else if (IsLinux)
                    rc = NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS, ref ui);
                else if (IsMac)
                    rc = NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                        DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS, ref ui);
            }

            if (rc == ReturnCode.Success)
            {
                Session.State = TwainState.S4;
                Session.OnSourceDisabled(new SourceDisabledEventArgs
                {
                    Source = Session.CurrentSource,
                    UIOnly = wasUIonly
                });
            }
            return rc;
        }

        public ReturnCode EnableDS(ref TW_USERINTERFACE ui)
        {
            var rc = ReturnCode.Failure;
            if (Session.State == TwainState.S4)
            {
                Session.State = TwainState.S5; //tentative

                if (Is32Bit)
                {
                    if (IsWin)
                        rc = NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDS, ref ui);
                    else if (IsLinux)
                        rc = NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDS, ref ui);
                    else if (IsMac)
                        rc = NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDS, ref ui);
                }
                else
                {
                    if (IsWin)
                        rc = NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDS, ref ui);
                    else if (IsLinux)
                        rc = NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDS, ref ui);
                    else if (IsMac)
                        rc = NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.DisableDS, ref ui);
                }

                if (!(rc == ReturnCode.Success &&
                    (ui.ShowUI == 0 && rc == ReturnCode.CheckStatus)))
                {
                    Session.State = TwainState.S4;
                }
            }
            return rc;
        }

        public ReturnCode EnableDSUIOnly(ref TW_USERINTERFACE ui)
        {
            var rc = ReturnCode.Failure;
            if (Session.State == TwainState.S4)
            {
                Session.State = TwainState.S5; //tentative

                if (Is32Bit)
                {
                    if (IsWin)
                        rc = NativeMethods.DsmWin32(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDSUIOnly, ref ui);
                    else if (IsLinux)
                        rc = NativeMethods.DsmLinux32(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDSUIOnly, ref ui);
                    else if (IsMac)
                        rc = NativeMethods.DsmMac32(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDSUIOnly, ref ui);
                }
                else
                {
                    if (IsWin)
                        rc = NativeMethods.DsmWin64(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDSUIOnly, ref ui);
                    else if (IsLinux)
                        rc = NativeMethods.DsmLinux64(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDSUIOnly, ref ui);
                    else if (IsMac)
                        rc = NativeMethods.DsmMac64(Session.Config.App32, Session.CurrentSource.Identity32,
                            DataGroups.Control, DataArgumentType.UserInterface, Message.EnableDSUIOnly, ref ui);
                }

                if (rc != ReturnCode.Success)
                {
                    Session.State = TwainState.S4;
                }
            }
            return rc;
        }
    }
}
