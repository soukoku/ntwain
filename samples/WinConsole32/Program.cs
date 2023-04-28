using NTwain;
using NTwain.Data;
using System.Diagnostics;

namespace WinConsole32
{
  internal class Program
  {
    static async Task Main(string[] args)
    {
      var libVer = FileVersionInfo.GetVersionInfo(typeof(TwainAppSession).Assembly.Location).ProductVersion;
      Console.WriteLine($"Console sample {(TWPlatform.Is32bit ? " 32bit" : " 64bit")} on NTwain {libVer}");

      TwainAppSession session = new TwainAppSession();

      session.StateChanged += Session_StateChanged;
      session.SourceDisabled += Session_SourceDisabled1;
      session.Transferred += Session_Transferred;

      var sts = await session.OpenDSMAsync();

      if (sts.IsSuccess)
      {
        Console.WriteLine("Available data sources:");

        TW_IDENTITY_LEGACY firstSrc = default;
        foreach (var src in session.GetSources())
        {
          if (!firstSrc.HasValue) firstSrc = src;
          Console.WriteLine($"\t{src}");
        }
        Console.WriteLine();

        var defaultSrc = session.DefaultSource;
        Console.WriteLine($"Default data source = {defaultSrc}");
        Console.WriteLine();

        session.ShowUserSelect();
        Console.WriteLine($"Selected data source = {session.DefaultSource}");
        Console.WriteLine();

        var targetSrc = defaultSrc.HasValue ? defaultSrc : firstSrc;

        if (targetSrc.HasValue)
        {
          TestThisSource(session, targetSrc);
        }
        else
        {
          Console.WriteLine("No data source to test.");
          Console.WriteLine();
        }

        Console.WriteLine("---------------------------------------------");
        Console.WriteLine("Test in progress, press Enter to stop testing");
        Console.WriteLine("---------------------------------------------");
        Console.ReadLine();
        session.TryStepdown(STATE.S1);
      }
      else
      {
        Console.WriteLine("Failed to attach: " + sts);
      }

      Console.WriteLine("-------------------");
      Console.WriteLine("Press Enter to exit");
      Console.WriteLine("-------------------");
      Console.ReadLine();
    }

    private static void Session_Transferred(TwainAppSession sender, TransferredEventArgs e)
    {
      if (e.Data != null)
      {
        Console.WriteLine("SUCCESS! Got twain data on thread {0}.", Environment.CurrentManagedThreadId);
      }
      else
      {
        Console.WriteLine("BUMMER! No twain data on thread {0}.", Environment.CurrentManagedThreadId);
      }
      e.Dispose();
    }

    private static void Session_StateChanged(TwainAppSession sender, STATE e)
    {
      Console.WriteLine($"Session state changed to {sender.State}");
    }

    private static void Session_SourceDisabled1(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
      Console.WriteLine($"Session source disabled.");
      sender.CloseSource();
    }

    private static void TestThisSource(TwainAppSession session, TW_IDENTITY_LEGACY source)
    {
      Console.WriteLine($"Testing data source {source}");
      Console.WriteLine();

      session.OpenSource(source);

      var rc = session.EnableSource(true, false);
    }
  }
}