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
            if (PlatformInfo.Current.IsApp64bit)
            {
                Console.WriteLine("[64bit]");
            }
            else
            {
                Console.WriteLine("[32bit]");
            }
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
            twain.TransferReady += (s, e) =>
            {
                Console.WriteLine("Got xfer ready on thread {0}.", Thread.CurrentThread.ManagedThreadId);
            };
            twain.DataTransferred += (s, e) =>
            {
                if (e.NativeData != IntPtr.Zero)
                {
                    Console.WriteLine("SUCCESS! Got twain data on thread {0}.", Thread.CurrentThread.ManagedThreadId);
                }
                else
                {
                    Console.WriteLine("BUMMER! No twain data on thread {0}.", Thread.CurrentThread.ManagedThreadId);
                }
            };

            twain.SourceDisabled += (s, e) =>
            {
                Console.WriteLine("Source disabled on thread {0}.", Thread.CurrentThread.ManagedThreadId);
                var rc = twain.CurrentSource.Close();
                rc = twain.Close();
            };
            return twain;
        }

        const string SAMPLE_SOURCE = "TWAIN2 FreeImage Software Scanner";
        static void DoTwainWork()
        {
            Console.WriteLine("Getting ready to do twain stuff on thread {0}...", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);

            var rc = twain.Open();

            if (rc == ReturnCode.Success)
            {
                var hit = twain.FirstOrDefault(s => string.Equals(s.Name, SAMPLE_SOURCE));
                if (hit == null)
                {
                    Console.WriteLine("The sample source \"" + SAMPLE_SOURCE + "\" is not installed.");
                    twain.Close();
                }
                else
                {
                    rc = hit.Open();

                    if (rc == ReturnCode.Success)
                    {
                        Console.WriteLine("Starting capture from the sample source...");
                        rc = hit.Enable(SourceEnableMode.NoUI, false, IntPtr.Zero);
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

    }
}
