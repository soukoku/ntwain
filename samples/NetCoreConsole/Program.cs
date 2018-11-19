using NTwain;
using System;
using System.Reflection;

namespace ConsoleApp
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

                Console.WriteLine($"App = {(config.Is32Bit ? "32bit" : "64bit")}");
                Console.WriteLine($"Platform = {config.Platform}");
                Console.WriteLine();

                using (var session = new TwainSession(config))
                {
                    session.PropertyChanged += Session_PropertyChanged;
                    session.SourceDisabled += Session_SourceDisabled;

                    if (session.Open(IntPtr.Zero) == NTwain.Data.ReturnCode.Success)
                    {
                        Console.WriteLine("Available data sources:");

                        DataSource firstSrc = null;
                        foreach (var src in session.GetSources())
                        {
                            if (firstSrc == null) firstSrc = src;
                            Console.WriteLine($"\t{src}");
                        }
                        Console.WriteLine();

                        var defaultSrc = session.DefaultSource;
                        Console.WriteLine($"Default data source = {defaultSrc}");
                        Console.WriteLine();

                        var selectSrc = session.ShowSourceSelector();
                        Console.WriteLine($"Selected data source = {selectSrc}");
                        Console.WriteLine();

                        var targetSrc = selectSrc ?? defaultSrc ?? firstSrc;

                        if (targetSrc != null)
                        {
                            TestThisSource(session, targetSrc);
                        }
                        else
                        {
                            Console.WriteLine("No data source to test.");
                            Console.WriteLine();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.ToString());
            }

            Console.WriteLine("----------------------------------");
            Console.WriteLine("Test ended, press Enter to exit...");
            Console.ReadLine();
        }

        private static void Session_SourceDisabled(object sender, EventArgs e)
        {
            var session = (TwainSession)sender;
            Console.WriteLine($"Session source disabled.");
            session.CurrentSource.Close();
        }

        private static void TestThisSource(TwainSession session, DataSource source)
        {
            Console.WriteLine($"Testing data source {source}");
            Console.WriteLine();

            source.Open();

            var testStatus = session.GetStatus();
            var testMessage = session.GetLocalizedStatus(ref testStatus);

            var rc = source.ShowUI(IntPtr.Zero);
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
