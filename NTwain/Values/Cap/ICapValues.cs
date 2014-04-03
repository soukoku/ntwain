namespace NTwain.Values.Cap
{

    /// <summary>
    /// ICapAutoDiscardBlankPages values.
    /// </summary>
    public enum AutoDiscardBlankPage
    {
        Invalid = 0,
        Disable = -2,
        Auto = -1
    }

    /// <summary>
    /// ICapAutoSize values.
    /// </summary>
    public enum AutoSize : ushort
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Auto = 1,
        /// <summary>
        /// 
        /// </summary>
        Current = 2
    }

    /// <summary>
    /// ICapBarcodeSearchMode,
    /// ICapPatchCodeSearchMode values.
    /// </summary>
    public enum BarcodeSearchMode : ushort
    {
        Horz = 0,
        Vert = 1,
        HorzVert = 2,
        VertHorz = 3
    }

    /// <summary>
    /// ICapBarcodeSearchPriorities,
    /// ICapSupportedBarcodeTypes values.
    /// </summary>
    public enum BarcodeTypes : ushort
    {
        Invalid = 0,
        ThreeOfNine,
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
    /// Values for ICapBitDepthReduction.
    /// </summary>
    public enum BitDepthReduction : ushort
    {
        /// <summary>
        /// 
        /// </summary>
        Threshold = 0,
        /// <summary>
        /// 
        /// </summary>
        Halftone = 1,
        /// <summary>
        /// 
        /// </summary>
        CustHalftone = 2,
        /// <summary>
        /// 
        /// </summary>
        Diffusion = 3
    }

    /// <summary>
    /// ICapBitOrder/ICapBitOrderCodes values.
    /// </summary>
    public enum BitOrder : ushort
    {
        /// <summary>
        /// 
        /// </summary>
        LsbFirst = 0,
        /// <summary>
        /// 
        /// </summary>
        MsbFirst = 1
    }

    /// <summary>
    /// ICapCompression values. Allows the application and Source to identify which compression schemes they have in common
    /// for Buffered Memory and File transfers. Since only certain file formats support compression, this capability must be negotiated after
    /// setting the desired file format with ICAP_IMAGEFILEFORMAT.
    /// </summary>
    public enum Compression : ushort
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
    /// ICapFeederType values.
    /// </summary>
    public enum FeederType : ushort
    {
        General = 0,
        Photo = 1
    }

    /// <summary>
    /// ICapFilter values.
    /// </summary>
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
    /// ICapFlashUsed2 values.
    /// </summary>
    public enum FlashedUsed : ushort
    {
        None = 0,
        Off = 1,
        On = 2,
        Auto = 3,
        RedEye = 4
    }

    /// <summary>
    /// ICapFlipRotation values.
    /// </summary>
    public enum FlipRotation : ushort
    {
        Book = 0,
        FanFold = 1
    }

    /// <summary>
    /// ICapIccProfile values.
    /// </summary>
    public enum IccProfile : ushort
    {
        None = 0,
        Link = 1,
        Embed = 2
    }

    /// <summary>
    /// ICapImageFileFormat values.
    /// </summary>
    public enum ImageFileFormat : ushort
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

    ///// <summary>
    ///// Audio file format values.
    ///// </summary>
    //public enum AudioFileFormat : ushort
    //{
    //    Wav = 0,
    //    Aiff = 1,
    //    AU = 3,
    //    Snd = 4,
    //}

    /// <summary>
    /// ICapImageFilter values.
    /// </summary>
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
    /// </summary>
    public enum ImageMerge : ushort
    {
        None = 0,
        FrontOnTop = 1,
        FrontOnBottom = 2,
        FrontOnLeft = 3,
        FrontOnRight = 4
    }

    /// <summary>
    /// Also ICapAutomaticColorNonColorPixelType (bw/gray),
    /// ICapJpegPixelType, ICapPixelType values.
    /// </summary>
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
        //ScRGB = 11,
        Infrared = 16
    }

    /// <summary>
    /// ICapJpegQuality values.
    /// </summary>
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
    /// </summary>
    public enum LightPath : ushort
    {
        Reflective = 0,
        Transmissive = 1
    }

    /// <summary>
    /// ICapLightSource values.
    /// </summary>
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
    /// ICapNoiseFilter values.
    /// </summary>
    public enum NoiseFilter : ushort
    {
        None = 0,
        Auto = 1,
        LonePixel = 2,
        MajorityRule = 3
    }

    /// <summary>
    /// ICapOrientation values.
    /// </summary>
    public enum Orientation : ushort
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
    /// </summary>
    public enum OverScan : ushort
    {
        None = 0,
        Auto = 1,
        TopBottom = 2,
        LeftRight = 3,
        All = 4
    }

    /// <summary>
    /// ICapPatchCodeSearchPriorities,
    /// ICapSupportedPatchCodeTypes values.
    /// </summary>
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
    /// </summary>
    public enum PixelFlavor : ushort
    {
        Chocolate = 0,
        Vanilla = 1
    }

    /// <summary>
    /// ICapPlanarChunky values.
    /// </summary>
    public enum PlanarChunky : ushort
    {
        Chunky = 0,
        Planar = 1
    }

    /// <summary>
    /// ICapSupportedSizes values.
    /// </summary>
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
    /// ICapUnits values.
    /// </summary>
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

    /// <summary>
    /// ICapXferMech, ACapXferMech values.
    /// </summary>
    public enum XferMech : ushort
    {
        Native = 0,
        File = 1,
        Memory = 2,
        /// <summary>
        /// Audio only.
        /// </summary>
        File2 = 3,
        MemFile = 4
    }
}
