using NTwain;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using TWAINWorkingGroup;

namespace Net5Console
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("Starting twain test in console...");
            Console.WriteLine();

            using (var session = new TwainSession(Assembly.GetExecutingAssembly(), null, IntPtr.Zero))
            using (var hold = new ManualResetEventSlim())
            {
                session.DeviceEvent += (sender, e) =>
                {
                    Console.WriteLine($"Got device event " + (TWDE)e.Event);
                    Console.WriteLine();
                };
                session.ScanEvent += (sender, closing) =>
                {
                    Console.WriteLine($"Got scan event " + closing);
                    Console.WriteLine();

                    // don't care, just end it
                    TW_PENDINGXFERS pending = default;
                    var sts = session.TWAIN.DatPendingxfers(DG.CONTROL, MSG.RESET, ref pending);

                    TW_USERINTERFACE twuserinterface = default;
                    if (session.TWAIN.DatUserinterface(DG.CONTROL, MSG.DISABLEDS, ref twuserinterface) == STS.SUCCESS)
                    {
                        Console.WriteLine("Disabled device.");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.Error.WriteLine("Failed to disabled device.");
                        Console.WriteLine();
                    }
                    hold.Set();
                };

                if (session.Open() == TWAINWorkingGroup.STS.SUCCESS)
                {
                    Console.WriteLine("Opened DSM");
                    Console.WriteLine();

                    Console.WriteLine("Default device:");
                    Console.WriteLine($"\t{session.DefaultDevice}");
                    Console.WriteLine();

                    Console.WriteLine("All devices:");
                    TW_IDENTITY dsToUse = default;
                    foreach (var dev in session.GetDevices())
                    {
                        Console.WriteLine($"\t{dev}");
                        if (dev.ProductName == "TWAIN2 FreeImage Software Scanner")
                        {
                            dsToUse = dev;
                        }
                    }
                    Console.WriteLine();

                    session.CurrentDevice = dsToUse;
                    if (session.CurrentDevice.HasValue)
                    {
                        Console.WriteLine("Current device after opening attempt:");
                        Console.WriteLine($"\t{session.CurrentDevice}");
                        Console.WriteLine();

                        var caps = session.Capabilities;
                        Console.WriteLine("Device supports these caps:");
                        //foreach (var cap in caps.Keys.OrderBy(c => c))
                        //{
                        //    Console.WriteLine($"\t{cap}: {caps[cap].Supports}");
                        //}
                        Console.WriteLine();

                        //if (caps.TryGetValue(CAP.ICAP_PIXELTYPE, out CapWrapper wrapper))
                        //{
                        //    Console.WriteLine($"Details on {wrapper.Cap}:");
                        //    Console.WriteLine($"\tDefault: {wrapper.GetDefault()}");
                        //    Console.WriteLine($"\tCurrent: {wrapper.GetCurrent()}");
                        //    Console.WriteLine($"\tValues:");
                        //    foreach (var val in wrapper.GetValues())
                        //    {
                        //        Console.WriteLine($"\t\t{val}");
                        //    }
                        //}
                        //Console.WriteLine();

                        var sts = session.StartCapture(false);
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
                else
                {
                    Console.Error.WriteLine("Failed to open DSM");
                }
                Console.WriteLine("Test Ended");
            }

        }
    }
}
