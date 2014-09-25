using System.Reflection;

[assembly: AssemblyCopyright("Copyright \x00a9 Yin-Chun Wang 2012-2014")]
[assembly: AssemblyCompany("Yin-Chun Wang")]

[assembly: AssemblyVersion(NTwain.NTwainVersionInfo.Release)]
[assembly: AssemblyFileVersion(NTwain.NTwainVersionInfo.Build)]
[assembly: AssemblyInformationalVersion(NTwain.NTwainVersionInfo.Build)]

namespace NTwain
{
    /// <summary>
    /// Contains version info of this assembly.
    /// </summary>
    public class NTwainVersionInfo
    {
        /// <summary>
        /// The major release version number.
        /// </summary>
        public const string Release = "3.0.0.0"; // keep this same in major (breaking) releases

        
        /// <summary>
        /// The build release version number.
        /// </summary>
        public const string Build = "3.1.0"; // change this for each nuget release


    }
}