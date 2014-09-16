using System;

namespace TsdLib.Instrument.Telnet
{
    /// <summary>
    /// Exception due to a communication failure with a Telnet-based instrument.
    /// </summary>
    [Serializable]
    public class TelnetException : TsdLibException
    {
        /// <summary>
        /// Initialize a new TelnetException with a specified message.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the TelnetException.</param>
        public TelnetException(string message, Exception inner = null)
            : base(message, inner) { }

        ///// <summary>
        ///// Deserialization constructor used by the .NET Framework to initialize an instance of the TelnetException class from serialized data.
        ///// </summary>
        ///// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        ///// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        //protected TelnetException(SerializationInfo info, StreamingContext context)
        //    : base(info, context) { }
    }
}
