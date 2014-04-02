// This file contains all the structs defined in the twain.h file.

// The TWAIN numeric types are mapped with "using"
// to aid in mapping against the twin.h file (copy/paste!).
// It also makes it easy to change all the types that 
// uses it if I made a mistake in the mapped value type.
// Consumers will not see those names.
using System;
using System.Runtime.InteropServices;

using TW_BOOL = System.UInt16;
// use HandleRef instead?
using TW_HANDLE = System.IntPtr;
using TW_INT16 = System.Int16;
using TW_INT32 = System.Int32;
using TW_MEMREF = System.IntPtr;
using TW_UINT16 = System.UInt16;
using TW_UINT32 = System.UInt32;
using TW_UINT8 = System.Byte;
// iffy
using TW_UINTPTR = System.UIntPtr;
using NTwain.Values;

// This mono doc is awesome. An interop must-read
// http://www.mono-project.com/Interop_with_Native_Libraries

//////////////////////////////////
// most of the doc text are copied 
// from the twain pdf. Data that
// are passed to the TWAIN method
// are defined as classes to reduce
// ref/out in the low-level calls. 
// Others continue to be structs.
//////////////////////////////////


namespace NTwain.Data
{
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial struct TWFix32
	{
		/// <summary>
		/// The Whole part of the floating point number. This number is signed.
		/// </summary>
		TW_INT16 _whole;
		/// <summary>
		/// The Fractional part of the floating point number. This number is unsigned.
		/// </summary>
		TW_UINT16 _frac;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial struct TWFrame
	{
		TWFix32 _left;
		TWFix32 _top;
		TWFix32 _right;
		TWFix32 _bottom;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial struct TWDecodeFunction
	{
		TWFix32 _startIn;
		TWFix32 _breakIn;
		TWFix32 _endIn;
		TWFix32 _startOut;
		TWFix32 _breakOut;
		TWFix32 _endOut;
		TWFix32 _gamma;
		TWFix32 _sampleCount;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial struct TWTransformStage
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		TWDecodeFunction[] _decode;
		// TODO: research jagged aray mapping. maybe use ptr?
		[MarshalAs(UnmanagedType.ByValArray)]
		TWFix32[][] _mix;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWArray
	{
		TW_UINT16 _itemType;
		TW_UINT32 _numItems;
		object[] _itemList;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2),
	BestFitMapping(false, ThrowOnUnmappableChar = true)]
	public partial class TWAudioInfo
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String255)]
		string _name;
		TW_UINT32 _reserved;
	}

	delegate ReturnCode CallbackDelegate(TWIdentity origin, TWIdentity destination,
		DataGroups dg, DataArgumentType dat, Message msg, TW_MEMREF data);

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	partial class TWCallback
	{
		[MarshalAs(UnmanagedType.FunctionPtr)]
		CallbackDelegate _callBackProc;
		TW_UINT32 _refCon;
		TW_INT16 _message;

	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	partial class TWCallback2
	{
		[MarshalAs(UnmanagedType.FunctionPtr)]
		CallbackDelegate _callBackProc;
		TW_UINTPTR _refCon;
		TW_INT16 _message;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWCapability
	{
		TW_UINT16 _cap;
		TW_UINT16 _conType;
		IntPtr _hContainer;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial struct TWCiePoint
	{
		TWFix32 _x;
		TWFix32 _y;
		TWFix32 _z;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWCieColor
	{
		TW_UINT16 _colorSpace;
		TW_INT16 _lowEndian;
		TW_INT16 _deviceDependent;
		TW_INT32 _versionNumber;
		TWTransformStage _stageABC;
		TWTransformStage _stageLMN;
		TWCiePoint _whitePoint;
		TWCiePoint _blackPoint;
		TWCiePoint _whitePaper;
		TWCiePoint _whiteInk;
		TWFix32[] _samples;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWCustomDSData
	{
		TW_UINT32 _infoLength;
		TW_HANDLE _hData;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2),
	BestFitMapping(false, ThrowOnUnmappableChar = true)]
	public partial class TWDeviceEvent
	{
		TW_UINT32 _event;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String255)]
		string _deviceName;
		TW_UINT32 _batteryMinutes;
		TW_INT16 _batteryPercentage;
		TW_INT32 _powerSupply;
		TWFix32 _xResolution;
		TWFix32 _yResolution;
		TW_UINT32 _flashUsed2;
		TW_UINT32 _automaticCapture;
		TW_UINT32 _timeBeforeFirstCapture;
		TW_UINT32 _timeBetweenCaptures;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	partial class TWEntryPoint
	{
		TW_UINT32 _size;
		// this is not a delegate cuz it's not used by the app
		IntPtr _dSM_Entry;

		[MarshalAs(UnmanagedType.FunctionPtr)]
		MemAllocateDelegate _dSM_MemAllocate;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		MemFreeDelegate _dSM_MemFree;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		MemLockDelegate _dSM_MemLock;
		[MarshalAs(UnmanagedType.FunctionPtr)]
		MemUnlockDelegate _dSM_MemUnlock;

		public delegate IntPtr MemAllocateDelegate(uint size);
		public delegate void MemFreeDelegate(IntPtr handle);
		public delegate IntPtr MemLockDelegate(IntPtr handle);
		public delegate void MemUnlockDelegate(IntPtr handle);
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial struct TWElement8
	{
		TW_UINT8 _index;
		TW_UINT8 _channel1;
		TW_UINT8 _channel2;
		TW_UINT8 _channel3;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWEnumeration
	{
		TW_UINT16 _itemType;
		TW_UINT32 _numItems;
		TW_UINT32 _currentIndex;
		TW_UINT32 _defaultIndex;
		object[] _itemList;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWEvent
	{
		TW_MEMREF _pEvent;
		TW_UINT16 _tWMessage;
	}

	[StructLayout(LayoutKind.Explicit, Pack = 2)]
	public partial struct TWInfo
	{
		[FieldOffset(0)]
		TW_UINT16 _infoID;
		[FieldOffset(2)]
		TW_UINT16 _itemType;
		[FieldOffset(4)]
		TW_UINT16 _numItems;

		[FieldOffset(6)]
		TW_UINT16 _condCode;
		[FieldOffset(6)]
		TW_UINT16 _returnCode;  /* TWAIN 2.1 and newer */

		[FieldOffset(8)]
		TW_UINTPTR Item;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWExtImageInfo
	{
		TW_UINT32 _numInfos;
		TWInfo[] _info;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2),
	BestFitMapping(false, ThrowOnUnmappableChar = true)]
	public partial class TWFileSystem
	{
		/* DG_CONTROL / DAT_FILESYSTEM / MSG_xxxx fields     */
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String255)]
		string _inputName;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String255)]
		string _outputName;
		TW_MEMREF _context;
		/* DG_CONTROL / DAT_FILESYSTEM / MSG_DELETE field    */
		//TODO: verify this field
		short _recursive; /* recursively delete all sub-directories */
		/* DG_CONTROL / DAT_FILESYSTEM / MSG_GETInfo fields  */
		TW_INT32 _fileType; /* One of the TWFY_xxxx values */
		TW_UINT32 _size; /* Size of current FileType */
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String32)]
		string _createTimeDate;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String32)]
		string _modifiedTimeDate;
		TW_UINT32 _freeSpace;
		TW_INT32 _newImageSize;
		TW_UINT32 _numberOfFiles;
		TW_UINT32 _numberOfSnippets;
		TW_UINT32 _deviceGroupMask;
		//TODO: verify this field, check if can just not use it
		//char       _reserved[508]; /**/
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 508)]
		byte[] _reserved;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWGrayResponse
	{
		TWElement8[] _response;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2),
	BestFitMapping(false, ThrowOnUnmappableChar = true)]
	public partial struct TWVersion
	{
		TW_UINT16 _majorNum;
		TW_UINT16 _minorNum;
		TW_UINT16 _language;
		TW_UINT16 _country;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String32)]
		string _info;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2),
	BestFitMapping(false, ThrowOnUnmappableChar = true)]
	public partial class TWIdentity
	{
		TW_UINT32 _id;
		TWVersion _version;
		TW_UINT16 _protocolMajor;
		TW_UINT16 _protocolMinor;
		TW_UINT32 _supportedGroups;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String32)]
		string _manufacturer;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String32)]
		string _productFamily;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String32)]
		string _productName;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWImageInfo
	{
		TWFix32 _xResolution;
		TWFix32 _yResolution;
		TW_INT32 _imageWidth;
		TW_INT32 _imageLength;
		TW_INT16 _samplesPerPixel;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		TW_INT16[] _bitsPerSample;
		TW_INT16 _bitsPerPixel;
		TW_BOOL _planar;
		TW_INT16 _pixelType;
		TW_UINT16 _compression;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWImageLayout
	{
		TWFrame _frame;
		TW_UINT32 _documentNumber;
		TW_UINT32 _pageNumber;
		TW_UINT32 _frameNumber;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial struct TWMemory
	{
		// not a class due to embedded

		TW_UINT32 _flags;
		TW_UINT32 _length;
		TW_MEMREF _theMem;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWImageMemXfer
	{
		TW_UINT16 _compression;
		TW_UINT32 _bytesPerRow;
		TW_UINT32 _columns;
		TW_UINT32 _rows;
		TW_UINT32 _xOffset;
		TW_UINT32 _yOffset;
		TW_UINT32 _bytesWritten;
		TWMemory _memory;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWJpegCompression
	{
		TW_UINT16 _colorSpace;
		TW_UINT32 _subSampling;
		TW_UINT16 _numComponents;
		TW_UINT16 _restartFrequency;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		TW_UINT16[] _quantMap;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		TWMemory[] _quantTable;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		TW_UINT16[] _huffmanMap;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		TWMemory[] _huffmanDC;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		TWMemory[] _huffmanAC;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWOneValue
	{
		TW_UINT16 _itemType;
		TW_UINT32 _item;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWPalette8
	{
		TW_UINT16 _numColors;
		TW_UINT16 _paletteType;
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
		TWElement8[] _colors;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWPassThru
	{
		TW_MEMREF _pCommand;
		TW_UINT32 _commandBytes;
		TW_INT32 _direction;
		TW_MEMREF _pData;
		TW_UINT32 _dataBytes;
		TW_UINT32 _dataBytesXfered;
	}

    //[StructLayout(LayoutKind.Explicit, Pack = 2)]
    [StructLayout(LayoutKind.Sequential, Pack = 2)]
	partial class TWPendingXfers
	{
        //[FieldOffset(0)]
		//TW_INT16 _count;
		TW_UINT16 _count;

        //[FieldOffset(2)]
		TW_UINT32 _eOJ;
        //[FieldOffset(2)]
        //TW_UINT32 _reserved;

	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWRange
	{
		TW_UINT16 _itemType;
		TW_UINT32 _minValue;
		TW_UINT32 _maxValue;
		TW_UINT32 _stepSize;
		TW_UINT32 _defaultValue;
		TW_UINT32 _currentValue;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWRgbResponse
	{
		TWElement8[] _response;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2),
	BestFitMapping(false, ThrowOnUnmappableChar = true)]
	public partial class TWSetupFileXfer
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = TwainConst.String255)]
		string _fileName;
		TW_UINT16 _format;
		TW_INT16 _vRefNum = -1;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public partial class TWSetupMemXfer
	{
		TW_UINT32 _minBufSize;
		TW_UINT32 _maxBufSize;
		TW_UINT32 _preferred;
	}

	[StructLayout(LayoutKind.Explicit, Pack = 2)]
	public partial class TWStatus
	{
		[FieldOffset(0)]
		TW_UINT16 _conditionCode;
		[FieldOffset(2)]
		TW_UINT16 _data;
		[FieldOffset(2)]
		TW_UINT16 _reserved;

	}

	[StructLayout(LayoutKind.Explicit, Pack = 2)]
	public partial class TWStatusUtf8
	{
		// rather than embedding the twstatus directly use its fields instead
		// so the twstatus could become an object. If twstatus changes
		// definition remember to change it here

		///// <summary>
		///// <see cref="TWStatus"/> data received from a previous call.
		///// </summary>
		//TWStatus Status;
		[FieldOffset(0)]
		TW_UINT16 _statusConditionCode;
		[FieldOffset(2)]
		TW_UINT16 _statusData;
		[FieldOffset(2)]
		TW_UINT16 _status_reserved;

		[FieldOffset(4)]
		TW_UINT32 _size;
		[FieldOffset(8)]
		TW_HANDLE _uTF8string;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	partial class TWUserInterface
	{
		TW_BOOL _showUI;
		TW_BOOL _modalUI;
        TW_HANDLE _hParent;
        //HandleRef _hParent;
	}
}
