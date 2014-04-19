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
        bool _started;
        WindowsHook _hook;
        private MessageLoop() { }

        public void EnsureStarted(WindowsHook.WndProcHook hook)
        {
            if (!_started)
            {
                // using this terrible hack so the new thread will start running before this function returns
                var hack = new ManualResetEvent(false);

                var loopThread = new Thread(new ThreadStart(() =>
                {
                    Debug.WriteLine("NTwain message loop started.");
                    _dispatcher = Dispatcher.CurrentDispatcher;
                    if (!Dsm.IsOnMono)
                    {
                        _hook = new WindowsHook(hook);
                    }
                    hack.Set();
                    Dispatcher.Run();
                    _started = false;
                }));
                loopThread.IsBackground = true;
                loopThread.SetApartmentState(ApartmentState.STA);
                loopThread.Start();
                hack.WaitOne();
                hack.Close();
                _started = true;
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
            else
            {
                //_dispatcher.Invoke(DispatcherPriority.Normal, action);
                var man = new ManualResetEvent(false);
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
                man.WaitOne();
                man.Close();
            }
        }
    }

    /// <summary>
    /// Abstracts out wnd proc hook on Windows from MessageLoop class.
    /// This allows things to not depend on PresentationCore.dll at runtime on mono.
    /// Not that everything works yet in mono but it's something.
    /// </summary>
    class WindowsHook
    {
        public WindowsHook(WndProcHook hook)
        {
            // hook into windows msg loop for old twain to post msgs.
            // the style values are purely guesses here with
            // CS_NOCLOSE, WS_DISABLED, and WS_EX_NOACTIVATE
            var win = new HwndSource(0x0200, 0x8000000, 0x8000000, 0, 0, "NTWAIN_LOOPER", IntPtr.Zero);
            Handle = win.Handle;
            _hook = hook;
            win.AddHook(WndProc);
        }

        public delegate void WndProcHook(ref MESSAGE winMsg, ref bool handled);

        WndProcHook _hook;

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
    }
}
