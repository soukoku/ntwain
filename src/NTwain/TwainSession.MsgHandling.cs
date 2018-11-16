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
        private void HandleSourceMsg(Message msg)
        {
            switch (msg)
            {
                case Message.DeviceEvent:
                    TW_DEVICEEVENT de = default;
                    var rc = DGControl.DeviceEvent.Get(ref de);
                    if (rc == ReturnCode.Success)
                    {
                        OnDeviceEvent(new DeviceEventArgs { Data = de });
                    }
                    break;
                case Message.XferReady:
                    break;
                case Message.CloseDSReq:
                    break;
                case Message.CloseDSOK:
                    break;
            }
        }
    }
}
