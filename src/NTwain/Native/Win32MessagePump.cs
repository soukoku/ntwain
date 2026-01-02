using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Threading;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using Windows.Win32.UI.WindowsAndMessaging;

namespace NTwain.Native;


#if !NETFRAMEWORK
[SupportedOSPlatform("windows5.1.2600")]
#endif
internal sealed class Win32MessagePump : IDisposable
{
    private const string WindowClassName = "MsgPumpParkWindow";
    private const uint WM_APP_INVOKE = PInvoke.WM_APP + 1;

    private readonly FreeLibrarySafeHandle _hInstance;
    private readonly uint _threadId;
    private HWND _mainWindow;
    private bool _disposed;

    // Store the delegate to prevent garbage collection
    private static WNDPROC? s_wndProc;

    // Queue for work items posted to the UI thread
    private readonly Queue<Action> _workQueue = new();
    private readonly object _workQueueLock = new();

    // Message filters
    private readonly List<IWin32MessageFilter> _messageFilters = new();
    private readonly object _messageFiltersLock = new();

    // SynchronizationContext
    private Win32SynchronizationContext? _synchronizationContext;

    public Win32MessagePump()
    {
        _hInstance = PInvoke.GetModuleHandle((string?)null);
        _threadId = PInvoke.GetCurrentThreadId();
    }

    /// <summary>
    /// Gets the main (hidden) message window handle.
    /// </summary>
    public HWND MainWindow => _mainWindow;

    public bool InvokeRequired => PInvoke.GetCurrentThreadId() != _threadId;

    /// <summary>
    /// Gets the SynchronizationContext associated with this message pump.
    /// </summary>
    public SynchronizationContext? SynchronizationContext => _synchronizationContext;

    /// <summary>
    /// Adds a message filter to the application's message pump.
    /// </summary>
    public void AddMessageFilter(IWin32MessageFilter filter)
    {
        lock (_messageFiltersLock)
        {
            _messageFilters.Add(filter);
        }
    }

    /// <summary>
    /// Removes a message filter from the application's message pump.
    /// </summary>
    public bool RemoveMessageFilter(IWin32MessageFilter filter)
    {
        lock (_messageFiltersLock)
        {
            return _messageFilters.Remove(filter);
        }
    }

    public int Run()
    {
        if (!RegisterWindowClass())
        {
            return -1;
        }

        if (!CreateMainWindow())
        {
            UnregisterWindowClass();
            return -1;
        }

        // Create and install the SynchronizationContext
        _synchronizationContext = new Win32SynchronizationContext(this);
        SynchronizationContext.SetSynchronizationContext(_synchronizationContext);

        int exitCode = RunMessageLoop();

        // Clear the SynchronizationContext
        SynchronizationContext.SetSynchronizationContext(null);

        Dispose();
        return exitCode;
    }

