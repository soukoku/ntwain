#if WINDOWS || NETFRAMEWORK
using Microsoft.Extensions.Logging;
using NTwain.Data;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTwain
{
    /// <summary>
    /// For use under Windows to host a message pump.
    /// </summary>
    class MessagePumpThread
    {
        KeepAliveForm? _dummyForm;
        TwainAppSession? _twain;

        public bool IsRunning => _dummyForm != null && _dummyForm.IsHandleCreated;

        /// <summary>
        /// Starts the thread, attaches a twain session to it,
        /// and opens the DSM.
        /// </summary>
        /// <param name="twain"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<STS> AttachAsync(TwainAppSession twain)
        {
            if (twain.State > STATE.S2) throw new InvalidOperationException("Cannot attach to an opened TWAIN session.");
            if (_twain != null || _dummyForm != null) throw new InvalidOperationException("Already attached previously.");

            Thread t = new(RunMessagePump);
            t.IsBackground = true;
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            while (_dummyForm == null || !_dummyForm.IsHandleCreated)
            {
                await Task.Delay(50);
            }

            STS sts = default;
            TaskCompletionSource<bool> tcs = new();
            _dummyForm.BeginInvoke(() =>
            {
                try
                {
                    sts = twain.OpenDSM(_dummyForm.Handle, SynchronizationContext.Current!);
                    if (sts.IsSuccess)
                    {
                        twain.AddWinformFilter();
                        _twain = twain;
                    }
                    else
                    {
                        _dummyForm.Close(true);
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
            if (_dummyForm != null && _twain != null)
            {
                TaskCompletionSource<STS> tcs = new();
                _dummyForm.BeginInvoke(() =>
                {
                    sts = _twain.CloseDSMReal();
                    if (sts.IsSuccess)
                    {
                        _twain.RemoveWinformFilter();
                        _dummyForm.Close(true);
                        _twain = null;
                    }
                    tcs.SetResult(sts);
                });
                await tcs.Task;
            }
            return sts;
        }

        public void BringWindowToFront()
        {
            if (_dummyForm != null)
            {
                _dummyForm.BeginInvoke(_dummyForm.BringToFront);
            }
        }

        void RunMessagePump()
        {
            _twain?.Logger.LogDebug("Starting TWAIN message pump thread.");
            Application.ThreadException += Application_ThreadException;
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            _dummyForm = new KeepAliveForm();
            _dummyForm.FormClosed += (s, e) =>
            {
                _dummyForm = null;
            };
            Application.Run(_dummyForm);
            _twain?.Logger.LogDebug("TWAIN message pump thread exiting.");
        }

        private void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            _twain?.Logger.LogError(e.Exception, "Unhandled exception in TWAIN message pump thread.");
        }

        class KeepAliveForm : Form
        {
            public KeepAliveForm()
            {
                ShowInTaskbar = false;
            }

            [DllImport("user32.dll")]
            static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                SetParent(this.Handle, new IntPtr(-3)); // HWND_MESSAGE
            }

            //protected override CreateParams CreateParams
            //{
            //    get
            //    {
            //        CreateParams cp = base.CreateParams;
            //        cp.ExStyle |= 0x80; // WS_EX_TOOLWINDOW
            //        return cp;
            //    }
            //}

            //protected override void OnShown(EventArgs e)
            //{
            //    Hide();
            //    base.OnShown(e);
            //}

            bool _closeForReal = false;
            internal void Close(bool forReal)
            {
                _closeForReal = forReal;
                Close();
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                if (e.CloseReason == CloseReason.UserClosing && !_closeForReal)
                {
                    e.Cancel = true;
                    Hide();
                }
                base.OnFormClosing(e);
            }
        }
    }
}
#endif