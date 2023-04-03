#if WINDOWS || NETFRAMEWORK
using NTwain.Data;
using NTwain.Native;
using NTwain.Triplets;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace NTwain
{

  // contains parts for winform/wpf message loop integration

  partial class TwainSession : IMessageFilter
  {
    // winform use with Application.AddMessageFilter(<TwainSession instance>)
    bool System.Windows.Forms.IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
    {
      return CheckIfTwainMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
    }

    // wpf use
    IntPtr HwndSourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      handled = CheckIfTwainMessage(hwnd, msg, wParam, lParam);
      return IntPtr.Zero;
    }

    private bool CheckIfTwainMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
      // this handles the message from a typical WndProc message loop and check if it's from the TWAIN source.
      bool handled = false;
      if (_state >= Data.STATE.S5)
      {
        TW_EVENT evt = default;
        try
        {
          var winMsg = new WIN_MESSAGE
          {
            hwnd = hWnd,
            message = (uint)msg,
            wParam = wParam,
            lParam = lParam
          };
          // no need to do another lock call when using marshal alloc
          evt.pEvent = Marshal.AllocHGlobal(Marshal.SizeOf(winMsg));
          Marshal.StructureToPtr(winMsg, evt.pEvent, false);

          var rc = DGControl.Event.ProcessEvent(ref _appIdentity, ref _currentDS, ref evt);
          handled = rc == STS.DSEVENT;
          if (evt.TWMessage != 0 && (handled || rc == STS.NOTDSEVENT))
          {
            Debug.WriteLine("Thread {0}: CheckIfTwainMessage at state {1} with MSG={2}.", Thread.CurrentThread.ManagedThreadId, State, (MSG)evt.TWMessage);
            HandleSourceMsg((MSG)evt.TWMessage);
          }
        }
        finally
        {
          // do we need to free it
          if (evt.pEvent != IntPtr.Zero) Marshal.FreeHGlobal(evt.pEvent);
        }
      }
      return handled;
    }
  }
}
#endif