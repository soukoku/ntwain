///////////////////////////////////////////////////////////////////////////////////////
//
//  TwainWorkingGroup.TWAIN
//
//  This is a wrapper class for basic TWAIN functionality.  It establishes
//  behavior that every application should adhere to.  It also hides OS
//  specific details, so that toolkits or applications can use one unified
//  interface to TWAIN.
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

using NTwain;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace TWAINWorkingGroup
{
    /// <summary>
    /// This is the base TWAIN class.  It's versioned so that developers can
    /// safely make a reference to it, if they don't want to actually include
    /// the TWAIN modules in their project.
    /// 
    /// Here are the goals of this class:
    ///
    /// - Make the interface better than raw TWAIN (like with C/C++), but don't
    ///   go as far as making a toolkit.  Expose as much of TWAIN as possible.
    ///   Part of this involves making a CSV interface to abstract away some of
    ///   the pain of the C/C++ structures, especially TW_CAPABILITY.
    ///   
    /// - Use type checking wherever possible.  This is why the TWAIN contants
    ///   tend to be enumerations and why there are multiple entry points into
    ///   the DSM_Entry function.
    ///   
    /// - Avoid unsafe code.  We use marshalling, and we're forced to use some
    ///   unmanaged memory, but that's it.
    ///   
    /// - Run thread safe.  Force as many TWAIN calls into a single thread as
    ///   possible.  The main exceptions are MSG_PROCESSEVENT and any of the
    ///   calls that unwind the state machine (ex: MSG_DISABLEDS).  Otherwise
    ///   all of the calls are serialized through a single thread.
    ///   
    /// - Avoid use of System.Windows content, so that the TWAIN assembly can be
    ///   used as broadly as possible (specifically with Mono).
    ///   
    /// - Support all platforms.  The code currently works on 32-bit and 64-bit
    ///   Windows.  It's been tested on 32-bit Linux and Mac OS X (using Mono).
    ///   It should work as 64-bit on those platforms.  We're also supporting
    ///   both TWAIN_32.DLL and TWAINDSM.DLL on Windows, and we'll support both
    ///   TWAIN.framework and a TWAINDSM.framework (whenever it gets created).
    ///   
    /// - Virtualiaze all public functions so that developers can extended the
    ///   class with a minimum of fuss.
    /// </summary>
    public partial class TWAIN : IDisposable
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Public Functions, these are the essentials...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Functions...

        /// <summary>
        /// Our constructor...
        /// </summary>
        /// <param name="a_szManufacturer">Application manufacturer</param>
        /// <param name="a_szProductFamily">Application product family</param>
        /// <param name="a_szProductName">Name of the application</param>
        /// <param name="a_u16ProtocolMajor">TWAIN protocol major (doesn't have to match TWAINH.CS)</param>
        /// <param name="a_u16ProtocolMinor">TWAIN protocol minor (doesn't have to match TWAINH.CS)</param>
        /// <param name="a_u32SupportedGroups">Bitmask of DG_ flags</param>
        /// <param name="a_twcy">Country code for the application</param>
        /// <param name="a_szInfo">Info about the application</param>
        /// <param name="a_twlg">Language code for the application</param>
        /// <param name="a_u16MajorNum">Application's major version</param>
        /// <param name="a_u16MinorNum">Application's minor version</param>
        /// <param name="a_blUseLegacyDSM">Use the legacy DSM (like TWAIN_32.DLL)</param>
        /// <param name="a_blUseCallbacks">Use callbacks instead of Windows post message</param>
        /// <param name="a_deviceeventback">Function to receive device events</param>
        /// <param name="a_scancallback">Function to handle scanning</param>
        /// <param name="a_runinuithreaddelegate">Help us run in the GUI thread on Windows</param>
        /// <param name="a_intptrHwnd">window handle</param>
        public TWAIN
        (
            string a_szManufacturer,
            string a_szProductFamily,
            string a_szProductName,
            ushort a_u16ProtocolMajor,
            ushort a_u16ProtocolMinor,
            uint a_u32SupportedGroups,
            TWCY a_twcy,
            string a_szInfo,
            TWLG a_twlg,
            ushort a_u16MajorNum,
            ushort a_u16MinorNum,
            bool a_blUseLegacyDSM,
            bool a_blUseCallbacks,
            DeviceEventCallback a_deviceeventback,
            ScanCallback a_scancallback,
            RunInUiThreadDelegate a_runinuithreaddelegate,
            IntPtr a_intptrHwnd
        )
        {
            TW_IDENTITY twidentity;

            // Since we're using P/Invoke in this sample, the DLL
            // is implicitly loaded as we access it, so we can
            // never go lower than state 2...
            m_state = STATE.S2;

            // Register the caller's info...
            twidentity = default;
            twidentity.Manufacturer.Set(a_szManufacturer);
            twidentity.ProductFamily.Set(a_szProductFamily);
            twidentity.ProductName.Set(a_szProductName);
            twidentity.ProtocolMajor = a_u16ProtocolMajor;
            twidentity.ProtocolMinor = a_u16ProtocolMinor;
            twidentity.SupportedGroups = a_u32SupportedGroups;
            twidentity.Version.Country = a_twcy;
            twidentity.Version.Info.Set(a_szInfo);
            twidentity.Version.Language = a_twlg;
            twidentity.Version.MajorNum = a_u16MajorNum;
            twidentity.Version.MinorNum = a_u16MinorNum;
            m_twidentityApp = twidentity;
            m_twidentitylegacyApp = TwidentityToTwidentitylegacy(twidentity);
            m_twidentitymacosxApp = TwidentityToTwidentitymacosx(twidentity);
            m_deviceeventcallback = a_deviceeventback;
            m_scancallback = a_scancallback;
            m_runinuithreaddelegate = a_runinuithreaddelegate;
            m_intptrHwnd = a_intptrHwnd;

            // Help for RunInUiThread...
            m_threaddataDatAudiofilexfer = default;
            m_threaddataDatAudionativexfer = default;
            m_threaddataDatCapability = default;
            m_threaddataDatEvent = default;
            m_threaddataDatExtimageinfo = default;
            m_threaddataDatIdentity = default;
            m_threaddataDatImagefilexfer = default;
            m_threaddataDatImageinfo = default;
            m_threaddataDatImagelayout = default;
            m_threaddataDatImagememfilexfer = default;
            m_threaddataDatImagememxfer = default;
            m_threaddataDatImagenativexfer = default;
            m_threaddataDatParent = default;
            m_threaddataDatPendingxfers = default;
            m_threaddataDatSetupfilexfer = default;
            m_threaddataDatSetupmemxfer = default;
            m_threaddataDatStatus = default;
            m_threaddataDatUserinterface = default;

            // We always go through a discovery process, even on 32-bit...
            m_linuxdsm = LinuxDsm.Unknown;

            // Placeholder for our DS identity...
            m_twidentityDs = default;
            m_twidentitylegacyDs = default;
            m_twidentitymacosxDs = default;

            // We'll normally do an automatic get of DAT.STATUS, but if we'd
            // like to turn it off, this is the variable to hit...
            m_blAutoDatStatus = true;

            // Our helper functions from the DSM...
            m_twentrypointdelegates = default;

            // Our events...
            m_autoreseteventCaller = new AutoResetEvent(false);
            m_autoreseteventThread = new AutoResetEvent(false);
            m_autoreseteventRollback = new AutoResetEvent(false);
            m_autoreseteventThreadStarted = new AutoResetEvent(false);
            m_lockTwain = new Object();

            //ms_platform = PlatformTools.Platform;
            
            // Windows only...
            if (PlatformInfo.IsWindows)
            {
                m_blUseLegacyDSM = a_blUseLegacyDSM;
                m_blUseCallbacks = a_blUseCallbacks;
                m_windowsdsmentrycontrolcallbackdelegate = WindowsDsmEntryCallbackProxy;
            }

            // Linux only...
            else if (PlatformInfo.IsLinux)
            {
                // The user can't set these value, we have to decide automatically
                // which DSM to use, and only callbacks are supported...
                m_blUseLegacyDSM = false;
                m_blUseCallbacks = true;
                m_linuxdsmentrycontrolcallbackdelegate = LinuxDsmEntryCallbackProxy;

                // We assume the new DSM for 32-bit systems...
                if (!PlatformInfo.IsApp64Bit)
                {
                    m_blFound020302Dsm64bit = false;
                    if (File.Exists("/usr/local/lib64/libtwaindsm.so"))
                    {
                        m_blFoundLatestDsm64 = true;
                    }
                    if (File.Exists("/usr/local/lib/libtwaindsm.so"))
                    {
                        m_blFoundLatestDsm = true;
                    }
                }

                // Check for the old DSM, but only on 64-bit systems...
                if ((PlatformInfo.IsApp64Bit) && File.Exists("/usr/local/lib/libtwaindsm.so.2.3.2"))
                {
                    m_blFound020302Dsm64bit = true;
                }

                // Check for any newer DSM, but only on 64-bit systems...
                if ((PlatformInfo.IsApp64Bit) && (File.Exists("/usr/local/lib/libtwaindsm.so") || File.Exists("/usr/local/lib64/libtwaindsm.so")))
                {
                    bool blCheckForNewDsm = true;

                    // Get the DSMs by their fully decorated names...
                    string[] aszDsm = Directory.GetFiles("/usr/local/lib64", "libtwaindsm.so.*.*.*");
                    if (aszDsm.Length == 0)
                    {
                        aszDsm = Directory.GetFiles("/usr/local/lib", "libtwaindsm.so.*.*.*");
                    }
                    if ((aszDsm != null) && (aszDsm.Length > 0))
                    {
                        // Check each name, we only want to launch the process if
                        // we find an old DSM...
                        foreach (string szDsm in aszDsm)
                        {
                            if (szDsm.EndsWith("so.2.0") || szDsm.Contains(".so.2.0.")
                                || szDsm.EndsWith("so.2.1") || szDsm.Contains(".so.2.1.")
                                || szDsm.EndsWith("so.2.2") || szDsm.Contains(".so.2.2.")
                                || szDsm.EndsWith("so.2.3") || szDsm.Contains(".so.2.3."))
                            {
                                // If we get a match, see if the symbolic link is
                                // pointing to old junk...
                                Process p = new Process();
                                p.StartInfo.UseShellExecute = false;
                                p.StartInfo.RedirectStandardOutput = true;
                                p.StartInfo.FileName = "readlink";
                                p.StartInfo.Arguments = "-e /usr/local/lib/libtwaindsm.so";
                                p.Start();
                                string szOutput = p.StandardOutput.ReadToEnd();
                                p.WaitForExit();
                                p.Dispose();
                                // We never did any 1.x stuff...
                                if ((szOutput != null)
                                    && (szOutput.EndsWith(".so.2.0") || szOutput.Contains(".so.2.0.")
                                    || szOutput.EndsWith(".so.2.1") || szOutput.Contains(".so.2.1.")
                                    || szOutput.EndsWith(".so.2.2") || szOutput.Contains(".so.2.2.")
                                    || szOutput.EndsWith(".so.2.3") || szOutput.Contains(".so.2.3.")))
                                {
                                    // libtwaindsm.so is pointing to an old DSM...
                                    blCheckForNewDsm = false;
                                }
                                break;
                            }
                        }
                    }

                    // Is the symbolic link pointing to a new DSM?
                    if (blCheckForNewDsm && (aszDsm != null) && (aszDsm.Length > 0))
                    {
                        foreach (string szDsm in aszDsm)
                        {
                            // I guess this is reasonably future-proof...
                            if (szDsm.Contains("so.2.4")
                                || szDsm.Contains("so.2.5")
                                || szDsm.Contains("so.2.6")
                                || szDsm.Contains("so.2.7")
                                || szDsm.Contains("so.2.8")
                                || szDsm.Contains("so.2.9")
                                || szDsm.Contains("so.2.10")
                                || szDsm.Contains("so.2.11")
                                || szDsm.Contains("so.2.12")
                                || szDsm.Contains("so.2.13")
                                || szDsm.Contains("so.2.14")
                                || szDsm.Contains("so.3")
                                || szDsm.Contains("so.4"))
                            {
                                // libtwaindsm.so is pointing to a new DSM...
                                if (szDsm.Contains("lib64"))
                                {
                                    m_blFoundLatestDsm64 = true;
                                }
                                else
                                {
                                    m_blFoundLatestDsm = true;
                                }
                                break;
                            }
                        }
                    }
                }
            }

            // Mac OS X only...
            else if (PlatformInfo.IsMacOSX)
            {
                m_blUseLegacyDSM = a_blUseLegacyDSM;
                m_blUseCallbacks = true;
                m_macosxdsmentrycontrolcallbackdelegate = MacosxDsmEntryCallbackProxy;
            }

            // Uh-oh, Log will throw an exception for us...
            else
            {
                TWAINWorkingGroup.Log.Assert("Unsupported platform..." + Environment.OSVersion.Platform);
            }

            // Activate our thread...
            /*
            if (m_threadTwain == null)
            {
                m_twaincommand = new TwainCommand();
                m_threadTwain = new Thread(Main);
                m_threadTwain.Start();
                if (!m_autoreseteventThreadStarted.WaitOne(5000))
                {
                    try
                    {
                        m_threadTwain.Abort();
                        m_threadTwain = null;
                    }
                    catch (Exception exception)
                    {
                        // Log will throw an exception for us...
                        TWAINWorkingGroup.Log.Assert("Failed to start the TWAIN background thread - " + exception.Message);
                    }
                }
            }
            */
        }

        /// <summary>
        /// Cleanup...
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Report the path to the DSM we're using...
        /// </summary>
        /// <returns>full path to the DSM</returns>
        public string GetDsmPath()
        {
            string szDsmPath = "";

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                if (m_blUseLegacyDSM)
                {
                    szDsmPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "twain_32.dll");
                }
                else
                {
                    szDsmPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "twaindsm.dll");
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                {
                    szDsmPath = "/usr/local/lib64/libtwaindsm.so";
                }
                else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                {
                    szDsmPath = "/usr/local/lib/libtwaindsm.so";
                }
                else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                {
                    szDsmPath = "/usr/local/lib/libtwaindsm.so.2.3.2";
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                if (m_blUseLegacyDSM)
                {
                    szDsmPath = "/System/Library/Frameworks/TWAIN.framework/TWAIN";
                }
                else
                {
                    szDsmPath = "/Library/Frameworks/TWAIN.framework/TWAIN";
                }
            }

            // If found...
            if (File.Exists(szDsmPath))
            {
                return (szDsmPath);
            }

            // Ruh-roh...
            if (string.IsNullOrEmpty(szDsmPath))
            {
                return ("(could not identify a DSM candidate for this platform - '" + Environment.OSVersion.Platform + "')");
            }

            // Hmmm...
            return ("(could not find '" + szDsmPath + "')");
        }

        /// <summary>
        /// Get the identity of the current application...
        /// </summary>
        /// <returns></returns>
        public string GetAppIdentity()
        {
            return (CsvSerializer.IdentityToCsv(m_twidentityApp));
        }

        /// <summary>
        /// Get the identity of the current driver, if we have one...
        /// </summary>
        /// <returns></returns>
        public string GetDsIdentity()
        {
            if (m_state < STATE.S4)
            {
                return (CsvSerializer.IdentityToCsv(default));
            }
            return (CsvSerializer.IdentityToCsv(m_twidentityDs));
        }

        /// <summary>
        /// Alloc memory used with the data source.
        /// </summary>
        /// <param name="a_u32Size">Number of bytes to allocate</param>
        /// <param name="a_blForcePointer"></param>
        /// <returns>Point to memory</returns>
        public IntPtr DsmMemAlloc(uint a_u32Size, bool a_blForcePointer = false)
        {
            IntPtr intptr;

            // Use the DSM...
            if ((m_twentrypointdelegates.DSM_MemAllocate != null) && !a_blForcePointer)
            {
                intptr = m_twentrypointdelegates.DSM_MemAllocate(a_u32Size);
                if (intptr == IntPtr.Zero)
                {
                    TWAINWorkingGroup.Log.Error("DSM_MemAllocate failed...");
                }
                return (intptr);
            }

            // Do it ourselves, Windows...
            if (PlatformInfo.IsWindows)
            {
                intptr = (IntPtr)NativeMethods.GlobalAlloc((uint)(a_blForcePointer ? 0x0040 /* GPTR */ : 0x0042 /* GHND */), (UIntPtr)a_u32Size);
                if (intptr == IntPtr.Zero)
                {
                    TWAINWorkingGroup.Log.Error("GlobalAlloc failed...");
                }
                return (intptr);
            }

            // Do it ourselves, Linux...
            if (PlatformInfo.IsLinux)
            {
                intptr = Marshal.AllocHGlobal((int)a_u32Size);
                if (intptr == IntPtr.Zero)
                {
                    TWAINWorkingGroup.Log.Error("AllocHGlobal failed...");
                }
                return (intptr);
            }

            // Do it ourselves, Mac OS X...
            if (PlatformInfo.IsMacOSX)
            {
                IntPtr intptrIndirect = Marshal.AllocHGlobal((int)a_u32Size);
                if (intptrIndirect == IntPtr.Zero)
                {
                    TWAINWorkingGroup.Log.Error("AllocHGlobal(indirect) failed...");
                    return (intptrIndirect);
                }
                IntPtr intptrDirect = Marshal.AllocHGlobal(Marshal.SizeOf(intptrIndirect));
                if (intptrDirect == IntPtr.Zero)
                {
                    TWAINWorkingGroup.Log.Error("AllocHGlobal(direct) failed...");
                    return (intptrDirect);
                }
                Marshal.StructureToPtr(intptrIndirect, intptrDirect, true);
                return (intptrDirect);
            }

            // Trouble, Log will throw an exception for us...
            TWAINWorkingGroup.Log.Assert("Unsupported platform..." + Environment.OSVersion.Platform);
            return (IntPtr.Zero);
        }

        /// <summary>
        /// Free memory used with the data source...
        /// </summary>
        /// <param name="a_intptrHandle">Pointer to free</param>
        /// <param name="a_blForcePointer"></param>
        public void DsmMemFree(ref IntPtr a_intptrHandle, bool a_blForcePointer = false)
        {
            // Validate...
            if (a_intptrHandle == IntPtr.Zero)
            {
                return;
            }

            // Use the DSM...
            if ((m_twentrypointdelegates.DSM_MemAllocate != null) && !a_blForcePointer)
            {
                m_twentrypointdelegates.DSM_MemFree(a_intptrHandle);
            }

            // Do it ourselves, Windows...
            else if (PlatformInfo.IsWindows)
            {
                NativeMethods.GlobalFree(a_intptrHandle);
            }

            // Do it ourselves, Linux...
            else if (PlatformInfo.IsLinux)
            {
                Marshal.FreeHGlobal(a_intptrHandle);
            }

            // Do it ourselves, Mac OS X...
            else if (PlatformInfo.IsMacOSX)
            {
                // Free the indirect pointer...
                IntPtr intptr = (IntPtr)Marshal.PtrToStructure(a_intptrHandle, typeof(IntPtr));
                if (intptr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(intptr);
                }

                // If we free the direct pointer the CLR tells us that we're
                // freeing something that was never allocated.  We're going
                // to believe it and not do a free.  But I'm also leaving this
                // here as a record of the decision...
                //Marshal.FreeHGlobal(a_twcapability.hContainer);
            }

            // Make sure the variable is cleared...
            a_intptrHandle = IntPtr.Zero;
        }

        /// <summary>
        /// Lock memory used with the data source...
        /// </summary>
        /// <param name="a_intptrHandle">Handle to lock</param>
        /// <returns>Locked pointer</returns>
        public IntPtr DsmMemLock(IntPtr a_intptrHandle)
        {
            // Validate...
            if (a_intptrHandle == IntPtr.Zero)
            {
                return (a_intptrHandle);
            }

            // Use the DSM...
            if (m_twentrypointdelegates.DSM_MemLock != null)
            {
                return (m_twentrypointdelegates.DSM_MemLock(a_intptrHandle));
            }

            // Do it ourselves, Windows...
            if (PlatformInfo.IsWindows)
            {
                return (NativeMethods.GlobalLock(a_intptrHandle));
            }

            // Do it ourselves, Linux...
            if (PlatformInfo.IsLinux)
            {
                return (a_intptrHandle);
            }

            // Do it ourselves, Mac OS X...
            if (PlatformInfo.IsMacOSX)
            {
                IntPtr intptr = (IntPtr)Marshal.PtrToStructure(a_intptrHandle, typeof(IntPtr));
                return (intptr);
            }

            // Trouble, Log will throw an exception for us...
            TWAINWorkingGroup.Log.Assert("Unsupported platform..." + Environment.OSVersion.Platform);
            return (IntPtr.Zero);
        }

        /// <summary>
        /// Unlock memory used with the data source...
        /// </summary>
        /// <param name="a_intptrHandle">Handle to unlock</param>
        public void DsmMemUnlock(IntPtr a_intptrHandle)
        {
            // Validate...
            if (a_intptrHandle == IntPtr.Zero)
            {
                return;
            }

            // Use the DSM...
            if (m_twentrypointdelegates.DSM_MemUnlock != null)
            {
                m_twentrypointdelegates.DSM_MemUnlock(a_intptrHandle);
                return;
            }

            // Do it ourselves, Windows...
            if (PlatformInfo.IsWindows)
            {
                NativeMethods.GlobalUnlock(a_intptrHandle);
                return;
            }

            // Do it ourselves, Linux...
            if (PlatformInfo.IsLinux)
            {
                return;
            }

            // Do it ourselves, Mac OS X...
            if (PlatformInfo.IsMacOSX)
            {
                return;
            }

            // Trouble, Log will throw an exception for us...
            TWAINWorkingGroup.Log.Assert("Unsupported platform..." + Environment.OSVersion.Platform);
        }

        /// <summary>
        /// Report the current TWAIN state as we understand it...
        /// </summary>
        /// <returns>The current TWAIN state for the application</returns>
        public STATE GetState()
        {
            return (m_state);
        }

        /// <summary>
        /// True if the DSM has the DSM2 flag set...
        /// </summary>
        /// <returns>True if we detect the TWAIN Working Group open source DSM</returns>
        public bool IsDsm2()
        {
            // Windows...
            if (PlatformInfo.IsWindows)
            {
                return ((m_twidentitylegacyApp.SupportedGroups & (uint)DG.DSM2) != 0);
            }

            // Linux...
            if (PlatformInfo.IsLinux)
            {
                return ((m_twidentitylegacyApp.SupportedGroups & (uint)DG.DSM2) != 0);
            }

            // Mac OS X...
            if (PlatformInfo.IsMacOSX)
            {
                return ((m_twidentitymacosxApp.SupportedGroups & (uint)DG.DSM2) != 0);
            }

            // Trouble, Log will throw an exception for us...
            TWAINWorkingGroup.Log.Assert("Unsupported platform..." + Environment.OSVersion.Platform);
            return (false);
        }

        /// <summary>
        /// Have we seen the first image since MSG.ENABLEDS?
        /// </summary>
        /// <returns>True if the driver is ready to transfer images</returns>
        public bool IsMsgXferReady()
        {
            return (m_blIsMsgxferready);
        }

        /// <summary>
        /// Has the cancel button been pressed since the last MSG.ENABLEDS?
        /// </summary>
        /// <returns>True if the cancel button was pressed</returns>
        public bool IsMsgCloseDsReq()
        {
            return (m_blIsMsgclosedsreq);
        }

        /// <summary>
        /// Has the OK button been pressed since the last MSG.ENABLEDS?
        /// </summary>
        /// <returns>True if the OK button was pressed</returns>
        public bool IsMsgCloseDsOk()
        {
            return (m_blIsMsgclosedsok);
        }

        /// <summary>
        /// Monitor for DAT.NULL / MSG.* stuff...
        /// </summary>
        /// <param name="a_intptrHwnd">Window handle that we're monitoring</param>
        /// <param name="a_iMsg">A message</param>
        /// <param name="a_intptrWparam">a parameter for the message</param>
        /// <param name="a_intptrLparam">another parameter for the message</param>
        /// <returns></returns>
        public bool PreFilterMessage
        (
            IntPtr a_intptrHwnd,
            int a_iMsg,
            IntPtr a_intptrWparam,
            IntPtr a_intptrLparam
        )
        {
            STS sts;
            MESSAGE msg;

            // This is only in effect after MSG.ENABLEDS*, or if we are in
            // the middle of processing DAT_USERINTERFACE.  We don't want to
            // bump to state 5 before processing DAT_USERINTERFACE, but some
            // drivers are really eager to get going, and throw MSG_XFERREADY
            // before even get a chance to finish the command...
            if ((m_state < STATE.S5) && !m_blRunningDatUserinterface)
            {
                return (false);
            }

            // Convert the data...
            msg = new MESSAGE();
            msg.hwnd = a_intptrHwnd;
            msg.message = (uint)a_iMsg;
            msg.wParam = a_intptrWparam;
            msg.lParam = a_intptrLparam;

            // Allocate memory that we can give to the driver...
            if (m_tweventPreFilterMessage.pEvent == IntPtr.Zero)
            {
                m_tweventPreFilterMessage.pEvent = Marshal.AllocHGlobal(Marshal.SizeOf(msg) + 65536);
            }
            Marshal.StructureToPtr(msg, m_tweventPreFilterMessage.pEvent, true);

            // See if the driver wants the event...
            m_tweventPreFilterMessage.TWMessage = 0;
            sts = DatEvent(DG.CONTROL, MSG.PROCESSEVENT, ref m_tweventPreFilterMessage, true);
            if ((sts != STS.DSEVENT) && (sts != STS.NOTDSEVENT))
            {
                return (false);
            }

            // All done, tell the app we consumed the event if we
            // got back a status telling us that...
            return (sts == STS.DSEVENT);
        }

        static int s_iCloseDsmDelay = 0;
        /// <summary>
        /// Rollback the TWAIN state machine to the specified value, with an
        /// automatic resync if it detects a sequence error...
        /// </summary>
        /// <param name="a_stateTarget">The TWAIN state that we want to end up at</param>
        public STATE Rollback(STATE a_stateTarget)
        {
            int iRetry;
            STS sts;
            STATE stateStart;

            // Nothing to do here...
            if (m_state <= STATE.S2)
            {
                return (m_state);
            }

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate != null)
            {
                lock (m_lockTwain)
                {
                    // No point in continuing...
                    if (m_twaincommand == null)
                    {
                        return (m_state);
                    }

                    // Set the command variables...
                    ThreadData threaddata = default;
                    threaddata.blExitThread = true;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply, the delay
                    // is needed because Mac OS X doesn't gracefully handle
                    // the loss of a mutex...
                    s_iCloseDsmDelay = 0;
                    CallerToThreadSet();
                    ThreadToRollbackWaitOne();
                    Thread.Sleep(s_iCloseDsmDelay);

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
            }
            else if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // No point in continuing...
                    if (m_twaincommand == null)
                    {
                        return (m_state);
                    }

                    // Set the command variables...
                    ThreadData threaddata = default;
                    threaddata.stateRollback = a_stateTarget;
                    threaddata.blRollback = true;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply, the delay
                    // is needed because Mac OS X doesn't gracefully handle
                    // the loss of a mutex...
                    s_iCloseDsmDelay = 0;
                    CallerToThreadSet();
                    ThreadToRollbackWaitOne();
                    Thread.Sleep(s_iCloseDsmDelay);

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (m_state);
            }

            // If we get a sequence error, then we'll repeat the loop from
            // the highest possible state to see if we can fix the problem...
            iRetry = 2;
            stateStart = GetState();
            while (iRetry-- > 0)
            {
                // State 7 --> State 6...
                if ((stateStart >= STATE.S7) && (a_stateTarget < STATE.S7))
                {
                    TW_PENDINGXFERS twpendingxfers = default;
                    sts = DatPendingxfers(DG.CONTROL, MSG.ENDXFER, ref twpendingxfers);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S6;
                }

                // State 6 --> State 5...
                if ((stateStart >= STATE.S6) && (a_stateTarget < STATE.S6))
                {
                    TW_PENDINGXFERS twpendingxfers = default;
                    sts = DatPendingxfers(DG.CONTROL, MSG.RESET, ref twpendingxfers);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S5;
                }

                // State 5 --> State 4...
                if ((stateStart >= STATE.S5) && (a_stateTarget < STATE.S5))
                {
                    TW_USERINTERFACE twuserinterface = default;
                    sts = DatUserinterface(DG.CONTROL, MSG.DISABLEDS, ref twuserinterface);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    if (m_tweventPreFilterMessage.pEvent != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(m_tweventPreFilterMessage.pEvent);
                        m_tweventPreFilterMessage.pEvent = IntPtr.Zero;
                    }
                    stateStart = STATE.S4;
                    m_blAcceptXferReady = false;
                    m_blIsMsgclosedsok = false;
                    m_blIsMsgclosedsreq = false;
                    m_blIsMsgxferready = false;
                }

                // State 4 --> State 3...
                if ((stateStart >= STATE.S4) && (a_stateTarget < STATE.S4))
                {
                    sts = DatIdentity(DG.CONTROL, MSG.CLOSEDS, ref m_twidentityDs);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S3;
                }

                // State 3 --> State 2...
                if ((stateStart >= STATE.S3) && (a_stateTarget < STATE.S3))
                {
                    // Do this to prevent a deadlock on Mac OS X, two seconds
                    // better be enough to finish up...
                    if (PlatformInfo.IsMacOSX)
                    {
                        ThreadToRollbackSet();
                        s_iCloseDsmDelay = 2000;
                    }

                    // Now do the rest of it...
                    sts = DatParent(DG.CONTROL, MSG.CLOSEDSM, ref m_intptrHwnd);
                    if (sts == STS.SEQERROR)
                    {
                        stateStart = STATE.S7;
                        continue;
                    }
                    stateStart = STATE.S2;
                }

                // All done...
                break;
            }

            // How did we do?
            return (m_state);
        }

        /// <summary>
        /// Send a command to the currently loaded DSM
        /// using tokenized command and anything needed.
        /// </summary>
        /// <param name="a_szDat"></param>
        /// <param name="a_szDg"></param>
        /// <param name="a_szMsg"></param>
        /// <param name="a_szTwmemref"></param>"
        /// <param name="a_szResult"></param>
        /// <returns>true to quit</returns>
        public STS Send(string a_szDg, string a_szDat, string a_szMsg, ref string a_szTwmemref, ref string a_szResult)
        {
            int iDg;
            int iDat;
            int iMsg;
            STS sts;
            DG dg = DG.MASK;
            DAT dat = DAT.NULL;
            MSG msg = MSG.NULL;

            // Init stuff...
            iDg = 0;
            iDat = 0;
            iMsg = 0;
            sts = STS.BADPROTOCOL;
            a_szResult = "";

            // Look for DG...
            if (!a_szDg.ToLowerInvariant().StartsWith("dg_"))
            {
                TWAINWorkingGroup.Log.Error("Unrecognized dg - <" + a_szDg + ">");
                return (STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szDg.ToLowerInvariant().StartsWith("dg_0x"))
                {
                    if (!int.TryParse(a_szDg.ToLowerInvariant().Substring(3), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iDg))
                    {
                        TWAINWorkingGroup.Log.Error("Badly constructed dg - <" + a_szDg + ">");
                        return (STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szDg.ToUpperInvariant().Substring(3), out dg))
                    {
                        TWAINWorkingGroup.Log.Error("Unrecognized dg - <" + a_szDg + ">");
                        return (STS.BADPROTOCOL);
                    }
                    iDg = (int)dg;
                }
            }

            // Look for DAT...
            if (!a_szDat.ToLowerInvariant().StartsWith("dat_"))
            {
                TWAINWorkingGroup.Log.Error("Unrecognized dat - <" + a_szDat + ">");
                return (STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szDat.ToLowerInvariant().StartsWith("dat_0x"))
                {
                    if (!int.TryParse(a_szDat.ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iDat))
                    {
                        TWAINWorkingGroup.Log.Error("Badly constructed dat - <" + a_szDat + ">");
                        return (STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szDat.ToUpperInvariant().Substring(4), out dat))
                    {
                        TWAINWorkingGroup.Log.Error("Unrecognized dat - <" + a_szDat + ">");
                        return (STS.BADPROTOCOL);
                    }
                    iDat = (int)dat;
                }
            }

            // Look for MSG...
            if (!a_szMsg.ToLowerInvariant().StartsWith("msg_"))
            {
                TWAINWorkingGroup.Log.Error("Unrecognized msg - <" + a_szMsg + ">");
                return (STS.BADPROTOCOL);
            }
            else
            {
                // Look for hex number (take anything)...
                if (a_szMsg.ToLowerInvariant().StartsWith("msg_0x"))
                {
                    if (!int.TryParse(a_szMsg.ToLowerInvariant().Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iMsg))
                    {
                        TWAINWorkingGroup.Log.Error("Badly constructed dat - <" + a_szMsg + ">");
                        return (STS.BADPROTOCOL);
                    }
                }
                else
                {
                    if (!Enum.TryParse(a_szMsg.ToUpperInvariant().Substring(4), out msg))
                    {
                        TWAINWorkingGroup.Log.Error("Unrecognized msg - <" + a_szMsg + ">");
                        return (STS.BADPROTOCOL);
                    }
                    iMsg = (int)msg;
                }
            }

            // Send the command...
            switch (iDat)
            {
                // Ruh-roh, since we can't marshal it, we have to return an error,
                // it would be nice to have a solution for this, but that will need
                // a dynamic marshalling system...
                default:
                    sts = STS.BADPROTOCOL;
                    break;

                // DAT_AUDIOFILEXFER...
                case (int)DAT.AUDIOFILEXFER:
                    {
                        sts = DatAudiofilexfer((DG)iDg, (MSG)iMsg);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_AUDIOINFO..
                case (int)DAT.AUDIOINFO:
                    {
                        TW_AUDIOINFO twaudioinfo = default;
                        sts = DatAudioinfo((DG)iDg, (MSG)iMsg, ref twaudioinfo);
                        a_szTwmemref = CsvSerializer.AudioinfoToCsv(twaudioinfo);
                    }
                    break;

                // DAT_AUDIONATIVEXFER..
                case (int)DAT.AUDIONATIVEXFER:
                    {
                        IntPtr intptr = IntPtr.Zero;
                        sts = DatAudionativexfer((DG)iDg, (MSG)iMsg, ref intptr);
                        a_szTwmemref = intptr.ToString();
                    }
                    break;

                // DAT_CALLBACK...
                case (int)DAT.CALLBACK:
                    {
                        TW_CALLBACK twcallback = default;
                        CsvSerializer.CsvToCallback(ref twcallback, a_szTwmemref);
                        sts = DatCallback((DG)iDg, (MSG)iMsg, ref twcallback);
                        a_szTwmemref = CsvSerializer.CallbackToCsv(twcallback);
                    }
                    break;

                // DAT_CALLBACK2...
                case (int)DAT.CALLBACK2:
                    {
                        TW_CALLBACK2 twcallback2 = default;
                        CsvSerializer.CsvToCallback2(ref twcallback2, a_szTwmemref);
                        sts = DatCallback2((DG)iDg, (MSG)iMsg, ref twcallback2);
                        a_szTwmemref = CsvSerializer.Callback2ToCsv(twcallback2);
                    }
                    break;

                // DAT_CAPABILITY...
                case (int)DAT.CAPABILITY:
                    {
                        // Skip symbols for msg_querysupport, otherwise 0 gets turned into false, also
                        // if the command fails the return value is whatever was sent into us, which
                        // matches the experience one should get with C/C++...
                        string szStatus = "";
                        TW_CAPABILITY twcapability = default;
                        CsvToCapability(ref twcapability, ref szStatus, a_szTwmemref);
                        sts = DatCapability((DG)iDg, (MSG)iMsg, ref twcapability);
                        if ((sts == STS.SUCCESS) || (sts == STS.CHECKSTATUS))
                        {
                            // Convert the data to CSV...
                            a_szTwmemref = CapabilityToCsv(twcapability, ((MSG)iMsg != MSG.QUERYSUPPORT));
                            // Free the handle if the driver created it...
                            switch ((MSG)iMsg)
                            {
                                default: break;
                                case MSG.GET:
                                case MSG.GETCURRENT:
                                case MSG.GETDEFAULT:
                                case MSG.QUERYSUPPORT:
                                case MSG.RESET:
                                    DsmMemFree(ref twcapability.hContainer);
                                    break;
                            }
                        }
                    }
                    break;

                // DAT_CIECOLOR..
                case (int)DAT.CIECOLOR:
                    {
                        //TW_CIECOLOR twciecolor = default(TW_CIECOLOR);
                        //sts = m_DATCiecolor((DG)iDg, (MSG)iMsg, ref twciecolor);
                        //a_szTwmemref = m_twain.CiecolorToCsv(twciecolor);
                    }
                    break;

                // DAT_CUSTOMDSDATA...
                case (int)DAT.CUSTOMDSDATA:
                    {
                        TW_CUSTOMDSDATA twcustomdsdata = default;
                        CsvToCustomdsdata(ref twcustomdsdata, a_szTwmemref);
                        sts = DatCustomdsdata((DG)iDg, (MSG)iMsg, ref twcustomdsdata);
                        a_szTwmemref = CustomdsdataToCsv(twcustomdsdata);
                    }
                    break;

                // DAT_DEVICEEVENT...
                case (int)DAT.DEVICEEVENT:
                    {
                        TW_DEVICEEVENT twdeviceevent = default;
                        sts = DatDeviceevent((DG)iDg, (MSG)iMsg, ref twdeviceevent);
                        a_szTwmemref = CsvSerializer.DeviceeventToCsv(twdeviceevent);
                    }
                    break;

                // DAT_ENTRYPOINT...
                case (int)DAT.ENTRYPOINT:
                    {
                        TW_ENTRYPOINT twentrypoint = default;
                        twentrypoint.Size = (uint)Marshal.SizeOf(twentrypoint);
                        sts = DatEntrypoint((DG)iDg, (MSG)iMsg, ref twentrypoint);
                        a_szTwmemref = CsvSerializer.EntrypointToCsv(twentrypoint);
                    }
                    break;

                // DAT_EVENT...
                case (int)DAT.EVENT:
                    {
                        TW_EVENT twevent = default;
                        sts = DatEvent((DG)iDg, (MSG)iMsg, ref twevent);
                        a_szTwmemref = CsvSerializer.EventToCsv(twevent);
                    }
                    break;

                // DAT_EXTIMAGEINFO...
                case (int)DAT.EXTIMAGEINFO:
                    {
                        TW_EXTIMAGEINFO twextimageinfo = default;
                        CsvSerializer.CsvToExtimageinfo(ref twextimageinfo, a_szTwmemref);
                        sts = DatExtimageinfo((DG)iDg, (MSG)iMsg, ref twextimageinfo);
                        a_szTwmemref = CsvSerializer.ExtimageinfoToCsv(twextimageinfo);
                    }
                    break;

                // DAT_FILESYSTEM...
                case (int)DAT.FILESYSTEM:
                    {
                        TW_FILESYSTEM twfilesystem = default;
                        CsvSerializer.CsvToFilesystem(ref twfilesystem, a_szTwmemref);
                        sts = DatFilesystem((DG)iDg, (MSG)iMsg, ref twfilesystem);
                        a_szTwmemref = CsvSerializer.FilesystemToCsv(twfilesystem);
                    }
                    break;

                // DAT_FILTER...
                case (int)DAT.FILTER:
                    {
                        //TW_FILTER twfilter = default(TW_FILTER);
                        //m_twain.CsvToFilter(ref twfilter, a_szTwmemref);
                        //sts = m_DATFilter((DG)iDg, (MSG)iMsg, ref twfilter);
                        //a_szTwmemref = m_twain.FilterToCsv(twfilter);
                    }
                    break;

                // DAT_GRAYRESPONSE...
                case (int)DAT.GRAYRESPONSE:
                    {
                        //TW_GRAYRESPONSE twgrayresponse = default(TW_GRAYRESPONSE);
                        //m_twain.CsvToGrayresponse(ref twgrayresponse, a_szTwmemref);
                        //sts = m_DATGrayresponse((DG)iDg, (MSG)iMsg, ref twgrayresponse);
                        //a_szTwmemref = m_twain.GrayresponseToCsv(twgrayresponse);
                    }
                    break;

                // DAT_ICCPROFILE...
                case (int)DAT.ICCPROFILE:
                    {
                        TW_MEMORY twmemory = default;
                        sts = DatIccprofile((DG)iDg, (MSG)iMsg, ref twmemory);
                        a_szTwmemref = CsvSerializer.IccprofileToCsv(twmemory);
                    }
                    break;

                // DAT_IDENTITY...
                case (int)DAT.IDENTITY:
                    {
                        TW_IDENTITY twidentity = default;
                        switch (iMsg)
                        {
                            default:
                                break;
                            case (int)MSG.SET:
                            case (int)MSG.OPENDS:
                                CsvSerializer.CsvToIdentity(ref twidentity, a_szTwmemref);
                                break;
                        }
                        sts = DatIdentity((DG)iDg, (MSG)iMsg, ref twidentity);
                        a_szTwmemref = CsvSerializer.IdentityToCsv(twidentity);
                    }
                    break;

                // DAT_IMAGEFILEXFER...
                case (int)DAT.IMAGEFILEXFER:
                    {
                        sts = DatImagefilexfer((DG)iDg, (MSG)iMsg);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_IMAGEINFO...
                case (int)DAT.IMAGEINFO:
                    {
                        TW_IMAGEINFO twimageinfo = default;
                        CsvSerializer.CsvToImageinfo(ref twimageinfo, a_szTwmemref);
                        sts = DatImageinfo((DG)iDg, (MSG)iMsg, ref twimageinfo);
                        a_szTwmemref = CsvSerializer.ImageinfoToCsv(twimageinfo);
                    }
                    break;

                // DAT_IMAGELAYOUT...
                case (int)DAT.IMAGELAYOUT:
                    {
                        TW_IMAGELAYOUT twimagelayout = default;
                        CsvSerializer.CsvToImagelayout(ref twimagelayout, a_szTwmemref);
                        sts = DatImagelayout((DG)iDg, (MSG)iMsg, ref twimagelayout);
                        a_szTwmemref = CsvSerializer.ImagelayoutToCsv(twimagelayout);
                    }
                    break;

                // DAT_IMAGEMEMFILEXFER...
                case (int)DAT.IMAGEMEMFILEXFER:
                    {
                        TW_IMAGEMEMXFER twimagememxfer = default;
                        CsvSerializer.CsvToImagememxfer(ref twimagememxfer, a_szTwmemref);
                        sts = DatImagememfilexfer((DG)iDg, (MSG)iMsg, ref twimagememxfer);
                        a_szTwmemref = CsvSerializer.ImagememxferToCsv(twimagememxfer);
                    }
                    break;

                // DAT_IMAGEMEMXFER...
                case (int)DAT.IMAGEMEMXFER:
                    {
                        TW_IMAGEMEMXFER twimagememxfer = default;
                        CsvSerializer.CsvToImagememxfer(ref twimagememxfer, a_szTwmemref);
                        sts = DatImagememxfer((DG)iDg, (MSG)iMsg, ref twimagememxfer);
                        a_szTwmemref = CsvSerializer.ImagememxferToCsv(twimagememxfer);
                    }
                    break;

                // DAT_IMAGENATIVEXFER...
                // TODO: Recode later
                //case (int)DAT.IMAGENATIVEXFER:
                //    {
                //        IntPtr intptrBitmapHandle = IntPtr.Zero;
                //        sts = DatImagenativexferHandle((DG)iDg, (MSG)iMsg, ref intptrBitmapHandle);
                //        a_szTwmemref = intptrBitmapHandle.ToString();
                //    }
                //    break;

                // DAT_JPEGCOMPRESSION...
                case (int)DAT.JPEGCOMPRESSION:
                    {
                        //TW_JPEGCOMPRESSION twjpegcompression = default(TW_JPEGCOMPRESSION);
                        //m_twain.CsvToJpegcompression(ref twjpegcompression, a_szTwmemref);
                        //sts = m_DATJpegcompression((DG)iDg, (MSG)iMsg, ref twjpegcompression);
                        //a_szTwmemref = m_twain.JpegcompressionToCsv(twjpegcompression);
                    }
                    break;

                // DAT_METRICS...
                case (int)DAT.METRICS:
                    {
                        TW_METRICS twmetrics = default;
                        twmetrics.SizeOf = (uint)Marshal.SizeOf(twmetrics);
                        sts = DatMetrics((DG)iDg, (MSG)iMsg, ref twmetrics);
                        a_szTwmemref = CsvSerializer.MetricsToCsv(twmetrics);
                    }
                    break;

                // DAT_PALETTE8...
                case (int)DAT.PALETTE8:
                    {
                        //TW_PALETTE8 twpalette8 = default(TW_PALETTE8);
                        //m_twain.CsvToPalette8(ref twpalette8, a_szTwmemref);
                        //sts = m_DATPalette8((DG)iDg, (MSG)iMsg, ref twpalette8);
                        //a_szTwmemref = m_twain.Palette8ToCsv(twpalette8);
                    }
                    break;

                // DAT_PARENT...
                case (int)DAT.PARENT:
                    {
                        sts = DatParent((DG)iDg, (MSG)iMsg, ref m_intptrHwnd);
                        a_szTwmemref = "";
                    }
                    break;

                // DAT_PASSTHRU...
                case (int)DAT.PASSTHRU:
                    {
                        TW_PASSTHRU twpassthru = default;
                        CsvSerializer.CsvToPassthru(ref twpassthru, a_szTwmemref);
                        sts = DatPassthru((DG)iDg, (MSG)iMsg, ref twpassthru);
                        a_szTwmemref = CsvSerializer.PassthruToCsv(twpassthru);
                    }
                    break;

                // DAT_PENDINGXFERS...
                case (int)DAT.PENDINGXFERS:
                    {
                        TW_PENDINGXFERS twpendingxfers = default;
                        sts = DatPendingxfers((DG)iDg, (MSG)iMsg, ref twpendingxfers);
                        a_szTwmemref = CsvSerializer.PendingxfersToCsv(twpendingxfers);
                    }
                    break;

                // DAT_RGBRESPONSE...
                case (int)DAT.RGBRESPONSE:
                    {
                        //TW_RGBRESPONSE twrgbresponse = default(TW_RGBRESPONSE);
                        //m_twain.CsvToRgbresponse(ref twrgbresponse, a_szTwmemref);
                        //sts = m_DATRgbresponse((DG)iDg, (MSG)iMsg, ref twrgbresponse);
                        //a_szTwmemref = m_twain.RgbresponseToCsv(twrgbresponse);
                    }
                    break;

                // DAT_SETUPFILEXFER...
                case (int)DAT.SETUPFILEXFER:
                    {
                        TW_SETUPFILEXFER twsetupfilexfer = default;
                        CsvSerializer.CsvToSetupfilexfer(ref twsetupfilexfer, a_szTwmemref);
                        sts = DatSetupfilexfer((DG)iDg, (MSG)iMsg, ref twsetupfilexfer);
                        a_szTwmemref = CsvSerializer.SetupfilexferToCsv(twsetupfilexfer);
                    }
                    break;

                // DAT_SETUPMEMXFER...
                case (int)DAT.SETUPMEMXFER:
                    {
                        TW_SETUPMEMXFER twsetupmemxfer = default;
                        sts = DatSetupmemxfer((DG)iDg, (MSG)iMsg, ref twsetupmemxfer);
                        a_szTwmemref = CsvSerializer.SetupmemxferToCsv(twsetupmemxfer);
                    }
                    break;

                // DAT_STATUS...
                case (int)DAT.STATUS:
                    {
                        TW_STATUS twstatus = default;
                        sts = DatStatus((DG)iDg, (MSG)iMsg, ref twstatus);
                        a_szTwmemref = CsvSerializer.StatusToCsv(twstatus);
                    }
                    break;

                // DAT_STATUSUTF8...
                case (int)DAT.STATUSUTF8:
                    {
                        TW_STATUSUTF8 twstatusutf8 = default;
                        sts = DatStatusutf8((DG)iDg, (MSG)iMsg, ref twstatusutf8);
                        a_szTwmemref = Statusutf8ToCsv(twstatusutf8);
                    }
                    break;

                // DAT_TWAINDIRECT...
                case (int)DAT.TWAINDIRECT:
                    {
                        TW_TWAINDIRECT twtwaindirect = default;
                        CsvSerializer.CsvToTwaindirect(ref twtwaindirect, a_szTwmemref);
                        sts = DatTwaindirect((DG)iDg, (MSG)iMsg, ref twtwaindirect);
                        a_szTwmemref = CsvSerializer.TwaindirectToCsv(twtwaindirect);
                    }
                    break;

                // DAT_USERINTERFACE...
                case (int)DAT.USERINTERFACE:
                    {
                        TW_USERINTERFACE twuserinterface = default;
                        CsvToUserinterface(ref twuserinterface, a_szTwmemref);
                        sts = DatUserinterface((DG)iDg, (MSG)iMsg, ref twuserinterface);
                        a_szTwmemref = CsvSerializer.UserinterfaceToCsv(twuserinterface);
                    }
                    break;

                // DAT_XFERGROUP...
                case (int)DAT.XFERGROUP:
                    {
                        uint uXferGroup = 0;
                        sts = DatXferGroup((DG)iDg, (MSG)iMsg, ref uXferGroup);
                        a_szTwmemref = string.Format("0x{0:X}", uXferGroup);
                    }
                    break;
            }

            // All done...
            return (sts);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Public Helper Functions, we're mapping TWAIN structures to strings to
        // make it easier for the application to work with the data.  All of the
        // functions that do that are located here...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Helper Functions...

        /// <summary>
        /// Copy intptr to intptr...
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        public static void MemCpy(IntPtr dest, IntPtr src, int count)
        {
            if (PlatformInfo.IsWindows)
            {
                NativeMethods.CopyMemory(dest, src, (uint)count);
            }
            else
            {
                NativeMethods.memcpy(dest, src, (IntPtr)count);
            }
        }

        /// <summary>
        /// Safely move intptr to intptr...
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="src"></param>
        /// <param name="count"></param>
        public static void MemMove(IntPtr dest, IntPtr src, int count)
        {
            if (PlatformInfo.IsWindows)
            {
                NativeMethods.MoveMemory(dest, src, (uint)count);
            }
            else
            {
                NativeMethods.memmove(dest, src, (IntPtr)count);
            }
        }

        /// <summary>
        /// Write stuff to a file without having to rebuffer it...
        /// </summary>
        /// <param name="a_szFilename"></param>
        /// <param name="a_intptrPtr"></param>
        /// <param name="a_iBytes"></param>
        /// <param name="a_szFinalFilename"></param>
        /// <returns></returns>
        public static int WriteImageFile(string a_szFilename, IntPtr a_intptrPtr, int a_iBytes, out string a_szFinalFilename)
        {
            // Init stuff...
            a_szFinalFilename = "";

            // Try to write our file...
            try
            {
                // If we don't have an extension, try to add one...
                if (!Path.GetFileName(a_szFilename).Contains(".") && (a_iBytes >= 2))
                {
                    byte[] abData = new byte[2];
                    Marshal.Copy(a_intptrPtr, abData, 0, 2);
                    // BMP
                    if ((abData[0] == 0x42) && (abData[1] == 0x4D)) // BM
                    {
                        a_szFilename += ".bmp";
                    }
                    else if ((abData[0] == 0x49) && (abData[1] == 0x49)) // II
                    {
                        a_szFilename += ".tif";
                    }
                    else if ((abData[0] == 0xFF) && (abData[1] == 0xD8))
                    {
                        a_szFilename += ".jpg";
                    }
                    else if ((abData[0] == 0x25) && (abData[1] == 0x50)) // %P
                    {
                        a_szFilename += ".pdf";
                    }
                }

                // For the caller...
                a_szFinalFilename = a_szFilename;

                // Handle Windows...
                if (PlatformInfo.IsWindows)
                {
                    IntPtr intptrFile;
                    IntPtr intptrBytes = (IntPtr)a_iBytes;
                    IntPtr intptrCount = (IntPtr)1;
                    if (NativeMethods._wfopen_s(out intptrFile, a_szFilename, "wb") != 0)
                    {
                        return (-1);
                    }
                    intptrBytes = NativeMethods.fwriteWin(a_intptrPtr, intptrCount, intptrBytes, intptrFile);
                    NativeMethods.fcloseWin(intptrFile);
                    return ((int)intptrBytes);
                }

                // Handle everybody else...
                else
                {
                    IntPtr intptrFile;
                    IntPtr intptrBytes;
                    intptrFile = NativeMethods.fopen(a_szFilename, "w");
                    if (intptrFile == IntPtr.Zero)
                    {
                        return (-1);
                    }
                    intptrBytes = NativeMethods.fwrite(a_intptrPtr, (IntPtr)1, (IntPtr)a_iBytes, intptrFile);
                    NativeMethods.fclose(intptrFile);
                    return ((int)intptrBytes);
                }
            }
            catch (Exception exception)
            {
                Log.Error("Write file failed <" + a_szFilename + "> - " + exception.Message);
                return (-1);
            }
        }

        /// <summary>
        /// Convert the contents of a capability to a string that we can show in
        /// our simple GUI....
        /// </summary>
        /// <param name="a_twcapability">A TWAIN structure</param>
        /// <param name="a_blUseSymbols"></param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public string CapabilityToCsv(TW_CAPABILITY a_twcapability, bool a_blUseSymbols)
        {
            IntPtr intptr;
            IntPtr intptrLocked;
            TWTY ItemType;
            uint NumItems;

            // Handle the container...
            switch (a_twcapability.ConType)
            {
                default:
                    return ("(unrecognized container)");

                case TWON.ARRAY:
                    {
                        uint uu;
                        CSV csvArray;

                        // Mac has a level of indirection and a different structure (ick)...
                        if (PlatformInfo.IsMacOSX)
                        {
                            // Crack the container...
                            TW_ARRAY_MACOSX twarraymacosx = default;
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twarraymacosx = (TW_ARRAY_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_ARRAY_MACOSX));
                            ItemType = (TWTY)twarraymacosx.ItemType;
                            NumItems = twarraymacosx.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twarraymacosx));
                        }
                        else
                        {
                            // Crack the container...
                            TW_ARRAY twarray = default;
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twarray = (TW_ARRAY)Marshal.PtrToStructure(intptrLocked, typeof(TW_ARRAY));
                            ItemType = twarray.ItemType;
                            NumItems = twarray.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twarray));
                        }

                        // Start building the string...
                        csvArray = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                        csvArray.Add(NumItems.ToString());

                        // Tack on the stuff from the ItemList...
                        if (a_blUseSymbols)
                        {
                            string szValue;
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                string szItem = GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu);
                                szValue = CsvSerializer.CvtCapValueToEnum(a_twcapability.Cap, szItem);
                                csvArray.Add(szValue);
                            }
                        }
                        else
                        {
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                csvArray.Add(GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu));
                            }
                        }

                        // All done...
                        DsmMemUnlock(a_twcapability.hContainer);
                        return (csvArray.Get());
                    }

                case TWON.ENUMERATION:
                    {
                        uint uu;
                        CSV csvEnum;

                        // Mac has a level of indirection and a different structure (ick)...
                        if (PlatformInfo.IsMacOSX)
                        {
                            // Crack the container...
                            TW_ENUMERATION_MACOSX twenumerationmacosx = default;
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twenumerationmacosx = (TW_ENUMERATION_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_ENUMERATION_MACOSX));
                            ItemType = (TWTY)twenumerationmacosx.ItemType;
                            NumItems = twenumerationmacosx.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twenumerationmacosx));

                            // Start building the string...
                            csvEnum = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                            csvEnum.Add(NumItems.ToString());
                            csvEnum.Add(twenumerationmacosx.CurrentIndex.ToString());
                            csvEnum.Add(twenumerationmacosx.DefaultIndex.ToString());
                        }
                        // Windows or the 2.4+ Linux DSM...
                        else if ((PlatformInfo.IsWindows) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm)))
                        {
                            // Crack the container...
                            TW_ENUMERATION twenumeration = default;
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twenumeration = (TW_ENUMERATION)Marshal.PtrToStructure(intptrLocked, typeof(TW_ENUMERATION));
                            ItemType = twenumeration.ItemType;
                            NumItems = twenumeration.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twenumeration));

                            // Start building the string...
                            csvEnum = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                            csvEnum.Add(NumItems.ToString());
                            csvEnum.Add(twenumeration.CurrentIndex.ToString());
                            csvEnum.Add(twenumeration.DefaultIndex.ToString());
                        }
                        // The -2.3 Linux DSM...
                        else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                        {
                            // Crack the container...
                            TW_ENUMERATION_LINUX64 twenumerationlinux64 = default;
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twenumerationlinux64 = (TW_ENUMERATION_LINUX64)Marshal.PtrToStructure(intptrLocked, typeof(TW_ENUMERATION_LINUX64));
                            ItemType = twenumerationlinux64.ItemType;
                            NumItems = (uint)twenumerationlinux64.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twenumerationlinux64));

                            // Start building the string...
                            csvEnum = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                            csvEnum.Add(NumItems.ToString());
                            csvEnum.Add(twenumerationlinux64.CurrentIndex.ToString());
                            csvEnum.Add(twenumerationlinux64.DefaultIndex.ToString());
                        }
                        // This shouldn't be possible, but what the hey...
                        else
                        {
                            Log.Error("This is serious, you win a cookie for getting here...");
                            DsmMemUnlock(a_twcapability.hContainer);
                            return ("");
                        }

                        // Tack on the stuff from the ItemList...
                        if (a_blUseSymbols)
                        {
                            string szValue;
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                string szItem = GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu);
                                szValue = CsvSerializer.CvtCapValueToEnum(a_twcapability.Cap, szItem);
                                csvEnum.Add(szValue);
                            }
                        }
                        else
                        {
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                csvEnum.Add(GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu));
                            }
                        }

                        // All done...
                        DsmMemUnlock(a_twcapability.hContainer);
                        return (csvEnum.Get());
                    }

                case TWON.ONEVALUE:
                    {
                        CSV csvOnevalue;

                        // Mac has a level of indirection and a different structure (ick)...
                        if (PlatformInfo.IsMacOSX)
                        {
                            // Crack the container...
                            TW_ONEVALUE_MACOSX twonevaluemacosx = default;
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twonevaluemacosx = (TW_ONEVALUE_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_ONEVALUE_MACOSX));
                            ItemType = (TWTY)twonevaluemacosx.ItemType;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twonevaluemacosx));
                        }
                        else
                        {
                            // Crack the container...
                            TW_ONEVALUE twonevalue = default;
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twonevalue = (TW_ONEVALUE)Marshal.PtrToStructure(intptrLocked, typeof(TW_ONEVALUE));
                            ItemType = (TWTY)twonevalue.ItemType;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twonevalue));
                        }

                        // Start building the string...
                        csvOnevalue = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);

                        // Tack on the stuff from the Item...
                        if (a_blUseSymbols)
                        {
                            string szValue;
                            string szItem = GetIndexedItem(a_twcapability, ItemType, intptr, 0);
                            szValue = CsvSerializer.CvtCapValueToEnum(a_twcapability.Cap, szItem);
                            csvOnevalue.Add(szValue);
                        }
                        else
                        {
                            csvOnevalue.Add(GetIndexedItem(a_twcapability, ItemType, intptr, 0));
                        }

                        // All done...
                        DsmMemUnlock(a_twcapability.hContainer);
                        return (csvOnevalue.Get());
                    }

                case TWON.RANGE:
                    {
                        CSV csvRange;
                        string szTmp;
                        TW_RANGE twrange;
                        TW_RANGE_LINUX64 twrangelinux64;
                        TW_RANGE_MACOSX twrangemacosx;
                        TW_RANGE_FIX32 twrangefix32;
                        TW_RANGE_FIX32_MACOSX twrangefix32macosx;

                        // Mac has a level of indirection and a different structure (ick)...
                        twrange = default;
                        if (PlatformInfo.IsMacOSX)
                        {
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twrangemacosx = (TW_RANGE_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_MACOSX));
                            twrangefix32macosx = (TW_RANGE_FIX32_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_FIX32_MACOSX));
                            twrange.ItemType = (TWTY)twrangemacosx.ItemType;
                            twrange.MinValue = twrangemacosx.MinValue;
                            twrange.MaxValue = twrangemacosx.MaxValue;
                            twrange.StepSize = twrangemacosx.StepSize;
                            twrange.DefaultValue = twrangemacosx.DefaultValue;
                            twrange.CurrentValue = twrangemacosx.CurrentValue;
                            twrangefix32.ItemType = (TWTY)twrangefix32macosx.ItemType;
                            twrangefix32.MinValue = twrangefix32macosx.MinValue;
                            twrangefix32.MaxValue = twrangefix32macosx.MaxValue;
                            twrangefix32.StepSize = twrangefix32macosx.StepSize;
                            twrangefix32.DefaultValue = twrangefix32macosx.DefaultValue;
                            twrangefix32.CurrentValue = twrangefix32macosx.CurrentValue;
                        }
                        // Windows or the 2.4+ Linux DSM...
                        else if ((PlatformInfo.IsWindows) || (m_linuxdsm == LinuxDsm.IsLatestDsm) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm)))
                        {
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twrange = (TW_RANGE)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE));
                            twrangefix32 = (TW_RANGE_FIX32)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_FIX32));
                        }
                        // The -2.3 Linux DSM...
                        else
                        {
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twrangelinux64 = (TW_RANGE_LINUX64)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_LINUX64));
                            twrangefix32macosx = (TW_RANGE_FIX32_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_FIX32_MACOSX));
                            twrange.ItemType = (TWTY)twrangelinux64.ItemType;
                            twrange.MinValue = (uint)twrangelinux64.MinValue;
                            twrange.MaxValue = (uint)twrangelinux64.MaxValue;
                            twrange.StepSize = (uint)twrangelinux64.StepSize;
                            twrange.DefaultValue = (uint)twrangelinux64.DefaultValue;
                            twrange.CurrentValue = (uint)twrangelinux64.CurrentValue;
                            twrangefix32.ItemType = (TWTY)twrangefix32macosx.ItemType;
                            twrangefix32.MinValue = twrangefix32macosx.MinValue;
                            twrangefix32.MaxValue = twrangefix32macosx.MaxValue;
                            twrangefix32.StepSize = twrangefix32macosx.StepSize;
                            twrangefix32.DefaultValue = twrangefix32macosx.DefaultValue;
                            twrangefix32.CurrentValue = twrangefix32macosx.CurrentValue;
                        }

                        // Start the string...
                        csvRange = Common(a_twcapability.Cap, a_twcapability.ConType, twrange.ItemType);

                        // Tack on the data...
                        switch ((TWTY)twrange.ItemType)
                        {
                            default:
                                DsmMemUnlock(a_twcapability.hContainer);
                                return ("(Get Capability: unrecognized data type)");

                            case TWTY.INT8:
                                csvRange.Add(((char)(twrange.MinValue)).ToString());
                                csvRange.Add(((char)(twrange.MaxValue)).ToString());
                                csvRange.Add(((char)(twrange.StepSize)).ToString());
                                csvRange.Add(((char)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((char)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.INT16:
                                csvRange.Add(((short)(twrange.MinValue)).ToString());
                                csvRange.Add(((short)(twrange.MaxValue)).ToString());
                                csvRange.Add(((short)(twrange.StepSize)).ToString());
                                csvRange.Add(((short)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((short)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.INT32:
                                csvRange.Add(((int)(twrange.MinValue)).ToString());
                                csvRange.Add(((int)(twrange.MaxValue)).ToString());
                                csvRange.Add(((int)(twrange.StepSize)).ToString());
                                csvRange.Add(((int)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((int)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.UINT8:
                                csvRange.Add(((byte)(twrange.MinValue)).ToString());
                                csvRange.Add(((byte)(twrange.MaxValue)).ToString());
                                csvRange.Add(((byte)(twrange.StepSize)).ToString());
                                csvRange.Add(((byte)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((byte)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.BOOL:
                            case TWTY.UINT16:
                                csvRange.Add(((ushort)(twrange.MinValue)).ToString());
                                csvRange.Add(((ushort)(twrange.MaxValue)).ToString());
                                csvRange.Add(((ushort)(twrange.StepSize)).ToString());
                                csvRange.Add(((ushort)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((ushort)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.UINT32:
                                csvRange.Add(((uint)(twrange.MinValue)).ToString());
                                csvRange.Add(((uint)(twrange.MaxValue)).ToString());
                                csvRange.Add(((uint)(twrange.StepSize)).ToString());
                                csvRange.Add(((uint)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((uint)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.FIX32:
                                szTmp = ((double)twrangefix32.MinValue.Whole + ((double)twrangefix32.MinValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.MaxValue.Whole + ((double)twrangefix32.MaxValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.StepSize.Whole + ((double)twrangefix32.StepSize.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.DefaultValue.Whole + ((double)twrangefix32.DefaultValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.CurrentValue.Whole + ((double)twrangefix32.CurrentValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());
                        }
                    }
            }
        }

        /// <summary>
        /// Convert the contents of a string into something we can poke into a
        /// TW_CAPABILITY structure...
        /// </summary>
        /// <param name="a_twcapability">A TWAIN structure</param>
        /// <param name="a_szSetting">A CSV string of the TWAIN structure</param>
        /// <param name="a_szValue">The container for this capability</param>
        /// <returns>True if the conversion is successful</returns>
        public bool CsvToCapability(ref TW_CAPABILITY a_twcapability, ref string a_szSetting, string a_szValue)
        {
            int ii = 0;
            TWTY twty = TWTY.BOOL;
            uint u32NumItems = 0;
            IntPtr intptr = IntPtr.Zero;
            string szResult;
            string[] asz;

            // We need some protection for this one...
            try
            {
                // Tokenize our values...
                asz = CSV.Parse(a_szValue);
                if (asz.GetLength(0) < 1)
                {
                    a_szSetting = "Set Capability: (insufficient number of arguments)";
                    return (false);
                }

                // Set the capability from text or hex...
                try
                {
                    // see if it is a custom cap
                    if ((asz[0][0] == '8') || asz[0].StartsWith("0x8"))
                    {
                        a_twcapability.Cap = (CAP)0xFFFF;
                    }
                    else if (asz[0].StartsWith("0x"))
                    {
                        int iNum = 0;
                        int.TryParse(asz[0].Substring(2), NumberStyles.AllowHexSpecifier | NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iNum);
                        a_twcapability.Cap = (CAP)iNum;
                    }
                    else
                    {
                        a_twcapability.Cap = (CAP)Enum.Parse(typeof(CAP), asz[0], true);
                    }
                }
                catch
                {
                    // don't log this exception...
                    a_twcapability.Cap = (CAP)0xFFFF;
                }
                if ((a_twcapability.Cap == (CAP)0xFFFF) || !asz[0].Contains("_"))
                {
                    a_twcapability.Cap = (CAP)Convert.ToUInt16(asz[0], 16);
                }

                // Set the container from text or decimal...
                if (asz.GetLength(0) >= 2)
                {
                    try
                    {
                        a_twcapability.ConType = (TWON)Enum.Parse(typeof(TWON), asz[1].Replace("TWON_", ""), true);
                    }
                    catch
                    {
                        // don't log this exception...
                        a_twcapability.ConType = (TWON)ushort.Parse(asz[1]);
                    }
                }

                // Set the item type from text or decimal...
                if (asz.GetLength(0) >= 3)
                {
                    try
                    {
                        twty = (TWTY)Enum.Parse(typeof(TWTY), asz[2].Replace("TWTY_", ""), true);
                    }
                    catch
                    {
                        // don't log this exception...
                        twty = (TWTY)ushort.Parse(asz[2]);
                    }
                }

                // Assign the new value...
                if (asz.GetLength(0) >= 4)
                {
                    switch (a_twcapability.ConType)
                    {
                        default:
                            a_szSetting = "(unrecognized container)";
                            return (false);

                        case TWON.ARRAY:
                            // Validate...
                            if (asz.GetLength(0) < 4)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Get the values...
                            u32NumItems = uint.Parse(asz[3]);

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (PlatformInfo.IsMacOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ARRAY_MACOSX)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ARRAY_MACOSX twarraymacosx = default;
                                twarraymacosx.ItemType = (uint)twty;
                                twarraymacosx.NumItems = u32NumItems;
                                Marshal.StructureToPtr(twarraymacosx, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twarraymacosx));
                            }
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ARRAY)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ARRAY twarray = default;
                                twarray.ItemType = twty;
                                twarray.NumItems = u32NumItems;
                                Marshal.StructureToPtr(twarray, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twarray));
                            }

                            // Set the ItemList...
                            for (ii = 0; ii < u32NumItems; ii++)
                            {
                                szResult = SetIndexedItem(a_twcapability, twty, intptr, ii, asz[ii + 4]);
                                if (szResult != "")
                                {
                                    return (false);
                                }
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);

                        case TWON.ENUMERATION:
                            // Validate...
                            if (asz.GetLength(0) < 6)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Get the values...
                            u32NumItems = uint.Parse(asz[3]);

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (PlatformInfo.IsMacOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION_MACOSX)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ENUMERATION_MACOSX twenumerationmacosx = default;
                                twenumerationmacosx.ItemType = (uint)twty;
                                twenumerationmacosx.NumItems = u32NumItems;
                                twenumerationmacosx.CurrentIndex = uint.Parse(asz[4]);
                                twenumerationmacosx.DefaultIndex = uint.Parse(asz[5]);
                                Marshal.StructureToPtr(twenumerationmacosx, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twenumerationmacosx));
                            }
                            // Windows or the 2.4+ Linux DSM...
                            else if ((PlatformInfo.IsWindows) || ((m_linuxdsm == LinuxDsm.IsLatestDsm) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm))))
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ENUMERATION twenumeration = default;
                                twenumeration.ItemType = twty;
                                twenumeration.NumItems = u32NumItems;
                                twenumeration.CurrentIndex = uint.Parse(asz[4]);
                                twenumeration.DefaultIndex = uint.Parse(asz[5]);
                                Marshal.StructureToPtr(twenumeration, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twenumeration));
                            }
                            // The -2.3 Linux DSM...
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION_LINUX64)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ENUMERATION_LINUX64 twenumerationlinux64 = default;
                                twenumerationlinux64.ItemType = twty;
                                twenumerationlinux64.NumItems = u32NumItems;
                                twenumerationlinux64.CurrentIndex = uint.Parse(asz[4]);
                                twenumerationlinux64.DefaultIndex = uint.Parse(asz[5]);
                                Marshal.StructureToPtr(twenumerationlinux64, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twenumerationlinux64));
                            }

                            // Set the ItemList...
                            for (ii = 0; ii < u32NumItems; ii++)
                            {
                                szResult = SetIndexedItem(a_twcapability, twty, intptr, ii, asz[ii + 6]);
                                if (szResult != "")
                                {
                                    return (false);
                                }
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);

                        case TWON.ONEVALUE:
                            // Validate...
                            if (asz.GetLength(0) < 4)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (PlatformInfo.IsMacOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE_MACOSX)) + Marshal.SizeOf(default(TW_STR255))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ONEVALUE_MACOSX twonevaluemacosx = default;
                                twonevaluemacosx.ItemType = (uint)twty;
                                Marshal.StructureToPtr(twonevaluemacosx, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twonevaluemacosx));
                            }
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE)) + Marshal.SizeOf(default(TW_STR255))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ONEVALUE twonevalue = default;
                                twonevalue.ItemType = twty;
                                Marshal.StructureToPtr(twonevalue, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twonevalue));
                            }

                            // Set the Item...
                            szResult = SetIndexedItem(a_twcapability, twty, intptr, 0, asz[3]);
                            if (szResult != "")
                            {
                                return (false);
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);

                        case TWON.RANGE:
                            // Validate...
                            if (asz.GetLength(0) < 8)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (PlatformInfo.IsMacOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_RANGE_MACOSX))));
                                intptr = DsmMemLock(a_twcapability.hContainer);
                            }
                            // Windows or the 2.4+ Linux DSM...
                            else if ((PlatformInfo.IsWindows) || ((m_linuxdsm == LinuxDsm.IsLatestDsm) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm))))
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_RANGE))));
                                intptr = DsmMemLock(a_twcapability.hContainer);
                            }
                            // The -2.3 Linux DSM...
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_RANGE_LINUX64))));
                                intptr = DsmMemLock(a_twcapability.hContainer);
                            }

                            // Set the Item...
                            szResult = SetRangeItem(twty, intptr, asz);
                            if (szResult != "")
                            {
                                return (false);
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);
                    }
                }

                // All done (this is good for a get where only the cap was specified)...
                return (true);
            }
            catch (Exception exception)
            {
                Log.Error("CsvToCapability exception - " + exception.Message);
                Log.Error("setting=<" + a_szSetting + ">");
                a_szValue = "(data error)";
                return (false);
            }
        }

        /// <summary>
        /// Convert the contents of a custom DS data to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twcustomdsdata">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public string CustomdsdataToCsv(TW_CUSTOMDSDATA a_twcustomdsdata)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twcustomdsdata.InfoLength.ToString());
                IntPtr intptr = DsmMemLock(a_twcustomdsdata.hData);
                csv.Add(intptr.ToString());
                DsmMemUnlock(a_twcustomdsdata.hData);
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert the contents of a string to a custom DS data structure...
        /// </summary>
        /// <param name="a_twcustomdsdata">A TWAIN structure</param>
        /// <param name="a_szCustomdsdata">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public bool CsvToCustomdsdata(ref TW_CUSTOMDSDATA a_twcustomdsdata, string a_szCustomdsdata)
        {
            // Init stuff...
            a_twcustomdsdata = default;

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szCustomdsdata);

                // Grab the values...
                a_twcustomdsdata.InfoLength = uint.Parse(asz[0]);
                a_twcustomdsdata.hData = DsmMemAlloc(a_twcustomdsdata.InfoLength);
                IntPtr intptr = DsmMemLock(a_twcustomdsdata.hData);
                byte[] bProfile = new byte[a_twcustomdsdata.InfoLength];
                Marshal.Copy((IntPtr)UInt64.Parse(asz[1]), bProfile, 0, (int)a_twcustomdsdata.InfoLength);
                Marshal.Copy(bProfile, 0, intptr, (int)a_twcustomdsdata.InfoLength);
                DsmMemUnlock(a_twcustomdsdata.hData);
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        /// <summary>
        /// Convert the contents of a statusutf8 structure to a string that
        /// we can show in our simple GUI...
        /// </summary>
        /// <param name="a_twstatusutf8">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        public string Statusutf8ToCsv(TW_STATUSUTF8 a_twstatusutf8)
        {
            try
            {
                CSV csv = new CSV();
                csv.Add(a_twstatusutf8.Status.ConditionCode.ToString());
                csv.Add(a_twstatusutf8.Status.Data.ToString());
                IntPtr intptr = DsmMemLock(a_twstatusutf8.UTF8string);
                csv.Add(intptr.ToString());
                DsmMemUnlock(a_twstatusutf8.UTF8string);
                return (csv.Get());
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return ("***error***");
            }
        }

        /// <summary>
        /// Convert a string to a userinterface...
        /// </summary>
        /// <param name="a_twuserinterface">A TWAIN structure</param>
        /// <param name="a_szUserinterface">A CSV string of the TWAIN structure</param>
        /// <returns>True if the conversion is successful</returns>
        public bool CsvToUserinterface(ref TW_USERINTERFACE a_twuserinterface, string a_szUserinterface)
        {
            // Init stuff...
            a_twuserinterface = default;

            // Build the string...
            try
            {
                string[] asz = CSV.Parse(a_szUserinterface);

                // Init stuff...
                a_twuserinterface.ShowUI = 0;
                a_twuserinterface.ModalUI = 0;
                a_twuserinterface.hParent = IntPtr.Zero;

                // Sort out the values...
                ushort.TryParse(CsvSerializer.CvtCapValueFromEnumHelper<bool>(asz[0]), out a_twuserinterface.ShowUI);
                ushort.TryParse(CsvSerializer.CvtCapValueFromEnumHelper<bool>(asz[1]), out a_twuserinterface.ModalUI);

                // Really shouldn't have this test, but I'll probably break things if I remove it...
                if (asz.Length >= 3)
                {
                    if (asz[2].ToLowerInvariant() == "hwnd")
                    {
                        a_twuserinterface.hParent = m_intptrHwnd;
                    }
                    else
                    {
                        Int64 i64;
                        if (Int64.TryParse(asz[2], out i64))
                        {
                            a_twuserinterface.hParent = (IntPtr)i64;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error("***error*** - " + exception.Message);
                return (false);
            }

            // All done...
            return (true);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Public DSM_Entry calls, most of the DSM_Entry calls are in here.  Their
        // main contribution is to make sure that they're running within the right
        // thread, but they also include the state transitions...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public DSM_Entry calls...

        /// <summary>
        /// Generic DSM when the dest must be zero (null)...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_dat">Data argument type</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twmemref">Pointer to data</param>
        /// <returns>TWAIN status</returns>
        public STS DsmEntryNullDest(DG a_dg, DAT a_dat, MSG a_msg, IntPtr a_twmemref)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twmemref = a_twmemref;
                    threaddata.msg = a_msg;
                    threaddata.dg = a_dg;
                    threaddata.dat = a_dat;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twmemref = m_twaincommand.Get(lIndex).twmemref;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), a_dat.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryNullDest(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryNullDest(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, a_dat, a_msg, a_twmemref);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryNullDest(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryNullDest(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryNullDest(ref m_twidentityApp, IntPtr.Zero, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryNullDest(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryNullDest(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, a_dat, a_msg, a_twmemref);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                TWAINWorkingGroup.Log.Assert("Unsupported platform..." + Environment.OSVersion.Platform);
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (sts);
        }

        /// <summary>
        /// Generic DSM when the dest must be a data source...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_dat">Data argument type</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twmemref">Pointer to data</param>
        /// <returns>TWAIN status</returns>
        public STS DsmEntry(DG a_dg, DAT a_dat, MSG a_msg, IntPtr a_twmemref)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twmemref = a_twmemref;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = a_dat;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twmemref = m_twaincommand.Get(lIndex).twmemref;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), a_dat.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntry(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntry(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, a_dat, a_msg, a_twmemref);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntry(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntry(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntry(ref m_twidentityApp, ref m_twidentityDs, a_dg, a_dat, a_msg, a_twmemref);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntry(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIOINFO, a_msg, a_twmemref);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntry(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIOINFO, a_msg, a_twmemref);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue file audio transfer commands...
        /// </summary>
        private void DatAudiofilexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatAudiofilexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudiofilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatAudiofilexfer.dg,
                m_threaddataDatAudiofilexfer.dat,
                m_threaddataDatAudiofilexfer.msg,
                IntPtr.Zero
            );
        }
        private void DatAudiofilexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatAudiofilexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryAudiofilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatAudiofilexfer.dg,
                m_threaddataDatAudiofilexfer.dat,
                m_threaddataDatAudiofilexfer.msg,
                IntPtr.Zero
            );
        }
        public STS DatAudiofilexfer(DG a_dg, MSG a_msg)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.AUDIOFILEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.AUDIOFILEXFER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatAudiofilexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudiofilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOFILEXFER, a_msg, IntPtr.Zero);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryAudiofilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOFILEXFER, a_msg, IntPtr.Zero);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatAudiofilexfer = default;
                                m_threaddataDatAudiofilexfer.blIsInuse = true;
                                m_threaddataDatAudiofilexfer.dg = a_dg;
                                m_threaddataDatAudiofilexfer.msg = a_msg;
                                m_threaddataDatAudiofilexfer.dat = DAT.AUDIOFILEXFER;
                                RunInUiThread(DatAudiofilexferWindowsTwain32);
                                sts = m_threaddataDatAudiofilexfer.sts;
                                m_threaddataDatAudiofilexfer = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatAudiofilexfer = default;
                                m_threaddataDatAudiofilexfer.blIsInuse = true;
                                m_threaddataDatAudiofilexfer.dg = a_dg;
                                m_threaddataDatAudiofilexfer.msg = a_msg;
                                m_threaddataDatAudiofilexfer.dat = DAT.AUDIOFILEXFER;
                                RunInUiThread(DatAudiofilexferWindowsTwainDsm);
                                sts = m_threaddataDatAudiofilexfer.sts;
                                m_threaddataDatAudiofilexfer = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryAudiofilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryAudiofilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryAudiofilexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.AUDIOFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryAudiofilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIOFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryAudiofilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIOFILEXFER, a_msg, IntPtr.Zero);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // If we had a successful transfer, then change state...
            if (sts == STS.XFERDONE)
            {
                m_state = STATE.S7;
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set audio info information...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twaudioinfo">Audio structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatAudioinfo(DG a_dg, MSG a_msg, ref TW_AUDIOINFO a_twaudioinfo)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twaudioinfo = a_twaudioinfo;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.AUDIOINFO;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twaudioinfo = m_twaincommand.Get(lIndex).twaudioinfo;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.AUDIOINFO.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudioAudioinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOINFO, a_msg, ref a_twaudioinfo);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryAudioAudioinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOINFO, a_msg, ref a_twaudioinfo);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryAudioAudioinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOINFO, a_msg, ref a_twaudioinfo);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryAudioAudioinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIOINFO, a_msg, ref a_twaudioinfo);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryAudioAudioinfo(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.AUDIOINFO, a_msg, ref a_twaudioinfo);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryAudioAudioinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIOINFO, a_msg, ref a_twaudioinfo);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryAudioAudioinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIOINFO, a_msg, ref a_twaudioinfo);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue native audio transfer commands...
        /// </summary>
        private void DatAudionativexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatAudionativexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudionativexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatAudionativexfer.dg,
                m_threaddataDatAudionativexfer.dat,
                m_threaddataDatAudionativexfer.msg,
                ref m_threaddataDatAudionativexfer.intptrAudio
            );
        }
        private void DatAudionativexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatAudionativexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryAudionativexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatAudionativexfer.dg,
                m_threaddataDatAudionativexfer.dat,
                m_threaddataDatAudionativexfer.msg,
                ref m_threaddataDatAudionativexfer.intptrAudio
            );
        }
        public STS DatAudionativexfer(DG a_dg, MSG a_msg, ref IntPtr a_intptrAudio)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.intptrAudio = a_intptrAudio;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.AUDIONATIVEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_intptrAudio = m_twaincommand.Get(lIndex).intptrAudio;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.AUDIONATIVEXFER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatAudionativexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatAudionativexfer = default;
                                m_threaddataDatAudionativexfer.blIsInuse = true;
                                m_threaddataDatAudionativexfer.dg = a_dg;
                                m_threaddataDatAudionativexfer.msg = a_msg;
                                m_threaddataDatAudionativexfer.dat = DAT.AUDIONATIVEXFER;
                                RunInUiThread(DatAudionativexferWindowsTwain32);
                                a_intptrAudio = m_threaddataDatAudionativexfer.intptrAudio;
                                sts = m_threaddataDatAudionativexfer.sts;
                                m_threaddataDatAudionativexfer = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatAudionativexfer = default;
                                m_threaddataDatAudionativexfer.blIsInuse = true;
                                m_threaddataDatAudionativexfer.dg = a_dg;
                                m_threaddataDatAudionativexfer.msg = a_msg;
                                m_threaddataDatAudionativexfer.dat = DAT.AUDIONATIVEXFER;
                                RunInUiThread(DatAudionativexferWindowsTwainDsm);
                                a_intptrAudio = m_threaddataDatAudionativexfer.intptrAudio;
                                sts = m_threaddataDatAudionativexfer.sts;
                                m_threaddataDatAudionativexfer = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    a_intptrAudio = IntPtr.Zero;
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryAudionativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryAudionativexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    a_intptrAudio = IntPtr.Zero;
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryAudionativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryAudionativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.AUDIONATIVEXFER, a_msg, ref a_intptrAudio);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // If we had a successful transfer, then change state...
            if (sts == STS.XFERDONE)
            {
                // Bump our state...
                m_state = STATE.S7;
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue callback commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twcallback">Callback structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatCallback(DG a_dg, MSG a_msg, ref TW_CALLBACK a_twcallback)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twcallback = a_twcallback;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.CALLBACK;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twcallback = m_twaincommand.Get(lIndex).twcallback;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.CALLBACK.ToString(), a_msg.ToString(), CsvSerializer.CallbackToCsv(a_twcallback));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK, a_msg, ref a_twcallback);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK, a_msg, ref a_twcallback);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK, a_msg, ref a_twcallback);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK, a_msg, ref a_twcallback);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCallback(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.CALLBACK, a_msg, ref a_twcallback);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryCallback(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DAT.CALLBACK, a_msg, ref a_twcallback);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCallback(ref m_twidentitymacosxApp, ref m_twidentityDs, a_dg, DAT.CALLBACK, a_msg, ref a_twcallback);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.CallbackToCsv(a_twcallback));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue callback2 commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twcallback2">Callback2 structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatCallback2(DG a_dg, MSG a_msg, ref TW_CALLBACK2 a_twcallback2)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twcallback2 = a_twcallback2;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.CALLBACK;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twcallback2 = m_twaincommand.Get(lIndex).twcallback2;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.CALLBACK2.ToString(), a_msg.ToString(), CsvSerializer.Callback2ToCsv(a_twcallback2));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryCallback2(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK2, a_msg, ref a_twcallback2);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCallback2(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK2, a_msg, ref a_twcallback2);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryCallback2(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK2, a_msg, ref a_twcallback2);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryCallback2(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CALLBACK2, a_msg, ref a_twcallback2);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCallback2(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.CALLBACK2, a_msg, ref a_twcallback2);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryCallback2(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DAT.CALLBACK2, a_msg, ref a_twcallback2);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCallback2(ref m_twidentitymacosxApp, ref m_twidentityDs, a_dg, DAT.CALLBACK2, a_msg, ref a_twcallback2);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.Callback2ToCsv(a_twcallback2));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue capabilities commands...
        /// </summary>
        private void DatCapabilityWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatCapability.sts = (STS)NativeMethods.WindowsTwain32DsmEntryCapability
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatCapability.dg,
                m_threaddataDatCapability.dat,
                m_threaddataDatCapability.msg,
                ref m_threaddataDatCapability.twcapability
            );
        }
        private void DatCapabilityWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatCapability.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCapability
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatCapability.dg,
                m_threaddataDatCapability.dat,
                m_threaddataDatCapability.msg,
                ref m_threaddataDatCapability.twcapability
            );
        }
        public STS DatCapability(DG a_dg, MSG a_msg, ref TW_CAPABILITY a_twcapability)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        ThreadData threaddata = default;
                        long lIndex = 0;

                        // TBD: sometimes this doesn't work!  Not sure why
                        // yet, but a retry takes care of it.
                        for (int ii = 0; ii < 5; ii++)
                        {
                            // Set our command variables...
                            threaddata = default;
                            threaddata.twcapability = a_twcapability;
                            threaddata.dg = a_dg;
                            threaddata.msg = a_msg;
                            threaddata.dat = DAT.CAPABILITY;
                            lIndex = m_twaincommand.Submit(threaddata);

                            // Submit the command and wait for the reply...
                            CallerToThreadSet();
                            ThreadToCallerWaitOne();

                            // Hmmm...
                            if ((a_msg == MSG.GETCURRENT)
                                && (m_twaincommand.Get(lIndex).sts == STS.SUCCESS)
                                && (m_twaincommand.Get(lIndex).twcapability.ConType == (TWON)0)
                                && (m_twaincommand.Get(lIndex).twcapability.hContainer == IntPtr.Zero))
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            // We're done...
                            break;
                        }

                        // Return the result...
                        a_twcapability = m_twaincommand.Get(lIndex).twcapability;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                if ((a_msg == MSG.SET) || (a_msg == MSG.SETCONSTRAINT))
                {
                    Log.LogSendBefore(a_dg.ToString(), DAT.CAPABILITY.ToString(), a_msg.ToString(), CapabilityToCsv(a_twcapability, (a_msg != MSG.QUERYSUPPORT)));
                }
                else
                {
                    string szCap = a_twcapability.Cap.ToString();
                    if (!szCap.Contains("_"))
                    {
                        szCap = "0x" + ((ushort)a_twcapability.Cap).ToString("X");
                    }
                    Log.LogSendBefore(a_dg.ToString(), DAT.CAPABILITY.ToString(), a_msg.ToString(), szCap + ",0,0");
                }
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatCapability.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CAPABILITY, a_msg, ref a_twcapability);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CAPABILITY, a_msg, ref a_twcapability);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatCapability = default;
                                m_threaddataDatCapability.blIsInuse = true;
                                m_threaddataDatCapability.dg = a_dg;
                                m_threaddataDatCapability.msg = a_msg;
                                m_threaddataDatCapability.dat = DAT.CAPABILITY;
                                m_threaddataDatCapability.twcapability = a_twcapability;
                                RunInUiThread(DatCapabilityWindowsTwain32);
                                a_twcapability = m_threaddataDatCapability.twcapability;
                                sts = m_threaddataDatCapability.sts;
                                m_threaddataDatCapability = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatCapability = default;
                                m_threaddataDatCapability.blIsInuse = true;
                                m_threaddataDatCapability.dg = a_dg;
                                m_threaddataDatCapability.msg = a_msg;
                                m_threaddataDatCapability.dat = DAT.CAPABILITY;
                                m_threaddataDatCapability.twcapability = a_twcapability;
                                RunInUiThread(DatCapabilityWindowsTwainDsm);
                                a_twcapability = m_threaddataDatCapability.twcapability;
                                sts = m_threaddataDatCapability.sts;
                                m_threaddataDatCapability = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CAPABILITY, a_msg, ref a_twcapability);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryCapability(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CAPABILITY, a_msg, ref a_twcapability);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCapability(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.CAPABILITY, a_msg, ref a_twcapability);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryCapability(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.CAPABILITY, a_msg, ref a_twcapability);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCapability(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.CAPABILITY, a_msg, ref a_twcapability);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                if ((a_msg == MSG.RESETALL) || ((sts != STS.SUCCESS) && (sts != STS.CHECKSTATUS)))
                {
                    Log.LogSendAfter(stsRcOrCc, "");
                }
                else
                {
                    Log.LogSendAfter(stsRcOrCc, CapabilityToCsv(a_twcapability, (a_msg != MSG.QUERYSUPPORT)));
                }
            }

            // if MSG_SET/CAP_LANGUAGE, then we want to track it
            if ((a_twcapability.Cap == CAP.CAP_LANGUAGE) && (a_msg == MSG.SET) && (a_twcapability.ConType == TWON.ONEVALUE))
            {
                string str;

                // if it was successful, then go with what was set.
                // otherwise ask the DS what it is currently set to
                if (sts == STS.SUCCESS)
                {
                    str = CapabilityToCsv(a_twcapability, (a_msg != MSG.QUERYSUPPORT));
                }
                else
                {
                    TW_CAPABILITY twcapability = new TW_CAPABILITY();
                    twcapability.Cap = CAP.CAP_LANGUAGE;
                    twcapability.ConType = TWON.ONEVALUE;
                    sts = DatCapability(a_dg, MSG.GETCURRENT, ref twcapability);
                    if (sts == STS.SUCCESS)
                    {
                        str = CapabilityToCsv(twcapability, (a_msg != MSG.QUERYSUPPORT));
                    }
                    else
                    {
                        // couldn't get the value, so go with English
                        str = "x," + ((int)TWLG.ENGLISH).ToString();
                    }
                }

                // get the value from the CSV string
                TWLG twlg = TWLG.ENGLISH;
                try
                {
                    string[] astr = str.Split(new char[] { ',' });
                    int result;
                    if (int.TryParse(astr[astr.Length - 1], out result))
                    {
                        twlg = (TWLG)result;
                    }
                }
                catch
                {
                    twlg = TWLG.ENGLISH;
                }
                Language.Set(twlg);
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set for CIE color...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twciecolor">CIECOLOR structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatCiecolor(DG a_dg, MSG a_msg, ref TW_CIECOLOR a_twciecolor)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twciecolor = a_twciecolor;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.CIECOLOR;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twciecolor = m_twaincommand.Get(lIndex).twciecolor;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.CIECOLOR.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryCiecolor(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCiecolor(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryCiecolor(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCiecolor(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.CIECOLOR, a_msg, ref a_twciecolor);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set the custom DS data...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twcustomdsdata">CUSTOMDSDATA structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatCustomdsdata(DG a_dg, MSG a_msg, ref TW_CUSTOMDSDATA a_twcustomdsdata)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twcustomdsdata = a_twcustomdsdata;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.CUSTOMDSDATA;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twcustomdsdata = m_twaincommand.Get(lIndex).twcustomdsdata;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.CUSTOMDSDATA.ToString(), a_msg.ToString(), CustomdsdataToCsv(a_twcustomdsdata));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryCustomdsdata(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CUSTOMDSDATA, a_msg, ref a_twcustomdsdata);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCustomdsdata(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CUSTOMDSDATA, a_msg, ref a_twcustomdsdata);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryCustomdsdata(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CUSTOMDSDATA, a_msg, ref a_twcustomdsdata);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryCustomdsdata(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.CUSTOMDSDATA, a_msg, ref a_twcustomdsdata);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCustomdsdata(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.CUSTOMDSDATA, a_msg, ref a_twcustomdsdata);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryCustomdsdata(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.CUSTOMDSDATA, a_msg, ref a_twcustomdsdata);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCustomdsdata(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.CUSTOMDSDATA, a_msg, ref a_twcustomdsdata);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CustomdsdataToCsv(a_twcustomdsdata));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get device events...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twdeviceevent">DEVICEEVENT structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatDeviceevent(DG a_dg, MSG a_msg, ref TW_DEVICEEVENT a_twdeviceevent)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twdeviceevent = a_twdeviceevent;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.DEVICEEVENT;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twdeviceevent = m_twaincommand.Get(lIndex).twdeviceevent;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.DEVICEEVENT.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryDeviceevent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.DEVICEEVENT, a_msg, ref a_twdeviceevent);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryDeviceevent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.DEVICEEVENT, a_msg, ref a_twdeviceevent);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryDeviceevent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.DEVICEEVENT, a_msg, ref a_twdeviceevent);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryDeviceevent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.DEVICEEVENT, a_msg, ref a_twdeviceevent);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryDeviceevent(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.DEVICEEVENT, a_msg, ref a_twdeviceevent);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryDeviceevent(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.DEVICEEVENT, a_msg, ref a_twdeviceevent);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryDeviceevent(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.DEVICEEVENT, a_msg, ref a_twdeviceevent);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.DeviceeventToCsv(a_twdeviceevent));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get the entrypoint data...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twentrypoint">ENTRYPOINT structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatEntrypoint(DG a_dg, MSG a_msg, ref TW_ENTRYPOINT a_twentrypoint)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twentrypoint = a_twentrypoint;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.ENTRYPOINT;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twentrypoint = m_twaincommand.Get(lIndex).twentrypoint;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.ENTRYPOINT.ToString(), a_msg.ToString(), CsvSerializer.EntrypointToCsv(a_twentrypoint));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    // Alright, this is super nasty.  The application issues the call to
                    // DAT_ENTRYPOINT before the call to MSG_OPENDS.  We don't know which
                    // driver they're going to use, so we don't know which DSM they're
                    // going to end up with.  This sucks in all kinds of ways.  The only
                    // reason we can hope for this to work is if all of the DSM's are in
                    // agreement about the memory functions they're using.  Lucky for us
                    // on Linux it's always been calloc/free.  So, we may be in the weird
                    // situation of using a different DSM for the memory functions, but
                    // it'l be okay.  You can stop breathing in and out of that paper bag...
                    sts = STS.BUMMER;
                    if (m_blFoundLatestDsm64)
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else if (m_blFoundLatestDsm)
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryEntrypoint(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else if (m_blFound020302Dsm64bit)
                    {
                        TW_ENTRYPOINT_LINUX64 twentrypointlinux64 = default;
                        twentrypointlinux64.Size = a_twentrypoint.Size;
                        twentrypointlinux64.DSM_MemAllocate = a_twentrypoint.DSM_MemAllocate;
                        twentrypointlinux64.DSM_MemFree = a_twentrypoint.DSM_MemFree;
                        twentrypointlinux64.DSM_MemLock = a_twentrypoint.DSM_MemLock;
                        twentrypointlinux64.DSM_MemUnlock = a_twentrypoint.DSM_MemUnlock;
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryEntrypoint(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.ENTRYPOINT, a_msg, ref twentrypointlinux64);
                        a_twentrypoint = default;
                        a_twentrypoint.Size = (uint)(twentrypointlinux64.Size & 0xFFFFFFFF);
                        a_twentrypoint.DSM_MemAllocate = twentrypointlinux64.DSM_MemAllocate;
                        a_twentrypoint.DSM_MemFree = twentrypointlinux64.DSM_MemFree;
                        a_twentrypoint.DSM_MemLock = twentrypointlinux64.DSM_MemLock;
                        a_twentrypoint.DSM_MemUnlock = twentrypointlinux64.DSM_MemUnlock;
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryEntrypoint(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryEntrypoint(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.ENTRYPOINT, a_msg, ref a_twentrypoint);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // If we were successful, then squirrel away the data...
            if (sts == STS.SUCCESS)
            {
                m_twentrypointdelegates = default;
                m_twentrypointdelegates.Size = a_twentrypoint.Size;
                m_twentrypointdelegates.DSM_Entry = a_twentrypoint.DSM_Entry;
                if (a_twentrypoint.DSM_MemAllocate != IntPtr.Zero)
                {
                    m_twentrypointdelegates.DSM_MemAllocate = (DSM_MEMALLOC)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemAllocate, typeof(DSM_MEMALLOC));
                }
                if (a_twentrypoint.DSM_MemFree != IntPtr.Zero)
                {
                    m_twentrypointdelegates.DSM_MemFree = (DSM_MEMFREE)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemFree, typeof(DSM_MEMFREE));
                }
                if (a_twentrypoint.DSM_MemLock != IntPtr.Zero)
                {
                    m_twentrypointdelegates.DSM_MemLock = (DSM_MEMLOCK)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemLock, typeof(DSM_MEMLOCK));
                }
                if (a_twentrypoint.DSM_MemUnlock != IntPtr.Zero)
                {
                    m_twentrypointdelegates.DSM_MemUnlock = (DSM_MEMUNLOCK)Marshal.GetDelegateForFunctionPointer(a_twentrypoint.DSM_MemUnlock, typeof(DSM_MEMUNLOCK));
                }
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.EntrypointToCsv(a_twentrypoint));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue event commands...
        /// </summary>
        private void DatEventWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatEvent.sts = (STS)NativeMethods.WindowsTwain32DsmEntryEvent
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatEvent.dg,
                m_threaddataDatEvent.dat,
                m_threaddataDatEvent.msg,
                ref m_threaddataDatEvent.twevent
            );
        }
        private void DatEventWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatEvent.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryEvent
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatEvent.dg,
                m_threaddataDatEvent.dat,
                m_threaddataDatEvent.msg,
                ref m_threaddataDatEvent.twevent
            );
        }
        public STS DatEvent(DG a_dg, MSG a_msg, ref TW_EVENT a_twevent, bool a_blInPreFilter = false)
        {
            STS sts;

            // Log it...
            if (Log.GetLevel() > 1)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.EVENT.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (a_blInPreFilter || m_threaddataDatEvent.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EVENT, a_msg, ref a_twevent);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EVENT, a_msg, ref a_twevent);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatEvent = default;
                                m_threaddataDatEvent.blIsInuse = true;
                                m_threaddataDatEvent.dg = a_dg;
                                m_threaddataDatEvent.msg = a_msg;
                                m_threaddataDatEvent.dat = DAT.EVENT;
                                m_threaddataDatEvent.twevent = a_twevent;
                                RunInUiThread(DatEventWindowsTwain32);
                                a_twevent = m_threaddataDatEvent.twevent;
                                sts = m_threaddataDatEvent.sts;
                                m_threaddataDatEvent = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatEvent = default;
                                m_threaddataDatEvent.blIsInuse = true;
                                m_threaddataDatEvent.dg = a_dg;
                                m_threaddataDatEvent.msg = a_msg;
                                m_threaddataDatEvent.dat = DAT.EVENT;
                                m_threaddataDatEvent.twevent = a_twevent;
                                RunInUiThread(DatEventWindowsTwainDsm);
                                a_twevent = m_threaddataDatEvent.twevent;
                                sts = m_threaddataDatEvent.sts;
                                m_threaddataDatEvent = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EVENT, a_msg, ref a_twevent);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryEvent(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EVENT, a_msg, ref a_twevent);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryEvent(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.EVENT, a_msg, ref a_twevent);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryEvent(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.EVENT, a_msg, ref a_twevent);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryEvent(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.EVENT, a_msg, ref a_twevent);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 1)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // Check the event for anything interesting...
            if ((sts == STS.DSEVENT) || (sts == STS.NOTDSEVENT))
            {
                ProcessEvent((MSG)a_twevent.TWMessage);
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set extended image info information...
        /// </summary>
        private void DatExtimageinfoWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatExtimageinfo.sts = (STS)NativeMethods.WindowsTwain32DsmEntryExtimageinfo
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatExtimageinfo.dg,
                m_threaddataDatExtimageinfo.dat,
                m_threaddataDatExtimageinfo.msg,
                ref m_threaddataDatExtimageinfo.twextimageinfo
            );
        }
        private void DatExtimageinfoWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatExtimageinfo.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryExtimageinfo
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatExtimageinfo.dg,
                m_threaddataDatExtimageinfo.dat,
                m_threaddataDatExtimageinfo.msg,
                ref m_threaddataDatExtimageinfo.twextimageinfo
            );
        }
        public STS DatExtimageinfo(DG a_dg, MSG a_msg, ref TW_EXTIMAGEINFO a_twextimageinfo)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twextimageinfo = a_twextimageinfo;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.EXTIMAGEINFO;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twextimageinfo = m_twaincommand.Get(lIndex).twextimageinfo;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.EXTIMAGEINFO.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatExtimageinfo.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatExtimageinfo = default;
                                m_threaddataDatExtimageinfo.blIsInuse = true;
                                m_threaddataDatExtimageinfo.dg = a_dg;
                                m_threaddataDatExtimageinfo.msg = a_msg;
                                m_threaddataDatExtimageinfo.dat = DAT.EXTIMAGEINFO;
                                m_threaddataDatExtimageinfo.twextimageinfo = a_twextimageinfo;
                                RunInUiThread(DatExtimageinfoWindowsTwain32);
                                a_twextimageinfo = m_threaddataDatExtimageinfo.twextimageinfo;
                                sts = m_threaddataDatExtimageinfo.sts;
                                m_threaddataDatExtimageinfo = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatExtimageinfo = default;
                                m_threaddataDatExtimageinfo.blIsInuse = true;
                                m_threaddataDatExtimageinfo.dg = a_dg;
                                m_threaddataDatExtimageinfo.msg = a_msg;
                                m_threaddataDatExtimageinfo.dat = DAT.EXTIMAGEINFO;
                                m_threaddataDatExtimageinfo.twextimageinfo = a_twextimageinfo;
                                RunInUiThread(DatExtimageinfoWindowsTwainDsm);
                                a_twextimageinfo = m_threaddataDatExtimageinfo.twextimageinfo;
                                sts = m_threaddataDatExtimageinfo.sts;
                                m_threaddataDatExtimageinfo = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryExtimageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryExtimageinfo(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryExtimageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryExtimageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.EXTIMAGEINFO, a_msg, ref a_twextimageinfo);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set the filesystem...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twfilesystem">FILESYSTEM structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatFilesystem(DG a_dg, MSG a_msg, ref TW_FILESYSTEM a_twfilesystem)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twfilesystem = a_twfilesystem;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.FILESYSTEM;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twfilesystem = m_twaincommand.Get(lIndex).twfilesystem;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.FILESYSTEM.ToString(), a_msg.ToString(), CsvSerializer.FilesystemToCsv(a_twfilesystem));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryFilesystem(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILESYSTEM, a_msg, ref a_twfilesystem);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryFilesystem(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILESYSTEM, a_msg, ref a_twfilesystem);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryFilesystem(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILESYSTEM, a_msg, ref a_twfilesystem);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryFilesystem(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILESYSTEM, a_msg, ref a_twfilesystem);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryFilesystem(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.FILESYSTEM, a_msg, ref a_twfilesystem);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryFilesystem(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.FILESYSTEM, a_msg, ref a_twfilesystem);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryFilesystem(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.FILESYSTEM, a_msg, ref a_twfilesystem);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.FilesystemToCsv(a_twfilesystem));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set filter information...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twfilter">FILTER structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatFilter(DG a_dg, MSG a_msg, ref TW_FILTER a_twfilter)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twfilter = a_twfilter;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.FILTER;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twfilter = m_twaincommand.Get(lIndex).twfilter;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.FILTER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryFilter(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILTER, a_msg, ref a_twfilter);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryFilter(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILTER, a_msg, ref a_twfilter);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryFilter(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILTER, a_msg, ref a_twfilter);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryFilter(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.FILTER, a_msg, ref a_twfilter);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryFilter(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.FILTER, a_msg, ref a_twfilter);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryFilter(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.FILTER, a_msg, ref a_twfilter);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryFilter(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.FILTER, a_msg, ref a_twfilter);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set for Gray response...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twgrayresponse">GRAYRESPONSE structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatGrayresponse(DG a_dg, MSG a_msg, ref TW_GRAYRESPONSE a_twgrayresponse)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twgrayresponse = a_twgrayresponse;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.GRAYRESPONSE;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twgrayresponse = m_twaincommand.Get(lIndex).twgrayresponse;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.GRAYRESPONSE.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryGrayresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.GRAYRESPONSE, a_msg, ref a_twgrayresponse);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryGrayresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.GRAYRESPONSE, a_msg, ref a_twgrayresponse);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryGrayresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.GRAYRESPONSE, a_msg, ref a_twgrayresponse);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryGrayresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.GRAYRESPONSE, a_msg, ref a_twgrayresponse);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryGrayresponse(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.GRAYRESPONSE, a_msg, ref a_twgrayresponse);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryGrayresponse(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.GRAYRESPONSE, a_msg, ref a_twgrayresponse);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryGrayresponse(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.GRAYRESPONSE, a_msg, ref a_twgrayresponse);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set an ICC profile...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twmemory">ICCPROFILE structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatIccprofile(DG a_dg, MSG a_msg, ref TW_MEMORY a_twmemory)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twmemory = a_twmemory;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.ICCPROFILE;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twmemory = m_twaincommand.Get(lIndex).twmemory;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.ICCPROFILE.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryIccprofile(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ICCPROFILE, a_msg, ref a_twmemory);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryIccprofile(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ICCPROFILE, a_msg, ref a_twmemory);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryIccprofile(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ICCPROFILE, a_msg, ref a_twmemory);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryIccprofile(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.ICCPROFILE, a_msg, ref a_twmemory);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIccprofile(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.ICCPROFILE, a_msg, ref a_twmemory);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryIccprofile(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.ICCPROFILE, a_msg, ref a_twmemory);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryIccprofile(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.ICCPROFILE, a_msg, ref a_twmemory);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue identity commands...
        /// </summary>
        private void DatIdentityWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            if (GetState() <= STATE.S3)
            {
                m_threaddataDatIdentity.sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentity
                (
                    ref m_twidentitylegacyApp,
                    IntPtr.Zero,
                    m_threaddataDatIdentity.dg,
                    m_threaddataDatIdentity.dat,
                    m_threaddataDatIdentity.msg,
                    ref m_threaddataDatIdentity.twidentitylegacy
                );
            }
            // Man, I'm learning stupid new stuff all the time, so the old DSM
            // had to have the destination.  Argh...
            else
            {
                m_threaddataDatIdentity.sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentityState4
                (
                    ref m_twidentitylegacyApp,
                    ref m_twidentitylegacyDs,
                    m_threaddataDatIdentity.dg,
                    m_threaddataDatIdentity.dat,
                    m_threaddataDatIdentity.msg,
                    ref m_threaddataDatIdentity.twidentitylegacy
                );
            }
        }
        private void DatIdentityWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatIdentity.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryIdentity
            (
                ref m_twidentitylegacyApp,
                IntPtr.Zero,
                m_threaddataDatIdentity.dg,
                m_threaddataDatIdentity.dat,
                m_threaddataDatIdentity.msg,
                ref m_threaddataDatIdentity.twidentitylegacy
            );
        }
        public STS DatIdentity(DG a_dg, MSG a_msg, ref TW_IDENTITY a_twidentity)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twidentity = a_twidentity;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.IDENTITY;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twidentity = m_twaincommand.Get(lIndex).twidentity;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.IDENTITY.ToString(), a_msg.ToString(), ((a_msg == MSG.OPENDS) ? CsvSerializer.IdentityToCsv(a_twidentity) : ""));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Convert the identity structure...
                TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);

                // Issue the command...
                try
                {
                    if (m_threaddataDatIdentity.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            if (GetState() <= STATE.S3)
                            {
                                sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                            else
                            {
                                sts = (STS)NativeMethods.WindowsTwain32DsmEntryIdentityState4(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatIdentity = default;
                                m_threaddataDatIdentity.blIsInuse = true;
                                m_threaddataDatIdentity.dg = a_dg;
                                m_threaddataDatIdentity.msg = a_msg;
                                m_threaddataDatIdentity.dat = DAT.IDENTITY;
                                m_threaddataDatIdentity.twidentitylegacy = twidentitylegacy;
                                RunInUiThread(DatIdentityWindowsTwain32);
                                twidentitylegacy = m_threaddataDatIdentity.twidentitylegacy;
                                sts = m_threaddataDatIdentity.sts;
                                m_threaddataDatIdentity = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatIdentity = default;
                                m_threaddataDatIdentity.blIsInuse = true;
                                m_threaddataDatIdentity.dg = a_dg;
                                m_threaddataDatIdentity.msg = a_msg;
                                m_threaddataDatIdentity.dat = DAT.IDENTITY;
                                m_threaddataDatIdentity.twidentitylegacy = twidentitylegacy;
                                RunInUiThread(DatIdentityWindowsTwainDsm);
                                twidentitylegacy = m_threaddataDatIdentity.twidentitylegacy;
                                sts = m_threaddataDatIdentity.sts;
                                m_threaddataDatIdentity = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
                a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command, we have a serious problem with 64-bit stuff
                // because TW_INT32 and TW_UINT32 were defined using long, which
                // made them 64-bit values (not a good idea for 32-bit types).  This
                // was fixed with TWAIN DSM 2.4, but it leaves us with a bit of a
                // mess.  So, unlike all of the other calls, we're going to allow
                // ourselves to access both DSMs from here.  This is only an issue
                // for 64-bit systems, and we're going to assume that the data source
                // is 2.4 or later, since that'll be the long term situation.  Note
                // that we assume the DSMs are protecting us from talking to the
                // wrong data source...
                try
                {
                    // Since life is complex, start by assuming failure...
                    sts = STS.FAILURE;

                    // Handle closeds...
                    if (a_msg == MSG.CLOSEDS)
                    {
                        // We've opened this source, and we know it's the new style...
                        if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }
                        // We've opened this source, and we know it's the new style...
                        else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }
                        // We've opened this source, and we know it's the old style...
                        else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                        {
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = TwidentityToTwidentitylinux64(a_twidentity);
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylinux64);
                            a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                        }
                        // We can't possibly have opened this source, so this had
                        // better be a sequence error...
                        else
                        {
                            sts = STS.SEQERROR;
                        }
                    }

                    // Getfirst always starts with the latest DSM, if it can't find it,
                    // or if it reports end of list, then go on to the old DSM, if we
                    // have one.  Note that it's up to the caller to handle any errors
                    // and keep searching.  We're not trying to figure out anything
                    // about the driver at this level...
                    else if (a_msg == MSG.GETFIRST)
                    {
                        m_linux64bitdsmDatIdentity = LinuxDsm.Unknown;

                        // Assume end of list for the outcome...
                        sts = STS.ENDOFLIST;

                        // Try to start with the latest DSM...
                        if (m_blFoundLatestDsm64)
                        {
                            m_linux64bitdsmDatIdentity = LinuxDsm.IsLatestDsm;
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }

                        // Try to start with the latest DSM...
                        if (m_blFoundLatestDsm)
                        {
                            m_linux64bitdsmDatIdentity = LinuxDsm.IsLatestDsm;
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                        }

                        // If the lastest DSM didn't work, try the old stuff...
                        if (m_blFound020302Dsm64bit && (sts == STS.ENDOFLIST))
                        {
                            m_linux64bitdsmDatIdentity = LinuxDsm.Is020302Dsm64bit;
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = TwidentityToTwidentitylinux64(a_twidentity);
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylinux64);
                            a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                        }
                    }

                    // Getnext gets its lead from getfirst, if we have a DSM
                    // value, we try it out, if we don't have one, we must be
                    // at the end of list.  We'll do the new DSM and then the
                    // old DSM (if we have one)...
                    else if (a_msg == MSG.GETNEXT)
                    {
                        bool blChangeToGetFirst = false;

                        // We're done, they'll have to use MSG_GETFIRST to start again...
                        if (m_linux64bitdsmDatIdentity == LinuxDsm.Unknown)
                        {
                            sts = STS.ENDOFLIST;
                        }

                        // We're working the latest DSM, if we hit end of list, then we'll
                        // try to switch over to the old DSM...
                        if (m_blFoundLatestDsm64 && (m_linux64bitdsmDatIdentity == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                            if (sts == STS.ENDOFLIST)
                            {
                                m_linux64bitdsmDatIdentity = m_blFound020302Dsm64bit ? LinuxDsm.Is020302Dsm64bit : LinuxDsm.Unknown;
                                blChangeToGetFirst = true;
                            }
                        }

                        // We're working the latest DSM, if we hit end of list, then we'll
                        // try to switch over to the old DSM...
                        if (m_blFoundLatestDsm && (m_linux64bitdsmDatIdentity == LinuxDsm.IsLatestDsm))
                        {
                            TW_IDENTITY_LEGACY twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                            if (sts == STS.ENDOFLIST)
                            {
                                m_linux64bitdsmDatIdentity = m_blFound020302Dsm64bit ? LinuxDsm.Is020302Dsm64bit : LinuxDsm.Unknown;
                                blChangeToGetFirst = true;
                            }
                        }

                        // We're working the old DSM, if we hit the end of list, then we
                        // clear the DSM indicator...
                        if (m_blFound020302Dsm64bit && (m_linux64bitdsmDatIdentity == LinuxDsm.Is020302Dsm64bit))
                        {
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = blChangeToGetFirst ? default(TW_IDENTITY_LINUX64) : TwidentityToTwidentitylinux64(a_twidentity);
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DAT.IDENTITY, blChangeToGetFirst ? MSG.GETFIRST : a_msg, ref twidentitylinux64);
                            a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                            if (sts == STS.ENDOFLIST)
                            {
                                m_linux64bitdsmDatIdentity = LinuxDsm.Unknown;
                            }
                        }
                    }

                    // Open always tries the current DSM, and then the older one, if needed...
                    else if (a_msg == MSG.OPENDS)
                    {
                        TW_IDENTITY_LEGACY twidentitylegacy = default;

                        // Prime the pump by assuming we didn't find anything...
                        sts = STS.NODS;

                        // Try with the latest DSM first, if we have one...
                        if (m_blFoundLatestDsm64)
                        {
                            twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            twidentitylegacy.Id = 0;
                            try
                            {
                                sts = (STS)NativeMethods.Linux64DsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                            catch
                            {
                                sts = STS.NODS;
                            }
                        }

                        // Try with the latest DSM first, if we have one...
                        if (m_blFoundLatestDsm)
                        {
                            twidentitylegacy = TwidentityToTwidentitylegacy(a_twidentity);
                            twidentitylegacy.Id = 0;
                            try
                            {
                                sts = (STS)NativeMethods.LinuxDsmEntryIdentity(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylegacy);
                            }
                            catch
                            {
                                sts = STS.NODS;
                            }
                        }

                        // We got it...
                        if (sts == STS.SUCCESS)
                        {
                            a_twidentity = TwidentitylegacyToTwidentity(twidentitylegacy);
                            m_linuxdsm = LinuxDsm.IsLatestDsm;
                        }

                        // No joy, so try the old DSM...
                        else if (m_blFound020302Dsm64bit)
                        {
                            TW_IDENTITY_LINUX64 twidentitylinux64App = TwidentityToTwidentitylinux64(m_twidentityApp);
                            TW_IDENTITY_LINUX64 twidentitylinux64 = TwidentityToTwidentitylinux64(a_twidentity);
                            twidentitylinux64.Id = 0;
                            try
                            {
                                sts = (STS)NativeMethods.Linux020302Dsm64bitEntryIdentity(ref twidentitylinux64App, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitylinux64);
                            }
                            catch
                            {
                                sts = STS.NODS;
                            }

                            // We got it...
                            if (sts == STS.SUCCESS)
                            {
                                a_twidentity = Twidentitylinux64ToTwidentity(twidentitylinux64);
                                m_linuxdsm = LinuxDsm.Is020302Dsm64bit;
                            }
                        }
                    }

                    // TBD: figure out how to safely do a set on Linux...
                    else if (a_msg == MSG.SET)
                    {
                        // Just pretend we did it...
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                TW_IDENTITY_MACOSX twidentitymacosx = TwidentityToTwidentitymacosx(a_twidentity);
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryIdentity(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitymacosx);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryIdentity(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DAT.IDENTITY, a_msg, ref twidentitymacosx);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
                a_twidentity = TwidentitymacosxToTwidentity(twidentitymacosx);
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.IdentityToCsv(a_twidentity));
            }

            // If we opened, go to state 4...
            if (a_msg == MSG.OPENDS)
            {
                if (sts == STS.SUCCESS)
                {
                    // Change our state, and record the identity we picked...
                    m_state = STATE.S4;
                    m_twidentityDs = a_twidentity;
                    m_twidentitylegacyDs = TwidentityToTwidentitylegacy(m_twidentityDs);
                    m_twidentitymacosxDs = TwidentityToTwidentitymacosx(m_twidentityDs);

                    // update language
                    Language.Set(m_twidentityDs.Version.Language);

                    // Register for callbacks...

                    // Windows...
                    if (PlatformInfo.IsWindows)
                    {
                        if (m_blUseCallbacks)
                        {
                            TW_CALLBACK twcallback = new TW_CALLBACK();
                            twcallback.CallBackProc = Marshal.GetFunctionPointerForDelegate(m_windowsdsmentrycontrolcallbackdelegate);
                            // Log it...
                            if (Log.GetLevel() > 0)
                            {
                                Log.LogSendBefore(a_dg.ToString(), DAT.CALLBACK.ToString(), a_msg.ToString(), CsvSerializer.CallbackToCsv(twcallback));
                            }
                            // Issue the command...
                            try
                            {
                                if (m_blUseLegacyDSM)
                                {
                                    sts = (STS)NativeMethods.WindowsTwain32DsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.CALLBACK, MSG.REGISTER_CALLBACK, ref twcallback);
                                }
                                else
                                {
                                    sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.CALLBACK, MSG.REGISTER_CALLBACK, ref twcallback);
                                }
                            }
                            catch (Exception exception)
                            {
                                // The driver crashed...
                                Log.Error("crash - " + exception.Message);
                                Log.LogSendAfter(STS.BUMMER, "");
                                return (STS.BUMMER);
                            }
                            // Log it...
                            if (Log.GetLevel() > 0)
                            {
                                Log.LogSendAfter(sts, "");
                            }
                        }
                    }

                    // Linux...
                    else if (PlatformInfo.IsLinux)
                    {
                        TW_CALLBACK twcallback = new TW_CALLBACK();
                        twcallback.CallBackProc = Marshal.GetFunctionPointerForDelegate(m_linuxdsmentrycontrolcallbackdelegate);
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendBefore(a_dg.ToString(), DAT.CALLBACK.ToString(), MSG.REGISTER_CALLBACK.ToString(), CsvSerializer.CallbackToCsv(twcallback));
                        }
                        // Issue the command...
                        try
                        {
                            if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                            {
                                sts = (STS)NativeMethods.Linux64DsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.CALLBACK, MSG.REGISTER_CALLBACK, ref twcallback);
                            }
                            else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                            {
                                sts = (STS)NativeMethods.LinuxDsmEntryCallback(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.CALLBACK, MSG.REGISTER_CALLBACK, ref twcallback);
                            }
                            else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                            {
                                sts = (STS)NativeMethods.Linux020302Dsm64bitEntryCallback(ref m_twidentityApp, ref m_twidentityDs, DG.CONTROL, DAT.CALLBACK, MSG.REGISTER_CALLBACK, ref twcallback);
                            }
                            else
                            {
                                Log.Error("apparently we don't have a DSM...");
                                sts = STS.BUMMER;
                            }
                        }
                        catch (Exception exception)
                        {
                            // The driver crashed...
                            Log.Error("crash - " + exception.Message);
                            Log.LogSendAfter(STS.BUMMER, "");
                            return (STS.BUMMER);
                        }
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendAfter(sts, "");
                        }
                    }

                    // Mac OS X, which has to be different...
                    else if (PlatformInfo.IsMacOSX)
                    {
                        IntPtr intptr = IntPtr.Zero;
                        TW_CALLBACK twcallback = new TW_CALLBACK();
                        twcallback.CallBackProc = Marshal.GetFunctionPointerForDelegate(m_macosxdsmentrycontrolcallbackdelegate);
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendBefore(a_dg.ToString(), DAT.CALLBACK.ToString(), a_msg.ToString(), CsvSerializer.CallbackToCsv(twcallback));
                        }
                        // Issue the command...
                        try
                        {
                            if (m_blUseLegacyDSM)
                            {
                                sts = (STS)NativeMethods.MacosxTwainDsmEntryCallback(ref m_twidentitymacosxApp, intptr, DG.CONTROL, DAT.CALLBACK, MSG.REGISTER_CALLBACK, ref twcallback);
                            }
                            else
                            {
                                sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryCallback(ref m_twidentitymacosxApp, ref m_twidentityDs, DG.CONTROL, DAT.CALLBACK, MSG.REGISTER_CALLBACK, ref twcallback);
                            }
                        }
                        catch (Exception exception)
                        {
                            // The driver crashed...
                            Log.Error("crash - " + exception.Message);
                            Log.LogSendAfter(STS.BUMMER, "");
                            return (STS.BUMMER);
                        }
                        // Log it...
                        if (Log.GetLevel() > 0)
                        {
                            Log.LogSendAfter(sts, "");
                        }
                    }
                }
            }

            // If we closed, go to state 3...
            else if (a_msg == MSG.CLOSEDS)
            {
                if (sts == STS.SUCCESS)
                {
                    m_state = STATE.S3;
                }
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set image info information...
        /// </summary>
        private void DatImageinfoWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImageinfo.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImageinfo
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImageinfo.dg,
                m_threaddataDatImageinfo.dat,
                m_threaddataDatImageinfo.msg,
                ref m_threaddataDatImageinfo.twimageinfo
            );
        }
        private void DatImageinfoWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImageinfo.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImageinfo
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImageinfo.dg,
                m_threaddataDatImageinfo.dat,
                m_threaddataDatImageinfo.msg,
                ref m_threaddataDatImageinfo.twimageinfo
            );
        }
        public STS DatImageinfo(DG a_dg, MSG a_msg, ref TW_IMAGEINFO a_twimageinfo)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twimageinfo = a_twimageinfo;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.IMAGEINFO;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twimageinfo = m_twaincommand.Get(lIndex).twimageinfo;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.IMAGEINFO.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImageinfo.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEINFO, a_msg, ref a_twimageinfo);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEINFO, a_msg, ref a_twimageinfo);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImageinfo = default;
                                m_threaddataDatImageinfo.blIsInuse = true;
                                m_threaddataDatImageinfo.dg = a_dg;
                                m_threaddataDatImageinfo.msg = a_msg;
                                m_threaddataDatImageinfo.dat = DAT.IMAGEINFO;
                                m_threaddataDatImageinfo.twimageinfo = a_twimageinfo;
                                RunInUiThread(DatImageinfoWindowsTwain32);
                                a_twimageinfo = m_threaddataDatImageinfo.twimageinfo;
                                sts = m_threaddataDatImageinfo.sts;
                                m_threaddataDatImageinfo = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImageinfo = default;
                                m_threaddataDatImageinfo.blIsInuse = true;
                                m_threaddataDatImageinfo.dg = a_dg;
                                m_threaddataDatImageinfo.msg = a_msg;
                                m_threaddataDatImageinfo.dat = DAT.IMAGEINFO;
                                m_threaddataDatImageinfo.twimageinfo = a_twimageinfo;
                                RunInUiThread(DatImageinfoWindowsTwainDsm);
                                a_twimageinfo = m_threaddataDatImageinfo.twimageinfo;
                                sts = m_threaddataDatImageinfo.sts;
                                m_threaddataDatImageinfo = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEINFO, a_msg, ref a_twimageinfo);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImageinfo(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEINFO, a_msg, ref a_twimageinfo);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        TW_IMAGEINFO_LINUX64 twimageinfolinux64 = default;
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImageinfo(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.IMAGEINFO, a_msg, ref twimageinfolinux64);
                        a_twimageinfo.XResolution = twimageinfolinux64.XResolution;
                        a_twimageinfo.YResolution = twimageinfolinux64.YResolution;
                        a_twimageinfo.ImageWidth = (int)twimageinfolinux64.ImageWidth;
                        a_twimageinfo.ImageLength = (int)twimageinfolinux64.ImageLength;
                        a_twimageinfo.SamplesPerPixel = twimageinfolinux64.SamplesPerPixel;
                        a_twimageinfo.BitsPerSample_0 = twimageinfolinux64.BitsPerSample_0;
                        a_twimageinfo.BitsPerSample_1 = twimageinfolinux64.BitsPerSample_1;
                        a_twimageinfo.BitsPerSample_2 = twimageinfolinux64.BitsPerSample_2;
                        a_twimageinfo.BitsPerSample_3 = twimageinfolinux64.BitsPerSample_3;
                        a_twimageinfo.BitsPerSample_4 = twimageinfolinux64.BitsPerSample_4;
                        a_twimageinfo.BitsPerSample_5 = twimageinfolinux64.BitsPerSample_5;
                        a_twimageinfo.BitsPerSample_6 = twimageinfolinux64.BitsPerSample_6;
                        a_twimageinfo.BitsPerSample_7 = twimageinfolinux64.BitsPerSample_7;
                        a_twimageinfo.BitsPerPixel = twimageinfolinux64.BitsPerPixel;
                        a_twimageinfo.Planar = twimageinfolinux64.Planar;
                        a_twimageinfo.PixelType = twimageinfolinux64.PixelType;
                        a_twimageinfo.Compression = twimageinfolinux64.Compression;
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEINFO, a_msg, ref a_twimageinfo);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImageinfo(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEINFO, a_msg, ref a_twimageinfo);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.ImageinfoToCsv(a_twimageinfo));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set layout information...
        /// </summary>
        private void DatImagelayoutWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagelayout.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagelayout
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagelayout.dg,
                m_threaddataDatImagelayout.dat,
                m_threaddataDatImagelayout.msg,
                ref m_threaddataDatImagelayout.twimagelayout
            );
        }
        private void DatImagelayoutWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagelayout.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagelayout
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagelayout.dg,
                m_threaddataDatImagelayout.dat,
                m_threaddataDatImagelayout.msg,
                ref m_threaddataDatImagelayout.twimagelayout
            );
        }
        public STS DatImagelayout(DG a_dg, MSG a_msg, ref TW_IMAGELAYOUT a_twimagelayout)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twimagelayout = a_twimagelayout;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.IMAGELAYOUT;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twimagelayout = m_twaincommand.Get(lIndex).twimagelayout;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.IMAGELAYOUT.ToString(), a_msg.ToString(), CsvSerializer.ImagelayoutToCsv(a_twimagelayout));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImagelayout.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagelayout = default;
                                m_threaddataDatImagelayout.blIsInuse = true;
                                m_threaddataDatImagelayout.dg = a_dg;
                                m_threaddataDatImagelayout.msg = a_msg;
                                m_threaddataDatImagelayout.dat = DAT.IMAGELAYOUT;
                                m_threaddataDatImagelayout.twimagelayout = a_twimagelayout;
                                RunInUiThread(DatImagelayoutWindowsTwain32);
                                a_twimagelayout = m_threaddataDatImagelayout.twimagelayout;
                                sts = m_threaddataDatImagelayout.sts;
                                m_threaddataDatImagelayout = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagelayout = default;
                                m_threaddataDatImagelayout.blIsInuse = true;
                                m_threaddataDatImagelayout.dg = a_dg;
                                m_threaddataDatImagelayout.msg = a_msg;
                                m_threaddataDatImagelayout.dat = DAT.IMAGELAYOUT;
                                m_threaddataDatImagelayout.twimagelayout = a_twimagelayout;
                                RunInUiThread(DatImagelayoutWindowsTwainDsm);
                                a_twimagelayout = m_threaddataDatImagelayout.twimagelayout;
                                sts = m_threaddataDatImagelayout.sts;
                                m_threaddataDatImagelayout = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImagelayout(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagelayout(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImagelayout(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagelayout(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGELAYOUT, a_msg, ref a_twimagelayout);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.ImagelayoutToCsv(a_twimagelayout));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue file image transfer commands...
        /// </summary>
        private void DatImagefilexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagefilexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagefilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagefilexfer.dg,
                m_threaddataDatImagefilexfer.dat,
                m_threaddataDatImagefilexfer.msg,
                IntPtr.Zero
            );
        }
        private void DatImagefilexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagefilexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagefilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagefilexfer.dg,
                m_threaddataDatImagefilexfer.dat,
                m_threaddataDatImagefilexfer.msg,
                IntPtr.Zero
            );
        }
        public STS DatImagefilexfer(DG a_dg, MSG a_msg)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.IMAGEFILEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.IMAGEFILEXFER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImagefilexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagefilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEFILEXFER, a_msg, IntPtr.Zero);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagefilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEFILEXFER, a_msg, IntPtr.Zero);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagefilexfer = default;
                                m_threaddataDatImagefilexfer.blIsInuse = true;
                                m_threaddataDatImagefilexfer.dg = a_dg;
                                m_threaddataDatImagefilexfer.msg = a_msg;
                                m_threaddataDatImagefilexfer.dat = DAT.IMAGEFILEXFER;
                                RunInUiThread(DatImagefilexferWindowsTwain32);
                                sts = m_threaddataDatImagefilexfer.sts;
                                m_threaddataDatImagefilexfer = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagefilexfer = default;
                                m_threaddataDatImagefilexfer.blIsInuse = true;
                                m_threaddataDatImagefilexfer.dg = a_dg;
                                m_threaddataDatImagefilexfer.msg = a_msg;
                                m_threaddataDatImagefilexfer.dat = DAT.IMAGEFILEXFER;
                                RunInUiThread(DatImagefilexferWindowsTwainDsm);
                                sts = m_threaddataDatImagefilexfer.sts;
                                m_threaddataDatImagefilexfer = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryImagefilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImagefilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagefilexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.IMAGEFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImagefilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEFILEXFER, a_msg, IntPtr.Zero);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagefilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEFILEXFER, a_msg, IntPtr.Zero);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // If we had a successful transfer, then change state...
            if (sts == STS.XFERDONE)
            {
                m_state = STATE.S7;
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue memory file image transfer commands...
        /// </summary>
        private void DatImagememfilexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagememfilexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagememfilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagememfilexfer.dg,
                m_threaddataDatImagememfilexfer.dat,
                m_threaddataDatImagememfilexfer.msg,
                ref m_threaddataDatImagememfilexfer.twimagememxfer
            );
        }
        private void DatImagememfilexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagememfilexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagememfilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagememfilexfer.dg,
                m_threaddataDatImagememfilexfer.dat,
                m_threaddataDatImagememfilexfer.msg,
                ref m_threaddataDatImagememfilexfer.twimagememxfer
            );
        }
        public STS DatImagememfilexfer(DG a_dg, MSG a_msg, ref TW_IMAGEMEMXFER a_twimagememxfer)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twimagememxfer = a_twimagememxfer;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.IMAGEMEMFILEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twimagememxfer = m_twaincommand.Get(lIndex).twimagememxfer;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.IMAGEMEMFILEXFER.ToString(), a_msg.ToString(), CsvSerializer.ImagememxferToCsv(a_twimagememxfer));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImagememfilexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagememfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMFILEXFER, a_msg, ref a_twimagememxfer);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagememfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMFILEXFER, a_msg, ref a_twimagememxfer);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagememfilexfer = default;
                                m_threaddataDatImagememfilexfer.blIsInuse = true;
                                m_threaddataDatImagememfilexfer.dg = a_dg;
                                m_threaddataDatImagememfilexfer.msg = a_msg;
                                m_threaddataDatImagememfilexfer.dat = DAT.IMAGEMEMFILEXFER;
                                m_threaddataDatImagememfilexfer.twimagememxfer = a_twimagememxfer;
                                RunInUiThread(DatImagememfilexferWindowsTwain32);
                                a_twimagememxfer = m_threaddataDatImagememfilexfer.twimagememxfer;
                                sts = m_threaddataDatImagememfilexfer.sts;
                                m_threaddataDatImagememfilexfer = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagememfilexfer = default;
                                m_threaddataDatImagememfilexfer.blIsInuse = true;
                                m_threaddataDatImagememfilexfer.dg = a_dg;
                                m_threaddataDatImagememfilexfer.msg = a_msg;
                                m_threaddataDatImagememfilexfer.dat = DAT.IMAGEMEMFILEXFER;
                                m_threaddataDatImagememfilexfer.twimagememxfer = a_twimagememxfer;
                                RunInUiThread(DatImagememfilexferWindowsTwainDsm);
                                a_twimagememxfer = m_threaddataDatImagememfilexfer.twimagememxfer;
                                sts = m_threaddataDatImagememfilexfer.sts;
                                m_threaddataDatImagememfilexfer = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryImagememfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMFILEXFER, a_msg, ref a_twimagememxfer);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImagememfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMFILEXFER, a_msg, ref a_twimagememxfer);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64 = default;
                        twimagememxferlinux64.BytesPerRow = a_twimagememxfer.BytesPerRow;
                        twimagememxferlinux64.BytesWritten = a_twimagememxfer.BytesWritten;
                        twimagememxferlinux64.Columns = a_twimagememxfer.Columns;
                        twimagememxferlinux64.Compression = a_twimagememxfer.Compression;
                        twimagememxferlinux64.MemoryFlags = a_twimagememxfer.Memory.Flags;
                        twimagememxferlinux64.MemoryLength = a_twimagememxfer.Memory.Length;
                        twimagememxferlinux64.MemoryTheMem = a_twimagememxfer.Memory.TheMem;
                        twimagememxferlinux64.Rows = a_twimagememxfer.Rows;
                        twimagememxferlinux64.XOffset = a_twimagememxfer.XOffset;
                        twimagememxferlinux64.YOffset = a_twimagememxfer.YOffset;
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagememfilexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.IMAGEMEMFILEXFER, a_msg, ref twimagememxferlinux64);
                        a_twimagememxfer.BytesPerRow = (uint)twimagememxferlinux64.BytesPerRow;
                        a_twimagememxfer.BytesWritten = (uint)twimagememxferlinux64.BytesWritten;
                        a_twimagememxfer.Columns = (uint)twimagememxferlinux64.Columns;
                        a_twimagememxfer.Compression = (ushort)twimagememxferlinux64.Compression;
                        a_twimagememxfer.Memory.Flags = (uint)twimagememxferlinux64.MemoryFlags;
                        a_twimagememxfer.Memory.Length = (uint)twimagememxferlinux64.MemoryLength;
                        a_twimagememxfer.Memory.TheMem = twimagememxferlinux64.MemoryTheMem;
                        a_twimagememxfer.Rows = (uint)twimagememxferlinux64.Rows;
                        a_twimagememxfer.XOffset = (uint)twimagememxferlinux64.XOffset;
                        a_twimagememxfer.YOffset = (uint)twimagememxferlinux64.YOffset;
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    TW_IMAGEMEMXFER_MACOSX twimagememxfermacosx = default;
                    twimagememxfermacosx.BytesPerRow = a_twimagememxfer.BytesPerRow;
                    twimagememxfermacosx.BytesWritten = a_twimagememxfer.BytesWritten;
                    twimagememxfermacosx.Columns = a_twimagememxfer.Columns;
                    twimagememxfermacosx.Compression = a_twimagememxfer.Compression;
                    twimagememxfermacosx.Memory.Flags = a_twimagememxfer.Memory.Flags;
                    twimagememxfermacosx.Memory.Length = a_twimagememxfer.Memory.Length;
                    twimagememxfermacosx.Memory.TheMem = a_twimagememxfer.Memory.TheMem;
                    twimagememxfermacosx.Rows = a_twimagememxfer.Rows;
                    twimagememxfermacosx.XOffset = a_twimagememxfer.XOffset;
                    twimagememxfermacosx.YOffset = a_twimagememxfer.YOffset;
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImagememfilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEMEMFILEXFER, a_msg, ref twimagememxfermacosx);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagememfilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEMEMFILEXFER, a_msg, ref twimagememxfermacosx);
                    }
                    a_twimagememxfer.BytesPerRow = twimagememxfermacosx.BytesPerRow;
                    a_twimagememxfer.BytesWritten = twimagememxfermacosx.BytesWritten;
                    a_twimagememxfer.Columns = twimagememxfermacosx.Columns;
                    a_twimagememxfer.Compression = (ushort)twimagememxfermacosx.Compression;
                    a_twimagememxfer.Memory.Flags = twimagememxfermacosx.Memory.Flags;
                    a_twimagememxfer.Memory.Length = twimagememxfermacosx.Memory.Length;
                    a_twimagememxfer.Memory.TheMem = twimagememxfermacosx.Memory.TheMem;
                    a_twimagememxfer.Rows = twimagememxfermacosx.Rows;
                    a_twimagememxfer.XOffset = twimagememxfermacosx.XOffset;
                    a_twimagememxfer.YOffset = twimagememxfermacosx.YOffset;
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.ImagememxferToCsv(a_twimagememxfer));
            }

            // If we had a successful transfer, then change state...
            if (sts == STS.XFERDONE)
            {
                m_state = STATE.S7;
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue memory image transfer commands...
        /// </summary>
        private void DatImagememxferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagememxfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagememxfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagememxfer.dg,
                m_threaddataDatImagememxfer.dat,
                m_threaddataDatImagememxfer.msg,
                ref m_threaddataDatImagememxfer.twimagememxfer
            );
        }
        private void DatImagememxferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagememxfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagememxfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagememxfer.dg,
                m_threaddataDatImagememxfer.dat,
                m_threaddataDatImagememxfer.msg,
                ref m_threaddataDatImagememxfer.twimagememxfer
            );
        }
        public STS DatImagememxfer(DG a_dg, MSG a_msg, ref TW_IMAGEMEMXFER a_twimagememxfer)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twimagememxfer = a_twimagememxfer;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.IMAGEMEMXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twimagememxfer = m_twaincommand.Get(lIndex).twimagememxfer;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.IMAGEMEMXFER.ToString(), a_msg.ToString(), CsvSerializer.ImagememxferToCsv(a_twimagememxfer));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImagememxfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagememxfer = default;
                                m_threaddataDatImagememxfer.blIsInuse = true;
                                m_threaddataDatImagememxfer.dg = a_dg;
                                m_threaddataDatImagememxfer.msg = a_msg;
                                m_threaddataDatImagememxfer.dat = DAT.IMAGEMEMXFER;
                                m_threaddataDatImagememxfer.twimagememxfer = a_twimagememxfer;
                                RunInUiThread(DatImagememxferWindowsTwain32);
                                a_twimagememxfer = m_threaddataDatImagememxfer.twimagememxfer;
                                sts = m_threaddataDatImagememxfer.sts;
                                m_threaddataDatImagememxfer = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagememxfer = default;
                                m_threaddataDatImagememxfer.blIsInuse = true;
                                m_threaddataDatImagememxfer.dg = a_dg;
                                m_threaddataDatImagememxfer.msg = a_msg;
                                m_threaddataDatImagememxfer.dat = DAT.IMAGEMEMXFER;
                                m_threaddataDatImagememxfer.twimagememxfer = a_twimagememxfer;
                                RunInUiThread(DatImagememxferWindowsTwainDsm);
                                a_twimagememxfer = m_threaddataDatImagememxfer.twimagememxfer;
                                sts = m_threaddataDatImagememxfer.sts;
                                m_threaddataDatImagememxfer = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64 = default;
                        twimagememxferlinux64.BytesPerRow = a_twimagememxfer.BytesPerRow;
                        twimagememxferlinux64.BytesWritten = a_twimagememxfer.BytesWritten;
                        twimagememxferlinux64.Columns = a_twimagememxfer.Columns;
                        twimagememxferlinux64.Compression = a_twimagememxfer.Compression;
                        twimagememxferlinux64.MemoryFlags = a_twimagememxfer.Memory.Flags;
                        twimagememxferlinux64.MemoryLength = a_twimagememxfer.Memory.Length;
                        twimagememxferlinux64.MemoryTheMem = a_twimagememxfer.Memory.TheMem;
                        twimagememxferlinux64.Rows = a_twimagememxfer.Rows;
                        twimagememxferlinux64.XOffset = a_twimagememxfer.XOffset;
                        twimagememxferlinux64.YOffset = a_twimagememxfer.YOffset;
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagememxfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.IMAGEMEMXFER, a_msg, ref twimagememxferlinux64);
                        a_twimagememxfer.BytesPerRow = (uint)twimagememxferlinux64.BytesPerRow;
                        a_twimagememxfer.BytesWritten = (uint)twimagememxferlinux64.BytesWritten;
                        a_twimagememxfer.Columns = (uint)twimagememxferlinux64.Columns;
                        a_twimagememxfer.Compression = (ushort)twimagememxferlinux64.Compression;
                        a_twimagememxfer.Memory.Flags = (uint)twimagememxferlinux64.MemoryFlags;
                        a_twimagememxfer.Memory.Length = (uint)twimagememxferlinux64.MemoryLength;
                        a_twimagememxfer.Memory.TheMem = twimagememxferlinux64.MemoryTheMem;
                        a_twimagememxfer.Rows = (uint)twimagememxferlinux64.Rows;
                        a_twimagememxfer.XOffset = (uint)twimagememxferlinux64.XOffset;
                        a_twimagememxfer.YOffset = (uint)twimagememxferlinux64.YOffset;
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    TW_IMAGEMEMXFER_MACOSX twimagememxfermacosx = default;
                    twimagememxfermacosx.BytesPerRow = a_twimagememxfer.BytesPerRow;
                    twimagememxfermacosx.BytesWritten = a_twimagememxfer.BytesWritten;
                    twimagememxfermacosx.Columns = a_twimagememxfer.Columns;
                    twimagememxfermacosx.Compression = a_twimagememxfer.Compression;
                    twimagememxfermacosx.Memory.Flags = a_twimagememxfer.Memory.Flags;
                    twimagememxfermacosx.Memory.Length = a_twimagememxfer.Memory.Length;
                    twimagememxfermacosx.Memory.TheMem = a_twimagememxfer.Memory.TheMem;
                    twimagememxfermacosx.Rows = a_twimagememxfer.Rows;
                    twimagememxfermacosx.XOffset = a_twimagememxfer.XOffset;
                    twimagememxfermacosx.YOffset = a_twimagememxfer.YOffset;
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImagememxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEMEMXFER, a_msg, ref twimagememxfermacosx);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagememxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGEMEMXFER, a_msg, ref twimagememxfermacosx);
                    }
                    a_twimagememxfer.BytesPerRow = twimagememxfermacosx.BytesPerRow;
                    a_twimagememxfer.BytesWritten = twimagememxfermacosx.BytesWritten;
                    a_twimagememxfer.Columns = twimagememxfermacosx.Columns;
                    a_twimagememxfer.Compression = (ushort)twimagememxfermacosx.Compression;
                    a_twimagememxfer.Memory.Flags = twimagememxfermacosx.Memory.Flags;
                    a_twimagememxfer.Memory.Length = twimagememxfermacosx.Memory.Length;
                    a_twimagememxfer.Memory.TheMem = twimagememxfermacosx.Memory.TheMem;
                    a_twimagememxfer.Rows = twimagememxfermacosx.Rows;
                    a_twimagememxfer.XOffset = twimagememxfermacosx.XOffset;
                    a_twimagememxfer.YOffset = twimagememxfermacosx.YOffset;
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.ImagememxferToCsv(a_twimagememxfer));
            }

            // If we had a successful transfer, then change state...
            if (sts == STS.XFERDONE)
            {
                m_state = STATE.S7;
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue native image transfer commands...
        /// </summary>
        private void DatImagenativexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagenativexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagenativexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagenativexfer.dg,
                m_threaddataDatImagenativexfer.dat,
                m_threaddataDatImagenativexfer.msg,
                ref m_threaddataDatImagenativexfer.intptrBitmap
            );
        }
        private void DatImagenativexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatImagenativexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagenativexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatImagenativexfer.dg,
                m_threaddataDatImagenativexfer.dat,
                m_threaddataDatImagenativexfer.msg,
                ref m_threaddataDatImagenativexfer.intptrBitmap
            );
        }
        // TODO: Recode later
        //public STS DatImagenativexfer(DG a_dg, MSG a_msg, ref Bitmap a_bitmap)
        //{
        //    IntPtr intptrBitmapHandle = IntPtr.Zero;
        //    return (DatImagenativexferBitmap(a_dg, a_msg, ref a_bitmap, ref intptrBitmapHandle, false));
        //}
        //public STS DatImagenativexferHandle(DG a_dg, MSG a_msg, ref IntPtr a_intptrBitmapHandle)
        //{
        //    Bitmap bitmap = null;
        //    return (DatImagenativexferBitmap(a_dg, a_msg, ref bitmap, ref a_intptrBitmapHandle, true));
        //}
        //public STS DatImagenativexferBitmap(DG a_dg, MSG a_msg, ref Bitmap a_bitmap, ref IntPtr a_intptrBitmapHandle, bool a_blUseBitmapHandle)
        //{
        //    STS sts;
        //    IntPtr intptrBitmap = IntPtr.Zero;

        //    // Submit the work to the TWAIN thread...
        //    if (this.m_runinuithreaddelegate == null)
        //    {
        //        if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
        //        {
        //            lock (m_lockTwain)
        //            {
        //                // Set our command variables...
        //                ThreadData threaddata = default;
        //                threaddata.bitmap = a_bitmap;
        //                threaddata.blUseBitmapHandle = a_blUseBitmapHandle;
        //                threaddata.dg = a_dg;
        //                threaddata.msg = a_msg;
        //                threaddata.dat = DAT.IMAGENATIVEXFER;
        //                long lIndex = m_twaincommand.Submit(threaddata);

        //                // Submit the command and wait for the reply...
        //                CallerToThreadSet();
        //                ThreadToCallerWaitOne();

        //                // Return the result...
        //                a_bitmap = m_twaincommand.Get(lIndex).bitmap;
        //                a_intptrBitmapHandle = m_twaincommand.Get(lIndex).intptrBitmap;
        //                sts = m_twaincommand.Get(lIndex).sts;

        //                // Clear the command variables...
        //                m_twaincommand.Delete(lIndex);
        //            }
        //            return (sts);
        //        }
        //    }

        //    // Log it...
        //    if (Log.GetLevel() > 0)
        //    {
        //        Log.LogSendBefore(a_dg.ToString(), DAT.IMAGENATIVEXFER.ToString(), a_msg.ToString(), "");
        //    }

        //    // Windows...
        //    if (PlatformInfo.IsWindows)
        //    {
        //        // Issue the command...
        //        try
        //        {
        //            if (m_threaddataDatImagenativexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
        //            {
        //                if (m_blUseLegacyDSM)
        //                {
        //                    sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
        //                }
        //                else
        //                {
        //                    sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
        //                }
        //            }
        //            else
        //            {
        //                if (m_blUseLegacyDSM)
        //                {
        //                    lock (m_lockTwain)
        //                    {
        //                        m_threaddataDatImagenativexfer = default;
        //                        m_threaddataDatImagenativexfer.blIsInuse = true;
        //                        m_threaddataDatImagenativexfer.dg = a_dg;
        //                        m_threaddataDatImagenativexfer.msg = a_msg;
        //                        m_threaddataDatImagenativexfer.dat = DAT.IMAGENATIVEXFER;
        //                        RunInUiThread(DatImagenativexferWindowsTwain32);
        //                        intptrBitmap = a_intptrBitmapHandle = m_threaddataDatImagenativexfer.intptrBitmap;
        //                        sts = m_threaddataDatImagenativexfer.sts;
        //                        m_threaddataDatImagenativexfer = default;
        //                    }
        //                }
        //                else
        //                {
        //                    lock (m_lockTwain)
        //                    {
        //                        m_threaddataDatImagenativexfer = default;
        //                        m_threaddataDatImagenativexfer.blIsInuse = true;
        //                        m_threaddataDatImagenativexfer.dg = a_dg;
        //                        m_threaddataDatImagenativexfer.msg = a_msg;
        //                        m_threaddataDatImagenativexfer.dat = DAT.IMAGENATIVEXFER;
        //                        RunInUiThread(DatImagenativexferWindowsTwainDsm);
        //                        intptrBitmap = a_intptrBitmapHandle = m_threaddataDatImagenativexfer.intptrBitmap;
        //                        sts = m_threaddataDatImagenativexfer.sts;
        //                        m_threaddataDatImagenativexfer = default;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            // The driver crashed...
        //            Log.Error("crash - " + exception.Message);
        //            Log.LogSendAfter(STS.BUMMER, "");
        //            return (STS.BUMMER);
        //        }
        //    }

        //    // Linux...
        //    else if (PlatformInfo.IsLinux)
        //    {
        //        // Issue the command...
        //        try
        //        {
        //            if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
        //            {
        //                sts = (STS)NativeMethods.Linux64DsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
        //            }
        //            else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
        //            {
        //                sts = (STS)NativeMethods.LinuxDsmEntryImagenativexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
        //            }
        //            else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
        //            {
        //                sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagenativexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
        //            }
        //            else
        //            {
        //                Log.Error("apparently we don't have a DSM...");
        //                sts = STS.BUMMER;
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            // The driver crashed...
        //            Log.Error("crash - " + exception.Message);
        //            Log.LogSendAfter(STS.BUMMER, "");
        //            return (STS.BUMMER);
        //        }
        //    }

        //    // Mac OS X, which has to be different...
        //    else if (PlatformInfo.IsMacOSX)
        //    {
        //        // Issue the command...
        //        try
        //        {
        //            intptrBitmap = IntPtr.Zero;
        //            if (m_blUseLegacyDSM)
        //            {
        //                sts = (STS)NativeMethods.MacosxTwainDsmEntryImagenativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
        //            }
        //            else
        //            {
        //                sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagenativexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.IMAGENATIVEXFER, a_msg, ref intptrBitmap);
        //            }
        //        }
        //        catch (Exception exception)
        //        {
        //            // The driver crashed...
        //            Log.Error("crash - " + exception.Message);
        //            Log.LogSendAfter(STS.BUMMER, "");
        //            return (STS.BUMMER);
        //        }
        //    }

        //    // Uh-oh...
        //    else
        //    {
        //        Log.LogSendAfter(STS.BUMMER, "");
        //        return (STS.BUMMER);
        //    }

        //    // Get DAT_STATUS, if needed...
        //    STS stsRcOrCc = AutoDatStatus(sts);

        //    // Log it...
        //    if (Log.GetLevel() > 0)
        //    {
        //        Log.LogSendAfter(stsRcOrCc, "");
        //    }

        //    // If we had a successful transfer, then convert the data...
        //    if (sts == STS.XFERDONE)
        //    {
        //        if (a_blUseBitmapHandle)
        //        {
        //            a_intptrBitmapHandle = intptrBitmap;
        //        }
        //        else
        //        {
        //            // Bump our state...
        //            m_state = STATE.S7;

        //            // Turn the DIB into a Bitmap object...
        //            a_bitmap = NativeToBitmap(ms_platform, intptrBitmap);

        //            // We're done with the data we got from the driver...
        //            Marshal.FreeHGlobal(intptrBitmap);
        //            intptrBitmap = IntPtr.Zero;
        //        }
        //    }

        //    // All done...
        //    return (stsRcOrCc);
        //}

        /// <summary>
        /// Get/Set JPEG compression tables...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twjpegcompression">JPEGCOMPRESSION structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatJpegcompression(DG a_dg, MSG a_msg, ref TW_JPEGCOMPRESSION a_twjpegcompression)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twjpegcompression = a_twjpegcompression;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.JPEGCOMPRESSION;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twjpegcompression = m_twaincommand.Get(lIndex).twjpegcompression;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.JPEGCOMPRESSION.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryJpegcompression(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.JPEGCOMPRESSION, a_msg, ref a_twjpegcompression);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryJpegcompression(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.JPEGCOMPRESSION, a_msg, ref a_twjpegcompression);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryJpegcompression(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.JPEGCOMPRESSION, a_msg, ref a_twjpegcompression);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryJpegcompression(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.JPEGCOMPRESSION, a_msg, ref a_twjpegcompression);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryJpegcompression(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.JPEGCOMPRESSION, a_msg, ref a_twjpegcompression);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryJpegcompression(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.JPEGCOMPRESSION, a_msg, ref a_twjpegcompression);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryJpegcompression(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.JPEGCOMPRESSION, a_msg, ref a_twjpegcompression);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set metrics...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twmetrics">JPEGCOMPRESSION structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatMetrics(DG a_dg, MSG a_msg, ref TW_METRICS a_twmetrics)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twmetrics = a_twmetrics;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.METRICS;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twmetrics = m_twaincommand.Get(lIndex).twmetrics;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.METRICS.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryMetrics(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.METRICS, a_msg, ref a_twmetrics);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryMetrics(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.METRICS, a_msg, ref a_twmetrics);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryMetrics(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.METRICS, a_msg, ref a_twmetrics);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryMetrics(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.METRICS, a_msg, ref a_twmetrics);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryMetrics(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.METRICS, a_msg, ref a_twmetrics);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryMetrics(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.METRICS, a_msg, ref a_twmetrics);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryMetrics(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.METRICS, a_msg, ref a_twmetrics);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set for a Pallete8...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twpalette8">PALETTE8 structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatPalette8(DG a_dg, MSG a_msg, ref TW_PALETTE8 a_twpalette8)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twpalette8 = a_twpalette8;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.PALETTE8;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twpalette8 = m_twaincommand.Get(lIndex).twpalette8;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.PALETTE8.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryPalette8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PALETTE8, a_msg, ref a_twpalette8);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryPalette8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PALETTE8, a_msg, ref a_twpalette8);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryPalette8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PALETTE8, a_msg, ref a_twpalette8);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryPalette8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PALETTE8, a_msg, ref a_twpalette8);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryPalette8(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.PALETTE8, a_msg, ref a_twpalette8);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryPalette8(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.PALETTE8, a_msg, ref a_twpalette8);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryPalette8(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.PALETTE8, a_msg, ref a_twpalette8);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue DSM commands...
        /// </summary>
        private void DatParentWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatParent.sts = (STS)NativeMethods.WindowsTwain32DsmEntryParent
            (
                ref m_twidentitylegacyApp,
                IntPtr.Zero,
                m_threaddataDatParent.dg,
                m_threaddataDatParent.dat,
                m_threaddataDatParent.msg,
                ref m_threaddataDatParent.intptrHwnd
            );
        }
        private void DatParentWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatParent.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryParent
            (
                ref m_twidentitylegacyApp,
                IntPtr.Zero,
                m_threaddataDatParent.dg,
                m_threaddataDatParent.dat,
                m_threaddataDatParent.msg,
                ref m_threaddataDatParent.intptrHwnd
            );
        }
        public STS DatParent(DG a_dg, MSG a_msg, ref IntPtr a_intptrHwnd)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.intptrHwnd = a_intptrHwnd;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.PARENT;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_intptrHwnd = m_twaincommand.Get(lIndex).intptrHwnd;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.PARENT.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatParent.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.PARENT, a_msg, ref a_intptrHwnd);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.PARENT, a_msg, ref a_intptrHwnd);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatParent = default;
                                m_threaddataDatParent.blIsInuse = true;
                                m_threaddataDatParent.dg = a_dg;
                                m_threaddataDatParent.msg = a_msg;
                                m_threaddataDatParent.dat = DAT.PARENT;
                                m_threaddataDatParent.intptrHwnd = a_intptrHwnd;
                                RunInUiThread(DatParentWindowsTwain32);
                                sts = m_threaddataDatParent.sts;
                                m_threaddataDatParent = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatParent = default;
                                m_threaddataDatParent.blIsInuse = true;
                                m_threaddataDatParent.dg = a_dg;
                                m_threaddataDatParent.msg = a_msg;
                                m_threaddataDatParent.dat = DAT.PARENT;
                                m_threaddataDatParent.intptrHwnd = a_intptrHwnd;
                                RunInUiThread(DatParentWindowsTwainDsm);
                                sts = m_threaddataDatParent.sts;
                                m_threaddataDatParent = default;
                            }
                        }
                    }
                    // Needed for the DF_DSM2 flag...
                    m_twidentityApp.SupportedGroups = m_twidentitylegacyApp.SupportedGroups;
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                STS stsLatest = STS.BUMMER;
                STS sts020302Dsm64bit = STS.BUMMER;

                // We're trying both DSM's...
                sts = STS.BUMMER;

                // Load the new DSM, whatever it is, if we found one...
                if (m_blFoundLatestDsm64)
                {
                    try
                    {
                        stsLatest = (STS)NativeMethods.Linux64DsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.PARENT, a_msg, ref a_intptrHwnd);
                        // Needed for the DF_DSM2 flag...
                        m_twidentityApp.SupportedGroups = m_twidentitylegacyApp.SupportedGroups;
                    }
                    catch
                    {
                        // Forget this...
                        m_blFoundLatestDsm64 = false;
                    }
                }

                // Load the new DSM, whatever it is, if we found one...
                if (m_blFoundLatestDsm)
                {
                    try
                    {
                        stsLatest = (STS)NativeMethods.LinuxDsmEntryParent(ref m_twidentitylegacyApp, IntPtr.Zero, a_dg, DAT.PARENT, a_msg, ref a_intptrHwnd);
                        // Needed for the DF_DSM2 flag...
                        m_twidentityApp.SupportedGroups = m_twidentitylegacyApp.SupportedGroups;
                    }
                    catch
                    {
                        // Forget this...
                        m_blFoundLatestDsm = false;
                    }
                }

                // Load libtwaindsm.so.2.3.2, if we found it...
                if (m_blFound020302Dsm64bit)
                {
                    try
                    {
                        sts020302Dsm64bit = (STS)NativeMethods.Linux020302Dsm64bitEntryParent(ref m_twidentityApp, IntPtr.Zero, a_dg, DAT.PARENT, a_msg, ref a_intptrHwnd);
                    }
                    catch
                    {
                        // Forget this...
                        m_blFound020302Dsm64bit = false;
                    }
                }

                // We only need one success to get through this...
                sts = STS.BUMMER;
                if ((stsLatest == STS.SUCCESS) || (sts020302Dsm64bit == STS.SUCCESS))
                {
                    sts = STS.SUCCESS;
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryParent(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DAT.PARENT, a_msg, ref a_intptrHwnd);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryParent(ref m_twidentitymacosxApp, IntPtr.Zero, a_dg, DAT.PARENT, a_msg, ref a_intptrHwnd);
                    }
                    // Needed for the DF_DSM2 flag...
                    m_twidentityApp.SupportedGroups = m_twidentitymacosxApp.SupportedGroups;
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // If we opened, go to state 3, and start tracking
            // TWAIN's state in the log file...
            if (a_msg == MSG.OPENDSM)
            {
                if (sts == STS.SUCCESS)
                {
                    m_state = STATE.S3;
                    Log.RegisterTwain(this);
                }
            }

            // If we closed, go to state 2, and stop tracking
            // TWAIN's state in the log file...
            else if (a_msg == MSG.CLOSEDSM)
            {
                if (sts == STS.SUCCESS)
                {
                    m_state = STATE.S2;
                    Log.RegisterTwain(null);
                }
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set for a raw commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twpassthru">PASSTHRU structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatPassthru(DG a_dg, MSG a_msg, ref TW_PASSTHRU a_twpassthru)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twpassthru = a_twpassthru;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.PASSTHRU;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twpassthru = m_twaincommand.Get(lIndex).twpassthru;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.PASSTHRU.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryPassthru(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryPassthru(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryPassthru(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryPassthru(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.PASSTHRU, a_msg, ref a_twpassthru);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue pendingxfers commands...
        /// </summary>
        private void DatPendingxfersWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatPendingxfers.sts = (STS)NativeMethods.WindowsTwain32DsmEntryPendingxfers
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatPendingxfers.dg,
                m_threaddataDatPendingxfers.dat,
                m_threaddataDatPendingxfers.msg,
                ref m_threaddataDatPendingxfers.twpendingxfers
            );
        }
        private void DatPendingxfersWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatPendingxfers.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryPendingxfers
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatPendingxfers.dg,
                m_threaddataDatPendingxfers.dat,
                m_threaddataDatPendingxfers.msg,
                ref m_threaddataDatPendingxfers.twpendingxfers
            );
        }
        public STS DatPendingxfers(DG a_dg, MSG a_msg, ref TW_PENDINGXFERS a_twpendingxfers)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twpendingxfers = a_twpendingxfers;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.PENDINGXFERS;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twpendingxfers = m_twaincommand.Get(lIndex).twpendingxfers;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.PENDINGXFERS.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatPendingxfers.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatPendingxfers = default;
                                m_threaddataDatPendingxfers.blIsInuse = true;
                                m_threaddataDatPendingxfers.dg = a_dg;
                                m_threaddataDatPendingxfers.msg = a_msg;
                                m_threaddataDatPendingxfers.dat = DAT.PENDINGXFERS;
                                m_threaddataDatPendingxfers.twpendingxfers = a_twpendingxfers;
                                RunInUiThread(DatPendingxfersWindowsTwain32);
                                a_twpendingxfers = m_threaddataDatPendingxfers.twpendingxfers;
                                sts = m_threaddataDatPendingxfers.sts;
                                m_threaddataDatPendingxfers = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatPendingxfers = default;
                                m_threaddataDatPendingxfers.blIsInuse = true;
                                m_threaddataDatPendingxfers.dg = a_dg;
                                m_threaddataDatPendingxfers.msg = a_msg;
                                m_threaddataDatPendingxfers.dat = DAT.PENDINGXFERS;
                                m_threaddataDatPendingxfers.twpendingxfers = a_twpendingxfers;
                                RunInUiThread(DatPendingxfersWindowsTwainDsm);
                                a_twpendingxfers = m_threaddataDatPendingxfers.twpendingxfers;
                                sts = m_threaddataDatPendingxfers.sts;
                                m_threaddataDatPendingxfers = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryPendingxfers(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryPendingxfers(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryPendingxfers(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryPendingxfers(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.PENDINGXFERS, a_msg, ref a_twpendingxfers);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.PendingxfersToCsv(a_twpendingxfers));
            }

            // If we endxfer, go to state 5 or 6...
            if (a_msg == MSG.ENDXFER)
            {
                if (sts == STS.SUCCESS)
                {
                    if (a_twpendingxfers.Count == 0)
                    {
                        m_blAcceptXferReady = true;
                        m_blIsMsgxferready = false;
                        m_state = STATE.S5;
                    }
                    else
                    {
                        m_state = STATE.S6;
                    }
                }
            }

            // If we reset, go to state 5...
            else if (a_msg == MSG.RESET)
            {
                if (sts == STS.SUCCESS)
                {
                    m_blAcceptXferReady = true;
                    m_blIsMsgxferready = false;
                    m_state = STATE.S5;
                }
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set for RGB response...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twrgbresponse">RGBRESPONSE structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatRgbresponse(DG a_dg, MSG a_msg, ref TW_RGBRESPONSE a_twrgbresponse)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twrgbresponse = a_twrgbresponse;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.RGBRESPONSE;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twrgbresponse = m_twaincommand.Get(lIndex).twrgbresponse;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.RGBRESPONSE.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryRgbresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.RGBRESPONSE, a_msg, ref a_twrgbresponse);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryRgbresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.RGBRESPONSE, a_msg, ref a_twrgbresponse);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryRgbresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.RGBRESPONSE, a_msg, ref a_twrgbresponse);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryRgbresponse(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.RGBRESPONSE, a_msg, ref a_twrgbresponse);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryRgbresponse(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.RGBRESPONSE, a_msg, ref a_twrgbresponse);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryRgbresponse(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.RGBRESPONSE, a_msg, ref a_twrgbresponse);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryRgbresponse(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.RGBRESPONSE, a_msg, ref a_twrgbresponse);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set for a file xfer...
        /// </summary>
        private void DatSetupfilexferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatSetupfilexfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntrySetupfilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatSetupfilexfer.dg,
                m_threaddataDatSetupfilexfer.dat,
                m_threaddataDatSetupfilexfer.msg,
                ref m_threaddataDatSetupfilexfer.twsetupfilexfer
            );
        }
        private void DatSetupfilexferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatSetupfilexfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntrySetupfilexfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatSetupfilexfer.dg,
                m_threaddataDatSetupfilexfer.dat,
                m_threaddataDatSetupfilexfer.msg,
                ref m_threaddataDatSetupfilexfer.twsetupfilexfer
            );
        }
        public STS DatSetupfilexfer(DG a_dg, MSG a_msg, ref TW_SETUPFILEXFER a_twsetupfilexfer)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twsetupfilexfer = a_twsetupfilexfer;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.SETUPFILEXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twsetupfilexfer = m_twaincommand.Get(lIndex).twsetupfilexfer;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.SETUPFILEXFER.ToString(), a_msg.ToString(), CsvSerializer.SetupfilexferToCsv(a_twsetupfilexfer));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatSetupfilexfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupfilexfer = default;
                                m_threaddataDatSetupfilexfer.blIsInuse = true;
                                m_threaddataDatSetupfilexfer.dg = a_dg;
                                m_threaddataDatSetupfilexfer.msg = a_msg;
                                m_threaddataDatSetupfilexfer.dat = DAT.SETUPFILEXFER;
                                m_threaddataDatSetupfilexfer.twsetupfilexfer = a_twsetupfilexfer;
                                RunInUiThread(DatSetupfilexferWindowsTwain32);
                                a_twsetupfilexfer = m_threaddataDatSetupfilexfer.twsetupfilexfer;
                                sts = m_threaddataDatSetupfilexfer.sts;
                                m_threaddataDatSetupfilexfer = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupfilexfer = default;
                                m_threaddataDatSetupfilexfer.blIsInuse = true;
                                m_threaddataDatSetupfilexfer.dg = a_dg;
                                m_threaddataDatSetupfilexfer.msg = a_msg;
                                m_threaddataDatSetupfilexfer.dat = DAT.SETUPFILEXFER;
                                m_threaddataDatSetupfilexfer.twsetupfilexfer = a_twsetupfilexfer;
                                RunInUiThread(DatSetupfilexferWindowsTwainDsm);
                                a_twsetupfilexfer = m_threaddataDatSetupfilexfer.twsetupfilexfer;
                                sts = m_threaddataDatSetupfilexfer.sts;
                                m_threaddataDatSetupfilexfer = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntrySetupfilexfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntrySetupfilexfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntrySetupfilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntrySetupfilexfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.SETUPFILEXFER, a_msg, ref a_twsetupfilexfer);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.SetupfilexferToCsv(a_twsetupfilexfer));
            }

            // All done...
            return (stsRcOrCc);
        }

        private void DatSetupmemxferWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatSetupmemxfer.sts = (STS)NativeMethods.WindowsTwain32DsmEntrySetupmemxfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatSetupmemxfer.dg,
                m_threaddataDatSetupmemxfer.dat,
                m_threaddataDatSetupmemxfer.msg,
                ref m_threaddataDatSetupmemxfer.twsetupmemxfer
            );
        }
        private void DatSetupmemxferWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatSetupmemxfer.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntrySetupmemxfer
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatSetupmemxfer.dg,
                m_threaddataDatSetupmemxfer.dat,
                m_threaddataDatSetupmemxfer.msg,
                ref m_threaddataDatSetupmemxfer.twsetupmemxfer
            );
        }
        public STS DatSetupmemxfer(DG a_dg, MSG a_msg, ref TW_SETUPMEMXFER a_twsetupmemxfer)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twsetupmemxfer = a_twsetupmemxfer;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.SETUPMEMXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twsetupmemxfer = m_twaincommand.Get(lIndex).twsetupmemxfer;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.SETUPMEMXFER.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatSetupmemxfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupmemxfer = default;
                                m_threaddataDatSetupmemxfer.blIsInuse = true;
                                m_threaddataDatSetupmemxfer.dg = a_dg;
                                m_threaddataDatSetupmemxfer.msg = a_msg;
                                m_threaddataDatSetupmemxfer.dat = DAT.SETUPMEMXFER;
                                m_threaddataDatSetupmemxfer.twsetupmemxfer = a_twsetupmemxfer;
                                RunInUiThread(DatSetupmemxferWindowsTwain32);
                                a_twsetupmemxfer = m_threaddataDatSetupmemxfer.twsetupmemxfer;
                                sts = m_threaddataDatSetupmemxfer.sts;
                                m_threaddataDatSetupmemxfer = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatSetupmemxfer = default;
                                m_threaddataDatSetupmemxfer.blIsInuse = true;
                                m_threaddataDatSetupmemxfer.dg = a_dg;
                                m_threaddataDatSetupmemxfer.msg = a_msg;
                                m_threaddataDatSetupmemxfer.dat = DAT.SETUPMEMXFER;
                                m_threaddataDatSetupmemxfer.twsetupmemxfer = a_twsetupmemxfer;
                                RunInUiThread(DatSetupmemxferWindowsTwainDsm);
                                a_twsetupmemxfer = m_threaddataDatSetupmemxfer.twsetupmemxfer;
                                sts = m_threaddataDatSetupmemxfer.sts;
                                m_threaddataDatSetupmemxfer = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntrySetupmemxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntrySetupmemxfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntrySetupmemxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntrySetupmemxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.SETUPMEMXFER, a_msg, ref a_twsetupmemxfer);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.SetupmemxferToCsv(a_twsetupmemxfer));
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get some text for an error...
        /// </summary>
        private void DatStatusWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatStatus.sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatus
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatStatus.dg,
                m_threaddataDatStatus.dat,
                m_threaddataDatStatus.msg,
                ref m_threaddataDatStatus.twstatus
            );
        }
        private void DatStatusWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatStatus.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatus
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatStatus.dg,
                m_threaddataDatStatus.dat,
                m_threaddataDatStatus.msg,
                ref m_threaddataDatStatus.twstatus
            );
        }
        public STS DatStatus(DG a_dg, MSG a_msg, ref TW_STATUS a_twstatus)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twstatus = a_twstatus;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.STATUS;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twstatus = m_twaincommand.Get(lIndex).twstatus;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.STATUS.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatStatus.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUS, a_msg, ref a_twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUS, a_msg, ref a_twstatus);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatStatus = default;
                                m_threaddataDatStatus.blIsInuse = true;
                                m_threaddataDatStatus.dg = a_dg;
                                m_threaddataDatStatus.msg = a_msg;
                                m_threaddataDatStatus.dat = DAT.STATUS;
                                m_threaddataDatStatus.twstatus = a_twstatus;
                                RunInUiThread(DatStatusWindowsTwain32);
                                a_twstatus = m_threaddataDatStatus.twstatus;
                                sts = m_threaddataDatStatus.sts;
                                m_threaddataDatStatus = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatStatus = default;
                                m_threaddataDatStatus.blIsInuse = true;
                                m_threaddataDatStatus.dg = a_dg;
                                m_threaddataDatStatus.msg = a_msg;
                                m_threaddataDatStatus.dat = DAT.STATUS;
                                m_threaddataDatStatus.twstatus = a_twstatus;
                                RunInUiThread(DatStatusWindowsTwainDsm);
                                a_twstatus = m_threaddataDatStatus.twstatus;
                                sts = m_threaddataDatStatus.sts;
                                m_threaddataDatStatus = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUS, a_msg, ref a_twstatus);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUS, a_msg, ref a_twstatus);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryStatus(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.STATUS, a_msg, ref a_twstatus);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryStatus(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.STATUS, a_msg, ref a_twstatus);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryStatus(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.STATUS, a_msg, ref a_twstatus);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Skip getting the status...  :)
            STS stsRcOrCc = sts;

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get some text for an error...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twstatusutf8">STATUSUTF8 structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatStatusutf8(DG a_dg, MSG a_msg, ref TW_STATUSUTF8 a_twstatusutf8)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twstatusutf8 = a_twstatusutf8;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.STATUSUTF8;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twstatusutf8 = m_twaincommand.Get(lIndex).twstatusutf8;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.STATUSUTF8.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatusutf8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUSUTF8, a_msg, ref a_twstatusutf8);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatusutf8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUSUTF8, a_msg, ref a_twstatusutf8);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryStatusutf8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUSUTF8, a_msg, ref a_twstatusutf8);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryStatusutf8(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.STATUSUTF8, a_msg, ref a_twstatusutf8);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryStatusutf8(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.STATUSUTF8, a_msg, ref a_twstatusutf8);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryStatusutf8(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.STATUSUTF8, a_msg, ref a_twstatusutf8);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryStatusutf8(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.STATUSUTF8, a_msg, ref a_twstatusutf8);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get some text for an error...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twtwaindirect">TWAINDIRECT structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatTwaindirect(DG a_dg, MSG a_msg, ref TW_TWAINDIRECT a_twtwaindirect)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twtwaindirect = a_twtwaindirect;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.TWAINDIRECT;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twtwaindirect = m_twaincommand.Get(lIndex).twtwaindirect;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.TWAINDIRECT.ToString(), a_msg.ToString(), "");
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryTwaindirect(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.TWAINDIRECT, a_msg, ref a_twtwaindirect);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryTwaindirect(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.TWAINDIRECT, a_msg, ref a_twtwaindirect);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryTwaindirect(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.TWAINDIRECT, a_msg, ref a_twtwaindirect);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryTwaindirect(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.TWAINDIRECT, a_msg, ref a_twtwaindirect);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryTwaindirect(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.TWAINDIRECT, a_msg, ref a_twtwaindirect);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryTwaindirect(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.TWAINDIRECT, a_msg, ref a_twtwaindirect);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryTwaindirect(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.TWAINDIRECT, a_msg, ref a_twtwaindirect);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // All done...
            return (stsRcOrCc);
        }

        /// <summary>
        /// Issue capabilities commands...
        /// </summary>
        private void DatUserinterfaceWindowsTwain32()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatUserinterface.sts = (STS)NativeMethods.WindowsTwain32DsmEntryUserinterface
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatUserinterface.dg,
                m_threaddataDatUserinterface.dat,
                m_threaddataDatUserinterface.msg,
                ref m_threaddataDatUserinterface.twuserinterface
            );
        }
        private void DatUserinterfaceWindowsTwainDsm()
        {
            // If you get a first chance exception, be aware that some drivers
            // will do that to you, you can ignore it and they'll keep going...
            m_threaddataDatUserinterface.sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryUserinterface
            (
                ref m_twidentitylegacyApp,
                ref m_twidentitylegacyDs,
                m_threaddataDatUserinterface.dg,
                m_threaddataDatUserinterface.dat,
                m_threaddataDatUserinterface.msg,
                ref m_threaddataDatUserinterface.twuserinterface
            );
        }
        public STS DatUserinterface(DG a_dg, MSG a_msg, ref TW_USERINTERFACE a_twuserinterface)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default;
                        threaddata.twuserinterface = a_twuserinterface;
                        threaddata.twuserinterface.hParent = m_intptrHwnd;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DAT.USERINTERFACE;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twuserinterface = m_twaincommand.Get(lIndex).twuserinterface;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.USERINTERFACE.ToString(), a_msg.ToString(), CsvSerializer.UserinterfaceToCsv(a_twuserinterface));
            }

            // We need this to handle data sources that return MSG_XFERREADY in
            // the midst of processing MSG_ENABLEDS, otherwise we'll miss it...
            m_blAcceptXferReady = (a_msg == MSG.ENABLEDS);
            m_blRunningDatUserinterface = (a_msg == MSG.ENABLEDS);

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatUserinterface.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.USERINTERFACE, a_msg, ref a_twuserinterface);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.USERINTERFACE, a_msg, ref a_twuserinterface);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatUserinterface = default;
                                m_threaddataDatUserinterface.blIsInuse = true;
                                m_threaddataDatUserinterface.dg = a_dg;
                                m_threaddataDatUserinterface.msg = a_msg;
                                m_threaddataDatUserinterface.dat = DAT.USERINTERFACE;
                                m_threaddataDatUserinterface.twuserinterface = a_twuserinterface;
                                RunInUiThread(DatUserinterfaceWindowsTwain32);
                                a_twuserinterface = m_threaddataDatUserinterface.twuserinterface;
                                sts = m_threaddataDatUserinterface.sts;
                                m_threaddataDatUserinterface = default;
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatUserinterface = default;
                                m_threaddataDatUserinterface.blIsInuse = true;
                                m_threaddataDatUserinterface.dg = a_dg;
                                m_threaddataDatUserinterface.msg = a_msg;
                                m_threaddataDatUserinterface.dat = DAT.USERINTERFACE;
                                m_threaddataDatUserinterface.twuserinterface = a_twuserinterface;
                                RunInUiThread(DatUserinterfaceWindowsTwainDsm);
                                a_twuserinterface = m_threaddataDatUserinterface.twuserinterface;
                                sts = m_threaddataDatUserinterface.sts;
                                m_threaddataDatUserinterface = default;
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    m_blRunningDatUserinterface = false;
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryUserinterface(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryUserinterface(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    m_blRunningDatUserinterface = false;
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryUserinterface(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryUserinterface(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.USERINTERFACE, a_msg, ref a_twuserinterface);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    m_blRunningDatUserinterface = false;
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                m_blRunningDatUserinterface = false;
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, "");
            }

            // If successful, decide which way to jump...
            if (sts == STS.SUCCESS)
            {
                switch (a_msg)
                {
                    // No clue...
                    default:
                        break;

                    // Jump up...
                    case MSG.ENABLEDS:
                    case MSG.ENABLEDSUIONLY:
                        m_state = STATE.S5;
                        break;

                    // Jump down...
                    case MSG.DISABLEDS:
                        m_blIsMsgclosedsreq = false;
                        m_blIsMsgclosedsok = false;
                        m_state = STATE.S4;
                        break;
                }
            }

            // All done...
            m_blRunningDatUserinterface = false;
            return (stsRcOrCc);
        }

        /// <summary>
        /// Get/Set the Xfer Group...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twuint32">XFERGROUP structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatXferGroup(DG a_dg, MSG a_msg, ref UInt32 a_twuint32)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
            {
                lock (m_lockTwain)
                {
                    // Set our command variables...
                    ThreadData threaddata = default;
                    threaddata.twuint32 = a_twuint32;
                    threaddata.dg = a_dg;
                    threaddata.msg = a_msg;
                    threaddata.dat = DAT.XFERGROUP;
                    long lIndex = m_twaincommand.Submit(threaddata);

                    // Submit the command and wait for the reply...
                    CallerToThreadSet();
                    ThreadToCallerWaitOne();

                    // Return the result...
                    a_twuint32 = m_twaincommand.Get(lIndex).twuint32;
                    sts = m_twaincommand.Get(lIndex).sts;

                    // Clear the command variables...
                    m_twaincommand.Delete(lIndex);
                }
                return (sts);
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DAT.XFERGROUP.ToString(), a_msg.ToString(), CsvSerializer.XfergroupToCsv(a_twuint32));
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.WindowsTwain32DsmEntryXfergroup(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.XFERGROUP, a_msg, ref a_twuint32);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryXfergroup(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.XFERGROUP, a_msg, ref a_twuint32);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryXfergroup(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.XFERGROUP, a_msg, ref a_twuint32);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryXfergroup(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DAT.XFERGROUP, a_msg, ref a_twuint32);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryXfergroup(ref m_twidentityApp, ref m_twidentityDs, a_dg, DAT.XFERGROUP, a_msg, ref a_twuint32);
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryXfergroup(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.XFERGROUP, a_msg, ref a_twuint32);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryXfergroup(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DAT.XFERGROUP, a_msg, ref a_twuint32);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, CsvSerializer.XfergroupToCsv(a_twuint32));
            }

            // All done...
            return (stsRcOrCc);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Public Definitions, this is where you get the callback definitions for
        // handling device events and scanning...
        ///////////////////////////////////////////////////////////////////////////////
        #region Public Definitions...

        /// <summary>
        /// The form of the device event callback used by the caller when they are
        /// running in states 4, 5, 6 and 7...
        /// </summary>
        /// <returns></returns>
        public delegate STS DeviceEventCallback();

        /// <summary>
        /// The form of the callback used by the caller when they are running
        /// in states 5, 6 and 7; anything after DG_CONTROL / DAT_USERINTERFACE /
        /// MSG_ENABLEDS* until DG_CONTROL / DAT_USERINTERFACE / MSG_DISABLEDS...
        /// </summary>
        /// <returns></returns>
        public delegate STS ScanCallback(bool a_blClosing);

        /// <summary>
        /// We use this to run code in the context of the caller's UI thread...
        /// </summary>
        /// <param name="a_action">code to run</param>
        public delegate void RunInUiThreadDelegate(Action a_action);

        /// <summary>
        /// Only one DSM can be installed on a Linux system, and it must match
        /// the architecture.  To handle the legacy problem due to the bad
        /// definition of TW_INT32/TW_UINT32, we also support access to the
        /// 64-bit 2.3.2 DSM.  This is only used if we see a driver that seems
        /// to be returning garbage for its TW_IDENTITY...
        /// </summary>
        public enum LinuxDsm
        {
            Unknown,
            IsLatestDsm,
            Is020302Dsm64bit
        }

        #endregion




        ///////////////////////////////////////////////////////////////////////////////
        // Private Functions, the main thread is in here...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Functions...

        /// <summary>
        /// Cleanup...
        /// </summary>
        /// <param name="a_blDisposing">true if we need to clean up managed resources</param>
        internal void Dispose(bool a_blDisposing)
        {
            // Free managed resources...
            if (a_blDisposing)
            {
                // Make sure we've closed any drivers...
                Rollback(STATE.S2);

                // Make sure that our thread is gone...
                if (m_threadTwain != null)
                {
                    m_threadTwain.Join();
                    m_threadTwain = null;
                }

                // This one too...
                if (m_threadRunningDatUserinterface != null)
                {
                    m_threadRunningDatUserinterface.Join();
                    m_threadRunningDatUserinterface = null;
                }

                // Clean up our communication thingy...
                m_twaincommand = null;

                // Cleanup...
                if (m_autoreseteventCaller != null)
                {
                    m_autoreseteventCaller.Close();
                    m_autoreseteventCaller = null;
                }
                if (m_autoreseteventThread != null)
                {
                    m_autoreseteventThread.Close();
                    m_autoreseteventThread = null;
                }
                if (m_autoreseteventRollback != null)
                {
                    m_autoreseteventRollback.Close();
                    m_autoreseteventRollback = null;
                }
                if (m_autoreseteventThreadStarted != null)
                {
                    m_autoreseteventThreadStarted.Close();
                    m_autoreseteventThreadStarted = null;
                }
            }
        }

        /// <summary>
        /// This is our main loop where we issue commands to the TWAIN
        /// object on behalf of the caller.  This function runs in its
        /// own thread...
        /// </summary>
        private void Main()
        {
            bool blRunning;
            bool blScanning;
            long lIndex;
            ThreadData threaddata;

            // Okay, we're ready to run...
            m_autoreseteventThreadStarted.Set();
            Log.Info("main>>> thread started...");

            //
            // We have three different ways of driving the TWAIN driver...
            //
            // First, we can accept a direct command from the user for commands
            // that move from state 2 to state 4, and for any commands that are
            // issued in state 4.
            //
            // Second, we have a scanning callback function, that operates when
            // we are transferring images; this means that we don't want the
            // user making those calls directly.
            //
            // Third, we have a rollback function, that allows the calls to
            // move anywhere from state 7 to state 2; what this means is that we
            // don't want the user making those calls directly.
            //
            // The trick is to move smoothly between these three styles of
            // access, and what we find is that the first and second are pretty
            // easy to do, but the third one is tricky...
            //
            blRunning = true;
            blScanning = false;
            while (blRunning)
            {
                // Get the next item, if we don't have anything, then we may
                // need to wait...
                if (!m_twaincommand.GetNext(out lIndex, out threaddata))
                {
                    // If we're not scanning, then wait for a command to wake
                    // us up...
                    if (!blScanning)
                    {
                        CallerToThreadWaitOne();
                        m_twaincommand.GetNext(out lIndex, out threaddata);
                    }
                }

                // Leave...now...
                if (threaddata.blExitThread)
                {
                    m_twaincommand.Complete(lIndex, threaddata);
                    m_scancallback(true);
                    ThreadToCallerSet();
                    ThreadToRollbackSet();
                    return;
                }

                // Process device events...
                if (IsMsgDeviceEvent())
                {
                    m_deviceeventcallback();
                }

                // We don't have a direct command, it's either a rollback request,
                // a request to run the scan callback, or its a false positive,
                // which we can safely ignore...
                if (threaddata.dat == default(DAT))
                {
                    // The caller has asked us to rollback the state machine...
                    if (threaddata.blRollback)
                    {
                        threaddata.blRollback = false;
                        Rollback(threaddata.stateRollback);
                        blScanning = (threaddata.stateRollback >= STATE.S5);
                        blRunning = (threaddata.stateRollback > STATE.S2);
                        if (!blRunning)
                        {
                            m_scancallback(true);
                        }
                        ThreadToRollbackSet();
                    }

                    // Callback stuff here between MSG_ENABLEDS* and MSG_DISABLEDS...
                    else if (GetState() >= STATE.S5)
                    {
                        m_scancallback(false);
                        blScanning = true;
                    }

                    // We're done scanning...
                    else
                    {
                        blScanning = false;
                    }

                    // Tag the command as complete...
                    m_twaincommand.Complete(lIndex, threaddata);

                    // Go back to the top...
                    continue;
                }

                // Otherwise, directly issue the command...
                switch (threaddata.dat)
                {
                    // Unrecognized DAT...
                    default:
                        if (m_state < STATE.S4)
                        {
                            threaddata.sts = DsmEntryNullDest(threaddata.dg, threaddata.dat, threaddata.msg, threaddata.twmemref);
                        }
                        else
                        {
                            threaddata.sts = DsmEntry(threaddata.dg, threaddata.dat, threaddata.msg, threaddata.twmemref);
                        }
                        break;

                    // Audio file xfer...
                    case DAT.AUDIOFILEXFER:
                        threaddata.sts = DatAudiofilexfer(threaddata.dg, threaddata.msg);
                        break;

                    // Audio info...
                    case DAT.AUDIOINFO:
                        threaddata.sts = DatAudioinfo(threaddata.dg, threaddata.msg, ref threaddata.twaudioinfo);
                        break;

                    // Audio native xfer...
                    case DAT.AUDIONATIVEXFER:
                        threaddata.sts = DatAudionativexfer(threaddata.dg, threaddata.msg, ref threaddata.intptrAudio);
                        break;

                    // Negotiation commands...
                    case DAT.CAPABILITY:
                        threaddata.sts = DatCapability(threaddata.dg, threaddata.msg, ref threaddata.twcapability);
                        break;

                    // CIE color...
                    case DAT.CIECOLOR:
                        threaddata.sts = DatCiecolor(threaddata.dg, threaddata.msg, ref threaddata.twciecolor);
                        break;

                    // Snapshots...
                    case DAT.CUSTOMDSDATA:
                        threaddata.sts = DatCustomdsdata(threaddata.dg, threaddata.msg, ref threaddata.twcustomdsdata);
                        break;

                    // Functions...
                    case DAT.ENTRYPOINT:
                        threaddata.sts = DatEntrypoint(threaddata.dg, threaddata.msg, ref threaddata.twentrypoint);
                        break;

                    // Image meta data...
                    case DAT.EXTIMAGEINFO:
                        threaddata.sts = DatExtimageinfo(threaddata.dg, threaddata.msg, ref threaddata.twextimageinfo);
                        break;

                    // Filesystem...
                    case DAT.FILESYSTEM:
                        threaddata.sts = DatFilesystem(threaddata.dg, threaddata.msg, ref threaddata.twfilesystem);
                        break;

                    // Filter...
                    case DAT.FILTER:
                        threaddata.sts = DatFilter(threaddata.dg, threaddata.msg, ref threaddata.twfilter);
                        break;

                    // Grayscale...
                    case DAT.GRAYRESPONSE:
                        threaddata.sts = DatGrayresponse(threaddata.dg, threaddata.msg, ref threaddata.twgrayresponse);
                        break;

                    // ICC color profiles...
                    case DAT.ICCPROFILE:
                        threaddata.sts = DatIccprofile(threaddata.dg, threaddata.msg, ref threaddata.twmemory);
                        break;

                    // Enumerate and Open commands...
                    case DAT.IDENTITY:
                        threaddata.sts = DatIdentity(threaddata.dg, threaddata.msg, ref threaddata.twidentity);
                        break;

                    // More meta data...
                    case DAT.IMAGEINFO:
                        threaddata.sts = DatImageinfo(threaddata.dg, threaddata.msg, ref threaddata.twimageinfo);
                        break;

                    // File xfer...
                    case DAT.IMAGEFILEXFER:
                        threaddata.sts = DatImagefilexfer(threaddata.dg, threaddata.msg);
                        break;

                    // Image layout commands...
                    case DAT.IMAGELAYOUT:
                        threaddata.sts = DatImagelayout(threaddata.dg, threaddata.msg, ref threaddata.twimagelayout);
                        break;

                    // Memory file transfer (yes, we're using TW_IMAGEMEMXFER, that's okay)...
                    case DAT.IMAGEMEMFILEXFER:
                        threaddata.sts = DatImagememfilexfer(threaddata.dg, threaddata.msg, ref threaddata.twimagememxfer);
                        break;

                    // Memory transfer...
                    case DAT.IMAGEMEMXFER:
                        threaddata.sts = DatImagememxfer(threaddata.dg, threaddata.msg, ref threaddata.twimagememxfer);
                        break;

                    // Native transfer...
                    // TODO: Recode later
                    //case DAT.IMAGENATIVEXFER:
                    //    if (threaddata.blUseBitmapHandle)
                    //    {
                    //        threaddata.sts = DatImagenativexferHandle(threaddata.dg, threaddata.msg, ref threaddata.intptrBitmap);
                    //    }
                    //    else
                    //    {
                    //        threaddata.sts = DatImagenativexfer(threaddata.dg, threaddata.msg, ref threaddata.bitmap);
                    //    }
                    //    break;

                    // JPEG compression...
                    case DAT.JPEGCOMPRESSION:
                        threaddata.sts = DatJpegcompression(threaddata.dg, threaddata.msg, ref threaddata.twjpegcompression);
                        break;

                    // Metrics...
                    case DAT.METRICS:
                        threaddata.sts = DatMetrics(threaddata.dg, threaddata.msg, ref threaddata.twmetrics);
                        break;

                    // Palette8...
                    case DAT.PALETTE8:
                        threaddata.sts = DatPalette8(threaddata.dg, threaddata.msg, ref threaddata.twpalette8);
                        break;

                    // DSM commands...
                    case DAT.PARENT:
                        threaddata.sts = DatParent(threaddata.dg, threaddata.msg, ref threaddata.intptrHwnd);
                        break;

                    // Raw commands...
                    case DAT.PASSTHRU:
                        threaddata.sts = DatPassthru(threaddata.dg, threaddata.msg, ref threaddata.twpassthru);
                        break;

                    // Pending transfers...
                    case DAT.PENDINGXFERS:
                        threaddata.sts = DatPendingxfers(threaddata.dg, threaddata.msg, ref threaddata.twpendingxfers);
                        break;

                    // RGB...
                    case DAT.RGBRESPONSE:
                        threaddata.sts = DatRgbresponse(threaddata.dg, threaddata.msg, ref threaddata.twrgbresponse);
                        break;

                    // Setup file transfer...
                    case DAT.SETUPFILEXFER:
                        threaddata.sts = DatSetupfilexfer(threaddata.dg, threaddata.msg, ref threaddata.twsetupfilexfer);
                        break;

                    // Get memory info...
                    case DAT.SETUPMEMXFER:
                        threaddata.sts = DatSetupmemxfer(threaddata.dg, threaddata.msg, ref threaddata.twsetupmemxfer);
                        break;

                    // Status...
                    case DAT.STATUS:
                        threaddata.sts = DatStatus(threaddata.dg, threaddata.msg, ref threaddata.twstatus);
                        break;

                    // Status text...
                    case DAT.STATUSUTF8:
                        threaddata.sts = DatStatusutf8(threaddata.dg, threaddata.msg, ref threaddata.twstatusutf8);
                        break;

                    // TWAIN Direct...
                    case DAT.TWAINDIRECT:
                        threaddata.sts = DatTwaindirect(threaddata.dg, threaddata.msg, ref threaddata.twtwaindirect);
                        break;

                    // Scan and GUI commands...
                    case DAT.USERINTERFACE:
                        threaddata.sts = DatUserinterface(threaddata.dg, threaddata.msg, ref threaddata.twuserinterface);
                        if (threaddata.sts == STS.SUCCESS)
                        {
                            if ((threaddata.dg == DG.CONTROL) && (threaddata.dat == DAT.USERINTERFACE) && (threaddata.msg == MSG.DISABLEDS))
                            {
                                blScanning = false;
                            }
                            else if ((threaddata.dg == DG.CONTROL) && (threaddata.dat == DAT.USERINTERFACE) && (threaddata.msg == MSG.DISABLEDS))
                            {
                                if (threaddata.twuserinterface.ShowUI == 0)
                                {
                                    blScanning = true;
                                }
                            }
                        }
                        break;

                    // Transfer group...
                    case DAT.XFERGROUP:
                        threaddata.sts = DatXferGroup(threaddata.dg, threaddata.msg, ref threaddata.twuint32);
                        break;
                }

                // Report to the caller that we're done, and loop back up for another...
                m_twaincommand.Complete(lIndex, threaddata);
                ThreadToCallerSet();
            }

            // Some insurance to make sure we loosen up the caller...
            m_scancallback(true);
            ThreadToCallerSet();
            return;
        }

        /// <summary>
        /// Use an event message to set the appropriate flags...
        /// </summary>
        /// <param name="a_msg">Message to process</param>
        private void ProcessEvent(MSG a_msg)
        {
            switch (a_msg)
            {
                // Do nothing...
                default:
                    break;

                // If we're in state 5, then go to state 6...
                case MSG.XFERREADY:
                    if (m_blAcceptXferReady)
                    {
                        // Protect us from driver's that spam this event...
                        m_blAcceptXferReady = false;

                        // We're still processing DAT_USERINTERFACE, that's a kick the
                        // teeth.  We can't wait for it here, so launch a thread to wait
                        // for it to finish, so we can go to the next state as soon as
                        // it's done...
                        if (m_blRunningDatUserinterface)
                        {
                            m_threadRunningDatUserinterface = new Thread(RunningDatUserinterface);
                            m_threadRunningDatUserinterface.Start();
                            return;
                        }

                        // Change our state...
                        m_state = STATE.S6;
                        m_blIsMsgxferready = true;
                        CallerToThreadSet();

                        // Kick off the scan engine...
                        if (m_scancallback != null)
                        {
                            m_scancallback(false);
                        }
                    }
                    break;

                // The cancel button was pressed...
                case MSG.CLOSEDSREQ:
                    m_blIsMsgclosedsreq = true;
                    CallerToThreadSet();
                    if (m_scancallback != null)
                    {
                        m_scancallback(false);
                    }
                    break;

                // The OK button was pressed...
                case MSG.CLOSEDSOK:
                    m_blIsMsgclosedsok = true;
                    CallerToThreadSet();
                    if (m_scancallback != null)
                    {
                        m_scancallback(false);
                    }
                    break;

                // A device event arrived...
                case MSG.DEVICEEVENT:
                    m_blIsMsgdeviceevent = true;
                    CallerToThreadSet();
                    break;
            }
        }

        /// <summary>
        /// As long as DAT_USERINTERFACE is running we'll spin here...
        /// </summary>
        private void RunningDatUserinterface()
        {
            // Wait until something kicks us out...
            while ((m_state >= STATE.S4) && m_blRunningDatUserinterface)
            {
                Thread.Sleep(20);
            }

            // If we never made it to state 5, then bail...
            if (m_state < STATE.S5)
            {
                return;
            }

            // Bump up our state...
            m_state = STATE.S6;
            m_blIsMsgxferready = true;
            CallerToThreadSet();

            // Kick off the scan engine...
            if (m_scancallback != null)
            {
                m_scancallback(false);
            }
        }

        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="a_action">the code to run</param>
        private void RunInUiThread(Action a_action)
        {
            m_runinuithreaddelegate(a_action);
        }

        /// <summary>
        /// The caller is asking the thread to wake-up...
        /// </summary>
        private void CallerToThreadSet()
        {
            m_autoreseteventCaller.Set();
        }

        /// <summary>
        /// The thread is waiting for the caller to wake it...
        /// </summary>
        private bool CallerToThreadWaitOne()
        {
            return (m_autoreseteventCaller.WaitOne());
        }

        /// <summary>
        /// The common start to every capability csv...
        /// </summary>
        /// <param name="a_cap">Capability number</param>
        /// <param name="a_twon">Container</param>
        /// <param name="a_twty">Data type</param>
        /// <returns></returns>
        private CSV Common(CAP a_cap, TWON a_twon, TWTY a_twty)
        {
            CSV csv = new CSV();

            // Add the capability...
            string szCap = a_cap.ToString();
            if (!szCap.Contains("_"))
            {
                szCap = "0x" + ((ushort)a_cap).ToString("X");
            }

            // Build the CSV...
            csv.Add(szCap);
            csv.Add("TWON_" + a_twon);
            csv.Add("TWTY_" + a_twty);

            // And return it...
            return (csv);
        }

        /// <summary>
        /// Has a device event arrived?  Make sure to clear it, because
        /// we can get many of these.  We don't have to worry about a
        /// race condition, because the caller is expected to drain the
        /// driver of all events.
        /// </summary>
        /// <returns>True if a device event is pending</returns>
        private bool IsMsgDeviceEvent()
        {
            if (m_blIsMsgdeviceevent)
            {
                m_blIsMsgdeviceevent = false;
                return (true);
            }
            return (false);
        }

        /// <summary>
        /// The thread is asking the caller to wake-up...
        /// </summary>
        private void ThreadToCallerSet()
        {
            m_autoreseteventThread.Set();
        }

        /// <summary>
        /// The caller is waiting for the thread to wake it...
        /// </summary>
        /// <returns>Result of the wait</returns>
        private bool ThreadToCallerWaitOne()
        {
            return (m_autoreseteventThread.WaitOne());
        }

        /// <summary>
        /// The thread is asking the rollback to wake-up...
        /// </summary>
        private void ThreadToRollbackSet()
        {
            m_autoreseteventRollback.Set();
        }

        /// <summary>
        /// The rollback is waiting for the thread to wake it...
        /// </summary>
        /// <returns>Result of the wait</returns>
        private bool ThreadToRollbackWaitOne()
        {
            return (m_autoreseteventRollback.WaitOne());
        }

        /// <summary>
        /// Automatically collect the condition code for TWRC_FAILURE's...
        /// </summary>
        /// <param name="a_sts">The return code from the last operation</param>
        /// <returns>The final statue return</returns>
        private STS AutoDatStatus(STS a_sts)
        {
            STS sts;
            TW_STATUS twstatus = new TW_STATUS();

            // Automatic system is off, or the status is not TWRC_FAILURE, so just return the status we got...
            if (!m_blAutoDatStatus || (a_sts != STS.FAILURE))
            {
                return (a_sts);
            }

            // Windows...
            if (PlatformInfo.IsWindows)
            {
                // Issue the command...
                try
                {
                    if (m_blUseLegacyDSM)
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                    }
                    else
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    TWAINWorkingGroup.Log.Error("Driver crash...");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (PlatformInfo.IsLinux)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.Linux64DsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.Linux64DsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.LinuxDsmEntryStatusState3(ref m_twidentitylegacyApp, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.LinuxDsmEntryStatus(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        if (GetState() <= STATE.S3)
                        {
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryStatusState3(ref m_twidentityApp, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.Linux020302Dsm64bitEntryStatus(ref m_twidentityApp, ref m_twidentityDs, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                        }
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    TWAINWorkingGroup.Log.Error("Driver crash...");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (PlatformInfo.IsMacOSX)
            {
                // Issue the command...
                try
                {
                    if (GetState() <= STATE.S3)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryStatusState3(ref m_twidentitymacosxApp, IntPtr.Zero, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryStatus(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, DG.CONTROL, DAT.STATUS, MSG.GET, ref twstatus);
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    TWAINWorkingGroup.Log.Error("Driver crash...");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                TWAINWorkingGroup.Log.Assert("Unsupported platform..." + Environment.OSVersion.Platform);
                return (STS.BUMMER);
            }

            // Uh-oh, the status call failed...
            if (sts != STS.SUCCESS)
            {
                return (a_sts);
            }

            // All done...
            return ((STS)(Consts.STSCC + twstatus.ConditionCode));
        }

        /// <summary>
        /// Convert the contents of a capability to a string that we can show in
        /// our simple GUI...
        /// </summary>
        /// <param name="a_twcapability"></param>
        /// <param name="a_twty">Data type</param>
        /// <param name="a_intptr">Pointer to the data</param>
        /// <param name="a_iIndex">Index of the item in the data</param>
        /// <returns>Data in CSV form</returns>
        public string GetIndexedItem(TW_CAPABILITY a_twcapability, TWTY a_twty, IntPtr a_intptr, int a_iIndex)
        {
            IntPtr intptr;

            // Index by type...
            switch (a_twty)
            {
                default:
                    return ("Get Capability: (unrecognized item type)..." + a_twty);

                case TWTY.INT8:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                        sbyte i8Value = (sbyte)Marshal.PtrToStructure(intptr, typeof(sbyte));
                        return (i8Value.ToString());
                    }

                case TWTY.INT16:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                        short i16Value = (short)Marshal.PtrToStructure(intptr, typeof(short));
                        return (i16Value.ToString());
                    }

                case TWTY.INT32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        int i32Value = (int)Marshal.PtrToStructure(intptr, typeof(int));
                        return (i32Value.ToString());
                    }

                case TWTY.UINT8:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                        byte u8Value = (byte)Marshal.PtrToStructure(intptr, typeof(byte));
                        return (u8Value.ToString());
                    }

                case TWTY.BOOL:
                case TWTY.UINT16:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                        ushort u16Value = (ushort)Marshal.PtrToStructure(intptr, typeof(ushort));
                        return (u16Value.ToString());
                    }

                case TWTY.UINT32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        uint u32Value = (uint)Marshal.PtrToStructure(intptr, typeof(uint));
                        return (u32Value.ToString());
                    }

                case TWTY.FIX32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        TW_FIX32 twfix32 = (TW_FIX32)Marshal.PtrToStructure(intptr, typeof(TW_FIX32));
                        return (((double)twfix32.Whole + ((double)twfix32.Frac / 65536.0)).ToString());
                    }

                case TWTY.FRAME:
                    {
                        CSV csv = new CSV();
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(16 * a_iIndex));
                        TW_FRAME twframe = (TW_FRAME)Marshal.PtrToStructure(intptr, typeof(TW_FRAME));
                        csv.Add(((double)twframe.Left.Whole + ((double)twframe.Left.Frac / 65536.0)).ToString());
                        csv.Add(((double)twframe.Top.Whole + ((double)twframe.Top.Frac / 65536.0)).ToString());
                        csv.Add(((double)twframe.Right.Whole + ((double)twframe.Right.Frac / 65536.0)).ToString());
                        csv.Add(((double)twframe.Bottom.Whole + ((double)twframe.Bottom.Frac / 65536.0)).ToString());
                        return (csv.Get());
                    }

                case TWTY.STR32:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(34 * a_iIndex));
                        TW_STR32 twstr32 = (TW_STR32)Marshal.PtrToStructure(intptr, typeof(TW_STR32));
                        return (twstr32.Get());
                    }

                case TWTY.STR64:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(66 * a_iIndex));
                        TW_STR64 twstr64 = (TW_STR64)Marshal.PtrToStructure(intptr, typeof(TW_STR64));
                        return (twstr64.Get());
                    }

                case TWTY.STR128:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(130 * a_iIndex));
                        TW_STR128 twstr128 = (TW_STR128)Marshal.PtrToStructure(intptr, typeof(TW_STR128));
                        return (twstr128.Get());
                    }

                case TWTY.STR255:
                    {
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(256 * a_iIndex));
                        TW_STR255 twstr255 = (TW_STR255)Marshal.PtrToStructure(intptr, typeof(TW_STR255));
                        return (twstr255.Get());
                    }
            }
        }

        /// <summary>
        /// Convert the value of a string into a capability...
        /// </summary>
        /// <param name="a_twcapability">All info on the capability</param>
        /// <param name="a_twty">Data type</param>
        /// <param name="a_intptr">Point to the data</param>
        /// <param name="a_iIndex">Index for item in the data</param>
        /// <param name="a_szValue">CSV value to be used to set the data</param>
        /// <returns>Empty string or an error string</returns>
        public string SetIndexedItem(TW_CAPABILITY a_twcapability, TWTY a_twty, IntPtr a_intptr, int a_iIndex, string a_szValue)
        {
            IntPtr intptr;

            // Index by type...
            switch (a_twty)
            {
                default:
                    return ("Set Capability: (unrecognized item type)..." + a_twty);

                case TWTY.INT8:
                    {
                        // We do this to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            int i32Value = sbyte.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(i32Value, a_intptr, true);
                            return ("");
                        }
                        // These items have to be packed on the type sizes...
                        else
                        {
                            sbyte i8Value = sbyte.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                            Marshal.StructureToPtr(i8Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.INT16:
                    {
                        // We use i32Value to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            int i32Value = short.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(i32Value, a_intptr, true);
                            return ("");
                        }
                        // These items have to be packed on the type sizes...
                        else
                        {
                            short i16Value = short.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                            Marshal.StructureToPtr(i16Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.INT32:
                    {
                        // Entire value will always be overwritten, so we don't have to get fancy...
                        int i32Value = int.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        Marshal.StructureToPtr(i32Value, intptr, true);
                        return ("");
                    }

                case TWTY.UINT8:
                    {
                        // We use u32Value to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            uint u32Value = byte.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(u32Value, a_intptr, true);
                            return ("");
                        }
                        // These items have to be packed on the type sizes...
                        else
                        {
                            byte u8Value = byte.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(1 * a_iIndex));
                            Marshal.StructureToPtr(u8Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.BOOL:
                case TWTY.UINT16:
                    {
                        // We use u32Value to make sure the entire Item value is overwritten...
                        if (a_twcapability.ConType == TWON.ONEVALUE)
                        {
                            uint u32Value = ushort.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            Marshal.StructureToPtr(u32Value, a_intptr, true);
                            return ("");
                        }
                        else
                        {
                            ushort u16Value = ushort.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                            intptr = (IntPtr)((ulong)a_intptr + (ulong)(2 * a_iIndex));
                            Marshal.StructureToPtr(u16Value, intptr, true);
                            return ("");
                        }
                    }

                case TWTY.UINT32:
                    {
                        // Entire value will always be overwritten, so we don't have to get fancy...
                        uint u32Value = uint.Parse(CsvSerializer.CvtCapValueFromEnum(a_twcapability.Cap, a_szValue));
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        Marshal.StructureToPtr(u32Value, intptr, true);
                        return ("");
                    }

                case TWTY.FIX32:
                    {
                        // Entire value will always be overwritten, so we don't have to get fancy...
                        TW_FIX32 twfix32 = default;
                        twfix32.Whole = (short)Convert.ToDouble(a_szValue);
                        twfix32.Frac = (ushort)((Convert.ToDouble(a_szValue) - (double)twfix32.Whole) * 65536.0);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(4 * a_iIndex));
                        Marshal.StructureToPtr(twfix32, intptr, true);
                        return ("");
                    }

                case TWTY.FRAME:
                    {
                        TW_FRAME twframe = default;
                        string[] asz = CSV.Parse(a_szValue);
                        twframe.Left.Whole = (short)Convert.ToDouble(asz[0]);
                        twframe.Left.Frac = (ushort)((Convert.ToDouble(asz[0]) - (double)twframe.Left.Whole) * 65536.0);
                        twframe.Top.Whole = (short)Convert.ToDouble(asz[1]);
                        twframe.Top.Frac = (ushort)((Convert.ToDouble(asz[1]) - (double)twframe.Top.Whole) * 65536.0);
                        twframe.Right.Whole = (short)Convert.ToDouble(asz[2]);
                        twframe.Right.Frac = (ushort)((Convert.ToDouble(asz[2]) - (double)twframe.Right.Whole) * 65536.0);
                        twframe.Bottom.Whole = (short)Convert.ToDouble(asz[3]);
                        twframe.Bottom.Frac = (ushort)((Convert.ToDouble(asz[3]) - (double)twframe.Bottom.Whole) * 65536.0);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(16 * a_iIndex));
                        Marshal.StructureToPtr(twframe, intptr, true);
                        return ("");
                    }

                case TWTY.STR32:
                    {
                        TW_STR32 twstr32 = default;
                        twstr32.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(34 * a_iIndex));
                        Marshal.StructureToPtr(twstr32, intptr, true);
                        return ("");
                    }

                case TWTY.STR64:
                    {
                        TW_STR64 twstr64 = default;
                        twstr64.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(66 * a_iIndex));
                        Marshal.StructureToPtr(twstr64, intptr, true);
                        return ("");
                    }

                case TWTY.STR128:
                    {
                        TW_STR128 twstr128 = default;
                        twstr128.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(130 * a_iIndex));
                        Marshal.StructureToPtr(twstr128, intptr, true);
                        return ("");
                    }

                case TWTY.STR255:
                    {
                        TW_STR255 twstr255 = default;
                        twstr255.Set(a_szValue);
                        intptr = (IntPtr)((ulong)a_intptr + (ulong)(256 * a_iIndex));
                        Marshal.StructureToPtr(twstr255, intptr, true);
                        return ("");
                    }
            }
        }

        /// <summary>
        /// Convert strings into a range...
        /// </summary>
        /// <param name="a_twty">Data type</param>
        /// <param name="a_intptr">Pointer to the data</param>
        /// <param name="a_asz">List of strings</param>
        /// <returns>Empty string or an error string</returns>
        public string SetRangeItem(TWTY a_twty, IntPtr a_intptr, string[] a_asz)
        {
            TW_RANGE twrange = default;
            TW_RANGE_MACOSX twrangemacosx = default;
            TW_RANGE_LINUX64 twrangelinux64 = default;
            TW_RANGE_FIX32 twrangefix32 = default;
            TW_RANGE_FIX32_MACOSX twrangefix32macosx = default;

            // Index by type...
            switch (a_twty)
            {
                default:
                    return ("Set Capability: (unrecognized item type)..." + a_twty);

                case TWTY.INT8:
                    {
                        if (PlatformInfo.IsMacOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)sbyte.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)sbyte.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)sbyte.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)sbyte.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)sbyte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)sbyte.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)sbyte.Parse(a_asz[4]);
                            twrange.StepSize = (uint)sbyte.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)sbyte.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)sbyte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)sbyte.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)sbyte.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)sbyte.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)sbyte.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)sbyte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.INT16:
                    {
                        if (PlatformInfo.IsMacOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)short.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)short.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)short.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)short.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)short.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)short.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)short.Parse(a_asz[4]);
                            twrange.StepSize = (uint)short.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)short.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)short.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)short.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)short.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)short.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)short.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)short.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.INT32:
                    {
                        if (PlatformInfo.IsMacOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)int.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)int.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)int.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)int.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)int.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)int.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)int.Parse(a_asz[4]);
                            twrange.StepSize = (uint)int.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)int.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)int.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)int.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)int.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)int.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)int.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)int.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.UINT8:
                    {
                        if (PlatformInfo.IsMacOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)byte.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)byte.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)byte.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)byte.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)byte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)byte.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)byte.Parse(a_asz[4]);
                            twrange.StepSize = (uint)byte.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)byte.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)byte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)byte.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)byte.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)byte.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)byte.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)byte.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.BOOL:
                case TWTY.UINT16:
                    {
                        if (PlatformInfo.IsMacOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = (uint)ushort.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = (uint)ushort.Parse(a_asz[4]);
                            twrangemacosx.StepSize = (uint)ushort.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = (uint)ushort.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = (uint)ushort.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = (uint)ushort.Parse(a_asz[3]);
                            twrange.MaxValue = (uint)ushort.Parse(a_asz[4]);
                            twrange.StepSize = (uint)ushort.Parse(a_asz[5]);
                            twrange.DefaultValue = (uint)ushort.Parse(a_asz[6]);
                            twrange.CurrentValue = (uint)ushort.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = (uint)ushort.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = (uint)ushort.Parse(a_asz[4]);
                            twrangelinux64.StepSize = (uint)ushort.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = (uint)ushort.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = (uint)ushort.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.UINT32:
                    {
                        if (PlatformInfo.IsMacOSX)
                        {
                            twrangemacosx.ItemType = (uint)a_twty;
                            twrangemacosx.MinValue = uint.Parse(a_asz[3]);
                            twrangemacosx.MaxValue = uint.Parse(a_asz[4]);
                            twrangemacosx.StepSize = uint.Parse(a_asz[5]);
                            twrangemacosx.DefaultValue = uint.Parse(a_asz[6]);
                            twrangemacosx.CurrentValue = uint.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangemacosx, a_intptr, true);
                        }
                        else if ((m_linuxdsm == LinuxDsm.Unknown) || (m_linuxdsm == LinuxDsm.IsLatestDsm))
                        {
                            twrange.ItemType = a_twty;
                            twrange.MinValue = uint.Parse(a_asz[3]);
                            twrange.MaxValue = uint.Parse(a_asz[4]);
                            twrange.StepSize = uint.Parse(a_asz[5]);
                            twrange.DefaultValue = uint.Parse(a_asz[6]);
                            twrange.CurrentValue = uint.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrange, a_intptr, true);
                        }
                        else
                        {
                            twrangelinux64.ItemType = a_twty;
                            twrangelinux64.MinValue = uint.Parse(a_asz[3]);
                            twrangelinux64.MaxValue = uint.Parse(a_asz[4]);
                            twrangelinux64.StepSize = uint.Parse(a_asz[5]);
                            twrangelinux64.DefaultValue = uint.Parse(a_asz[6]);
                            twrangelinux64.CurrentValue = uint.Parse(a_asz[7]);
                            Marshal.StructureToPtr(twrangelinux64, a_intptr, true);
                        }
                        return ("");
                    }

                case TWTY.FIX32:
                    {
                        double dMinValue = Convert.ToDouble(a_asz[3]);
                        double dMaxValue = Convert.ToDouble(a_asz[4]);
                        double dStepSize = Convert.ToDouble(a_asz[5]);
                        double dDefaultValue = Convert.ToDouble(a_asz[6]);
                        double dCurrentValue = Convert.ToDouble(a_asz[7]);
                        if (PlatformInfo.IsMacOSX)
                        {
                            twrangefix32macosx.ItemType = (uint)a_twty;
                            twrangefix32macosx.MinValue.Whole = (short)dMinValue;
                            twrangefix32macosx.MinValue.Frac = (ushort)((dMinValue - (double)twrangefix32macosx.MinValue.Whole) * 65536.0);
                            twrangefix32macosx.MaxValue.Whole = (short)dMaxValue;
                            twrangefix32macosx.MaxValue.Frac = (ushort)((dMaxValue - (double)twrangefix32macosx.MaxValue.Whole) * 65536.0);
                            twrangefix32macosx.StepSize.Whole = (short)dStepSize;
                            twrangefix32macosx.StepSize.Frac = (ushort)((dStepSize - (double)twrangefix32macosx.StepSize.Whole) * 65536.0);
                            twrangefix32macosx.DefaultValue.Whole = (short)dDefaultValue;
                            twrangefix32macosx.DefaultValue.Frac = (ushort)((dDefaultValue - (double)twrangefix32macosx.DefaultValue.Whole) * 65536.0);
                            twrangefix32macosx.CurrentValue.Whole = (short)dCurrentValue;
                            twrangefix32macosx.CurrentValue.Frac = (ushort)((dCurrentValue - (double)twrangefix32macosx.CurrentValue.Whole) * 65536.0);
                            Marshal.StructureToPtr(twrangefix32macosx, a_intptr, true);
                        }
                        else
                        {
                            twrangefix32.ItemType = a_twty;
                            twrangefix32.MinValue.Whole = (short)dMinValue;
                            twrangefix32.MinValue.Frac = (ushort)((dMinValue - (double)twrangefix32.MinValue.Whole) * 65536.0);
                            twrangefix32.MaxValue.Whole = (short)dMaxValue;
                            twrangefix32.MaxValue.Frac = (ushort)((dMaxValue - (double)twrangefix32.MaxValue.Whole) * 65536.0);
                            twrangefix32.StepSize.Whole = (short)dStepSize;
                            twrangefix32.StepSize.Frac = (ushort)((dStepSize - (double)twrangefix32.StepSize.Whole) * 65536.0);
                            twrangefix32.DefaultValue.Whole = (short)dDefaultValue;
                            twrangefix32.DefaultValue.Frac = (ushort)((dDefaultValue - (double)twrangefix32.DefaultValue.Whole) * 65536.0);
                            twrangefix32.CurrentValue.Whole = (short)dCurrentValue;
                            twrangefix32.CurrentValue.Frac = (ushort)((dCurrentValue - (double)twrangefix32.CurrentValue.Whole) * 65536.0);
                            Marshal.StructureToPtr(twrangefix32, a_intptr, true);
                        }
                        return ("");
                    }
            }
        }

        /// <summary>
        /// Our callback delegate for Windows...
        /// </summary>
        /// <param name="origin">Origin of message</param>
        /// <param name="dest">Message target</param>
        /// <param name="dg">Data group</param>
        /// <param name="dat">Data argument type</param>
        /// <param name="msg">Operation</param>
        /// <param name="twnull">NULL pointer</param>
        /// <returns>TWAIN status</returns>
        private UInt16 WindowsDsmEntryCallbackProxy
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        )
        {
            ProcessEvent(msg);
            return ((UInt16)STS.SUCCESS);
        }

        /// <summary>
        /// Our callback delegate for Linux...
        /// </summary>
        /// <param name="origin">Origin of message</param>
        /// <param name="dest">Message target</param>
        /// <param name="dg">Data group</param>
        /// <param name="dat">Data argument type</param>
        /// <param name="msg">Operation</param>
        /// <param name="twnull">NULL pointer</param>
        /// <returns>TWAIN status</returns>
        private UInt16 LinuxDsmEntryCallbackProxy
        (
            ref TW_IDENTITY_LEGACY origin,
            ref TW_IDENTITY_LEGACY dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        )
        {
            ProcessEvent(msg);
            return ((UInt16)STS.SUCCESS);
        }

        /// <summary>
        /// Our callback delegate for Mac OS X...
        /// </summary>
        /// <param name="origin">Origin of message</param>
        /// <param name="dest">Message target</param>
        /// <param name="dg">Data group</param>
        /// <param name="dat">Data argument type</param>
        /// <param name="msg">Operation</param>
        /// <param name="twnull">NULL pointer</param>
        /// <returns>TWAIN status</returns>
        private UInt16 MacosxDsmEntryCallbackProxy
        (
            ref TW_IDENTITY_MACOSX origin,
            ref TW_IDENTITY_MACOSX dest,
            DG dg,
            DAT dat,
            MSG msg,
            IntPtr twnull
        )
        {
            ProcessEvent(msg);
            return ((UInt16)STS.SUCCESS);
        }

        ///// <summary>
        ///// Get .NET 'Bitmap' object from memory DIB via stream constructor.
        ///// This should work for most DIBs.
        ///// </summary>
        ///// <param name="a_platform">Our operating system</param>
        ///// <param name="a_intptrNative">The pointer to something (presumably a BITMAP or a TIFF image)</param>
        ///// <returns>C# Bitmap of image</returns>
        //private Bitmap NativeToBitmap(Platform a_platform, IntPtr a_intptrNative)
        //{
        //    ushort u16Magic;
        //    IntPtr intptrNative;

        //    // We need the first two bytes to decide if we have a DIB or a TIFF.  Don't
        //    // forget to lock the silly thing...
        //    intptrNative = DsmMemLock(a_intptrNative);
        //    u16Magic = (ushort)Marshal.PtrToStructure(intptrNative, typeof(ushort));

        //    // Windows uses a DIB, the first usigned short is 40...
        //    if (u16Magic == 40)
        //    {
        //        byte[] bBitmap;
        //        BITMAPFILEHEADER bitmapfileheader;
        //        BITMAPINFOHEADER bitmapinfoheader;

        //        // Our incoming DIB is a bitmap info header...
        //        bitmapinfoheader = (BITMAPINFOHEADER)Marshal.PtrToStructure(intptrNative, typeof(BITMAPINFOHEADER));

        //        // Build our file header...
        //        bitmapfileheader = new BITMAPFILEHEADER();
        //        bitmapfileheader.bfType = 0x4D42; // "BM"
        //        bitmapfileheader.bfSize
        //            = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
        //               bitmapinfoheader.biSize +
        //               (bitmapinfoheader.biClrUsed * 4) +
        //               bitmapinfoheader.biSizeImage;
        //        bitmapfileheader.bfOffBits
        //            = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
        //               bitmapinfoheader.biSize +
        //               (bitmapinfoheader.biClrUsed * 4);

        //        // Copy the file header into our byte array...
        //        IntPtr intptr = Marshal.AllocHGlobal(Marshal.SizeOf(bitmapfileheader));
        //        Marshal.StructureToPtr(bitmapfileheader, intptr, true);
        //        bBitmap = new byte[bitmapfileheader.bfSize];
        //        Marshal.Copy(intptr, bBitmap, 0, Marshal.SizeOf(bitmapfileheader));
        //        Marshal.FreeHGlobal(intptr);
        //        intptr = IntPtr.Zero;

        //        // Copy the rest of the DIB into our byte array......
        //        Marshal.Copy(intptrNative, bBitmap, Marshal.SizeOf(typeof(BITMAPFILEHEADER)), (int)bitmapfileheader.bfSize - Marshal.SizeOf(typeof(BITMAPFILEHEADER)));

        //        // Now we can turn the in-memory bitmap file into a Bitmap object...
        //        MemoryStream memorystream = new MemoryStream(bBitmap);

        //        // Unfortunately the stream has to be kept with the bitmap...
        //        Bitmap bitmapStream = new Bitmap(memorystream);

        //        // So we make a copy (ick)...
        //        Bitmap bitmap;
        //        switch (bitmapinfoheader.biBitCount)
        //        {
        //            default:
        //            case 24:
        //                bitmap = bitmapStream.Clone(new Rectangle(0, 0, bitmapStream.Width, bitmapStream.Height), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        //                break;
        //            case 8:
        //                bitmap = bitmapStream.Clone(new Rectangle(0, 0, bitmapStream.Width, bitmapStream.Height), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
        //                break;
        //            case 1:
        //                bitmap = bitmapStream.Clone(new Rectangle(0, 0, bitmapStream.Width, bitmapStream.Height), System.Drawing.Imaging.PixelFormat.Format1bppIndexed);
        //                break;
        //        }

        //        // Fix the resolution...
        //        bitmap.SetResolution((int)(bitmap.HorizontalResolution + 0.5), (int)(bitmap.VerticalResolution + 0.5));

        //        // Cleanup...
        //        //bitmapStream.Dispose();
        //        //memorystream.Close();
        //        bitmapStream = null;
        //        memorystream = null;
        //        bBitmap = null;

        //        // Return our bitmap...
        //        DsmMemUnlock(a_intptrNative);
        //        return (bitmap);
        //    }

        //    // Linux and Mac OS X use TIFF.  We'll handle a simple Intel TIFF ("II")...
        //    else if (u16Magic == 0x4949)
        //    {
        //        int iTiffSize;
        //        ulong u64;
        //        ulong u64Pointer;
        //        ulong u64TiffHeaderSize;
        //        ulong u64TiffTagSize;
        //        byte[] abTiff;
        //        TIFFHEADER tiffheader;
        //        TIFFTAG tifftag;

        //        // Init stuff...
        //        tiffheader = new TIFFHEADER();
        //        tifftag = new TIFFTAG();
        //        u64TiffHeaderSize = (ulong)Marshal.SizeOf(tiffheader);
        //        u64TiffTagSize = (ulong)Marshal.SizeOf(tifftag);

        //        // Find the size of the image so we can turn it into a memory stream...
        //        iTiffSize = 0;
        //        tiffheader = (TIFFHEADER)Marshal.PtrToStructure(intptrNative, typeof(TIFFHEADER));
        //        for (u64 = 0; u64 < 999; u64++)
        //        {
        //            u64Pointer = (ulong)intptrNative + u64TiffHeaderSize + (u64TiffTagSize * u64);
        //            tifftag = (TIFFTAG)Marshal.PtrToStructure((IntPtr)u64Pointer, typeof(TIFFTAG));

        //            // StripOffsets...
        //            if (tifftag.u16Tag == 273)
        //            {
        //                iTiffSize += (int)tifftag.u32Value;
        //            }

        //            // StripByteCounts...
        //            if (tifftag.u16Tag == 279)
        //            {
        //                iTiffSize += (int)tifftag.u32Value;
        //            }
        //        }

        //        // No joy...
        //        if (iTiffSize == 0)
        //        {
        //            DsmMemUnlock(a_intptrNative);
        //            return (null);
        //        }

        //        // Copy the data to our byte array...
        //        abTiff = new byte[iTiffSize];
        //        Marshal.Copy(intptrNative, abTiff, 0, iTiffSize);

        //        // Move the image into a memory stream...
        //        MemoryStream memorystream = new MemoryStream(abTiff);

        //        // Turn the memory stream into an in-memory TIFF image...
        //        Image imageTiff = Image.FromStream(memorystream);

        //        // Convert the in-memory tiff to a Bitmap object...
        //        Bitmap bitmap = new Bitmap(imageTiff);

        //        // Cleanup...
        //        abTiff = null;
        //        memorystream = null;
        //        imageTiff = null;

        //        // Return our bitmap...
        //        DsmMemUnlock(a_intptrNative);
        //        return (bitmap);
        //    }

        //    // Uh-oh...
        //    DsmMemUnlock(a_intptrNative);
        //    return (null);
        //}

        /// <summary>
        /// Get .NET 'Bitmap' object from memory DIB via stream constructor.
        /// This should work for most DIBs.
        /// </summary>
        /// <param name="a_intptrNative">The pointer to something (presumably a BITMAP or a TIFF image)</param>
        /// <param name="a_blIsHandle"></param>
        /// <param name="a_iHeaderBytes"></param>
        /// <returns>C# Bitmap of image</returns>
        public byte[] NativeToByteArray(IntPtr a_intptrNative, bool a_blIsHandle, out int a_iHeaderBytes)
        {
            ushort u16Magic;
            UIntPtr uintptrBytes;
            IntPtr intptrNative;

            // Init stuff...
            a_iHeaderBytes = 0;

            // Give ourselves what protection we can...
            try
            {
                // We need the first two bytes to decide if we have a DIB or a TIFF.  Don't
                // forget to lock the silly thing...
                intptrNative = a_blIsHandle ? DsmMemLock(a_intptrNative) : a_intptrNative;
                u16Magic = (ushort)Marshal.PtrToStructure(intptrNative, typeof(ushort));

                // Windows uses a DIB, the first unsigned short is 40...
                if (u16Magic == 40)
                {
                    byte[] abBitmap;
                    BITMAPFILEHEADER bitmapfileheader;
                    BITMAPINFOHEADER bitmapinfoheader;

                    // Our incoming DIB is a bitmap info header...
                    bitmapinfoheader = (BITMAPINFOHEADER)Marshal.PtrToStructure(intptrNative, typeof(BITMAPINFOHEADER));

                    // Build our file header...
                    bitmapfileheader = new BITMAPFILEHEADER();
                    bitmapfileheader.bfType = 0x4D42; // "BM"
                    bitmapfileheader.bfSize
                        = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
                           bitmapinfoheader.biSize +
                           (bitmapinfoheader.biClrUsed * 4) +
                           bitmapinfoheader.biSizeImage;
                    bitmapfileheader.bfOffBits
                        = (uint)Marshal.SizeOf(typeof(BITMAPFILEHEADER)) +
                           bitmapinfoheader.biSize +
                           (bitmapinfoheader.biClrUsed * 4);

                    // Copy the file header into our byte array...
                    IntPtr intptr = Marshal.AllocHGlobal(Marshal.SizeOf(bitmapfileheader));
                    Marshal.StructureToPtr(bitmapfileheader, intptr, true);
                    abBitmap = new byte[bitmapfileheader.bfSize];
                    Marshal.Copy(intptr, abBitmap, 0, Marshal.SizeOf(bitmapfileheader));
                    Marshal.FreeHGlobal(intptr);
                    intptr = IntPtr.Zero;

                    // Copy the rest of the DIB into our byte array...
                    a_iHeaderBytes = (int)bitmapfileheader.bfOffBits;
                    Marshal.Copy(intptrNative, abBitmap, Marshal.SizeOf(typeof(BITMAPFILEHEADER)), (int)bitmapfileheader.bfSize - Marshal.SizeOf(typeof(BITMAPFILEHEADER)));

                    // Unlock the handle, and return our byte array...
                    if (a_blIsHandle)
                    {
                        DsmMemUnlock(a_intptrNative);
                    }
                    return (abBitmap);
                }

                // Linux and Mac OS X use TIFF.  We'll handle a simple Intel TIFF ("II")...
                else if (u16Magic == 0x4949)
                {
                    int iTiffSize;
                    ulong u64;
                    ulong u64Pointer;
                    ulong u64TiffHeaderSize;
                    ulong u64TiffTagSize;
                    byte[] abTiff;
                    TIFFHEADER tiffheader;
                    TIFFTAG tifftag;

                    // Init stuff...
                    tiffheader = new TIFFHEADER();
                    tifftag = new TIFFTAG();
                    u64TiffHeaderSize = (ulong)Marshal.SizeOf(tiffheader);
                    u64TiffTagSize = (ulong)Marshal.SizeOf(tifftag);

                    // Find the size of the image so we can turn it into a byte array...
                    iTiffSize = 0;
                    tiffheader = (TIFFHEADER)Marshal.PtrToStructure(intptrNative, typeof(TIFFHEADER));
                    for (u64 = 0; u64 < 999; u64++)
                    {
                        u64Pointer = (ulong)intptrNative + u64TiffHeaderSize + (u64TiffTagSize * u64);
                        tifftag = (TIFFTAG)Marshal.PtrToStructure((IntPtr)u64Pointer, typeof(TIFFTAG));

                        // StripOffsets...
                        if (tifftag.u16Tag == 273)
                        {
                            iTiffSize += (int)tifftag.u32Value;
                            a_iHeaderBytes = (int)tifftag.u32Value;
                        }

                        // StripByteCounts...
                        if (tifftag.u16Tag == 279)
                        {
                            iTiffSize += (int)tifftag.u32Value;
                        }
                    }

                    // No joy...
                    if (iTiffSize == 0)
                    {
                        if (a_blIsHandle)
                        {
                            DsmMemUnlock(a_intptrNative);
                        }
                        return (null);
                    }

                    // Copy the data to our byte array...
                    abTiff = new byte[iTiffSize];
                    Marshal.Copy(intptrNative, abTiff, 0, iTiffSize);

                    // Unlock the handle, and return our byte array...
                    if (a_blIsHandle)
                    {
                        DsmMemUnlock(a_intptrNative);
                    }
                    return (abTiff);
                }

                // As long as we're here, let's handle JFIF (JPEG) too,
                // this can never be a handle...
                else if (u16Magic == 0xFFD8)
                {
                    byte[] abJfif;

                    // We need the size of this memory block...
                    if (PlatformInfo.IsWindows)
                    {
                        uintptrBytes = NativeMethods._msize(a_intptrNative);
                    }
                    else if (PlatformInfo.IsLinux)
                    {
                        uintptrBytes = NativeMethods.malloc_usable_size(a_intptrNative);
                    }
                    else if (PlatformInfo.IsMacOSX)
                    {
                        uintptrBytes = NativeMethods.malloc_size(a_intptrNative);
                    }
                    else
                    {
                        Log.Error("Really? <" + Environment.OSVersion.Platform + ">");
                        return (null);
                    }

                    abJfif = new byte[(int)uintptrBytes];
                    Marshal.Copy(a_intptrNative, abJfif, 0, (int)(int)uintptrBytes);
                    return (abJfif);
                }
            }
            catch (Exception exception)
            {
                Log.Error("NativeToByteArray threw an exceptions - " + exception.Message);
            }

            // Byte-bye...
            DsmMemUnlock(a_intptrNative);
            return (null);
        }

        /// <summary>
        /// Convert a public identity to a legacy identity...
        /// </summary>
        /// <param name="a_twidentity">Identity to convert</param>
        /// <returns>Legacy form of identity</returns>
        private TW_IDENTITY_LEGACY TwidentityToTwidentitylegacy(TW_IDENTITY a_twidentity)
        {
            TW_IDENTITY_LEGACY twidentitylegacy = new TW_IDENTITY_LEGACY();
            twidentitylegacy.Id = (uint)a_twidentity.Id;
            twidentitylegacy.Manufacturer = a_twidentity.Manufacturer;
            twidentitylegacy.ProductFamily = a_twidentity.ProductFamily;
            twidentitylegacy.ProductName = a_twidentity.ProductName;
            twidentitylegacy.ProtocolMajor = a_twidentity.ProtocolMajor;
            twidentitylegacy.ProtocolMinor = a_twidentity.ProtocolMinor;
            twidentitylegacy.SupportedGroups = a_twidentity.SupportedGroups;
            twidentitylegacy.Version.Country = a_twidentity.Version.Country;
            twidentitylegacy.Version.Info = a_twidentity.Version.Info;
            twidentitylegacy.Version.Language = a_twidentity.Version.Language;
            twidentitylegacy.Version.MajorNum = a_twidentity.Version.MajorNum;
            twidentitylegacy.Version.MinorNum = a_twidentity.Version.MinorNum;
            return (twidentitylegacy);
        }

        /// <summary>
        /// Convert a public identity to a linux64 identity...
        /// </summary>
        /// <param name="a_twidentity">Identity to convert</param>
        /// <returns>Linux64 form of identity</returns>
        private TW_IDENTITY_LINUX64 TwidentityToTwidentitylinux64(TW_IDENTITY a_twidentity)
        {
            TW_IDENTITY_LINUX64 twidentitylinux64 = new TW_IDENTITY_LINUX64();
            twidentitylinux64.Id = a_twidentity.Id;
            twidentitylinux64.Manufacturer = a_twidentity.Manufacturer;
            twidentitylinux64.ProductFamily = a_twidentity.ProductFamily;
            twidentitylinux64.ProductName = a_twidentity.ProductName;
            twidentitylinux64.ProtocolMajor = a_twidentity.ProtocolMajor;
            twidentitylinux64.ProtocolMinor = a_twidentity.ProtocolMinor;
            twidentitylinux64.SupportedGroups = a_twidentity.SupportedGroups;
            twidentitylinux64.Version.Country = a_twidentity.Version.Country;
            twidentitylinux64.Version.Info = a_twidentity.Version.Info;
            twidentitylinux64.Version.Language = a_twidentity.Version.Language;
            twidentitylinux64.Version.MajorNum = a_twidentity.Version.MajorNum;
            twidentitylinux64.Version.MinorNum = a_twidentity.Version.MinorNum;
            return (twidentitylinux64);
        }

        /// <summary>
        /// Convert a public identity to a macosx identity...
        /// </summary>
        /// <param name="a_twidentity">Identity to convert</param>
        /// <returns>Mac OS X form of identity</returns>
        public static TW_IDENTITY_MACOSX TwidentityToTwidentitymacosx(TW_IDENTITY a_twidentity)
        {
            TW_IDENTITY_MACOSX twidentitymacosx = new TW_IDENTITY_MACOSX();
            twidentitymacosx.Id = (uint)a_twidentity.Id;
            twidentitymacosx.Manufacturer = a_twidentity.Manufacturer;
            twidentitymacosx.ProductFamily = a_twidentity.ProductFamily;
            twidentitymacosx.ProductName = a_twidentity.ProductName;
            twidentitymacosx.ProtocolMajor = a_twidentity.ProtocolMajor;
            twidentitymacosx.ProtocolMinor = a_twidentity.ProtocolMinor;
            twidentitymacosx.SupportedGroups = a_twidentity.SupportedGroups;
            twidentitymacosx.Version.Country = a_twidentity.Version.Country;
            twidentitymacosx.Version.Info = a_twidentity.Version.Info;
            twidentitymacosx.Version.Language = a_twidentity.Version.Language;
            twidentitymacosx.Version.MajorNum = a_twidentity.Version.MajorNum;
            twidentitymacosx.Version.MinorNum = a_twidentity.Version.MinorNum;
            return (twidentitymacosx);
        }

        /// <summary>
        /// Convert a legacy identity to a public identity...
        /// </summary>
        /// <param name="a_twidentitylegacy">Legacy identity to convert</param>
        /// <returns>Regular form of identity</returns>
        private TW_IDENTITY TwidentitylegacyToTwidentity(TW_IDENTITY_LEGACY a_twidentitylegacy)
        {
            TW_IDENTITY twidentity = new TW_IDENTITY();
            twidentity.Id = a_twidentitylegacy.Id;
            twidentity.Manufacturer = a_twidentitylegacy.Manufacturer;
            twidentity.ProductFamily = a_twidentitylegacy.ProductFamily;
            twidentity.ProductName = a_twidentitylegacy.ProductName;
            twidentity.ProtocolMajor = a_twidentitylegacy.ProtocolMajor;
            twidentity.ProtocolMinor = a_twidentitylegacy.ProtocolMinor;
            twidentity.SupportedGroups = a_twidentitylegacy.SupportedGroups;
            twidentity.Version.Country = a_twidentitylegacy.Version.Country;
            twidentity.Version.Info = a_twidentitylegacy.Version.Info;
            twidentity.Version.Language = a_twidentitylegacy.Version.Language;
            twidentity.Version.MajorNum = a_twidentitylegacy.Version.MajorNum;
            twidentity.Version.MinorNum = a_twidentitylegacy.Version.MinorNum;
            return (twidentity);
        }

        /// <summary>
        /// Convert a linux64 identity to a public identity...
        /// </summary>
        /// <param name="a_twidentitylinux64">identity to convert</param>
        /// <returns>Regular form of identity</returns>
        private TW_IDENTITY Twidentitylinux64ToTwidentity(TW_IDENTITY_LINUX64 a_twidentitylinux64)
        {
            TW_IDENTITY twidentity = new TW_IDENTITY();
            twidentity.Id = a_twidentitylinux64.Id;
            twidentity.Manufacturer = a_twidentitylinux64.Manufacturer;
            twidentity.ProductFamily = a_twidentitylinux64.ProductFamily;
            twidentity.ProductName = a_twidentitylinux64.ProductName;
            twidentity.ProtocolMajor = a_twidentitylinux64.ProtocolMajor;
            twidentity.ProtocolMinor = a_twidentitylinux64.ProtocolMinor;
            twidentity.SupportedGroups = (uint)a_twidentitylinux64.SupportedGroups;
            twidentity.Version.Country = a_twidentitylinux64.Version.Country;
            twidentity.Version.Info = a_twidentitylinux64.Version.Info;
            twidentity.Version.Language = a_twidentitylinux64.Version.Language;
            twidentity.Version.MajorNum = a_twidentitylinux64.Version.MajorNum;
            twidentity.Version.MinorNum = a_twidentitylinux64.Version.MinorNum;
            return (twidentity);
        }

        /// <summary>
        /// Convert a macosx identity to a public identity...
        /// </summary>
        /// <param name="a_twidentitymacosx">Mac OS X identity to convert</param>
        /// <returns>Regular identity</returns>
        private TW_IDENTITY TwidentitymacosxToTwidentity(TW_IDENTITY_MACOSX a_twidentitymacosx)
        {
            TW_IDENTITY twidentity = new TW_IDENTITY();
            twidentity.Id = a_twidentitymacosx.Id;
            twidentity.Manufacturer = a_twidentitymacosx.Manufacturer;
            twidentity.ProductFamily = a_twidentitymacosx.ProductFamily;
            twidentity.ProductName = a_twidentitymacosx.ProductName;
            twidentity.ProtocolMajor = a_twidentitymacosx.ProtocolMajor;
            twidentity.ProtocolMinor = a_twidentitymacosx.ProtocolMinor;
            twidentity.SupportedGroups = a_twidentitymacosx.SupportedGroups;
            twidentity.Version.Country = a_twidentitymacosx.Version.Country;
            twidentity.Version.Info = a_twidentitymacosx.Version.Info;
            twidentity.Version.Language = a_twidentitymacosx.Version.Language;
            twidentity.Version.MajorNum = a_twidentitymacosx.Version.MajorNum;
            twidentity.Version.MinorNum = a_twidentitymacosx.Version.MinorNum;
            return (twidentity);
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Structures, things we need for the thread, and to support stuff
        // like DAT_IMAGENATIVEXFER...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Structures...

        /// <summary>
        /// The data we share with the thread...
        /// </summary>
        
        private struct ThreadData
        {
            // The state of the structure...
            public bool blIsInuse;
            public bool blIsComplete;
            public bool blExitThread;

            // Command...
            public DG dg;
            public DAT dat;
            public MSG msg;
            public STATE stateRollback;
            public bool blRollback;

            // Payload...
            public IntPtr intptrHwnd;
            public IntPtr intptrBitmap;
            public IntPtr intptrAudio;
            public IntPtr twmemref;
            // TODO: Recode later
            //public Bitmap bitmap;
            //public bool blUseBitmapHandle;
            public UInt32 twuint32;
            public TW_AUDIOINFO twaudioinfo;
            public TW_CALLBACK twcallback;
            public TW_CALLBACK2 twcallback2;
            public TW_CAPABILITY twcapability;
            public TW_CIECOLOR twciecolor;
            public TW_CUSTOMDSDATA twcustomdsdata;
            public TW_DEVICEEVENT twdeviceevent;
            public TW_ENTRYPOINT twentrypoint;
            public TW_EVENT twevent;
            public TW_EXTIMAGEINFO twextimageinfo;
            public TW_FILESYSTEM twfilesystem;
            public TW_FILTER twfilter;
            public TW_GRAYRESPONSE twgrayresponse;
            public TW_IDENTITY twidentity;
            public TW_IDENTITY_LEGACY twidentitylegacy;
            public TW_IMAGEINFO twimageinfo;
            public TW_IMAGELAYOUT twimagelayout;
            public TW_IMAGEMEMXFER twimagememxfer;
            public TW_JPEGCOMPRESSION twjpegcompression;
            public TW_METRICS twmetrics;
            public TW_MEMORY twmemory;
            public TW_PALETTE8 twpalette8;
            public TW_PASSTHRU twpassthru;
            public TW_PENDINGXFERS twpendingxfers;
            public TW_RGBRESPONSE twrgbresponse;
            public TW_SETUPFILEXFER twsetupfilexfer;
            public TW_SETUPMEMXFER twsetupmemxfer;
            public TW_STATUS twstatus;
            public TW_STATUSUTF8 twstatusutf8;
            public TW_TWAINDIRECT twtwaindirect;
            public TW_USERINTERFACE twuserinterface;

            // Result...
            public STS sts;
        }

        /// <summary>
        /// The Windows Point structure.
        /// Needed for the PreFilterMessage function when we're
        /// handling DAT_EVENT...
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct POINT
        {
            public int X;
            public int Y;
        }

        /// <summary>
        /// The Windows MSG structure.
        /// Needed for the PreFilterMessage function when we're
        /// handling DAT_EVENT...
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct MESSAGE
        {
            public IntPtr hwnd;
            public UInt32 message;
            public IntPtr wParam;
            public IntPtr lParam;
            public UInt32 time;
            public POINT pt;
        }

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// Our application identity...
        /// </summary>
        private TW_IDENTITY m_twidentityApp;
        private TW_IDENTITY_LEGACY m_twidentitylegacyApp;
        private TW_IDENTITY_MACOSX m_twidentitymacosxApp;

        /// <summary>
        /// Our Data Source identity...
        /// </summary>
        internal TW_IDENTITY m_twidentityDs;
        private TW_IDENTITY_LEGACY m_twidentitylegacyDs;
        private TW_IDENTITY_MACOSX m_twidentitymacosxDs;

        /// <summary>
        /// Our current TWAIN state...
        /// </summary>
        private STATE m_state;
        private bool m_blAcceptXferReady;

        /// <summary>
        /// DAT_NULL flags that we've seen after entering into
        /// state 5 through MSG_ENABLEDS or MSG_ENABLEDSUIONLY,
        /// or coming down from DAT_PENDINGXFERS, either
        /// MSG_ENDXFER or MSG_RESET...
        /// </summary>
        private bool m_blIsMsgxferready;
        private bool m_blIsMsgclosedsreq;
        private bool m_blIsMsgclosedsok;
        private bool m_blIsMsgdeviceevent;
        private bool m_blRunningDatUserinterface;
        private Thread m_threadRunningDatUserinterface;

        /// <summary>
        /// Automatically issue DAT.STATUS on TWRC_FAILURE...
        /// </summary>
        private bool m_blAutoDatStatus;

        /// <summary>
        /// Windows, pick between TWAIN_32.DLL and TWAINDSM.DLL...
        /// Mac OS X, pick between /System/Library/Frameworks/TWAIN.framework and /Library/Frameworks/TWAINDSM.framework
        /// </summary>
        private bool m_blUseLegacyDSM;

        /// <summary>
        /// Help us pick the right DSM for the current data source,
        /// the first one is for the session, the second one is for
        /// getfirst/getnext, and allows us to check for drivers
        /// using either one or both DSMs, depending on what is
        /// available...
        /// </summary>
        internal LinuxDsm m_linuxdsm;
        private LinuxDsm m_linux64bitdsmDatIdentity;
        internal bool m_blFoundLatestDsm;
        internal bool m_blFoundLatestDsm64;
        internal bool m_blFound020302Dsm64bit;

        /// <summary>
        /// Use the callback system (TWAINDSM.DLL only)...
        /// </summary>
        private bool m_blUseCallbacks;

        ///// <summary>
        ///// The platform we're running on...
        ///// </summary>
        //static Platform ms_platform;

        /// <summary>
        /// Delegates for DAT_CALLBACK...
        /// </summary>
        private NativeMethods.WindowsDsmEntryCallbackDelegate m_windowsdsmentrycontrolcallbackdelegate;
        private NativeMethods.LinuxDsmEntryCallbackDelegate m_linuxdsmentrycontrolcallbackdelegate;
        private NativeMethods.MacosxDsmEntryCallbackDelegate m_macosxdsmentrycontrolcallbackdelegate;

        /// <summary>
        /// We only allow one thread at a time to talk to the TWAIN driver...
        /// </summary>
        private Object m_lockTwain;

        /// <summary>
        /// Use this to wait for commands from the caller...
        /// </summary>
        private AutoResetEvent m_autoreseteventCaller;

        /// <summary>
        /// Use this to force the user's command to block until TWAIN has
        /// a response...
        /// </summary>
        private AutoResetEvent m_autoreseteventThread;

        /// <summary>
        /// Use this to force the user's rollback to block until TWAIN has
        /// a response...
        /// </summary>
        private AutoResetEvent m_autoreseteventRollback;

        /// <summary>
        /// One can get into a race condition with the thread, so we use
        /// this event to confirm that it's started and ready for use...
        /// </summary>
        private AutoResetEvent m_autoreseteventThreadStarted;

        ///// <summary>
        ///// The data we share with the thread...
        ///// </summary>
        ////private ThreadData m_threaddata;

        /// <summary>
        /// Our callback for device events...
        /// </summary>
        private DeviceEventCallback m_deviceeventcallback;

        /// <summary>
        /// Our callback function for scanning...
        /// </summary>
        private ScanCallback m_scancallback;

        /// <summary>
        /// Run stuff in a caller's UI thread...
        /// </summary>
        private RunInUiThreadDelegate m_runinuithreaddelegate;

        /// <summary>
        /// The event calls don't go through the thread...
        /// </summary>
        private TW_EVENT m_tweventPreFilterMessage;

        // Remember the window handle, so we can reuse it...
        private IntPtr m_intptrHwnd;

        /// <summary>
        /// Our thread...
        /// </summary>
        private Thread m_threadTwain;

        /// <summary>
        /// How we talk to our thread...
        /// </summary>
        private TwainCommand m_twaincommand;

        /// <summary>
        ///  Indecies for commands that have to do something a
        ///  bit more fancy, such as running the command in the
        ///  context of a GUI thread.  And flags to help know
        ///  when we are doing this...
        /// </summary>
        private ThreadData m_threaddataDatAudiofilexfer;
        private ThreadData m_threaddataDatAudionativexfer;
        private ThreadData m_threaddataDatCapability;
        private ThreadData m_threaddataDatEvent;
        private ThreadData m_threaddataDatExtimageinfo;
        private ThreadData m_threaddataDatIdentity;
        private ThreadData m_threaddataDatImagefilexfer;
        private ThreadData m_threaddataDatImageinfo;
        private ThreadData m_threaddataDatImagelayout;
        private ThreadData m_threaddataDatImagememfilexfer;
        private ThreadData m_threaddataDatImagememxfer;
        private ThreadData m_threaddataDatImagenativexfer;
        private ThreadData m_threaddataDatParent;
        private ThreadData m_threaddataDatPendingxfers;
        private ThreadData m_threaddataDatSetupfilexfer;
        private ThreadData m_threaddataDatSetupmemxfer;
        private ThreadData m_threaddataDatStatus;
        private ThreadData m_threaddataDatUserinterface;

        /// <summary>
        /// Our helper functions from the DSM...
        /// </summary>
        private TW_ENTRYPOINT_DELEGATES m_twentrypointdelegates;

        #endregion


        ///////////////////////////////////////////////////////////////////////////////
        // TwainCommand...
        ///////////////////////////////////////////////////////////////////////////////
        #region TwainCommand...

        /// <summary>
        /// We have TWAIN commands that can be called by the application from any
        /// thread they want.  We do a lock, and then build a command and submit
        /// it to the Main thread.  The Main thread runs without locks, so all it
        /// is allowed to do is examine TwainCommands to see if it has work.  If
        /// it finds an item, it takes care of it, and changes it to complete.
        /// </summary>
        private sealed class TwainCommand
        {
            ///////////////////////////////////////////////////////////////////////////
            // Public Functions...
            ///////////////////////////////////////////////////////////////////////////
            #region Public Functions...

            /// <summary>
            /// Initialize an array that we'll be sharing between the TWAIN operations
            /// and the Main thread...
            /// </summary>
            public TwainCommand()
            {
                m_athreaddata = new ThreadData[8];
            }

            /// <summary>
            /// Complete a command
            /// </summary>
            /// <param name="a_lIndex">index to update</param>
            /// <param name="a_threaddata">data to use</param>
            public void Complete(long a_lIndex, ThreadData a_threaddata)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return;
                }

                // We're not really a command...
                if (!m_athreaddata[a_lIndex].blIsInuse)
                {
                    return;
                }

                // Do the update and tag it complete...
                m_athreaddata[a_lIndex] = a_threaddata;
                m_athreaddata[a_lIndex].blIsComplete = true;
            }

            /// <summary>
            /// Delete a command...
            /// </summary>
            /// <returns>the requested command</returns>
            public void Delete(long a_lIndex)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return;
                }

                // Clear the record...
                m_athreaddata[a_lIndex] = default;
            }

            /// <summary>
            /// Get a command...
            /// </summary>
            /// <returns>the requested command</returns>
            public ThreadData Get(long a_lIndex)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return (new ThreadData());
                }

                // Return what we found...
                return (m_athreaddata[a_lIndex]);
            }

            /// <summary>
            /// Get the next command in the list...
            /// </summary>
            /// <param name="a_lIndex">the index of the data</param>
            /// <param name="a_threaddata">the command we'll return</param>
            /// <returns>true if we found something</returns>
            public bool GetNext(out long a_lIndex, out ThreadData a_threaddata)
            {
                long lIndex;

                // Init stuff...
                lIndex = m_lIndex;
                a_lIndex = 0;
                a_threaddata = default;

                // Cycle once through the commands to see if we have any...
                for (; ; )
                {
                    // We found something, copy it out, point to the next
                    // item (so we know we're looking at the whole list)
                    // and return...
                    if (m_athreaddata[lIndex].blIsInuse && !m_athreaddata[lIndex].blIsComplete)
                    {
                        a_threaddata = m_athreaddata[lIndex];
                        a_lIndex = lIndex;
                        m_lIndex = lIndex + 1;
                        if (m_lIndex >= m_athreaddata.Length)
                        {
                            m_lIndex = 0;
                        }
                        return (true);
                    }

                    // Next item...
                    lIndex += 1;
                    if (lIndex >= m_athreaddata.Length)
                    {
                        lIndex = 0;
                    }

                    // We've cycled, and we didn't find anything...
                    if (lIndex == m_lIndex)
                    {
                        a_lIndex = lIndex;
                        return (false);
                    }
                }
            }

            /// <summary>
            /// Submit a new command...
            /// </summary>
            /// <returns></returns>
            public long Submit(ThreadData a_threadata)
            {
                long ll;

                // We won't leave until we've submitted the beastie...
                for (; ; )
                {
                    // Look for a free slot...
                    for (ll = 0; ll < m_athreaddata.Length; ll++)
                    {
                        if (!m_athreaddata[ll].blIsInuse)
                        {
                            m_athreaddata[ll] = a_threadata;
                            m_athreaddata[ll].blIsInuse = true;
                            return (ll);
                        }
                    }

                    // Wait a little...
                    Thread.Sleep(0);
                }
            }

            /// <summary>
            /// Update a command
            /// </summary>
            /// <param name="a_lIndex">index to update</param>
            /// <param name="a_threaddata">data to use</param>
            public void Update(long a_lIndex, ThreadData a_threaddata)
            {
                // If we're out of bounds, return an empty structure...
                if ((a_lIndex < 0) || (a_lIndex >= m_athreaddata.Length))
                {
                    return;
                }

                // We're not really a command...
                if (!m_athreaddata[a_lIndex].blIsInuse)
                {
                    return;
                }

                // Do the update...
                m_athreaddata[a_lIndex] = a_threaddata;
            }

            #endregion


            ///////////////////////////////////////////////////////////////////////////
            // Private Attributes...
            ///////////////////////////////////////////////////////////////////////////
            #region Private Attributes...

            /// <summary>
            /// The data we're sharing.  A null in a position means its available for
            /// use.  The Main thread only consumes items, it never creates or
            /// destroys them, that's done by the various commands.
            /// </summary>
            private ThreadData[] m_athreaddata;

            /// <summary>
            /// Index for browsing m_athreaddata for work...
            /// </summary>
            private long m_lIndex;

            #endregion
        }

        #endregion
    }
}
