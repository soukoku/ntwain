using NTwain;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using TWAINWorkingGroup;
using static TWAINWorkingGroup.TWAIN;

namespace Net5Console
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Console.WriteLine("Starting twain test in console...");
            Console.WriteLine();

            using (var session = new TwainSession(Assembly.GetExecutingAssembly(), null, IntPtr.Zero))
            using (var hold = new ManualResetEventSlim())
            {
                session.DeviceEvent += (sender, e) =>
                {
                    Console.WriteLine($"Got device event " + e.Event);
                    Console.WriteLine();
                };
                session.TransferError += (sender, e) =>
                {
                    Console.WriteLine($"Transfer error {e.Code} {e.Exception}.");
                };
                session.TransferReady += (sender, e) =>
                {
                    Console.WriteLine($"Transfer ready, count={e.PendingCount}.");
                };
                session.SourceDisabled += (sender, e) =>
                {
                    Console.WriteLine("Disabled device.");
                    Console.WriteLine();
                    hold.Set();
                };

                if (session.Open() == STS.SUCCESS)
                {
                    Console.WriteLine("Opened DSM");
                    Console.WriteLine();

                    Console.WriteLine("Default device:");
                    Console.WriteLine($"\t{session.DefaultDataSource}");
                    Console.WriteLine();

                    Console.WriteLine("All devices:");
                    TW_IDENTITY? dsToUse = null;
                    foreach (var dev in session.GetDataSources())
                    {
                        Console.WriteLine($"\t{dev}");
                        if (dev.ProductName == "TWAIN2 Software Scanner")
                        {
                            dsToUse = dev;
                        }
                    }
                    Console.WriteLine();

                    if (!dsToUse.HasValue)
                    {
                        Console.WriteLine("Sample scanner not found.");
                    }
                    else
                    {
                        session.CurrentDataSource = dsToUse.Value;
                        if (session.CurrentDataSource.HasValue)
                        {
                            Console.WriteLine("Current device after opening attempt:");
                            Console.WriteLine($"\t{session.CurrentDataSource}");
                            Console.WriteLine();

                            var caps = session.Capabilities;
                            Console.WriteLine("All device caps:");
                            foreach (var cap in caps.CAP_SUPPORTEDCAPS.GetValues())
                            {
                                WriteCapInfo(caps, cap);
                            }
                            Console.WriteLine();

                            short count = 3;
                            var sts = caps.CAP_XFERCOUNT.SetOrConstraint(MSG.SET, count);
                            if (sts == STS.SUCCESS)
                            {
                                Console.WriteLine($"Successfully set xfercount to {count}.");
                            }
                            else
                            {
                                Console.WriteLine($"Failed set xfercount: {sts}.");
                            }
                            Console.WriteLine();


                            sts = caps.ICAP_PIXELTYPE.SetOrConstraint(MSG.SET, TWPT.GRAY);
                            if (sts == STS.SUCCESS)
                            {
                                Console.WriteLine("Successfully set pixel type to GRAY.");
                            }
                            else
                            {
                                Console.WriteLine($"Failed set pixel type: {sts}.");
                            }
                            Console.WriteLine();

                            sts = session.StartCapture(false);
                            if (sts == STS.SUCCESS)
                            {
                                Console.Error.WriteLine("Waiting for capture to complete.");
                                Console.WriteLine();

                                while (!hold.IsSet) Thread.Sleep(100);
                            }
                            else
                            {
                                Console.Error.WriteLine("Failed to start capture: " + sts);
                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("No devices opened.");
                            Console.WriteLine();
                        }
                    }
                }
                else
                {
                    Console.Error.WriteLine("Failed to open DSM");
                }
                Console.WriteLine("Test Ended");
            }

        }

        private static void Session_SourceDisabled(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void WriteCapInfo(Capabilities caps, CAP cap)
        {
            // use reflection due to unknown generics
            var propInfo = typeof(Capabilities).GetProperty(cap.ToString());
            if (propInfo == null) return;

            var capWrapper = propInfo.GetValue(caps);
            var wrapType = capWrapper.GetType();
            var label = (string)wrapType.GetMethod(nameof(CapWrapper<int>.GetLabel)).Invoke(capWrapper, null);
            var supports = (TWQC)wrapType.GetMethod(nameof(CapWrapper<int>.QuerySupport)).Invoke(capWrapper, null);

            Console.WriteLine($"\t{label ?? cap.ToString()}: {supports}");
            Console.WriteLine($"\t\tDefault: {wrapType.GetMethod(nameof(CapWrapper<int>.GetDefault)).Invoke(capWrapper, null)}");
            Console.WriteLine($"\t\tCurrent: {wrapType.GetMethod(nameof(CapWrapper<int>.GetCurrent)).Invoke(capWrapper, null)}");
            bool first = true;
            foreach (var val in (IEnumerable)wrapType.GetMethod(nameof(CapWrapper<int>.GetValues)).Invoke(capWrapper, null))
            {
                if (first)
                {
                    Console.WriteLine($"\t\tValues:\t{val}");
                    first = false;
                }
                else
                {
                    Console.WriteLine($"\t\t\t{val}");
                }
            }
        }
    }
}
