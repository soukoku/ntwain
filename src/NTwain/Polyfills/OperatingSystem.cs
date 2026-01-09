#if NETFRAMEWORK
using System;
using System.Collections.Generic;

namespace NTwain;

/// <summary>
/// Nicety for old framework use to not have to use preprocessors.
/// </summary>
static class OperatingSystem
{
    static readonly bool _isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
    public static bool IsWindows() => _isWindows;

    static readonly bool _isLinux = Environment.OSVersion.Platform == PlatformID.Unix;
    public static bool IsLinux() => _isLinux;

    static readonly bool _isMacOS = Environment.OSVersion.Platform == PlatformID.MacOSX;
    public static bool IsMacOS() => _isMacOS;

    private struct VersionKey : IEquatable<VersionKey>
    {
        public int Major;
        public int Minor;
        public int Build;
        public int Revision;

        public VersionKey(int major, int minor, int build, int revision)
        {
            Major = major;
            Minor = minor;
            Build = build;
            Revision = revision;
        }

        public bool Equals(VersionKey other)
        {
            return Major == other.Major && Minor == other.Minor && Build == other.Build && Revision == other.Revision;
        }

        public override bool Equals(object obj)
        {
            return obj is VersionKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + Major;
                hash = hash * 31 + Minor;
                hash = hash * 31 + Build;
                hash = hash * 31 + Revision;
                return hash;
            }
        }
    }

    private static readonly Dictionary<VersionKey, bool> _versionCache = new Dictionary<VersionKey, bool>();

    public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
    {
        if (!IsWindows())
            return false;

        var key = new VersionKey(major, minor, build, revision);
        if (!_versionCache.TryGetValue(key, out bool result))
        {
            result = IsOSVersionAtLeast(major, minor, build, revision);
            _versionCache[key] = result;
        }
        return result;
    }

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