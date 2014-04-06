using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTwain.Triplets;
using NTwain.Data;
using NTwain.Values;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Permissions;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace NTwain
{
    /// <summary>
    /// Provides a session for working with TWAIN api in an application.
    /// This is the old implementation for reference purposes only.
    /// </summary>
    [Obsolete("For reference purposes only.")]
    public class TwainSessionOld : TwainSessionBase, IMessageFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainSessionOld" /> class.
        /// </summary>
        /// <param name="appId">The app id.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TwainSessionOld(TWIdentity appId) : base(appId) { }

        protected override void DoTransferRoutine()
        {
            TWPendingXfers pending = new TWPendingXfers();
            var rc = ReturnCode.Success;

            do
            {
                IList<FileFormat> formats = Enumerable.Empty<FileFormat>().ToList();
                IList<Compression> compressions = Enumerable.Empty<Compression>().ToList();
                bool canDoFileXfer = this.CapGetImageXferMech().Contains(XferMech.File);
                var curFormat = this.GetCurrentCap<FileFormat>(CapabilityId.ICapImageFileFormat);
                var curComp = this.GetCurrentCap<Compression>(CapabilityId.ICapCompression);
                TWImageInfo imgInfo;
                bool skip = false;
                if (DGImage.ImageInfo.Get(out imgInfo) != ReturnCode.Success)
                {
                    // bad!
                    skip = true;
                }

                try
                {
                    formats = this.CapGetImageFileFormat();
                }
                catch { }
                try
                {
                    compressions = this.CapGetCompression();
                }
                catch { }

                // ask consumer for cancel in case of non-ui multi-page transfers
                TransferReadyEventArgs args = new TransferReadyEventArgs(pending, formats, curFormat, compressions,
                    curComp, canDoFileXfer, imgInfo);
                args.CancelCurrent = skip;

                OnTransferReady(args);


                if (!args.CancelAll && !args.CancelCurrent)
                {
                    Values.XferMech mech = this.GetCurrentCap<XferMech>(CapabilityId.ICapXferMech);

                    if (args.CanDoFileXfer && !string.IsNullOrEmpty(args.OutputFile))
                    {
                        var setXferRC = DGControl.SetupFileXfer.Set(new TWSetupFileXfer
                        {
                            FileName = args.OutputFile,
                            Format = args.ImageFormat
                        });
                        if (setXferRC == ReturnCode.Success)
                        {
                            mech = XferMech.File;
                        }
                    }

                    // I don't know how this is supposed to work so it probably doesn't
                    //this.CapSetImageFormat(args.ImageFormat);
                    //this.CapSetImageCompression(args.ImageCompression);

                    #region do xfer

                    // TODO: expose all swallowed exceptions somehow later

                    IntPtr dataPtr = IntPtr.Zero;
                    IntPtr lockedPtr = IntPtr.Zero;
                    string file = null;
                    try
                    {
                        ReturnCode xrc = ReturnCode.Cancel;
                        switch (mech)
                        {
                            case Values.XferMech.Native:
                                xrc = DGImage.ImageNativeXfer.Get(ref dataPtr);
                                break;
                            case Values.XferMech.File:
                                xrc = DGImage.ImageFileXfer.Get();
                                if (File.Exists(args.OutputFile))
                                {
                                    file = args.OutputFile;
                                }
                                break;
                            case Values.XferMech.MemFile:
                                // not supported yet
                                //TWImageMemXfer memxfer = new TWImageMemXfer();
                                //xrc = DGImage.ImageMemXfer.Get(memxfer);
                                break;
                        }
                        if (xrc == ReturnCode.XferDone)
                        {
                            State = 7;
                            if (dataPtr != IntPtr.Zero)
                            {
                                lockedPtr = MemoryManager.Instance.Lock(dataPtr);
                            }
                            OnDataTransferred(new DataTransferredEventArgs(lockedPtr, file));
                        }
                        //}
                        //else if (group == DataGroups.Audio)
                        //{
                        //	var xrc = DGAudio.AudioNativeXfer.Get(ref dataPtr);
                        //	if (xrc == ReturnCode.XferDone)
                        //	{
                        //		State = 7;
                        //		try
                        //		{
                        //			var dtHand = DataTransferred;
                        //			if (dtHand != null)
                        //			{
                        //				lockedPtr = MemoryManager.Instance.MemLock(dataPtr);
                        //				dtHand(this, new DataTransferredEventArgs(lockedPtr));
                        //			}
                        //		}
                        //		catch { }
                        //	}
                        //}
                    }
                    finally
                    {
                        State = 6;
                        // data here is allocated by source so needs to use shared mem calls
                        if (lockedPtr != IntPtr.Zero)
                        {
                            MemoryManager.Instance.Unlock(lockedPtr);
                            lockedPtr = IntPtr.Zero;
                        }
                        if (dataPtr != IntPtr.Zero)
                        {
                            MemoryManager.Instance.Free(dataPtr);
                            dataPtr = IntPtr.Zero;
                        }
                    }
                    #endregion
                }

                if (args.CancelAll)
                {
                    rc = DGControl.PendingXfers.Reset(pending);
                    if (rc == ReturnCode.Success)
                    {
                        // if audio exit here
                        //if (group == DataGroups.Audio)
                        //{
                        //	//???
                        //	return;
                        //}

                    }
                }
                else
                {
                    rc = DGControl.PendingXfers.EndXfer(pending);
                }
            } while (rc == ReturnCode.Success && pending.Count != 0);

            State = 5;
            DisableSource();

        }

        #region messaging use

        /// <summary>
        /// Message loop processor for winform. 
        /// Use this by adding the <see cref="TwainSessionOld"/> as an <see cref="IMessageFilter "/>.
        /// </summary>
        /// <param name="m">The message to be dispatched. You cannot modify this message.</param>
        /// <returns>
        /// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.
        /// </returns>
        //[EnvironmentPermissionAttribute(SecurityAction.LinkDemand)]
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        bool IMessageFilter.PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            MESSAGE winmsg = default(MESSAGE);
            winmsg.hwnd = m.HWnd;
            winmsg.lParam = m.LParam;
            winmsg.message = (uint)m.Msg;
            winmsg.wParam = m.WParam;

            return HandleWndProcMessage(ref winmsg);
        }

        /// <summary>
        /// Message loop processor for WPF.
        /// </summary>
        /// <param name="hwnd">The window handle.</param>
        /// <param name="msg">The message ID.</param>
        /// <param name="wParam">The message's wParam value.</param>
        /// <param name="lParam">The message's lParam value.</param>
        /// <param name="handled">A value that indicates whether the message was handled. Set the value to true if the message was handled; otherwise, false.</param>
        /// <returns></returns>
        [EnvironmentPermissionAttribute(SecurityAction.LinkDemand)]
        public IntPtr PreFilterMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            MESSAGE winmsg = default(MESSAGE);
            winmsg.hwnd = hwnd;
            winmsg.lParam = lParam;
            winmsg.message = (uint)msg;
            winmsg.wParam = wParam;

            handled = base.HandleWndProcMessage(ref winmsg);

            return IntPtr.Zero;
        }

        #endregion

    }
}
