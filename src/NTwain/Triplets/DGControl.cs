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

  }
}