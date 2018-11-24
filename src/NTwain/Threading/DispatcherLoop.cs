using NTwain.Data.Win32;
using NTwain.Internals;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace NTwain.Threading
{
    /// <summary>
    /// Provides an internal message pump on non-Windows using a background thread.
    /// </summary>
    class DispatcherLoop : IThreadContext
    {
        readonly TwainSession session;
        BlockingCollection<ActionItem> actionQueue;
        CancellationTokenSource stopToken;

        Thread loopThread;

        public DispatcherLoop(TwainSession session)
        {
            this.session = session;
        }

        public void Start()
        {
            if (loopThread != null) return;

            Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {nameof(WinMsgLoop)}.{nameof(Start)}()");

            actionQueue = new BlockingCollection<ActionItem>();
            stopToken = new CancellationTokenSource();

            // startWaiter ensures thread is running before this method returns
            using (var startWaiter = new ManualResetEventSlim())
            {
                loopThread = new Thread(new ThreadStart(() =>
                {
                    Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: starting msg pump...");

                    startWaiter.Set();

                    try
                    {
                        while (actionQueue.TryTake(out ActionItem work, -1, stopToken.Token))
                        {
                            work.DoAction();
                        }
                    }
                    catch (OperationCanceledException) { }
                    finally
                    {
                        // clear queue
                        actionQueue.Dispose();

                        loopThread = null;
                    }
                }));
                loopThread.IsBackground = true;
                loopThread.TrySetApartmentState(ApartmentState.STA);
                loopThread.Start();
                startWaiter.Wait();
            }
        }

        public void Stop()
        {
            if (!stopToken.IsCancellationRequested) stopToken.Cancel();
        }

        bool IsSameThread()
        {
            return loopThread == Thread.CurrentThread || loopThread == null;
        }


        public void Invoke(Action action)
        {
            if (IsSameThread())
            {
                // ok
                action();
            }
            else
            {
                // queue up work
                using (var waiter = new ManualResetEventSlim())
                {
                    var work = new ActionItem(waiter, action);
                    actionQueue.TryAdd(work);
                    waiter.Wait();
                    work.Error.TryRethrow();
                }
            }
        }

        public void BeginInvoke(Action action)
        {
            actionQueue.TryAdd(new ActionItem(action));
        }
    }
}
