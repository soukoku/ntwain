#if !NET10_0_OR_GREATER
namespace NTwain.Events;

/// <summary>
/// An event handler delegate with typed sender and arguments.
/// </summary>
/// <typeparam name="TSender"></typeparam>
/// <typeparam name="TArgs"></typeparam>
/// <param name="sender"></param>
/// <param name="args"></param>
public delegate void EventHandler<in TSender, in TArgs>(TSender sender, TArgs args);
#endif