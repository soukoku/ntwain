/////////////////////////////////////////////////////////////////////////////////////////
////
////  TwainWorkingGroup.TWAIN
////
////  These are the definitions for   They're essentially the C/C++
////  H file contents translated to C#, with modifications that
////  recognize the differences between Windows, Linux and Mac OS X.
////
/////////////////////////////////////////////////////////////////////////////////////////
////  Author          Date            TWAIN       Comment
////  M.McLaughlin    17-May-2021     2.5.0.0     Updated to latest spec
////  M.McLaughlin    13-Mar-2019     2.4.0.3     Add language code page support for strings
////  M.McLaughlin    13-Nov-2015     2.4.0.0     Updated to latest spec
////  M.McLaughlin    13-Sep-2015     2.3.1.2     DsmMem bug fixes
////  M.McLaughlin    26-Aug-2015     2.3.1.1     Log fix and sync with TWAIN Direct
////  M.McLaughlin    13-Mar-2015     2.3.1.0     Numerous fixes
////  M.McLaughlin    13-Oct-2014     2.3.0.4     Added logging
////  M.McLaughlin    24-Jun-2014     2.3.0.3     Stability fixes
////  M.McLaughlin    21-May-2014     2.3.0.2     64-Bit Linux
////  M.McLaughlin    27-Feb-2014     2.3.0.1     AnyCPU support
////  M.McLaughlin    21-Oct-2013     2.3.0.0     Initial Release
/////////////////////////////////////////////////////////////////////////////////////////
////  Copyright (C) 2013-2021 Kodak Alaris Inc.
////
////  Permission is hereby granted, free of charge, to any person obtaining a
////  copy of this software and associated documentation files (the "Software"),
////  to deal in the Software without restriction, including without limitation
////  the rights to use, copy, modify, merge, publish, distribute, sublicense,
////  and/or sell copies of the Software, and to permit persons to whom the
////  Software is furnished to do so, subject to the following conditions:
////
////  The above copyright notice and this permission notice shall be included in
////  all copies or substantial portions of the Software.
////
////  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
////  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
////  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
////  THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
////  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
////  FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
////  DEALINGS IN THE SOFTWARE.
/////////////////////////////////////////////////////////////////////////////////////////

//using NTwain.Data;
//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Runtime.InteropServices;

//namespace TWAINWorkingGroup
//{
//  /// <summary>
//  /// All of our DllImports live here...
//  /// </summary>
//  internal sealed class NativeMethods
//  {
//    ///////////////////////////////////////////////////////////////////////////////
//    // Windows
//    ///////////////////////////////////////////////////////////////////////////////
//    #region Windows

//    /// <summary>
//    /// Get the ID for the current thread...
//    /// </summary>
//    /// <returns></returns>
//    [DllImport("kernel32.dll")]
//    internal static extern uint GetCurrentThreadId();

//    /// <summary>
//    /// Allocate a handle to memory...
//    /// </summary>
//    /// <param name="uFlags"></param>
//    /// <param name="dwBytes"></param>
//    /// <returns></returns>
//    [DllImport("kernel32.dll")]
//    internal static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

//    /// <summary>
//    /// Free a memory handle...
//    /// </summary>
//    /// <param name="hMem"></param>
//    /// <returns></returns>
//    [DllImport("kernel32.dll")]
//    internal static extern IntPtr GlobalFree(IntPtr hMem);

//    /// <summary>
//    /// Lock a memory handle...
//    /// </summary>
//    /// <param name="hMem"></param>
//    /// <returns></returns>
//    [DllImport("kernel32.dll")]
//    internal static extern IntPtr GlobalLock(IntPtr hMem);

//    /// <summary>
//    /// Unlock a memory handle...
//    /// </summary>
//    /// <param name="hMem"></param>
//    /// <returns></returns>
//    [DllImport("kernel32.dll")]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    internal static extern bool GlobalUnlock(IntPtr hMem);

//    [DllImport("kernel32.dll")]
//    internal static extern UIntPtr GlobalSize(IntPtr hMem);

//    [DllImport("msvcrt.dll")]
//    internal static extern UIntPtr _msize(IntPtr ptr);

//    [DllImport("libc.so")]
//    internal static extern UIntPtr malloc_usable_size(IntPtr ptr);

