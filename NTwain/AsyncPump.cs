//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Windows.Threading;

//namespace NTwain
//{
//    // from http://blogs.msdn.com/b/pfxteam/archive/2012/01/20/10259049.aspx

//    /// <summary>
//    /// Provides a pump that supports running asynchronous methods on the current thread.
//    /// </summary>
//    public static class AsyncPump
//    {
//        /// <summary>
//        /// Runs the specified asynchronous function.
//        /// </summary>
//        /// <param name="func">The asynchronous function to execute.</param>
//        /// <exception cref="System.ArgumentNullException">func</exception>
//        /// <exception cref="System.InvalidOperationException">No task provided.</exception>
//        public static void Run(Func<Task> func)
//        {
//            if (func == null) throw new ArgumentNullException("func");

//            var prevCtx = SynchronizationContext.Current;
//            try
//            {
//                var syncCtx = new DispatcherSynchronizationContext();
//                SynchronizationContext.SetSynchronizationContext(syncCtx);

//                var t = func();
//                if (t == null) throw new InvalidOperationException();

//                var frame = new DispatcherFrame();
//                t.ContinueWith(_ => { frame.Continue = false; },
//                    TaskScheduler.Default);
//                Dispatcher.PushFrame(frame);

//                t.GetAwaiter().GetResult();
//            }
//            finally
//            {
//                SynchronizationContext.SetSynchronizationContext(prevCtx);
//            }

//        }
//    }
//}
