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
            using (StreamWriter stream = new StreamWriter(ex.LogFile, false))
                stream.WriteLine(ex + additional);
        }

        private string _logFile;
        /// <summary>
        /// Gets or sets the file path where exception details are recorded.
        /// </summary>
        public string LogFile
        {
            get { return _logFile ?? (_logFile = Path.Combine(SpecialFolders.Logs, DateTime.Now.ToString("MMM_dd_yyyy_HH-mm-ss") + ".txt")); }
        }

        /// <summary>
        /// Initializes a new instance of the TsdLibException class with a message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: Another exception that has caused the specified exception.</param>
        protected TsdLibException(string message, Exception inner = null)
            : base(message, inner)
        {
            string stackTrace = new StackTrace(2, true).ToString();
            string stackFrame = stackTrace.Split('\n')[0];

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
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the TsdLibException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected TsdLibException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// Sets the Runtime.Serialization.SerializationInfo with information about the exception.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("LogFile", LogFile, typeof (string));
        }
    }
}
