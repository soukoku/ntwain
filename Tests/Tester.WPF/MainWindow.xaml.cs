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
        TwainVM _twainVM;

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

            _twainVM = new TwainVM();
            _twainVM.SourceDisabled += delegate
            {
                ModernMessageBox.Show(this, "Success!");
            };
            this.DataContext = _twainVM;
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_twainVM.State == 4)
            {
                _twainVM.CloseSource();
            }
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            var hwnd = new WindowInteropHelper(this).Handle;

            // this line is unnecessary if using twain 2 dsm but doesn't hurt to use it
            HwndSource.FromHwnd(hwnd).AddHook(_twainVM.PreFilterMessage);

            var rc = _twainVM.OpenManager(hwnd);
            if (rc == ReturnCode.Success)
            {
                SrcList.ItemsSource = _twainVM.GetSources().Select(s => new DSVM { DS = s });
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _twainVM.TestCapture(new WindowInteropHelper(this).Handle);
        }

        private void SrcList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_twainVM.State == 4)
            {
                _twainVM.CloseSource();
            }

            var dsId = SrcList.SelectedItem as DSVM;
            if (dsId != null)
            {
                var rc = _twainVM.OpenSource(dsId.Name);
                //rc = DGControl.Status.Get(dsId, ref stat);
                if (rc == ReturnCode.Success)
                {
                    var caps = _twainVM.SupportedCaps.Select(o => new CapVM
                    {
                        Cap = o
                    }).OrderBy(o => o.Name).ToList();
                    CapList.ItemsSource = caps;
                }
            }
            else
            {
                CapList.ItemsSource = null;
            }
        }


        private void CapList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var capVM = CapList.SelectedItem as CapVM;
            if (capVM != null)
            {
                var cap = capVM.Cap;
                switch (cap)
                {
                    case CapabilityId.ACapXferMech:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<XferMech>(cap, true);
                        break;
                    case CapabilityId.CapAlarms:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<AlarmType>(cap, true);
                        break;
                    case CapabilityId.CapAlarmVolume:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    //case CapabilityId.CapAuthor:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<AlarmType>(cap, true);
                    //    break;
                    case CapabilityId.CapAutoFeed:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapAutomaticCapture:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapAutomaticSenseMedium:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapAutoScan:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapBatteryMinutes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapBatteryPercentage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraOrder:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraPreviewUI:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCameraSide:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<CameraSide>(cap, true);
                        break;
                    //case CapabilityId.CapCaption:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapClearBuffers:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ClearBuffer>(cap, true);
                        break;
                    case CapabilityId.CapClearPage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapCustomDSData:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    //case CapabilityId.CapCustomInterfaceGuid:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapDeviceEvent:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<DeviceEvent>(cap, true);
                        break;
                    case CapabilityId.CapDeviceOnline:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapDeviceTimeDate:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetection:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<DoubleFeedDetection>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionLength:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionResponse:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<DoubleFeedDetectionResponse>(cap, true);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionSensitivity:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<DoubleFeedDetectionSensitivity>(cap, true);
                        break;
                    case CapabilityId.CapDuplex:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Duplex>(cap, true);
                        break;
                    case CapabilityId.CapDuplexEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapEnableDSUIOnly:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    //case CapabilityId.CapEndorser:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapExtendedCaps:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeederAlignment:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FeederAlignment>(cap, true);
                        break;
                    case CapabilityId.CapFeederEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeederLoaded:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeederOrder:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FeederOrder>(cap, true);
                        break;
                    case CapabilityId.CapFeederPocket:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FeederPocket>(cap, true);
                        break;
                    case CapabilityId.CapFeederPrep:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapFeedPage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapIndicators:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapIndicatorsMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<IndicatorsMode>(cap, true);
                        break;
                    case CapabilityId.CapJobControl:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<JobControl>(cap, true);
                        break;
                    case CapabilityId.CapLanguage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Language>(cap, true);
                        break;
                    case CapabilityId.CapMaxBatchBuffers:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapMicrEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPaperDetectable:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPaperHandling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PaperHandling>(cap, true);
                        break;
                    case CapabilityId.CapPowerSaveTime:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPowerSupply:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PowerSupply>(cap, true);
                        break;
                    case CapabilityId.CapPrinter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Printer>(cap, true);
                        break;
                    case CapabilityId.CapPrinterCharRotation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterFontStyle:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PrinterFontStyle>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndex:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexLeadChar:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexMaxValue:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexNumDigits:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexStep:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapPrinterIndexTrigger:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PrinterIndexTrigger>(cap, true);
                        break;
                    case CapabilityId.CapPrinterMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PrinterMode>(cap, true);
                        break;
                    //case CapabilityId.CapPrinterString:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    //case CapabilityId.CapPrinterStringPreview:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    //case CapabilityId.CapPrinterSuffix:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapPrinterVerticalOffset:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapReacquireAllowed:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapRewindPage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapSegmented:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Segmented>(cap, true);
                        break;
                    //case CapabilityId.CapSerialNumber:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    //case CapabilityId.CapSupportedCaps:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                    //    break;
                    case CapabilityId.CapSupportedCapsSegmentUnique:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapSupportedDATs:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapThumbnailsEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapTimeBeforeFirstCapture:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapTimeBetweenCaptures:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapTimeDate:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapUIControllable:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.CapXferCount:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<uint>(cap, true); // spec says ushort but who knows
                        break;
                    case CapabilityId.CustomBase:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutoBright:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutoDiscardBlankPages:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticBorderDetection:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticColorEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticColorNonColorPixelType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PixelType>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticCropUsesFrame:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticDeskew:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticLengthDetection:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutomaticRotate:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapAutoSize:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeDetectionEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeMaxRetries:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeSearchMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBarcodeTimeout:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBitDepth:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBitDepthReduction:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<BitDepthReduction>(cap, true);
                        break;
                    case CapabilityId.ICapBitOrder:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<BitOrder>(cap, true);
                        break;
                    case CapabilityId.ICapBitOrderCodes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapBrightness:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapCCITTKFactor:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapColorManagementEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapCompression:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Compression>(cap, true);
                        break;
                    case CapabilityId.ICapContrast:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapCustHalftone:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapExposureTime:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapExtImageInfo:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapFeederType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FeederType>(cap, true);
                        break;
                    case CapabilityId.ICapFilmType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FilmType>(cap, true);
                        break;
                    case CapabilityId.ICapFilter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FilterType>(cap, true);
                        break;
                    case CapabilityId.ICapFlashUsed:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapFlashUsed2:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapFlipRotation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FlipRotation>(cap, true);
                        break;
                    //case CapabilityId.ICapFrames:
                    //    CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<TWFrame>(cap, true);
                    //    break;
                    case CapabilityId.ICapGamma:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapHalftones:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapHighlight:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapICCProfile:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<IccProfile>(cap, true);
                        break;
                    case CapabilityId.ICapImageDataset:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapImageFileFormat:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<FileFormat>(cap, true);
                        break;
                    case CapabilityId.ICapImageFilter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ImageFilter>(cap, true);
                        break;
                    case CapabilityId.ICapImageMerge:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ImageMerge>(cap, true);
                        break;
                    case CapabilityId.ICapImageMergeHeightThreshold:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapJpegPixelType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PixelType>(cap, true);
                        break;
                    case CapabilityId.ICapJpegQuality:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<JpegQuality>(cap, true);
                        break;
                    case CapabilityId.ICapJpegSubSampling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<JpegSubSampling>(cap, true);
                        break;
                    case CapabilityId.ICapLampState:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapLightPath:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<LightPath>(cap, true);
                        break;
                    case CapabilityId.ICapLightSource:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<LightSource>(cap, true);
                        break;
                    case CapabilityId.ICapMaxFrames:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapMinimumHeight:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapMinimumWidth:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapMirror:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Mirror>(cap, true);
                        break;
                    case CapabilityId.ICapNoiseFilter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<NoiseFilter>(cap, true);
                        break;
                    case CapabilityId.ICapOrientation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<NTwain.Values.Orientation>(cap, true);
                        break;
                    case CapabilityId.ICapOverScan:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<OverScan>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeDetectionEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeMaxRetries:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PatchCode>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeSearchMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPatchCodeTimeout:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    // TODO phys size are twfix32
                    case CapabilityId.ICapPhysicalHeight:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapPhysicalWidth:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<uint>(cap, true);
                        break;
                    case CapabilityId.ICapPixelFlavor:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PixelFlavor>(cap, true);
                        break;
                    case CapabilityId.ICapPixelFlavorCodes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapPixelType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PixelType>(cap, true);
                        break;
                    case CapabilityId.ICapPlanarChunky:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PlanarChunky>(cap, true);
                        break;
                    case CapabilityId.ICapRotation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Rotation>(cap, true);
                        break;
                    case CapabilityId.ICapShadow:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedBarcodeTypes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<BarcodeType>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedExtImageInfo:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ExtendedImageInfo>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedPatchCodeTypes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<PatchCode>(cap, true);
                        break;
                    case CapabilityId.ICapSupportedSizes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<SupportedSize>(cap, true);
                        break;
                    case CapabilityId.ICapThreshold:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapTiles:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapTimeFill:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapUndefinedImageSize:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapUnits:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<Unit>(cap, true);
                        break;
                    case CapabilityId.ICapXferMech:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<XferMech>(cap, true);
                        break;
                    case CapabilityId.ICapXNativeResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapXResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapXScaling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapYNativeResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapYResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapYScaling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    case CapabilityId.ICapZoomFactor:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
                        break;
                    default:
                        if (cap > CapabilityId.CustomBase)
                        {
                            CapDetailList.ItemsSource = _twainVM.GetCapabilityValues<ushort>(cap, true);
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
