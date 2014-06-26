using System;

namespace NTwain.Data
{
    // these are from the corresponding twain.h sections

    /****************************************************************************
     * Generic Constants                                                        *
     ****************************************************************************/

    #region gen constants

    /// <summary>
    /// Indicates the type of container used in capability.
    /// Corresponds to TWON_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ContainerType : ushort
    {
        /// <summary>
        /// The default value for this enum.
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// The container is <see cref="TWArray"/>.
        /// </summary>
        Array = 3,
        /// <summary>
        /// The container is <see cref="TWEnumeration"/>.
        /// </summary>
        Enum = 4,
        /// <summary>
        /// The container is <see cref="TWOneValue"/>.
        /// </summary>
        OneValue = 5,
        /// <summary>
        /// The container is <see cref="TWRange"/>.
        /// </summary>
        Range = 6,
        /// <summary>
        /// The don't care value.
        /// </summary>
        DontCare = TwainConst.DontCare16,
    }

    /// <summary>
    /// Flags used in <see cref="TWMemory"/>.
    /// Corresponds to TWMF_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Flags"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum MemoryFlags : uint
    {
        None = 0,
        AppOwns = 0x1,
        DsmOwns = 0x2,
        DSOwns = 0x4,
        Pointer = 0x8,
        Handle = 0x10
    }

    /// <summary>
    /// The data types of item in TWAIN, used in the
    /// capability containers.
    /// Corresponds to TWTY_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum ItemType : ushort
    {
        /// <summary>
        /// Means Item is a an 8 bit value.
        /// </summary>
        Int8 = 0,
        /// <summary>
        /// Means Item is a 16 bit value.
        /// </summary>
        Int16 = 1,
        /// <summary>
        /// Means Item is a 32 bit value.
        /// </summary>
        Int32 = 2,
        /// <summary>
        /// Means Item is an unsigned 8 bit value.
        /// </summary>
        UInt8 = 3,
        /// <summary>
        /// Means Item is an unsigned 16 bit value.
        /// </summary>
        UInt16 = 4,
        /// <summary>
        /// Means Item is an unsigned 32 bit value.
        /// </summary>
        UInt32 = 5,
        /// <summary>
        /// Means Item is an unsigned 16 bit value (supposedly, YMMV).
        /// </summary>
        Bool = 6,
        /// <summary>
        /// Means Item is a <see cref="Fix32"/>.
        /// </summary>
        Fix32 = 7,
        /// <summary>
        /// Means Item is a <see cref="Frame"/>.
        /// </summary>
        Frame = 8,
        /// <summary>
        /// Means Item is a 32 char string (max).
        /// </summary>
        String32 = 9,
        /// <summary>
        /// Means Item is a 64 char string (max).
        /// </summary>
        String64 = 0xa,
        /// <summary>
        /// Means Item is a 128 char string (max).
        /// </summary>
        String128 = 0xb,
        /// <summary>
        /// Means Item is a char string shorter than 255.
        /// </summary>
        String255 = 0xc,
        //String1024 = 0xd,
        //Unicode512 = 0xe,
        /// <summary>
        /// Means Item is a handle (pointer).
        /// </summary>
        Handle = 0xf
    }

    #endregion

    /****************************************************************************
     * Capability Constants                                                     *
     ****************************************************************************/

    #region cap constants

    /// <summary>
    /// CapAlarms values.
    /// Corresponds to TWAL_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum AlarmType : ushort
    {
        Alarm = 0,
        FeederError = 1,
        FeederWarning = 2,
        Barcode = 3,
        DoubleFeed = 4,
        Jam = 5,
        PatchCode = 6,
        Power = 7,
        Skew = 8
    }

    /// <summary>
    /// ICapAutoSize values.
    /// Corresponds to TWAS_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum AutoSize : ushort
    {
        None = 0,
        Auto = 1,
        Current = 2
    }

    /// <summary>
    /// The bar code’s orientation on the scanned image is described in
    /// reference to a Western-style interpretation of the image.
    /// Corresponds to TWBCOR_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum BarcodeRotation : uint
    {
        /// <summary>
        /// Normal reading orientation.
        /// </summary>
        Rot0 = 0,
        /// <summary>
        /// Rotated 90 degrees clockwise.
        /// </summary>
        Rot90 = 1,
        /// <summary>
        /// Rotated 180 degrees clockwise.
        /// </summary>
        Rot180 = 2,
        /// <summary>
        /// Rotated 270 degrees clockwise.
        /// </summary>
        Rot270 = 3,
        /// <summary>
        /// The orientation is not known.
        /// </summary>
        RotX = 4
    }

    /// <summary>
    /// ICapBarcodeSearchMode values,
    /// ICapPatchCodeSearchMode values.
    /// Corresponds to TWBD_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum BarcodeDirection : ushort
    {
        Horz = 0,
        Vert = 1,
        HorzVert = 2,
        VertHorz = 3
    }

    /// <summary>
    /// ICapBitOrder/ICapBitOrderCodes values.
    /// Corresponds to TWBO_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum BitOrder : ushort
    {
        LsbFirst = 0,
        MsbFirst = 1
    }

    /// <summary>
    /// ICapAutoDiscardBlankPages values.
    /// Corresponds to TWBP_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum BlankPage : short
    {
        Invalid = 0,
        Disable = -2,
        Auto = -1
    }

    /// <summary>
    /// Values for ICapBitDepthReduction.
    /// Corresponds to TWBR_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum BitDepthReduction : ushort
    {
        Threshold = 0,
        Halftone = 1,
        CustHalftone = 2,
        Diffusion = 3,
        DynamicThreashold = 4
    }

    /// <summary>
    /// ICapBarcodeSearchPriorities values,
    /// ICapSupportedBarcodeTypes values.
    /// Corresponds to TWBT_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum BarcodeType : ushort
    {
        ThreeOfNine = 0,
        TwoOfFiveInterleaved = 1,
        TwoOfFiveNonInterleaved = 2,
        Code92 = 3,
        Code128 = 4,
        Ucc128 = 5,
        Codabar = 6,
        UpcA = 7,
        UpcE = 8,
        Ean8 = 9,
        Ean13 = 10,
        PostNet = 11,
        Pdf417 = 12,
        TwoOfFiveIndustrial = 13,
        TwoOfFiveMatrix = 14,
        TwoOfFiveDataLogic = 15,
        TwoOfFiveIata = 16,
        ThreeOfNineFullAscii = 17,
        CodabarWithStartStop = 18,
        MaxiCode = 19,
        QRCode = 20
    }

