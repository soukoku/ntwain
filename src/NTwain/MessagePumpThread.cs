using Microsoft.Extensions.Logging;
using NTwain.Data;
using NTwain.Native;
using System;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace NTwain;

/// <summary>
/// For use under Windows to host a message pump.
/// </summary>
#if !NETFRAMEWORK
[SupportedOSPlatform("windows5.1.2600")]
#endif
class MessagePumpThread
{
    Win32MessagePump? _pump;
    TwainAppSession? _twain;

    public bool IsRunning => _pump != null && !_pump.MainWindow.IsNull;

    /// <summary>
    /// Starts the thread, attaches a twain session to it,
    /// and opens the DSM.
    /// </summary>
    /// <param name="twain"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<STS> AttachAsync(TwainAppSession twain)
    {
        if (_twain != null) return new STS { RC = TWRC.SUCCESS };

        Thread t = new(RunMessagePump);
        t.IsBackground = true;
        t.SetApartmentState(ApartmentState.STA);
        t.Start();

        while (_pump == null || _pump.MainWindow.IsNull)
        {
            await Task.Delay(50);
        }

        STS sts = default;
        TaskCompletionSource<bool> tcs = new();
        _pump.PostToUIThread(() =>
        {
            try
            {
                sts = twain.OpenDSM(_pump.MainWindow, SynchronizationContext.Current!);
                if (sts.IsSuccess)
                {
                    _pump.AddMessageFilter(twain);
                    _twain = twain;
                }
                else
                {
                    _pump.Quit();
                    _pump = null;
                }
            }
            finally
            {
                tcs.TrySetResult(true);
            }
        });
        await tcs.Task;
        return sts;
    }

    /// <summary>
    /// Detatches a previously attached session and stops the thread.
    /// </summary>
    public async Task<STS> DetachAsync()
    {
        STS sts = default;
        if (_pump != null && _twain != null)
        {
            TaskCompletionSource<STS> tcs = new();
            _pump.PostToUIThread(() =>
            {
                if (_twain == null) return;

                sts = _twain.CloseDSMReal();
                if (sts.IsSuccess)
                {
                    _pump.RemoveMessageFilter(_twain);
                    _pump.Quit();
                    _twain = null;
                }
                tcs.SetResult(sts);
            });
            await tcs.Task;
        }
        return sts;
    }

    //public void BringWindowToFront()
    //{
    //    if (_dummyForm != null)
    //    {
    //        _dummyForm.BeginInvoke(_dummyForm.BringToFront);
    //    }
    //}

    void RunMessagePump()
    {
        _twain?.Logger.LogDebug("Starting TWAIN message pump thread.");
        _pump = new Win32MessagePump();
        _pump.UnhandledException += Application_ThreadException;
        _pump.Run();
        _twain?.Logger.LogDebug("TWAIN message pump thread exiting.");
    }

    private void Application_ThreadException(object? sender, Win32MessagePumpExceptionEventArgs e)
    {
        _twain?.Logger.LogError(e.Exception, "Unhandled exception in TWAIN message pump thread from {Source}.", e.Source);
        e.Handled = true;
    }
}