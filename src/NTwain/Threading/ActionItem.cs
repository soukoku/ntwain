using System;
using System.Threading;

namespace NTwain.Threading
{
    internal class ActionItem
    {
        private ManualResetEventSlim waiter;
        private Action action;

        public Exception Error { get; private set; }

        public ActionItem(Action action)
        {
            this.action = action;
        }

        public ActionItem(ManualResetEventSlim waiter, Action action)
        {
            this.waiter = waiter;
            this.action = action;
        }

        public void DoAction()
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Error = ex;
            }
            finally
            {
                waiter?.Set();
            }
        }
    }
}