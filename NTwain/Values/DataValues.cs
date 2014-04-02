// The MIT License (MIT)
// Copyright (c) 2013 Yin-Chun Wang
//
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF 
// OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using System;
using NTwain.Data;

namespace NTwain.Values
{
	public enum Direction
	{
		Invalid = 0,
		Get = 1,
		Set = 2
	}

	public enum FileType
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

	public enum PaletteType : ushort
	{
		Rgb = 0,
		Gray = 1,
		Cmy = 2
	}

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
		/* Added For 1.8 */
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

	[Flags]
	public enum DataGroups : uint
	{
		None = 0,
		Control = 0x1,
		Image = 0x2,
		Audio = 0x4
	}

	[Flags]
	public enum DataFunctionalities : uint
	{
		None = 0,
		Dsm2 = 0x10000000,
		App2 = 0x20000000,
		DS2 = 0x40000000,
	}

	public enum DataArgumentType : ushort
	{
		None = 0,
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

		// audio group
		AudioFileXfer = 0x201,
		AudioInfo = 0x202,
		AudioNativeXfer = 0x203,

		// crap
		IccProfile = 0x401,
		ImageMemFileXfer = 0x402,
		EntryPoint = 0x403,
	}

	public enum Message : ushort
	{
		Null = 0,
		CustomBase = 0x8000,
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

		XferReady = 0x101,
		CloseDSReq = 0x102,
		CloseDSOK = 0x103,
		DeviceEvent = 0x104,

		OpenDsm = 0x301,
		CloseDsm = 0x302,

		OpenDS = 0x401,
		CloseDS = 0x402,
		UserSelect = 0x403,

		DisableDS = 0x501,
		EnableDS = 0x502,
		EnableDSUIOnly = 0x503,

		ProcessEvent = 0x601,

		EndXfer = 0x701,
		StopFeeder = 0x702,

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

		PassThru = 0x901,
		RegisterCallback = 0x902,
		ResetAll = 0xa01,
	}

	/// <summary>
	/// Indicates the type of capability.
	/// </summary>
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
		CapSupportedCapsExt = 0x100c,   /* Added 1.6 */
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
		CapSUPPORTEDCAPSSEGMENTUNIQUE = 0x103d,
		CapSUPPORTEDDATS = 0x103e,
		CapDoubleFeedDetection = 0x103f,
		CapDoubleFeedDetectionLength = 0x1040,
		CapDoubleFeedDetectionSensitivity = 0x1041,
		CapDoubleFeedDetectionResponse = 0x1042,
		CapPaperHandling = 0x1043,
		CapIndicatorsMode = 0x1044,
		CapPrinterVerticalOffset = 0x1045,
		CapPowerSaveTime = 0x1046,

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
		ICapImageDataset = 0x112e,  /* Added 1.7 */
		ICapExtImageInfo = 0x112f,  /* Added 1.7 */
		ICapMinimumHeight = 0x1130,  /* Added 1.7 */
		ICapMinimumWidth = 0x1131,  /* Added 1.7 */
		ICapAutoDiscardBlankPages = 0x1134,  /* Added 2.0 */
		ICapFlipRotation = 0x1136,  /* Added 1.8 */
		ICapBarcodeDetectionEnabled = 0x1137,  /* Added 1.8 */
		ICapSupportedBARCODETYPES = 0x1138,  /* Added 1.8 */
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
		ICapOverscan = 0x1149,  /* Added 1.8 */
		ICapAutomaticBorderDetection = 0x1150,  /* Added 1.8 */
		ICapAutomaticDeskew = 0x1151,  /* Added 1.8 */
		ICapAutomaticRotate = 0x1152,  /* Added 1.8 */
		ICapJpegQuality = 0x1153,  /* Added 1.9 */
		ICapFeederTYPE = 0x1154,  /* Added 1.91 */
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
		ICapJpegSubSampling = 0x1161,

		/* image data sources MAY support these audio caps */
		ACapXferMech = 0x1202,  /* Added 1.8 */

	}

	public enum ReturnCode : ushort
	{
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
		CustomBase = 0x8000
	}

	public enum ConditionCode : ushort
	{
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

		CustomBase = 0x8000
	}

	/// <summary>
	/// Indicates the type of container used in capability.
	/// </summary>
	public enum ContainerType : ushort
	{
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
		DontCare = TwainConst.DontCare16,
	}

	/// <summary>
	/// The data types of item in TWAIN, used in the
	/// capability containers.
	/// </summary>
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
		String1024 = 0xd,
		Unicode512 = 0xe,
		/// <summary>
		/// Means Item is a handle (pointer).
		/// </summary>
		Handle = 0xf
	}

	/// <summary>
	/// Flags used in <see cref="TWMemory"/>.
	/// </summary>
	[Flags]
	public enum MemoryFlags : uint
	{
		Invalid = 0,
		AppOwns = 0x1,
		DsmOwns = 0x2,
		DSOwns = 0x4,
		Pointer = 0x8,
		Handle = 0x10
	}
}
