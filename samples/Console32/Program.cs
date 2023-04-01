using NTwain;
using System.Reflection;

namespace SampleConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var twain = new TwainSession(Environment.ProcessPath ?? Assembly.GetExecutingAssembly().Location);
      twain.StateChanged += Twain_StateChanged;
    }

    private static void Twain_StateChanged(TwainSession session, TWAINWorkingGroup.STATE state)
    {
      Console.WriteLine($"State changed to {state}");
    }
  }
}