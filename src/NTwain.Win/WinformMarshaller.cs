using System;

namespace NTwain
{
    /// <summary>
    /// Uses a winform UI thread to do the work.
    /// </summary>
    public class WinformMarshaller : IThreadMarshaller
    {
        private readonly System.Windows.Forms.Control _uiControl;

        /// <summary>
        /// Uses a control whose UI thread is used to run the work.
        /// </summary>
        /// <param name="uiControl"></param>
        public WinformMarshaller(System.Windows.Forms.Control uiControl)
        {
            _uiControl = uiControl ?? throw new ArgumentNullException(nameof(uiControl));
        }

        public void BeginInvoke(Delegate work, params object[] args)
        {
            _uiControl.BeginInvoke(work, args);
        }

        public object Invoke(Delegate work, params object[] args)
        {
            return _uiControl.Invoke(work, args);
        }
    }
}
