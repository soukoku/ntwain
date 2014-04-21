using NTwain.Data;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace NTwain
{
    /// <summary>
    /// Represents an exception from calling a TWAIN triplet operation in the wrong state.
    /// </summary>
    [Serializable]
    public class TwainStateException : TwainException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TwainStateException"/> class.
        /// </summary>
        public TwainStateException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwainStateException"/> class.
        /// </summary>
        /// <param name="currentState">The actual state number.</param>
        /// <param name="minStateExpected">The minimum state allowed.</param>
        /// <param name="maxStateExpected">The maximum state allowed.</param>
        /// <param name="dataGroup">The data group used.</param>
        /// <param name="argumentType">The data argument type used.</param>
        /// <param name="twainMessage">The twain message used.</param>
        /// <param name="message">The message.</param>
        public TwainStateException(int currentState, int minStateExpected, int maxStateExpected, DataGroups dataGroup, DataArgumentType argumentType, Message twainMessage, string message)
            : base(default(ReturnCode), message)
        {
            ActualState = currentState;
            MinStateExpected = minStateExpected;
            MaxStateExpected = maxStateExpected;
            DataGroup = dataGroup;
            ArgumentType = argumentType;
            TwainMessage = twainMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwainStateException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected TwainStateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                MinStateExpected = info.GetInt32("MIN");
                MaxStateExpected = info.GetInt32("MAX");
                ActualState = info.GetInt32("State");
                DataGroup = (DataGroups)info.GetUInt32("DG");
                ArgumentType = (DataArgumentType)info.GetUInt16("DAT");
                TwainMessage = (Message)info.GetUInt16("MSG");
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic).
        /// </exception>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/>
        /// 	<IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/>
        /// </PermissionSet>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info != null)
            {
                info.AddValue("MIN", MinStateExpected);
                info.AddValue("MAX", MaxStateExpected);
                info.AddValue("State", ActualState);
                info.AddValue("DG", DataGroup);
                info.AddValue("DAT", ArgumentType);
                info.AddValue("MSG", TwainMessage);
            }
            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Gets the allowed minimum state.
        /// </summary>
        /// <value>The minimum.</value>
        public int MinStateExpected { get; private set; }
        /// <summary>
        /// Gets the allowed maximum state.
        /// </summary>
        /// <value>The maximum.</value>
        public int MaxStateExpected { get; private set; }
        /// <summary>
        /// Gets the actual state number.
        /// </summary>
        /// <value>The state.</value>
        public int ActualState { get; private set; }
        /// <summary>
        /// Gets the triplet data group.
        /// </summary>
        /// <value>The data group.</value>
        public DataGroups DataGroup { get; private set; }
        /// <summary>
        /// Gets the triplet data argument type.
        /// </summary>
        /// <value>The type of the argument.</value>
        public DataArgumentType ArgumentType { get; private set; }
        /// <summary>
        /// Gets the triplet message.
        /// </summary>
        /// <value>The twain message.</value>
        public Message TwainMessage { get; private set; }
    }
}
