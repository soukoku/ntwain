using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TWAINWorkingGroup;

namespace NTwain
{
  // this file contains callback methods

  partial class TwainSession
  {
    // these are kept around while a callback ptr is registered so they
    // don't get gc'd
    readonly NativeMethods.WindowsDsmEntryCallbackDelegate _legacyCallbackDelegate;
    readonly NativeMethods.MacosxDsmEntryCallbackDelegate _osxCallbackDelegate;

    /// <summary>
    /// Try to registers callbacks for after opening the source.
    /// </summary>
    internal void RegisterCallback()
    {
      IntPtr cbPtr = IntPtr.Zero;

      if (TwainPlatform.IsMacOSX)
      {
        cbPtr = Marshal.GetFunctionPointerForDelegate(_osxCallbackDelegate);
      }
      else
      {
        cbPtr = Marshal.GetFunctionPointerForDelegate(_legacyCallbackDelegate);
      }

      var rc = STS.FAILURE;

      // per the spec (pg 8-10), apps for 2.2 or higher uses callback2 so try this first
      if (_appIdentity.ProtocolMajor > 2 || (_appIdentity.ProtocolMajor >= 2 && _appIdentity.ProtocolMinor >= 2))
      {
        var cb2 = new TW_CALLBACK2 { CallBackProc = cbPtr };
        rc = DGControl.Callback2.RegisterCallback(ref _appIdentity, ref _currentDS, ref cb2);
      }
      if (rc != STS.SUCCESS)
      {
        // always try old callback
        var cb = new TW_CALLBACK { CallBackProc = cbPtr };
        DGControl.Callback.RegisterCallback(ref _appIdentity, ref _currentDS, ref cb);
      }
    }

    private ushort LegacyCallbackHandler
    (
        ref TW_IDENTITY_LEGACY origin,
        ref TW_IDENTITY_LEGACY dest,
        DG dg,
        DAT dat,
        MSG msg,
        IntPtr twnull
    )
    {
      Debug.WriteLine($"Legacy callback got {msg}");
      HandleSourceMsg(msg);
      return (ushort)STS.SUCCESS;
    }

    private ushort OSXCallbackHandler
    (
        ref TW_IDENTITY_MACOSX origin,
        ref TW_IDENTITY_MACOSX dest,
        DG dg,
        DAT dat,
        MSG msg,
        IntPtr twnull
    )
    {
      Debug.WriteLine($"OSX callback got {msg}");
      HandleSourceMsg(msg);
      return (ushort)STS.SUCCESS;
    }

    private void HandleSourceMsg(MSG msg)
    {
      switch (msg)
      {
        case MSG.XFERREADY:
          break;
        case MSG.DEVICEEVENT:
          break;
        case MSG.CLOSEDSOK:
          DisableSource();
          break;
        case MSG.CLOSEDSREQ:
          DisableSource();
          break;
      }
    }

    public void HandleWin32Message(uint msg)
    {

    }
  }
}