//    [DllImport("libSystem.dylib")]
//    internal static extern UIntPtr malloc_size(IntPtr ptr);

//    [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
//    internal static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

//    [DllImport("libc", EntryPoint = "memcpy", SetLastError = false)]
//    internal static extern void memcpy(IntPtr dest, IntPtr src, IntPtr count);

//    [DllImport("kernel32.dll", EntryPoint = "MoveMemory", SetLastError = false)]
//    internal static extern void MoveMemory(IntPtr dest, IntPtr src, uint count);

//    [DllImport("libc", EntryPoint = "memmove", SetLastError = false)]
//    internal static extern void memmove(IntPtr dest, IntPtr src, IntPtr count);

//    [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
//    internal static extern Int32 _wfopen_s(out IntPtr pFile, string filename, string mode);

//    [DllImport("libc", CharSet = CharSet.Ansi, SetLastError = true, BestFitMapping = false, ThrowOnUnmappableChar = true)]
//    internal static extern IntPtr fopen([MarshalAs(UnmanagedType.LPStr)] string filename, [MarshalAs(UnmanagedType.LPStr)] string mode);

//    [DllImport("msvcrt.dll", EntryPoint = "fwrite", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
//    internal static extern IntPtr fwriteWin(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

//    [DllImport("libc", EntryPoint = "fwrite", SetLastError = true)]
//    internal static extern IntPtr fwrite(IntPtr buffer, IntPtr size, IntPtr number, IntPtr file);

//    [DllImport("msvcrt.dll", EntryPoint = "fclose", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
//    internal static extern IntPtr fcloseWin(IntPtr file);

//    [DllImport("libc", EntryPoint = "fclose", SetLastError = true)]
//    internal static extern IntPtr fclose(IntPtr file);

//    #endregion


//    // We're supporting every DSM that we can...

//    /// <summary>
//    /// Use this entry for generic access to the DSM where the
//    /// destination must be IntPtr.Zero (null)...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="memref"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryNullDest
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr zero,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryNullDest
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr zero,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryNullDest
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr zero,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryNullDest
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr zero,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryNullDest
//    (
//        TW_IDENTITY origin,
//        IntPtr zero,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryNullDest
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr zero,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryNullDest
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr zero,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );


//    /// <summary>
//    /// Use for generic access to the DSM where the destination must
//    /// reference a data source...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="memref"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntry
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntry
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntry
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntry
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntry
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntry
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntry
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );


//    /// <summary>
//    /// Use this for DG_AUDIO / DAT.AUDIOFILEXFER / MSG.GET calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="memref"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryAudioAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryAudioAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryAudioAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryAudioAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryAudioAudiofilexfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryAudioAudiofilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryAudioAudiofilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr memref
//    );

//    /// <summary>
//    /// Use this for DG_AUDIO / DAT.AUDIOINFO / MSG.GET calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twaudioinfo"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryAudioAudioinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_AUDIOINFO twaudioinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryAudioAudioinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_AUDIOINFO twaudioinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryAudioAudioinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_AUDIOINFO twaudioinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryAudioAudioinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_AUDIOINFO twaudioinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryAudioAudioinfo
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_AUDIOINFO twaudioinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryAudioAudioinfo
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_AUDIOINFO twaudioinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryAudioAudioinfo
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_AUDIOINFO twaudioinfo
//    );

