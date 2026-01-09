using NTwain.Data;
using NTwain.Events;
using NTwain.Platform;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

// this file tracks twain state and has methods that can change the twain state and 
// manage data sources.

namespace NTwain;

partial class TwainAppSession
{
    /// <summary>
    /// Occurs when the <see cref="State"/> changes.
    /// </summary>
    public event EventHandler<TwainAppSession, STATE>? StateChanged;

    /// <summary>
    /// Occurs when the <see cref="CurrentSource"/> changes.
    /// </summary>
    public event EventHandler<TwainAppSession, TWIdentityWrapper?>? CurrentSourceChanged;

    /// <summary>
    /// Fires when <see cref="DefaultSource"/> changes.
    /// </summary>
    public event EventHandler<TwainAppSession, TWIdentityWrapper?>? DefaultSourceChanged;

    /// <summary>
    /// Fires when the source is disabled.
    /// </summary>
    public event EventHandler<TwainAppSession, TWIdentityWrapper>? SourceDisabled;

    /// <summary>
    /// Fires when the source sends a device event.
    /// </summary>
    public event EventHandler<TwainAppSession, TW_DEVICEEVENT>? DeviceEvent;

    /// <summary>
    /// Fires when there's an upcoming transfer. App can inspect the image info,
    /// setup certain transfer parameters, or cancel the transfer if needed.
    /// </summary>
    public event EventHandler<TwainAppSession, TransferReadyEventArgs>? TransferReady;

    /// <summary>
    /// Fires when there's an error during transfer.
    /// </summary>
    public event EventHandler<TwainAppSession, TransferErrorEventArgs>? TransferError;

    /// <summary>
    /// Fires when transferred data is available for app to use.
    /// </summary>
    public event EventHandler<TwainAppSession, TransferredEventArgs>? Transferred;

    internal void RaiseTransferError(TransferErrorEventArgs args)
    {
        RaiseEvent(TransferError, args);
    }
    internal void RaiseTransferReady(TransferReadyEventArgs args)
    {
        RaiseEventAndWait(TransferReady, args);
    }
    internal void RaiseTransferred(TransferredEventArgs args)
    {
        // no wait here, app can handle at its leisure
        RaiseEvent(Transferred, args);
    }



    private STATE _state;
    /// <summary>
    /// The current TWAIN state of the application session.
    /// </summary>
    public STATE State
    {
        get { return _state; }
        internal set
        {
            _state = value;
            RaiseEvent(StateChanged, value);
        }
    }


    TWIdentityWrapper? _defaultDs;
    /// <summary>
    /// The default TWAIN data source.
    /// </summary>
    public TWIdentityWrapper? DefaultSource
    {
        get { return _defaultDs; }
        private set
        {
            _defaultDs = value;
            RaiseEvent(DefaultSourceChanged, value);
        }
    }


    TWIdentityWrapper? _currentDs;
    /// <summary>
    /// The current open TWAIN data source.
    /// </summary>
    public TWIdentityWrapper? CurrentSource
    {
        get { return _currentDs; }
        private set
        {
            _currentDs = value;
            RaiseEvent(CurrentSourceChanged, value);
        }
    }

    private IMemoryManager? _memoryManager;

    /// <summary>
    /// Gets the memory manager for this TWAIN session.
    /// </summary>
    /// <exception cref="InvalidOperationException">If OpenDsm() has not been called.</exception>
    public IMemoryManager MemoryManager
    {
        get
        {
            if (_memoryManager == null)
            {
                throw new InvalidOperationException("Memory manager is not initialized. Call OpenDsm() first.");
            }
            return _memoryManager;
        }
    }

