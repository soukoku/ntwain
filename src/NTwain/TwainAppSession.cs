﻿using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml;

namespace NTwain
{
  // this file contains initialization/cleanup things.

  public partial class TwainAppSession : IDisposable
  {
    static bool __encodingRegistered;

    /// <summary>
    /// Creates TWAIN session with app info derived an executable file.
    /// </summary>
    /// <param name="exeFilePath"></param>
    /// <param name="appLanguage"></param>
    /// <param name="appCountry"></param>
    public TwainAppSession(
      string exeFilePath,
      TWLG appLanguage = TWLG.ENGLISH_USA, TWCY appCountry = TWCY.USA) :
      this(FileVersionInfo.GetVersionInfo(exeFilePath), appLanguage, appCountry)
    { }
    /// <summary>
    /// Creates TWAIN session with app info derived from a <see cref="FileVersionInfo"/> object.
    /// </summary>
    /// <param name="appInfo"></param>
    /// <param name="appLanguage"></param>
    /// <param name="appCountry"></param>
    public TwainAppSession(
        FileVersionInfo appInfo,
        TWLG appLanguage = TWLG.ENGLISH_USA, TWCY appCountry = TWCY.USA) :
        this(
          appInfo.CompanyName ?? "",
          appInfo.ProductName ?? "",
          appInfo.ProductName ?? "",
          new Version(appInfo.FileVersion ?? "1.0"),
          appInfo.FileDescription ?? "", appLanguage, appCountry)
    { }
    /// <summary>
    /// Creates TWAIN session with explicit app info.
    /// </summary>
    /// <param name="companyName"></param>
    /// <param name="productFamily"></param>
    /// <param name="productName"></param>
    /// <param name="productVersion"></param>
    /// <param name="productDescription"></param>
    /// <param name="appLanguage"></param>
    /// <param name="appCountry"></param>
    /// <param name="supportedTypes"></param>
    public TwainAppSession(
        string companyName, string productFamily, string productName,
        Version productVersion, string productDescription = "",
        TWLG appLanguage = TWLG.ENGLISH_USA, TWCY appCountry = TWCY.USA,
        DG supportedTypes = DG.IMAGE)
    {
#if WINDOWS || NETFRAMEWORK
      DSM.DsmLoader.TryLoadCustomDSM();
#endif
      // todo: find a better place for this
      if (!__encodingRegistered)
      {
#if !NETFRAMEWORK
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        __encodingRegistered = true;
      }

      _appIdentity = new()
      {
        Manufacturer = companyName,
        ProductFamily = productFamily,
        ProductName = productName,
        ProtocolMajor = (ushort)TWON_PROTOCOL.MAJOR,
        ProtocolMinor = (ushort)TWON_PROTOCOL.MINOR,
        SupportedGroups = (uint)(supportedTypes | DG.CONTROL | DG.APP2),
        Version = new TW_VERSION
        {
          Country = appCountry,
          Info = productDescription,
          Language = appLanguage,
          MajorNum = (ushort)productVersion.Major,
          MinorNum = (ushort)productVersion.Minor,
        }
      };

      _legacyCallbackDelegate = LegacyCallbackHandler;
      _osxCallbackDelegate = OSXCallbackHandler;

      StartTransferThread();
    }

    internal IntPtr _hwnd;
    internal TW_USERINTERFACE _userInterface; // kept around for disable to use
#if WINDOWS || NETFRAMEWORK
    TW_EVENT _procEvent; // kept here so the alloc/free only happens once
#endif
    // test threads a bit
    //readonly BlockingCollection<MSG> _bgPendingMsgs = new();
    SynchronizationContext? _pumpThreadMarshaller;
    bool _closeDsRequested;
    bool _inTransfer;
    readonly AutoResetEvent _xferReady = new(false);
    private bool disposedValue;

    void StartTransferThread()
    {
      Thread t = new(TransferLoopLoop)
      {
        IsBackground = true
      };
#if WINDOWS || NETFRAMEWORK
      t.SetApartmentState(ApartmentState.STA); // just in case
#endif
      t.Start();
    }

    private void TransferLoopLoop(object? obj)
    {
      while (!disposedValue)
      {
        try
        {
          _xferReady.WaitOne();
        }
        catch (ObjectDisposedException) { break; }
        try
        {
          EnterTransferRoutine();
        }
        catch { }
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // this will end the bg thread
          _xferReady.Dispose();

          //_bgPendingMsgs.CompleteAdding();
        }
#if WINDOWS || NETFRAMEWORK
        if (_procEvent.pEvent != IntPtr.Zero) Marshal.FreeHGlobal(_procEvent.pEvent);
#endif
        disposedValue = true;
      }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~TwainSession()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }


