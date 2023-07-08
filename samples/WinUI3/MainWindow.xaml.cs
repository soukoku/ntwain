using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NTwain;
using NTwain.Data;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3
{
  /// <summary>
  /// An empty window that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainWindow : Window
  {
    TwainAppSession session;
    readonly int _jpegQuality = 75;
    readonly string saveFolder;
    readonly Stopwatch watch = new();
    readonly ImageCodecInfo _jpegEncoder;
    readonly EncoderParameters _jpegParameters;
    bool _useThreadForImag;
    bool _useSystemDrawing;
    bool _saveDisk;

    public MainWindow()
    {
      this.InitializeComponent();

      var libVer = FileVersionInfo.GetVersionInfo(typeof(TwainAppSession).Assembly.Location).ProductVersion;
      Title = $"WinUI3 sample {(TWPlatform.Is32bit ? " 32bit" : " 64bit")} on NTwain {libVer}";

      saveFolder = Path.Combine(Path.GetTempPath(), "ntwain-sample" + Path.DirectorySeparatorChar);
      Directory.CreateDirectory(saveFolder);

      session = new TwainAppSession();

      session.StateChanged += Session_StateChanged;
      session.SourceDisabled += Session_SourceDisabled;
      session.TransferReady += Session_TransferReady;
      session.Transferred += Session_Transferred;
      session.TransferError += Session_TransferError;

      this.Closed += MainWindow_Closed;

      _jpegParameters = new EncoderParameters(1);
      _jpegParameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)_jpegQuality);
      _jpegEncoder = ImageCodecInfo.GetImageEncoders().First(enc => enc.FormatID == ImageFormat.Jpeg.Guid);
    }

    private void Session_TransferError(TwainAppSession sender, TransferErrorEventArgs e)
    {
      if (e.Exception != null)
      {
        Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] transfer error {e.Exception}.");
      }
      else
      {
        Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] transfer error {e.Code}.");
      }
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
      session.TryStepdown(STATE.S2);
    }

    private void Session_TransferReady(TwainAppSession sender, TransferReadyEventArgs e)
    {
      Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] transfer ready.");
    }
    private void Session_Transferred(TwainAppSession sender, TransferredEventArgs e)
    {
      Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] data transferred with info {e.ImageInfo}");
      // if using a high-speed scanner, imaging handling could be a bottleneck
      // so it's possible to pass the data to another thread while the scanning
      // loop happens. Just remember to dispose it after.

      if (_useThreadForImag)
      {
        // bad thread example but whatev. should use a dedicated thread of some sort for real
        Task.Run(() =>
        {
          HandleTransferredData(e);
        });
      }
      else
      {
        HandleTransferredData(e);
      }
    }
    private void HandleTransferredData(TransferredEventArgs e)
    {
      try
      {
        // example of using some lib to handle image data
        var saveFile = Path.Combine(saveFolder, (DateTime.Now.Ticks / 1000).ToString());
        if (_useSystemDrawing)
        {
          using (var img = System.Drawing.Image.FromStream(e.Data.AsStream()))
          {
            if (img.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed ||
              img.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
            {
              // bw or gray
              saveFile += ".png";
              if (_saveDisk)
              {
                img.Save(saveFile, ImageFormat.Png);
                Debug.WriteLine($"Saved image to {saveFile}");
              }
              else img.Save(new NoOpStream(), ImageFormat.Png);
            }
            else
            {
              // color
              saveFile += ".jpg";
              if (_saveDisk)
              {
                img.Save(saveFile, _jpegEncoder, _jpegParameters);
                Debug.WriteLine($"Saved image to {saveFile}");
              }
              else img.Save(new NoOpStream(), _jpegEncoder, _jpegParameters);
            }
          }
        }
        else
        {
          using (var stream = e.Data?.AsStream())
          {
            using (var img = new ImageMagick.MagickImage(e.Data.AsSpan()))
            {
              var format = ImageMagick.MagickFormat.Png;
              if (img.ColorType == ImageMagick.ColorType.Palette)
              {
                // bw or gray
                saveFile += ".png";
              }
              else
              {
                // color
                saveFile += ".jpg";
                format = ImageMagick.MagickFormat.Jpeg;
                img.Quality = _jpegQuality;
              }
              if (_saveDisk)
              {
                img.Write(saveFile, format);
                Debug.WriteLine($"Saved image to {saveFile}");
              }
              else img.Write(new NoOpStream(), format);
            }
          }
        }
      }
      catch { }
      finally
      {
        e.Dispose();
      }
    }

    private void Session_SourceDisabled(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
      session.CloseSource();
      DispatcherQueue.TryEnqueue(() =>
      {
        if (watch.IsRunning)
        {
          watch.Stop();
          var dlg = new ContentDialog
          {
            Title = "Completed",
            Content = $"Took {watch.Elapsed} to finish that transfer.",
            CloseButtonText = "OK",
            XamlRoot = Content.XamlRoot
          };
          _ = dlg.ShowAsync();

          if (_saveDisk)
          {
            try
            {
              if (Directory.Exists(saveFolder))
              {
                using (Process.Start(new ProcessStartInfo { FileName = saveFolder, UseShellExecute = true })) { }
              }
            }
            catch { }
          }
        }
      });
    }

    private void Session_StateChanged(TwainAppSession sender, STATE e)
    {

    }

    private async void myButton_Click(object sender, RoutedEventArgs e)
    {
      if (session.State < STATE.S3)
        await session.OpenDSMAsync();

      if (session.ShowUserSelect().IsSuccess)
      {
        if (session.OpenSource(session.CurrentSource).IsSuccess)
        {
          if (session.EnableSource(true, false).IsSuccess)
          {
            _saveDisk = true;
            watch.Restart();
          }
        }
      }
    }
  }
}