//    /// <summary>
//    /// Use this for DG_AUDIO / DAT.AUDIONATIVEXFER / MSG.GET...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="hWav"></param>
//    /// <returns></returns>
//    /// *** We'll add this later...maybe***

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.CALLBACK / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twcallback"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryCallback
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK twcallback
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryCallback
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK twcallback
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryCallback
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK twcallback
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryCallback
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK twcallback
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryCallback
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK twcallback
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryCallback
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK twcallback
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryCallback
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK twcallback
//    );
//    public delegate UInt16 WindowsDsmEntryCallbackDelegate
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );
//    public delegate UInt16 LinuxDsmEntryCallbackDelegate
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );
//    public delegate UInt16 Linux020302Dsm64bitEntryCallbackDelegate
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );
//    public delegate UInt16 MacosxDsmEntryCallbackDelegate
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.CALLBACK2 / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twcallback"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryCallback2
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK2 twcallback2
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryCallback2
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK2 twcallback2
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryCallback2
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK2 twcallback2
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryCallback2
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK2 twcallback2
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryCallback2
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK2 twcallback2
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryCallback2
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK2 twcallback
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryCallback2
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY des,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CALLBACK2 twcallback
//    );
//    private delegate UInt16 WindowsDsmEntryCallback2Delegate
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );
//    private delegate UInt16 LinuxDsmEntryCallback2Delegate
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );
//    private delegate UInt16 Linux020302Dsm64bitEntryCallback2Delegate
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );
//    private delegate UInt16 MacosxDsmEntryCallback2Delegate
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twnull
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.CAPABILITY / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twcapability"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryCapability
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CAPABILITY twcapability
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryCapability
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CAPABILITY twcapability
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryCapability
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CAPABILITY twcapability
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryCapability
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CAPABILITY twcapability
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryCapability
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CAPABILITY twcapability
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryCapability
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CAPABILITY twcapability
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryCapability
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CAPABILITY twcapability
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.CUSTOMDSDATA / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twcustomdsdata"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryCustomdsdata
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CUSTOMDSDATA twcustomedsdata
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryCustomdsdata
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CUSTOMDSDATA twcustomdsdata
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryCustomdsdata
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CUSTOMDSDATA twcustomdsdata
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryCustomdsdata
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CUSTOMDSDATA twcustomdsdata
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryCustomdsdata
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CUSTOMDSDATA twcustomdsdata
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryCustomdsdata
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CUSTOMDSDATA twcustomedsdata
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryCustomdsdata
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CUSTOMDSDATA twcustomedsdata
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.DEVICEEVENT / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twdeviceevent"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryDeviceevent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_DEVICEEVENT twdeviceevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryDeviceevent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_DEVICEEVENT twdeviceevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryDeviceevent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_DEVICEEVENT twdeviceevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryDeviceevent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_DEVICEEVENT twdeviceevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryDeviceevent
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_DEVICEEVENT twdeviceevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryDeviceevent
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_DEVICEEVENT twdeviceevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryDeviceevent
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_DEVICEEVENT twdeviceevent
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.EVENT / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twevent"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryEvent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EVENT twevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryEvent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EVENT twevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryEvent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EVENT twevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryEvent
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EVENT twevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryEvent
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EVENT twevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryEvent
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EVENT twevent
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryEvent
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EVENT twevent
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.ENTRYPOINT / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twentrypoint"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryEntrypoint
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_ENTRYPOINT twentrypoint
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryEntrypoint
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_ENTRYPOINT twentrypoint
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryEntrypoint
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_ENTRYPOINT twentrypoint
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryEntrypoint
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_ENTRYPOINT twentrypoint
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryEntrypoint
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_ENTRYPOINT_LINUX64 twentrypoint
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryEntrypoint
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_ENTRYPOINT twentrypoint
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryEntrypoint
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_ENTRYPOINT twentrypoint
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.FILESYSTEM / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twentrypoint"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryFilesystem
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILESYSTEM twfilesystem
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryFilesystem
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILESYSTEM twfilesystem
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryFilesystem
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILESYSTEM twfilesystem
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryFilesystem
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILESYSTEM twfilesystem
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryFilesystem
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILESYSTEM twfilesystem
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryFilesystem
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILESYSTEM twfilesystem
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryFilesystem
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILESYSTEM twfilesystem
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.IDENTITY / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twidentity"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryIdentity
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_LEGACY twidentity
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryIdentityState4
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_LEGACY twidentity
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryIdentity
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_LEGACY twidentity
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryIdentity
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_LEGACY twidentity
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryIdentity
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_LEGACY twidentity
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryIdentity
//    (
//        TW_IDENTITY_LINUX64 origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_LINUX64 twidentity
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryIdentity
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_MACOSX twidentity
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryIdentity
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        TW_IDENTITY_MACOSX twidentity
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.NULL / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="memref"></param>
//    /// <returns></returns>
//    /// ***Only needed for drivers, so we don't have it***

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.PARENT / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="hbitmap"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryParent
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr hwnd
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryParent
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr hwnd
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryParent
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr hwnd
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryParent
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr hwnd
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryParent
//    (
//        TW_IDENTITY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr hwnd
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryParent
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr hwnd
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryParent
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr hwnd
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.PASSTHRU / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twpassthru"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryPassthru
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PASSTHRU twpassthru
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryPassthru
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PASSTHRU twpassthru
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryPassthru
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PASSTHRU twpassthru
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryPassthru
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PASSTHRU twpassthru
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryPassthru
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PASSTHRU twpassthru
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryPassthru
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PASSTHRU twpassthru
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryPassthru
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PASSTHRU twpassthru
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.PENDINGXFERS / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twpendingxfers"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryPendingxfers
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PENDINGXFERS twpendingxfers
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryPendingxfers
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PENDINGXFERS twpendingxfers
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryPendingxfers
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PENDINGXFERS twpendingxfers
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryPendingxfers
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PENDINGXFERS twpendingxfers
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryPendingxfers
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PENDINGXFERS twpendingxfers
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryPendingxfers
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PENDINGXFERS twpendingxfers
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryPendingxfers
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PENDINGXFERS twpendingxfers
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.SETUPFILEXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twsetupfilexfer"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntrySetupfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPFILEXFER twsetupfilexfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntrySetupfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPFILEXFER twsetupfilexfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntrySetupfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPFILEXFER twsetupfilexfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntrySetupfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPFILEXFER twsetupfilexfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntrySetupfilexfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPFILEXFER twsetupfilexfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntrySetupfilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPFILEXFER twsetupfilexfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntrySetupfilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPFILEXFER twsetupfilexfer
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.SETUPMEMXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twsetupmemxfer"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntrySetupmemxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPMEMXFER twsetupmemxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntrySetupmemxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPMEMXFER twsetupmemxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntrySetupmemxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPMEMXFER twsetupmemxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntrySetupmemxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPMEMXFER twsetupmemxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntrySetupmemxfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPMEMXFER twsetupmemxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntrySetupmemxfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPMEMXFER twsetupmemxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntrySetupmemxfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_SETUPMEMXFER twsetupmemxfer
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.STATUS / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twstatus"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryStatus
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryStatusState3
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryStatus
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryStatusState3
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryStatus
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryStatusState3
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryStatus
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryStatusState3
//    (
//        TW_IDENTITY_LEGACY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryStatus
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryStatusState3
//    (
//        TW_IDENTITY origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryStatus
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryStatusState3
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryStatus
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryStatusState3
//    (
//        TW_IDENTITY_MACOSX origin,
//        IntPtr dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUS twstatus
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.STATUSUTF8 / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twstatusutf8"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryStatusutf8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUSUTF8 twstatusutf8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryStatusutf8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUSUTF8 twstatusutf8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryStatusutf8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUSUTF8 twstatusutf8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryStatusutf8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUSUTF8 twstatusutf8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryStatusutf8
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUSUTF8 twstatusutf8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryStatusutf8
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUSUTF8 twstatusutf8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryStatusutf8
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_STATUSUTF8 twstatusutf8
//    );

