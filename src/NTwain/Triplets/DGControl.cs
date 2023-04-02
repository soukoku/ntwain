using NTwain.Triplets.ControlDATs;

namespace NTwain.Triplets
{
  /// <summary>
  /// Contains calls used with <see cref="DG.CONTROL"/>.
  /// </summary>
  public class DGControl
  {
    private Parent? _parent;
    public Parent Parent => _parent ??= new Parent();

    private EntryPoint? _entryPoint;
    public EntryPoint EntryPoint => _entryPoint ??= new EntryPoint();

    private Identity? _identity;
    public Identity Identity => _identity ??= new Identity();

    private Status? _status;
    public Status Status => _status ??= new Status();

    private StatusUtf8? _statusUtf8;
    public StatusUtf8 StatusUtf8 => _statusUtf8 ??= new StatusUtf8();

    private CustomDsData? _customDsData;
    public CustomDsData CustomDsData => _customDsData ??= new CustomDsData();

    private DeviceEvent? _deviceEvent;
    public DeviceEvent DeviceEvent => _deviceEvent ??= new DeviceEvent();

    private Callback? _callback;
    public Callback Callback => _callback ??= new Callback();

    private Callback2? _callback2;
    public Callback2 Callback2 => _callback2 ??= new Callback2();

    private XferGroup? _xferGroup;
    public XferGroup XferGroup => _xferGroup ??= new XferGroup();

    private UserInterface? _userInterface;
    public UserInterface UserInterface => _userInterface ??= new UserInterface();

  }
}