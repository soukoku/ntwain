using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace NTwain
{
    partial class TwainSession
    {
        internal TW_USERINTERFACE _lastEnableUI;

        private void HandleSourceMsg(Message msg)
        {
            switch (msg)
            {
                case Message.DeviceEvent:
                    TW_DEVICEEVENT de = default;
                    var rc = DGControl.DeviceEvent.Get(ref de);
                    if (rc == ReturnCode.Success)
                    {
                        OnDeviceEventReceived(new DeviceEventArgs { Data = de });
                    }
                    break;
                case Message.XferReady:
                    if (State < TwainState.S6)
                    {
                        State = TwainState.S6;
                    }
                    DoTransferRoutine();
                    break;
                case Message.CloseDSReq:
                    DGControl.UserInterface.DisableDS(ref _lastEnableUI, false);
                    break;
                case Message.CloseDSOK:
                    DGControl.UserInterface.DisableDS(ref _lastEnableUI, true);
                    break;
            }
        }

        private void DoTransferRoutine()
        {
            throw new NotImplementedException();
        }
    }
}
