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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TWAINWorkingGroup
{

    /// <summary>
    /// All of our DllImports live here...
    /// </summary>
    static class NativeMethods
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Windows
        ///////////////////////////////////////////////////////////////////////////////
        #region Windows

        /// <summary>
        /// Get the ID for the current thread...
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern uint GetCurrentThreadId();

        /// <summary>
        /// Allocate a handle to memory...
        /// </summary>
        /// <param name="uFlags"></param>
        /// <param name="dwBytes"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        /// <summary>
        /// Free a memory handle...
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GlobalFree(IntPtr hMem);

        /// <summary>
        /// Lock a memory handle...
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        internal static extern IntPtr GlobalLock(IntPtr hMem);

        /// <summary>
        /// Unlock a memory handle...
        /// </summary>
        /// <param name="hMem"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        internal static extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("msvcrt.dll")]
        internal static extern UIntPtr _msize(IntPtr ptr);

        [DllImport("libc.so")]
        internal static extern UIntPtr malloc_usable_size(IntPtr ptr);

        [DllImport("libSystem.dylib")]
        internal static extern UIntPtr malloc_size(IntPtr ptr);

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        internal static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("libc", EntryPoint = "memcpy", SetLastError = false)]
        internal static extern void memcpy(IntPtr dest, IntPtr src, IntPtr count);

        [DllImport("kernel32.dll", EntryPoint = "MoveMemory", SetLastError = false)]
        internal static extern void MoveMemory(IntPtr dest, IntPtr src, uint count);

        [DllImport("libc", EntryPoint = "memmove", SetLastError = false)]
        internal static extern void memmove(IntPtr dest, IntPtr src, IntPtr count);

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern Int32 _wfopen_s(out IntPtr pFile, string filename, string mode);

        [DllImport("libc", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr fopen([MarshalAs(UnmanagedType.LPStr)] string filename, [MarshalAs(UnmanagedType.LPStr)] string mode);

        [DllImport("msvcrt.dll", EntryPoint = "fwrite", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern IntPtr fwriteWin(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

        [DllImport("libc", EntryPoint = "fwrite", SetLastError = true)]
        internal static extern IntPtr fwrite(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

        [DllImport("msvcrt.dll", EntryPoint = "fclose", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        internal static extern IntPtr fcloseWin(IntPtr file);

        [DllImport("libc", EntryPoint = "fclose", SetLastError = true)]
        internal static extern IntPtr fclose(IntPtr file);

        #endregion


        // We're supporting every DSM that we can...

        /// <summary>
        /// Use this entry for generic access to the DSM where the
        /// destination must be IntPtr.Zero (null)...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="memref"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryNullDest
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryNullDest
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryNullDest
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryNullDest
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryNullDest
        (
            ref TW_IDENTITY origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryNullDest
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryNullDest
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr zero,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );


        /// <summary>
        /// Use for generic access to the DSM where the destination must
        /// reference a data source...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="memref"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntry
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntry
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntry
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntry
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntry
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntry
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntry
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );


        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIOFILEXFER / MSG.GET calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="memref"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudioAudiofilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr memref
        );

        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIOINFO / MSG.GET calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twaudioinfo"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudioAudioinfo
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudioAudioinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_AUDIOINFO twaudioinfo
        );

        ///// <summary>
        ///// Use this for DG_AUDIO / DAT.AUDIONATIVEXFER / MSG.GET...
        ///// </summary>
        ///// <param name="origin"></param>
        ///// <param name="dest"></param>
        ///// <param name="dg"></param>
        ///// <param name="dat"></param>
        ///// <param name="msg"></param>
        ///// <param name="hWav"></param>
        ///// <returns></returns>
        ///// *** We'll add this later...maybe***

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CALLBACK / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcallback"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCallback
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCallback
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCallback
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCallback
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCallback
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCallback
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCallback
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK twcallback
        );
        public delegate UInt16 WindowsDsmEntryCallbackDelegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        public delegate UInt16 LinuxDsmEntryCallbackDelegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        public delegate UInt16 Linux020302Dsm64bitEntryCallbackDelegate
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        public delegate UInt16 MacosxDsmEntryCallbackDelegate
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CALLBACK2 / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcallback"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCallback2
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCallback2
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCallback2
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCallback2
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCallback2
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback2
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCallback2
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCallback2
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY des,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CALLBACK2 twcallback
        );
        private delegate UInt16 WindowsDsmEntryCallback2Delegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 LinuxDsmEntryCallback2Delegate
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 Linux020302Dsm64bitEntryCallback2Delegate
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );
        private delegate UInt16 MacosxDsmEntryCallback2Delegate
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CAPABILITY / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcapability"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCapability
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCapability
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCapability
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCapability
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCapability
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCapability
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCapability
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CAPABILITY twcapability
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.CUSTOMDSDATA / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twcustomdsdata"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCustomdsdata
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomedsdata
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCustomdsdata
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomdsdata
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCustomdsdata
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomdsdata
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCustomdsdata
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomdsdata
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCustomdsdata
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomdsdata
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCustomdsdata
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomedsdata
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCustomdsdata
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CUSTOMDSDATA twcustomedsdata
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.DEVICEEVENT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twdeviceevent"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryDeviceevent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryDeviceevent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryDeviceevent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryDeviceevent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryDeviceevent
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryDeviceevent
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryDeviceevent
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_DEVICEEVENT twdeviceevent
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.EVENT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twevent"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryEvent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryEvent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryEvent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryEvent
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryEvent
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryEvent
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryEvent
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EVENT twevent
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.ENTRYPOINT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twentrypoint"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryEntrypoint
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryEntrypoint
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryEntrypoint
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryEntrypoint
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryEntrypoint
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT_LINUX64 twentrypoint
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryEntrypoint
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryEntrypoint
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_ENTRYPOINT twentrypoint
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.FILESYSTEM / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twentrypoint"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryFilesystem
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryFilesystem
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryFilesystem
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryFilesystem
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryFilesystem
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryFilesystem
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryFilesystem
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILESYSTEM twfilesystem
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.IDENTITY / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twidentity"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryIdentity
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryIdentityState4
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryIdentity
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryIdentity
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryIdentity
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LEGACY twidentity
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryIdentity
        (
            ref TW_IDENTITY_LINUX64 origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_LINUX64 twidentity
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryIdentity
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_MACOSX twidentity
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryIdentity
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IDENTITY_MACOSX twidentity
        );

        ///// <summary>
        ///// Use this for DG_CONTROL / DAT.NULL / MSG.* calls...
        ///// </summary>
        ///// <param name="origin"></param>
        ///// <param name="dest"></param>
        ///// <param name="dg"></param>
        ///// <param name="dat"></param>
        ///// <param name="msg"></param>
        ///// <param name="memref"></param>
        ///// <returns></returns>
        ///// ***Only needed for drivers, so we don't have it***

        /// <summary>
        /// Use this for DG_CONTROL / DAT.PARENT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="hbitmap"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryParent
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryParent
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryParent
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryParent
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryParent
        (
            ref TW_IDENTITY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryParent
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryParent
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr hwnd
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.PASSTHRU / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twpassthru"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryPassthru
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryPassthru
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryPassthru
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryPassthru
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryPassthru
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryPassthru
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryPassthru
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PASSTHRU twpassthru
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.PENDINGXFERS / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twpendingxfers"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryPendingxfers
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryPendingxfers
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryPendingxfers
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryPendingxfers
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryPendingxfers
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryPendingxfers
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryPendingxfers
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PENDINGXFERS twpendingxfers
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.SETUPFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twsetupfilexfer"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntrySetupfilexfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntrySetupfilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPFILEXFER twsetupfilexfer
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.SETUPMEMXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twsetupmemxfer"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntrySetupmemxfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntrySetupmemxfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_SETUPMEMXFER twsetupmemxfer
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.STATUS / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twstatus"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryStatus
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryStatusState3
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryStatus
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryStatusState3
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryStatus
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryStatusState3
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryStatus
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryStatusState3
        (
            ref TW_IDENTITY_LEGACY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryStatus
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryStatusState3
        (
            ref TW_IDENTITY origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryStatus
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryStatusState3
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryStatus
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryStatusState3
        (
            ref TW_IDENTITY_MACOSX origin,
            IntPtr dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUS twstatus
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.STATUSUTF8 / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twstatusutf8"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryStatusutf8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryStatusutf8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryStatusutf8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryStatusutf8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryStatusutf8
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryStatusutf8
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryStatusutf8
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_STATUSUTF8 twstatusutf8
        );

        /// <summary>
        /// Use this for DG.CONTROL / DAT.TWAINDIRECT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twtwaindirect"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryTwaindirect
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_TWAINDIRECT twtwaindirect
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryTwaindirect
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_TWAINDIRECT twtwaindirect
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryTwaindirect
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_TWAINDIRECT twtwaindirect
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryTwaindirect
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_TWAINDIRECT twtwaindirect
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryTwaindirect
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_TWAINDIRECT twtwaindirect
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryTwaindirect
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_TWAINDIRECT twtwaindirect
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryTwaindirect
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_TWAINDIRECT twtwaindirect
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.USERINTERFACE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twuserinterface"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryUserinterface
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryUserinterface
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryUserinterface
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryUserinterface
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryUserinterface
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryUserinterface
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryUserinterface
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_USERINTERFACE twuserinterface
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.XFERGROUP / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twuint32"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryXfergroup
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryXfergroup
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryXfergroup
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryXfergroup
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryXfergroup
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryXfergroup
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryXfergroup
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref UInt32 twuint32
        );

        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIOFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmemref"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudiofilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudiofilexfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudiofilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudiofilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );

        /// <summary>
        /// Use this for DG_AUDIO / DAT.AUDIONATIVEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="intptr"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryAudionativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrWav
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryAudionativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrWav
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryAudionativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrWav
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryAudionativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrWav
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryAudionativexfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrWav
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryAudionativexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrAiff
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryAudionativexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrAiff
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.CIECOLOR / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twciecolor"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryCiecolor
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryCiecolor
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryCiecolor
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryCiecolor
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryCiecolor
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryCiecolor
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryCiecolor
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_CIECOLOR twciecolor
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.EXTIMAGEINFO / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twextimageinfo"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryExtimageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryExtimageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryExtimageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryExtimageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryExtimageinfo
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryExtimageinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryExtimageinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_EXTIMAGEINFO twextimageinfo
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.FILTER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twfilter"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryFilter
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryFilter
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryFilter
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryFilter
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryFilter
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryFilter
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryFilter
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_FILTER twfilter
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.GRAYRESPONSE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twgrayresponse"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryGrayresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryGrayresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryGrayresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryGrayresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryGrayresponse
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryGrayresponse
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryGrayresponse
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_GRAYRESPONSE twgrayresponse
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.ICCPROFILE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmemory"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryIccprofile
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryIccprofile
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryIccprofile
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryIccprofile
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryIccprofile
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryIccprofile
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryIccprofile
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_MEMORY twmemory
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmemref"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagefilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagefilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagefilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagefilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagefilexfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagefilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagefilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twmemref
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEINFO / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimageinfo"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImageinfo
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImageinfo
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO_LINUX64 twimageinfolinux64
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImageinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImageinfo
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEINFO twimageinfo
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGELAYOUT / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimagelayout"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagelayout
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagelayout
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagelayout
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagelayout
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagelayout
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagelayout
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagelayout
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGELAYOUT twimagelayout
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEMEMFILEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimagememxfer"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagememfilexfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagememfilexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGEMEMXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twimagememxfer"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagememxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagememxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagememxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagememxfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER twimagememxfer
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagememxfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagememxfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagememxfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.IMAGENATIVEXFER / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryImagenativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryImagenativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryImagenativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryImagenativexfer
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryImagenativexfer
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryImagenativexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryImagenativexfer
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref IntPtr intptrBitmap
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.JPEGCOMPRESSION / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twjpegcompression"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryJpegcompression
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryJpegcompression
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryJpegcompression
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryJpegcompression
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryJpegcompression
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryJpegcompression
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryJpegcompression
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_JPEGCOMPRESSION twjpegcompression
        );

        /// <summary>
        /// Use this for DG_CONTROL / DAT.METRICS / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twmetrics"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryMetrics
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_METRICS twmetrics
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryMetrics
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_METRICS twmetrics
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryMetrics
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_METRICS twmetrics
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryMetrics
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_METRICS twmetrics
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryMetrics
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_METRICS twmetrics
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryMetrics
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_METRICS twmetrics
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryMetrics
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_METRICS twmetrics
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.PALETTE8 / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twpalette8"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryPalette8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryPalette8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryPalette8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryPalette8
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryPalette8
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryPalette8
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryPalette8
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_PALETTE8 twpalette8
        );

        /// <summary>
        /// Use this for DG_IMAGE / DAT.RGBRESPONSE / MSG.* calls...
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="dest"></param>
        /// <param name="dg"></param>
        /// <param name="dat"></param>
        /// <param name="msg"></param>
        /// <param name="twrgbresponse"></param>
        /// <returns></returns>
        
        [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwain32DsmEntryRgbresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        
        [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 WindowsTwaindsmDsmEntryRgbresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 LinuxDsmEntryRgbresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        
        [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux64DsmEntryRgbresponse
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        
        [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 Linux020302Dsm64bitEntryRgbresponse
        (
            ref TW_IDENTITY origin,
            ref TW_IDENTITY dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        
        [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwainDsmEntryRgbresponse
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );
        
        [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
        internal static extern UInt16 MacosxTwaindsmDsmEntryRgbresponse
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            ref TW_RGBRESPONSE twrgbresponse
        );

    }
}
