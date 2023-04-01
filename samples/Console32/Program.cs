using NTwain;
using System;
using System.Diagnostics;
using System.Reflection;
using TWAINWorkingGroup;

namespace SampleConsole
{
  internal class Program
  {
    static void Main(string[] args)
    {
      TwainPlatform.PreferLegacyDSM = true;

      var twain = new TwainSession(Assembly.GetExecutingAssembly().Location);
      twain.StateChanged += Twain_StateChanged;

      var hwnd = IntPtr.Zero;
      var rc = twain.DGControl.Parent.OpenDSM(ref hwnd);
      Debug.WriteLine($"OpenDSM={rc}");

      if (rc == TWAINWorkingGroup.STS.SUCCESS)
      {
        Debug.WriteLine($"CloseDSM={rc}");
        rc = twain.DGControl.Parent.CloseDSM(ref hwnd);
      }
    }

    private static void Twain_StateChanged(TwainSession session, TWAINWorkingGroup.STATE state)
    {
      Console.WriteLine($"State changed to {state}");
    }
  }
}