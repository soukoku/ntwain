using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTwain
{
    /// <summary>
    /// Simple log interface used by NTwain.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Gets or sets a value indicating whether info messages will be logged.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable info logging.
        /// </value>
        bool IsInfoEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether debug messages will be logged.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable debug logging.
        /// </value>
        bool IsDebugEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether error messages will be logged.
        /// </summary>
        /// <value>
        /// <c>true</c> to enable error logging.
        /// </value>
        bool IsErrorEnabled { get; set; }

        /// <summary>
        /// Logs info type message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        /// <summary>
        /// Logs info type message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The arguments.</param>
        void Info(string messageFormat, params object[] args);

        /// <summary>
        /// Logs debug type message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Debug(string message);
        /// <summary>
        /// Logs debug type message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="args">The arguments.</param>
        void Debug(string messageFormat, params object[] args);

        /// <summary>
        /// Logs error type message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);
        
        /// <summary>
        /// Logs error type message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Logs error type message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="args">The arguments.</param>
        void Error(string messageFormat, Exception exception, params object[] args);
    }
}
