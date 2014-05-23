using NTwain;
using NTwain.Data;

namespace Tester.WPF
{
    /// <summary>
    /// Wraps a data source as view model.
    /// </summary>
    class DSVM
    {
        public TwainSource DS { get; set; }

        public string Name { get { return DS.Name; } }
        public string Version { get { return DS.Version.Info; } }
        public string Protocol { get { return string.Format("{0}.{1}", DS.ProtocolMajor, DS.ProtocolMinor); } }
    }
}
