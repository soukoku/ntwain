using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTwain
{
    /// <summary>
    /// A customized TWAIN session for use in winform environment.
    /// Use this by adding this as an <see cref="IMessageFilter "/> via <see cref="Application.AddMessageFilter"/>.
    /// </summary>
    public class TwainSessionWinform : TwainSession, IMessageFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSessionWinform" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSessionWinform(TWIdentity appId) : base(appId) { }

        #region IMessageFilter Members

        bool IMessageFilter.PreFilterMessage(ref Message m)
        {
            MESSAGE winmsg = default(MESSAGE);
            winmsg.hwnd = m.HWnd;
            winmsg.lParam = m.LParam;
            winmsg.message = (uint)m.Msg;
            winmsg.wParam = m.WParam;

            return HandleWndProcMessage(ref winmsg);
        }

        #endregion
    }
}
