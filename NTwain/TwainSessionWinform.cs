using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;

namespace NTwain
{
    /// <summary>
    /// A customized TWAIN session for use in winform environment.
    /// Use this by adding this as an <see cref="IMessageFilter "/> via <see cref="Application.AddMessageFilter"/>.
    /// </summary>
    public class TwainSessionWinform : TwainSessionBase, IMessageFilter
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
            MSG winmsg = default(MSG);
            winmsg.hwnd = m.HWnd;
            winmsg.lParam = m.LParam;
            winmsg.message = m.Msg;
            winmsg.wParam = m.WParam;

            return HandleWndProcMessage(ref winmsg);
        }

        #endregion
    }
}
