using NTwain;
using NTwain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace WinFormSample
{
  public partial class Form1 : Form
  {
    private TwainAppSession twain;

    public Form1()
    {
      InitializeComponent();
      var libVer = FileVersionInfo.GetVersionInfo(typeof(TwainAppSession).Assembly.Location).FileVersion;
      Text += $"{(TwainPlatform.Is32bit ? " 32bit" : " 64bit")} on NTwain {libVer}";

      TwainPlatform.PreferLegacyDSM = false;

      twain = new TwainAppSession(new WinformMarshaller(this), Assembly.GetExecutingAssembly().Location);
      twain.StateChanged += Twain_StateChanged;
      twain.DefaultSourceChanged += Twain_DefaultSourceChanged;
      twain.CurrentSourceChanged += Twain_CurrentSourceChanged;

      this.Disposed += Form1_Disposed;
    }

    private void Form1_Disposed(object? sender, EventArgs e)
    {
      twain.Dispose();
    }

    private void Twain_CurrentSourceChanged(TwainAppSession arg1, TW_IDENTITY_LEGACY ds)
    {
      lblCurrent.Text = ds.ProductName;
      if (twain.State == STATE.S4)
      {
        var caps = twain.GetAllCaps();
        foreach (var c in caps)
          listCaps.Items.Add(c);

        // never seen a driver support these but here it is
        var sts = twain.GetCapLabel(CAP.ICAP_SUPPORTEDSIZES, out string? test);
        var sts2 = twain.GetCapHelp(CAP.ICAP_SUPPORTEDSIZES, out string? test2);
        var sts3 = twain.GetCapLabelEnum(CAP.ICAP_SUPPORTEDSIZES, out IList<string>? test3);

        if (sts.RC == TWRC.SUCCESS || sts2.RC == TWRC.SUCCESS || sts3.RC == TWRC.SUCCESS)
        {
          Debugger.Break();
        }
      }
      else
      {
        listCaps.Items.Clear();
      }
    }

    private void Twain_DefaultSourceChanged(TwainAppSession arg1, TW_IDENTITY_LEGACY ds)
    {
      lblDefault.Text = ds.ProductName;
    }

    private void Twain_StateChanged(TwainAppSession session, STATE state)
    {
      lblState.Text = state.ToString();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);


      var hwnd = this.Handle;
      var rc = twain.OpenDSM(hwnd);
      twain.AddWinformFilter();
      Debug.WriteLine($"OpenDSM={rc}");
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      var finalState = twain.TryStepdown(STATE.S2);
      Debug.WriteLine($"Stepdown result state={finalState}");
      twain.RemoveWinformFilter();
      base.OnClosing(e);
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      twain.ShowUserSelect();
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
        twain.SetDefaultSource(ds);
      }
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
      if (listSources.SelectedItem is TW_IDENTITY_LEGACY ds)
      {
        twain.TryStepdown(STATE.S3);

        twain.OpenSource(ds);
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      twain.CloseSource();
    }

    private void btnOpenDef_Click(object sender, EventArgs e)
    {
      twain.TryStepdown(STATE.S3);

      twain.OpenSource(twain.DefaultSource);
    }

    private void btnShowSettings_Click(object sender, EventArgs e)
    {
      twain.EnableSource(true, true);
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      twain.EnableSource(true, false);
    }
  }
}