using System.Reflection;

[assembly: AssemblyCopyright("Copyright \x00a9 Yin-Chun Wang 2012-2016")]
[assembly: AssemblyCompany("Yin-Chun Wang")]

[assembly: AssemblyVersion(NTwain.VersionInfo.Release)]
[assembly: AssemblyFileVersion(NTwain.VersionInfo.Build)]
[assembly: AssemblyInformationalVersion(NTwain.VersionInfo.Build)]

namespace NTwain
{
    /// <summary>
    /// Contains version info of NTwain.
    /// </summary>
    static class VersionInfo
    {
        /// <summary>
        /// The major release version number.
        /// </summary>
        public const string Release = "3.0.0.0"; // keep this same in major (breaking) releases

        
        /// <summary>
        /// The build release version number.
        /// </summary>
        public const string Build = "3.4.0"; // change this for each nuget release


    }
}