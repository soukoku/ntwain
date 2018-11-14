using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NTwain
{
    public partial class TwainSession
    {
        internal TwainConfig Config;
        private IntPtr _hWnd;

        public TwainSession(TwainConfig config)
        {
            Config = config;
        }

        public ReturnCode Open(ref IntPtr hWnd)
        {
            _hWnd = hWnd;
            return DGControl.Parent.OpenDSM(ref hWnd);
        }

        public ReturnCode StepDown(TwainState targetState)
        {
            var rc = ReturnCode.Failure;
            while (State > targetState)
            {
                switch (State)
                {
                    case TwainState.DsmOpened:
                        rc = DGControl.Parent.CloseDSM(ref _hWnd);
                        if (rc != ReturnCode.Success) return rc;
                        break;
                }
            }
            return rc;
        }
    }
}
