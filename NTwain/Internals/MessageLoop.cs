using NTwain.Properties;
using NTwain.Triplets;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;

namespace NTwain.Internals
{
    /// <summary>
    /// Provides a message loop for old TWAIN to post or new TWAIN to synchronize callbacks.
    /// </summary>
    class MessageLoop
    {
        Dispatcher _dispatcher;
        WindowsHook _hook;

        public void Stop()
        {
            if (_dispatcher != null)
            {
                _dispatcher.InvokeShutdown();
            }
        }
        public void Start(IWinMessageFilter filter)
        {
            if (_dispatcher == null)
            {
                // using this hack so the new thread will start running before this function returns
                using (var hack = new WrappedManualResetEvent())
                {
                    var loopThread = new Thread(new ThreadStart(() =>
                    {
                        Debug.WriteLine("NTwain message loop is starting.");
                        _dispatcher = Dispatcher.CurrentDispatcher;
                        if (!Platform.IsOnMono)
                        {
                            _hook = new WindowsHook(filter);
                        }
                        hack.Set();
                        Dispatcher.Run();
                        // if dispatcher shutsdown we'll get here so make everything uninitialized
                        _dispatcher = null;
                        if (_hook != null)
                        {
                            _hook.Dispose();
                            _hook = null;
                        }
                    }));
                    loopThread.IsBackground = true;
                    loopThread.SetApartmentState(ApartmentState.STA);
                    loopThread.Start();
                    hack.Wait();
                }
            }
        }

        public IntPtr LoopHandle
        {
            get
            {
                return _hook == null ? IntPtr.Zero : _hook.Handle;
            }
        }

        public void BeginInvoke(Action action)
        {
            if (_dispatcher == null) { throw new InvalidOperationException(Resources.MsgLoopUnavailble); }

            _dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
        }

        public void Invoke(Action action)
        {
            if (_dispatcher == null) { throw new InvalidOperationException(Resources.MsgLoopUnavailble); }

            if (_dispatcher.CheckAccess())
            {
                action();
            }
            else if (Platform.IsOnMono)
            {
                using (var man = new WrappedManualResetEvent())
                {
                    _dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        try
                        {
                            action();
                        }
                        finally
                        {
                            man.Set();
                        }
                    }));
                    man.Wait();
                }
            }
            else
            {
                _dispatcher.Invoke(DispatcherPriority.Normal, action);
            }
        }
    }
}
