using System;
using System.Runtime.InteropServices;
using System.Text;
using NTwain.Data;
using NTwain.Internals;

namespace NTwain.Triplets.Control
{
    sealed class StatusUtf8 : BaseTriplet
    {
        internal StatusUtf8(TwainSession session) : base(session) { }

        public ReturnCode Get(ref TW_STATUS status, out string message)
        {
            message = null;
            var rc = ReturnCode.Failure;

            TW_STATUSUTF8 real = new TW_STATUSUTF8 { Status = status };
            if (Use32BitData)
            {
                rc = NativeMethods.Dsm32(Session.Config.App32, Session.CurrentSource?.Identity,
                    DataGroups.Control, DataArgumentType.StatusUtf8, Message.Get, ref real);
            }

            if (rc == ReturnCode.Success)
            {
                message = ReadUtf8String(ref real);
            }

            return rc;
        }


        private string ReadUtf8String(ref TW_STATUSUTF8 status)
        {
            if (status.UTF8string != IntPtr.Zero)
            {
                try
                {
                    IntPtr lockedPtr = Session.Config.MemoryManager.Lock(status.UTF8string);
                    var buffer = new byte[status.Size - 1];

                    Marshal.Copy(lockedPtr, buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(buffer);

                }
                finally
                {
                    Session.Config.MemoryManager.Unlock(status.UTF8string);
                    Session.Config.MemoryManager.Free(status.UTF8string);
                }
            }
            return null;
        }
    }
}