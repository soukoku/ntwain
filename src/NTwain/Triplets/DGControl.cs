using NTwain.Triplets.ControlDATs;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains triplet calls starting with <see cref="DG.CONTROL"/>.
  /// </summary>
  public static class DGControl
  {
    public static readonly Parent Parent = new();

    public static readonly EntryPoint EntryPoint = new();

    public static readonly Identity Identity = new();

    public static readonly Status Status = new();

    public static readonly StatusUtf8 StatusUtf8 = new();

    public static readonly CustomDsData CustomDsData = new();

    public static readonly DeviceEvent DeviceEvent = new();

    public static readonly Callback Callback = new();

    public static readonly Callback2 Callback2 = new();

    public static readonly XferGroup XferGroup = new();

    public static readonly UserInterface UserInterface = new();

    public static readonly Event Event = new();

    public static readonly PendingXfers PendingXfers = new();

  }
}