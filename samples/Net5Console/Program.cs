using NTwain;
using System;
using System.Reflection;
using System.Threading;
using TWAINWorkingGroup;

namespace Net5Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting twain test in console...");

            using (var session = new TwainSession(Assembly.GetExecutingAssembly(), null, IntPtr.Zero))
            using (var hold = new ManualResetEventSlim())
            {
                session.DeviceEvent += (sender, e) =>
                {
                    Console.WriteLine($"Got device event " + (TWDE)e.Event);
                };
                session.ScanEvent += (sender, closing) =>
                {
                    Console.WriteLine($"Got scan event " + closing);

                    // don't care, just end it
                    TW_PENDINGXFERS pending = default;
                    var sts = session.TWAIN.DatPendingxfers(DG.CONTROL, MSG.RESET, ref pending);

                    TW_USERINTERFACE twuserinterface = default;
                    if (session.TWAIN.DatUserinterface(DG.CONTROL, MSG.DISABLEDS, ref twuserinterface) == STS.SUCCESS)
                    {
                        Console.WriteLine("Disabled device.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Failed to disabled device.");
                    }
                    hold.Set();
                };

                if (session.Open() == TWAINWorkingGroup.STS.SUCCESS)
                {
                    Console.WriteLine("Opened DSM");

                    Console.WriteLine("Default device:");
                    Console.WriteLine($"\t{session.DefaultDevice}");

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

                    session.CurrentDevice = dsToUse;
                    if (session.CurrentDevice.HasValue)
                    {
                        Console.WriteLine("Current device after opening attempt:");
                        Console.WriteLine($"\t{session.CurrentDevice}");

                        var sts = session.StartCapture(false);
                        if (sts == STS.SUCCESS)
                        {
                            Console.Error.WriteLine("Waiting for capture to complete.");

                            while (!hold.IsSet) Thread.Sleep(100);
                        }
                        else
                        {
                            Console.Error.WriteLine("Failed to start capture: " + sts);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No devices opened.");
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
