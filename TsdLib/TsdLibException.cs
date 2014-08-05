using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace TsdLib
{
    [Serializable]
    public abstract class TsdLibException : Exception
    {
        static void LogException(TsdLibException ex)
        {
            using (StreamWriter stream = new StreamWriter(ex.LogFile, false))
                stream.WriteLine(ex);
        }

        private string _logFile;
        public string LogFile
        {
            get { return _logFile ?? Path.Combine(SpecialFolders.Logs, DateTime.Now.ToString("MMM_dd_yyyy_HH-mm-ss") + ".txt"); }
            set { _logFile = value; }
        }

        protected TsdLibException(string message)
            : this(message, null) { }

        protected TsdLibException(string message, Exception inner)
            : base(message, inner)
        {
            Trace.WriteLine(this);
            LogException(this);
        }

        protected TsdLibException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Trace.WriteLine(this);
        }

#if !DEBUG
        public override string ToString()
        {
            return GetType().Name + ":" + Message;
        }
#endif

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("LogFile", LogFile, typeof (string));
        }
    }
}
