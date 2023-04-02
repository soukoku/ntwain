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

    private DATParent? _parent;
    public DATParent Parent => _parent ??= new DATParent(Session);

    private DATEntryPoint? _entryPoint;
    public DATEntryPoint EntryPoint => _entryPoint ??= new DATEntryPoint(Session);

    private DATIdentity? _identity;
    public DATIdentity Identity => _identity ??= new DATIdentity(Session);

    private DATStatus? _status;
    public DATStatus Status => _status ??= new DATStatus(Session);

    private DATStatusUtf8? _statusUtf8;
    public DATStatusUtf8 StatusUtf8 => _statusUtf8 ??= new DATStatusUtf8(Session);

    private DATCustomDsData? _customDsData;
    public DATCustomDsData CustomDsData => _customDsData ??= new DATCustomDsData(Session);

  }
}