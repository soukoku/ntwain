using System;
using System.Threading;

namespace NTwain.Internals
{

    // just a test

    class WrappedManualResetEvent : IDisposable
    {
#if NET4
        ManualResetEventSlim _slim;
#else
        ManualResetEvent _mre;
#endif

        public WrappedManualResetEvent()
        {
#if NET4
            _slim = new ManualResetEventSlim();
#else
            _mre = new ManualResetEvent(false);
#endif
        }

        public void Wait()
        {
#if NET4
            _slim.Wait();
#else
            _mre.WaitOne();
#endif
        }

        public void Set()
        {
#if NET4
            _slim.Set();
#else
            _mre.Set();
#endif
        }

        #region IDisposable Members

        public void Dispose()
        {
#if NET4
            _slim.Dispose();
#else
            _mre.Close();
#endif
        }

        #endregion
    }
}
