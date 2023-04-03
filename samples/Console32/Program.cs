using NTwain;
using NTwain.Data;
using System;
using System.Diagnostics;
using System.Reflection;

namespace SampleConsole
{
  internal class Program
  {
    // CONSOLE won't work yet until I got a message loop going.

    static void Main(string[] args)
    {
      TwainPlatform.PreferLegacyDSM = true;

      var twain = new TwainSession(new InPlaceMarshaller(), Assembly.GetExecutingAssembly().Location);
      twain.StateChanged += Twain_StateChanged;

      var hwnd = IntPtr.Zero; // required for windows
      var rc = twain.OpenDSM(hwnd);
      Debug.WriteLine($"OpenDSM={rc}");

      if (rc == STS.SUCCESS)
      {
        Debug.WriteLine($"CloseDSM={rc}");
        rc = twain.CloseDSM();
      }
    }

    private static void Twain_StateChanged(TwainSession session, STATE state)
    {
      Console.WriteLine($"State changed to {state}");
    }
  }
}