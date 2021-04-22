///////////////////////////////////////////////////////////////////////////////////////
//
//  TwainWorkingGroup.TWAIN
//
//  These are the definitions for TWAIN.  They're essentially the C/C++
//  TWAIN.H file contents translated to C#, with modifications that
//  recognize the differences between Windows, Linux and Mac OS X.
//
///////////////////////////////////////////////////////////////////////////////////////
//  Author          Date            TWAIN       Comment
//  M.McLaughlin    13-Mar-2019     2.4.0.3     Add language code page support for strings
//  M.McLaughlin    13-Nov-2015     2.4.0.0     Updated to latest spec
//  M.McLaughlin    13-Sep-2015     2.3.1.2     DsmMem bug fixes
//  M.McLaughlin    26-Aug-2015     2.3.1.1     Log fix and sync with TWAIN Direct
//  M.McLaughlin    13-Mar-2015     2.3.1.0     Numerous fixes
//  M.McLaughlin    13-Oct-2014     2.3.0.4     Added logging
//  M.McLaughlin    24-Jun-2014     2.3.0.3     Stability fixes
//  M.McLaughlin    21-May-2014     2.3.0.2     64-Bit Linux
//  M.McLaughlin    27-Feb-2014     2.3.0.1     AnyCPU support
//  M.McLaughlin    21-Oct-2013     2.3.0.0     Initial Release
///////////////////////////////////////////////////////////////////////////////////////
//  Copyright (C) 2013-2020 Kodak Alaris Inc.
//
//  Permission is hereby granted, free of charge, to any person obtaining a
//  copy of this software and associated documentation files (the "Software"),
//  to deal in the Software without restriction, including without limitation
//  the rights to use, copy, modify, merge, publish, distribute, sublicense,
//  and/or sell copies of the Software, and to permit persons to whom the
//  Software is furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in
//  all copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
//  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
//  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//  DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////

namespace TWAINWorkingGroup
{
    public static class Consts
    {
        /// <summary>
        /// Don't care values...
        /// </summary>
        public const byte TWON_DONTCARE8 = 0xff;
        public const ushort TWON_DONTCARE16 = 0xff;
        public const uint TWON_DONTCARE32 = 0xffffffff;


        /// <summary>
        /// We're departing from a strict translation of TWAIN.H so that
        /// we can achieve a unified status return type.  
        /// </summary>
        public const int STSCC = 0x10000; // get us past the custom space
    }
}
