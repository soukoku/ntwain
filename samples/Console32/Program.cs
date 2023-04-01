using NTwain;
using System.Reflection;

namespace SampleConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var twain = new TwainSession(Environment.ProcessPath ?? Assembly.GetExecutingAssembly().Location);
    }
  }
}