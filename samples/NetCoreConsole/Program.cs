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

            }


            Console.WriteLine("Hello World!");
        }
    }
}
