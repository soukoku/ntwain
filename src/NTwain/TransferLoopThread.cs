using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace NTwain;

/// <summary>
/// Contains logic for handling the transfer loop of data from a TWAIN source
/// in another thread.
/// </summary>
partial class TransferLoopThread : IDisposable
{
    readonly Thread _transferThread;
    readonly CancellationTokenSource _transferThreadEnder = new();
    readonly ManualResetEventSlim _transferReady = new(false);
    readonly TwainAppSession _twain;

    public TransferLoopThread(TwainAppSession twain)
    {
        _twain = twain;
        _transferThread = new(ThreadWork)
        {
            IsBackground = true
        };
        if (OperatingSystem.IsWindowsVersionAtLeast(6, 0, 6000))
        {
            // is this necessary? who knows
            _transferThread.SetApartmentState(ApartmentState.STA);
        }
        _transferThread.Start();
    }

    public bool CloseDsRequested { get; internal set; }
    public bool IsTransferring { get; private set; }

    public void NotifyTransferReady(bool ready)
    {
        if (ready)
        {
            _transferReady.Set();
        }
        else
        {
            _transferReady.Reset();
        }
    }

    private void ThreadWork()
    {
        if (_twain.Logger.IsEnabled(LogLevel.Trace))
            _twain.Logger.LogTrace("[thread {ThreadId}] Transfer thread started.", Environment.CurrentManagedThreadId);
        try
        {
            while (!_transferThreadEnder.Token.IsCancellationRequested)
            {
                _transferReady.Wait(_transferThreadEnder.Token);
                if (_twain.Logger.IsEnabled(LogLevel.Trace))
                    _twain.Logger.LogTrace("[thread {ThreadId}] Transfer thread woke up to do transfer.", Environment.CurrentManagedThreadId);

                IsTransferring = true;
                try
                {
                    EnterTransferLoop();
                }
                catch (Exception ex)
                {
                    _twain.Logger.LogError(ex, "Exception occurred during transfer loop.");
                }
                finally
                {
                    _transferReady.Reset();
                    _twain.DisableSource();

                    IsTransferring = false;
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Clean shutdown
        }
        if (_twain.Logger.IsEnabled(LogLevel.Trace))
            _twain.Logger.LogTrace("[thread {ThreadId}] Transfer thread ending.", Environment.CurrentManagedThreadId);
    }

    bool _disposed = false;
    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        _transferThreadEnder.Cancel();
        _transferThread.Join(TimeSpan.FromSeconds(5));

        _transferReady.Dispose();
        _transferThreadEnder.Dispose();
    }
}
