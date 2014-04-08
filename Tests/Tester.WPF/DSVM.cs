using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tester.WPF
{
    /// <summary>
    /// Wraps a data source as view model.
    /// </summary>
    class DSVM
    {
        public TWIdentity DS { get; set; }

        public string Name { get { return DS.ProductName; } }
        public string Version { get { return DS.Version.Info; } }
        public string Protocol { get { return string.Format("{0}.{1}", DS.ProtocolMajor, DS.ProtocolMinor); } }
    }
}
