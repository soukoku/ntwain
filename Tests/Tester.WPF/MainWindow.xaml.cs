using GalaSoft.MvvmLight.Messaging;
using ModernWPF.Controls;
using NTwain;
using NTwain.Data;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

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
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
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

                Messenger.Default.Register<DialogMessage>(this, msg =>
                {
                    if (Dispatcher.CheckAccess())
                    {
                        ModernMessageBox.Show(this, msg.Content, msg.Caption, msg.Button, msg.Icon, msg.DefaultResult);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            ModernMessageBox.Show(this, msg.Content, msg.Caption, msg.Button, msg.Icon, msg.DefaultResult);
                        }));
                    }
                });
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = _twainVM.State > 4;
            base.OnClosing(e);
        }
        protected override void OnClosed(EventArgs e)
        {
            if (_twainVM.State == 4)
            {
                _twainVM.CurrentSource.Close();
            }
            _twainVM.Close();
            base.OnClosed(e);
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // use this for internal msg loop
            //var rc = _twainVM.Open();

            // use this to hook into current app loop
            var rc = _twainVM.Open(new WpfMessageLoopHook(new WindowInteropHelper(this).Handle));

            if (rc == ReturnCode.Success)
            {
                SrcList.ItemsSource = _twainVM.Select(s => new DSVM { DS = s });
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
                _twainVM.CurrentSource.Close();
            }

            var dsId = SrcList.SelectedItem as DSVM;
            if (dsId != null)
            {
                var rc = dsId.DS.Open();
                //rc = DGControl.Status.Get(dsId, ref stat);
                if (rc == ReturnCode.Success)
                {
                    var caps = dsId.DS.SupportedCaps.Select(o => new CapVM
                    {
                        Cap = o,
                        Supports = dsId.DS.CapQuerySupport(o)
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
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ACapXferMech.Get();
                        break;
                    case CapabilityId.CapAlarms:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapAlarmVolume.Get();
                        break;
                    case CapabilityId.CapAlarmVolume:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapAlarmVolume.Get();
                        break;
                    case CapabilityId.CapAuthor:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapAutoFeed:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapAutomaticCapture:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapAutomaticSenseMedium:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapAutoScan:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapBatteryMinutes:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapBatteryPercentage:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapCameraEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapCameraOrder:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapCameraPreviewUI:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapCameraSide:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<CameraSide>();
                        break;
                    case CapabilityId.CapCaption:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapClearBuffers:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<ClearBuffer>();
                        break;
                    case CapabilityId.CapClearPage:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapClearPage.Get();
                        break;
                    case CapabilityId.CapCustomDSData:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapCustomDSData.Get();
                        break;
                    case CapabilityId.CapCustomInterfaceGuid:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapDeviceEvent:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<DeviceEvent>();
                        break;
                    case CapabilityId.CapDeviceOnline:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapDeviceOnline.Get();
                        break;
                    case CapabilityId.CapDeviceTimeDate:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapDoubleFeedDetection:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<DoubleFeedDetection>();
                        break;
                    case CapabilityId.CapDoubleFeedDetectionLength:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapDoubleFeedDetectionResponse:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<DoubleFeedDetectionResponse>();
                        break;
                    case CapabilityId.CapDoubleFeedDetectionSensitivity:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<DoubleFeedDetectionSensitivity>();
                        break;
                    case CapabilityId.CapDuplex:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapDuplex.Get();
                        break;
                    case CapabilityId.CapDuplexEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapDuplexEnabled.Get();
                        break;
                    case CapabilityId.CapEnableDSUIOnly:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapEnableDSUIOnly.Get();
                        break;
                    case CapabilityId.CapEndorser:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapExtendedCaps:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapFeederAlignment:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<FeederAlignment>();
                        break;
                    case CapabilityId.CapFeederEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapFeederEnabled.Get();
                        break;
                    case CapabilityId.CapFeederLoaded:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapFeederLoaded.Get();
                        break;
                    case CapabilityId.CapFeederOrder:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<FeederOrder>();
                        break;
                    case CapabilityId.CapFeederPocket:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<FeederPocket>();
                        break;
                    case CapabilityId.CapFeederPrep:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapFeedPage:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapFeedPage.Get();
                        break;
                    case CapabilityId.CapIndicators:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapIndicators.Get();
                        break;
                    case CapabilityId.CapIndicatorsMode:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<IndicatorsMode>();
                        break;
                    case CapabilityId.CapJobControl:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapJobControl.Get();
                        break;
                    case CapabilityId.CapLanguage:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<Language>();
                        break;
                    case CapabilityId.CapMaxBatchBuffers:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapMicrEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPaperDetectable:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapPaperDetectable.Get();
                        break;
                    case CapabilityId.CapPaperHandling:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<PaperHandling>();
                        break;
                    case CapabilityId.CapPowerSaveTime:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPowerSupply:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<PowerSupply>();
                        break;
                    case CapabilityId.CapPrinter:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<Printer>();
                        break;
                    case CapabilityId.CapPrinterCharRotation:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterFontStyle:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<PrinterFontStyle>();
                        break;
                    case CapabilityId.CapPrinterIndex:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterIndexLeadChar:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterIndexMaxValue:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterIndexNumDigits:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterIndexStep:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterIndexTrigger:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<PrinterIndexTrigger>();
                        break;
                    case CapabilityId.CapPrinterMode:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<PrinterMode>();
                        break;
                    case CapabilityId.CapPrinterString:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterStringPreview:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterSuffix:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapPrinterVerticalOffset:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapReacquireAllowed:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapRewindPage:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapRewindPage.Get();
                        break;
                    case CapabilityId.CapSegmented:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<Segmented>();
                        break;
                    case CapabilityId.CapSerialNumber:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapSupportedCaps:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap).CastToEnum<CapabilityId>();
                        break;
                    case CapabilityId.CapSupportedCapsSegmentUnique:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapSupportedDATs:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapThumbnailsEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapThumbnailsEnabled.Get();
                        break;
                    case CapabilityId.CapTimeBeforeFirstCapture:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapTimeBetweenCaptures:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapTimeDate:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.CapUIControllable:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapUIControllable.Get();
                        break;
                    case CapabilityId.CapXferCount:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapXferCount.Get();
                        break;
                    case CapabilityId.CustomBase:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.ICapAutoBright:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutoBright.Get();
                        break;
                    case CapabilityId.ICapAutoDiscardBlankPages:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutoDiscardBlankPages.Get();
                        break;
                    case CapabilityId.ICapAutomaticBorderDetection:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutomaticBorderDetection.Get();
                        break;
                    case CapabilityId.ICapAutomaticColorEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutomaticColorEnabled.Get();
                        break;
                    case CapabilityId.ICapAutomaticColorNonColorPixelType:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutomaticColorNonColorPixelType.Get();
                        break;
                    case CapabilityId.ICapAutomaticCropUsesFrame:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutomaticCropUsesFrame.Get();
                        break;
                    case CapabilityId.ICapAutomaticDeskew:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutomaticDeskew.Get();
                        break;
                    case CapabilityId.ICapAutomaticLengthDetection:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutomaticLengthDetection.Get();
                        break;
                    case CapabilityId.ICapAutomaticRotate:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutomaticRotate.Get();
                        break;
                    case CapabilityId.ICapAutoSize:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapAutoSize.Get();
                        break;
                    case CapabilityId.ICapBarcodeDetectionEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBarcodeDetectionEnabled.Get();
                        break;
                    case CapabilityId.ICapBarcodeMaxRetries:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBarcodeMaxRetries.Get();
                        break;
                    case CapabilityId.ICapBarcodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBarcodeSearchPriorities.Get();
                        break;
                    case CapabilityId.ICapBarcodeSearchMode:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBarcodeSearchMode.Get();
                        break;
                    case CapabilityId.ICapBarcodeSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBarcodeSearchPriorities.Get();
                        break;
                    case CapabilityId.ICapBarcodeTimeout:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBarcodeTimeout.Get();
                        break;
                    case CapabilityId.ICapBitDepth:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBitDepth.Get();
                        break;
                    case CapabilityId.ICapBitDepthReduction:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBitDepthReduction.Get();
                        break;
                    case CapabilityId.ICapBitOrder:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBitOrder.Get();
                        break;
                    case CapabilityId.ICapBitOrderCodes:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBitOrderCodes.Get();
                        break;
                    case CapabilityId.ICapBrightness:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapBrightness.Get();
                        break;
                    case CapabilityId.ICapCCITTKFactor:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapCCITTKFactor.Get();
                        break;
                    case CapabilityId.ICapColorManagementEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapColorManagementEnabled.Get();
                        break;
                    case CapabilityId.ICapCompression:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapCompression.Get();
                        break;
                    case CapabilityId.ICapContrast:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapContrast.Get();
                        break;
                    case CapabilityId.ICapCustHalftone:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapCustHalftone.Get();
                        break;
                    case CapabilityId.ICapExposureTime:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapExposureTime.Get();
                        break;
                    case CapabilityId.ICapExtImageInfo:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapExtImageInfo.Get();
                        break;
                    case CapabilityId.ICapFeederType:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapFeederType.Get();
                        break;
                    case CapabilityId.ICapFilmType:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapFilmType.Get();
                        break;
                    case CapabilityId.ICapFilter:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapFilter.Get();
                        break;
                    case CapabilityId.ICapFlashUsed:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.ICapFlashUsed2:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapFlashUsed2.Get();
                        break;
                    case CapabilityId.ICapFlipRotation:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapFlipRotation.Get();
                        break;
                    case CapabilityId.ICapFrames:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.ICapGamma:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapGamma.Get();
                        break;
                    case CapabilityId.ICapHalftones:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.ICapHighlight:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapHighlight.Get();
                        break;
                    case CapabilityId.ICapICCProfile:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapICCProfile.Get();
                        break;
                    case CapabilityId.ICapImageDataSet:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
                        break;
                    case CapabilityId.ICapImageFileFormat:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapImageFileFormat.Get();
                        break;
                    case CapabilityId.ICapImageFilter:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapImageFilter.Get();
                        break;
                    case CapabilityId.ICapImageMerge:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapImageMerge.Get();
                        break;
                    case CapabilityId.ICapImageMergeHeightThreshold:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapImageMergeHeightThreshold.Get();
                        break;
                    case CapabilityId.ICapJpegPixelType:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapJpegPixelType.Get();
                        break;
                    case CapabilityId.ICapJpegQuality:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapJpegQuality.Get();
                        break;
                    case CapabilityId.ICapJpegSubsampling:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapJpegSubsampling.Get();
                        break;
                    case CapabilityId.ICapLampState:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapLampState.Get();
                        break;
                    case CapabilityId.ICapLightPath:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapLightPath.Get();
                        break;
                    case CapabilityId.ICapLightSource:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapLightSource.Get();
                        break;
                    case CapabilityId.ICapMaxFrames:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapMaxFrames.Get();
                        break;
                    case CapabilityId.ICapMinimumHeight:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapMinimumHeight.Get();
                        break;
                    case CapabilityId.ICapMinimumWidth:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapMinimumWidth.Get();
                        break;
                    case CapabilityId.ICapMirror:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapMirror.Get();
                        break;
                    case CapabilityId.ICapNoiseFilter:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapNoiseFilter.Get();
                        break;
                    case CapabilityId.ICapOrientation:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapOrientation.Get();
                        break;
                    case CapabilityId.ICapOverScan:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapOverScan.Get();
                        break;
                    case CapabilityId.ICapPatchCodeDetectionEnabled:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPatchCodeDetectionEnabled.Get();
                        break;
                    case CapabilityId.ICapPatchCodeMaxRetries:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPatchCodeMaxRetries.Get();
                        break;
                    case CapabilityId.ICapPatchCodeMaxSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPatchCodeMaxSearchPriorities.Get();
                        break;
                    case CapabilityId.ICapPatchCodeSearchMode:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPatchCodeSearchMode.Get();
                        break;
                    case CapabilityId.ICapPatchCodeSearchPriorities:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPatchCodeSearchPriorities.Get();
                        break;
                    case CapabilityId.ICapPatchCodeTimeout:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPatchCodeTimeout.Get();
                        break;
                    // TODO phys size are twfix32
                    case CapabilityId.ICapPhysicalHeight:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPhysicalHeight.Get();
                        break;
                    case CapabilityId.ICapPhysicalWidth:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPhysicalWidth.Get();
                        break;
                    case CapabilityId.ICapPixelFlavor:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPixelFlavor.Get();
                        break;
                    case CapabilityId.ICapPixelFlavorCodes:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPixelFlavorCodes.Get();
                        break;
                    case CapabilityId.ICapPixelType:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPixelType.Get();
                        break;
                    case CapabilityId.ICapPlanarChunky:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapPlanarChunky.Get();
                        break;
                    case CapabilityId.ICapRotation:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapRotation.Get();
                        break;
                    case CapabilityId.ICapShadow:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapShadow.Get();
                        break;
                    case CapabilityId.ICapSupportedBarcodeTypes:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapSupportedBarcodeTypes.Get();
                        break;
                    case CapabilityId.ICapSupportedExtImageInfo:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapSupportedExtImageInfo.Get();
                        break;
                    case CapabilityId.ICapSupportedPatchCodeTypes:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapSupportedPatchCodeTypes.Get();
                        break;
                    case CapabilityId.ICapSupportedSizes:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapSupportedSizes.Get();
                        break;
                    case CapabilityId.ICapThreshold:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapThreshold.Get();
                        break;
                    case CapabilityId.ICapTiles:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapTiles.Get();
                        break;
                    case CapabilityId.ICapTimeFill:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapTimeFill.Get();
                        break;
                    case CapabilityId.ICapUndefinedImageSize:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapUndefinedImageSize.Get();
                        break;
                    case CapabilityId.ICapUnits:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapUnits.Get();
                        break;
                    case CapabilityId.ICapXferMech:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapXferMech.Get();
                        break;
                    case CapabilityId.ICapXNativeResolution:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapXNativeResolution.Get();
                        break;
                    case CapabilityId.ICapXResolution:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapXResolution.Get();
                        break;
                    case CapabilityId.ICapXScaling:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapXScaling.Get();
                        break;
                    case CapabilityId.ICapYNativeResolution:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapYNativeResolution.Get();
                        break;
                    case CapabilityId.ICapYResolution:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapYResolution.Get();
                        break;
                    case CapabilityId.ICapYScaling:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapYScaling.Get();
                        break;
                    case CapabilityId.ICapZoomFactor:
                        CapDetailList.ItemsSource = _twainVM.CurrentSource.ICapZoomFactor.Get();
                        break;
                    default:
                        if (cap > CapabilityId.CustomBase)
                        {
                            CapDetailList.ItemsSource = _twainVM.CurrentSource.CapGet(cap);
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
