#if WINDOWS || NETFRAMEWORK
using System;
using System.Windows.Forms;
using System.Windows.Threading;

namespace NTwain
{
  /// <summary>
  /// An <see cref="IThreadMarshaller"/> that can be used
  /// to integrate <see cref="TwainSession"/> with
  /// an existing Winforms app.
  /// </summary>
  public class WinformMarshaller : IThreadMarshaller
  {
    private readonly Control control;

    public WinformMarshaller(System.Windows.Forms.Control control)
    {
      this.control = control;
    }

    public void BeginInvoke(Delegate work, params object[] args)
    {
      control.BeginInvoke(work, args);
    }

    public object? Invoke(Delegate work, params object[] args)
    {
      return control.Invoke(work, args);
    }
  }

  /// <summary>
  /// An <see cref="IThreadMarshaller"/> that can be used
  /// to integrate <see cref="TwainSession"/> with
  /// an existing WPF app.
  /// </summary>
  public class WpfMarshaller : IThreadMarshaller
  {
    private readonly Dispatcher dispatcher;

    public WpfMarshaller(Dispatcher dispatcher)
    {
      this.dispatcher = dispatcher;
    }

    public void BeginInvoke(Delegate work, params object[] args)
    {
      dispatcher.BeginInvoke(work, args);
    }

    public object? Invoke(Delegate work, params object[] args)
    {
      return dispatcher.Invoke(work, args);
    }
  }
}
#endif
