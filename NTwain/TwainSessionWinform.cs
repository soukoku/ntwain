using NTwain.Data;
using System.Security.Permissions;
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

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage(ref Message m)
        {
            var winmsg = new MESSAGE(m.HWnd, m.Msg, m.WParam, m.LParam);

            return HandleWndProcMessage(ref winmsg);
        }

        #endregion
    }
}