    private bool RegisterWindowClass()
    {
        s_wndProc = WindowProc;

        unsafe
        {
            fixed (char* className = WindowClassName)
            {
                var wc = new WNDCLASSEXW
                {
                    cbSize = (uint)sizeof(WNDCLASSEXW),
                    style = 0,
                    lpfnWndProc = s_wndProc,
                    cbClsExtra = 0,
                    cbWndExtra = 0,
                    hInstance = (HINSTANCE)_hInstance.DangerousGetHandle(),
                    hIcon = HICON.Null,
                    hCursor = HCURSOR.Null,
                    hbrBackground = HBRUSH.Null,
                    lpszMenuName = null,
                    lpszClassName = className,
                    hIconSm = HICON.Null
                };

                ushort atom = PInvoke.RegisterClassEx(in wc);
                if (atom == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void UnregisterWindowClass()
    {
        PInvoke.UnregisterClass(WindowClassName, _hInstance);
    }

    private bool CreateMainWindow()
    {
        unsafe
        {
            _mainWindow = PInvoke.CreateWindowEx(
                0,
                WindowClassName,
                "MsgPump Window",
                0,
                0, 0, 0, 0,
                new HWND(unchecked((nint)(-3))), // HWND_MESSAGE
                null,
                _hInstance,
                null);
        }

        return !_mainWindow.IsNull;
    }

    private int RunMessageLoop()
    {
        MSG msg;

        while (true)
        {
            int result;
            unsafe
            {
                result = PInvoke.GetMessage(&msg, HWND.Null, 0, 0);
            }

            if (result == 0) // WM_QUIT
            {
                return (int)msg.wParam.Value;
            }

            if (result == -1) // Error
            {
                return -1;
            }

            ProcessWorkQueue();

            if (FilterMessage(ref msg))
            {
                continue;
            }

            PInvoke.TranslateMessage(in msg);
            PInvoke.DispatchMessage(in msg);
        }
    }

    private bool FilterMessage(ref MSG msg)
    {
        var message = Win32Message.FromMSG(in msg);

        lock (_messageFiltersLock)
        {
            foreach (var filter in _messageFilters)
            {
                try
                {
                    if (filter.PreFilterMessage(ref message))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Message filter exception: {ex}");
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Post work to be executed on the UI thread.
    /// Can be called from any thread.
    /// </summary>
    public void PostToUIThread(Action action)
    {
        if (InvokeRequired)
        {
            lock (_workQueueLock)
            {
                _workQueue.Enqueue(action);
            }

            if (!_mainWindow.IsNull)
            {
                PInvoke.PostMessage(_mainWindow, WM_APP_INVOKE, 0, 0);
            }
        }
        else
        {
            action();
        }
    }

    /// <summary>
    /// Posts a quit message to terminate the message loop.
    /// </summary>
    public void Quit(int exitCode = 0)
    {
        if (InvokeRequired)
        {
            PostToUIThread(() =>
            {
                PInvoke.PostQuitMessage(exitCode);
            });
        }
        else
        {
            PInvoke.PostQuitMessage(exitCode);
        }
    }

    private void ProcessWorkQueue()
    {
        while (true)
        {
            Action? action;
            lock (_workQueueLock)
            {
                if (_workQueue.Count == 0)
                    break;
                action = _workQueue.Dequeue();
            }

            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Work item exception: {ex}");
            }
        }
    }

    private static LRESULT WindowProc(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
    {
        if (msg == PInvoke.WM_DESTROY)
        {
            PInvoke.PostQuitMessage(0);
            return new LRESULT(0);
        }

        return PInvoke.DefWindowProc(hwnd, msg, wParam, lParam);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _disposed = true;

        if (!_mainWindow.IsNull)
        {
            PInvoke.DestroyWindow(_mainWindow);
            _mainWindow = HWND.Null;
        }

        UnregisterWindowClass();
    }
}

/// <summary>
/// SynchronizationContext implementation for Win32MessagePump that integrates
/// with async/await and other .NET frameworks.
/// </summary>
#if !NETFRAMEWORK
[SupportedOSPlatform("windows5.1.2600")]
#endif
internal sealed class Win32SynchronizationContext : SynchronizationContext
{
    private readonly Win32MessagePump _messagePump;

    public Win32SynchronizationContext(Win32MessagePump messagePump)
    {
        _messagePump = messagePump ?? throw new ArgumentNullException(nameof(messagePump));
    }

    /// <summary>
    /// Dispatches an asynchronous message to the message pump thread.
    /// </summary>
    public override void Post(SendOrPostCallback d, object? state)
    {
        if (d == null) return;

        _messagePump.PostToUIThread(() =>
        {
            d(state);
        });
    }

    /// <summary>
    /// Dispatches a synchronous message to the message pump thread.
    /// This is not recommended for general use as it can cause deadlocks.
    /// </summary>
    public override void Send(SendOrPostCallback d, object? state)
    {
        if (d == null) return;

        if (!_messagePump.InvokeRequired)
        {
            // Already on the UI thread
            d(state);
            return;
        }

        // Send is synchronous - we need to block until the callback completes
        // This is dangerous and can cause deadlocks, but it's part of the contract
        using var completed = new ManualResetEventSlim(false);
        Exception? exception = null;

        _messagePump.PostToUIThread(() =>
        {
            try
            {
                d(state);
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            finally
            {
                completed.Set();
            }
        });

        completed.Wait();

        if (exception != null)
        {
            throw new InvalidOperationException("Exception occurred in Send operation", exception);
        }
    }

    /// <summary>
    /// Creates a copy of the synchronization context.
    /// </summary>
    public override SynchronizationContext CreateCopy()
    {
        return new Win32SynchronizationContext(_messagePump);
    }
}

/// <summary>
/// Defines a message filter interface that allows external code to participate
/// in the message loop processing, similar to WinForms' IMessageFilter.
/// </summary>
public interface IWin32MessageFilter
{
    /// <summary>
    /// Filters a message before it is dispatched.
    /// </summary>
    /// <param name="message">The message to filter.</param>
    /// <returns>true to filter the message and stop it from being dispatched; 
    /// false to allow the message to continue to the next filter or be dispatched.</returns>
    bool PreFilterMessage(ref Win32Message message);
}

/// <summary>
/// Represents a Windows message for use with IMessageFilter.
/// </summary>
public struct Win32Message
{
    public nint HWnd;
    public uint Msg;
    public nuint WParam;
    public nint LParam;

    internal static Win32Message FromMSG(in MSG msg) => new()
    {
        HWnd = msg.hwnd,
        Msg = msg.message,
        WParam = msg.wParam,
        LParam = msg.lParam
    };
}