using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NTwain.Data;
using NTwain.Platform;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using WinMSG = Windows.Win32.UI.WindowsAndMessaging.MSG;

namespace NTwain;

// this file is for (non-twain) setup and cleanup.

/// <summary>
/// The main TWAIN access object for an application.
/// There should only be one of these per application.
/// </summary>
public partial class TwainAppSession : IDisposable
{
    // internal pump is used for windows so it can run headless
    Win32MessagePump? _twainPumpForWin;
    TW_EVENT _procEvent;
    // transfer has to happen on its own thread (not app thread, not windows message pump thread)
    // so it won't block either.
    TransferLoopThread _transferThread;

    readonly TWIdentityWrapper _appIdentity;
    public TWIdentityWrapper AppIdentity { get { return _appIdentity; } }


    /// <summary>
    /// Initializes a new instance of the TwainAppSession class, configuring the application session for TWAIN
    /// operations and background processing.
    /// </summary>
    /// <param name="appIdentity">The app information to use in TWAIN dsm calls. 
    /// If null, a default identity will be generated.</param>
    /// <param name="appThreadContext">The synchronization context associated with the application's main thread. 
    /// If null, then callbacks could happen on random threads.</param>
    /// <param name="logger">The logger instance used for diagnostic and error logging.</param>
    public TwainAppSession(
        TWIdentityWrapper? appIdentity = null,
        SynchronizationContext? appThreadContext = null,
        ILogger? logger = null)
    {
        _appIdentity = appIdentity ??
            new TWIdentityWrapper(
#if !NETFRAMEWORK
                Environment.ProcessPath ??
#endif
                Assembly.GetEntryAssembly()?.Location ??
                Assembly.GetExecutingAssembly().Location);

#if !NETFRAMEWORK
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        AppThreadContext = appThreadContext ?? SynchronizationContext.Current;
        _logger = logger ?? NullLogger.Instance;
        _legacyCallbackDelegate = LegacyCallbackHandler;
        _osxCallbackDelegate = OSXCallbackHandler;
        //_dsmCalls = Channel.CreateUnbounded<Action>();
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
        {
            DllPath.TryUseLocalDsm();

            // no need to do another lock call when using marshal alloc
            TW_EVENT _procEvent = default;
            _procEvent.pEvent = Marshal.AllocHGlobal(Marshal.SizeOf<WinMSG>());

            Thread pumpThread = new(() =>
            {
                if (OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
                {
                    _twainPumpForWin = new Win32MessagePump(_logger);
                    _twainPumpForWin.UnhandledException += (s, e) =>
                    {
                        _logger.LogError(e.Exception, "Unhandled exception in TWAIN message pump.");
                        e.Handled = true;
                    };
                    _twainPumpForWin.AddMessageFilter(this);
                    _twainPumpForWin.Run();
                }
            });
            pumpThread.IsBackground = true;
            pumpThread.SetApartmentState(ApartmentState.STA);
            pumpThread.Start();

            while (_twainPumpForWin == null || _twainPumpForWin.MainWindow.IsNull)
            {
                Thread.Sleep(100);
            }
        }
        _transferThread = new TransferLoopThread(this);
    }


    /// <summary>
    /// Used to marshal results back to the application thread if set,
    /// as many TWAIN activities occur on other threads.
    /// </summary>
    public SynchronizationContext? AppThreadContext { get; set; }

    private ILogger _logger;

    /// <summary>
    /// Gets or sets the logger used to record diagnostic and operational messages for this instance.
    /// </summary>
    public ILogger Logger
    {
        get { return _logger; }
        set { _logger = value ?? NullLogger.Instance; }
    }

    private bool _disposed;
    /// <summary>
    /// Overrides to clean up resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void OnDispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;

            if (disposing)
            {
                _transferThread.Dispose();

                //_dsmCalls.Writer.TryComplete();
                if (_twainPumpForWin != null && OperatingSystem.IsWindowsVersionAtLeast(5, 1, 2600))
                {
                    _twainPumpForWin.RemoveMessageFilter(this);
                    _twainPumpForWin.Quit();
                    _twainPumpForWin.Dispose();
                }

                if (_procEvent.pEvent != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_procEvent.pEvent);
                    _procEvent.pEvent = IntPtr.Zero;
                }
            }
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~TwainAppSession()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        OnDispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public override string ToString()
    {
        return $"State: {State}, App: {_appIdentity}, Source: {_currentDs}";
    }
}