    /// <summary>
    /// ICapCompression values. Allows the application and Source to identify which compression schemes they have in common
    /// for Buffered Memory and File transfers. Since only certain file formats support compression, this capability must be negotiated after
    /// setting the desired file format with ICAP_IMAGEFILEFORMAT.
    /// Corresponds to TWCP_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CompressionType : ushort
    {
        /// <summary>
        /// All Sources must support this.
        /// </summary>
        None = 0,
        /// <summary>
        /// Can be used with TIFF or PICT
        /// </summary>
        PackBits = 1,
        /// <summary>
        /// From the CCITT specification (now ITU), intended for document images (can be used with TIFF).
        /// </summary>
        Group31D = 2,
        /// <summary>
        /// From the CCITT specification (now ITU), intended for document images (can be used with TIFF).
        /// </summary>
        Group31DEol = 3,
        /// <summary>
        /// From the CCITT specification (now ITU), intended for document images (can be used with TIFF).
        /// </summary>
        Group32D = 4,
        /// <summary>
        /// From the CCITT specification (now ITU), intended for document images (can be used with TIFF).
        /// </summary>
        Group4 = 5,
        /// <summary>
        /// Intended for the compression of color photographs (can be used with TIFF, JFIF or SPIFF).
        /// </summary>
        Jpeg = 6,
        /// <summary>
        /// A compression licensed by UNISYS (can be used with TIFF).
        /// </summary>
        Lzw = 7,
        /// <summary>
        /// Intended for bitonal and grayscale document images (can be used with TIFF or SPIFF).
        /// </summary>
        Jbig = 8,
        /// <summary>
        /// This compression can only be used if ICAP_IMAGEFILEFORMAT is set to TWFF_PNG.
        /// </summary>
        Png = 9,
        /// <summary>
        /// Can only be used if ICAP_IMAGEFILEFORMAT is set to TWFF_BMP.
        /// </summary>
        Rle4 = 10,
        /// <summary>
        /// Can only be used if ICAP_IMAGEFILEFORMAT is set to TWFF_BMP.
        /// </summary>
        Rle8 = 11,
        /// <summary>
        /// Can only be used if ICAP_IMAGEFILEFORMAT is set to TWFF_BMP.
        /// </summary>
        BitFields = 12,
        /// <summary>
        /// Per RFC 1951 (AKA 'Flate' and 'Deflate')
        /// </summary>
        Zip = 13,
        /// <summary>
        /// Per ISO/IEC 15444
        /// </summary>
        Jpeg2000 = 14
    }

    /// <summary>
    /// CapCameraSide/TWEI_PAGESIDE values.
    /// Corresponds to TWCS_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CameraSide : ushort
    {
        Both = 0,
        Top = 1,
        Bottom = 2
    }

    /// <summary>
    /// CapClearBuffers values.
    /// Corresponds to TWCB_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ClearBuffer : ushort
    {
        Auto = 0,
        Clear = 1,
        NoClear = 2
    }

    /// <summary>
    /// Indicates the type of event from the source.
    /// Also CapDeviceEvent values. If used as
    /// a cap value it's ushort.
    /// Corresponds to TWDE_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum DeviceEvent : uint // using uint to support custom event values
    {
        CheckAutomaticCapture = 0,
        CheckBattery = 1,
        CheckDeviceOnline = 2,
        CheckFlash = 3,
        CheckPowerSupply = 4,
        CheckResolution = 5,
        DeviceAdded = 6,
        DeviceOffline = 7,
        DeviceReady = 8,
        DeviceRemoved = 9,
        ImageCaptured = 10,
        ImageDeleted = 11,
        PaperDoubleFeed = 12,
        PaperJam = 13,
        LampFailure = 14,
        PowerSave = 15,
        PowerSaveNotify = 16,

        CustomEvents = 0x8000
    }

    /// <summary>
    /// <see cref="TWPassThru.Direction"/> values.
    /// Corresponds to TWDR_* values.
    /// </summary>
    public enum Direction // using int to match TWPassThru
    {
        Invalid = 0,
        Get = 1,
        Set = 2
    }

    /// <summary>
    /// TWEI_DESKEWSTATUS values.
    /// Corresponds to TWDSK_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DeskewStatus : uint
    {
        Success = 0,
        ReportOnly = 1,
        Fail = 2,
        Disabled = 3
    }

    /// <summary>
    /// CapDuplex values.
    /// Corresponds to TWDX_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Duplex : ushort
    {
        None = 0,
        OnePass = 1,
        TwoPass = 2
    }

    /// <summary>
    /// CapFeederAlignment values.
    /// Corresponds to TWFA_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FeederAlignment : ushort
    {
        None = 0,
        Left = 1,
        Center = 2,
        Right = 3
    }

    /// <summary>
    /// ICapFeederType values.
    /// Corresponds to TWFE_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FeederType : ushort
    {
        General = 0,
        Photo = 1
    }

    /// <summary>
    /// ICapImageFileFormat values.
    /// Corresponds to TWFF_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum FileFormat : ushort
    {
        /// <summary>
        /// Used for document imaging. Native Linux format.
        /// </summary>
        Tiff = 0,
        /// <summary>
        /// Native Macintosh format
        /// </summary>
        Pict = 1,
        /// <summary>
        /// Native Microsoft format
        /// </summary>
        Bmp = 2,
        /// <summary>
        /// Used for document imaging
        /// </summary>
        Xbm = 3,
        /// <summary>
        /// Wrapper for JPEG images
        /// </summary>
        Jfif = 4,
        /// <summary>
        /// FlashPix, used with digital cameras
        /// </summary>
        Fpx = 5,
        /// <summary>
        /// Multi-page TIFF files
        /// </summary>
        TiffMulti = 6,
        /// <summary>
        /// An image format standard intended for use on the web, replaces GIF
        /// </summary>
        Png = 7,
        /// <summary>
        /// A standard from JPEG, intended to replace JFIF, also supports JBIG
        /// </summary>
        Spiff = 8,
        /// <summary>
        /// File format for use with digital cameras.
        /// </summary>
        Exif = 9,
        /// <summary>
        /// A file format from Adobe
        /// </summary>
        Pdf = 10,
        /// <summary>
        /// A file format from the Joint Photographic Experts Group ISO/IEC 15444-1
        /// </summary>
        Jp2 = 11,
        /// <summary>
        /// A file format from the Joint Photographic Experts Group ISO/IEC 15444-2
        /// </summary>
        Jpx = 13,
        /// <summary>
        /// A file format from LizardTech
        /// </summary>
        Dejavu = 14,
        /// <summary>
        /// A file format from Adobe PDF/A, Version 1
        /// </summary>
        PdfA = 15,
        /// <summary>
        /// A file format from Adobe PDF/A, Version 2
        /// </summary>
        PdfA2 = 16
    }

    /// <summary>
    /// ICapFlashUsed2 values.
    /// Corresponds to TWFL_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FlashedUsed : ushort
    {
        None = 0,
        Off = 1,
        On = 2,
        Auto = 3,
        Redeye = 4
    }

    /// <summary>
    /// CapFeederOrder values.
    /// Corresponds to TWFO_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FeederOrder : ushort
    {
        FirstPageFirst = 0,
        LastPageFirst = 1
    }

    /// <summary>
    /// CapFeederPocket values.
    /// Corresponds to TWFP_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FeederPocket : ushort
    {
        PocketError = 0,
        Pocket1 = 1,
        Pocket2 = 2,
        Pocket3 = 3,
        Pocket4 = 4,
        Pocket5 = 5,
        Pocket6 = 6,
        Pocket7 = 7,
        Pocket8 = 8,
        Pocket9 = 9,
        Pocket10 = 10,
        Pocket11 = 11,
        Pocket12 = 12,
        Pocket13 = 13,
        Pocket14 = 14,
        Pocket15 = 15,
        Pocket16 = 16,
    }

    /// <summary>
    /// ICapFlipRotation values.
    /// Corresponds to TWFR_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FlipRotation : ushort
    {
        Book = 0,
        Fanfold = 1
    }

