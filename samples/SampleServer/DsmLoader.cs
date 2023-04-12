using NTwain.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SampleServer
{
  /// <summary>
  /// For demoing loading dsm from custom path in case
  /// it's not installed on system and don't want to be 
  /// placed besides the exe.
  /// </summary>
  static class DsmLoader
  {
    static IntPtr __dllPtr;

    public static bool TryUseCustomDSM()
    {
      if (__dllPtr == IntPtr.Zero)
      {
        var dll = Path.Combine(
          Path.GetDirectoryName(Environment.ProcessPath ?? Assembly.GetExecutingAssembly().Location)!,
          $@"runtimes\win-{(TWPlatform.Is32bit ? "x86" : "x64")}\native\TWAINDSM.dll");

        __dllPtr = LoadLibraryW(dll);
      }
      return __dllPtr != IntPtr.Zero;
    }

    [DllImport("kernel32", SetLastError = true)]
    static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
  }
}
