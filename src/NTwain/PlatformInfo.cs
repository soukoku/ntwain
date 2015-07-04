using NTwain.Internals;
using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Contains various platform requirements and conditions for TWAIN.
    /// </summary>
    public class PlatformInfo : IPlatformInfo
    {
        static readonly PlatformInfo __global = new PlatformInfo();
        internal static PlatformInfo InternalCurrent { get { return __global; } }
        /// <summary>
        /// Gets the current platform info related to TWAIN.
        /// </summary>
        /// <value>
        /// The current info.
        /// </value>
        public static IPlatformInfo Current { get { return __global; } }


        PlatformInfo()
        {
            IsApp64Bit = IntPtr.Size == 8;

            IsOnMono = Type.GetType("Mono.Runtime") != null;
            IsWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            IsLinux = Environment.OSVersion.Platform == PlatformID.Unix;
            _defaultLog = new TraceLog();
            if (IsWindows)
            {
                _defaultMemManager = new WinMemoryManager();

                newDsmPath = Path.Combine(Environment.SystemDirectory, Dsm.WIN_NEW_DSM_NAME);
#if NET35
                oldDsmPath = Path.Combine(Environment.GetEnvironmentVariable("windir"), Dsm.WIN_OLD_DSM_NAME);
#else
                oldDsmPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), Dsm.WIN_OLD_DSM_NAME);
#endif
                PreferNewDSM = true;
            }
            else if (IsLinux)
            {
                _defaultMemManager = new LinuxMemoryManager();

                ExpectedDsmPath = Dsm.LINUX_DSM_PATH;
                DsmExists = File.Exists(ExpectedDsmPath);
                IsSupported = DsmExists && IsOnMono;
            }
            else
            {
                // mac? not gonna happen
            }
        }

        string oldDsmPath;
        string newDsmPath;

        private bool _preferNewDSM;

        /// <summary>
        /// Gets a value indicating whether to prefer using the new DSM on Windows over old twain_32 dsm if applicable.
        /// </summary>
        /// <value>
        ///   <c>true</c> to prefer new DSM; otherwise, <c>false</c>.
        /// </value>
        public bool PreferNewDSM
        {
            get { return _preferNewDSM; }
            set
            {
                if (IsWindows)
                {
                    _preferNewDSM = value;

                    if (IsApp64Bit)
                    {
                        ExpectedDsmPath = newDsmPath;
                        IsSupported = DsmExists = File.Exists(ExpectedDsmPath);
                        UseNewWinDSM = true;
                        Log.Debug("Using new dsm in windows.");
                    }
                    else
                    {
                        if (_preferNewDSM && File.Exists(newDsmPath))
                        {
                            ExpectedDsmPath = newDsmPath;
                            UseNewWinDSM = IsSupported = DsmExists = true;
                            Log.Debug("Using new dsm in windows.");
                        }
                        else
                        {
                            ExpectedDsmPath = oldDsmPath;
                            IsSupported = DsmExists = File.Exists(ExpectedDsmPath);
                            UseNewWinDSM = false;
                            Log.Debug("Using old dsm in windows.");
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Gets a value indicating whether the lib is expecting to use new DSM.
        /// </summary>
        /// <value>
        ///   <c>true</c> if using the new DSM; otherwise, <c>false</c>.
        /// </value>
        public bool UseNewWinDSM { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current runtime is mono.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current runtime is on mono; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnMono { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the current OS is windows.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current OS is windows; otherwise, <c>false</c>.
        /// </value>
        public bool IsWindows { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the current OS is linux.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current OS is linux; otherwise, <c>false</c>.
        /// </value>
        public bool IsLinux { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the application is running in 64-bit.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application is 64-bit; otherwise, <c>false</c>.
        /// </value>
        public bool IsApp64Bit { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the applicable TWAIN DSM library exists in the operating system.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the TWAIN DSM; otherwise, <c>false</c>.
        /// </value>
        public bool DsmExists { get; private set; }

        /// <summary>
        /// Gets the expected TWAIN DSM dll path.
        /// </summary>
        /// <value>
        /// The expected DSM path.
        /// </value>
        public string ExpectedDsmPath { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this library is supported on current OS.
        /// Check the other platform properties to determine the reason if this is false.
        /// </summary>
        /// <value>
        /// <c>true</c> if this library is supported; otherwise, <c>false</c>.
        /// </value>
        public bool IsSupported { get; private set; }


        readonly IMemoryManager _defaultMemManager;
        IMemoryManager _specifiedMemManager;

        /// <summary>
        /// Gets the <see cref="IMemoryManager"/> for communicating with data sources.
        /// This should only be used when a <see cref="TwainSession"/> is open.
        /// </summary>
        /// <value>
        /// The memory manager.
        /// </value>
        public IMemoryManager MemoryManager
        {
            get
            {
                return _specifiedMemManager ?? _defaultMemManager;
            }
            internal set
            {
                _specifiedMemManager = value;
            }
        }


        readonly ILog _defaultLog;
        private ILog _log;

        /// <summary>
        /// Gets or sets the log used by NTwain.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public ILog Log
        {
            get { return _log ?? _defaultLog; }
            set { _log = value; }
        }

    }
}