    /// <summary>
    /// ICapFilter values.
    /// Corresponds to TWFT_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FilterType : ushort
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        None = 3,
        White = 4,
        Cyan = 5,
        Magenta = 6,
        Yellow = 7,
        Black = 8
    }

    /// <summary>
    /// <see cref="TWFileSystem.FileType"/> values.
    /// Corresponds to TWFY_* values.
    /// </summary>
    public enum FileType // using int to match value
    {
        Camera = 0,
        CameraTop = 1,
        CameraBottom = 2,
        CameraPreview = 3,
        Domain = 4,
        Host = 5,
        Directory = 6,
        Image = 7,
        Unknown = 8
    }

    /// <summary>
    /// ICapIccProfile values.
    /// Corresponds to TWIC* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum IccProfile : ushort
    {
        None = 0,
        Link = 1,
        Embed = 2
    }

    /// <summary>
    /// ICapImageFilter values.
    /// Corresponds to TWIF_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ImageFilter : ushort
    {
        None = 0,
        Auto = 1,
        LowPass = 2,
        BandPass = 3,
        HighPass = 4,
        Text = BandPass,
        FineLine = HighPass
    }

    /// <summary>
    /// ICapImageMerge values.
    /// Corresponds to TWIM_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ImageMerge : ushort
    {
        None = 0,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "FrontOn")]
        FrontOnTop = 1,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "FrontOn")]
        FrontOnBottom = 2,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "FrontOn")]
        FrontOnLeft = 3,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "FrontOn")]
        FrontOnRight = 4
    }

    /// <summary>
    /// CapJobControl values.
    /// Corresponds to TWJC_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum JobControl : ushort
    {
        /// <summary>
        /// No job control.
        /// </summary>
        None = 0,
        /// <summary>
        /// Detect and include job separator and continue scanning.
        /// </summary>
        IncludeContinue = 1,
        /// <summary>
        /// Detect and include job separator and stop scanning.
        /// </summary>
        IncludeStop = 2,
        /// <summary>
        /// Detect and exclude job separator and continue scanning.
        /// </summary>
        ExcludeContinue = 3,
        /// <summary>
        /// Detect and exclude job separator and stop scanning.
        /// </summary>
        ExcludeStop = 4
    }

    /// <summary>
    /// ICapJpegQuality values.
    /// Corresponds to TWJQ_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum JpegQuality : short
    {
        Invalid = 0,
        Unknown = -4,
        Low = -3,
        Medium = -2,
        High = -1
    }

    /// <summary>
    /// ICapLightPath values.
    /// Corresponds to TWLP_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum LightPath : ushort
    {
        Reflective = 0,
        Transmissive = 1
    }

    /// <summary>
    /// ICapLightSource values.
    /// Corresponds to TWLS_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum LightSource : ushort
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        None = 3,
        White = 4,
        UV = 5,
        IR = 6
    }

    /// <summary>
    /// TWEI_MAGTYPE values.
    /// Corresponds to TWMD_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum MagType : ushort
    {
        Micr = 0,
        Raw = 1,
        Invalid = 2
    }

    /// <summary>
    /// ICapNoiseFilter values.
    /// Corresponds to TWNF_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum NoiseFilter : ushort
    {
        None = 0,
        Auto = 1,
        LonePixel = 2,
        MajorityRule = 3
    }

    /// <summary>
    /// ICapOrientation values.
    /// Corresponds to TWOR_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum OrientationType : ushort
    {
        Rot0 = 0,
        Rot90 = 1,
        Rot180 = 2,
        Rot270 = 3,
        Portrait = Rot0,
        Landscape = Rot270,
        Auto = 4,
        AutoTet = 5,
        AutoPicture = 6
    }

    /// <summary>
    /// ICapOverscan values.
    /// Corresponds to TWOV_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum OverScan : ushort
    {
        None = 0,
        Auto = 1,
        TopBottom = 2,
        LeftRight = 3,
        All = 4
    }

    /// <summary>
    /// <see cref="TWPalette8.PaletteType"/> values.
    /// Corresponds to TWPA_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PaletteType : ushort
    {
        Rgb = 0,
        Gray = 1,
        Cmy = 2
    }

    /// <summary>
    /// ICapPlanarChunky values.
    /// Corresponds to TWPC_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PlanarChunky : ushort
    {
        Chunky = 0,
        Planar = 1
    }

    /// <summary>
    /// ICapPatchCodeSearchPriorities,
    /// ICapSupportedPatchCodeTypes values.
    /// Corresponds to TWPCH_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PatchCode : ushort
    {
        Patch1 = 0,
        Patch2 = 1,
        Patch3 = 2,
        Patch4 = 3,
        Patch6 = 4,
        PatchT = 5
    }

    /// <summary>
    /// ICapPixelFlavor, ICapPixelFlavorCodes values.
    /// Corresponds to TWPF_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PixelFlavor : ushort
    {
        Chocolate = 0,
        Vanilla = 1
    }

    /// <summary>
    /// CapPrinterMode values.
    /// Corresponds to TWPM_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PrinterMode : ushort
    {
        SingleString = 0,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "MultiString")]
        MultiString = 1,
        CompoundString = 2
    }

    /// <summary>
    /// CapPrinter values.
    /// Corresponds to TWPR_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Printer : ushort
    {
        ImprinterTopBefore = 0,
        ImprinterTopAfter = 1,
        ImprinterBottomBefore = 2,
        ImprinterBottomAfter = 3,
        EndorserTopBefore = 4,
        EndorserTopAfter = 5,
        EndorserBottomBefore = 6,
        EndorserBottomAfter = 7
    }

    /// <summary>
    /// CapPrinterFontStyle values.
    /// Corresponds to TWPF_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PrinterFontStyle : ushort
    {
        Normal = 0,
        Bold = 1,
        Italic = 2,
        LargeSize = 3,
        SmallSize = 4
    }

    /// <summary>
    /// CapPrinterIndexTrigger values.
    /// Corresponds to TWCT_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PrinterIndexTrigger : ushort
    {
        Page = 0,
        Patch1 = 1,
        Patch2 = 2,
        Patch3 = 3,
        Patch4 = 4,
        PatchT = 5,
        Patch6 = 6
    }

    /// <summary>
    /// CapPowerSupply values.
    /// Corresponds to TWPS_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PowerSupply : ushort
    {
        External = 0,
        Battery = 1
    }

    /// <summary>
    /// Also ICapAutomaticColorNonColorPixelType (bw/gray),
    /// ICapJpegPixelType, ICapPixelType values.
    /// Corresponds to TWPT_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum PixelType : ushort
    {
        BlackWhite = 0,
        Gray = 1,
        RGB = 2,
        Palette = 3,
        CMY = 4,
        CMYK = 5,
        YUV = 6,
        YUVK = 7,
        CieXYZ = 8,
        Lab = 9,
        SRGB = 10,
        SCRGB = 11,
        Infrared = 16
    }

    /// <summary>
    /// CapSegmented values.
    /// Corresponds to TWSG_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Segmented : ushort
    {
        None = 0,
        Auto = 1,
        Manual = 2
    }

    /// <summary>
    /// ICapFilmType values.
    /// Corresponds to TWFM_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum FilmType : ushort
    {
        Positive = 0,
        Negative = 1
    }

    /// <summary>
    /// CapDoubleFeedDetection values.
    /// Corresponds to TWDF_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DoubleFeedDetection : ushort
    {
        Ultrasonic = 0,
        ByLength = 1,
        Infrared = 2
    }

    /// <summary>
    /// CapDoubleFeedDetectionSensitivity values.
    /// Corresponds to TWUS_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DoubleFeedDetectionSensitivity : ushort
    {
        Low = 0,
        Medium = 1,
        High = 2
    }

    /// <summary>
    /// CapDoubleFeedDetectionResponse values.
    /// Corresponds to TWDP_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DoubleFeedDetectionResponse : ushort
    {
        Stop = 0,
        StopAndWait = 1,
        Sound = 2,
        DoNotImprint = 3
    }

    /// <summary>
    /// ICapMirror values.
    /// Corresponds to TWMR* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Mirror : ushort
    {
        None = 0,
        Vertical = 1,
        Horizontal = 2
    }

    /// <summary>
    /// ICapJpegSubsampling values.
    /// Corresponds to TWJS_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum JpegSubsampling : ushort
    {
        x444YCBCR = 0,
        x444RGB = 1,
        x422 = 2,
        x421 = 3,
        x411 = 4,
        x420 = 5,
        x410 = 6,
        x311 = 7
    }

    /// <summary>
    /// CapPaperHandling values.
    /// Corresponds to TWPH_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum PaperHandling : ushort
    {
        Normal = 0,
        Fragile = 1,
        Thick = 2,
        Trifold = 3,
        Photograph = 4
    }

    /// <summary>
    /// CapIndicatorsMode values.
    /// Corresponds to TWCI_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum IndicatorsMode : ushort
    {
        Info = 0,
        Warning = 1,
        Error = 2,
        Warmup = 3
    }

    /// <summary>
    /// ICapSupportedSizes values.
    /// Corresponds to TWSS_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum SupportedSize : ushort
    {
        None = 0,
        A4 = 1,
        JisB5 = 2,
        USLetter = 3,
        USLegal = 4,
        A5 = 5,
        IsoB4 = 6,
        IsoB6 = 7,
        USLedger = 9,
        USExecutive = 10,
        A3 = 11,
        IsoB3 = 12,
        A6 = 13,
        C4 = 14,
        C5 = 15,
        C6 = 16,
        FourA0 = 17,
        TwoA0 = 18,
        A0 = 19,
        A1 = 20,
        A2 = 21,
        A7 = 22,
        A8 = 23,
        A9 = 24,
        A10 = 25,
        IsoB0 = 26,
        IsoB1 = 27,
        IsoB2 = 28,
        IsoB5 = 29,
        IsoB7 = 30,
        IsoB8 = 31,
        IsoB9 = 31,
        IsoB10 = 33,
        JisB0 = 34,
        JisB1 = 35,
        JisB2 = 36,
        JisB3 = 37,
        JisB4 = 38,
        JisB6 = 39,
        JisB7 = 40,
        JisB8 = 41,
        JisB9 = 42,
        JisB10 = 43,
        C0 = 44,
        C1 = 45,
        C2 = 46,
        C3 = 47,
        C7 = 48,
        C8 = 49,
        C9 = 50,
        C10 = 51,
        USStatement = 52,
        BusinessCard = 53,
        MaxSize = 54
    }

    /// <summary>
    /// ICapXferMech, ACapXferMech values.
    /// Corresponds to TWSX_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum XferMech : ushort
    {
        Native = 0,
        File = 1,
        Memory = 2,
        MemFile = 4
    }

    /// <summary>
    /// ICapUnits values.
    /// Corresponds to TWUN_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Unit : ushort
    {
        Inches = 0,
        Centimeters = 1,
        Picas = 2,
        Points = 3,
        Twips = 4,
        Pixels = 5,
        Millimeters = 6
    }

    #endregion

    /****************************************************************************
     * others                                                                   *
     ****************************************************************************/

    /// <summary>
    /// Corresponds to TWCY_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Country : ushort
    {
        None = 0,
        Afghanistan = 1001,
        Algeria = 213,
        AmericanSamoa = 684,
        Andorra = 033,
        Angola = 1002,
        Anguilla = 8090,
        Antigua = 8091,
        Argentina = 54,
        Aruba = 297,
        AscensionI = 247,
        Australia = 61,
        Austria = 43,
        Bahamas = 8092,
        Bahrain = 973,
        Bangladesh = 880,
        Barbados = 8093,
        Belgium = 32,
        Belize = 501,
        Benin = 229,
        Bermuda = 8094,
        Bhutan = 1003,
        Bolivia = 591,
        Botswana = 267,
        Britain = 6,
        BritVirginIs = 8095,
        Brazil = 55,
        Brunei = 673,
        Bulgaria = 359,
        BurkinaFaso = 1004,
        Burma = 1005,
        Burundi = 1006,
        Camaroon = 237,
        Canada = 2,
        CapeVerdeIs = 238,
        CaymanIs = 8096,
        CentralAfrep = 1007,
        Chad = 1008,
        Chile = 56,
        China = 86,
        ChristmasIs = 1009,
        CocosLs = 1009,
        Colombia = 57,
        Comoros = 1010,
        Congo = 1011,
        CookIs = 1012,
        Costarica = 506,
        Cuba = 005,
        Cyprus = 357,
        Czechoslovakia = 42,
        Denmark = 45,
        Djibouti = 1013,
        Dominica = 8097,
        DomincanRep = 8098,
        EasterIs = 1014,
        Ecuador = 593,
        Egypt = 20,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "El")]
        ElSalvador = 503,
        Eqguinea = 1015,
        Ethiopia = 251,
        FalklandIs = 1016,
        FaeroeIs = 298,
        FijiIslands = 679,
        Finland = 358,
        France = 33,
        Frantilles = 596,
        Frguiana = 594,
        Frpolyneisa = 689,
        Futanais = 1043,
        Gabon = 241,
        Gambia = 220,
        Germany = 49,
        Ghana = 233,
        Gibralter = 350,
        Greece = 30,
        Greenland = 299,
        Grenada = 8099,
        Grenedines = 8015,
        Guadeloupe = 590,
        Guam = 671,
        GuantanamoBay = 5399,
        Guatemala = 502,
        Guinea = 224,
        GuineaBissau = 1017,
        Guyana = 592,
        Haiti = 509,
        Honduras = 504,
        HongKong = 852,
        Hungary = 36,
        Iceland = 354,
        India = 91,
        Indonesia = 62,
        Iran = 98,
        Iraq = 964,
        Ireland = 353,
        Israel = 972,
        Italy = 39,
        IvoryCoast = 225,
        Jamaica = 8010,
        Japan = 81,
        Jordan = 962,
        Kenya = 254,
        Kiribati = 1018,
        Korea = 82,
        Kuwait = 965,
        Laos = 1019,
        Lebanon = 1020,
        Liberia = 231,
        Libya = 218,
        Liechtenstein = 41,
        Luxenbourg = 352,
        Macao = 853,
        Madagascar = 1021,
        Malawi = 265,
        Malaysia = 60,
        Maldives = 960,
        Mali = 1022,
        Malta = 356,
        MarshallIs = 692,
        Mauritania = 1023,
        Mauritius = 230,
        Mexico = 3,
        Micronesia = 691,
        Miquelon = 508,
        Monaco = 33,
        Mongolia = 1024,
        Montserrat = 8011,
        Morocco = 212,
        Mozambique = 1025,
        Namibia = 264,
        Nauru = 1026,
        Nepal = 977,
        Netherlands = 31,
        NethAntilles = 599,
        Nevis = 8012,
        NewCaledonia = 687,
        NewZealand = 64,
        Nicaragua = 505,
        Niger = 227,
        Nigeria = 234,
        Niue = 1027,
        NorfolkI = 1028,
        Norway = 47,
        Oman = 968,
        Pakistan = 92,
        Palau = 1029,
        Panama = 507,
        Paraguay = 595,
        Peru = 51,
        Phillippines = 63,
        PitcairnIs = 1030,
        PNewGuinea = 675,
        Poland = 48,
        Portugal = 351,
        Qatar = 974,
        Reunioni = 1031,
        Romania = 40,
        Rwanda = 250,
        Saipan = 670,
        SanMarino = 39,
        Saotome = 1033,
        SaudiArabia = 966,
        Senegal = 221,
        Seychellesis = 1034,
        Sierraleone = 1035,
        Singapore = 65,
        Solomonis = 1036,
        Somali = 1037,
        SouthAfrica = 27,
        Spain = 34,
        Srilanka = 94,
        Sthelena = 1032,
        Stkitts = 8013,
        Stlucia = 8014,
        Stpierre = 508,
        Stvincent = 8015,
        Sudan = 1038,
        Suriname = 597,
        Swaziland = 268,
        Sweden = 46,
        Switzerland = 41,
        Syria = 1039,
        Taiwan = 886,
        Tanzania = 255,
        Thailand = 66,
        Tobago = 8016,
        Togo = 228,
        Tongais = 676,
        Trinidad = 8016,
        Tunisia = 216,
        Turkey = 90,
        TurksCaicos = 8017,
        Tuvalu = 1040,
        Uganda = 256,
        Ussr = 7,
        UAEmirates = 971,
        UnitedKingdom = 44,
        Usa = 1,
        Uruguay = 598,
        Vanuatu = 1041,
        VaticanCity = 39,
        Venezuela = 58,
        Wake = 1042,
        WallisIs = 1043,
        WesternSahara = 1044,
        WesternSamoa = 1045,
        Yemen = 1046,
        Yugoslavia = 38,
        Zaire = 243,
        Zambia = 260,
        Zimbabwe = 263,
        Albania = 355,
        Armenia = 374,
        Azerbaijan = 994,
        Belarus = 375,
        BosniaHerzgo = 387,
        Cambodia = 855,
        Croatia = 385,
        CzechRepublic = 420,
        Diegogarcia = 246,
        Eritrea = 291,
        Estonia = 372,
        Georgia = 995,
        Latvia = 371,
        Lesotho = 266,
        Lithuania = 370,
        Macedonia = 389,
        Mayotteis = 269,
        Moldova = 373,
        Myanmar = 95,
        NorthKorea = 850,
        PuertoRico = 787,
        Russia = 7,
        Serbia = 381,
        Slovakia = 421,
        Slovenia = 386,
        SouthKorea = 82,
        Ukraine = 380,
        USVirginIs = 340,
        Vietnam = 84,
    }

    /// <summary>
    /// Corresponds to TWLG_* values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Language : short
    {
        UserLocale = -1,
        Danish = 0,
        Dutch = 1,
        English = 2,
        FrenchCanadian = 3,
        Finnish = 4,
        French = 5,
        German = 6,
        Icelandic = 7,
        Italian = 8,
        Norwegian = 9,
        Portuguese = 10,
        Spanish = 11,
        Swedish = 12,
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "USA")]
        EnglishUSA = 13,
        Afrikaans = 14,
        Albania = 15,
        Arabic = 16,
        ArabicAlgeria = 17,
        ArabicBahrain = 18,
        ArabicEgypt = 19,
        ArabicIraq = 20,
        ArabicJordan = 21,
        ArabicKuwait = 22,
        ArabicLebanon = 23,
        ArabicLibya = 24,
        ArabicMorocco = 25,
        ArabicOman = 26,
        ArabicQatar = 27,
        ArabicSaudiArabia = 28,
        ArabicSyria = 29,
        ArabicTunisia = 30,
        ArabicUAE = 31,
        ArabicYemen = 32,
        Basque = 33,
        Byelorussian = 34,
        Bulgarian = 35,
        Catalan = 36,
        Chinese = 37,
        ChineseHongKong = 38,
        ChinesePRC = 39,
        ChineseSingapore = 40,
        ChineseSimplified = 41,
        ChineseTaiwan = 42,
        ChineseTraditional = 43,
        Croatia = 44,
        Czech = 45,
        DutchBelgian = 46,
        EnglishAustralian = 47,
        EnglishCanadian = 48,
        EnglishIreland = 49,
        EnglishNewZealand = 50,
        EnglishSouthAfrica = 51,
        EnglishUK = 52,
        Estonian = 53,
        Faeroese = 54,
        Farsi = 55,
        FrenchBelgian = 56,
        FrenchLuxembourg = 57,
        FrenchSwiss = 58,
        GermanAustrian = 59,
        GermanLuxembourg = 60,
        GermanLiechtenstein = 61,
        GermanSwiss = 62,
        Greek = 63,
        Hebrew = 64,
        Hungarian = 65,
        Indonesian = 66,
        ItalianSwiss = 67,
        Japanese = 68,
        Korean = 69,
        KoreanJohab = 70,
        Latvian = 71,
        Lithuanian = 72,
        NorwegianBokmal = 73,
        NorwegianNynorsk = 74,
        Polish = 75,
        PortugueseBrazil = 76,
        Romanian = 77,
        Russian = 78,
        SerbianLatin = 79,
        Slovak = 80,
        Slovenian = 81,
        SpanishMexican = 82,
        SpanishModern = 83,
        Thai = 84,
        Turkish = 85,
        Ukranian = 86,
        Assamese = 87,
        Bengali = 88,
        Bihari = 89,
        Bodo = 90,
        Dogri = 91,
        Gujarati = 92,
        Haryanvi = 93,
        Hindi = 94,
        Kannada = 95,
        Kashmiri = 96,
        Malayalam = 97,
        Marathi = 98,
        Marwari = 99,
        Meghalayan = 100,
        Mizo = 101,
        Naga = 102,
        Orissi = 103,
        Punjabi = 104,
        Pushtu = 105,
        SerbianCyrillic = 106,
        Sikkimi = 107,
        SwedishFinland = 108,
        Tamil = 109,
        Telugu = 110,
        Tripuri = 111,
        Urdu = 112,
        Vietnamese = 113,
    }


    /// <summary>
    /// Corresponds to DG_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags]
    public enum DataGroups : uint
    {
        None = 0,
        Control = 0x1,
        Image = 0x2,
        Audio = 0x4,
        Mask = 0xffff,
    }

    /// <summary>
    /// Corresponds to DF_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    [Flags]
    public enum DataFunctionalities : uint
    {
        None = 0,
        Dsm2 = 0x10000000,
        App2 = 0x20000000,
        Ds2 = 0x40000000,
    }

    /// <summary>
    /// Corresponds to DAT_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum DataArgumentType : ushort
    {
        Null = 0,
        CustomBase = 0x8000,

        // control group
        Capability = 0x1,
        Event = 0x2,
        Identity = 0x3,
        Parent = 0x4,
        PendingXfers = 0x5,
        SetupMemXfer = 0x6,
        SetupFileXfer = 0x7,
        Status = 0x8,
        UserInterface = 0x9,
        XferGroup = 0xa,
        CustomDSData = 0xc,
        DeviceEvent = 0xd,
        FileSystem = 0xe,
        PassThru = 0xf,
        Callback = 0x10,
        StatusUtf8 = 0x11,
        Callback2 = 0x12,

        // image group
        ImageInfo = 0x101,
        ImageLayout = 0x102,
        ImageMemXfer = 0x103,
        ImageNativeXfer = 0x104,
        ImageFileXfer = 0x105,
        CieColor = 0x106,
        GrayResponse = 0x107,
        RgbResponse = 0x108,
        JpegCompression = 0x109,
        Palette8 = 0x10a,
        ExtImageInfo = 0x10b,
        Filter = 0x10c,

        // audio group
        AudioFileXfer = 0x201,
        AudioInfo = 0x202,
        AudioNativeXfer = 0x203,

        // crap
        IccProfile = 0x401,
        ImageMemFileXfer = 0x402,
        EntryPoint = 0x403,
    }

    /// <summary>
    /// Corresponds to MSG_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum Message : ushort
    {
        Null = 0,
        CustomBase = 0x8000,

        // Generic messages may be used with any of several DATs.
        Get = 0x1,
        GetCurrent = 0x2,
        GetDefault = 0x3,
        GetFirst = 0x4,
        GetNext = 0x5,
        Set = 0x6,
        Reset = 0x7,
        QuerySupport = 0x8,
        GetHelp = 0x9,
        GetLabel = 0xa,
        GetLabelEnum = 0xb,
        SetConstraint = 0xc,

        // Messages used with DAT_NULL
        XferReady = 0x101,
        CloseDSReq = 0x102,
        CloseDSOK = 0x103,
        DeviceEvent = 0x104,

        // Messages used with a pointer to DAT_PARENT data    
        OpenDSM = 0x301,
        CloseDSM = 0x302,

        // Messages used with a pointer to a DAT_IDENTITY structure 
        OpenDS = 0x401,
        CloseDS = 0x402,
        UserSelect = 0x403,

        // Messages used with a pointer to a DAT_USERINTERFACE structure
        DisableDS = 0x501,
        EnableDS = 0x502,
        EnableDSUIOnly = 0x503,

        // Messages used with a pointer to a DAT_EVENT structure     
        ProcessEvent = 0x601,

        // Messages used with a pointer to a DAT_PENDINGXFERS structure
        EndXfer = 0x701,
        StopFeeder = 0x702,

        // Messages used with a pointer to a DAT_FILESYSTEM structure
        ChangeDirectory = 0x801,
        CreateDirectory = 0x802,
        Delete = 0x803,
        FormatMedia = 0x804,
        GetClose = 0x805,
        GetFirstFile = 0x806,
        GetInfo = 0x807,
        GetNextFile = 0x808,
        Rename = 0x809,
        Copy = 0x80a,
        AutomaticCaptureDirectory = 0x80b,

        // Messages used with a pointer to a DAT_PASSTHRU structure
        PassThru = 0x901,

        // used with DAT_CALLBACK
        RegisterCallback = 0x902,

        // used with DAT_CAPABILITY
        ResetAll = 0xa01,
    }

    /// <summary>
    /// Indicates the type of capability.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum CapabilityId : ushort
    {
        None = 0,
        CustomBase = 0x8000, /* Base of custom capabilities */

        /* all data sources are REQUIRED to support these caps */
        CapXferCount = 0x0001,

        /* image data sources are REQUIRED to support these caps */
        ICapCompression = 0x0100,
        ICapPixelType = 0x0101,
        ICapUnits = 0x0102, /* default is TWUN_INCHES */
        ICapXferMech = 0x0103,

        /* all data sources MAY support these caps */
        CapAuthor = 0x1000,
        CapCaption = 0x1001,
        CapFeederEnabled = 0x1002,
        CapFeederLoaded = 0x1003,
        CapTimeDate = 0x1004,
        CapSupportedCaps = 0x1005,
        CapExtendedCaps = 0x1006,
        CapAutoFeed = 0x1007,
        CapClearPage = 0x1008,
        CapFeedPage = 0x1009,
        CapRewindPage = 0x100a,
        CapIndicators = 0x100b,   /* Added 1.1 */
        CapPaperDetectable = 0x100d,   /* Added 1.6 */
        CapUIControllable = 0x100e,   /* Added 1.6 */
        CapDeviceOnline = 0x100f,   /* Added 1.6 */
        CapAutoScan = 0x1010,   /* Added 1.6 */
        CapThumbnailsEnabled = 0x1011,   /* Added 1.7 */
        CapDuplex = 0x1012,   /* Added 1.7 */
        CapDuplexEnabled = 0x1013,   /* Added 1.7 */
        CapEnableDSUIOnly = 0x1014,   /* Added 1.7 */
        CapCustomDSData = 0x1015,   /* Added 1.7 */
        CapEndorser = 0x1016,   /* Added 1.7 */
        CapJobControl = 0x1017,   /* Added 1.7 */
        CapAlarms = 0x1018,   /* Added 1.8 */
        CapAlarmVolume = 0x1019,   /* Added 1.8 */
        CapAutomaticCapture = 0x101a,   /* Added 1.8 */
        CapTimeBeforeFirstCapture = 0x101b,   /* Added 1.8 */
        CapTimeBetweenCaptures = 0x101c,   /* Added 1.8 */
        CapClearBuffers = 0x101d,   /* Added 1.8 */
        CapMaxBatchBuffers = 0x101e,   /* Added 1.8 */
        CapDeviceTimeDate = 0x101f,   /* Added 1.8 */
        CapPowerSupply = 0x1020,   /* Added 1.8 */
        CapCameraPreviewUI = 0x1021,   /* Added 1.8 */
        CapDeviceEvent = 0x1022,   /* Added 1.8 */
        CapSerialNumber = 0x1024,   /* Added 1.8 */
        CapPrinter = 0x1026,   /* Added 1.8 */
        CapPrinterEnabled = 0x1027,   /* Added 1.8 */
        CapPrinterIndex = 0x1028,   /* Added 1.8 */
        CapPrinterMode = 0x1029,   /* Added 1.8 */
        CapPrinterString = 0x102a,   /* Added 1.8 */
        CapPrinterSuffix = 0x102b,   /* Added 1.8 */
        CapLanguage = 0x102c,   /* Added 1.8 */
        CapFeederAlignment = 0x102d,   /* Added 1.8 */
        CapFeederOrder = 0x102e,   /* Added 1.8 */
        CapReacquireAllowed = 0x1030,   /* Added 1.8 */
        CapBatteryMinutes = 0x1032,   /* Added 1.8 */
        CapBatteryPercentage = 0x1033,   /* Added 1.8 */
        CapCameraSide = 0x1034,   /* Added 1.91 */
        CapSegmented = 0x1035,   /* Added 1.91 */
        CapCameraEnabled = 0x1036,   /* Added 2.0 */
        CapCameraOrder = 0x1037,   /* Added 2.0 */
        CapMicrEnabled = 0x1038,   /* Added 2.0 */
        CapFeederPrep = 0x1039,   /* Added 2.0 */
        CapFeederPocket = 0x103a,   /* Added 2.0 */
        CapAutomaticSenseMedium = 0x103b,   /* Added 2.1 */
        CapCustomInterfaceGuid = 0x103c,   /* Added 2.1 */
        CapSupportedCapsSegmentUnique = 0x103d,
        CapSupportedDATs = 0x103e,
        CapDoubleFeedDetection = 0x103f,
        CapDoubleFeedDetectionLength = 0x1040,
        CapDoubleFeedDetectionSensitivity = 0x1041,
        CapDoubleFeedDetectionResponse = 0x1042,
        CapPaperHandling = 0x1043,
        CapIndicatorsMode = 0x1044,
        CapPrinterVerticalOffset = 0x1045,
        CapPowerSaveTime = 0x1046,
        CapPrinterCharRotation = 0x1047,
        CapPrinterFontStyle = 0x1048,
        CapPrinterIndexLeadChar = 0x1049,
        CapPrinterIndexMaxValue = 0x104A,
        CapPrinterIndexNumDigits = 0x104B,
        CapPrinterIndexStep = 0x104C,
        CapPrinterIndexTrigger = 0x104D,
        CapPrinterStringPreview = 0x104E,


        /* image data sources MAY support these caps */
        ICapAutoBright = 0x1100,
        ICapBrightness = 0x1101,
        ICapContrast = 0x1103,
        ICapCustHalftone = 0x1104,
        ICapExposureTime = 0x1105,
        ICapFilter = 0x1106,
        ICapFlashUsed = 0x1107,
        ICapGamma = 0x1108,
        ICapHalftones = 0x1109,
        ICapHighlight = 0x110a,
        ICapImageFileFormat = 0x110c,
        ICapLampState = 0x110d,
        ICapLightSource = 0x110e,
        ICapOrientation = 0x1110,
        ICapPhysicalWidth = 0x1111,
        ICapPhysicalHeight = 0x1112,
        ICapShadow = 0x1113,
        ICapFrames = 0x1114,
        ICapXNativeResolution = 0x1116,
        ICapYNativeResolution = 0x1117,
        ICapXResolution = 0x1118,
        ICapYResolution = 0x1119,
        ICapMaxFrames = 0x111a,
        ICapTiles = 0x111b,
        ICapBitOrder = 0x111c,
        ICapCCITTKFactor = 0x111d,
        ICapLightPath = 0x111e,
        ICapPixelFlavor = 0x111f,
        ICapPlanarChunky = 0x1120,
        ICapRotation = 0x1121,
        ICapSupportedSizes = 0x1122,
        ICapThreshold = 0x1123,
        ICapXScaling = 0x1124,
        ICapYScaling = 0x1125,
        ICapBitOrderCodes = 0x1126,
        ICapPixelFlavorCodes = 0x1127,
        ICapJpegPixelType = 0x1128,
        ICapTimeFill = 0x112a,
        ICapBitDepth = 0x112b,
        ICapBitDepthReduction = 0x112c,  /* Added 1.5 */
        ICapUndefinedImageSize = 0x112d,  /* Added 1.6 */
        ICapImageDataSet = 0x112e,  /* Added 1.7 */
        ICapExtImageInfo = 0x112f,  /* Added 1.7 */
        ICapMinimumHeight = 0x1130,  /* Added 1.7 */
        ICapMinimumWidth = 0x1131,  /* Added 1.7 */
        ICapAutoDiscardBlankPages = 0x1134,  /* Added 2.0 */
        ICapFlipRotation = 0x1136,  /* Added 1.8 */
        ICapBarcodeDetectionEnabled = 0x1137,  /* Added 1.8 */
        ICapSupportedBarcodeTypes = 0x1138,  /* Added 1.8 */
        ICapBarcodeMaxSearchPriorities = 0x1139,  /* Added 1.8 */
        ICapBarcodeSearchPriorities = 0x113a,  /* Added 1.8 */
        ICapBarcodeSearchMode = 0x113b,  /* Added 1.8 */
        ICapBarcodeMaxRetries = 0x113c,  /* Added 1.8 */
        ICapBarcodeTimeout = 0x113d,  /* Added 1.8 */
        ICapZoomFactor = 0x113e,  /* Added 1.8 */
        ICapPatchCodeDetectionEnabled = 0x113f,  /* Added 1.8 */
        ICapSupportedPatchCodeTypes = 0x1140,  /* Added 1.8 */
        ICapPatchCodeMaxSearchPriorities = 0x1141,  /* Added 1.8 */
        ICapPatchCodeSearchPriorities = 0x1142,  /* Added 1.8 */
        ICapPatchCodeSearchMode = 0x1143,  /* Added 1.8 */
        ICapPatchCodeMaxRetries = 0x1144,  /* Added 1.8 */
        ICapPatchCodeTimeout = 0x1145,  /* Added 1.8 */
        ICapFlashUsed2 = 0x1146,  /* Added 1.8 */
        ICapImageFilter = 0x1147,  /* Added 1.8 */
        ICapNoiseFilter = 0x1148,  /* Added 1.8 */
        ICapOverScan = 0x1149,  /* Added 1.8 */
        ICapAutomaticBorderDetection = 0x1150,  /* Added 1.8 */
        ICapAutomaticDeskew = 0x1151,  /* Added 1.8 */
        ICapAutomaticRotate = 0x1152,  /* Added 1.8 */
        ICapJpegQuality = 0x1153,  /* Added 1.9 */
        ICapFeederType = 0x1154,  /* Added 1.91 */
        ICapICCProfile = 0x1155,  /* Added 1.91 */
        ICapAutoSize = 0x1156,  /* Added 2.0 */
        ICapAutomaticCropUsesFrame = 0x1157,  /* Added 2.1 */
        ICapAutomaticLengthDetection = 0x1158,  /* Added 2.1 */
        ICapAutomaticColorEnabled = 0x1159,  /* Added 2.1 */
        ICapAutomaticColorNonColorPixelType = 0x115a,  /* Added 2.1 */
        ICapColorManagementEnabled = 0x115b,  /* Added 2.1 */
        ICapImageMerge = 0x115c,  /* Added 2.1 */
        ICapImageMergeHeightThreshold = 0x115d,  /* Added 2.1 */
        ICapSupportedExtImageInfo = 0x115e,  /* Added 2.1 */
        ICapFilmType = 0x115f,
        ICapMirror = 0x1160,
        ICapJpegSubsampling = 0x1161,

        /* image data sources MAY support these audio caps */
        ACapXferMech = 0x1202,  /* Added 1.8 */

    }

    /// <summary>
    /// Extended Image Info Attributes.
    /// Corresponds to TWEI_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ExtendedImageInfo : ushort
    {
        Invalid = 0,
        BarcodeX = 0x1200,
        BarcodeY = 0x1201,
        BarcodeText = 0x1202,
        BarcodeType = 0x1203,
        DeshadeTop = 0x1204,
        DeshadeLeft = 0x1205,
        DeshadeHeight = 0x1206,
        DeshadeWidth = 0x1207,
        DeshadeSize = 0x1208,
        SpecklesRemoved = 0x1209,
        HorzLineXCoord = 0x120A,
        HorzLineYCoord = 0x120B,
        HorzLineLength = 0x120C,
        HorzLineThickness = 0x120D,
        VertLineXCoord = 0x120E,
        VertLineYCoord = 0x120F,
        VertLineLength = 0x1210,
        VertLineThickness = 0x1211,
        PatchCode = 0x1212,
        EndorsedText = 0x1213,
        FormConfidence = 0x1214,
        FormTemplateMatch = 0x1215,
        FormTemplatePageMatch = 0x1216,
        FormHorzDocOffset = 0x1217,
        FormVertDocOffset = 0x1218,
        BarcodeCount = 0x1219,
        BarcodeConfidence = 0x121A,
        BarcodeRotation = 0x121B,
        BarcodeTextLength = 0x121C,
        DeshadeCount = 0x121D,
        DeshadeBlackCountOld = 0x121E,
        DeshadeBlackCountNew = 0x121F,
        DeshadeBlackRLMin = 0x1220,
        DeshadeBlackRLMax = 0x1221,
        DeshadeWhiteCountOld = 0x1222,
        DeshadeWhiteCountNew = 0x1223,
        DeshadeWhiteRLMin = 0x1224,
        DeshadeWhiteRLAve = 0x1225,
        DeshadeWhiteRLMax = 0x1226,
        BlackSpecklesRemoved = 0x1227,
        WhiteSpecklesRemoved = 0x1228,
        HorzLineCount = 0x1229,
        VertLineCount = 0x122A,
        DeskewStatus = 0x122B,
        SkewOriginalAngle = 0x122C,
        SkewFinalAngle = 0x122D,
        SkewConfidence = 0x122E,
        SkewWindowX1 = 0x122F,
        SkewWindowY1 = 0x1230,
        SkewWindowX2 = 0x1231,
        SkewWindowY2 = 0x1232,
        SkewWindowX3 = 0x1233,
        SkewWindowY3 = 0x1234,
        SkewWindowX4 = 0x1235,
        SkewWindowY4 = 0x1236,
        BookName = 0x1238, /* added 1.9 */
        ChapterNumber = 0x1239,/* added 1.9 */
        DocumentNumber = 0x123A,  /* added 1.9 */
        PageNumber = 0x123B,  /* added 1.9 */
        Camera = 0x123C,  /* added 1.9 */
        FrameNumber = 0x123D,  /* added 1.9 */
        Frame = 0x123E,  /* added 1.9 */
        PixelFlavor = 0x123F,  /* added 1.9 */
        IccProfile = 0x1240,  /* added 1.91 */
        LastSegment = 0x1241,  /* added 1.91 */
        SegmentNumber = 0x1242,  /* added 1.91 */
        MagData = 0x1243,  /* added 2.0 */
        MagType = 0x1244,  /* added 2.0 */
        PageSide = 0x1245,  /* added 2.0 */
        FileSystemSource = 0x1246,  /* added 2.0 */
        ImageMerged = 0x1247,  /* added 2.1 */
        MagDataLength = 0x1248,  /* added 2.1 */
        PaperCount = 0x1249,
        PrinterText = 0x124a
    }

    /// <summary>
    /// EndXfer job control values.
    /// Corresponds to TWEJ_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum TWEJ : ushort
    {
        None = 0x0000,
        MidSeparator = 0x0001,
        Patch1 = 0x0002,
        Patch2 = 0x0003,
        Patch3 = 0x0004,
        Patch4 = 0x0005,
        Patch6 = 0x0006,
        PatchT = 0x0007
    }

    /// <summary>
    /// Corresponds to TWRC_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
    public enum ReturnCode : ushort
    {
        CustomBase = 0x8000,

        Success = 0,
        Failure = 1,
        CheckStatus = 2,
        Cancel = 3,
        DSEvent = 4,
        NotDSEvent = 5,
        XferDone = 6,
        EndOfList = 7,
        InfoNotSupported = 8,
        DataNotAvailable = 9,
        Busy = 10,
        ScannerLocked = 11,
    }

    /// <summary>
    /// Corresponds to TWCC_*.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum ConditionCode : ushort
    {
        CustomBase = 0x8000,

        Success = 0,
        Bummer = 1,
        LowMemory = 2,
        NoDS = 3,
        MaxConnections = 4,
        OperationError = 5,
        BadCap = 6,
        BadProtocol = 9,
        BadValue = 10,
        SeqError = 11,
        BadDest = 12,
        CapUnsupported = 13,
        CapBadOperation = 14,
        CapSeqError = 15,
        Denied = 16,
        FileExists = 17,
        FileNotFound = 18,
        NotEmpty = 19,
        PaperJam = 20,
        PaperDoubleFeed = 21,
        FileWriteError = 22,
        CheckDeviceOnline = 23,

        Interlock = 24,
        DamagedCorner = 25,
        FocusError = 26,
        DocTooLight = 27,
        DocTooDark = 28,
        NoMedia = 29,

    }

    /// <summary>
    /// Bit mask for querying the operation that are supported by the data source on a capability.
    /// Corresponds to TWQC_*.
    /// </summary>
    [Flags]
    public enum QuerySupport
    {
        None = 0,
        Get = 0x1,
        Set = 0x2,
        GetDefault = 0x4,
        GetCurrent = 0x8,
        Reset = 0x10,
        SetConstraint = 0x20,
        Constrainable = 0x40,
        GetHelp = 0x100,
        GetLabel = 0x200,
        GetLabelEnum = 0x400,

        /// <summary>
        /// Cap applies to entire session/machine.
        /// </summary>
        Machine = 0x1000,
        /// <summary>
        /// Cap applies to bitonal cameras.
        /// </summary>
        Bitonal = 0x2000,
        /// <summary>
        /// Cap applies to color cameras.
        /// </summary>
        Color = 0x4000
    }

    /// <summary>
    /// TWAIN's boolean values.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32")]
    public enum BoolType : ushort
    {
        /// <summary>
        /// The false value (0).
        /// </summary>
        False = 0,
        /// <summary>
        /// The true value (1).
        /// </summary>
        True = 1
    }


    /// <summary>
    /// Contains direct magic values for some TWAIN stuff.
    /// </summary>
    public static class TwainConst
    {
        // these are specified here since the actual
        // array length doesn't reflect the name :(

        /// <summary>
        /// Length of an array that holds TW_STR32.
        /// </summary>
        public const int String32 = 34;
        /// <summary>
        /// Length of an array that holds TW_STR64.
        /// </summary>
        public const int String64 = 66;
        /// <summary>
        /// Length of an array that holds TW_STR128.
        /// </summary>
        public const int String128 = 130;
        /// <summary>
        /// Length of an array that holds TW_STR255.
        /// </summary>
        public const int String255 = 256;

        // deprecated 
        //public const int String1024 = 1026;

        /// <summary>
        /// Don't care value for 8 bit types.
        /// </summary>
        public const byte DontCare8 = 0xff;
        /// <summary>
        /// Don't care value for 16 bit types.
        /// </summary>
        public const ushort DontCare16 = 0xffff;
        /// <summary>
        /// Don't care value for 32 bit types.
        /// </summary>
        public const uint DontCare32 = 0xffffffff;

        /// <summary>
        /// The major version number of TWAIN supported by this library.
        /// </summary>
        public const short ProtocolMajor = 2;
        /// <summary>
        /// The minor version number of TWAIN supported by this library.
        /// </summary>
        public const short ProtocolMinor = 3;

        /// <summary>
        /// Value for false where applicable.
        /// </summary>
        public const ushort True = 1;
        /// <summary>
        /// Value for true where applicable.
        /// </summary>
        public const ushort False = 0;
    }
}
