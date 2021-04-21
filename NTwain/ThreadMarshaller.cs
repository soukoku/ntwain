using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NTwain
{
    /// <summary>
    /// Allows work to be marshalled to a different (usually UI) thread if necessary.
    /// </summary>
    public interface IThreadMarshaller
    {
        /// <summary>
        /// Starts work asynchronously and returns immediately.
        /// </summary>
        /// <param name="work"></param>
        void BeginInvoke(Delegate work, params object[] args);

        /// <summary>
        /// Starts work synchronously until it returns.
        /// </summary>
        /// <param name="work"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object Invoke(Delegate work, params object[] args);
    }

    /// <summary>
    /// Doesn't actually use any particular thread. 
    /// Should only be used in non-UI apps.
    /// </summary>
    public class NoParticularMarshaller : IThreadMarshaller
    {
        public bool InvokeRequired => throw new NotImplementedException();

        public void BeginInvoke(Delegate work, params object[] args)
        {
            Task.Run(() => work.DynamicInvoke(args));
        }

        public object Invoke(Delegate work, params object[] args)
        {
            return work.DynamicInvoke(args);
        }
    }


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

    /// <summary>
    /// Uses a WPF dispatcher to do the work.
    /// </summary>
    public class DispatcherMarshaller : IThreadMarshaller
    {
        private readonly System.Windows.Threading.Dispatcher _dispatcher;

        /// <summary>
        /// Uses a dispatcher whose UI thread is used to run the work.
        /// </summary>
        /// <param name="dispatcher"></param>
        public DispatcherMarshaller(System.Windows.Threading.Dispatcher dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public void BeginInvoke(Delegate work, params object[] args)
        {
            _dispatcher.BeginInvoke(work, args);
        }

        public object Invoke(Delegate work, params object[] args)
        {
            return _dispatcher.Invoke(work, args);
        }
    }
}
