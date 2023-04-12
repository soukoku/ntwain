using Microsoft.AspNetCore.SignalR;
using NTwain;
using NTwain.Data;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace SampleServer
{
  public partial class TwainForm : Form
  {
    private readonly IHubContext<TwainHub, ITwainClient> _hub;
    private int _count;

    public TwainAppSession Session { get; }
    // dont really care about value
    public ConcurrentDictionary<string, bool> KnownClients { get; } = new ConcurrentDictionary<string, bool>();

    public TwainForm(IHubContext<TwainHub, ITwainClient> hub)
    {
      InitializeComponent();
      var libVer = FileVersionInfo.GetVersionInfo(typeof(TwainAppSession).Assembly.Location).ProductVersion;
      Text += $"{(TWPlatform.Is32bit ? " 32bit" : " 64bit")} on NTwain {libVer}";
      _hub = hub;

      Session = new TwainAppSession(SynchronizationContext.Current!, Environment.ProcessPath!);
      Session.StateChanged += Session_StateChanged;
      Session.DefaultSourceChanged += Session_DefaultSourceChanged;
      Session.CurrentSourceChanged += Session_CurrentSourceChanged;
      Session.SourceDisabled += Session_SourceDisabled;
      Session.TransferReady += Session_TransferReady;
      Session.Transferred += Session_Transferred;
      Session.TransferCanceled += Session_TransferCanceled;
      Session.TransferError += Session_TransferError;
    }

    private void Session_TransferError(TwainAppSession sender, TransferErrorEventArgs e)
    {
      _hub.Clients.All.TransferError();
    }

    private void Session_TransferCanceled(TwainAppSession sender, TransferCanceledEventArgs e)
    {
      _hub.Clients.All.TransferCanceled();
    }

    private void Session_DefaultSourceChanged(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
      _hub.Clients.All.DefaultSourceChanged(e.HasValue ? e.ProductName.ToString() : null);
    }

    private void Session_CurrentSourceChanged(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
      _hub.Clients.All.CurrentSourceChanged(e.HasValue ? e.ProductName.ToString() : null);
    }

    private void Session_SourceDisabled(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
      Debug.WriteLine("Ended");
      _hub.Clients.All.SourceDisabled(e.ProductName.ToString());
    }

    private async void Session_TransferReady(TwainAppSession sender, TransferReadyEventArgs e)
    {
      foreach (var c in KnownClients.Keys)
      {
        var test = await _hub.Clients.Client(c).TransferReady(e.PendingCount, e.EndOfJobFlag);
        if (test != CancelType.None)
        {
          e.Cancel = test;
          break;
        }
      }
    }

    private void Session_StateChanged(TwainAppSession sender, STATE e)
    {
      _hub.Clients.All.StateChanged(e);
    }

    private void Session_Transferred(TwainAppSession sender, TransferredEventArgs e)
    {
      _count++;
      Debug.WriteLine("Transferred " + _count);
      e.Dispose();
    }

    protected override void OnHandleCreated(EventArgs e)
    {
      Session.OpenDSM(Handle);
      Session.AddWinformFilter();
      base.OnHandleCreated(e);
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
      Session.RemoveWinformFilter();
      Session.CloseDSM();
      Session.Dispose();
      base.OnHandleDestroyed(e);
    }

    private void button1_Click(object sender, EventArgs e)
    {
      if (Session.State == STATE.S3)
      {
        var sts = Session.OpenSource(Session.DefaultSource);
      }
      if (Session.State == STATE.S4)
      {
        _count = 0;
        var sts = Session.EnableSource(true, false);
      }
    }
  }
}
