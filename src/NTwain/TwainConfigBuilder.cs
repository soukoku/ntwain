using NTwain.Data;
using NTwain.Internals;
using NTwain.Resources;
using System;
using System.Diagnostics;
using System.Reflection;

namespace NTwain
{
    /// <summary>
    /// Builder for generating a <see cref="TwainConfig"/> object.
    /// </summary>
    public class TwainConfigBuilder
    {
        //private bool _legacy;
        private string _appName;
        private Version _version;
        private string _companyName;
        private Language _lang;
        private DataGroups _dg = DataGroups.Image;
        private bool _32bit;
        private PlatformID _platform;
        private Country _country;

        /// <summary>
        /// Default ctor.
        /// </summary>
        public TwainConfigBuilder()
        {
            _32bit = IntPtr.Size == 4;
            _platform = Environment.OSVersion.Platform;
        }

        ///// <summary>
        ///// Specifies which DSM to use. 
        ///// </summary>
        ///// <param name="legacyDsm"></param>
        ///// <returns></returns>
        //public TwainConfigurationBuilder UseDsm(bool legacyDsm = false)
        //{
        //    if (legacyDsm)
        //    {
        //        if (_64bit) throw new InvalidOperationException("Cannot use legacy DSM under 64bit.");
        //    }
        //    _legacy = legacyDsm;
        //    return this;
        //}

        /// <summary>
        /// Specifies what kind of data the app can to handle from TWAIN devices.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="audio"></param>
        /// <returns></returns>
        public TwainConfigBuilder HandlesDataType(bool image = true, bool audio = false)
        {
            DataGroups dg = DataGroups.None;
            if (image) dg |= DataGroups.Image;
            if (audio) dg |= DataGroups.Audio;

            if (dg == DataGroups.None) throw new InvalidOperationException(MsgText.NoDataTypesSpecified);
            _dg = dg;
            return this;
        }


        /// <summary>
        /// Defines the app info that will interface with TWAIN.
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="appVersion"></param>
        /// <param name="companyName"></param>
        /// <param name="language"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public TwainConfigBuilder DefineApp(string appName,
            Version appVersion, string companyName = null,
            Language language = Language.EnglishUSA, Country country = Country.USA)
        {
            _appName = appName;
            _version = appVersion;
            _companyName = companyName;
            _lang = language;
            _country = country;
            return this;
        }

        /// <summary>
        /// Defines the app info that will interface with TWAIN.
        /// </summary>
        /// <param name="appAssembly">Assembly containing the app info.</param>
        /// <param name="language"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public TwainConfigBuilder DefineApp(Assembly appAssembly,
            Language language = Language.EnglishUSA, Country country = Country.USA)
        {
            var info = FileVersionInfo.GetVersionInfo(appAssembly.Location);
            return DefineApp(info.ProductName, appAssembly.GetName().Version, info.CompanyName, language, country);
        }


        /// <summary>
        /// Builds the final configuration object.
        /// </summary>
        /// <returns></returns>
        public TwainConfig Build()
        {
            var config = new TwainConfig
            {
                Platform = _platform,
                Is32Bit = _32bit
            };

            // todo: change id based on platform
            switch (_platform)
            {
                case PlatformID.Win32NT:
                    config.DefaultMemoryManager = new WinMemoryManager(); // initial default
                    config.App32 = new TW_IDENTITY
                    {
                        DataFlags = DataFlags.App2,
                        DataGroup = DataGroups.Control | _dg,
                        Manufacturer = _companyName ?? "Unknown",
                        ProductFamily = _appName ?? "Unknown",
                        ProductName = _appName ?? "Unknown",
                        ProtocolMajor = TwainConst.ProtocolMajor,
                        ProtocolMinor = TwainConst.ProtocolMinor,
                        Version = new TW_VERSION
                        {
                            Country = _country,
                            Language = _lang,
                            Major = (short)_version.Major,
                            Minor = (short)_version.Minor,
                            Info = ""
                        },
                    };
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                default:
                    throw new PlatformNotSupportedException(string.Format(MsgText.PlatformNotSupported, _platform));
            }
            return config;
        }
    }
}
