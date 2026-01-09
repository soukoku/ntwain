//using Microsoft.Extensions.Logging;
//using NTwain.Data;
//using System;
//using System.Diagnostics;
//using System.IO;
//using System.Reflection;
//using System.Runtime.InteropServices;

//namespace NTwain.DSM;

///// <summary>
///// For demoing loading dsm from custom path in case
///// it's not installed on system and don't want to be 
///// placed besides the exe.
///// </summary>
//static class DsmLoader
//{
//    static IntPtr __dllPtr;

//    public static bool TryLoadCustomDSM(ILogger logger)
//    {
//        if (__dllPtr == IntPtr.Zero)
//        {
//#if NETFRAMEWORK
//            var curFile = Assembly.GetExecutingAssembly().Location;
//            if (string.IsNullOrEmpty(curFile))
//            {
//                using var proc = Process.GetCurrentProcess();
//                curFile = proc.MainModule.FileName;
//            }
//            var folder = Path.GetDirectoryName(curFile);
//#else
//            var folder = AppContext.BaseDirectory;
//#endif
//            if (!string.IsNullOrEmpty(folder))
//            {
//                var dll = Path.Combine(
//                  folder,
//                  $@"runtimes\win-{(TWPlatform.Is32bit ? "x86" : "x64")}\native\TWAINDSM.dll");

//                __dllPtr = LoadLibraryW(dll);
//            }

//            if (__dllPtr != IntPtr.Zero)
//            {
//                logger.LogTrace("Using our own dsm now :)");
//            }
//            else
//            {
//                logger.LogTrace("Will attempt to use default dsm :(");
//            }
//        }
//        return __dllPtr != IntPtr.Zero;
//    }

//    [DllImport("kernel32", SetLastError = true)]
//    static extern IntPtr LoadLibraryW([MarshalAs(UnmanagedType.LPWStr)] string lpFileName);
//}
