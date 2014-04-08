using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NTwain;
using NTwain.Data;
using NTwain.Values;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using CommonWin32;
using System.Threading;
using ModernWPF.Controls;
using System.Reflection;

namespace Tester.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TwainSessionWPF twain;
        public MainWindow()
        {
            InitializeComponent();
            if (IntPtr.Size == 8)
            {
                Title = Title + " (64bit)";
            }
            else
            {
                Title = Title + " (32bit)";
            }

            SetupTwain();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (twain.State == 4)
            {
                twain.CloseSource();
            }
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(twain.PreFilterMessage);

            var rc = twain.OpenManager(hwnd);
            if (rc == ReturnCode.Success)
            {
                SrcList.ItemsSource = twain.GetSources();
            }
        }

        private void SetupTwain()
        {
            TWIdentity appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetEntryAssembly());
            twain = new TwainSessionWPF(appId);
            twain.DataTransferred += (s, e) =>
            {
                if (e.Data != IntPtr.Zero)
                {
                    ImageDisplay.Source = e.Data.GetWPFBitmap();
                }
                else if (!string.IsNullOrEmpty(e.FilePath))
                {
                    var img = new BitmapImage(new Uri(e.FilePath));
                    ImageDisplay.Source = img;
                }
            };

            twain.SourceDisabled += delegate
            {
                ModernMessageBox.Show(this, "Success!");
            };
            twain.TransferReady += (s, te) =>
            {
                if (twain.GetCurrentCap<XferMech>(CapabilityId.ICapXferMech) == XferMech.File)
                {
                    var formats = twain.CapGetImageFileFormat();
                    var wantFormat = formats.Contains(FileFormat.Tiff) ? FileFormat.Tiff : FileFormat.Bmp;

                    var fileSetup = new TWSetupFileXfer
                    {
                        Format = wantFormat,
                        FileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test.tif")
                    };
                    var rc = twain.DGControl.SetupFileXfer.Set(fileSetup);
                }
            };
            this.DataContext = twain;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (twain.State == 4)
            {
                if (twain.CapGetPixelTypes().Contains(PixelType.BlackWhite))
                {
                    twain.CapSetPixelType(PixelType.BlackWhite);
                }

                if (twain.CapGetImageXferMechs().Contains(XferMech.File))
                {
                    twain.CapSetImageXferMech(XferMech.File);
                }

                var rc = twain.EnableSource(SourceEnableMode.NoUI, false, new WindowInteropHelper(this).Handle, SynchronizationContext.Current);
            }
        }

        private void SrcList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (twain.State == 4)
            {
                twain.CloseSource();
            }

            var dsId = SrcList.SelectedItem as TWIdentity;
            if (dsId != null)
            {
                var rc = twain.OpenSource(dsId.ProductName);
                //rc = DGControl.Status.Get(dsId, ref stat);
                if (rc == ReturnCode.Success)
                {
                    var caps = twain.SupportedCaps.Select(o => new CapVM
                    {
                        Id = o
                    }).OrderBy(o => o.Name).ToList();
                    CapList.ItemsSource = caps;
                }
            }
            else
            {
                CapList.ItemsSource = null;
            }
        }

        class CapVM
        {
            public CapabilityId Id { get; set; }
            public string Name
            {
                get
                {
                    if (Id > CapabilityId.CustomBase)
                    {
                        return "[Custom] " + ((int)Id - (int)CapabilityId.CustomBase);
                    }
                    return Id.ToString();
                }
            }
            public override string ToString()
            {
                return Name;
            }
        }

        private void CapList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var capVM = CapList.SelectedItem as CapVM;
            if (capVM != null)
            {
                var cap = capVM.Id;
                switch (cap)
                {
                    case CapabilityId.ACapXferMech:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<XferMech>(cap, true);
                        break;
                    case CapabilityId.CapAlarms:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<AlarmType>(cap, true);
                        break;
                    case CapabilityId.CapAlarmVolume:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    //case CapabilityId.CapAuthor:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<AlarmType>(cap, true);
                    //    break;
                    case CapabilityId.CapAutoFeed:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapAutomaticCapture:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapAutomaticSenseMedium:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapAutoScan:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapBatteryMinutes:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapBatteryPercentage:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraOrder:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraPreviewUI:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraSide:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<CameraSide>(cap, true);
                        break;
                    //case CapabilityId.CapCaption:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapClearBuffers:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ClearBuffer>(cap, true);
                        break;
                    case CapabilityId.CapClearPage:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCustomDSData:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    //case CapabilityId.CapCustomInterfaceGuid:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapDeviceEvent:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<DeviceEvent>(cap, true);
                        break;
                    case CapabilityId.CapDeviceOnline:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapDeviceTimeDate:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetection:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<DoubleFeedDetection>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionLength:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionResponse:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<DoubleFeedDetectionResponse>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionSensitivity:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<DoubleFeedDetectionSensitivity>(cap, true);
                        break;
                    case CapabilityId.CapDuplex:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Duplex>(cap, true);
                        break;
                    case CapabilityId.CapDuplexEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapEnableDSUIOnly:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    //case CapabilityId.CapEndorser:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapExtendedCaps:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeederAlignment:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FeederAlignment>(cap, true);
                        break;
                    case CapabilityId.CapFeederEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeederLoaded:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeederOrder:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FeederOrder>(cap, true);
                        break;
                    case CapabilityId.CapFeederPocket:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FeederPocket>(cap, true);
                        break;
                    case CapabilityId.CapFeederPrep:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeedPage:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapIndicators:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapIndicatorsMode:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<IndicatorsMode>(cap, true);
                        break;
                    case CapabilityId.CapJobControl:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<JobControl>(cap, true);
                        break;
                    case CapabilityId.CapLanguage:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Language>(cap, true);
                        break;
                    case CapabilityId.CapMaxBatchBuffers:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapMicrEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPaperDetectable:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPaperHandling:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PaperHandling>(cap, true);
                        break;
                    case CapabilityId.CapPowerSaveTime:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPowerSupply:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PowerSupply>(cap, true);
                        break;
                    case CapabilityId.CapPrinter:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Printer>(cap, true);
                        break;
                    case CapabilityId.CapPrinterCharRotation:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterFontStyle:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PrinterFontStyle>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndex:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexLeadChar:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexMaxValue:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexNumDigits:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexStep:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexTrigger:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PrinterIndexTrigger>(cap, true);
                        break;
                    case CapabilityId.CapPrinterMode:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PrinterMode>(cap, true);
                        break;
                    //case CapabilityId.CapPrinterString:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    //case CapabilityId.CapPrinterStringPreview:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    //case CapabilityId.CapPrinterSuffix:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapPrinterVerticalOffset:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapReacquireAllowed:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapRewindPage:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapSegmented:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Segmented>(cap, true);
                        break;
                    //case CapabilityId.CapSerialNumber:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    //case CapabilityId.CapSupportedCaps:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapSupportedCapsSegmentUnique:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapSupportedDATs:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapThumbnailsEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapTimeBeforeFirstCapture:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapTimeBetweenCaptures:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapTimeDate:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapUIControllable:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapXferCount:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<uint>(cap, true); // spec says ushort but who knows
                        break;
                    case CapabilityId.CustomBase:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutoBright:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutoDiscardBlankPages:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticBorderDetection:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticColorEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticColorNonColorPixelType:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PixelType>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticCropUsesFrame:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticDeskew:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticLengthDetection:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticRotate:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutoSize:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeDetectionEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeMaxRetries:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeSearchMode:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeSearchPriorities:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeTimeout:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBitDepth:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBitDepthReduction:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<BitDepthReduction>(cap, true);
                        break;
                    case CapabilityId.ICapBitOrder:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<BitOrder>(cap, true);
                        break;
                    case CapabilityId.ICapBitOrderCodes:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBrightness:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapCCITTKFactor:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapColorManagementEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapCompression:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Compression>(cap, true);
                        break;
                    case CapabilityId.ICapContrast:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapCustHalftone:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapExposureTime:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapExtImageInfo:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapFeederType:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FeederType>(cap, true);
                        break;
                    case CapabilityId.ICapFilmType:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FilmType>(cap, true);
                        break;
                    case CapabilityId.ICapFilter:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FilterType>(cap, true);
                        break;
                    case CapabilityId.ICapFlashUsed:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapFlashUsed2:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapFlipRotation:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FlipRotation>(cap, true);
                        break;
                    //case CapabilityId.ICapFrames:
                    //    CapDetailList.ItemsSource = twain.GetCapabilityValues<TWFrame>(cap, true);
                    //    break;
                    case CapabilityId.ICapGamma:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapHalftones:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapHighlight:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapICCProfile:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<IccProfile>(cap, true);
                        break;
                    case CapabilityId.ICapImageDataset:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapImageFileFormat:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<FileFormat>(cap, true);
                        break;
                    case CapabilityId.ICapImageFilter:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ImageFilter>(cap, true);
                        break;
                    case CapabilityId.ICapImageMerge:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ImageMerge>(cap, true);
                        break;
                    case CapabilityId.ICapImageMergeHeightThreshold:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapJpegPixelType:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PixelType>(cap, true);
                        break;
                    case CapabilityId.ICapJpegQuality:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<JpegQuality>(cap, true);
                        break;
                    case CapabilityId.ICapJpegSubSampling:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<JpegSubSampling>(cap, true);
                        break;
                    case CapabilityId.ICapLampState:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapLightPath:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<LightPath>(cap, true);
                        break;
                    case CapabilityId.ICapLightSource:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<LightSource>(cap, true);
                        break;
                    case CapabilityId.ICapMaxFrames:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapMinimumHeight:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapMinimumWidth:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapMirror:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Mirror>(cap, true);
                        break;
                    case CapabilityId.ICapNoiseFilter:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<NoiseFilter>(cap, true);
                        break;
                    case CapabilityId.ICapOrientation:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<NTwain.Values.Orientation>(cap, true);
                        break;
                    case CapabilityId.ICapOverScan:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<OverScan>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeDetectionEnabled:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeMaxRetries:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PatchCode>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeSearchMode:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeSearchPriorities:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeTimeout:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    // TODO phys size are twfix32
                    case CapabilityId.ICapPhysicalHeight:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapPhysicalWidth:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapPixelFlavor:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PixelFlavor>(cap, true);
                        break;
                    case CapabilityId.ICapPixelFlavorCodes:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPixelType:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PixelType>(cap, true);
                        break;
                    case CapabilityId.ICapPlanarChunky:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PlanarChunky>(cap, true);
                        break;
                    case CapabilityId.ICapRotation:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Rotation>(cap, true);
                        break;
                    case CapabilityId.ICapShadow:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedBarcodeTypes:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<BarcodeType>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedExtImageInfo:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ExtendedImageInfo>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedPatchCodeTypes:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<PatchCode>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedSizes:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<SupportedSize>(cap, true);
                        break;
                    case CapabilityId.ICapThreshold:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapTiles:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapTimeFill:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapUndefinedImageSize:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapUnits:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<Unit>(cap, true);
                        break;
                    case CapabilityId.ICapXferMech:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<XferMech>(cap, true);
                        break;
                    case CapabilityId.ICapXNativeResolution:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapXResolution:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapXScaling:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapYNativeResolution:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapYResolution:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapYScaling:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapZoomFactor:
                        CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        break;
                    default:
                        if (cap > CapabilityId.CustomBase)
                        {
                            CapDetailList.ItemsSource = twain.GetCapabilityValues<ushort>(cap, true);
                        }
                        else
                        {
                            CapDetailList.ItemsSource = null;
                        }
                        break;
                }

            }
            else
            {
                CapDetailList.ItemsSource = null;
            }
        }

        private void CapDetailList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
