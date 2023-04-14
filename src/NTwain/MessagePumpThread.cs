#if WINDOWS || NETFRAMEWORK
using NTwain.Data;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NTwain
{
  /// <summary>
  /// For use under Windows to host a message pump in non-winform/wpf apps.
  /// This is highly experimental.
  /// </summary>
  public class MessagePumpThread
  {
    DummyForm? _dummyForm;
    TwainAppSession? _twain;

    public bool IsRunning => _dummyForm != null && _dummyForm.IsHandleCreated;

    /// <summary>
    /// Starts the thread, attaches a twain session to it,
    /// and opens the DSM.
    /// </summary>
    /// <param name="twain"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<STS> AttachAsync(TwainAppSession twain)
    {
      if (twain.State > STATE.S2) throw new InvalidOperationException("Cannot attach to an opened TWAIN session.");
      if (_twain != null || _dummyForm != null) throw new InvalidOperationException("Already attached previously.");

      Thread t = new(RunMessagePump);
      t.IsBackground = true;
      t.SetApartmentState(ApartmentState.STA);
      t.Start();

      while (_dummyForm == null || !_dummyForm.IsHandleCreated)
      {
        await Task.Delay(50);
      }

      STS sts = default;
      TaskCompletionSource<bool> tcs = new();
      _dummyForm.BeginInvoke(() =>
      {
        try
        {
          sts = twain.OpenDSM(_dummyForm.Handle, SynchronizationContext.Current!);
          if (sts.IsSuccess)
          {
            twain.AddWinformFilter();
            _twain = twain;
          }
          else
          {
            _dummyForm.Close();
          }
        }
        finally
        {
          tcs.TrySetResult(true);
        }
      });
      await tcs.Task;
      return sts;
    }

    /// <summary>
    /// Detatches a previously attached session and stops the thread.
    /// </summary>
    public void Detatch()
    {
      if (_dummyForm != null && _twain != null)
      {
        _dummyForm.BeginInvoke(_dummyForm.Close);
      }
    }

    void RunMessagePump()
    {
      Debug.WriteLine("TWAIN pump thread starting");
      _dummyForm = new DummyForm();
      _dummyForm.FormClosed += (s, e) =>
      {
        if (_twain != null)
        {
          _twain.TryStepdown(STATE.S4);
          _twain.RemoveWinformFilter();
          _twain.CloseDSM();
          _twain = null;
        }
        _dummyForm = null;
      };
      Application.Run(_dummyForm);
      Debug.WriteLine("TWAIN pump thread ended");
    }

    class DummyForm : Form
    {
      public DummyForm()
      {
        ShowInTaskbar = false;
        WindowState = FormWindowState.Minimized;
      }
    }
  }
}
#endif