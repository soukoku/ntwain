using Microsoft.Extensions.Logging;
using NTwain.Data;
using NTwain.Platform;
using NTwain.Triplets;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MSG = NTwain.Data.MSG;
using WinMSG = Windows.Win32.UI.WindowsAndMessaging.MSG;

namespace NTwain;


// contains callback handling, from either the new registered callback function
// or the internal win32 message loop.

partial class TwainAppSession : IWin32MessageFilter
{
    private bool _procEventHasData = false;

    /// <summary>
    /// Used to check if a window message is for the TWAIN source.
    /// </summary>
    /// <param name="winMsg"></param>
    /// <returns></returns>
    bool IWin32MessageFilter.PreFilterMessage(ref WinMSG winMsg)
    {
        // this handles the message from a typical WndProc message loop and checks if it's for the TWAIN source.
        bool handled = false;
        //if (_state >= STATE.S5)
        //{

        //    if (!_closeDsRequested)
        //    {
        if (_currentDs != null && _procEvent.pEvent != IntPtr.Zero)
        {
            Marshal.StructureToPtr(winMsg, _procEvent.pEvent, _procEventHasData);
            _procEventHasData = true;

            var rc = DGControl.Event.ProcessEvent(_appIdentity, _currentDs, ref _procEvent);
            handled = rc == TWRC.DSEVENT;
            if (_procEvent.TWMessage != MSG.NULL && (handled || rc == TWRC.NOTDSEVENT))
            {
                if (Logger.IsEnabled(LogLevel.Trace))
                    Logger.LogTrace("[thread {ThreadId}] CheckIfTwainMessage at state {State} with MSG={Msg}.",
                    Environment.CurrentManagedThreadId, State, _procEvent.TWMessage);
                HandleSourceMsg(_procEvent.TWMessage);
            }

        }
        return handled;
    }






    delegate ushort LegacyIDCallbackDelegate(
      ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, IntPtr twnull
    );
    delegate ushort BotchedLinuxCallbackDelegate
    (
        ref TW_IDENTITY origin, ref TW_IDENTITY dest,
        DG dg, DAT dat, MSG msg, IntPtr twnull
    );
    delegate ushort OSXCallbackDelegate
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, IntPtr twnull
    );

    // these are kept around while a callback ptr is registered so they
    // don't get gc'd
    readonly LegacyIDCallbackDelegate _legacyCallbackDelegate;
    readonly OSXCallbackDelegate _osxCallbackDelegate;

    /// <summary>
    /// Try to registers callbacks for after opening the source.
    /// </summary>
    internal void RegisterCallback()
    {
        IntPtr cbPtr = IntPtr.Zero;

        if (TWPlatform.IsMacOSX)
        {
            cbPtr = Marshal.GetFunctionPointerForDelegate(_osxCallbackDelegate);
        }
        else
        {
            cbPtr = Marshal.GetFunctionPointerForDelegate(_legacyCallbackDelegate);
        }

        if (cbPtr == IntPtr.Zero)
        {
            Logger.LogError("Failed to get callback function pointer.");
            return;
        }

        var rc = TWRC.FAILURE;

        // per the spec (pg 8-10), apps for 2.2 or higher uses callback2 so try this first
        if (_appIdentity.ProtocolMajor > 2 || (_appIdentity.ProtocolMajor >= 2 && _appIdentity.ProtocolMinor >= 2))
        {
            var cb2 = new TW_CALLBACK2 { CallBackProc = cbPtr };
            rc = DGControl.Callback2.RegisterCallback(_appIdentity, _currentDs!, ref cb2);
        }
        if (rc != TWRC.SUCCESS)
        {
            // always try old callback
            var cb = new TW_CALLBACK { CallBackProc = cbPtr };
            DGControl.Callback.RegisterCallback(_appIdentity, _currentDs!, ref cb);
        }
    }

    private ushort LegacyCallbackHandler
    (
        ref TW_IDENTITY_LEGACY origin, ref TW_IDENTITY_LEGACY dest,
        DG dg, DAT dat, MSG msg, IntPtr twnull
    )
    {
        if (Logger.IsEnabled(LogLevel.Trace))
            Logger.LogTrace("Legacy callback got {Msg}", msg);
        HandleSourceMsg(msg);
        return (ushort)TWRC.SUCCESS;
    }

    private ushort OSXCallbackHandler
    (
        ref TW_IDENTITY_MACOSX origin, ref TW_IDENTITY_MACOSX dest,
        DG dg, DAT dat, MSG msg, IntPtr twnull
    )
    {
        if (Logger.IsEnabled(LogLevel.Trace))
            Logger.LogTrace("OSX callback got {Msg}", msg);
        HandleSourceMsg(msg);
        return (ushort)TWRC.SUCCESS;
    }

    private void HandleSourceMsg(MSG msg, [CallerMemberName] string? caller = null)
    {
        if (Logger.IsEnabled(LogLevel.Trace))
            Logger.LogTrace("[thread {ThreadId}] {Caller} called by {Caller} at state {State} with {Msg}.",
            Environment.CurrentManagedThreadId, nameof(HandleSourceMsg), caller, State, msg);

        switch (msg)
        {
            case MSG.XFERREADY:
                _transferThread.NotifyTransferReady(true);
                break;
            case MSG.CLOSEDSOK: // user click ok in driver-only dialog
                DisableSource();
                break;
            case MSG.CLOSEDSREQ:
                if (EnabledWithOption == SourceEnableOption.UIOnly ||// user click cancel in driver dialog
                    !_transferThread.IsTransferring)
                {
                    DisableSource();
                }
                else
                {
                    // set flag to cancel transfer and let it disable ds
                    _transferThread.CloseDsRequested = true;
                }
                break;
            case MSG.DEVICEEVENT:
                if (DeviceEvent != null &&
                    _currentDs != null &&
                    DGControl.DeviceEvent.Get(_appIdentity, _currentDs, out TW_DEVICEEVENT de) == TWRC.SUCCESS)
                {
                    RaiseEvent(DeviceEvent, de);
                }
                break;
        }
    }
}