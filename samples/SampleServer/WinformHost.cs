using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleServer
{
  /// <summary>
  /// Hosts the winform UI message loop.
  /// </summary>
  internal class WinformHost : IHostedService
  {
    private readonly IServiceProvider services;
    private readonly IHostApplicationLifetime lifetime;
    ManualResetEvent _threadEnded;
    TwainForm? _mainWindow;

    public WinformHost(IServiceProvider services, IHostApplicationLifetime lifetime)
    {
      _threadEnded = new ManualResetEvent(false);
      this.services = services;
      this.lifetime = lifetime;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      Thread t = new(RunWinform);
      t.SetApartmentState(ApartmentState.STA);
      t.Start();

      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      if (_mainWindow != null && _mainWindow.IsHandleCreated)
      {
        _mainWindow.BeginInvoke(Application.ExitThread);
        return Task.Run(() =>
        {
          _threadEnded.WaitOne();
        });
      }
      return Task.CompletedTask;
    }

    void RunWinform()
    {
      if (DsmLoader.TryUseCustomDSM())
      {
        Debug.WriteLine("Using our own dsm now :)");
      }
      else
      {
        Debug.WriteLine("Will attempt to use default dsm :(");
      }

      // To customize application configuration such as set high DPI settings or default font,
      // see https://aka.ms/applicationconfiguration.
      ApplicationConfiguration.Initialize();
      _mainWindow = services.GetRequiredService<TwainForm>();
      Application.Run(_mainWindow);

      _threadEnded.Set();
      lifetime.StopApplication();
    }
  }
}