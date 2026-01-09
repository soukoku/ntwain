using NTwain.Data;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using Windows.Win32;

namespace NTwain.Platform;

static class DllPath
{
    static FreeLibrarySafeHandle _loadDllPtr = new();

    /// <summary>
    /// Try to add the runtimes/win-{arch}/native path to the DLL search path.
    /// </summary>
    /// <returns></returns>
#if !NETFRAMEWORK
    [SupportedOSPlatform("windows6.0.6000")]
#endif
    public static void TryUseLocalDsm()
    {
        if (_loadDllPtr.IsInvalid == false) return;

        var arch = Environment.Is64BitProcess ? "x64" : "x86";
        var curFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (curFolder == null) return;

        var dsmFile = Path.Combine(curFolder, "twaindsm.dll");
        if (!File.Exists(dsmFile)) return;

        if (!TWPlatform.PreferLegacyDSM)
        {
            // try to preemptively load this dll into process before pinvoke does
            _loadDllPtr = PInvoke.LoadLibrary(dsmFile);
        }
    }
}
