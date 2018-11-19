//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;

//namespace NTwain.Threading
//{
//    /// <summary>
//    /// A base thread-marshalling class that does no marshalling.
//    /// </summary>
//    public class ThreadDispatcher
//    {
//        /// <summary>
//        /// Invokes an action and returns after it's done.
//        /// </summary>
//        /// <param name="action"></param>
//        public virtual void Invoke(Action action)
//        {
//            action(); SynchronizationContext.Current
//        }
//    }
//}
