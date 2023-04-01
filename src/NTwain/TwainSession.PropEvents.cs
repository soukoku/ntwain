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
    private STATE _state = STATE.S1;

    /// <summary>
    /// Current TWAIN session state.
    /// </summary>
    public STATE State
    {
      get { return _state; }
      private set
      {
        if (_state != value)
        {
          _state = value;
          StateChanged?.Invoke(this, value);
        }
      }
    }

    /// <summary>
    /// Fired when state changes.
    /// </summary>
    public event Action<TwainSession, STATE>? StateChanged;
  }
}
