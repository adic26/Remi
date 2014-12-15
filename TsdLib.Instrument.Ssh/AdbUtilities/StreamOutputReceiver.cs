using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.Adb;

namespace TsdLib.Instrument.Ssh.AdbUtilities
{
    /// <summary>
    /// class used to receive shell outputs from ADB shell
    /// </summary>
    class StreamOutputReceiver : MultiLineReceiver
    {
        private Stream _outputStream;
        public StreamReader OutputStream;

        private static StreamOutputReceiver _instance = null;
        /// <summary>
        /// Gets an instance of this class
        /// </summary>
        public static StreamOutputReceiver Instance 
        { 
            get 
            {
                if (_instance == null)
                    _instance = new StreamOutputReceiver();
                return _instance;
            } 
        }
        /// <summary>
        /// private constructor. Instantiates the streams
        /// </summary>
        private StreamOutputReceiver()
            : base()
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
