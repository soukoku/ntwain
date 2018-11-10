using NTwain.Data;
using NTwain.Internals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NTwain
{
    public class TwainConfig
    {
        internal TwainConfig() { }

        public bool PreferLegacyDsm { get; internal set; }

        public string AppName { get; internal set; }

        internal DataGroups DataGroup { get; set; }
        internal IMemoryManager MemoryManager { get; set; }
    }

    public class TwainConfigurationBuilder
    {
        private bool _legacy;
        private string _appName;
        private string _companyName;
        private DataGroups _dg = DataGroups.Image;
        private bool _64bit;
        private PlatformID _platform;

        /// <summary>
        /// Default ctor.
        /// </summary>
        public TwainConfigurationBuilder()
        {
            _64bit = IntPtr.Size == 8;
            _platform = Environment.OSVersion.Platform;
        }

        /// <summary>
        /// Specifies which DSM to use. 
        /// </summary>
        /// <param name="legacyDsm"></param>
        /// <returns></returns>
        public TwainConfigurationBuilder UseDsm(bool legacyDsm = false)
        {
            if (legacyDsm)
            {
                if (_64bit) throw new InvalidOperationException("Cannot use legacy DSM under 64bit.");
            }
            _legacy = legacyDsm;
            return this;
        }

        /// <summary>
        /// Specifies what kind of data the app can to handle from TWAIN devices.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="audio"></param>
        /// <returns></returns>
        public TwainConfigurationBuilder HandlesDataType(bool image = true, bool audio = false)
        {
            DataGroups dg = DataGroups.None;
            if (image) dg |= DataGroups.Image;
            if (audio) dg |= DataGroups.Audio;

            if (dg == DataGroups.None) throw new InvalidOperationException("No data type specified.");
            _dg = dg;
            return this;
        }

        /// <summary>
        /// Defines the app info that will interface with TWAIN.
        /// </summary>
        /// <param name="appName"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public TwainConfigurationBuilder DefineApp(string appName, string companyName = null)
        {
            _appName = appName;
            _companyName = companyName;
            return this;
        }

        /// <summary>
        /// Defines the app info that will interface with TWAIN.
        /// </summary>
        /// <param name="appAssembly">Assembly containing the app info.</param>
        /// <returns></returns>
        public TwainConfigurationBuilder DefineApp(Assembly appAssembly)
        {
            var info = FileVersionInfo.GetVersionInfo(appAssembly.Location);
            return DefineApp(info.ProductName, info.CompanyName);
        }


        /// <summary>
        /// Builds the final configuration object.
        /// </summary>
        /// <returns></returns>
        public TwainConfig Build()
        {
            return new TwainConfig
            {
                PreferLegacyDsm = _legacy,
                AppName = _appName,
                DataGroup = _dg
            };
        }
    }
}
