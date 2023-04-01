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
        /// <param name="args"></param>
        void BeginInvoke(Delegate work, params object[] args);

        /// <summary>
        /// Starts work synchronously until it returns.
        /// </summary>
        /// <param name="work"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        object Invoke(Delegate work, params object[] args);
    }

    ///// <summary>
    ///// Async calls are marshalled to threadpool thread. 
    ///// Should only be used in non-UI apps.
    ///// </summary>
    //public class ThreadPoolMarshaller : IThreadMarshaller
    //{
    //    public bool InvokeRequired => throw new NotImplementedException();

    //    public void BeginInvoke(Delegate work, params object[] args)
    //    {
    //        Task.Run(() => work.DynamicInvoke(args));
    //    }

    //    public object Invoke(Delegate work, params object[] args)
    //    {
    //        return work.DynamicInvoke(args);
    //    }
    //}
}
