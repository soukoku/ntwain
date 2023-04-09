// this is used to verify size of things on different platforms and archtectures

#include "pch.h"
#include <iostream>
#include "twain.h"

int main()
{
	bool is32 = sizeof(void*) == 4;
	bool is64 = sizeof(void*) == 8;
	std::cout << (is32 ? "32bit" : "") << (is64 ? "64bit" : "") << std::endl;
	std::cout << std::endl;
	std::cout << "TW_STR32  = " << sizeof(TW_STR32) << std::endl;
	std::cout << "TW_STR64  = " << sizeof(TW_STR64) << std::endl;
	std::cout << "TW_STR128 = " << sizeof(TW_STR128) << std::endl;
	std::cout << "TW_STR255 = " << sizeof(TW_STR255) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_INT8   = " << sizeof(TW_INT8) << std::endl;
	std::cout << "TW_INT16  = " << sizeof(TW_INT16) << std::endl;
	std::cout << "TW_INT32  = " << sizeof(TW_INT32) << std::endl;
	std::cout << "TW_UINT8  = " << sizeof(TW_UINT8) << std::endl;
	std::cout << "TW_UINT16 = " << sizeof(TW_UINT16) << std::endl;
	std::cout << "TW_UINT32 = " << sizeof(TW_UINT32) << std::endl;
	std::cout << "TW_BOOL   = " << sizeof(TW_BOOL) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_IDENTITY = " << sizeof(TW_IDENTITY) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_HANDLE = " << sizeof(TW_HANDLE) << std::endl;
	std::cout << "TW_MEMREF = " << sizeof(TW_MEMREF) << std::endl;
	std::cout << "TW_UINTPTR= " << sizeof(TW_UINTPTR) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_ONEVALUE    = " << sizeof(TW_ONEVALUE) << std::endl;
	std::cout << "TW_ARRAY       = " << sizeof(TW_ARRAY) << std::endl;
	std::cout << "TW_ENUMERATION = " << sizeof(TW_ENUMERATION) << std::endl;
	std::cout << "TW_RANGE       = " << sizeof(TW_RANGE) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_GRAYRESPONSE = " << sizeof(TW_GRAYRESPONSE) << std::endl;
	std::cout << "TW_RGBRESPONSE  = " << sizeof(TW_RGBRESPONSE) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_CALLBACK = " << sizeof(TW_CALLBACK) << std::endl;
	std::cout << "TW_CALLBACK2 = " << sizeof(TW_CALLBACK2) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_USERINTERFACE = " << sizeof(TW_USERINTERFACE) << std::endl;
	std::cout << "TW_PENDINGXFERS = " << sizeof(TW_PENDINGXFERS) << std::endl;
	std::cout << "TW_IMAGEMEMXFER = " << sizeof(TW_IMAGEMEMXFER) << std::endl;
	std::cout << "TW_MEMORY = " << sizeof(TW_MEMORY) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_CAPABILITY = " << sizeof(TW_CAPABILITY) << std::endl;
	std::cout << "TW_CUSTOMDSDATA = " << sizeof(TW_CUSTOMDSDATA) << std::endl;
	std::cout << "TW_EVENT = " << sizeof(TW_EVENT) << std::endl;
	std::cout << "TW_FILESYSTEM = " << sizeof(TW_FILESYSTEM) << std::endl;
	std::cout << "TW_JPEGCOMPRESSION = " << sizeof(TW_JPEGCOMPRESSION) << std::endl;
	std::cout << "TW_PASSTHRU = " << sizeof(TW_PASSTHRU) << std::endl;
	std::cout << "TW_STATUSUTF8 = " << sizeof(TW_STATUSUTF8) << std::endl;
	std::cout << "TW_TWAINDIRECT = " << sizeof(TW_TWAINDIRECT) << std::endl;
	std::cout << std::endl;
	std::cout << "TW_EXTIMAGEINFO = " << sizeof(TW_EXTIMAGEINFO) << std::endl;
	std::cout << "TW_INFO = " << sizeof(TW_INFO) << std::endl;
	int test;
	std::cin >> test;
}