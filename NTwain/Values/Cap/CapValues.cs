namespace NTwain.Values.Cap
{
	/// <summary>
	/// CapAlarms values.
	/// </summary>
	public enum AlarmType : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		Alarm = 0,
		/// <summary>
		/// 
		/// </summary>
		FeederError = 1,
		/// <summary>
		/// 
		/// </summary>
		FeederWarning = 2,
		/// <summary>
		/// 
		/// </summary>
		Barcode = 3,
		/// <summary>
		/// 
		/// </summary>
		DoubleFeed = 4,
		/// <summary>
		/// 
		/// </summary>
		Jam = 5,
		/// <summary>
		/// 
		/// </summary>
		PatchCode = 6,
		/// <summary>
		/// 
		/// </summary>
		Power = 7,
		/// <summary>
		/// 
		/// </summary>
		Skew = 8
	}

	/// <summary>
	/// CapCameraSide values.
	/// </summary>
	public enum CameraSide : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		Both = 0,
		/// <summary>
		/// 
		/// </summary>
		Top = 1,
		/// <summary>
		/// 
		/// </summary>
		Bottom = 2
	}

	/// <summary>
	/// CapClearBuffers values.
	/// </summary>
	public enum ClearBuffers : ushort
	{
		Auto = 0,
		Clear = 1,
		NoClear = 2
	}

	/// <summary>
	/// Indicates the type of event from the source.
	/// Also CapDeviceEvent values. If used as
	/// a cap value it's ushort.
	/// </summary>
	public enum DeviceEvent : uint
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
	/// CapDuplex values.
	/// </summary>
	public enum Duplex : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		None = 0,
		/// <summary>
		/// 
		/// </summary>
		OnePass = 1,
		/// <summary>
		/// 
		/// </summary>
		TwoPass = 2
	}

	/// <summary>
	/// CapFeederAlignment values.
	/// </summary>
	public enum FeederAlignment : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		None = 0,
		/// <summary>
		/// 
		/// </summary>
		Left = 1,
		/// <summary>
		/// 
		/// </summary>
		Center = 2,
		/// <summary>
		/// 
		/// </summary>
		Right = 3
	}


	/// <summary>
	/// CapFeederOrder values.
	/// </summary>
	public enum FeederOrder : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		FirstPageFirst = 0,
		/// <summary>
		/// 
		/// </summary>
		LastPageFirst = 1
	}

	/// <summary>
	/// CapFeederPocket values.
	/// </summary>
	public enum FeederPocket : ushort
	{
		/// <summary>
		/// 
		/// </summary>
		PocketError = 0,
		/// <summary>
		/// 
		/// </summary>
		Pocket1 = 1,
		/// <summary>
		/// 
		/// </summary>
		Pocket2 = 2,
		/// <summary>
		/// 
		/// </summary>
		Pocket3 = 3,
		/// <summary>
		/// 
		/// </summary>
		Pocket4 = 4,
		/// <summary>
		/// 
		/// </summary>
		Pocket5 = 5,
		/// <summary>
		/// 
		/// </summary>
		Pocket6 = 6,
		/// <summary>
		/// 
		/// </summary>
		Pocket7 = 7,
		/// <summary>
		/// 
		/// </summary>
		Pocket8 = 8,
		/// <summary>
		/// 
		/// </summary>
		Pocket9 = 9,
		/// <summary>
		/// 
		/// </summary>
		Pocket10 = 10,
		/// <summary>
		/// 
		/// </summary>
		Pocket11 = 11,
		/// <summary>
		/// 
		/// </summary>
		Pocket12 = 12,
		/// <summary>
		/// 
		/// </summary>
		Pocket13 = 13,
		/// <summary>
		/// 
		/// </summary>
		Pocket14 = 14,
		/// <summary>
		/// 
		/// </summary>
		Pocket15 = 15,
		/// <summary>
		/// 
		/// </summary>
		Pocket16 = 16,
	}

	/// <summary>
	/// CapJobControl values.
	/// </summary>
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
	/// CapLanguage values.
	/// </summary>
	public enum Language : ushort
	{
		UserLocale = 0xff,
		Danish = 0,    /* Danish                 */
		Dutch = 1,    /* Dutch                  */
		English = 2,    /* International English  */
		FrenchCanadian = 3,    /* French Canadian        */
		Finnish = 4,    /* Finnish                */
		French = 5,    /* French                 */
		German = 6,    /* German                 */
		Icelandic = 7,    /* Icelandic              */
		Italian = 8,    /* Italian                */
		Norwegian = 9,    /* Norwegian              */
		Portuguese = 10,   /* Portuguese             */
		Spanish = 11,  /* Spanish                */
		Swedish = 12,  /* Swedish                */
		EnglishUsa = 13,  /* U.S. English           */
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
		ArabicUae = 31,/* United Arabic Emirates */
		ArabicYemen = 32,
		Basque = 33,
		Byelorussian = 34,
		Bulgarian = 35,
		Catalan = 36,
		Chinese = 37,
		ChineseHongKong = 38,
		ChinesePrc = 39,/* People's Republic Of China */
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
		/* More Stuff Added For 1.8 */
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
	/// CapPowerSupply values.
	/// </summary>
	public enum PowerSupply : ushort
	{
		External = 0,
		Battery = 1
	}

	/// <summary>
	/// CapPrinter values.
	/// </summary>
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
	/// CapPrinterMode values.
	/// </summary>
	public enum PrinterMode : ushort
	{
		SingleString = 0,
		MultiString = 1,
		CompoundString = 2
	}

	/// <summary>
	/// CapSegmented values.
	/// </summary>
	public enum Segmented : ushort
	{
		None = 0,
		Auto = 1
	}
}
