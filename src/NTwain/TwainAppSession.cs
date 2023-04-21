using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NTwain
{
  // this file contains initialization/cleanup things.

  public partial class TwainAppSession : IDisposable
  {
    /// <summary>
    /// Creates TWAIN session with explicit app info.
    /// </summary>
    /// <param name="appId"></param>
    public TwainAppSession(TW_IDENTITY_LEGACY appId)
    {
#if WINDOWS || NETFRAMEWORK
      DSM.DsmLoader.TryLoadCustomDSM();
#endif
      _appIdentity = appId;

      _legacyCallbackDelegate = LegacyCallbackHandler;
      _osxCallbackDelegate = OSXCallbackHandler;

      StartTransferThread();
    }


    internal IntPtr _hwnd;
    internal TW_USERINTERFACE _userInterface; // kept around for disable to use
#if WINDOWS || NETFRAMEWORK
    MessagePumpThread? _selfPump;
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

#if WINDOWS || NETFRAMEWORK
    /// <summary>
    /// Loads and opens the TWAIN data source manager in a self-hosted message queue thread.
    /// Highly experimental and only use if necessary. Must close with <see cref="CloseDSMAsync"/>
    /// if used.
    /// </summary>
    /// <returns></returns>
    public async Task<STS> OpenDSMAsync()
    {
      if (_selfPump == null)
      {
        var pump = new MessagePumpThread();
        var sts = await pump.AttachAsync(this);
        if (sts.IsSuccess)
        {
          _selfPump = pump;
        }
        return sts;
      }
      return new STS { RC = TWRC.FAILURE, STATUS = new TW_STATUS { ConditionCode = TWCC.SEQERROR } };
    }

    /// <summary>
    /// Closes the TWAIN data source manager if opened with <see cref="OpenDSMAsync"/>.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<STS> CloseDSMAsync()
    {
      if (_selfPump == null) throw new InvalidOperationException($"Cannot close if not opened with {nameof(OpenDSMAsync)}().");

      var sts = await _selfPump.DetatchAsync();
      if (sts.IsSuccess)
      {
        _selfPump = null;
      }
      return sts;
    }
#endif

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
    /// <exception cref="InvalidOperationException"></exception>
    public STS CloseDSM()
    {
#if WINDOWS || NETFRAMEWORK
      if (_selfPump != null) throw new InvalidOperationException($"Cannot close if opened with {nameof(OpenDSMAsync)}().");
#endif
      return CloseDSMReal();
    }

    /// <summary>
    /// Closes the TWAIN data source manager.
    /// </summary>
    /// <returns></returns>
    internal STS CloseDSMReal()
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
#if WINDOWS || NETFRAMEWORK
            if (_selfPump != null)
            {
              try
              {
                _ = CloseDSMAsync();
              }
              catch (InvalidOperationException) { }
            }
            else
            {
              CloseDSM();
            }
#else
            CloseDSM();
#endif
            break;
          case STATE.S2:
            // can't really go lower
            if (targetState < STATE.S2)
            {
              return State;
            }
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
