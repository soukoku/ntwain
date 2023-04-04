using System;

namespace NTwain
{
  /// <summary>
  /// Allows work to be marshalled to a different (usually UI) thread as necessary.
  /// </summary>
  public interface IThreadMarshaller
  {
    /// <summary>
    /// Starts work asynchronously and returns immediately.
    /// </summary>
    /// <param name="work"></param>
    /// <param name="args"></param>
    void BeginInvoke(Delegate work, params object[] args);

    /// <summary>
    /// Starts work synchronously until it returns.
    /// </summary>
    /// <param name="work"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    object? Invoke(Delegate work, params object[] args);
  }

  /// <summary>
  /// No marshalling occurs. Invokes happen right in place synchronously.
  /// </summary>
  public class InPlaceMarshaller : IThreadMarshaller
  {
    /// <summary>
    /// No async invocation here.
    /// </summary>
    /// <param name="work"></param>
    /// <param name="args"></param>
    public void BeginInvoke(Delegate work, params object[] args)
    {
      work.DynamicInvoke(args);
    }

    public object? Invoke(Delegate work, params object[] args)
    {
      return work.DynamicInvoke(args);
    }
  }
}
