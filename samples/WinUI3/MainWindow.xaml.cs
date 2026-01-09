using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NTwain;
using NTwain.Data;
using NTwain.Events;
using System;
using System.Diagnostics;
using System.IO;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUI3;
/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    TwainAppSession session;
    readonly uint _jpegQuality = 85;
    readonly string saveFolder;
    readonly Stopwatch watch = new();

    public MainWindow()
    {
        InitializeComponent();

        var libVer = FileVersionInfo.GetVersionInfo(typeof(TwainAppSession).Assembly.Location).ProductVersion;
        Title = $"WinUI3 sample {(Environment.Is64BitProcess ? " 64bit" : " 32bit")} on NTwain {libVer}";

        saveFolder = Path.Combine(Path.GetTempPath(), "ntwain-sample" + Path.DirectorySeparatorChar);
        Directory.CreateDirectory(saveFolder);

        session = new TwainAppSession();

        session.StateChanged += Session_StateChanged;
        session.SourceDisabled += Session_SourceDisabled;
        session.TransferReady += Session_TransferReady;
        session.Transferred += Session_Transferred;
        session.TransferError += Session_TransferError;

        this.Closed += MainWindow_Closed;
    }

    private void testButton_Click(object sender, RoutedEventArgs e)
    {
        if (session.State < STATE.S3)
            session.OpenDsm();

        if (session.ShowUserSelect().IsSuccess)
        {
            if (session.OpenSource(session.DefaultSource!).IsSuccess)
            {
                if (session.EnableSource(SourceEnableOption.ShowUI).IsSuccess)
                {
                    watch.Restart();
                }
            }
        }
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
        session.Dispose();
    }

    private void Session_TransferReady(TwainAppSession sender, TransferReadyEventArgs e)
    {
        Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] transfer ready.");
    }
    private void Session_Transferred(TwainAppSession sender, TransferredEventArgs e)
    {
        try
        {
            // example of using some lib to handle image data
            var saveFile = Path.Combine(saveFolder, (DateTime.Now.Ticks / 1000).ToString());
            if (e.Data != null)
            {
                using var img = new ImageMagick.MagickImage(e.Data.AsSpan());
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
                img.Write(saveFile, format);
                Debug.WriteLine($"Saved image to {saveFile}");
            }
        }
        catch { }
        finally
        {
            e.Dispose();
        }
    }

    private void Session_SourceDisabled(TwainAppSession sender, TWIdentityWrapper e)
    {
        session.CloseSource();
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

    private void Session_StateChanged(TwainAppSession sender, STATE e)
    {

    }

}
