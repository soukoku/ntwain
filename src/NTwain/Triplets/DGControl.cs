using NTwain.Data;
using NTwain.Triplets.ControlDATs;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains triplet calls starting with <see cref="DG.CONTROL"/>.
  /// </summary>
  public static class DGControl
  {
    public static readonly Callback Callback = new();

    public static readonly Callback2 Callback2 = new();

    public static readonly Capability Capability = new();

    public static readonly CapabilityCustom CapabilityCustom = new();

    public static readonly CustomDsData CustomDsData = new();

    public static readonly DeviceEvent DeviceEvent = new();

    public static readonly EntryPoint EntryPoint = new();

    public static readonly Event Event = new();

    public static readonly FileSystem FileSystem = new();

    public static readonly Identity Identity = new();

    public static readonly Metrics Metrics = new();

    public static readonly Parent Parent = new();

    public static readonly Passthru Passthru = new();

    public static readonly PendingXfers PendingXfers = new();

    public static readonly SetupFileXfer SetupFileXfer = new();

    public static readonly SetupMemXfer SetupMemXfer = new();

    public static readonly Status Status = new();

    public static readonly StatusUtf8 StatusUtf8 = new();

    public static readonly TwainDirect TwainDirect = new();

    public static readonly UserInterface UserInterface = new();

    public static readonly XferGroup XferGroup = new();

  }
}