    /// <summary>
    /// Raises an event on the application thread context if available, otherwise on the current thread.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of event arguments.</typeparam>
    /// <param name="eventHandler">The event handler to invoke.</param>
    /// <param name="eventArgs">The event arguments.</param>
    protected void RaiseEvent<TEventArgs>(EventHandler<TwainAppSession, TEventArgs>? eventHandler, TEventArgs eventArgs)
    {
        if (eventHandler == null) return;

        if (AppThreadContext != null)
        {
            AppThreadContext.Post(_ => eventHandler(this, eventArgs), null);
        }
        else
        {
            eventHandler(this, eventArgs);
        }
    }

    /// <summary>
    /// Raises an event on the application thread context if available, otherwise on the current thread.
    /// </summary>
    /// <typeparam name="TEventArgs">The type of event arguments.</typeparam>
    /// <param name="eventHandler">The event handler to invoke.</param>
    /// <param name="eventArgs">The event arguments.</param>
    protected void RaiseEventAndWait<TEventArgs>(EventHandler<TwainAppSession, TEventArgs>? eventHandler, TEventArgs eventArgs)
    {
        if (eventHandler == null) return;

        if (AppThreadContext != null)
        {
            using var waitHandle = new ManualResetEventSlim(false);
            AppThreadContext.Post(_ =>
            {
                try
                {
                    eventHandler(this, eventArgs);
                }
                finally
                {
                    waitHandle.Set();
                }
            }, null);
            waitHandle.Wait();
        }
        else
        {
            eventHandler(this, eventArgs);
        }
    }

