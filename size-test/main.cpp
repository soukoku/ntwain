// code block to verify size of things on different platforms and archtectures

#include "twain.h"

int main()
{
	bool is32 = sizeof(void*) == 4;
	bool is64 = sizeof(void*) == 8;
	std::cout << (is32 ? "32bit= " : "") << (is64 ? "64bit" : "") << std::endl;
	std::cout << std::endl;
	std::cout << "TW_STR32  = " << sizeof TW_STR32 << std::endl;
	std::cout << "TW_STR64  = " << sizeof TW_STR64 << std::endl;
	std::cout << "TW_STR128 = " << sizeof TW_STR128 << std::endl;
	std::cout << "TW_STR255 = " << sizeof TW_STR255 << std::endl;
	std::cout << std::endl;
	std::cout << "TW_INT8   = " << sizeof TW_INT8 << std::endl;
	std::cout << "TW_INT16  = " << sizeof TW_INT16 << std::endl;
	std::cout << "TW_INT32  = " << sizeof TW_INT32 << std::endl;
	std::cout << "TW_UINT8  = " << sizeof TW_UINT8 << std::endl;
	std::cout << "TW_UINT16 = " << sizeof TW_UINT16 << std::endl;
	std::cout << "TW_UINT32 = " << sizeof TW_UINT32 << std::endl;
	std::cout << "TW_BOOL   = " << sizeof TW_BOOL << std::endl;
	std::cout << std::endl;
	std::cout << "TW_HANDLE = " << sizeof TW_HANDLE << std::endl;
	std::cout << "TW_MEMREF = " << sizeof TW_MEMREF << std::endl;
	std::cout << "TW_UINTPTR= " << sizeof TW_UINTPTR << std::endl;
	std::cout << std::endl;
	std::cout << "TW_IDENTITY = " << sizeof TW_IDENTITY << std::endl;
	int test;
	std::cin >> test;
}
