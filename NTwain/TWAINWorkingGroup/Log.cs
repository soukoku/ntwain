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
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace TWAINWorkingGroup
{
    /// <summary>
    /// Our logger.  If we bump up to 4.5 (and if mono supports it at compile
    /// time), then we'll be able to add the following to our traces, which
    /// seems like it should be more than enough to locate log messages.  For
    /// now we'll leave the log messages undecorated:
    ///     [CallerFilePath] string file = "",
    ///     [CallerMemberName] string member = "",
    ///     [CallerLineNumber] int line = 0
    /// </summary>
    public static class Log
    {
        // Public Methods...
        #region Public Methods...

        /// <summary>
        /// Initialize our delegates...
        /// </summary>
        static Log()
        {
            Close = CloseLocal;
            GetLevel = GetLevelLocal;
            Open = OpenLocal;
            RegisterTwain = RegisterTwainLocal;
            SetFlush = SetFlushLocal;
            SetLevel = SetLevelLocal;
            WriteEntry = WriteEntryLocal;
        }

        /// <summary>
        /// Let the caller override our delegates with their own functions...
        /// </summary>
        /// <param name="a_closedelegate">use this to close the logging session</param>
        /// <param name="a_getleveldelegate">get the current log level</param>
        /// <param name="a_opendelegate">open the logging session</param>
        /// <param name="a_registertwaindelegate">not needed at this time</param>
        /// <param name="a_setflushdelegate">turn flushing on and off</param>
        /// <param name="a_setleveldelegate">set the new log level</param>
        /// <param name="a_writeentrydelegate">the function that actually writes to the log</param>
        /// <param name="a_getstatedelegate">returns a way to get the current TWAIN state</param>
        public static void Override
        (
            CloseDelegate a_closedelegate,
            GetLevelDelegate a_getleveldelegate,
            OpenDelegate a_opendelegate,
            RegisterTwainDelegate a_registertwaindelegate,
            SetFlushDelegate a_setflushdelegate,
            SetLevelDelegate a_setleveldelegate,
            WriteEntryDelegate a_writeentrydelegate,
            out GetStateDelegate a_getstatedelegate
        )
        {
            Close = (a_closedelegate != null) ? a_closedelegate : CloseLocal;
            GetLevel = (a_getleveldelegate != null) ? a_getleveldelegate : GetLevelLocal;
            Open = (a_opendelegate != null) ? a_opendelegate : OpenLocal;
            RegisterTwain = (a_registertwaindelegate != null) ? a_registertwaindelegate : RegisterTwainLocal;
            SetFlush = (a_setflushdelegate != null) ? a_setflushdelegate : SetFlushLocal;
            SetLevel = (a_setleveldelegate != null) ? a_setleveldelegate : SetLevelLocal;
            WriteEntry = (a_writeentrydelegate != null) ? a_writeentrydelegate : WriteEntryLocal;
            a_getstatedelegate = GetStateLocal;
        }

        /// <summary>
        /// Write an assert message, but only throw with a debug build...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Assert(string a_szMessage)
        {
            WriteEntry("A", a_szMessage, true);
#if DEBUG
            throw new Exception(a_szMessage);
#endif
        }

        /// <summary>
        /// Write an error message...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Error(string a_szMessage)
        {
            WriteEntry("E", a_szMessage, true);
        }

        /// <summary>
        /// Write an informational message...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Info(string a_szMessage)
        {
            WriteEntry(".", a_szMessage, ms_blFlush);
        }

        /// <summary>
        /// Log after sending to the TWAIN driver...
        /// </summary>
        /// <param name="a_sts">status</param>
        /// <param name="a_szMemref">data</param>
        public static void LogSendAfter(STS a_sts, string a_szMemref)
        {
            // The data argument type (DAT) stuff...
            if ((a_szMemref != null) && (a_szMemref != "") && (a_szMemref[0] != '('))
            {
                Log.Info("twn> " + a_szMemref);
            }

            // TWRC...
            if ((int)a_sts < OtherConsts.STSCC)
            {
                Log.Info("twn> " + a_sts);
            }
            // TWCC...
            else
            {
                Log.Info("twn> FAILURE/" + a_sts);
            }
        }

        /// <summary>
        /// Log before sending to the TWAIN driver...
        /// </summary>
        /// <param name="a_szDg">data group</param>
        /// <param name="a_szDat">data argument type</param>
        /// <param name="a_szMsg">message</param>
        /// <param name="a_szMemref">data</param>
        public static void LogSendBefore(string a_szDg, string a_szDat, string a_szMsg, string a_szMemref)
        {
            Log.Info("");
            Log.Info("twn> DG_" + a_szDg + "/DAT_" + a_szDat + "/MSG_" + a_szMsg);
            if ((a_szMemref != null) && (a_szMemref != "") && (a_szMemref[0] != '('))
            {
                Log.Info("twn> " + a_szMemref);
            }
        }

        /// <summary>
        /// Write a verbose message, this is extra info that isn't normally
        /// needed to diagnose problems, but may provide insight into what
        /// the code is doing...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Verbose(string a_szMessage)
        {
            WriteEntry("V", a_szMessage, ms_blFlush);
        }

        /// <summary>
        /// Write a verbose data message, this is extra info, specifically
        /// data transfers, that isn't normally needed to diagnose problems.
        /// Turning this one can really bloat the logs...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void VerboseData(string a_szMessage)
        {
            WriteEntry("D", a_szMessage, ms_blFlush);
        }

        /// <summary>
        /// Write an warning message...
        /// </summary>
        /// <param name="a_szMessage">message to log</param>
        public static void Warn(string a_szMessage)
        {
            WriteEntry("W", a_szMessage, ms_blFlush);
        }

        #endregion


        // Public Definitions...
        #region Public Definitions...

        // The public methods that need attributes, here offered
        // as delegates, so that a caller will be able to override
        // them...
        public delegate void CloseDelegate();
        public delegate int GetLevelDelegate();
        public delegate string GetStateDelegate();
        public delegate void OpenDelegate(string a_szName, string a_szPath, int a_iLevel);
        public delegate void RegisterTwainDelegate(TWAIN a_twain);
        public delegate void SetFlushDelegate(bool a_blFlush);
        public delegate void SetLevelDelegate(int a_iLevel);
        public delegate void WriteEntryDelegate(string a_szSeverity, string a_szMessage, bool a_blFlush);

        #endregion


        // Public Attributes...
        #region Public Attributes...

        // The public methods that need attributes, here offered
        // as delegates, so that a caller will be able to override
        // them...
        public static CloseDelegate Close;
        public static GetLevelDelegate GetLevel;
        public static OpenDelegate Open;
        public static RegisterTwainDelegate RegisterTwain;
        public static SetFlushDelegate SetFlush;
        public static SetLevelDelegate SetLevel;
        public static WriteEntryDelegate WriteEntry;

        #endregion


        // Private Methods...
        #region Private Methods...

        /// <summary>
        /// Close tracing...
        /// </summary>
        private static void CloseLocal()
        {
            if (!ms_blFirstPass)
            {
                Trace.Close();
                ms_filestream.Close();
                ms_filestream = null;
            }
            ms_blFirstPass = true;
            ms_blOpened = false;
            ms_blFlush = false;
            ms_iMessageNumber = 0;
        }

        /// <summary>
        /// Get the debugging level...
        /// </summary>
        /// <returns>the level</returns>
        private static int GetLevelLocal()
        {
            return (ms_iLevel);
        }

        /// <summary>
        /// Get the state...
        /// </summary>
        /// <returns>the level</returns>
        private static string GetStateLocal()
        {
            return ((ms_twain == null) ? "S1" : ms_twain.GetState().ToString());
        }

        /// <summary>
        /// Turn on the listener for our log file...
        /// </summary>
        /// <param name="a_szName">the name of our log</param>
        /// <param name="a_szPath">the path where we want our log to go</param>
        /// <param name="a_iLevel">debug level</param>
        private static void OpenLocal(string a_szName, string a_szPath, int a_iLevel)
        {
            string szLogFile;

            // Init stuff...
            ms_blFirstPass = true;
            ms_blOpened = true;
            ms_blFlush = false;
            ms_iMessageNumber = 0;
            ms_iLevel = a_iLevel;

            // Ask for a TWAINDSM log...
            if (a_iLevel > 0)
            {
                Environment.SetEnvironmentVariable("TWAINDSM_LOG", Path.Combine(a_szPath, "twaindsm.log"));
                Environment.SetEnvironmentVariable("TWAINDSM_MODE", "w");
            }

            // Backup old stuff...
            szLogFile = Path.Combine(a_szPath, a_szName);
            try
            {
                if (File.Exists(szLogFile + "_backup_2.log"))
                {
                    File.Delete(szLogFile + "_backup_2.log");
                }
                if (File.Exists(szLogFile + "_backup_1.log"))
                {
                    File.Move(szLogFile + "_backup_1.log", szLogFile + "_backup_2.log");
                }
                if (File.Exists(szLogFile + ".log"))
                {
                    File.Move(szLogFile + ".log", szLogFile + "_backup_1.log");
                }
            }
            catch
            {
                // Don't care, keep going...
            }

            // Turn on the listener...
            ms_filestream = File.Open(szLogFile + ".log", FileMode.Append, FileAccess.Write, FileShare.Read);
            Trace.Listeners.Add(new TextWriterTraceListener(ms_filestream, a_szName + "Listener"));
        }

        /// <summary>
        /// Register the TWAIN object so we can get some extra info...
        /// </summary>
        /// <param name="a_twain">twain object or null</param>
        private static void RegisterTwainLocal(TWAIN a_twain)
        {
            ms_twain = a_twain;
        }

        /// <summary>
        /// Flush data to the file...
        /// </summary>
        private static void SetFlushLocal(bool a_blFlush)
        {
            ms_blFlush = a_blFlush;
            if (a_blFlush)
            {
                Trace.Flush();
            }
        }

        /// <summary>
        /// Set the debugging level
        /// </summary>
        /// <param name="a_iLevel"></param>
        private static void SetLevelLocal(int a_iLevel)
        {
            // Squirrel this value away...
            ms_iLevel = a_iLevel;

            // One has to opt out of flushing, since the consequence
            // of turning it off often involves losing log data...
            if ((a_iLevel & c_iDebugNoFlush) == c_iDebugNoFlush)
            {
                SetFlush(false);
            }
            else
            {
                SetFlush(true);
            }
        }

        /// <summary>
        /// Do this for all of them...
        /// </summary>
        /// <param name="a_szMessage">The message</param>
        /// <param name="a_szSeverity">Message severity</param>
        /// <param name="a_blFlush">Flush it to disk</param>
        private static void WriteEntryLocal(string a_szSeverity, string a_szMessage, bool a_blFlush)
        {
            long lThreadId;

            // Filter...
            switch (a_szSeverity)
            {
                // Always log these, and always flush them to disk...
                case "A":
                case "E":
                case "W":
                    a_blFlush = true;
                    break;

                // Log informationals when bit-0 is set...
                case ".":
                    if ((ms_iLevel & c_iDebugInfo) != 0)
                    {
                        break;
                    }
                    return;

                // Log verbose when bit-1 is set...
                case "V":
                    if ((ms_iLevel & c_iDebugVerbose) != 0)
                    {
                        a_szSeverity = ".";
                        break;
                    }
                    return;

                // Log verbose data when bit-1 is set...
                case "D":
                    if ((ms_iLevel & c_iDebugVerboseData) != 0)
                    {
                        a_szSeverity = ".";
                        break;
                    }
                    return;
            }

            // Get our thread id...
            if (ms_blIsWindows)
            {
                lThreadId = NativeMethods.GetCurrentThreadId();
            }
            else
            {
                lThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            // First pass...
            if (ms_blFirstPass)
            {
                string szPlatform;

                // We're Windows...
                if (Environment.OSVersion.ToString().Contains("Microsoft Windows"))
                {
                    szPlatform = "windows";
                }

                // We're Mac OS X (this has to come before LINUX!!!)...
                else if (Directory.Exists("/Library/Application Support"))
                {
                    szPlatform = "macosx";
                }

                // We're Linux...
                else if (Environment.OSVersion.ToString().Contains("Unix"))
                {
                    szPlatform = "linux";
                }

                // We have a problem, Log will throw for us...
                else
                {
                    szPlatform = "unknown";
                }
                if (!ms_blOpened)
                {
                    // We'll assume they want logging, since they didn't tell us...
                    Open("Twain", ".", 1);
                }
                Trace.UseGlobalLock = true;
                ms_blFirstPass = false;
                Trace.WriteLine
                (
                    string.Format
                    (
                        "{0:D6} {1} {2} T{3:D8} V{4} ts:{5} os:{6}",
                        ms_iMessageNumber++,
                        DateTime.Now.ToString("HHmmssffffff"),
                        (ms_twain != null) ? ms_twain.GetState().ToString() : "S1",
                        lThreadId,
                        a_szSeverity.ToString(),
                        DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffffff"),
                        szPlatform
                    )
                );
            }

            // And log it...
            Trace.WriteLine
            (
                string.Format
                (
                    "{0:D6} {1} {2} T{3:D8} V{4} {5}",
                    ms_iMessageNumber++,
                    DateTime.Now.ToString("HHmmssffffff"),
                    (ms_twain != null) ? ms_twain.GetState().ToString() : "S1",
                    lThreadId,
                    a_szSeverity.ToString(),
                    a_szMessage
                )
            );

            // Flush it...
            if (a_blFlush)
            {
                Trace.Flush();
            }
        }

        #endregion


        // Private Definitions...
        #region Private Definitions

        /// <summary>
        /// LogLevel bitmask...
        /// </summary>
        private const int c_iDebugInfo = 0x0001;
        private const int c_iDebugVerbose = 0x0002;
        private const int c_iDebugVerboseData = 0x0004;
        private const int c_iDebugNoFlush = 0x0008;

        #endregion


        // Private Attributes...
        #region Private Attributes

        private static bool ms_blFirstPass = true;
        private static bool ms_blOpened = false;
        private static bool ms_blFlush = false;
        private static int ms_iMessageNumber = 0;
        private static int ms_iLevel = 0;
        private static TWAIN ms_twain = null;
        private static bool ms_blIsWindows = false;
        private static FileStream ms_filestream;

        #endregion
    }
}
