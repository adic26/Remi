using System;
using System.Diagnostics;
using System.Text;

namespace TsdLib.Instrument.Adb
{
    ///// <summary>
    ///// Contains functionality to communicate with an instrument using adb.exe.
    ///// </summary>
    public class AdbConnection : ConnectionBase
    {
        private readonly string _adbExe;
        private readonly StringBuilder _output;
        private readonly StringBuilder _error;
        private int _exitCode;

        internal AdbConnection(string address, string adbFileLocation)
            : base(address)
        {
            _output = new StringBuilder();
            _error = new StringBuilder();
            _adbExe = adbFileLocation;
        }

        protected override void Write(string message)
        {
            _output.Clear();
            _error.Clear();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = string.Format(@"/c {0} -s {1} shell ""{2}""", _adbExe, Address, message),
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            Process process = Process.Start(startInfo);
            if (process== null || !process.WaitForExit(5000))
                throw new Exception(string.Format("Command failed: {0}. Timeout waiting for Adb process to exit", message));

            _output.Append(process.StandardOutput.ReadToEnd().Trim());
            _error.Append(process.StandardError.ReadToEnd().Trim());

            _exitCode = process.ExitCode;
        }

        protected override string ReadString()
        {
            string response = _output.ToString();
            _output.Clear();
            return response;
        }

        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }

        protected override bool CheckForError()
        {
            return _exitCode != 0 || _error.Length != 0;
        }

        public override bool IsConnected
        {
            get { return true; }
        }
    }
}
