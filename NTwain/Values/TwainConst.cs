namespace NTwain.Values
{
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
