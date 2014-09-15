using NTwain.Internals;
using System;
using System.Threading;
using System.Windows.Interop;

namespace NTwain
{
    /// <summary>
    /// An abstract class for TWAIN to hook into windows message loops.
    /// </summary>
    public abstract class MessageLoopHook
    {
        internal IntPtr Handle { get; set; }
        internal SynchronizationContext SyncContext { get; set; }

        internal abstract void Start(IWinMessageFilter filter);
        internal abstract void Stop();

        /// <summary>
        /// Asynchronously invokes the specified action on the message loop thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public virtual void BeginInvoke(Action action)
        {
            if (SyncContext == null)
            {
                action();
            }
            else
            {
                SyncContext.Post(o =>
                {
                    action();
                }, null);
            }
        }

        /// <summary>
        /// Synchronously invokes the specified action on the message loop thread.
        /// </summary>
        /// <param name="action">The action.</param>
        public virtual void Invoke(Action action)
        {
            if (SyncContext == null)
            {
                action();
            }
            else
            {
                SyncContext.Send(o =>
                {
                    action();
                }, null);
            }
        }

        internal void ThrowInvalidOp()
        {
            throw new InvalidOperationException(InvalidMessage);
        }

        internal virtual string InvalidMessage { get { return string.Empty; } }
    }

    /// <summary>
    /// A <see cref="MessageLoopHook"/> for use in winform applications.
    /// </summary>
    public sealed class WindowsFormsMessageLoopHook : MessageLoopHook, System.Windows.Forms.IMessageFilter
    {
        IWinMessageFilter _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsFormsMessageLoopHook"/> class.
        /// </summary>
        /// <param name="windowHandle">The handle to the app window.</param>
        /// <exception cref="System.ArgumentException">A valid window handle is required.</exception>
        public WindowsFormsMessageLoopHook(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero) { throw new ArgumentException("A valid window handle is required."); }

            if (!System.Windows.Forms.Application.MessageLoop)
            {
                ThrowInvalidOp();
            }
            var sync = SynchronizationContext.Current;
            if (sync == null)
            {
                ThrowInvalidOp();
            }
            Handle = windowHandle;
            SyncContext = sync;
        }
        internal override string InvalidMessage
        {
            get
            {
                return "This can only be created on the Windows Forms UI thread.";
            }
        }

        internal override void Start(IWinMessageFilter filter)
        {
            //Invoke(() =>
            //{
            _filter = filter;
            System.Windows.Forms.Application.AddMessageFilter(this);
            //});
        }

        internal override void Stop()
        {
            //Invoke(() =>
            //{
            System.Windows.Forms.Application.RemoveMessageFilter(this);
            _filter = null;
            //});
        }


        #region IMessageFilter Members

        bool System.Windows.Forms.IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (_filter != null)
            {
                return _filter.IsTwainMessage(m.HWnd, m.Msg, m.WParam, m.LParam);
            }
            return false;
        }

        #endregion
    }

    /// <summary>
    /// A <see cref="MessageLoopHook"/> for use in WPF applications.
    /// </summary>
    public sealed class WpfMessageLoopHook : MessageLoopHook
    {
        HwndSource _hooker;
        IWinMessageFilter _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfMessageLoopHook"/> class.
        /// </summary>
        /// <param name="windowHandle">The handle to the app window.</param>
        /// <exception cref="System.ArgumentException">A valid window handle is required.</exception>
        public WpfMessageLoopHook(IntPtr windowHandle)
        {
            if (windowHandle == IntPtr.Zero) { throw new ArgumentException("A valid window handle is required."); }

            if (System.Windows.Application.Current == null ||
                !System.Windows.Application.Current.Dispatcher.CheckAccess())
            {
                ThrowInvalidOp();
            }
            var sync = SynchronizationContext.Current;
            if (sync == null)
            {
                ThrowInvalidOp();
            }
            Handle = windowHandle;
            SyncContext = sync;
        }
        internal override string InvalidMessage
        {
            get
            {
                return "This can only be created on the WPF UI thread.";
            }
        }

        internal override void Start(IWinMessageFilter filter)
        {
            _filter = filter;
            _hooker = HwndSource.FromHwnd(Handle);
            _hooker.AddHook(FilterMessage);
        }

        private IntPtr FilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (_filter != null)
            {
                handled = _filter.IsTwainMessage(hwnd, msg, wParam, lParam);
            }
            return IntPtr.Zero;
        }

        internal override void Stop()
        {
            if (_hooker != null)
            {
                _hooker.RemoveHook(FilterMessage);
                // cannot really dispose _hook or the window will also dispose
                //_hooker.Dispose();
                _hooker = null;
            }
        }
    }
}
