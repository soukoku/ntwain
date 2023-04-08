using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormSample
{
  static class ControlExtensions
  {
    public static void SetDoubleBufferedAsNeeded(this Control control)
    {
      var dbprop = control.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
      dbprop!.SetValue(control, !SystemInformation.TerminalServerSession);
    }

  }
}
