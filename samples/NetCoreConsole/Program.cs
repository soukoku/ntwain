using NTwain;
using System;
using System.Reflection;

namespace NetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new TwainConfigurationBuilder()
                .DefineApp(Assembly.GetExecutingAssembly())
                .Build();
            using (var session = new TwainSession(config))
            {
                session.PropertyChanged += Session_PropertyChanged;

                var handle = IntPtr.Zero;
                session.Open(ref handle);

            }

        }

        private static void Session_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var session = (TwainSession)sender;
            if (e.PropertyName == "State")
            {
                Console.WriteLine($"State changed to {session.State}");
            }
        }
    }
}
