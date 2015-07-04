using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NTwain.Internals
{
    class TraceLog : ILog
    {
        public TraceLog()
        {
            IsInfoEnabled = true;
            IsErrorEnabled = true;
        }

        public bool IsInfoEnabled { get; set; }

        public bool IsDebugEnabled { get; set; }

        public bool IsErrorEnabled { get; set; }


        public void Info(string message)
        {
            if (IsInfoEnabled && message != null)
            {
                System.Diagnostics.Trace.WriteLine(message, "Info");
            }
        }

        public void Info(string messageFormat, params object[] args)
        {
            Debug(string.Format(CultureInfo.CurrentCulture, messageFormat, args));
        }

        public void Debug(string message)
        {
            if (IsDebugEnabled && message != null)
            {
                System.Diagnostics.Trace.WriteLine(message, "Debug");
            }
        }

        public void Debug(string messageFormat, params object[] args)
        {
            Debug(string.Format(CultureInfo.CurrentCulture, messageFormat, args));
        }

        public void Error(string message)
        {
            if (IsErrorEnabled && message != null)
            {
                System.Diagnostics.Trace.WriteLine(message, "Error");
            }
        }

        public void Error(string message, Exception exception)
        {
            if (exception == null)
            {
                Error(message);
            }
            else
            {
                Error(message + Environment.NewLine + exception.ToString());
            }
        }

        public void Error(string messageFormat, Exception exception, params object[] args)
        {
            if (exception == null)
            {
                Error(string.Format(CultureInfo.CurrentCulture, messageFormat, args));
            }
            else
            {
                Error(string.Format(CultureInfo.CurrentCulture, messageFormat, args) + Environment.NewLine + exception.ToString());
            }
        }
    }
}
