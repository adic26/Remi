using System;
using System.Runtime.Serialization;

namespace TsdLib.Instrument.Adb
{
    [Serializable]
    public class AdbConnectException : TsdLibException
    {
        public AdbConnectException(string message, Exception inner = null)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the CommandException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected AdbConnectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    [Serializable]
    public class AdbCommandException : CommandException
    {
        public AdbCommandException(AdbConnection connection, string command, string message, Exception inner = null)
            : base(connection, command, message, inner)
        {

        }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the AdbCommandException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected AdbCommandException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

}