using System;
using System.IO;
using Managed.Adb;

namespace TsdLib.Instrument.Adb.ManagedAdb
{
    /// <summary>
    /// class used to receive shell outputs from ADB shell
    /// </summary>
    [Obsolete("Only needed for ManagedAdb")]
    class StreamOutputReceiver : MultiLineReceiver
    {
        private readonly Stream _outputStream;
        public StreamReader OutputStream;

        private static StreamOutputReceiver _instance;
        /// <summary>
        /// Gets an instance of this class
        /// </summary>
        public static StreamOutputReceiver Instance 
        { 
            get { return _instance ?? (_instance = new StreamOutputReceiver()); }
        }
        /// <summary>
        /// private constructor. Instantiates the streams
        /// </summary>
        private StreamOutputReceiver()
        {
            _outputStream = new MemoryStream();
            OutputStream = new StreamReader(_outputStream);
        }
        /// <summary>
        /// Writes Shell output to the memory stream
        /// </summary>
        /// <param name="lines"></param>
        protected override void ProcessNewLines(string[] lines)
        {
            _outputStream.SetLength(0);

            StreamWriter sw = new StreamWriter(_outputStream);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("$"))
                    continue;
                sw.WriteLine(line);
            }
            sw.Flush();

            _outputStream.Position = 0;
        }
    }
}
