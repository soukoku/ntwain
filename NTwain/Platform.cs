using NTwain.Triplets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Class for checking various platform requirements and conditions.
    /// </summary>
    public static class Platform
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static Platform()
        {
            IsApp64bit = IntPtr.Size == 8;

            IsOnMono = Type.GetType("Mono.Runtime") != null;
            IsWin = Environment.OSVersion.Platform == PlatformID.Win32NT;
            IsLinux = Environment.OSVersion.Platform == PlatformID.Unix;

            if (IsWin)
            {
                var newDsmPath = Path.Combine(Environment.SystemDirectory, Dsm.WIN_NEW_DSM_NAME);
                var oldDsmPath = Path.Combine(Environment.SystemDirectory, Dsm.WIN_OLD_DSM_NAME);

                if (IsApp64bit)
                {
                    IsSupported = DsmExists = File.Exists(newDsmPath);
                    UseNewWinDSM = true;
                }
                else
                {
                    if (File.Exists(newDsmPath))
                    {
                        UseNewWinDSM = IsSupported = DsmExists = true;
                    }
                    else
                    {
                        IsSupported = DsmExists = File.Exists(oldDsmPath);
                    }
                }
            }
            else if (IsLinux)
            {
                DsmExists = File.Exists(Dsm.LINUX_DSM_PATH);
                IsSupported = DsmExists && IsOnMono;
            }
            else
            {
                // mac? not gonna happen
            }

            _defaultMemManager = new WinMemoryManager();
        }

        // prefer the use of the twain dsm on windows.
        internal static readonly bool UseNewWinDSM;
        internal static readonly bool IsOnMono;
        internal static readonly bool IsWin;
        internal static readonly bool IsLinux;

        /// <summary>
        /// Gets a value indicating whether the application is running in 64-bit.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application is 64-bit; otherwise, <c>false</c>.
        /// </value>
        public static bool IsApp64bit { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the applicable TWAIN DSM library exists in the operating system.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the TWAIN DSM; otherwise, <c>false</c>.
        /// </value>
        public static bool DsmExists { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this library is supported on current OS.
        /// Check the other platform properties to determine the reason if this is false.
        /// </summary>
        /// <value>
        /// <c>true</c> if this library is supported; otherwise, <c>false</c>.
        /// </value>
        public static bool IsSupported { get; private set; }


        static readonly IMemoryManager _defaultMemManager;
        static IMemoryManager _specifiedMemManager;

        /// <summary>
        /// Gets the <see cref="IMemoryManager"/> for communicating with data sources.
        /// This should only be used when a <see cref="TwainSession"/> is open.
        /// </summary>
        /// <value>
        /// The memory manager.
        /// </value>
        public static IMemoryManager MemoryManager
        {
            get
            {
                if (_specifiedMemManager == null) { return _defaultMemManager; }
                return _specifiedMemManager;
            }
            internal set
            {
                _specifiedMemManager = value;
            }
        }
    }
}
