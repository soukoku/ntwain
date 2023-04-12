using NTwain;
using NTwain.Data;
using System;
using System.Threading.Tasks;

namespace SampleServer
{
  public interface ITwainClient
  {
    // mirrors the twain session events
    Task StateChanged(STATE state);
    Task DefaultSourceChanged(string? source);
    Task CurrentSourceChanged(string? source);
    Task SourceDisabled(string source);
    Task DeviceEvent(TW_DEVICEEVENT evt);
    Task TransferError();
    Task<CancelType> TransferReady(int pending, TWEJ eojType);
    Task TransferCanceled();
    Task Transferred();
  }
}
