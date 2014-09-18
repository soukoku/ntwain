using NTwain;
using NTwain.Data;

namespace Tester.WPF
{
    /// <summary>
    /// Wraps a data source as view model.
    /// </summary>
    class DSVM
    {
        public DataSource DS { get; set; }

        public string Name { get { return DS.Name; } }
        public string Version { get { return DS.Version.Info; } }
        public string Protocol { get { return DS.ProtocolVersion.ToString(); } }
    }
}
