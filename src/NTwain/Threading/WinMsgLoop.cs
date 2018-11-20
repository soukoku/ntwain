using NTwain.Data.Win32;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;

namespace NTwain.Threading
{
    /// <summary>
    /// Provides an internal message pump on Windows using a background thread.
    /// </summary>
    class WinMsgLoop
    {
        static ushort classAtom;
        static IntPtr hInstance;
        static readonly int dequeMsg = UnsafeNativeMethods.RegisterWindowMessage("WinMsgLoopQueue");

        const int CW_USEDEFAULT = -1;

        static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            Debug.WriteLine($"Dummy window got msg {(WindowMessage)msg}.");
            switch ((WindowMessage)msg)
            {
                case WindowMessage.WM_DESTROY:
                    UnsafeNativeMethods.PostQuitMessage(0);
                    return IntPtr.Zero;
            }
            return UnsafeNativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        static void InitGlobal()
        {
            if (classAtom == 0)
            {
                hInstance = UnsafeNativeMethods.GetModuleHandle(null);

                var wc = new WNDCLASSEX();
                wc.cbSize = Marshal.SizeOf(wc);
                wc.style = ClassStyles.CS_VREDRAW | ClassStyles.CS_HREDRAW;

                var procPtr = Marshal.GetFunctionPointerForDelegate(new WndProcDelegate(WndProc));
                wc.lpfnWndProc = procPtr;

                wc.cbClsExtra = 0;
                wc.cbWndExtra = 0;
                wc.hInstance = hInstance;
                wc.hIcon = IntPtr.Zero;
                wc.hCursor = IntPtr.Zero;
                wc.hbrBackground = IntPtr.Zero;
                wc.lpszMenuName = null;
                wc.lpszClassName = Guid.NewGuid().ToString("n");
                wc.hIconSm = IntPtr.Zero;

                classAtom = UnsafeNativeMethods.RegisterClassEx(ref wc);
                if (classAtom == 0)
                {
                    throw new Win32Exception();
                }
            }
        }



        readonly TwainSession session;
        readonly ConcurrentQueue<ActionItem> actionQueue;

        Thread loopThread;
        bool stop;
        IntPtr hWnd;

        public WinMsgLoop(TwainSession session)
        {
            this.session = session;
            actionQueue = new ConcurrentQueue<ActionItem>();
        }

        public void Start()
        {
            if (stop || loopThread != null || hWnd != IntPtr.Zero) return;

            Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {nameof(WinMsgLoop)}.{nameof(Start)}()");

            InitGlobal();

            Win32Exception startErr = null;

            // startWaiter ensures thread is running before this method returns
            using (var startWaiter = new ManualResetEventSlim())
            {
                stop = false;
                loopThread = new Thread(new ThreadStart(() =>
                {
                    Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: starting msg pump...");

                    hWnd = UnsafeNativeMethods.CreateWindowEx(WindowStylesEx.WS_EX_LEFT, classAtom, Guid.NewGuid().ToString(),
                        WindowStyles.WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT, CW_USEDEFAULT,
                        IntPtr.Zero, IntPtr.Zero, hInstance, IntPtr.Zero);
                    if (hWnd == IntPtr.Zero)
                    {
                        startErr = new Win32Exception();

                        // clear queue
                        while (actionQueue.TryDequeue(out ActionItem dummy)) { }

                        loopThread = null;
                        stop = false;

                        startWaiter.Set();
                    }
                    else
                    {
                        startWaiter.Set();

                        MSG msg = default;
                        while (!stop && UnsafeNativeMethods.GetMessage(ref msg, IntPtr.Zero, 0, 0))
                        {
                            UnsafeNativeMethods.TranslateMessage(ref msg);
                            if (!session.HandleWindowsMessage(ref msg))
                            {
                                if (msg.message == dequeMsg)
                                {
                                    if (actionQueue.TryDequeue(out ActionItem work))
                                    {
                                        work.DoAction();
                                        if (stop) break;
                                    }
                                }
                                else
                                {
                                    UnsafeNativeMethods.DispatchMessage(ref msg);
                                }
                            }
                        }
                        // clear queue
                        while (actionQueue.TryDequeue(out ActionItem dummy)) { }

                        loopThread = null;
                        hWnd = IntPtr.Zero;
                        stop = false;
                    }
                }));
                loopThread.IsBackground = true;
                loopThread.SetApartmentState(ApartmentState.STA);
                loopThread.Start();
                startWaiter.Wait();

                if (startErr != null) throw startErr;
            }
        }

        public void Stop()
        {
            if (stop) return;
            stop = true;
            if (hWnd != IntPtr.Zero)
            {
                Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: calling destroy window...");
                BeginInvoke(() =>
                {
                    if (UnsafeNativeMethods.DestroyWindow(hWnd))
                    {
                        hWnd = IntPtr.Zero;
                    }
                    else
                    {
                        Debug.WriteLine("Failed to destroy window: " + new Win32Exception().Message);
                    }
                });
            }
        }

        bool IsSameThread()
        {
            return loopThread == Thread.CurrentThread || loopThread == null;
        }

        /// <summary>
        /// Runs the action synchronously on the internal message pump thread.
        /// </summary>
        /// <param name="action"></param>
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
                    actionQueue.Enqueue(new ActionItem(waiter, action));
                    UnsafeNativeMethods.PostMessage(hWnd, dequeMsg, IntPtr.Zero, IntPtr.Zero);
                    waiter.Wait();
                }
            }
        }

        /// <summary>
        /// Runs the action asynchronously on the internal message pump thread.
        /// </summary>
        /// <param name="action"></param>
        public void BeginInvoke(Action action)
        {
            if (hWnd == IntPtr.Zero) action();
            else
            {
                actionQueue.Enqueue(new ActionItem(action));
                UnsafeNativeMethods.PostMessage(hWnd, dequeMsg, IntPtr.Zero, IntPtr.Zero);
            }
        }


        [SuppressUnmanagedCodeSecurity]
        internal static class UnsafeNativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, ThrowOnUnmappableChar = true, BestFitMapping = false)]
            public static extern IntPtr GetModuleHandle(string modName);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetMessage([In, Out] ref MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool TranslateMessage([In, Out] ref MSG lpMsg);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr DispatchMessage([In] ref MSG lpmsg);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern int PostMessage(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

            [DllImport("user32.dll")]
            public static extern void PostQuitMessage(int nExitCode);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, BestFitMapping = false)]
            public static extern int RegisterWindowMessage(string msg);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern ushort RegisterClassEx([In] ref WNDCLASSEX lpwcx);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
            public static extern IntPtr CreateWindowEx(WindowStylesEx dwExStyle,
                ushort lpszClassName,
                [MarshalAs(UnmanagedType.LPStr)] string lpszWindowName,
                WindowStyles style,
                int x, int y, int width, int height,
                IntPtr hWndParent, IntPtr hMenu, IntPtr hInst, IntPtr lpParam);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool DestroyWindow(IntPtr hWnd);
        }
    }
}
