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
    public static void SetDoubleBuffered(this Control control, bool value)
    {
      if (SystemInformation.TerminalServerSession) return;

      var dbprop = control.GetType().GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
      dbprop!.SetValue(control, value);
    }

  }
}
