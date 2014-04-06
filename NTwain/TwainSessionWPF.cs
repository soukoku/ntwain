using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace NTwain
{
    /// <summary>
    /// A customized TWAIN session for use in WPF environment.
    /// Use this by using <see cref="PreFilterMessage"/> method as the target of <see cref="HwndSource.AddHook"/> delegate.
    /// </summary>
    public class TwainSessionWPF : TwainSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSessionWPF" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSessionWPF(TWIdentity appId) : base(appId) { }

        /// <summary>
        /// Message loop processor for WPF.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message ID.</param>
        /// <param name="wParam">The message's wParam value.</param>
        /// <param name="lParam">The message's lParam value.</param>
        /// <param name="handled">A value that indicates whether the message was handled. Set the value to true if the message was handled; otherwise, false.</param>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand)]
        public IntPtr PreFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MESSAGE winmsg = default(MESSAGE);
            winmsg.hwnd = hwnd;
            winmsg.lParam = lParam;
            winmsg.message = (uint)msg;
            winmsg.wParam = wParam;

            handled = base.HandleWndProcMessage(ref winmsg);

            return IntPtr.Zero;
        }
    }
}