    /// <summary>
    /// Executes a triplet call, handling Windows-specific synchronization if needed.
    /// </summary>
    /// <typeparam name="T">The return type of the triplet call.</typeparam>
    /// <param name="action">The triplet action to execute.</param>
    /// <returns>The result of the triplet call.</returns>
    protected T InvokeTriplet<T>(Func<T> action)
    {
        if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600) && _twainPumpForWin != null)
        {
            T result = default!;
            _twainPumpForWin.SynchronizationContext!.Send(obj =>
            {
                result = action();
            }, this);
            return result;
        }
        else
        {
            return action();
        }
    }

    /// <summary>
    /// Loads and opens the DSM for use. Can only be called in S1 or S3 state.
    /// </summary>
    /// <returns></returns>
    public STS OpenDsm()
    {
        if (State >= STATE.S3)
            return new STS { RC = TWRC.FAILURE, STATUS = new TW_STATUS { ConditionCode = TWCC.SEQERROR } };

        var hwnd = OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600) && _twainPumpForWin != null
            ? _twainPumpForWin.MainWindow
            : IntPtr.Zero;

        var sts = InvokeTriplet(() =>
        {
            var rc = DGControl.Parent.OpenDSM(_appIdentity, hwnd);
            var sts = WrapInSTS(rc, forDsm: true);

            if (rc == TWRC.SUCCESS)
            {
                State = STATE.S3;

                // determine memory mgmt routines used
                TW_ENTRYPOINT_DELEGATES entryPoint = default;
                if (((DG)_appIdentity.SupportedGroups & DG.DSM2) == DG.DSM2)
                {
                    DGControl.EntryPoint.Get(_appIdentity, out entryPoint);
                }

                if (entryPoint.HasDelegates())
                {
                    _memoryManager = new TwainMemoryManager(entryPoint);
                }
                else if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
                {
                    _memoryManager = new Win32MemoryManager();
                }
                else
                {
                    _memoryManager = new FallbackMemoryManager();
                }

                // get default source
                if (DGControl.Identity.GetDefault(_appIdentity, out TWIdentityWrapper ds) == TWRC.SUCCESS)
                {
                    DefaultSource = ds;
                }
            }
            return sts;
        });

        return sts;
    }

    /// <summary>
    /// Closes the DSM. Can only be done in S3 state.
    /// </summary>
    /// <returns></returns>
    public STS CloseDsm()
    {
        if (State != STATE.S3)
            return new STS { RC = TWRC.FAILURE, STATUS = new TW_STATUS { ConditionCode = TWCC.SEQERROR } };

        var hwnd = OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600) && _twainPumpForWin != null
            ? _twainPumpForWin.MainWindow
            : IntPtr.Zero;

        var sts = InvokeTriplet(() =>
        {
            var rc = DGControl.Parent.CloseDSM(_appIdentity, hwnd);
            if (rc == TWRC.SUCCESS)
            {
                _memoryManager = null;
                DefaultSource = null;
                State = STATE.S2;
            }
            return WrapInSTS(rc, forDsm: true);
        });
        return sts;
    }

    /// <summary>
    /// Gets all available sources.
    /// </summary>
    /// <returns></returns>
    public IList<TWIdentityWrapper> GetSources()
    {
        // haven't found a way to enumerate like before with
        // potentially a different thread so List it is.
        return InvokeTriplet(() =>
        {
            List<TWIdentityWrapper> sources = [];
            var rc = DGControl.Identity.GetFirst(_appIdentity, out TWIdentityWrapper ds);
            while (rc == TWRC.SUCCESS)
            {
                sources.Add(ds);
                rc = DGControl.Identity.GetNext(_appIdentity, out ds);
            }
            return sources;
        });
    }

    /// <summary>
    /// Shows the TWAIN source selection UI for setting the default source.
    /// Only included for completeness, not recommended for real app usage.
    /// </summary>
    public STS ShowUserSelect()
    {
        return InvokeTriplet(() =>
        {
            var rc = DGControl.Identity.UserSelect(_appIdentity, out TWIdentityWrapper ds);
            if (rc == TWRC.SUCCESS)
            {
                DefaultSource = ds;
            }
            return WrapInSTS(rc);
        });
    }

    /// <summary>
    /// Sets the default data source.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public STS SetDefaultSource(TWIdentityWrapper source)
    {
        // this doesn't work on windows legacy twain_32.dll

        return InvokeTriplet(() =>
        {
            var rc = DGControl.Identity.Set(_appIdentity, source);
            if (rc == TWRC.SUCCESS)
            {
                DefaultSource = source;
            }
            return WrapInSTS(rc);
        });
    }

    /// <summary>
    /// Opens the specified source.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public STS OpenSource(TWIdentityWrapper source)
    {
        if (_currentDs != null)
        {
            var sts = CloseSource();
            if (!sts.IsSuccess) return sts;
        }

        return InvokeTriplet(() =>
        {
            var rc = DGControl.Identity.OpenDS(_appIdentity, source);
            if (rc == TWRC.SUCCESS)
            {
                Language.Set(source.Version.Language);
                CurrentSource = source;
                RegisterCallback();
                State = STATE.S4;
            }
            return WrapInSTS(rc);
        });
    }

    /// <summary>
    /// Closes the current source if it's open.
    /// </summary>
    /// <returns></returns>
    public STS CloseSource()
    {
        if (_currentDs == null)
        {
            return STS.SequenceError();
        }

        return InvokeTriplet(() =>
        {
            var rc = DGControl.Identity.CloseDS(_appIdentity, _currentDs);
            if (rc == TWRC.SUCCESS)
            {
                CurrentSource = null;
                State = STATE.S3;
            }
            return WrapInSTS(rc);
        });
    }

    /// <summary>
    /// Option that was used to enable the current data source.
    /// </summary>
    public SourceEnableOption EnabledWithOption { get; private set; }

    /// <summary>
    /// Enables the current data source, optionally displaying the user interface based on the specified option.
    /// </summary>
    /// <param name="option"></param>
    public STS EnableSource(SourceEnableOption option)
    {
        if (_currentDs == null)
        {
            return STS.SequenceError();
        }

        var hwnd = OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600) && _twainPumpForWin != null
            ? _twainPumpForWin.MainWindow
            : IntPtr.Zero;

        return InvokeTriplet(() =>
        {
            var showUI = option == SourceEnableOption.ShowUI || option == SourceEnableOption.UIOnly;

            TW_USERINTERFACE ui = new()
            {
                ShowUI = (ushort)(showUI ? 1 : 0),
                hParent = hwnd
            };

            _transferThread.NotifyTransferReady(false);

            var rc = option == SourceEnableOption.UIOnly ?
                DGControl.UserInterface.EnableDSUIOnly(_appIdentity, _currentDs, ref ui) :
                DGControl.UserInterface.EnableDS(_appIdentity, _currentDs, ref ui);

            // user may choose no ui but if it's not supported by source
            // then it returns check status but still considered a success.
            if (rc == TWRC.SUCCESS || (!showUI && rc == TWRC.CHECKSTATUS))
            {
                _transferThread.CloseDsRequested = false;
                EnabledWithOption = option;
                State = STATE.S5;
            }
            return WrapInSTS(rc);
        });
    }


    /// <summary>
    /// Disables the currently enabled source.
    /// </summary>
    /// <returns></returns>
    public STS DisableSource()
    {
        if (_currentDs == null)
        {
            return STS.SequenceError();
        }

        var hwnd = OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600) && _twainPumpForWin != null
            ? _twainPumpForWin.MainWindow
            : IntPtr.Zero;

        return InvokeTriplet(() =>
        {
            var showUI = EnabledWithOption == SourceEnableOption.ShowUI || EnabledWithOption == SourceEnableOption.UIOnly;
            TW_USERINTERFACE ui = new()
            {
                ShowUI = (ushort)(showUI ? 1 : 0),
                hParent = hwnd
            };

            var rc = DGControl.UserInterface.DisableDS(_appIdentity, _currentDs, ref ui);
            if (rc == TWRC.SUCCESS)
            {
                State = STATE.S4;
                RaiseEvent(SourceDisabled, _currentDs);
            }
            return WrapInSTS(rc);
        });
    }

    /// <summary>
    /// Wraps a return code with additional status if not successful.
    /// Use this right after an API call to get its condition code.
    /// </summary>
    /// <param name="rc"></param>
    /// <param name="forDsm">true to get status for dsm operation error, false to get status for ds operation error,</param>
    /// <returns></returns>
    public STS WrapInSTS(TWRC rc, bool forDsm = false)
    {
        if (rc != TWRC.FAILURE) return new STS { RC = rc };
        var sts = new STS { RC = rc, STATUS = GetLastStatus(forDsm) };

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
    /// <param name="forDsm">true to get status for dsm operation error, false to get status for ds operation error,</param>
    /// <returns></returns>
    public TW_STATUS GetLastStatus(bool forDsm = false)
    {
        return InvokeTriplet(() =>
        {
            TW_STATUS status = default;
            if (forDsm)
            {
                DGControl.Status.GetForDSM(_appIdentity, out status);
            }
            else
            {
                if (_currentDs == null) return new TW_STATUS { ConditionCode = TWCC.BADDEST };
                DGControl.Status.GetForDS(_appIdentity, _currentDs, out status);
            }
            return status;
        });
    }


    /// <summary>
    /// Tries to bring the TWAIN session down to some state.
    /// For use when things got out of hand.
    /// </summary>
    /// <param name="targetState"></param>
    /// <returns>The final state.</returns>
    public STATE TryStepdown(STATE targetState)
    {
        return InvokeTriplet(() =>
        {
            if (targetState < State && State >= STATE.S6)
            {
                var pending = TW_PENDINGXFERS.DONTCARE();
                DGControl.PendingXfers.EndXfer(_appIdentity, _currentDs!, ref pending);
                pending = TW_PENDINGXFERS.DONTCARE();
                DGControl.PendingXfers.Reset(_appIdentity, _currentDs!, ref pending);
            }

            if (targetState < State && State == STATE.S5)
            {
                DisableSource();
            }

            if (targetState < State && State == STATE.S4)
            {
                CloseSource();
            }

            if (targetState < State && State == STATE.S3)
            {
                CloseDsm();
            }

            // can't go lower than S2

            return State;
        });
    }
}