//    /// <summary>
//    /// Use this for DG.CONTROL / DAT.TWAINDIRECT / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twtwaindirect"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryTwaindirect
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_TWAINDIRECT twtwaindirect
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryTwaindirect
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_TWAINDIRECT twtwaindirect
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryTwaindirect
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_TWAINDIRECT twtwaindirect
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryTwaindirect
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_TWAINDIRECT twtwaindirect
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryTwaindirect
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_TWAINDIRECT twtwaindirect
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryTwaindirect
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_TWAINDIRECT twtwaindirect
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryTwaindirect
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_TWAINDIRECT twtwaindirect
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.USERINTERFACE / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twuserinterface"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryUserinterface
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_USERINTERFACE twuserinterface
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryUserinterface
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_USERINTERFACE twuserinterface
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryUserinterface
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_USERINTERFACE twuserinterface
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryUserinterface
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_USERINTERFACE twuserinterface
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryUserinterface
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_USERINTERFACE twuserinterface
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryUserinterface
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_USERINTERFACE twuserinterface
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryUserinterface
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_USERINTERFACE twuserinterface
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.XFERGROUP / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twuint32"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryXfergroup
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref UInt32 twuint32
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryXfergroup
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref UInt32 twuint32
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryXfergroup
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref UInt32 twuint32
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryXfergroup
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref UInt32 twuint32
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryXfergroup
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref UInt32 twuint32
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryXfergroup
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref UInt32 twuint32
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryXfergroup
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref UInt32 twuint32
//    );

