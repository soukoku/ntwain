using NTwain;
using NTwain.Data;
using NTwain.Data.Kds;
using NTwain.Events;
using NTwain.Triplets;
using System.Diagnostics;
using System.Text;

namespace KodakTester;

public partial class MainForm : Form
{
    TwainAppSession _twain;
    private bool _stopTransfer;

    public MainForm()
    {
        InitializeComponent();

        _twain = new TwainAppSession(appThreadContext: SynchronizationContext.Current);
        _twain.TransferReady += _twain_TransferReady;
        _twain.Transferred += _twain_Transferred;
        _twain.SourceDisabled += _twain_SourceDisabled;
    }

    private void _twain_SourceDisabled(TwainAppSession sender, TWIdentityWrapper e)
    {

    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        _twain.OpenDsm();
        //_ = _twain.OpenDSMAsync();
    }

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _twain.CloseDsm();
        //_ = _twain.CloseDSMAsync();
        base.OnFormClosed(e);
    }

    private void _twain_Transferred(TwainAppSession sender, TransferredEventArgs e)
    {
        TW_EXTIMAGEINFO extInfo = TW_EXTIMAGEINFO.CreateRequest(TWEI.CAMERA);
        e.GetExtendedImageInfo(ref extInfo);
        string? camera = null;
        foreach (var ei in extInfo.AsInfos())
        {
            if (ei.ReturnCode == TWRC.SUCCESS)
            {
                switch (ei.InfoId)
                {
                    //case (TWEI)KDS_TWEI.HDR_PAGENUMBER:
                    //    LogIt($"TWEI_HDR_PAGENUMBER Value = {ei.ReadNonPointerData<int>()}");
                    //    break;
                    case TWEI.CAMERA:
                        camera = ei.ReadHandleString(_twain.MemoryManager);
                        LogIt($"{ei.InfoId} Value = {camera}");
                        break;
                }
            }
            else
            {
                LogIt($"{ei.InfoId} RC = {ei.ReturnCode}");
            }
        }
        extInfo.Free(_twain.MemoryManager);

        if (e.Data != null)
        {
            LogIt($"Received {e.ImageInfo.PixelType} in {e.ImageInfo.Compression} compressed memory image.");
            var folder = boxFolder.Text;
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = "Images";
                Directory.CreateDirectory(folder);
                boxFolder.Text = folder;
            }
            var prefix = boxNamePrefix.Text;
            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = "Capture_";
                boxNamePrefix.Text = prefix;
            }

            using (var img = System.Drawing.Image.FromStream(e.Data.AsStream()))
            {
                //var saveFile = img.SaveToSmallestFormat(folder, prefix, lossless: false).ToString();
                //LogIt($"File saved to {saveFile}");
            }
        }
        else if (e.FileInfo != null)
        {
            var info = e.FileInfo.Value;
            var path = info.FileName.ToString();
            LogIt($"Received {e.ImageInfo.PixelType} {info.Format} in {e.ImageInfo.Compression} compressed file {path}");
        }
        e.Dispose();
        LogIt("");
    }

    int _xferCount = 0;
    private void _twain_TransferReady(TwainAppSession sender, TransferReadyEventArgs e)
    {
        if (_stopTransfer)
        {
            e.Cancel = CancelType.EndNow;
            return;
        }

        _xferCount++;
        LogIt($"Got pending transfer with mode = {e.ImgXferMech}");
        if (e.ImgXferMech == TWSX.FILE)
        {
            var folder = boxFolder.Text;
            if (string.IsNullOrWhiteSpace(folder))
            {
                folder = "Images";
                Directory.CreateDirectory(folder);
                boxFolder.Text = folder;
            }
            var prefix = boxNamePrefix.Text;
            if (string.IsNullOrWhiteSpace(prefix))
            {
                prefix = "Capture_";
                boxNamePrefix.Text = prefix;
            }

            TWCP comp = TWCP.NONE;
            TW_EXTIMAGEINFO extInfo = TW_EXTIMAGEINFO.CreateRequest((TWEI)KDS_TWEI.HDR_COMPRESSION);
            e.GetExtendedImageInfo(ref extInfo);
            foreach (var ei in extInfo.AsInfos())
            {
                if (ei.ReturnCode == TWRC.SUCCESS)
                {
                    switch (ei.InfoId)
                    {
                        case (TWEI)KDS_TWEI.HDR_COMPRESSION:
                            comp = ei.ReadNonPointerData<TWCP>();
                            LogIt($"{ei.InfoId} Value = {comp}");
                            break;
                    }
                }
                else
                {
                    LogIt($"{ei.InfoId} RC = {ei.ReturnCode}");
                }
            }
            extInfo.Free(_twain.MemoryManager);

            LogIt($"Compression at ready step = {comp}");
            string? targetName = $"{prefix}_{_xferCount:D4}";
            TWFF format = TWFF.TIFF;
            switch (comp)
            {
                case TWCP.JPEG:
                    targetName = $"{prefix}_{_xferCount:D4}.jpg";
                    format = TWFF.JFIF;
                    break;
                case TWCP.NONE:
                    targetName = $"{prefix}_{_xferCount:D4}.bmp";
                    format = TWFF.BMP;
                    break;
                case TWCP.GROUP4:
                default:
                    targetName = $"{prefix}_{_xferCount:D4}.tif";
                    break;
            }
            TW_SETUPFILEXFER setup = new()
            {
                FileName = Path.Combine(folder, targetName),
                Format = format
            };

            var sts = e.SetupFileTransfer(ref setup);

            LogIt($"Want to save image as {setup.Format} {setup.FileName}.", sts);

            // verify it again
            var appId = _twain.AppIdentity;
            var srcId = _twain.CurrentSource!;

            sts = _twain.WrapInSTS(DGControl.SetupFileXfer.Get(appId, srcId, out setup));

            LogIt($"Checked actual file settings as {setup.Format} {setup.FileName}.", sts);

        }
        LogIt("");
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        if (_twain.State > STATE.S5)
        {
            e.Cancel = true;
        }
        _twain.TryStepdown(STATE.S2);
        base.OnFormClosing(e);
    }

    private void btnSelectScanner_Click(object sender, EventArgs e)
    {
        var sts = _twain.ShowUserSelect();
        if (sts.IsSuccess && _twain.DefaultSource.Id > 0)
        {
            if (_twain.State > STATE.S3)
            {
                _twain.TryStepdown(STATE.S3);
            }
            sts = _twain.OpenSource(_twain.DefaultSource);
            LogIt("Open scanner", sts);
            if (sts.IsSuccess)
            {
                var src = _twain.CurrentSource;
                lblCurScanner.Text = $"{src.ProductName} | v{src.Version} | protocol: {src.ProtocolMajor}.{src.ProtocolMinor}";
                LoadSettings();
                return;
            }
        }

        lblCurScanner.Text = "None selected";
    }

    private void LogIt(string msg)
    {
        boxLog.AppendText($"{msg}\n");
    }
    private void LogIt(string msg, STS sts)
    {
        if (sts.IsSuccess)
        {
            boxLog.AppendText($"{msg} result = {sts.RC}\n");
        }
        else
        {
            boxLog.AppendText($"{msg} result = {sts.RC} - {sts.ConditionCode}\n");
        }
    }

    private void LoadSettings()
    {
        var mechs = _twain.Caps.ICAP_XFERMECH.Get().GetValues();

        if (!mechs.Contains(TWSX.FILE))
        {
            LogIt("File transfer is not supported.");
        }

        var sts = _twain.Caps.ICAP_XFERMECH.Set(TWSX.FILE);
        LogIt("Use file transfer", sts);

        if (_twain.Caps.ICAP_UNITS.GetCurrent().FirstOrDefault() != TWUN.INCHES)
        {
            sts = _twain.Caps.ICAP_UNITS.Set(TWUN.INCHES);
            LogIt("Set unit to inches", sts);
        }

        var dpis = _twain.Caps.ICAP_XRESOLUTION.Get().GetValues();
        listDpi.Items.Clear();
        if (dpis.Contains(200))
        {
            listDpi.Items.Add(200);
            listDpi.SelectedItem = 200;
        }
        else
        {
            LogIt("200 DPI doesn't appear to be supported.");
        }
        if (dpis.Contains(300))
        {
            listDpi.Items.Add(300);
            listDpi.SelectedItem = 300;
        }
        else
        {
            LogIt("300 DPI doesn't appear to be supported.");
        }

        var formats = _twain.Caps.ICAP_IMAGEFILEFORMAT.Get().GetValues();
        listFormat.Items.Clear();
        foreach (var format in formats)
        {
            listFormat.Items.Add(format);
        }
        listFormat.SelectedItem = _twain.Caps.ICAP_IMAGEFILEFORMAT.GetCurrent().FirstOrDefault();

        LogIt("");
    }

    private void btnDriverOnly_Click(object sender, EventArgs e)
    {
        if (_twain.State != STATE.S4) return;

        var sts = _twain.EnableSource(SourceEnableOption.UIOnly);
        LogIt("Show drivers", sts);
        LogIt("");
    }

    private void btnTransfer_Click(object sender, EventArgs e)
    {
        if (_twain.State != STATE.S4 || !EnsureBoxFolder()) return;


        var _isIndustrialKodak = _twain.CurrentSource!.ProductName.ToString().StartsWith("KODAK Scanner: i");

        if (_isIndustrialKodak)
        {
            CaptureAsKodakSDMI();
        }
        else
        {
            CaptureAsStandardScanner();
        }

        _stopTransfer = false;
        _xferCount = 0;
        var sts = _twain.EnableSource(ckShowUI.Checked ? SourceEnableOption.ShowUI : SourceEnableOption.NoUI);
        LogIt("Start capture", sts);
        LogIt("");
    }

    private void CaptureAsKodakSDMI()
    {
        LogIt("Attempting Kodak SDMI mode");

        LogIt($"Resolution supports {FlattenFlag(_twain.Caps.ICAP_XRESOLUTION.Supports)}");
        LogIt($"File format supports {FlattenFlag(_twain.Caps.ICAP_IMAGEFILEFORMAT.Supports)}");
        LogIt($"Compression supports {FlattenFlag(_twain.Caps.ICAP_COMPRESSION.Supports)}");
        LogIt($"EXTINFO supports {FlattenFlag(_twain.Caps.ICAP_EXTIMAGEINFO.Supports)}");

        LogIt("");

        var limit = (short)boxLimit.Value;
        if (limit > 0) limit *= 2;
        var sts = _twain.Caps.CAP_XFERCOUNT.Set(limit);
        LogIt($"Set transfer limit {limit}", sts);

        sts = _twain.Caps.ICAP_EXTIMAGEINFO.Set(TW_BOOL.True);
        LogIt($"Set extimageinfo enabled", sts);

        var format = (TWFF)listFormat.SelectedItem!;
        sts = _twain.Caps.ICAP_IMAGEFILEFORMAT.Set(format);
        LogIt($"Set {format} format.", sts);

        if (!sts.IsSuccess)
        {
            return;
        }

        LogIt("");

        var appId = _twain.AppIdentity;
        var srcId = _twain.CurrentSource!;


        TW_FILESYSTEM fs = new() { InputName = "/Camera_Bitonal_Both" };
        sts = _twain.WrapInSTS(DGControl.FileSystem.ChangeDirectory(appId, srcId, ref fs));
        LogIt("Change to bw cameras", sts);
        if (sts.IsSuccess)
        {
            sts = _twain.Caps.CAP_CAMERAENABLED.Set(TW_BOOL.True);
            LogIt("Set camera enabled", sts);

            var dpi = listDpi.SelectedValue == null ? 300 : Convert.ToInt32(listDpi.SelectedValue);
            sts = _twain.Caps.ICAP_XRESOLUTION.Set(dpi);
            LogIt("Set x-resolution", sts);
            sts = _twain.Caps.ICAP_YRESOLUTION.Set(dpi);
            LogIt("Set y-resolution", sts);


            if (format != TWFF.BMP)
            {
                LogIt($"Current format={_twain.Caps.ICAP_IMAGEFILEFORMAT.GetCurrent().First()}");
                LogIt($"Current compression={_twain.Caps.ICAP_COMPRESSION.GetCurrent().First()}");
                if (_twain.Caps.ICAP_COMPRESSION.Supports.HasFlag(TWQC.SET))
                {
                    sts = _twain.Caps.ICAP_COMPRESSION.Set(TWCP.GROUP4);
                    LogIt("Set compression to group4", sts);
                }
            }
        }
        LogIt("");


        fs = new() { FileType = (int)TWFY.CAMERA, InputName = "/Camera_Color_Both" };
        sts = _twain.WrapInSTS(DGControl.FileSystem.ChangeDirectory(appId, srcId, ref fs));
        LogIt("Change to color cameras", sts);
        if (sts.IsSuccess)
        {
            sts = _twain.Caps.CAP_CAMERAENABLED.Set(TW_BOOL.True);
            LogIt("Set camera enabled", sts);


            var dpi = listDpi.SelectedValue == null ? 300 : Convert.ToInt32(listDpi.SelectedValue);
            sts = _twain.Caps.ICAP_XRESOLUTION.Set(dpi);
            LogIt("Set x-resolution", sts);
            sts = _twain.Caps.ICAP_YRESOLUTION.Set(dpi);
            LogIt("Set y-resolution", sts);


            if (format != TWFF.BMP)
            {
                LogIt($"Current format={_twain.Caps.ICAP_IMAGEFILEFORMAT.GetCurrent().First()}");
                LogIt($"Current compression={_twain.Caps.ICAP_COMPRESSION.GetCurrent().First()}");
                if (_twain.Caps.ICAP_COMPRESSION.Supports.HasFlag(TWQC.SET))
                {
                    sts = _twain.Caps.ICAP_COMPRESSION.Set(TWCP.JPEG);
                    LogIt("Set compression to jpg", sts);
                    if (sts.IsSuccess)
                    {
                        LogIt($"jpeg quality supports {FlattenFlag(_twain.Caps.ICAP_JPEGQUALITY.Supports)}");

                        short quality = 90;
                        sts = _twain.Caps.ICAP_JPEGQUALITY.Set((TWJQ)quality);
                        LogIt($"Set jpg quality to {quality}", sts);
                        if (!sts.IsSuccess)
                        {
                            quality = 85;
                            sts = _twain.Caps.ICAP_JPEGQUALITY.Set((TWJQ)quality);
                            LogIt($"Set jpg quality to {quality}", sts);

                            if (!sts.IsSuccess)
                            {
                                sts = _twain.Caps.ICAP_JPEGQUALITY.Set(TWJQ.HIGH);
                                LogIt($"Set jpg quality to {TWJQ.HIGH}", sts);
                            }
                        }
                    }
                }
            }
        }
        LogIt("");
    }

    const ushort TWQC_MACHINE = 0x1000;// applies to entire session/machine
    const ushort TWQC_BITONAL = 0x2000; // applies to Bitonal "cameras"
    const ushort TWQC_COLOR = 0x4000;  // applies to Color "cameras"
    [Flags]
    public enum TWQC2 : ushort
    {
        MACHINE = 0x1000,
        BITONAL = 0x2000,
        COLOR = 0x4000
    }

    private string FlattenFlag(TWQC val)
    {
        StringBuilder sb = new();

        foreach (var type in Enum.GetValues(typeof(TWQC)))
        {
            if (((ushort)val & (ushort)type) > 0)
            {
                sb.Append(type).Append(", ");
            }
        }
        foreach (var type in Enum.GetValues(typeof(TWQC2)))
        {
            if (((ushort)val & (ushort)type) > 0)
            {
                sb.Append(type).Append(", ");
            }
        }
        if (sb.Length > 0)
        {
            sb.Length = sb.Length - 2;
        }
        return sb.ToString();
    }

    private void CaptureAsStandardScanner()
    {
        LogIt("Attempting Standard Scanner mode");

        var sts = _twain.Caps.ICAP_PIXELTYPE.Set(TWPT.RGB);
        LogIt("Set rgb pixel type", sts);

        if (_twain.Caps.CAP_DUPLEXENABLED.Supports.HasFlag(TWQC.SET))
        {
            sts = _twain.Caps.CAP_DUPLEXENABLED.Set(TW_BOOL.True);
            LogIt("Set duplex enabled", sts);
        }

        var dpi = listDpi.SelectedValue == null ? 300 : Convert.ToInt32(listDpi.SelectedValue);
        sts = _twain.Caps.ICAP_XRESOLUTION.Set(dpi);
        LogIt("Set x-resolution", sts);
        sts = _twain.Caps.ICAP_YRESOLUTION.Set(dpi);
        LogIt("Set y-resolution", sts);

        if (listFormat.SelectedItem != null)
        {
            var format = (TWFF)listFormat.SelectedItem;

            sts = _twain.Caps.ICAP_IMAGEFILEFORMAT.Set(format);
            LogIt($"Set {format} format.", sts);

            if (!sts.IsSuccess)
            {
                return;
            }

            if (format != TWFF.BMP)
            {
                if (_twain.Caps.ICAP_COMPRESSION.Supports.HasFlag(TWQC.SET))
                {
                    sts = _twain.Caps.ICAP_COMPRESSION.Set(TWCP.JPEG);
                    LogIt("Set compression to jpg", sts);
                    if (sts.IsSuccess)
                    {
                        short quality = 90;
                        sts = _twain.Caps.ICAP_JPEGQUALITY.Set((TWJQ)quality);
                        LogIt($"Set jpg quality to {quality}", sts);
                        if (!sts.IsSuccess)
                        {
                            quality = 85;
                            sts = _twain.Caps.ICAP_JPEGQUALITY.Set((TWJQ)quality);
                            LogIt($"Set jpg quality to {quality}", sts);

                            if (!sts.IsSuccess)
                            {
                                sts = _twain.Caps.ICAP_JPEGQUALITY.Set(TWJQ.HIGH);
                                LogIt($"Set jpg quality to {TWJQ.HIGH}", sts);
                            }
                        }
                    }
                }
            }
        }
        var limit = (short)boxLimit.Value;
        sts = _twain.Caps.CAP_XFERCOUNT.Set(limit);
        LogIt($"Set transfer limit {boxLimit.Value}", sts);
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        _stopTransfer = true;
    }

    private void btnBrowseFolder_Click(object sender, EventArgs e)
    {
        using FolderBrowserDialog dialog = new();
        if (dialog.ShowDialog() == DialogResult.OK)
        {
            boxFolder.Text = dialog.SelectedPath;
        }
    }

    private void btnOpenFolder_Click(object sender, EventArgs e)
    {
        if (EnsureBoxFolder())
        {
            try
            {
                using var p = Process.Start("explorer.exe", boxFolder.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private bool EnsureBoxFolder()
    {
        if (!string.IsNullOrEmpty(boxFolder.Text))
        {
            if (!Directory.Exists(boxFolder.Text))
            {
                try
                {
                    Directory.CreateDirectory(boxFolder.Text);
                }
                catch (Exception ex)
                {
                    LogIt($"Failed to ensure save folder: {ex.Message}");
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}
