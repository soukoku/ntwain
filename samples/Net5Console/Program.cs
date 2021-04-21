using NTwain;
using System;
using System.Reflection;

namespace Net5Console
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var twain = new TwainSession(Assembly.GetExecutingAssembly(), null, IntPtr.Zero))
            {
                if (twain.Open() == TWAINWorkingGroup.STS.SUCCESS)
                {
                    Console.WriteLine("Opened DSM");
                    Console.WriteLine("Default device:");
                    Console.WriteLine($"\t{twain.DefaultDevice}");
                    Console.WriteLine("All devices:");
                    foreach (var dev in twain.GetDevices())
                    {
                        Console.WriteLine($"\t{dev}");
                    }
                    Console.WriteLine("Current device:");
                    Console.WriteLine($"\t{twain.CurrentDevice}");
                    twain.CurrentDevice = twain.DefaultDevice;
                    Console.WriteLine("Current device after setting:");
                    Console.WriteLine($"\t{twain.CurrentDevice}");
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
