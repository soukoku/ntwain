using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Runtime.InteropServices;

namespace NTwain
{
  // this file contains property and event definitions

  partial class TwainAppSession
  {
    /// <summary>
    /// Gets the app identity.
    /// </summary>
    public TW_IDENTITY_LEGACY AppIdentity
    {
      get => _appIdentity;
    }
    TW_IDENTITY_LEGACY _appIdentity;

    /// <summary>
    /// Gets the current (opened) data source.
    /// </summary>
    public TW_IDENTITY_LEGACY CurrentSource
    {
      get => _currentDS;
      protected set
      {
        _currentDS = value;
        try
        {
          CurrentSourceChanged?.Invoke(this, value);
        }
        catch { }
      }
    }
    TW_IDENTITY_LEGACY _currentDS;

    /// <summary>
    /// Gets/sets the default data source.
    /// </summary>
    public TW_IDENTITY_LEGACY DefaultSource
    {
      get => _defaultDS;
    }
    TW_IDENTITY_LEGACY _defaultDS;


    /// <summary>
    /// Current TWAIN session state.
    /// </summary>
    public STATE State
    {
      get => _state;
      protected set
      {
        if (_state != value)
        {
          _state = value;
          _uiThreadMarshaller.Send(obj =>
          {
            try
            {
              ((TwainAppSession)obj!).StateChanged?.Invoke(this, value);
            }
            catch { }
          }, this);
        }
      }
    }
    STATE _state = STATE.S1;


    /// <summary>
    /// Gets/sets the current source's settings as opaque data.
    /// Returns null if not supported. This is only valid in <see cref="STATE.S4"/>.
    /// </summary>
    public byte[]? CustomDsData
    {
      get
      {
        var rc = DGControl.CustomDsData.Get(ref _appIdentity, ref _currentDS, out TW_CUSTOMDSDATA data);
        if (rc == TWRC.SUCCESS)
        {
          if (data.hData != IntPtr.Zero && data.InfoLength > 0)
          {
            try
            {
              var lockedPtr = Lock(data.hData);
              var bytes = new byte[data.InfoLength];
              Marshal.Copy(lockedPtr, bytes, 0, bytes.Length);
            }
            finally
            {
              Unlock(data.hData);
              Free(data.hData);
            }
          }
          //return Array.Empty<byte>();
        }
        return null;
      }
      set
      {
        if (value == null || value.Length == 0) return;

        TW_CUSTOMDSDATA data = default;
        data.InfoLength = (uint)value.Length;
        data.hData = Alloc(data.InfoLength);
        try
        {
          var lockedPtr = Lock(data.hData);
          Marshal.Copy(value, 0, lockedPtr, value.Length);
          Unlock(data.hData);
          var rc = DGControl.CustomDsData.Set(ref _appIdentity, ref _currentDS, ref data);
        }
        finally
        {
          // should be freed already if no error but just in case
          if (data.hData != IntPtr.Zero) Free(data.hData);
        }
      }
    }


    /// <summary>
    /// Fires when <see cref="State"/> changes.
    /// </summary>
    public event TwainEventDelegate<STATE>? StateChanged;

    /// <summary>
    /// Fires when <see cref="DefaultSource"/> changes.
    /// </summary>
    public event TwainEventDelegate<TW_IDENTITY_LEGACY>? DefaultSourceChanged;

    /// <summary>
    /// Fires when <see cref="CurrentSource"/> changes (opened and closed).
    /// </summary>
    public event TwainEventDelegate<TW_IDENTITY_LEGACY>? CurrentSourceChanged;

    /// <summary>
    /// Fires when source has moved from enabled to disabled state.
    /// </summary>
    public event TwainEventDelegate<TW_IDENTITY_LEGACY>? SourceDisabled;

    /// <summary>
    /// Fires when the source has some device event happening.
    /// </summary>
    public event TwainEventDelegate<TW_DEVICEEVENT>? DeviceEvent;

    /// <summary>
    /// Fires when there's an error during transfer.
    /// </summary>
    public event TwainEventDelegate<TransferErrorEventArgs>? TransferError;

    /// <summary>
    /// Fires when there's an upcoming transfer. App can inspect the image info
    /// and cancel if needed.
    /// </summary>
    public event TwainEventDelegate<TransferReadyEventArgs>? TransferReady;

    /// <summary>
    /// Fires when there's a transfer cancellation, e.g. if the user pressed the "Cancel" button.
    /// </summary>
    public event TwainEventDelegate<TransferCanceledEventArgs>? TransferCanceled;

    /// <summary>
    /// Fires when transferred data is available for app to use.
    /// This is NOT raised on the UI thread for reasons.
    /// </summary>
    public event TwainEventDelegate<TransferredEventArgs>? Transferred;
  }
}
