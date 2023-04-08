using NTwain.Data;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WinFormSample
{
  internal static class Program
  {
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      if (DsmLoader.TryUseCustomDSM())
      {
        Debug.WriteLine("Using our own dsm now :)");
      }
      else
      {
        Debug.WriteLine("Will attempt to use default dsm :(");
      }

      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();
      Application.Run(new Form1());
    }
  }
}