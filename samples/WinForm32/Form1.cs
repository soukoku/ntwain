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
      Text += TwainPlatform.Is32bit ? " 32bit" : " 64bit";
      TwainPlatform.PreferLegacyDSM = true;

      twain = new TwainSession(Assembly.GetExecutingAssembly().Location);
      twain.StateChanged += Twain_StateChanged;
      twain.DefaultSourceChanged += Twain_DefaultSourceChanged;
      twain.CurrentSourceChanged += Twain_CurrentSourceChanged;
    }

    private void Twain_CurrentSourceChanged(TwainSession arg1, TW_IDENTITY_LEGACY ds)
    {
      lblCurrent.Text = ds.ProductName;
    }

    private void Twain_DefaultSourceChanged(TwainSession arg1, TW_IDENTITY_LEGACY ds)
    {
      lblDefault.Text = ds.ProductName;
    }

    private void Twain_StateChanged(TwainSession session, STATE state)
    {
      lblState.Text = state.ToString();
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

    private void btnSelect_Click(object sender, EventArgs e)
    {
      twain.DGControl.Identity.UserSelect();
    }

    private void btnEnumSources_Click(object sender, EventArgs e)
    {
      listSources.Items.Clear();
      foreach (var ds in twain.GetSources())
      {
        listSources.Items.Add(ds);
      }
    }

    private void btnSetDef_Click(object sender, EventArgs e)
    {
      if (listSources.SelectedItem is TW_IDENTITY_LEGACY ds)
      {
        twain.DGControl.Identity.Set(ds);
      }
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
      if (listSources.SelectedItem is TW_IDENTITY_LEGACY ds)
      {
        twain.TryStepdown(STATE.S3);

        twain.DGControl.Identity.OpenDS(ds);
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      twain.DGControl.Identity.CloseDS();
    }

    private void btnOpenDef_Click(object sender, EventArgs e)
    {
      twain.TryStepdown(STATE.S3);

      twain.DGControl.Identity.OpenDS(twain.DefaultSource);
    }
  }
}