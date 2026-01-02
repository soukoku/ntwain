#if NETFRAMEWORK
using System;

namespace NTwain;

/// <summary>
/// Hack to make it build without preprocessor directives all over.
/// </summary>
static class OperatingSystem
{
    public static bool IsWindows()
    {
        return Environment.OSVersion.Platform == PlatformID.Win32NT;
    }
}
#endif