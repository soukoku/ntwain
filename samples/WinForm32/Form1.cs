using NTwain;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using TWAINWorkingGroup;

namespace WinForm32
{
  public partial class Form1 : Form
  {
    private TwainSession twain;

    public Form1()
    {
      InitializeComponent();

      TwainPlatform.PreferLegacyDSM = true;

      twain = new TwainSession(Assembly.GetExecutingAssembly().Location);
      twain.StateChanged += Twain_StateChanged;
    }

    private static void Twain_StateChanged(TwainSession session, STATE state)
    {
      Debug.WriteLine($"State changed to {state}");
    }

    protected override void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);


      var hwnd = this.Handle;
      var rc = twain.DGControl.Parent.OpenDSM(ref hwnd);
      Debug.WriteLine($"OpenDSM={rc}");

    }

    protected override void OnClosing(CancelEventArgs e)
    {
      var finalState = twain.TryStepdown(STATE.S2);
      Debug.WriteLine($"Stepdown result state={finalState}");

      base.OnClosing(e);
    }
  }
}