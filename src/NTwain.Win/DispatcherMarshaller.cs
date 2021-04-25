using System;

namespace NTwain
{
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
