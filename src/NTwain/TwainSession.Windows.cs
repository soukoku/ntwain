#if WINDOWS || NETFRAMEWORK
using NTwain.Data;
using NTwain.Native;
using NTwain.Triplets;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Interop;
using MSG = NTwain.Data.MSG;

namespace NTwain
{

  // contains parts for winform/wpf message loop integration

  partial class TwainSession : IMessageFilter
  {
    private HwndSource? _wpfhook;

    /// <summary>
    /// Registers this session for use in a Winform UI thread.
    /// </summary>
    public void AddWinformFilter()
    {
      Application.AddMessageFilter(this);
    }
    /// <summary>
    /// Unregisters this session if previously registered with <see cref="AddWinformFilter"/>.
    /// </summary>
    public void RemoveWinformFilter()
    {
      Application.RemoveMessageFilter(this);
    }

    /// <summary>
    /// Registers this session for use in a WPF UI thread.
    /// </summary>
    public void AddWpfHook()
    {
      if (_wpfhook == null)
      {
        _wpfhook = HwndSource.FromHwnd(_hwnd);
        _wpfhook.AddHook(WpfHook);
      }
    }
    /// <summary>
    /// Unregisters this session if previously registered with <see cref="AddWpfHook"/>.
    /// </summary>
    public void RemoveWpfHook()
    {
      if (_wpfhook != null)
      {
        _wpfhook.RemoveHook(WpfHook);
        _wpfhook = null;
      }
    }

    bool System.Windows.Forms.IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
    {
      return CheckIfTwainMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
    }

    // wpf use with HwndSource.FromHwnd(Handle).AddHook(
    IntPtr WpfHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      handled = CheckIfTwainMessage(hwnd, msg, wParam, lParam);
      return IntPtr.Zero;
    }

    private bool CheckIfTwainMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
      // this handles the message from a typical WndProc message loop and check if it's from the TWAIN source.
      bool handled = false;
      if (_state >= STATE.S5)
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