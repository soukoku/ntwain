using NTwain.Data;
using NTwain.Triplets;
using System;
using System.Runtime.InteropServices;

namespace NTwain
{
  // this file contains property and event definitions

  partial class TwainSession
  {
    /// <summary>
    /// Gets the app identity.
    /// </summary>
    public TW_IDENTITY_LEGACY AppIdentity
    {
      get => _appIdentity;
      internal set
      {
        _appIdentity = value;
      }
    }
    TW_IDENTITY_LEGACY _appIdentity;

    /// <summary>
    /// Gets the current (opened) data source.
    /// </summary>
    public TW_IDENTITY_LEGACY CurrentSource
    {
      get => _currentDS;
      internal set
      {
        _currentDS = value;
        CurrentSourceChanged?.Invoke(this, value);
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
      internal set
      {
        if (_state != value)
        {
          _state = value;
          StateChanged?.Invoke(this, value); // TODO: should care about thread
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
        var sts = DGControl.CustomDsData.Get(ref _appIdentity, ref _currentDS, out TW_CUSTOMDSDATA data);
        if (sts == STS.SUCCESS)
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
          var sts = DGControl.CustomDsData.Set(ref _appIdentity, ref _currentDS, ref data);
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
    public event Action<TwainSession, STATE>? StateChanged;

    /// <summary>
    /// Fires when <see cref="DefaultSource"/> changes.
    /// </summary>
    public event Action<TwainSession, TW_IDENTITY_LEGACY>? DefaultSourceChanged;

    /// <summary>
    /// Fires when <see cref="CurrentSource"/> changes.
    /// </summary>
    public event Action<TwainSession, TW_IDENTITY_LEGACY>? CurrentSourceChanged;

    /// <summary>
    /// Fires when the source has some device event happening.
    /// </summary>
    public event Action<TwainSession, TW_DEVICEEVENT>? DeviceEvent;

    /// <summary>
    /// Fires when there's an error during transfer.
    /// </summary>
    public event EventHandler<TransferErrorEventArgs>? TransferError;
  }
}
