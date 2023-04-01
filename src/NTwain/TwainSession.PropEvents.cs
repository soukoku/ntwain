using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TWAINWorkingGroup;

namespace NTwain
{
  // this file contains property and event definitions

  partial class TwainSession
  {

    // really legacy version is the one to be used (except on mac) or
    // until it doesn't work (special linux)

    /// <summary>
    /// Gets the app identity.
    /// </summary>
    public TW_IDENTITY_LEGACY AppIdentity => _appIdentityLegacy;
    internal TW_IDENTITY_LEGACY _appIdentityLegacy;
    internal TW_IDENTITY _appIdentity;
    internal TW_IDENTITY_MACOSX _appIdentityOSX;

    /// <summary>
    /// Gets the current data source.
    /// </summary>
    public TW_IDENTITY_LEGACY DSIdentity => _dsIdentityLegacy;
    internal TW_IDENTITY_LEGACY _dsIdentityLegacy;
    internal TW_IDENTITY _dsIdentity;
    internal TW_IDENTITY_MACOSX _dsIdentityOSX;


    private STATE _state = STATE.S1;

    /// <summary>
    /// Current TWAIN session state.
    /// </summary>
    public STATE State
    {
      get => _state;
      private set
      {
        if (_state != value)
        {
          _state = value;
          StateChanged?.Invoke(this, value); // TODO: should care about thread
        }
      }
    }

    /// <summary>
    /// Fired when state changes.
    /// </summary>
    public event Action<TwainSession, STATE>? StateChanged;
  }
}
