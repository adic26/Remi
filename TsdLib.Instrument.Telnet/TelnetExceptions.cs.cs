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
        public TelnetException(string message)
            : base(message) { }

        /// <summary>
        /// Initialize a new TelnetException with a specified message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">The Exception that is the cause of the TelnetException.</param>
        public TelnetException(string message, Exception inner)
            : base(message, inner) { }
    }
}
