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
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<XferMech>();
                        break;
                    case CapabilityId.CapAlarms:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<AlarmType>();
                        break;
                    case CapabilityId.CapAlarmVolume:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapAuthor:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapAutoFeed:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapAutomaticCapture:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapAutomaticSenseMedium:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapAutoScan:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapBatteryMinutes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapBatteryPercentage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapCameraEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapCameraOrder:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapCameraPreviewUI:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapCameraSide:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<CameraSide>();
                        break;
                    case CapabilityId.CapCaption:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapClearBuffers:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<ClearBuffer>();
                        break;
                    case CapabilityId.CapClearPage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapCustomDSData:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapCustomInterfaceGuid:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapDeviceEvent:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<DeviceEvent>();
                        break;
                    case CapabilityId.CapDeviceOnline:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapDeviceTimeDate:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapDoubleFeedDetection:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<DoubleFeedDetection>();
                        break;
                    case CapabilityId.CapDoubleFeedDetectionLength:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionResponse:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<DoubleFeedDetectionResponse>();
                        break;
                    case CapabilityId.CapDoubleFeedDetectionSensitivity:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<DoubleFeedDetectionSensitivity>();
                        break;
                    case CapabilityId.CapDuplex:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Duplex>();
                        break;
                    case CapabilityId.CapDuplexEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapEnableDSUIOnly:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapEndorser:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapExtendedCaps:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapFeederAlignment:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FeederAlignment>();
                        break;
                    case CapabilityId.CapFeederEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapFeederLoaded:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapFeederOrder:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FeederOrder>();
                        break;
                    case CapabilityId.CapFeederPocket:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FeederPocket>();
                        break;
                    case CapabilityId.CapFeederPrep:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapFeedPage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapIndicators:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapIndicatorsMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<IndicatorsMode>();
                        break;
                    case CapabilityId.CapJobControl:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<JobControl>();
                        break;
                    case CapabilityId.CapLanguage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Language>();
                        break;
                    case CapabilityId.CapMaxBatchBuffers:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapMicrEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPaperDetectable:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPaperHandling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PaperHandling>();
                        break;
                    case CapabilityId.CapPowerSaveTime:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPowerSupply:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PowerSupply>();
                        break;
                    case CapabilityId.CapPrinter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Printer>();
                        break;
                    case CapabilityId.CapPrinterCharRotation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterFontStyle:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PrinterFontStyle>();
                        break;
                    case CapabilityId.CapPrinterIndex:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterIndexLeadChar:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterIndexMaxValue:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterIndexNumDigits:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterIndexStep:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterIndexTrigger:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PrinterIndexTrigger>();
                        break;
                    case CapabilityId.CapPrinterMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PrinterMode>();
                        break;
                    case CapabilityId.CapPrinterString:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterStringPreview:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterSuffix:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapPrinterVerticalOffset:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapReacquireAllowed:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapRewindPage:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapSegmented:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Segmented>();
                        break;
                    case CapabilityId.CapSerialNumber:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapSupportedCaps:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<CapabilityId>();
                        break;
                    case CapabilityId.CapSupportedCapsSegmentUnique:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapSupportedDATs:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapThumbnailsEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapTimeBeforeFirstCapture:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapTimeBetweenCaptures:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapTimeDate:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapUIControllable:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CapXferCount:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.CustomBase:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutoBright:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutoDiscardBlankPages:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutomaticBorderDetection:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutomaticColorEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutomaticColorNonColorPixelType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PixelType>();
                        break;
                    case CapabilityId.ICapAutomaticCropUsesFrame:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutomaticDeskew:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutomaticLengthDetection:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutomaticRotate:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapAutoSize:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBarcodeDetectionEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBarcodeMaxRetries:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBarcodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBarcodeSearchMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBarcodeSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBarcodeTimeout:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBitDepth:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBitDepthReduction:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<BitDepthReduction>();
                        break;
                    case CapabilityId.ICapBitOrder:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<BitOrder>();
                        break;
                    case CapabilityId.ICapBitOrderCodes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapBrightness:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapCCITTKFactor:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapColorManagementEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapCompression:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Compression>();
                        break;
                    case CapabilityId.ICapContrast:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapCustHalftone:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapExposureTime:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapExtImageInfo:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapFeederType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FeederType>();
                        break;
                    case CapabilityId.ICapFilmType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FilmType>();
                        break;
                    case CapabilityId.ICapFilter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FilterType>();
                        break;
                    case CapabilityId.ICapFlashUsed:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapFlashUsed2:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapFlipRotation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FlipRotation>();
                        break;
                    case CapabilityId.ICapFrames:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapGamma:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapHalftones:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapHighlight:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapICCProfile:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<IccProfile>();
                        break;
                    case CapabilityId.ICapImageDataset:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapImageFileFormat:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<FileFormat>();
                        break;
                    case CapabilityId.ICapImageFilter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<ImageFilter>();
                        break;
                    case CapabilityId.ICapImageMerge:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<ImageMerge>();
                        break;
                    case CapabilityId.ICapImageMergeHeightThreshold:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapJpegPixelType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PixelType>();
                        break;
                    case CapabilityId.ICapJpegQuality:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<JpegQuality>();
                        break;
                    case CapabilityId.ICapJpegSubSampling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<JpegSubSampling>();
                        break;
                    case CapabilityId.ICapLampState:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapLightPath:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<LightPath>();
                        break;
                    case CapabilityId.ICapLightSource:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<LightSource>();
                        break;
                    case CapabilityId.ICapMaxFrames:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapMinimumHeight:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapMinimumWidth:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapMirror:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Mirror>();
                        break;
                    case CapabilityId.ICapNoiseFilter:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<NoiseFilter>();
                        break;
                    case CapabilityId.ICapOrientation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<NTwain.Values.Orientation>();
                        break;
                    case CapabilityId.ICapOverScan:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<OverScan>();
                        break;
                    case CapabilityId.ICapPatchCodeDetectionEnabled:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapPatchCodeMaxRetries:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapPatchCodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PatchCode>();
                        break;
                    case CapabilityId.ICapPatchCodeSearchMode:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapPatchCodeSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapPatchCodeTimeout:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    // TODO phys size are twfix32
                    case CapabilityId.ICapPhysicalHeight:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapPhysicalWidth:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapPixelFlavor:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PixelFlavor>();
                        break;
                    case CapabilityId.ICapPixelFlavorCodes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapPixelType:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PixelType>();
                        break;
                    case CapabilityId.ICapPlanarChunky:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PlanarChunky>();
                        break;
                    case CapabilityId.ICapRotation:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Rotation>();
                        break;
                    case CapabilityId.ICapShadow:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapSupportedBarcodeTypes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<BarcodeType>();
                        break;
                    case CapabilityId.ICapSupportedExtImageInfo:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<ExtendedImageInfo>();
                        break;
                    case CapabilityId.ICapSupportedPatchCodeTypes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<PatchCode>();
                        break;
                    case CapabilityId.ICapSupportedSizes:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<SupportedSize>();
                        break;
                    case CapabilityId.ICapThreshold:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapTiles:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapTimeFill:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapUndefinedImageSize:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapUnits:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<Unit>();
                        break;
                    case CapabilityId.ICapXferMech:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap).CastToEnum<XferMech>();
                        break;
                    case CapabilityId.ICapXNativeResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapXResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapXScaling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapYNativeResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapYResolution:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapYScaling:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    case CapabilityId.ICapZoomFactor:
                        CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
                        break;
                    default:
                        if (cap > CapabilityId.CustomBase)
                        {
                            CapDetailList.ItemsSource = _twainVM.GetCapabilityValues(cap);
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
