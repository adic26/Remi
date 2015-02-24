using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace TsdLib
{
    /// <summary>
    /// Base exception class for TsdLib applications.
    /// </summary>
    [Serializable]
    public abstract class TsdLibException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the TsdLibException class with a message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: Another exception that has caused the specified exception.</param>
        protected TsdLibException(string message, Exception inner = null)
            : base(message, inner)
        {

        }

        /// <summary>
        /// Gets a link to the database help content for this exception.
        /// </summary>
        public override string HelpLink
        {
            get { return "http://www.google.com/search?q=" + GetType().Name; }
        }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the <see cref="TsdLibException"/> class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected TsdLibException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
