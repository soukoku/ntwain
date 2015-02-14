using NTwain.Data;
using System;
using System.Collections.Generic;

namespace NTwain
{
    /// <summary>
    /// Contains the minimum TWAIN protocol version for various things.
    /// </summary>
    public static class ProtocolVersions
    {
        internal static readonly Version v10 = new Version(1, 0);
        internal static readonly Version v11 = new Version(1, 1);
        internal static readonly Version v15 = new Version(1, 5);
        internal static readonly Version v16 = new Version(1, 6);
        internal static readonly Version v17 = new Version(1, 7);
        internal static readonly Version v18 = new Version(1, 8);
        internal static readonly Version v19 = new Version(1, 9);
        internal static readonly Version v20 = new Version(2, 0);
        internal static readonly Version v21 = new Version(2, 1);
        internal static readonly Version v22 = new Version(2, 2);
        internal static readonly Version v23 = new Version(2, 3);


        static readonly Dictionary<CapabilityId, Version> __capMinVersions = new Dictionary<CapabilityId, Version>
        {
            { CapabilityId.ACapXferMech, v18 },

            { CapabilityId.CapAlarms, v18 },
            { CapabilityId.CapAlarmVolume, v18 },
            { CapabilityId.CapAuthor, v10 },
            { CapabilityId.CapAutoFeed, v10 },
            { CapabilityId.CapAutomaticCapture, v18 },
            { CapabilityId.CapAutomaticSenseMedium, v21 },
            { CapabilityId.CapAutoScan, v16 },
            { CapabilityId.CapBatteryMinutes, v18 },
            { CapabilityId.CapBatteryPercentage, v18 },
            { CapabilityId.CapCameraEnabled, v20 },
            { CapabilityId.CapCameraOrder, v20 },
            { CapabilityId.CapCameraPreviewUI, v18 },
            { CapabilityId.CapCameraSide, v19 },
            { CapabilityId.CapCaption, v10 },
            { CapabilityId.CapClearBuffers, v18 },
            { CapabilityId.CapClearPage, v10 },
            { CapabilityId.CapCustomDSData, v17 },
            { CapabilityId.CapCustomInterfaceGuid, v21 },
            { CapabilityId.CapDeviceEvent, v18 },
            { CapabilityId.CapDeviceOnline, v16 },
            { CapabilityId.CapDeviceTimeDate, v18 },
            { CapabilityId.CapDoubleFeedDetection, v22 },
            { CapabilityId.CapDoubleFeedDetectionLength, v22 },
            { CapabilityId.CapDoubleFeedDetectionResponse, v22 },
            { CapabilityId.CapDoubleFeedDetectionSensitivity, v22 },
            { CapabilityId.CapDuplex, v17 },
            { CapabilityId.CapDuplexEnabled, v17 },
            { CapabilityId.CapEnableDSUIOnly, v17 },
            { CapabilityId.CapEndorser, v17 },
            { CapabilityId.CapExtendedCaps, v10 },
            { CapabilityId.CapFeederAlignment, v18 },
            { CapabilityId.CapFeederEnabled, v10 },
            { CapabilityId.CapFeederLoaded, v10 },
            { CapabilityId.CapFeederOrder, v18 },
            { CapabilityId.CapFeederPocket, v20 },
            { CapabilityId.CapFeederPrep, v20},
            { CapabilityId.CapFeedPage, v10 },
            { CapabilityId.CapIndicators, v11 },
            { CapabilityId.CapIndicatorsMode, v22 },
            { CapabilityId.CapJobControl, v17 },
            { CapabilityId.CapLanguage, v18 },
            { CapabilityId.CapMaxBatchBuffers, v18 },
            { CapabilityId.CapMicrEnabled, v20 },
            { CapabilityId.CapPaperDetectable, v16 },
            { CapabilityId.CapPaperHandling, v22 },
            { CapabilityId.CapPowerSaveTime, v18 },
            { CapabilityId.CapPowerSupply, v18 },
            { CapabilityId.CapPrinter, v18 },
            { CapabilityId.CapPrinterEnabled, v18 },
            { CapabilityId.CapPrinterCharRotation, v23 },
            { CapabilityId.CapPrinterFontStyle, v23 },
            { CapabilityId.CapPrinterIndex, v18 },
            { CapabilityId.CapPrinterIndexLeadChar, v23 },
            { CapabilityId.CapPrinterIndexMaxValue, v23 },
            { CapabilityId.CapPrinterIndexNumDigits, v23 },
            { CapabilityId.CapPrinterIndexStep, v23 },
            { CapabilityId.CapPrinterIndexTrigger, v23 },
            { CapabilityId.CapPrinterMode, v18 },
            { CapabilityId.CapPrinterString, v18 },
            { CapabilityId.CapPrinterStringPreview, v23 },
            { CapabilityId.CapPrinterSuffix, v18 },
            { CapabilityId.CapPrinterVerticalOffset, v22 },
            { CapabilityId.CapReacquireAllowed, v18 },
            { CapabilityId.CapRewindPage, v10 },
            { CapabilityId.CapSegmented, v19 },
            { CapabilityId.CapSerialNumber, v18 },
            { CapabilityId.CapSupportedCaps, v10 },
            { CapabilityId.CapSupportedCapsSegmentUnique, v22 },
            { CapabilityId.CapSupportedDATs, v22 },
            { CapabilityId.CapTimeBeforeFirstCapture, v18 },
            { CapabilityId.CapTimeBetweenCaptures, v18 },
            { CapabilityId.CapTimeDate, v10 },
            { CapabilityId.CapThumbnailsEnabled, v17 },
            { CapabilityId.CapUIControllable, v16 },
            { CapabilityId.CapXferCount, v10 },

            { CapabilityId.ICapAutoBright, v10 },
            { CapabilityId.ICapAutoDiscardBlankPages, v20 },
            { CapabilityId.ICapAutomaticBorderDetection, v18 },
            { CapabilityId.ICapAutomaticColorEnabled, v21 },
            { CapabilityId.ICapAutomaticColorNonColorPixelType, v18 },
            { CapabilityId.ICapAutomaticCropUsesFrame, v21 },
            { CapabilityId.ICapAutomaticDeskew, v18 },
            { CapabilityId.ICapAutomaticLengthDetection, v21 },
            { CapabilityId.ICapAutomaticRotate, v18 },
            { CapabilityId.ICapAutoSize, v20 },
            { CapabilityId.ICapBarcodeDetectionEnabled, v18 },
            { CapabilityId.ICapBarcodeMaxRetries, v18 },
            { CapabilityId.ICapBarcodeMaxSearchPriorities, v18 },
            { CapabilityId.ICapBarcodeSearchMode, v18 },
            { CapabilityId.ICapBarcodeSearchPriorities, v18 },
            { CapabilityId.ICapBarcodeTimeout, v18 },
            { CapabilityId.ICapBitDepth, v10 },
            { CapabilityId.ICapBitDepthReduction, v15 },
            { CapabilityId.ICapBitOrder, v10 },
            { CapabilityId.ICapBitOrderCodes, v10 },
            { CapabilityId.ICapBrightness, v10 },
            { CapabilityId.ICapCCITTKFactor, v10 },
            { CapabilityId.ICapColorManagementEnabled, v21 },
            { CapabilityId.ICapCompression, v10 },
            { CapabilityId.ICapContrast, v10 },
            { CapabilityId.ICapCustHalftone, v10 },
            { CapabilityId.ICapExposureTime, v10 },
            { CapabilityId.ICapExtImageInfo, v17 },
            { CapabilityId.ICapFeederType, v19 },
            { CapabilityId.ICapFilmType, v22 },
            { CapabilityId.ICapFilter, v10 },
            { CapabilityId.ICapFlashUsed, v16 }, // maybe
            { CapabilityId.ICapFlashUsed2, v18 },
            { CapabilityId.ICapFlipRotation, v18 },
            { CapabilityId.ICapFrames, v10 },
            { CapabilityId.ICapGamma, v10 },
            { CapabilityId.ICapHalftones, v10 },
            { CapabilityId.ICapHighlight, v10 },
            { CapabilityId.ICapICCProfile, v19 },
            { CapabilityId.ICapImageDataSet, v17 },
            { CapabilityId.ICapImageFileFormat, v10 },
            { CapabilityId.ICapImageFilter, v18 },
            { CapabilityId.ICapImageMerge, v21 },
            { CapabilityId.ICapImageMergeHeightThreshold, v21 },
            { CapabilityId.ICapJpegPixelType, v10 },
            { CapabilityId.ICapJpegQuality, v19 },
            { CapabilityId.ICapJpegSubsampling, v22 },
            { CapabilityId.ICapLampState, v10 },
            { CapabilityId.ICapLightPath, v10 },
            { CapabilityId.ICapLightSource, v10 },
            { CapabilityId.ICapMaxFrames, v10 },
            { CapabilityId.ICapMinimumHeight, v17 },
            { CapabilityId.ICapMinimumWidth, v17 },
            { CapabilityId.ICapMirror, v22 },
            { CapabilityId.ICapNoiseFilter, v18 },
            { CapabilityId.ICapOrientation, v10 },
            { CapabilityId.ICapOverScan, v18 },
            { CapabilityId.ICapPatchCodeDetectionEnabled, v18 },
            { CapabilityId.ICapPatchCodeMaxRetries, v18 },
            { CapabilityId.ICapPatchCodeMaxSearchPriorities, v18 },
            { CapabilityId.ICapPatchCodeSearchMode, v18 },
            { CapabilityId.ICapPatchCodeSearchPriorities, v18 },
            { CapabilityId.ICapPatchCodeTimeout, v18 },
            { CapabilityId.ICapPhysicalHeight, v10 },
            { CapabilityId.ICapPhysicalWidth, v10 },
            { CapabilityId.ICapPixelFlavor, v10 },
            { CapabilityId.ICapPixelFlavorCodes, v10 },
            { CapabilityId.ICapPixelType, v10 },
            { CapabilityId.ICapPlanarChunky, v10 },
            { CapabilityId.ICapRotation, v10 },
            { CapabilityId.ICapShadow, v10 },
            { CapabilityId.ICapSupportedBarcodeTypes, v18 },
            { CapabilityId.ICapSupportedExtImageInfo, v21 },
            { CapabilityId.ICapSupportedPatchCodeTypes, v18 },
            { CapabilityId.ICapSupportedSizes, v10 },
            { CapabilityId.ICapThreshold, v10 },
            { CapabilityId.ICapTiles, v10 },
            { CapabilityId.ICapTimeFill, v10 },
            { CapabilityId.ICapUndefinedImageSize, v16 },
            { CapabilityId.ICapUnits, v10 },
            { CapabilityId.ICapXferMech, v10 },
            { CapabilityId.ICapXNativeResolution, v10 },
            { CapabilityId.ICapXResolution, v10 },
            { CapabilityId.ICapXScaling, v10 },
            { CapabilityId.ICapYNativeResolution, v10 },
            { CapabilityId.ICapYResolution, v10 },
            { CapabilityId.ICapYScaling, v10 },
            { CapabilityId.ICapZoomFactor, v18 },
        };

        /// <summary>
        /// Gets the minimum TWAIN protocol version for a capability.
        /// </summary>
        /// <param name="id">The capability type.</param>
        /// <returns></returns>
        public static Version GetMinimumVersion(CapabilityId id)
        {
            if (__capMinVersions.ContainsKey(id))
            {
                return __capMinVersions[id];
            }
            return v10;
        }
    }
}
