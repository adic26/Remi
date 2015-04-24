using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace TsdLib.Instrument.Adb
{
    public class AdbConnection : ConnectionBase
    {
        private const int Timeout = 10000;
        private static readonly string[] commandSeparators = { ";" };
        private static readonly string workingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "platform-tools");
        private static readonly string adbExe = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "platform-tools", "adb.exe");
        
        private ProcessRunner _adbShellProcess;

        /// <summary>
        /// Gets an array of strings that are used to delimit commands placed on a single line.
        /// </summary>
        protected override string[] CommandSeparators
        {
            get { return commandSeparators; }
        }
        public AdbConnection(string address)
            : base(address)
        {
            _adbShellProcess = new ProcessRunner(adbExe, "shell", workingDirectory, "exit", Timeout);
        }

        protected override void Write(string command)
        {
            if (command.StartsWith("adb"))
                runNewAdbProcess(command);
            else
                _adbShellProcess.SendCommand(command);
        }

        //TODO: encapsulate this - it is the same as used in factory
        private void runNewAdbProcess(string command)
        {
            StringBuilder sbOut = new StringBuilder();
            StringBuilder sbErr = new StringBuilder();

            ProcessStartInfo _startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe"),
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                WorkingDirectory = @"platform-tools"
            };

            using (Process cmdProcess = Process.Start(_startInfo))
            using (ManualResetEvent mreOut = new ManualResetEvent(false), mreErr = new ManualResetEvent(false))
            {
                cmdProcess.OutputDataReceived += (o, e) =>
                {
                    if (e.Data == null) mreOut.Set();
                    else sbOut.AppendLine(e.Data);
                };
                cmdProcess.BeginOutputReadLine();
                cmdProcess.ErrorDataReceived += (o, e) =>
                {
                    if (e.Data == null) mreErr.Set();
                    else sbErr.AppendLine(e.Data);
                };
                cmdProcess.BeginErrorReadLine();

                cmdProcess.StandardInput.WriteLine(command);

                cmdProcess.StandardInput.Close();
                bool exited = cmdProcess.WaitForExit(Timeout);

                if (!exited)
                    throw new AdbCommandException(this, command, "The adb process did not exit after " + Timeout + " milliSeconds.");

                mreOut.WaitOne();
                mreErr.WaitOne();

                if (cmdProcess.ExitCode != 0)
                    throw new AdbCommandException(this, command, "The adb root process exited with code: " + cmdProcess.ExitCode);

                if (sbErr.Length > 0)
                    throw new AdbCommandException(this, command, "Error read from buffer");
            }


            bool restart = sbOut.ToString().Contains("restarting adbd");
            if (restart)
            {
                _adbShellProcess.Dispose();
                Trace.WriteLine("Waiting for adbd to restart");
                Thread.Sleep(2000);
                _adbShellProcess = new ProcessRunner(adbExe, "shell", workingDirectory, "exit", Timeout);
            }
        }

        protected override string ReadString()
        {
            return _adbShellProcess.ReadBuffer();
        }

        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }

        protected override bool CheckForError()
        {
            //TODO: need a safe way to read the error code CheckForError gets called after each command (before the read in a query) so it clears the buffer
            //Write("echo $?");
            //string errorCode = ReadString();

            return !IsConnected;
        }

        public override bool IsConnected
        {
            get { return _adbShellProcess.IsConnected; }
        }

        protected override void Dispose(bool disposing)
        {
            _adbShellProcess.Dispose();
            base.Dispose(disposing);
        }
    }
}
