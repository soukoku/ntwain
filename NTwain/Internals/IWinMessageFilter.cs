using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain.Internals
{
    /// <summary>
    /// Interface for checking whether messages from WndProc is a TWAIN message and is handled
    /// internally.
    /// </summary>
    interface IWinMessageFilter
    {
        /// <summary>
        /// Checks and handle the message if it's a TWAIN message.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>true if handled internally.</returns>
        bool IsTwainMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);
    }


}
