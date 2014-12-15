using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managed.Adb;

namespace TsdLib.Instrument.Ssh.AdbUtilities
{
    class StreamOutputReceiver : MultiLineReceiver
    {
        private Stream _outputStream;
        public StreamReader OutputStream;

        private static StreamOutputReceiver _instance = null;

        public static StreamOutputReceiver Instance 
        { 
            get 
            {
                if (_instance == null)
                    _instance = new StreamOutputReceiver();
                return _instance;
            } 
        }

        private StreamOutputReceiver()
            : base()
        {
            _outputStream = new MemoryStream();
            OutputStream = new StreamReader(_outputStream);
        }

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
