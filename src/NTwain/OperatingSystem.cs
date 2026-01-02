#if NETFRAMEWORK
using System;

namespace NTwain;

/// <summary>
/// Hack to make it build without preprocessor directives all over.
/// </summary>
static class OperatingSystem
{
    static readonly bool _isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;

    public static bool IsWindows() => _isWindows;

    // copied from .net logic

    public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
            => IsWindows() && IsOSVersionAtLeast(major, minor, build, revision);

    private static bool IsOSVersionAtLeast(int major, int minor, int build, int revision)
    {
        Version current = Environment.OSVersion.Version;

        if (current.Major != major)
        {
            return current.Major > major;
        }
        if (current.Minor != minor)
        {
            return current.Minor > minor;
        }
        // Unspecified build component is to be treated as zero
        int currentBuild = current.Build < 0 ? 0 : current.Build;
        build = build < 0 ? 0 : build;
        if (currentBuild != build)
        {
            return currentBuild > build;
        }

        // Unspecified revision component is to be treated as zero
        int currentRevision = current.Revision < 0 ? 0 : current.Revision;
        revision = revision < 0 ? 0 : revision;

        return currentRevision >= revision;
    }
}
#endif