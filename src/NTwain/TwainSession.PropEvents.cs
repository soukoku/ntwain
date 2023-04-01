using NTwain.Triplets;
using System;
using TWAINWorkingGroup;

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
    /// Gets the default data source.
    /// </summary>
    public TW_IDENTITY_LEGACY DefaultSource
    {
      get => _defaultDS;
      internal set
      {
        _defaultDS = value;
        DefaultSourceChanged?.Invoke(this, value);
      }
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
    /// TWAIN triplet API calls with <see cref="DG.CONTROL"/>.
    /// </summary>
    public DGControl DGControl { get; }
    /// <summary>
    /// TWAIN triplet API calls with <see cref="DG.IMAGE"/>.
    /// </summary>
    public DGImage DGImage { get; }
    /// <summary>
    /// TWAIN triplet API calls with <see cref="DG.AUDIO"/>.
    /// </summary>
    public DGAudio DGAudio { get; }




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

  }
}
