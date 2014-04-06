using NTwain;
using NTwain.Data;
using NTwain.Values;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // just an amusing example to do twain in console without UI, but may not work in real life

            Console.WriteLine("Running Main on thread {0}", Thread.CurrentThread.ManagedThreadId);
            new Thread(new ParameterizedThreadStart(DoTwainWork)).Start(Dispatcher.CurrentDispatcher);

            // basically just needs a msg loop to act as the UI thread
            Dispatcher.Run();
            Console.WriteLine("Test completed.");
            Console.ReadLine();
        }



        static readonly TwainSession twain = InitTwain();
        private static TwainSession InitTwain()
        {
            var twain = new TwainSession(TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetExecutingAssembly()));
            twain.DataTransferred += twain_DataTransferred;
            twain.TransferReady += twain_TransferReady;
            twain.SourceDisabled += twain_SourceDisabled;
            return twain;
        }


        static void DoTwainWork(object obj)
        {
            Console.WriteLine("Getting ready to do twain stuff on thread {0}", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);

            var mySyncer = SynchronizationContext.Current;

            if (mySyncer == null)
            {
                mySyncer = new DispatcherSynchronizationContext(obj as Dispatcher);
            }

            var rc = twain.OpenManager(default(HandleRef));

            if (rc == ReturnCode.Success)
            {
                rc = twain.OpenSource("TWAIN2 FreeImage Software Scanner");

                if (rc == ReturnCode.Success)
                {
                    // enablesource must be on the thread the sync context works on
                    mySyncer.Post(blah =>
                    {
                        rc = twain.EnableSource(SourceEnableMode.NoUI, false, default(HandleRef), blah as SynchronizationContext);
                    }, mySyncer);
                    return;
                }
                else
                {
                    twain.CloseManager();
                }
            }

            Dispatcher.ExitAllFrames();
        }

        static void twain_SourceDisabled(object sender, EventArgs e)
        {
            Console.WriteLine("Source disabled on thread {0}", Thread.CurrentThread.ManagedThreadId);
            var rc = twain.CloseSource();
            rc = twain.CloseManager();

            Dispatcher.ExitAllFrames();
        }

        static void twain_TransferReady(object sender, TransferReadyEventArgs e)
        {
            Console.WriteLine("Got xfer ready on thread {0}", Thread.CurrentThread.ManagedThreadId);
        }

        static void twain_DataTransferred(object sender, DataTransferredEventArgs e)
        {
            if (e.Data != IntPtr.Zero)
            {
                Console.WriteLine("Got twain data on thread {0}", Thread.CurrentThread.ManagedThreadId);
            }
            else
            {
                Console.WriteLine("No twain data on thread {0}", Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
