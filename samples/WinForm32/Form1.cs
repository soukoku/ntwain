using Microsoft.Win32;
using NTwain;
using NTwain.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormSample
{
  public partial class Form1 : Form
  {
    bool useDiyPump = true;
    TwainAppSession twain;
    readonly string saveFolder;
    readonly Stopwatch watch = new();
    readonly ImageCodecInfo _jpegEncoder;
    readonly EncoderParameters _jpegParameters;
    readonly int _jpegQuality = 75;
    bool _useThreadForImag;
    bool _useSystemDrawing;
    bool _saveDisk;

    public Form1()
    {
      InitializeComponent();
      var libVer = FileVersionInfo.GetVersionInfo(typeof(TwainAppSession).Assembly.Location).ProductVersion;
      Text += $"{(TWPlatform.Is32bit ? " 32bit" : " 64bit")} on NTwain {libVer}";

      TWPlatform.PreferLegacyDSM = false;

      twain = new TwainAppSession(Assembly.GetExecutingAssembly().Location);
      twain.StateChanged += Twain_StateChanged;
      twain.DefaultSourceChanged += Twain_DefaultSourceChanged;
      twain.CurrentSourceChanged += Twain_CurrentSourceChanged;
      twain.SourceDisabled += Twain_SourceDisabled;
      twain.TransferReady += Twain_TransferReady;
      twain.Transferred += Twain_Transferred;
      twain.TransferError += Twain_TransferError;
      twain.DeviceEvent += Twain_DeviceEvent;

      capListView.SetDoubleBufferedAsNeeded();
      SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

      saveFolder = Path.Combine(Path.GetTempPath(), "ntwain-sample" + Path.DirectorySeparatorChar);
      Directory.CreateDirectory(saveFolder);

      this.Disposed += Form1_Disposed;


      _jpegParameters = new EncoderParameters(1);
      _jpegParameters.Param[0] = new EncoderParameter(Encoder.Quality, (long)_jpegQuality);
      _jpegEncoder = ImageCodecInfo.GetImageEncoders().First(enc => enc.FormatID == ImageFormat.Jpeg.Guid);
    }

    private void Twain_SourceDisabled(TwainAppSession sender, TW_IDENTITY_LEGACY e)
    {
      BeginInvoke(() =>
      {
        if (watch.IsRunning)
        {
          watch.Stop();
          MessageBox.Show($"Took {watch.Elapsed} to finish that transfer.");
        }
      });
    }

    private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
      switch (e.Reason)
      {
        case SessionSwitchReason.RemoteConnect:
        case SessionSwitchReason.SessionUnlock:
        case SessionSwitchReason.SessionLogon:
          capListView.SetDoubleBufferedAsNeeded();
          break;
      }
    }

    protected override async void OnHandleCreated(EventArgs e)
    {
      base.OnHandleCreated(e);

      if (useDiyPump)
      {
        var sts = await twain.OpenDSMAsync();
        Debug.WriteLine($"OpenDSMAsync={sts}");
      }
      else
      {
        var hwnd = this.Handle;
        var sts = twain.OpenDSM(hwnd, SynchronizationContext.Current!);
        twain.AddWinformFilter();
        Debug.WriteLine($"OpenDSM={sts}");
      }
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      var finalState = twain.TryStepdown(STATE.S2);
      Debug.WriteLine($"Stepdown result state={finalState}");
      twain.RemoveWinformFilter();
      base.OnClosing(e);
    }

    private void Form1_Disposed(object? sender, EventArgs e)
    {
      twain.Dispose();
    }


    private void Twain_DeviceEvent(TwainAppSession sender, TW_DEVICEEVENT e)
    {
      Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] device event {e}.");

    }

    private void Twain_TransferError(TwainAppSession sender, TransferErrorEventArgs e)
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

    private void Twain_Transferred(TwainAppSession sender, TransferredEventArgs e)
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
      if (e.Data != null)
      {
        try
        {
          // example of using some lib to handle image data
          var saveFile = Path.Combine(saveFolder, (DateTime.Now.Ticks / 1000).ToString());

          if (_useSystemDrawing)
          {
            using (var img = Image.FromStream(e.Data.AsStream()))
            {
              if (img.PixelFormat == System.Drawing.Imaging.PixelFormat.Format1bppIndexed ||
                img.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
              {
                // bw or gray
                saveFile += ".png";
                if (_saveDisk) img.Save(saveFile, ImageFormat.Png);
                else img.Save(new NoOpStream(), ImageFormat.Png);
              }
              else
              {
                // color
                saveFile += ".jpg";
                if (_saveDisk) img.Save(saveFile, _jpegEncoder, _jpegParameters);
                else img.Save(new NoOpStream(), _jpegEncoder, _jpegParameters);
              }
            }
          }
          else
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
              if (_saveDisk) img.Write(saveFile);
              else img.Write(new NoOpStream(), format);
            }
            Debug.WriteLine($"Saved image to {saveFile}");
          }
        }
        catch { }
        finally
        {
          e.Dispose();
        }
      }
    }

    private void Twain_TransferReady(TwainAppSession sender, TransferReadyEventArgs e)
    {
      Debug.WriteLine($"[thread {Environment.CurrentManagedThreadId}] transfer ready.");
    }

    private void Twain_DefaultSourceChanged(TwainAppSession sender, TW_IDENTITY_LEGACY ds)
    {
      BeginInvoke(() => lblDefault.Text = ds.ProductName);
    }

    private void Twain_StateChanged(TwainAppSession sender, STATE state)
    {
      BeginInvoke(() => lblState.Text = state.ToString());
    }

    private void Twain_CurrentSourceChanged(TwainAppSession sender, TW_IDENTITY_LEGACY ds)
    {
      BeginInvoke(() =>
      {
        lblCurrent.Text = ds.ToString();
        if (twain.State == STATE.S4)
        {
          LoadCapInfoList();

          // never seen a driver support these but here it is to test it
          if (twain.GetCapLabel(CAP.ICAP_SUPPORTEDSIZES, out string? test).RC == TWRC.SUCCESS)
          {
            Debug.WriteLine($"Supported sizes label from ds = {test}");
          }
          if (twain.GetCapHelp(CAP.ICAP_SUPPORTEDSIZES, out string? test2).RC == TWRC.SUCCESS)
          {
            Debug.WriteLine($"Supported sizes help from ds = {test2}");
          }
          if (twain.GetCapLabelEnum(CAP.ICAP_SUPPORTEDSIZES, out IList<string>? test3).RC == TWRC.SUCCESS && test3 != null)
          {
            Debug.WriteLine($"Supported sizes label enum from ds = {string.Join(Environment.NewLine, test3)}");
          }
        }
        else
        {
          capListView.Items.Clear();
        }
      });
    }

    private void LoadCapInfoList()
    {
      twain.GetCapValues(CAP.CAP_SUPPORTEDCAPS, out IList<CAP> caps);
      twain.GetCapValues(CAP.CAP_EXTENDEDCAPS, out IList<CAP> extended);
      foreach (var c in caps)
      {
        ListViewItem it = new(GetFriendlyName(c));

        if (twain.GetCapCurrent(c, out TW_CAPABILITY twcap).RC == TWRC.SUCCESS)
        {
          var enumType = SizeAndConversionUtils.GetEnumType(c);
          var realType = twcap.DetermineValueType(twain);
          it.SubItems.Add(enumType?.Name.ToString() ?? realType.ToString());
          it.SubItems.Add(ReadTypedValue(c, enumType, realType, forCurrent: true));
          it.SubItems.Add(ReadTypedValue(c, enumType, realType, forCurrent: false));
        }
        else
        {
          it.SubItems.Add("");
          it.SubItems.Add("");
          it.SubItems.Add("");
        }
        it.SubItems.Add(extended.Contains(c).ToString());
        var supports = twain.QueryCapSupport(c);
        it.SubItems.Add(supports.ToString());
        if (!supports.HasFlag(TWQC.SET)) it.ForeColor = Color.Gray;
        capListView.Items.Add(it);
      }
    }

    private string GetFriendlyName(CAP c)
    {
      if (c > CAP.CAP_CUSTOMBASE)
      {
        return $"{CAP.CAP_CUSTOMBASE} + {c - CAP.CAP_CUSTOMBASE}";
      }
      return c.ToString();
    }

    // there may be a better way...
    MethodInfo[] twainMethods = typeof(TwainAppSession).GetMethods();

    private string ReadTypedValue(CAP cap, Type? enumType, TWTY type, bool forCurrent)
    {
      if (enumType != null)
      {
        if (forCurrent)
        {
          var currentMethod = twainMethods
            .FirstOrDefault(m => m.Name == nameof(TwainAppSession.GetCapCurrent) && m.IsGenericMethod)!
            .MakeGenericMethod(enumType);
          var args = new object?[] { cap, null };
          currentMethod.Invoke(twain, args);
          var values = (System.Collections.IList)args[1]!;
          if (values.Count == 1)
          {
            return values[0]!.ToString()!;
          }
          else if (values.Count > 1)
          {
            return string.Join(", ", values);
          }
          return "";
        }
        else
        {
          var defaultMethod = twainMethods
            .FirstOrDefault(m => m.Name == nameof(TwainAppSession.GetCapDefault) && m.IsGenericMethod)!
            .MakeGenericMethod(enumType);
          var args = new object?[] { cap, null };
          defaultMethod.Invoke(twain, args);
          var values = (System.Collections.IList)args[1]!;
          if (values.Count == 1)
          {
            return values[0]!.ToString()!;
          }
          else if (values.Count > 1)
          {
            return string.Join(", ", values);
          }
          return "";
        }
      }

      STS sts = default;
      switch (type)
      {
        case TWTY.UINT8:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<byte> ubval) :
            twain.GetCapDefault(cap, out ubval);
          return ubval.FirstOrDefault().ToString();
        case TWTY.INT8:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<sbyte> sbval) :
            twain.GetCapDefault(cap, out sbval);
          return sbval.FirstOrDefault().ToString();
        case TWTY.UINT16:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<ushort> usval) :
            twain.GetCapDefault(cap, out usval);
          return usval.FirstOrDefault().ToString();
        case TWTY.INT16:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<short> ssval) :
            twain.GetCapDefault(cap, out ssval);
          return ssval.FirstOrDefault().ToString();
        case TWTY.UINT32:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<uint> uival) :
            twain.GetCapDefault(cap, out uival);
          return uival.FirstOrDefault().ToString();
        case TWTY.INT32:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<int> sival) :
            twain.GetCapDefault(cap, out sival);
          return sival.FirstOrDefault().ToString();
        case TWTY.BOOL:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<TW_BOOL> tbval) :
            twain.GetCapDefault(cap, out tbval);
          return tbval.FirstOrDefault().ToString();
        case TWTY.FIX32:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<TW_FIX32> fxval) :
            twain.GetCapDefault(cap, out fxval);
          return fxval.FirstOrDefault().ToString();
        case TWTY.FRAME:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<TW_FRAME> frval) :
            twain.GetCapDefault(cap, out frval);
          return frval.FirstOrDefault().ToString();
        case TWTY.STR32:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<TW_STR32> s32val) :
            twain.GetCapDefault(cap, out s32val);
          return s32val.FirstOrDefault().ToString();
        case TWTY.STR64:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<TW_STR64> s64val) :
            twain.GetCapDefault(cap, out s64val);
          return s64val.FirstOrDefault().ToString();
        case TWTY.STR128:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<TW_STR128> s128val) :
            twain.GetCapDefault(cap, out s128val);
          return s128val.FirstOrDefault().ToString();
        case TWTY.STR255:
          sts = forCurrent ?
            twain.GetCapCurrent(cap, out List<TW_STR255> s255val) :
            twain.GetCapDefault(cap, out s255val);
          return s255val.FirstOrDefault().ToString();
        case TWTY.HANDLE:
          break;
      }
      Debug.WriteLine($"{nameof(ReadTypedValue)}({cap}, {type}, {forCurrent}) => {sts}");
      return "";
    }

    private void btnSelect_Click(object sender, EventArgs e)
    {
      twain.ShowUserSelect();
    }

    private void btnEnumSources_Click(object sender, EventArgs e)
    {
      listSources.Items.Clear();
      foreach (var ds in twain.GetSources())
      {
        listSources.Items.Add(ds);
      }
    }

    private void btnSetDef_Click(object sender, EventArgs e)
    {
      if (listSources.SelectedItem is TW_IDENTITY_LEGACY ds)
      {
        twain.SetDefaultSource(ds);
      }
    }

    private void btnOpen_Click(object sender, EventArgs e)
    {
      if (listSources.SelectedItem is TW_IDENTITY_LEGACY ds)
      {
        twain.TryStepdown(STATE.S3);

        twain.OpenSource(ds);
      }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
      twain.CloseSource();
    }

    private void btnOpenDef_Click(object sender, EventArgs e)
    {
      twain.TryStepdown(STATE.S3);

      twain.OpenSource(twain.DefaultSource);
    }

    private void btnShowSettings_Click(object sender, EventArgs e)
    {
      twain.EnableSource(true, true);
    }

    private void btnStart_Click(object sender, EventArgs e)
    {
      if (twain.EnableSource(ckShowUI.Checked, false).IsSuccess)
      {
        _useThreadForImag = ckBgImageHandling.Checked;
        _useSystemDrawing = ckSystemDrawing.Checked;
        _saveDisk = ckSaveDisk.Checked;
        watch.Restart();
      }
    }

    private void btnOpenFolder_Click(object sender, EventArgs e)
    {
      try
      {
        if (!Directory.Exists(saveFolder)) Directory.CreateDirectory(saveFolder);
        using (Process.Start(new ProcessStartInfo { FileName = saveFolder, UseShellExecute = true })) { }
      }
      catch { }
    }
  }
}