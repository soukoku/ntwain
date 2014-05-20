using NTwain;
using NTwain.Data;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // just an amusing example to do twain in console without UI
            ThreadPool.QueueUserWorkItem(o =>
            {
                DoTwainWork();
            });
            Console.WriteLine("Test started, press Enter to exit.");
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


        static void DoTwainWork()
        {
            Console.WriteLine("Getting ready to do twain stuff on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);

            var rc = twain.Open();

            if (rc == ReturnCode.Success)
            {
                var hit = twain.GetSources().Where(s => string.Equals(s.Name, "TWAIN2 FreeImage Software Scanner")).FirstOrDefault();
                if (hit == null)
                {
                    Console.WriteLine("The sample source \"TWAIN2 FreeImage Software Scanner\" is not installed.");
                    twain.Close();
                }
                else
                {
                    rc = hit.Open();

                    if (rc == ReturnCode.Success)
                    {
                        Console.WriteLine("Start capture from the sample source.");
                        rc = hit.StartTransfer(SourceEnableMode.NoUI, false, IntPtr.Zero);
                    }
                    else
                    {
                        twain.Close();
                    }
                }
            }
            else
            {
                Console.WriteLine("Failed to open dsm with rc={0}!", rc);
            }
        }

        static void twain_SourceDisabled(object sender, EventArgs e)
        {
            Console.WriteLine("Source disabled on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            var rc = twain.Source.Close();
            rc = twain.Close();
        }

        static void twain_TransferReady(object sender, TransferReadyEventArgs e)
        {
            Console.WriteLine("Got xfer ready on thread {0}.", Thread.CurrentThread.ManagedThreadId);
        }

        static void twain_DataTransferred(object sender, DataTransferredEventArgs e)
        {
            if (e.NativeData != IntPtr.Zero)
            {
                Console.WriteLine("Got twain data on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            }
            else
            {
                Console.WriteLine("No twain data on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}