//    /// <summary>
//    /// Use this for DG_AUDIO / DAT.AUDIOFILEXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twmemref"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryAudiofilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryAudiofilexfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryAudiofilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryAudiofilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );

//    /// <summary>
//    /// Use this for DG_AUDIO / DAT.AUDIONATIVEXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="intptr"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryAudionativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrWav
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryAudionativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrWav
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryAudionativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrWav
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryAudionativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrWav
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryAudionativexfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrWav
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryAudionativexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrAiff
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryAudionativexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrAiff
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.CIECOLOR / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twciecolor"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryCiecolor
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CIECOLOR twciecolor
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryCiecolor
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CIECOLOR twciecolor
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryCiecolor
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CIECOLOR twciecolor
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryCiecolor
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CIECOLOR twciecolor
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryCiecolor
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CIECOLOR twciecolor
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryCiecolor
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CIECOLOR twciecolor
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryCiecolor
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_CIECOLOR twciecolor
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.EXTIMAGEINFO / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twextimageinfo"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryExtimageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EXTIMAGEINFO twextimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryExtimageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EXTIMAGEINFO twextimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryExtimageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EXTIMAGEINFO twextimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryExtimageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EXTIMAGEINFO twextimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryExtimageinfo
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EXTIMAGEINFO twextimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryExtimageinfo
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EXTIMAGEINFO twextimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryExtimageinfo
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_EXTIMAGEINFO twextimageinfo
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.FILTER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twfilter"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryFilter
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILTER twfilter
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryFilter
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILTER twfilter
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryFilter
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILTER twfilter
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryFilter
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILTER twfilter
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryFilter
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILTER twfilter
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryFilter
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILTER twfilter
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryFilter
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_FILTER twfilter
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.GRAYRESPONSE / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twgrayresponse"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryGrayresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_GRAYRESPONSE twgrayresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryGrayresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_GRAYRESPONSE twgrayresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryGrayresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_GRAYRESPONSE twgrayresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryGrayresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_GRAYRESPONSE twgrayresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryGrayresponse
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_GRAYRESPONSE twgrayresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryGrayresponse
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_GRAYRESPONSE twgrayresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryGrayresponse
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_GRAYRESPONSE twgrayresponse
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.ICCPROFILE / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twmemory"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryIccprofile
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_MEMORY twmemory
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryIccprofile
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_MEMORY twmemory
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryIccprofile
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_MEMORY twmemory
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryIccprofile
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_MEMORY twmemory
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryIccprofile
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_MEMORY twmemory
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryIccprofile
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_MEMORY twmemory
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryIccprofile
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_MEMORY twmemory
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.IMAGEFILEXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twmemref"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryImagefilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryImagefilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryImagefilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryImagefilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryImagefilexfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryImagefilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryImagefilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        IntPtr twmemref
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.IMAGEINFO / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twimageinfo"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryImageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEINFO twimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryImageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEINFO twimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryImageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEINFO twimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryImageinfo
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEINFO twimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryImageinfo
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEINFO_LINUX64 twimageinfolinux64
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryImageinfo
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEINFO twimageinfo
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryImageinfo
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEINFO twimageinfo
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.IMAGELAYOUT / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twimagelayout"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryImagelayout
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGELAYOUT twimagelayout
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryImagelayout
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGELAYOUT twimagelayout
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryImagelayout
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGELAYOUT twimagelayout
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryImagelayout
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGELAYOUT twimagelayout
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryImagelayout
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGELAYOUT twimagelayout
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryImagelayout
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGELAYOUT twimagelayout
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryImagelayout
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGELAYOUT twimagelayout
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.IMAGEMEMFILEXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twimagememxfer"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryImagememfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryImagememfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryImagememfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryImagememfilexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryImagememfilexfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryImagememfilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryImagememfilexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.IMAGEMEMXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twimagememxfer"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryImagememxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryImagememxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryImagememxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryImagememxfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryImagememxfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryImagememxfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryImagememxfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_IMAGEMEMXFER_MACOSX twimagememxfer
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.IMAGENATIVEXFER / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="bitmap"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryImagenativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrBitmap
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryImagenativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrBitmap
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryImagenativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrBitmap
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryImagenativexfer
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrBitmap
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryImagenativexfer
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrBitmap
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryImagenativexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrBitmap
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryImagenativexfer
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref IntPtr intptrBitmap
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.JPEGCOMPRESSION / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twjpegcompression"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryJpegcompression
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_JPEGCOMPRESSION twjpegcompression
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryJpegcompression
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_JPEGCOMPRESSION twjpegcompression
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryJpegcompression
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_JPEGCOMPRESSION twjpegcompression
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryJpegcompression
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_JPEGCOMPRESSION twjpegcompression
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryJpegcompression
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_JPEGCOMPRESSION twjpegcompression
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryJpegcompression
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_JPEGCOMPRESSION twjpegcompression
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryJpegcompression
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_JPEGCOMPRESSION twjpegcompression
//    );

