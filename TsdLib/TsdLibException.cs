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
        static void LogException(TsdLibException ex)
        {
            using (StreamWriter stream = new StreamWriter(ex.LogFile, false))
                stream.WriteLine(ex);
        }

        private string _logFile;
        /// <summary>
        /// Gets or sets the file path where exception details are recorded.
        /// </summary>
        public string LogFile
        {
            get { return _logFile ?? Path.Combine(SpecialFolders.Logs, DateTime.Now.ToString("MMM_dd_yyyy_HH-mm-ss") + ".txt"); }
            set { _logFile = value; }
        }

        /// <summary>
        /// Initializes a new instance of the TsdLibException class with a message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">OPTIONAL: Another exception that has caused the specified exception.</param>
        protected TsdLibException(string message, Exception inner = null)
            : base(message, inner)
        {
            Trace.WriteLine(ToString());
            LogException(this);
        }

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
