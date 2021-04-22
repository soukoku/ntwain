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

            using (var twain = new TwainSession(Assembly.GetExecutingAssembly(), null, IntPtr.Zero))
            using (var hold = new ManualResetEventSlim())
            {
                twain.DeviceEvent += (sender, e) =>
                {
                    Console.WriteLine($"Got device event " + (TWDE)e.Event);
                };
                twain.ScanEvent += (sender, closing) =>
                {
                    Console.WriteLine($"Got scan event " + closing);

                    TW_USERINTERFACE twuserinterface = default;
                    if (twain.TWAIN.DatUserinterface(DG.CONTROL, MSG.DISABLEDS, ref twuserinterface) == STS.SUCCESS)
                    {
                        Console.WriteLine("Disabled device.");
                    }
                    else
                    {
                        Console.Error.WriteLine("Failed to disabled device.");
                    }
                    hold.Set();
                };

                if (twain.Open() == TWAINWorkingGroup.STS.SUCCESS)
                {
                    Console.WriteLine("Opened DSM");

                    Console.WriteLine("Default device:");
                    Console.WriteLine($"\t{twain.DefaultDevice}");

                    Console.WriteLine("All devices:");
                    TW_IDENTITY dsToUse = default;
                    foreach (var dev in twain.GetDevices())
                    {
                        Console.WriteLine($"\t{dev}");
                        if (dev.ProductName == "TWAIN2 FreeImage Software Scanner")
                        {
                            dsToUse = dev;
                        }
                    }

                    twain.CurrentDevice = dsToUse;
                    if (twain.CurrentDevice.HasValue)
                    {
                        Console.WriteLine("Current device after opening attempt:");
                        Console.WriteLine($"\t{twain.CurrentDevice}");

                        var sts = twain.StartCapture(false);
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
