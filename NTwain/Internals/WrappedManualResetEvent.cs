using System;
using System.Threading;

namespace NTwain.Internals
{

    // just a test

    class WrappedManualResetEvent : IDisposable
    {
#if NET35
        ManualResetEvent _mre;
#else
        ManualResetEventSlim _slim;
#endif

        public WrappedManualResetEvent()
        {
#if NET35
            _mre = new ManualResetEvent(false);
#else
            _slim = new ManualResetEventSlim();
#endif
        }

        public void Wait()
        {
#if NET35
            _mre.WaitOne();
#else
            _slim.Wait();
#endif
        }

        public void Set()
        {
#if NET35
            _mre.Set();
#else
            _slim.Set();
#endif
        }

        #region IDisposable Members

        public void Dispose()
        {
#if NET35
            _mre.Close();
#else
            _slim.Dispose();
#endif
        }

        #endregion
    }
}