//    /// <summary>
//    /// Use this for DG_CONTROL / DAT.METRICS / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twmetrics"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryMetrics
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_METRICS twmetrics
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryMetrics
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_METRICS twmetrics
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryMetrics
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_METRICS twmetrics
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryMetrics
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_METRICS twmetrics
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryMetrics
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_METRICS twmetrics
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryMetrics
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_METRICS twmetrics
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryMetrics
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_METRICS twmetrics
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.PALETTE8 / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twpalette8"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryPalette8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PALETTE8 twpalette8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryPalette8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PALETTE8 twpalette8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryPalette8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PALETTE8 twpalette8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryPalette8
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PALETTE8 twpalette8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryPalette8
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PALETTE8 twpalette8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryPalette8
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PALETTE8 twpalette8
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryPalette8
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_PALETTE8 twpalette8
//    );

//    /// <summary>
//    /// Use this for DG_IMAGE / DAT.RGBRESPONSE / MSG.* calls...
//    /// </summary>
//    /// <param name="origin"></param>
//    /// <param name="dest"></param>
//    /// <param name="dg"></param>
//    /// <param name="dat"></param>
//    /// <param name="msg"></param>
//    /// <param name="twrgbresponse"></param>
//    /// <returns></returns>
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twain_32.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwain32DsmEntryRgbresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_RGBRESPONSE twrgbresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("twaindsm.dll", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 WindowsTwaindsmDsmEntryRgbresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_RGBRESPONSE twrgbresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 LinuxDsmEntryRgbresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_RGBRESPONSE twrgbresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib64/libtwaindsm.so", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux64DsmEntryRgbresponse
//    (
//        TW_IDENTITY_LEGACY origin,
//        TW_IDENTITY_LEGACY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_RGBRESPONSE twrgbresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/usr/local/lib/libtwaindsm.so.2.3.2", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 Linux020302Dsm64bitEntryRgbresponse
//    (
//        TW_IDENTITY origin,
//        TW_IDENTITY dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_RGBRESPONSE twrgbresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/System/Library/Frameworks/framework/TWAIN", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwainDsmEntryRgbresponse
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_RGBRESPONSE twrgbresponse
//    );
//    [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
//    [DllImport("/Library/Frameworks/TWAINDSM.framework/TWAINDSM", EntryPoint = "DSM_Entry", CharSet = CharSet.Ansi)]
//    internal static extern UInt16 MacosxTwaindsmDsmEntryRgbresponse
//    (
//        TW_IDENTITY_MACOSX origin,
//        TW_IDENTITY_MACOSX dest,
//        DG dg,
//        DAT dat,
//        MSG msg,
//        ref TW_RGBRESPONSE twrgbresponse
//    );

//    //}
//  }
//}