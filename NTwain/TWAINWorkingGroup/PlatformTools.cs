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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TWAINWorkingGroup
{
    static partial class PlatformTools
    {
        /// <summary>
        /// The platform we're running on...
        /// </summary>
        private static Platform ms_platform;
        private static Processor ms_processor;
        private static bool ms_blFirstPassGetPlatform = true;

        /// <summary>
        /// 32-bit or 64-bit...
        /// </summary>
        /// <returns>Number of bits in the machine word for this process</returns>
        public static int GetMachineWordBitSize()
        {
            return ((IntPtr.Size == 4) ? 32 : 64);
        }

        /// <summary>
        /// Quick access to our platform id...
        /// </summary>
        /// <returns></returns>
        public static Platform GetPlatform()
        {
            // First pass...
            if (ms_blFirstPassGetPlatform)
            {
                // Dont'c come in here again...
                ms_blFirstPassGetPlatform = false;

                // We're Windows...
                if (Environment.OSVersion.ToString().Contains("Microsoft Windows"))
                {
                    ms_platform = Platform.WINDOWS;
                    ms_processor = (GetMachineWordBitSize() == 64) ? Processor.X86_64 : Processor.X86;
                }

                // We're Mac OS X (this has to come before LINUX!!!)...
                else if (Directory.Exists("/Library/Application Support"))
                {
                    ms_platform = Platform.MACOSX;
                    ms_processor = (GetMachineWordBitSize() == 64) ? Processor.X86_64 : Processor.X86;
                }

                // We're Linux...
                else if (Environment.OSVersion.ToString().Contains("Unix"))
                {
                    string szProcessor = "";
                    ms_platform = Platform.LINUX;
                    try
                    {
                        using (Process process = new Process
                        {
                            StartInfo = new ProcessStartInfo()
                            {
                                FileName = "uname",
                                Arguments = "-m",
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                CreateNoWindow = true
                            }
                        })
                        {
                            process.Start();
                            process.WaitForExit();
                            szProcessor = process.StandardOutput.ReadToEnd();
                            process.Close();
                        }
                    }
                    catch
                    {
                        Console.Out.WriteLine("Oh dear, this isn't good...where's uname?");
                    }
                    if (szProcessor.Contains("mips64"))
                    {
                        ms_processor = Processor.MIPS64EL;
                    }
                    else
                    {
                        ms_processor = (GetMachineWordBitSize() == 64) ? Processor.X86_64 : Processor.X86;
                    }
                }

                // We have a problem, Log will throw for us...
                else
                {
                    ms_platform = Platform.UNKNOWN;
                    ms_processor = Processor.UNKNOWN;
                    Log.Assert("Unsupported platform..." + ms_platform);
                }
            }

            // All done...
            return (ms_platform);
        }

        /// <summary>
        /// Quick access to our processor id...
        /// </summary>
        /// <returns></returns>
        public static Processor GetProcessor()
        {
            // First pass...
            if (ms_blFirstPassGetPlatform)
            {
                GetPlatform();
            }

            // All done...
            return (ms_processor);
        }

    }
}
