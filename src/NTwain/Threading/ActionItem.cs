using System;
using System.Threading;

namespace NTwain.Threading
{
    internal class ActionItem
    {
        private ManualResetEventSlim waiter;
        private Action action;

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
            finally
            {
                waiter?.Set();
            }
        }
    }
}