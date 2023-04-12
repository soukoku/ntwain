using Microsoft.AspNetCore.SignalR;
using NTwain.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SampleServer
{
  public class TwainHub : Hub<ITwainClient>
  {
    private readonly TwainForm twain;

    public TwainHub(TwainForm twain)
    {
      this.twain = twain;
    }

    public override Task OnConnectedAsync()
    {
      Debug.WriteLine("Server hub got connected by " + Context.ConnectionId);
      twain.KnownClients.TryAdd(Context.ConnectionId, true);
      return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
      Debug.WriteLine("Server hub got disconnected by " + Context.ConnectionId);
      twain.KnownClients.TryRemove(Context.ConnectionId, out _);
      return base.OnDisconnectedAsync(exception);
    }

    public IEnumerable<string> GetSources()
    {
      return twain.Session.GetSources().Select(s => s.ProductName.ToString());
    }

    public STS OpenSource(string source)
    {
      if (twain.Session.State == STATE.S3)
      {
        var hit = twain.Session.GetSources()
        .FirstOrDefault(s => s.ProductName.ToString() == source);

        if (hit.HasValue) return twain.Session.OpenSource(hit);

        return new STS { RC = TWRC.FAILURE, STATUS = new TW_STATUS { ConditionCode = TWCC.BADDEST } };
      }
      return new STS { RC = TWRC.FAILURE, STATUS = new TW_STATUS { ConditionCode = TWCC.SEQERROR } };
    }
    public STS CloseSource(string source)
    {
      if (twain.Session.State == STATE.S4)
      {
        return twain.Session.CloseSource();
      }
      return new STS { RC = TWRC.FAILURE, STATUS = new TW_STATUS { ConditionCode = TWCC.SEQERROR } };
    }
    public STS EnableSource(bool showUI, bool uiOnly)
    {
      if (twain.Session.State == STATE.S4)
        return twain.Invoke(new Func<STS>(() => twain.Session.EnableSource(showUI, uiOnly)));
      return new STS { RC = TWRC.FAILURE, STATUS = new TW_STATUS { ConditionCode = TWCC.SEQERROR } };
    }
  }
}