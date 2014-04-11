using System.Reflection;

[assembly: AssemblyCopyright("Copyright \x00a9 Yin-Chun Wang 2012-2014")]
[assembly: AssemblyCompany("Yin-Chun Wang")]

[assembly: AssemblyVersion(NTwain._NTwainVersionInfo.Release)]
[assembly: AssemblyFileVersion(NTwain._NTwainVersionInfo.Build)]
[assembly: AssemblyInformationalVersion(NTwain._NTwainVersionInfo.Build)]

namespace NTwain
{
    class _NTwainVersionInfo
    {
        // keep this same in majors releases
        public const string Release = "0.10.0.0";
        // change this for each nuget release
        public const string Build = "0.10.1";
    }
}