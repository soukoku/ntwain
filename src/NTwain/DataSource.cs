using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using NTwain.Data;

namespace NTwain
{
    /// <summary>
    /// A TWAIN data source.
    /// </summary>
    public partial class DataSource
    {
        internal readonly TwainSession Session;

        internal TW_IDENTITY Identity32 { get; }

        internal DataSource(TwainSession session, TW_IDENTITY src)
        {
            this.Session = session;
            this.Identity32 = src;
            ProtocolVersion = new Version(src.ProtocolMajor, src.ProtocolMinor);
        }

        /// <summary>
        /// Opens the source for capability negotiation.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Open() => Session.DGControl.Identity.OpenDS(Identity32);

        /// <summary>
        /// Closes the source.
        /// </summary>
        /// <returns></returns>
        public ReturnCode Close() => Session.DGControl.Identity.CloseDS(Identity32);

        /// <summary>
        /// Showing driver configuration UI if supported.
        /// </summary>
        /// <param name="windowHandle">The parent window handle if on Windows. Use <see cref="IntPtr.Zero"/> if not applicable.</param>
        /// <param name="modal">if set to <c>true</c> the driver UI may be displayed as modal.</param>
        /// <returns></returns>
        public ReturnCode ShowUI(IntPtr windowHandle, bool modal = false)
        {
            var ui = new TW_USERINTERFACE
            {
                ShowUI = 1,
                ModalUI = (ushort)(modal ? 1 : 0),
                hParent = windowHandle
            };
            Session._disableDSNow = false;
            var rc = Session.DGControl.UserInterface.EnableDSUIOnly(ref ui);
            if (rc == ReturnCode.Success)
            {
                Session._lastEnableUI = ui;
            }
            return rc;
        }

        /// <summary>
        /// Enables the source for transferring data.
        /// </summary>
        /// <param name="showUI">if set to <c>true</c> then show driver UI. Not all sources support turning UI off.</param>
        /// <param name="windowHandle">The parent window handle if on Windows. Use <see cref="IntPtr.Zero"/> if not applicable.</param>
        /// <param name="modal">if set to <c>true</c> any driver UI may be displayed as modal.</param>
        /// <returns></returns>
        public ReturnCode Enable(bool showUI, IntPtr windowHandle, bool modal = false)
        {
            var ui = new TW_USERINTERFACE
            {
                ShowUI = (ushort)(showUI ? 1 : 0),
                ModalUI = (ushort)(modal ? 1 : 0),
                hParent = windowHandle
            };
            Session._disableDSNow = false;
            var rc = Session.DGControl.UserInterface.EnableDS(ref ui);
            if (rc == ReturnCode.Success)
            {
                Session._lastEnableUI = ui;
            }
            return rc;
        }

        /// <summary>
        /// Gets the source status. Useful after getting a non-success return code.
        /// </summary>
        /// <returns></returns>
        public TW_STATUS GetStatus()
        {
            TW_STATUS stat = default;
            var rc = Session.DGControl.Status.Get(ref stat, this);
            return stat;
        }


        /// <summary>
        /// Gets or sets the opaque source settings (aka TW_CUSTOMDSDATA) of this source if supported.
        /// </summary>
        public byte[] CustomDSData
        {
            // TODO: check if memory mgmt is correct
            get
            {
                byte[] value = null;

                TW_CUSTOMDSDATA data = default;
                if (Session.DGControl.CustomDSData.Get(ref data) == ReturnCode.Success && data.InfoLength > 0)
                {
                    try
                    {
                        value = new byte[data.InfoLength];
                        var ptr = Session.Config.MemoryManager.Lock(data.hData);
                        Marshal.Copy(ptr, value, 0, (int)data.InfoLength);
                    }
                    finally
                    {
                        Session.Config.MemoryManager.Unlock(data.hData);
                        Session.Config.MemoryManager.Free(data.hData);
                    }
                }
                return value;
            }
            set
            {
                if (value != null && value.Length > 0)
                {
                    var data = new TW_CUSTOMDSDATA
                    {
                        InfoLength = (uint)value.Length
                    };
                    try
                    {
                        data.hData = Session.Config.MemoryManager.Allocate(data.InfoLength);
                        var ptr = Session.Config.MemoryManager.Lock(data.hData);
                        Marshal.Copy(value, 0, ptr, value.Length);
                        var rc = Session.DGControl.CustomDSData.Set(ref data);
                        if (rc != ReturnCode.Success)
                        {
                            // do something
                        }
                    }
                    finally
                    {
                        if (data.hData != IntPtr.Zero)
                        {
                            Session.Config.MemoryManager.Unlock(data.hData);
                            Session.Config.MemoryManager.Free(data.hData);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{Name} {Version}";
        }
    }
}