    /// <summary>
    /// Loads and opens the TWAIN data source manager.
    /// </summary>
    /// <param name="hwnd">Required if on Windows.</param>
    /// <param name="uiThreadMarshaller">Context for TWAIN to invoke certain actions on the thread that the hwnd lives on.</param>
    /// <returns></returns>
    public STS OpenDSM(IntPtr hwnd, SynchronizationContext uiThreadMarshaller)
    {
      var rc = DGControl.Parent.OpenDSM(ref _appIdentity, hwnd);
      if (rc == TWRC.SUCCESS)
      {
        _hwnd = hwnd;
        _pumpThreadMarshaller = uiThreadMarshaller;
        State = STATE.S3;
        // get default source
        if (DGControl.Identity.GetDefault(ref _appIdentity, out TW_IDENTITY_LEGACY ds) == TWRC.SUCCESS)
        {
          _defaultDS = ds;
          try
          {
            DefaultSourceChanged?.Invoke(this, _defaultDS);
          }
          catch { }
        }

        // determine memory mgmt routines used
        if (((DG)AppIdentity.SupportedGroups & DG.DSM2) == DG.DSM2)
        {
          DGControl.EntryPoint.Get(ref _appIdentity, out _entryPoint);
        }
      }
      return WrapInSTS(rc, true);
    }


    /// <summary>
    /// Closes the TWAIN data source manager.
    /// </summary>
    /// <returns></returns>
    public STS CloseDSM()
    {
      var rc = DGControl.Parent.CloseDSM(ref _appIdentity, _hwnd);
      if (rc == TWRC.SUCCESS)
      {
        State = STATE.S2;
        _entryPoint = default;
        _defaultDS = default;
        try
        {
          DefaultSourceChanged?.Invoke(this, _defaultDS);
        }
        catch { }
        _hwnd = IntPtr.Zero;
        _pumpThreadMarshaller = null;
      }
      return WrapInSTS(rc, true);
    }

    /// <summary>
    /// Wraps a return code with additional status if not successful.
    /// Use this right after an API call to get its condition code.
    /// </summary>
    /// <param name="rc"></param>
    /// <param name="dsmOnly">true to get status for dsm operation error, false to get status for ds operation error,</param>
    /// <returns></returns>
    public STS WrapInSTS(TWRC rc, bool dsmOnly = false)
    {
      if (rc != TWRC.FAILURE) return new STS { RC = rc };
      var sts = new STS { RC = rc, STATUS = GetLastStatus(dsmOnly) };
      if (sts.STATUS.ConditionCode == TWCC.BADDEST)
      {
        // TODO: the current ds is bad, should assume we're back in S3?
        // needs the dest parameter to find out.
      }
      else if (sts.STATUS.ConditionCode == TWCC.BUMMER)
      {
        // TODO: notify with critical event to end the twain stuff
      }
      return sts;
    }

    /// <summary>
    /// Gets the last status code if an operation did not return success.
    /// This can only be done once after an error.
    /// </summary>
    /// <param name="dsmOnly">true to get status for dsm operation error, false to get status for ds operation error,</param>
    /// <returns></returns>
    public TW_STATUS GetLastStatus(bool dsmOnly = false)
    {
      if (dsmOnly)
      {
        DGControl.Status.GetForDSM(ref _appIdentity, out TW_STATUS status);
        return status;
      }
      else
      {
        DGControl.Status.GetForDS(ref _appIdentity, ref _currentDS, out TW_STATUS status);
        return status;
      }
    }

    /// <summary>
    /// Tries to get string representation of a previously gotten status 
    /// from <see cref="GetLastStatus"/> if possible.
    /// </summary>
    /// <param name="status"></param>
    /// <returns></returns>
    public string? GetStatusText(TW_STATUS status)
    {
      if (DGControl.StatusUtf8.Get(ref _appIdentity, status, out TW_STATUSUTF8 extendedStatus) == TWRC.SUCCESS)
      {
        return extendedStatus.Read(this);
      }
      return null;
    }

    /// <summary>
    /// Tries to bring the TWAIN session down to some state.
    /// </summary>
    /// <param name="targetState"></param>
    /// <returns>The final state.</returns>
    public STATE TryStepdown(STATE targetState)
    {
      int tries = 0;
      while (State > targetState)
      {
        var oldState = State;

        switch (oldState)
        {
          // todo: finish
          case STATE.S7:
          case STATE.S6:
            break;
          case STATE.S5:
            DisableSource();
            break;
          case STATE.S4:
            CloseSource();
            break;
          case STATE.S3:
            CloseDSM();
            break;
        }
        if (oldState == State)
        {
          // didn't work
          if (tries++ > 5) break;
        }
      }
      return State;
    }
  }
}
