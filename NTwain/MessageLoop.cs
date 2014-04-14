using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        // mostly wraps around a dispatcher?
        static MessageLoop _instance = new MessageLoop();
        public static MessageLoop Instance { get { return _instance; } }

        Dispatcher _dispatcher;
        bool _started;
        HwndSource _dummyWindow;

        private MessageLoop() { }
        public void EnsureStarted()
        {
            if (!_started)
            {
                var loopThread = new Thread(new ThreadStart(() =>
                {
                    _dispatcher = Dispatcher.CurrentDispatcher;
                    if (Dsm.IsWin)
                    {
                        // start a windows msg loop for old twain to post msgs
                        // the style values are purely guesses here with
                        // CS_NOCLOSE, WS_DISABLED, and WS_EX_NOACTIVATE
                        _dummyWindow = new HwndSource(0x0200, 0x8000000, 0x8000000, 0, 0, "NTWAIN_LOOPER", IntPtr.Zero);
                    }
                    Dispatcher.Run();
                }));
                loopThread.IsBackground = true;
                loopThread.SetApartmentState(ApartmentState.STA);
                loopThread.Start();
                _started = true;
            }
        }

        public IntPtr LoopHandle
        {
            get
            {
                return _dummyWindow == null ? IntPtr.Zero : _dummyWindow.Handle;
            }
        }

        public void BeginInvoke(Action action)
        {
            if (_dispatcher != null)
            {
                _dispatcher.BeginInvoke(DispatcherPriority.Normal, action);
            }
        }

        public void Invoke(Action action)
        {
            if (_dispatcher != null)
            {
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
                        action();
                        man.Set();
                    }));
                    man.WaitOne();
                    man.Close();
                }
            }
        }

        public void AddHook(HwndSourceHook hook)
        {
            if (_dummyWindow != null) { _dummyWindow.AddHook(hook); }
        }
        public void RemoveHook(HwndSourceHook hook)
        {
            if (_dummyWindow != null) { _dummyWindow.RemoveHook(hook); }
        }
    }
}
