namespace NTwain
{
  /// <summary>
  /// General event delegate use by <see cref="TwainAppSession"/>.
  /// Better than basic object sender and requires EventArgs.
  /// </summary>
  /// <typeparam name="TEventArgs"></typeparam>
  /// <param name="sender"></param>
  /// <param name="e"></param>
  public delegate void TwainEventDelegate<TEventArgs>(TwainAppSession sender, TEventArgs e);
}
