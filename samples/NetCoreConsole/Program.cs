using NTwain;
using System;
using System.Reflection;

namespace NetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = new TwainConfigBuilder()
                    .DefineApp(Assembly.GetExecutingAssembly())
                    .Build();
                Console.WriteLine($"App = {(config.Is64Bit ? "64bit" : "32bit")}");
                Console.WriteLine($"Platform = {config.Platform}");

                using (var session = new TwainSession(config))
                {
                    session.PropertyChanged += Session_PropertyChanged;

                    var handle = IntPtr.Zero;
                    if (session.Open(ref handle) == NTwain.Data.ReturnCode.Success)
                    {

                    }

                }

                Console.WriteLine("Test ended, press Enter to exit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }
        }

        private static void Session_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var session = (TwainSession)sender;
            if (e.PropertyName == "State")
            {
                Console.WriteLine($"Session state changed to {session.State}");
            }
        }
    }
}
