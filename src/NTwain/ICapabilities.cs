using System.Collections.Generic;
using NTwain.Data;

namespace NTwain
{
    /// <summary>
    /// Exposes capabilities of a data source as properties.
    /// </summary>
    public interface ICapabilities
    {
        /// <summary>
        /// Gets the property to work with audio <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The audio xfer mech.
        /// </value>
        ICapWrapper<XferMech> ACapXferMech { get; }


        /// <summary>
        /// Gets the property to work with alarms for the current source.
        /// </summary>
        /// <value>
        /// The alarms.
        /// </value>
        ICapWrapper<AlarmType> CapAlarms { get; }
        /// <summary>
        /// Gets the property to work with alarm volume for the current source.
        /// </summary>
        /// <value>
        /// The alarm volume.
        /// </value>
        ICapWrapper<int> CapAlarmVolume { get; }
        /// <summary>
        /// Gets the property to work with the name or other identifying information about the 
        /// Author of the image. It may include a copyright string.
        /// </summary>
        /// <value>
        /// The author string.
        /// </value>
        ICapWrapper<string> CapAuthor { get; }
        /// <summary>
        /// Gets the property to work with auto feed page flag for the current source.
        /// </summary>
        /// <value>
        /// The auto feed flag.
        /// </value>
        ICapWrapper<BoolType> CapAutoFeed { get; }
        /// <summary>
        /// Gets the property to work with auto capture count for the current source.
        /// </summary>
        /// <value>
        /// The auto capture count.
        /// </value>
        ICapWrapper<int> CapAutomaticCapture { get; }
        /// <summary>
        /// Gets the property to work with auto-sense medium (paper source) flag.
        /// </summary>
        /// <value>
        /// The auto-sense medium flag.
        /// </value>
        ICapWrapper<BoolType> CapAutomaticSenseMedium { get; }
        /// <summary>
        /// Gets the property to work with auto scan page flag for the current source.
        /// </summary>
        /// <value>
        /// The auto scan flag.
        /// </value>
        ICapWrapper<BoolType> CapAutoScan { get; }
        /// <summary>
        /// Gets the property to see the remaining battery power for the device.
        /// </summary>
        /// <value>
        /// The battery minutes.
        /// </value>
        IReadOnlyCapWrapper<int> CapBatteryMinutes { get; }
        /// <summary>
        /// Gets the property to see the remaining battery percentage for the device.
        /// </summary>
        /// <value>
        /// The battery percentage.
        /// </value>
        IReadOnlyCapWrapper<int> CapBatteryPercentage { get; }
        /// <summary>
        /// Gets the property to work with camera enabled flag.
        /// </summary>
        /// <value>
        /// The camera enabled flag.
        /// </value>
        ICapWrapper<BoolType> CapCameraEnabled { get; }
        /// <summary>
        /// Gets the property to work with camera order for the current source.
        /// </summary>
        /// <value>
        /// The camera order setting.
        /// </value>
        ICapWrapper<PixelType> CapCameraOrder { get; }
        /// <summary>
        /// Gets the property to see whether device supports camera preview UI flag.
        /// </summary>
        /// <value>
        /// The camera preview UI flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapCameraPreviewUI { get; }
        /// <summary>
        /// Gets the property to work with camera side for the current source.
        /// </summary>
        /// <value>
        /// The camera side.
        /// </value>
        ICapWrapper<CameraSide> CapCameraSide { get; }
        /// <summary>
        /// Gets the property to work with the general note about the acquired image.
        /// </summary>
        /// <value>
        /// The general note string.
        /// </value>
        ICapWrapper<string> CapCaption { get; }
        /// <summary>
        /// Gets the property to work with the clear buffers option for the current source.
        /// </summary>
        /// <value>
        /// The clear buffers option.
        /// </value>
        ICapWrapper<ClearBuffer> CapClearBuffers { get; }
        /// <summary>
        /// Gets the property to work with clear page flag for the current source.
        /// </summary>
        /// <value>
        /// The clear page flag.
        /// </value>
        ICapWrapper<BoolType> CapClearPage { get; }
        /// <summary>
        /// Gets the property to see whether device supports custom data triplets.
        /// </summary>
        /// <value>
        /// The custom data flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapCustomDSData { get; }
        /// <summary>
        /// Gets the property for device interface guid.
        /// </summary>
        /// <value>
        /// The device interface guid.
        /// </value>
        IReadOnlyCapWrapper<string> CapCustomInterfaceGuid { get; }
        /// <summary>
        /// Gets the property to work with the reported device events for the current source.
        /// </summary>
        /// <value>
        /// The reported device events.
        /// </value>
        ICapWrapper<DeviceEvent> CapDeviceEvent { get; }
        /// <summary>
        /// Gets the property to work with devince online flag for the current source.
        /// </summary>
        /// <value>
        /// The devince online flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapDeviceOnline { get; }
        /// <summary>
        /// Gets the property to work with the device's time and date.
        /// </summary>
        /// <value>
        /// The device time and date.
        /// </value>
        ICapWrapper<string> CapDeviceTimeDate { get; }
        /// <summary>
        /// Gets the property to work with double feed detection option for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection option.
        /// </value>
        ICapWrapper<DoubleFeedDetection> CapDoubleFeedDetection { get; }
        /// <summary>
        /// Gets the property to work with double feed detection length for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection length.
        /// </value>
        ICapWrapper<TWFix32> CapDoubleFeedDetectionLength { get; }
        /// <summary>
        /// Gets the property to work with double feed detection response for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection response.
        /// </value>
        ICapWrapper<DoubleFeedDetectionResponse> CapDoubleFeedDetectionResponse { get; }
        /// <summary>
        /// Gets the property to work with double feed detection sensitivity for the current source.
        /// </summary>
        /// <value>
        /// The double feed detection sensitivity.
        /// </value>
        ICapWrapper<DoubleFeedDetectionSensitivity> CapDoubleFeedDetectionSensitivity { get; }
        /// <summary>
        /// Gets the property to see what's the duplex mode for the current source.
        /// </summary>
        /// <value>
        /// The duplex mode.
        /// </value>
        IReadOnlyCapWrapper<Duplex> CapDuplex { get; }
        /// <summary>
        /// Gets the property to work with duplex enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The duplex enabled flag.
        /// </value>
        ICapWrapper<BoolType> CapDuplexEnabled { get; }
        /// <summary>
        /// Gets the property to see whether device supports UI only flag (no transfer).
        /// </summary>
        /// <value>
        /// The UI only flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapEnableDSUIOnly { get; }
        /// <summary>
        /// Gets the property to work with endorser for the current source.
        /// </summary>
        /// <value>
        /// The endorser option.
        /// </value>
        ICapWrapper<uint> CapEndorser { get; }
        /// <summary>
        /// Gets the extended caps for the current source.
        /// </summary>
        /// <value>
        /// The extended caps.
        /// </value>
        ICapWrapper<CapabilityId> CapExtendedCaps { get; }
        /// <summary>
        /// Gets the property to work with feeder alignment for the current source.
        /// </summary>
        /// <value>
        /// The feeder alignment.
        /// </value>
        ICapWrapper<FeederAlignment> CapFeederAlignment { get; }
        /// <summary>
        /// Gets the property to work with feeder enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The feeder enabled flag.
        /// </value>
        ICapWrapper<BoolType> CapFeederEnabled { get; }
        /// <summary>
        /// Gets the property to work with feeder loaded flag for the current source.
        /// </summary>
        /// <value>
        /// The feeder loaded flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapFeederLoaded { get; }
        /// <summary>
        /// Gets the property to work with feeder order for the current source.
        /// </summary>
        /// <value>
        /// The feeder order.
        /// </value>
        ICapWrapper<FeederOrder> CapFeederOrder { get; }
        /// <summary>
        /// Gets the property to work with feeder pocket for the current source.
        /// </summary>
        /// <value>
        /// The feeder pocket setting.
        /// </value>
        ICapWrapper<FeederPocket> CapFeederPocket { get; }
        /// <summary>
        /// Gets the property to work with feeder prep flag.
        /// </summary>
        /// <value>
        /// The feeder prep flag.
        /// </value>
        ICapWrapper<BoolType> CapFeederPrep { get; }
        /// <summary>
        /// Gets the property to work with feed page flag for the current source.
        /// </summary>
        /// <value>
        /// The feed page flag.
        /// </value>
        ICapWrapper<BoolType> CapFeedPage { get; }
        /// <summary>
        /// Gets the property to work with indicators flag for the current source.
        /// </summary>
        /// <value>
        /// The indicators flag.
        /// </value>
        ICapWrapper<BoolType> CapIndicators { get; }
        /// <summary>
        /// Gets the property to work with diplayed indicators for the current source.
        /// </summary>
        /// <value>
        /// The diplayed indicators.
        /// </value>
        ICapWrapper<IndicatorsMode> CapIndicatorsMode { get; }
        /// <summary>
        /// Gets the property to work with job control option for the current source.
        /// </summary>
        /// <value>
        /// The job control option.
        /// </value>
        ICapWrapper<JobControl> CapJobControl { get; }
        /// <summary>
        /// Gets the property to work with string data language for the current source.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        ICapWrapper<Language> CapLanguage { get; }
        /// <summary>
        /// Gets the property to work with the max buffered pages for the current source.
        /// </summary>
        /// <value>
        /// The max batch buffered pages.
        /// </value>
        ICapWrapper<uint> CapMaxBatchBuffers { get; }
        /// <summary>
        /// Gets the property to work with check scanning support flag.
        /// </summary>
        /// <value>
        /// The check scanning support flag.
        /// </value>
        ICapWrapper<BoolType> CapMicrEnabled { get; }
        /// <summary>
        /// Gets the property to work with paper sensor flag for the current source.
        /// </summary>
        /// <value>
        /// The paper sensor flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapPaperDetectable { get; }
        /// <summary>
        /// Gets the property to work with paper handling option for the current source.
        /// </summary>
        /// <value>
        /// The paper handling option.
        /// </value>
        ICapWrapper<PaperHandling> CapPaperHandling { get; }
        /// <summary>
        /// Gets the property to work with camera power down time (seconds) for the current source.
        /// </summary>
        /// <value>
        /// The camera power down time.
        /// </value>
        ICapWrapper<int> CapPowerSaveTime { get; }
        /// <summary>
        /// Gets the property to see current device's power supply.
        /// </summary>
        /// <value>
        /// The power supply indicator.
        /// </value>
        IReadOnlyCapWrapper<PowerSupply> CapPowerSupply { get; }
        /// <summary>
        /// Gets the property to work with printer list for the current source.
        /// </summary>
        /// <value>
        /// The printer list.
        /// </value>
        ICapWrapper<Printer> CapPrinter { get; }
        /// <summary>
        /// Gets the property to work with printer character rotation for the current source.
        /// </summary>
        /// <value>
        /// The printer character rotation.
        /// </value>
        ICapWrapper<int> CapPrinterCharRotation { get; }
        /// <summary>
        /// Gets the property to work with printer enabled flag.
        /// </summary>
        /// <value>
        /// The printer enabled flag.
        /// </value>
        ICapWrapper<BoolType> CapPrinterEnabled { get; }
        /// <summary>
        /// Gets the property to work with printer font style for the current source.
        /// </summary>
        /// <value>
        /// The printer font style.
        /// </value>
        ICapWrapper<PrinterFontStyle> CapPrinterFontStyle { get; }
        /// <summary>
        /// Gets the property to work with the starting printer index for the current source.
        /// </summary>
        /// <value>
        /// The printer index.
        /// </value>
        ICapWrapper<int> CapPrinterIndex { get; }
        /// <summary>
        /// Set the character to be used for filling the leading digits before the counter value if the
        /// counter digits are fewer than <see cref="CapPrinterIndexNumDigits"/>.
        /// </summary>
        /// <value>
        /// The printer leading string.
        /// </value>
        ICapWrapper<string> CapPrinterIndexLeadChar { get; }
        /// <summary>
        /// Gets the property to work with printer index max value for the current source.
        /// </summary>
        /// <value>
        /// The printer index max value.
        /// </value>
        ICapWrapper<uint> CapPrinterIndexMaxValue { get; }
        /// <summary>
        /// Gets the property to work with printer number digits value for the current source.
        /// </summary>
        /// <value>
        /// The printer number digits value.
        /// </value>
        ICapWrapper<int> CapPrinterIndexNumDigits { get; }
        /// <summary>
        /// Gets the property to work with printer index step value for the current source.
        /// </summary>
        /// <value>
        /// The printer index step value.
        /// </value>
        ICapWrapper<int> CapPrinterIndexStep { get; }
        /// <summary>
        /// Gets the property to work with printer index trigger for the current source.
        /// </summary>
        /// <value>
        /// The printer index trigger.
        /// </value>
        ICapWrapper<PrinterIndexTrigger> CapPrinterIndexTrigger { get; }
        /// <summary>
        /// Gets the property to work with printer mode for the current source.
        /// </summary>
        /// <value>
        /// The printer mode.
        /// </value>
        ICapWrapper<PrinterMode> CapPrinterMode { get; }
        /// <summary>
        /// Specifies the string(s) that are to be used in the string component when the current <see cref="CapPrinter"/>
        /// device is enabled.
        /// </summary>
        /// <value>
        /// The printer string.
        /// </value>
        ICapWrapper<string> CapPrinterString { get; }
        /// <summary>
        /// Gets the next print values.
        /// </summary>
        /// <value>
        /// The next print values.
        /// </value>
        IReadOnlyCapWrapper<string> CapPrinterStringPreview { get; }
        /// <summary>
        /// Specifies the string that shall be used as the current <see cref="CapPrinter"/> device’s suffix.
        /// </summary>
        /// <value>
        /// The printer suffix string.
        /// </value>
        ICapWrapper<string> CapPrinterSuffix { get; }
        /// <summary>
        /// Gets the property to work with printer y-offset for the current source.
        /// </summary>
        /// <value>
        /// The printer y-offset.
        /// </value>
        ICapWrapper<TWFix32> CapPrinterVerticalOffset { get; }
        /// <summary>
        /// Gets the property to see whether device supports reacquire flag.
        /// </summary>
        /// <value>
        /// The reacquire flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapReacquireAllowed { get; }
        /// <summary>
        /// Gets the property to work with rewind page flag for the current source.
        /// </summary>
        /// <value>
        /// The rewind page flag.
        /// </value>
        ICapWrapper<BoolType> CapRewindPage { get; }
        /// <summary>
        /// Gets the property to work with segmentation setting for the current source.
        /// </summary>
        /// <value>
        /// The segmentation setting.
        /// </value>
        ICapWrapper<Segmented> CapSegmented { get; }
        /// <summary>
        /// Gets the property for device serial number.
        /// </summary>
        /// <value>
        /// The device serial number.
        /// </value>
        IReadOnlyCapWrapper<string> CapSerialNumber { get; }
        /// <summary>
        /// Gets the supported caps for the current source. This is not supported by all sources.
        /// </summary>
        /// <value>
        /// The supported caps.
        /// </value>
        IReadOnlyCapWrapper<CapabilityId> CapSupportedCaps { get; }
        /// <summary>
        /// Gets the supported caps for unique segments for the current source.
        /// </summary>
        /// <value>
        /// The supported caps for unique segments.
        /// </value>
        IReadOnlyCapWrapper<CapabilityId> CapSupportedCapsSegmentUnique { get; }
        /// <summary>
        /// Gets the supported caps for supported DATs.
        /// </summary>
        /// <value>
        /// The supported DATs.
        /// </value>
        IReadOnlyCapWrapper<uint> CapSupportedDATs { get; }
        /// <summary>
        /// Gets the property to work with thumbnails enabled flag for the current source.
        /// </summary>
        /// <value>
        /// The thumbnails enabled flag.
        /// </value>
        ICapWrapper<BoolType> CapThumbnailsEnabled { get; }
        /// <summary>
        /// Gets the property to work with the time before first capture (milliseconds) for the current source.
        /// </summary>
        /// <value>
        /// The time before first capture.
        /// </value>
        ICapWrapper<int> CapTimeBeforeFirstCapture { get; }
        /// <summary>
        /// Gets the property to work with the time between captures (milliseconds) for the current source.
        /// </summary>
        /// <value>
        /// The time between captures.
        /// </value>
        ICapWrapper<int> CapTimeBetweenCaptures { get; }
        /// <summary>
        /// Gets the property to get the image acquired time and date.
        /// </summary>
        /// <value>
        /// The time and date string.
        /// </value>
        IReadOnlyCapWrapper<string> CapTimeDate { get; }
        /// <summary>
        /// Gets the property to work with UI controllable flag for the current source.
        /// </summary>
        /// <value>
        /// The UI controllable flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> CapUIControllable { get; }
        /// <summary>
        /// Gets the property to work with xfer count for the current source.
        /// </summary>
        /// <value>
        /// The xfer count.
        /// </value>
        ICapWrapper<int> CapXferCount { get; }


