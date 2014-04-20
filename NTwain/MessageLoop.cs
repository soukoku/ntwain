using NTwain.Properties;
using NTwain.Triplets;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Interop;
using System.Windows.Threading;

namespace NTwain
{
    /// <summary>
    /// Provides a message loop for old TWAIN to post or new TWAIN to synchronize callbacks.
    /// </summary>
    class MessageLoop
    {
        static MessageLoop _instance = new MessageLoop();
        public static MessageLoop Instance { get { return _instance; } }

        Dispatcher _dispatcher;
        WindowsHook _hook;
        private MessageLoop() { }

        public void EnsureStarted(WindowsHook.WndProcHook hook)
        {
            if (_dispatcher == null)
            {
                // using this terrible hack so the new thread will start running before this function returns
                using (var hack = new WrappedManualResetEvent())
                {
                    var loopThread = new Thread(new ThreadStart(() =>
                    {
                        Debug.WriteLine("NTwain message loop is starting.");
                        _dispatcher = Dispatcher.CurrentDispatcher;
                        if (!Dsm.IsOnMono)
                        {
                            _hook = new WindowsHook(hook);
                        }
                        hack.Set();
                        Dispatcher.Run();
                        // if for whatever reason it ever gets here make everything uninitialized
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
            else if (Dsm.IsOnMono)
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

    /// <summary>
    /// Abstracts out wnd proc hook on Windows from MessageLoop class.
    /// This allows things to not depend on PresentationCore.dll at runtime on mono.
    /// Not that everything works yet in mono but it's something.
    /// </summary>
    class WindowsHook : IDisposable
    {
        IDisposable _win;
        WndProcHook _hook;

        public WindowsHook(WndProcHook hook)
        {
            // hook into windows msg loop for old twain to post msgs.
            // the style values are purely guesses here with
            // CS_NOCLOSE, WS_DISABLED, and WS_EX_NOACTIVATE
            HwndSource win = null;
            try
            {
                win = new HwndSource(0x0200, 0x8000000, 0x8000000, 0, 0, "NTWAIN_LOOPER", IntPtr.Zero);
                Handle = win.Handle;
                win.AddHook(WndProc);
                _win = win;
                _hook = hook;
            }
            catch
            {
                if (win != null) { win.Dispose(); }
            }
        }

        public delegate void WndProcHook(ref MESSAGE winMsg, ref bool handled);


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (_hook != null)
            {
                var winmsg = new MESSAGE(hwnd, msg, wParam, lParam);
                _hook(ref winmsg, ref handled);
            }
            return IntPtr.Zero;
        }

        public IntPtr Handle { get; private set; }


        /// <summary>
        /// The MSG structure in Windows for TWAIN use.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct MESSAGE
        {
            public MESSAGE(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam)
            {
                _hwnd = hwnd;
                _message = (uint)message;
                _wParam = wParam;
                _lParam = lParam;
                _time = 0;
                _x = 0;
                _y = 0;
            }

            IntPtr _hwnd;
            uint _message;
            IntPtr _wParam;
            IntPtr _lParam;
            uint _time;
            int _x;
            int _y;
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_win != null)
            {
                ((HwndSource)_win).RemoveHook(WndProc);
                _win.Dispose();
                _win = null;
            }
        }

        #endregion
    }

    // just a test

    class WrappedManualResetEvent : IDisposable
    {
#if NET4
        ManualResetEventSlim _slim;
#else
        ManualResetEvent _mre;
#endif

        public WrappedManualResetEvent()
        {
#if NET4
            _slim = new ManualResetEventSlim();
#else
            _mre = new ManualResetEvent(false);
#endif
        }

        public void Wait()
        {
#if NET4
            _slim.Wait();
#else
            _mre.WaitOne();
#endif
        }

        public void Set()
        {
#if NET4
            _slim.Set();
#else
            _mre.Set();
#endif
        }

        #region IDisposable Members

        public void Dispose()
        {
#if NET4
            _slim.Dispose();
#else
            _mre.Close();
#endif
        }

        #endregion
    }
}
