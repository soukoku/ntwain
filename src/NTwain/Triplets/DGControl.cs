using NTwain.Triplets.ControlDATs;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/>.
  /// </summary>
  public class DGControl : TripletBase
  {
    public DGControl(TwainSession session) : base(session)
    {
    }

    private Parent? _parent;
    public Parent Parent => _parent ??= new Parent(Session);

    private EntryPoint? _entryPoint;
    public EntryPoint EntryPoint => _entryPoint ??= new EntryPoint(Session);

    private Identity? _identity;
    public Identity Identity => _identity ??= new Identity(Session);

    private Status? _status;
    public Status Status => _status ??= new Status(Session);

    private StatusUtf8? _statusUtf8;
    public StatusUtf8 StatusUtf8 => _statusUtf8 ??= new StatusUtf8(Session);

    private CustomDsData? _customDsData;
    public CustomDsData CustomDsData => _customDsData ??= new CustomDsData(Session);

    private DeviceEvent? _deviceEvent;
    public DeviceEvent DeviceEvent => _deviceEvent ??= new DeviceEvent(Session);

    private Callback? _callback;
    public Callback Callback => _callback ??= new Callback(Session);

    private Callback2? _callback2;
    public Callback2 Callback2 => _callback2 ??= new Callback2(Session);

    private XferGroup? _xferGroup;
    public XferGroup XferGroup => _xferGroup ??= new XferGroup(Session);

  }
}