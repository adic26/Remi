using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace TsdLib.Instrument.Adb
{
    public class AdbConnection : ConnectionBase
    {
        private readonly StringBuilder _output;
        private readonly StringBuilder _error;

        internal AdbConnection(string address)
            : base(address)
        {
            _output = new StringBuilder();
            _error = new StringBuilder();
        }

        protected override void Write(string message)
        {
            _output.Clear();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = @"platform-tools\adb.exe",
                Arguments = "-s " + Address + " shell " + message,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            using (Process process = Process.Start(startInfo))
            {
                if (process == null)
                    throw new AdbConnectException("Could not start adb process");
                using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                {
                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                            outputWaitHandle.Set();
                        else if (!string.IsNullOrEmpty(e.Data))
                            _output.Append(e.Data.Trim());
                    };
                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (e.Data == null)
                            errorWaitHandle.Set();
                        else if (!string.IsNullOrEmpty(e.Data))
                            _error.Append(e.Data.Trim());
                    };

                    process.Start();

                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    if (process.WaitForExit(5000) && outputWaitHandle.WaitOne(5000) && errorWaitHandle.WaitOne(5000))
                    {
                        if (process.ExitCode != 0)
                            throw new AdbCommandException(this, message, "Adb process exited with code " + process.ExitCode);
                    }
                    else
                    {
                        process.Kill();
                        throw new AdbCommandException(this, message, "Timeout waiting for Adb process to exit");
                    }
                }
            }


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
            return _error.Length != 0;
        }

        public override bool IsConnected
        {
            get { return true; }
        }
    }
}
