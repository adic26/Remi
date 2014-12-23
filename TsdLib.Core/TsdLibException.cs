using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace TsdLib
{
    /// <summary>
    /// Base exception class for TsdLib applications. Adds logging and lookup functionality to standard exception behaviour.
    /// </summary>
    [Serializable]
    public abstract class TsdLibException : Exception
    {
        static void LogException(TsdLibException ex, string additional = "")
        {
            using (StreamWriter stream = new StreamWriter(SpecialFolders.ErrorLogs.FullName, true))
                stream.WriteLine(ex + additional);
        }

        /// <summary>
        /// Initializes a new instance of the TsdLibException class with a message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: Another exception that has caused the specified exception.</param>
        /// <param name="supressTrace">OPTIONAL: Pass true to prevent the exception information from being written to the <see cref="Trace"/>.</param>
        protected TsdLibException(string message, Exception inner = null, bool supressTrace = false)
            : base(message, inner)
        {
            string stackTrace = new StackTrace(2, true).ToString();
            string stackFrame = stackTrace.Split('\n')[0];

            if (!supressTrace)
                Trace.WriteLine(this + stackFrame);
            LogException(this, stackTrace);
        }

        /// <summary>
        /// Gets a link to the database help content for this exception.
        /// </summary>
        public override string HelpLink
        {
            get { return "http://www.google.com/search?q=" + GetType().Name; }
        }

        /// <summary>
        /// Gets a string representation of the exception.
        /// </summary>
        /// <returns>A string containing the type of exception, message and stack trace.</returns>
        public override sealed string ToString()
        {
            return base.ToString();
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