        /// <summary>
        /// Gets the property to work with image auto brightness flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto brightness flag.
        /// </value>
        ICapWrapper<BoolType> ICapAutoBright { get; }
        /// <summary>
        /// Gets the property to work with image blank page behavior for the current source.
        /// </summary>
        /// <value>
        /// The image blank page behavior.
        /// </value>
        ICapWrapper<BlankPage> ICapAutoDiscardBlankPages { get; }
        /// <summary>
        /// Gets the property to work with auto border detection flag for the current source.
        /// </summary>
        /// <value>
        /// The auto border detection flag.
        /// </value>
        ICapWrapper<BoolType> ICapAutomaticBorderDetection { get; }
        /// <summary>
        /// Gets the property to work with image auto color detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto color detection flag.
        /// </value>
        ICapWrapper<BoolType> ICapAutomaticColorEnabled { get; }
        /// <summary>
        /// Gets the property to work with image auto non-color pixel type for the current source.
        /// </summary>
        /// <value>
        /// The image auto non-color pixel type.
        /// </value>
        ICapWrapper<PixelType> ICapAutomaticColorNonColorPixelType { get; }
        /// <summary>
        /// Gets the property to work with image auto crop flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto crop flag.
        /// </value>
        IReadOnlyCapWrapper<BoolType> ICapAutomaticCropUsesFrame { get; }
        /// <summary>
        /// Gets the property to work with image auto deskew flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto deskew flag.
        /// </value>
        ICapWrapper<BoolType> ICapAutomaticDeskew { get; }
        /// <summary>
        /// Gets the property to work with image auto length detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto length detection flag.
        /// </value>
        ICapWrapper<BoolType> ICapAutomaticLengthDetection { get; }
        /// <summary>
        /// Gets the property to work with image auto rotate flag for the current source.
        /// </summary>
        /// <value>
        /// The image auto rotate flag.
        /// </value>
        ICapWrapper<BoolType> ICapAutomaticRotate { get; }
        /// <summary>
        /// Gets the property to work with image auto size option for the current source.
        /// </summary>
        /// <value>
        /// The image auto size option.
        /// </value>
        ICapWrapper<AutoSize> ICapAutoSize { get; }
        /// <summary>
        /// Gets the property to work with image barcode detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image barcode detection flag.
        /// </value>
        ICapWrapper<BoolType> ICapBarcodeDetectionEnabled { get; }
        /// <summary>
        /// Gets the property to work with image barcode max search retries for the current source.
        /// </summary>
        /// <value>
        /// The image barcode max search retries.
        /// </value>
        ICapWrapper<uint> ICapBarcodeMaxRetries { get; }
        /// <summary>
        /// Gets the property to work with image barcode max search priorities for the current source.
        /// </summary>
        /// <value>
        /// The image barcode max search priorities.
        /// </value>
        ICapWrapper<uint> ICapBarcodeMaxSearchPriorities { get; }
        /// <summary>
        /// Gets the property to work with image barcode search direction for the current source.
        /// </summary>
        /// <value>
        /// The image barcode search direction.
        /// </value>
        ICapWrapper<BarcodeDirection> ICapBarcodeSearchMode { get; }
        /// <summary>
        /// Gets the property to work with image barcode search priority for the current source.
        /// </summary>
        /// <value>
        /// The image barcode search priority.
        /// </value>
        ICapWrapper<BarcodeType> ICapBarcodeSearchPriorities { get; }
        /// <summary>
        /// Gets the property to work with image barcode max search timeout for the current source.
        /// </summary>
        /// <value>
        /// The image barcode max search timeout.
        /// </value>
        ICapWrapper<uint> ICapBarcodeTimeout { get; }
        /// <summary>
        /// Gets the property to work with image bit depth for the current source.
        /// </summary>
        /// <value>
        /// The image bit depth.
        /// </value>
        ICapWrapper<int> ICapBitDepth { get; }
        /// <summary>
        /// Gets the property to work with image bit depth reduction method for the current source.
        /// </summary>
        /// <value>
        /// The image bit depth reduction method.
        /// </value>
        ICapWrapper<BitDepthReduction> ICapBitDepthReduction { get; }
        /// <summary>
        /// Gets the property to work with image <see cref="BitOrder"/> for the current source.
        /// </summary>
        /// <value>
        /// The image bit order.
        /// </value>
        ICapWrapper<BitOrder> ICapBitOrder { get; }
        /// <summary>
        /// Gets the property to work with image CCITT compression <see cref="BitOrder"/> for the current source.
        /// </summary>
        /// <value>
        /// The image bit order for CCITT compression.
        /// </value>
        ICapWrapper<BitOrder> ICapBitOrderCodes { get; }
        /// <summary>
        /// Gets the property to work with image brightness for the current source.
        /// </summary>
        /// <value>
        /// The image brightness.
        /// </value>
        ICapWrapper<TWFix32> ICapBrightness { get; }
        /// <summary>
        /// Gets the property to work with image CCITT K factor for the current source.
        /// </summary>
        /// <value>
        /// The image CCITT K factor.
        /// </value>
        ICapWrapper<int> ICapCCITTKFactor { get; }
        /// <summary>
        /// Gets the property to work with image color management flag for the current source.
        /// </summary>
        /// <value>
        /// The image color management flag.
        /// </value>
        ICapWrapper<BoolType> ICapColorManagementEnabled { get; }
        /// <summary>
        /// Gets the property to work with image <see cref="CompressionType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image compression.
        /// </value>
        ICapWrapper<CompressionType> ICapCompression { get; }
        /// <summary>
        /// Gets the property to work with image contrast for the current source.
        /// </summary>
        /// <value>
        /// The image contrast.
        /// </value>
        ICapWrapper<TWFix32> ICapContrast { get; }
        /// <summary>
        /// Gets the property to work with image square-cell halftone for the current source.
        /// </summary>
        /// <value>
        /// The image square-cell halftone.
        /// </value>
        ICapWrapper<byte> ICapCustHalftone { get; }
        /// <summary>
        /// Gets the property to work with image exposure time (in seconds) for the current source.
        /// </summary>
        /// <value>
        /// The image exposure time.
        /// </value>
        ICapWrapper<TWFix32> ICapExposureTime { get; }
        /// <summary>
        /// Gets the property to work with ext image info flag for the current source.
        /// </summary>
        /// <value>
        /// The ext image info flag.
        /// </value>
        ICapWrapper<BoolType> ICapExtImageInfo { get; }
        /// <summary>
        /// Gets the property to work with feeder type for the current source.
        /// </summary>
        /// <value>
        /// The feeder type.
        /// </value>
        ICapWrapper<FeederType> ICapFeederType { get; }
        /// <summary>
        /// Gets the property to work with image film type for the current source.
        /// </summary>
        /// <value>
        /// The image film type.
        /// </value>
        ICapWrapper<FilmType> ICapFilmType { get; }
        /// <summary>
        /// Gets the property to work with image color filter for the current source.
        /// </summary>
        /// <value>
        /// The image color filter type.
        /// </value>
        ICapWrapper<FilterType> ICapFilter { get; }
        /// <summary>
        /// Gets the property to work with flash option for the current source.
        /// </summary>
        /// <value>
        /// The flash option.
        /// </value>
        ICapWrapper<FlashedUsed> ICapFlashUsed2 { get; }
        /// <summary>
        /// Gets the property to work with image flip-rotation behavior for the current source.
        /// </summary>
        /// <value>
        /// The image flip-rotation behavior.
        /// </value>
        ICapWrapper<FlipRotation> ICapFlipRotation { get; }
        /// <summary>
        /// Gets the property to work with the list of frames the source will acquire on each page.
        /// </summary>
        /// <value>
        /// The capture frames.
        /// </value>
        ICapWrapper<TWFrame> ICapFrames { get; }
        /// <summary>
        /// Gets the property to work with image gamma value for the current source.
        /// </summary>
        /// <value>
        /// The image gamma.
        /// </value>
        ICapWrapper<TWFix32> ICapGamma { get; }
        /// <summary>
        /// Gets the property to work with image halftone patterns for the current source.
        /// </summary>
        /// <value>
        /// The image halftone patterns.
        /// </value>
        ICapWrapper<string> ICapHalftones { get; }
        /// <summary>
        /// Gets the property to work with image highlight value for the current source.
        /// </summary>
        /// <value>
        /// The image highlight.
        /// </value>
        ICapWrapper<TWFix32> ICapHighlight { get; }
        /// <summary>
        /// Gets the property to work with image icc profile for the current source.
        /// </summary>
        /// <value>
        /// The image icc profile.
        /// </value>
        ICapWrapper<IccProfile> ICapICCProfile { get; }
        /// <summary>
        /// Gets or sets the image indices that will be delivered during the standard image transfer done in
        /// States 6 and 7.
        /// </summary>
        /// <value>
        /// The image indicies.
        /// </value>
        ICapWrapper<uint> ICapImageDataSet { get; }
        /// <summary>
        /// Gets the property to work with image <see cref="FileFormat"/> for the current source.
        /// </summary>
        /// <value>
        /// The image file format.
        /// </value>
        ICapWrapper<FileFormat> ICapImageFileFormat { get; }
        /// <summary>
        /// Gets the property to work with image enhancement filter for the current source.
        /// </summary>
        /// <value>
        /// The image enhancement filter.
        /// </value>
        ICapWrapper<ImageFilter> ICapImageFilter { get; }
        /// <summary>
        /// Gets the property to work with image merge option for the current source.
        /// </summary>
        /// <value>
        /// The image merge option.
        /// </value>
        ICapWrapper<ImageMerge> ICapImageMerge { get; }
        /// <summary>
        /// Gets the property to work with image merge height threshold for the current source.
        /// </summary>
        /// <value>
        /// The image merge height threshold.
        /// </value>
        ICapWrapper<TWFix32> ICapImageMergeHeightThreshold { get; }
        /// <summary>
        /// Gets the property to work with image jpeg compression <see cref="PixelType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel type for jpeg compression.
        /// </value>
        ICapWrapper<PixelType> ICapJpegPixelType { get; }
        /// <summary>
        /// Gets the property to work with image jpeg quality for the current source.
        /// </summary>
        /// <value>
        /// The image jpeg quality.
        /// </value>
        ICapWrapper<JpegQuality> ICapJpegQuality { get; }
        /// <summary>
        /// Gets the property to work with image jpeg sub sampling for the current source.
        /// </summary>
        /// <value>
        /// The image jpeg sub sampling.
        /// </value>
        ICapWrapper<JpegSubsampling> ICapJpegSubsampling { get; }
        /// <summary>
        /// Gets the property to work with image lamp state flag for the current source.
        /// </summary>
        /// <value>
        /// The image lamp state flag.
        /// </value>
        ICapWrapper<BoolType> ICapLampState { get; }
        /// <summary>
        /// Gets the property to work with image light path for the current source.
        /// </summary>
        /// <value>
        /// The image light path.
        /// </value>
        ICapWrapper<LightPath> ICapLightPath { get; }
        /// <summary>
        /// Gets the property to work with image light source for the current source.
        /// </summary>
        /// <value>
        /// The image light source.
        /// </value>
        ICapWrapper<LightSource> ICapLightSource { get; }
        /// <summary>
        /// Gets the property to work with image max frames for the current source.
        /// </summary>
        /// <value>
        /// The image max frames.
        /// </value>
        ICapWrapper<int> ICapMaxFrames { get; }
        /// <summary>
        /// Gets the property to work with image minimum height for the current source.
        /// </summary>
        /// <value>
        /// The image minimumm height.
        /// </value>
        IReadOnlyCapWrapper<TWFix32> ICapMinimumHeight { get; }
        /// <summary>
        /// Gets the property to work with image minimum width for the current source.
        /// </summary>
        /// <value>
        /// The image minimumm width.
        /// </value>
        IReadOnlyCapWrapper<TWFix32> ICapMinimumWidth { get; }
        /// <summary>
        /// Gets the property to work with image mirror option for the current source.
        /// </summary>
        /// <value>
        /// The image mirror option.
        /// </value>
        ICapWrapper<Mirror> ICapMirror { get; }
        /// <summary>
        /// Gets the property to work with image noise filter for the current source.
        /// </summary>
        /// <value>
        /// The image noise filter.
        /// </value>
        ICapWrapper<NoiseFilter> ICapNoiseFilter { get; }
        /// <summary>
        /// Gets the property to work with image orientation for the current source.
        /// </summary>
        /// <value>
        /// The image orientation.
        /// </value>
        ICapWrapper<OrientationType> ICapOrientation { get; }
        /// <summary>
        /// Gets the property to work with image overscan option for the current source.
        /// </summary>
        /// <value>
        /// The image overscan option.
        /// </value>
        ICapWrapper<OverScan> ICapOverScan { get; }
        /// <summary>
        /// Gets the property to work with image patch code detection flag for the current source.
        /// </summary>
        /// <value>
        /// The image patch code detection flag.
        /// </value>
        ICapWrapper<BoolType> ICapPatchCodeDetectionEnabled { get; }
        /// <summary>
        /// Gets the property to work with image patch code max search retries for the current source.
        /// </summary>
        /// <value>
        /// The image patch code max search retries.
        /// </value>
        ICapWrapper<uint> ICapPatchCodeMaxRetries { get; }
        /// <summary>
        /// Gets the property to work with image patch code max search priorities for the current source.
        /// </summary>
        /// <value>
        /// The image patch code max search priorities.
        /// </value>
        ICapWrapper<uint> ICapPatchCodeMaxSearchPriorities { get; }
        /// <summary>
        /// Gets the property to work with image patch code search direction for the current source.
        /// </summary>
        /// <value>
        /// The image patch code search direction.
        /// </value>
        ICapWrapper<BarcodeDirection> ICapPatchCodeSearchMode { get; }
        /// <summary>
        /// Gets the property to work with image patch code search priority for the current source.
        /// </summary>
        /// <value>
        /// The image patch code search priority.
        /// </value>
        ICapWrapper<PatchCode> ICapPatchCodeSearchPriorities { get; }
        /// <summary>
        /// Gets the property to work with image patch code max search timeout for the current source.
        /// </summary>
        /// <value>
        /// The image patch code max search timeout.
        /// </value>
        ICapWrapper<uint> ICapPatchCodeTimeout { get; }
        /// <summary>
        /// Gets the property to work with image physical height for the current source.
        /// </summary>
        /// <value>
        /// The image physical height.
        /// </value>
        IReadOnlyCapWrapper<TWFix32> ICapPhysicalHeight { get; }
        /// <summary>
        /// Gets the property to work with image physical width for the current source.
        /// </summary>
        /// <value>
        /// The image physical width.
        /// </value>
        IReadOnlyCapWrapper<TWFix32> ICapPhysicalWidth { get; }
        /// <summary>
        /// Gets the property to work with image pixel flavor for the current source.
        /// </summary>
        /// <value>
        /// The image pixel flavor.
        /// </value>
        ICapWrapper<PixelFlavor> ICapPixelFlavor { get; }
        /// <summary>
        /// Gets the property to work with image CCITT compression <see cref="PixelFlavor"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel flavor for CCITT compression.
        /// </value>
        ICapWrapper<PixelFlavor> ICapPixelFlavorCodes { get; }
        /// <summary>
        /// Gets the property to work with image <see cref="PixelType"/> for the current source.
        /// </summary>
        /// <value>
        /// The image pixel type.
        /// </value>
        ICapWrapper<PixelType> ICapPixelType { get; }
        /// <summary>
        /// Gets the property to work with image color format for the current source.
        /// </summary>
        /// <value>
        /// The image color format.
        /// </value>
        ICapWrapper<PlanarChunky> ICapPlanarChunky { get; }
        /// <summary>
        /// Gets the property to work with image rotation for the current source.
        /// </summary>
        /// <value>
        /// The image rotation.
        /// </value>
        ICapWrapper<TWFix32> ICapRotation { get; }
        /// <summary>
        /// Gets the property to work with image shadow value for the current source.
        /// </summary>
        /// <value>
        /// The image shadow.
        /// </value>
        ICapWrapper<TWFix32> ICapShadow { get; }
        /// <summary>
        /// Gets the property to work with image barcode types for the current source.
        /// </summary>
        /// <value>
        /// The image barcode types.
        /// </value>
        IReadOnlyCapWrapper<BarcodeType> ICapSupportedBarcodeTypes { get; }
        /// <summary>
        /// Gets the property to get supported ext image info for the current source.
        /// </summary>
        /// <value>
        /// The supported ext image info.
        /// </value>
        IReadOnlyCapWrapper<ExtendedImageInfo> ICapSupportedExtImageInfo { get; }
        /// <summary>
        /// Gets the property to work with image patch code types for the current source.
        /// </summary>
        /// <value>
        /// The image patch code types.
        /// </value>
        IReadOnlyCapWrapper<PatchCode> ICapSupportedPatchCodeTypes { get; }
        /// <summary>
        /// Gets the property to work with image <see cref="SupportedSize"/> for the current source.
        /// </summary>
        /// <value>
        /// The image supported size.
        /// </value>
        ICapWrapper<SupportedSize> ICapSupportedSizes { get; }
        /// <summary>
        /// Gets the property to work with image threshold for the current source.
        /// </summary>
        /// <value>
        /// The image threshold.
        /// </value>
        ICapWrapper<TWFix32> ICapThreshold { get; }
        /// <summary>
        /// Gets the property to work with image tiles flag for the current source.
        /// </summary>
        /// <value>
        /// The image tiles flag.
        /// </value>
        ICapWrapper<BoolType> ICapTiles { get; }
        /// <summary>
        /// Gets the property to work with image CCITT time fill for the current source.
        /// </summary>
        /// <value>
        /// The image CCITT time fill.
        /// </value>
        ICapWrapper<int> ICapTimeFill { get; }
        /// <summary>
        /// Gets the property to work with image undefined size flag for the current source.
        /// </summary>
        /// <value>
        /// The image undefined size flag.
        /// </value>
        ICapWrapper<BoolType> ICapUndefinedImageSize { get; }
        /// <summary>
        /// Gets the property to work with image <see cref="Unit"/> for the current source.
        /// </summary>
        /// <value>
        /// The image unit of measure.
        /// </value>
        ICapWrapper<Unit> ICapUnits { get; }
        /// <summary>
        /// Gets the property to work with image <see cref="XferMech"/> for the current source.
        /// </summary>
        /// <value>
        /// The image xfer mech.
        /// </value>
        ICapWrapper<XferMech> ICapXferMech { get; }
        /// <summary>
        /// Gets the property to work with image's native x-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image's native x-axis resolution.
        /// </value>
        IReadOnlyCapWrapper<TWFix32> ICapXNativeResolution { get; }
        /// <summary>
        /// Gets the property to work with image x-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image x-axis resolution.
        /// </value>
        ICapWrapper<TWFix32> ICapXResolution { get; }
        /// <summary>
        /// Gets the property to work with image x-axis scaling for the current source.
        /// </summary>
        /// <value>
        /// The image x-axis scaling.
        /// </value>
        ICapWrapper<TWFix32> ICapXScaling { get; }
        /// <summary>
        /// Gets the property to work with image's native y-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image's native y-axis resolution.
        /// </value>
        IReadOnlyCapWrapper<TWFix32> ICapYNativeResolution { get; }
        /// <summary>
        /// Gets the property to work with image y-axis resolution for the current source.
        /// </summary>
        /// <value>
        /// The image y-axis resolution.
        /// </value>
        ICapWrapper<TWFix32> ICapYResolution { get; }
        /// <summary>
        /// Gets the property to work with image y-axis scaling for the current source.
        /// </summary>
        /// <value>
        /// The image y-axis scaling.
        /// </value>
        ICapWrapper<TWFix32> ICapYScaling { get; }
        /// <summary>
        /// Gets the property to work with image zoom factor for the current source.
        /// </summary>
        /// <value>
        /// The image zoom factor.
        /// </value>
        ICapWrapper<int> ICapZoomFactor { get; }


        /// <summary>
        /// Gets the current value for a capability.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        object GetCurrent(CapabilityId capabilityId);
        /// <summary>
        /// Gets the default value for a capability.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        object GetDefault(CapabilityId capabilityId);
        /// <summary>
        /// A general method that tries to get capability values from current <see cref="DataSource" />.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        IEnumerable<object> GetValues(CapabilityId capabilityId);
        /// <summary>
        /// Gets all the possible values of this capability without expanding.
        /// This may be required to work with large range values that cannot be safely enumerated
        /// with <see cref="GetValues"/>.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        CapabilityReader GetValuesRaw(CapabilityId capabilityId);
        /// <summary>
        /// Gets the actual supported operations for a capability. This is not supported by all sources.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        QuerySupports? QuerySupport(CapabilityId capabilityId);
        /// <summary>
        /// Resets the current value to power-on default.
        /// </summary>
        /// <param name="capabilityId">The capability id.</param>
        /// <returns></returns>
        ReturnCode Reset(CapabilityId capabilityId);
        /// <summary>
        /// Resets all values and constraint to power-on defaults.
        /// </summary>
        /// <returns></returns>
        ReturnCode ResetAll();
    }
}