using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TsdLib.Instrument.Adb
{
    public class AdbConnection : ConnectionBase
    {
        private readonly Process cmdProcess;
        //private readonly StringBuilder sbOut = new StringBuilder();

        private readonly ConcurrentStack<string> outputStack = new ConcurrentStack<string>();

        private readonly EventWaitHandle mreOut = new AutoResetEvent(false);
        private readonly EventWaitHandle mreErr = new AutoResetEvent(false);

        public AdbConnection(string address)
            : base(address)
        {
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

            cmdProcess = Process.Start(_startInfo);

            cmdProcess.OutputDataReceived += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                    mreOut.Set();
                else
                    //sbOut.AppendLine(e.Data);
                    outputStack.Push(e.Data);
            };
            cmdProcess.BeginOutputReadLine();
            cmdProcess.ErrorDataReceived += (o, e) =>
            {
                if (string.IsNullOrWhiteSpace(e.Data))
                    mreErr.Set();
                else
                {
                    bool gotOutputWithinFiveSeconds = mreOut.WaitOne(5000);
                    string command;
                    bool success = outputStack.TryPop(out command);
                    throw new AdbCommandException(this, command, e.Data);
                }
            };
            cmdProcess.BeginErrorReadLine();

            mreOut.WaitOne();

            //if (cmdProcess.ExitCode != 0)
            //    throw new AdbConnectException("The adb devices process exited with code: " + cmdProcess.ExitCode);
            

            //if (sbErr.Length > 0)
            //    throw new AdbConnectException("The adb devices process generated the error: " + sbErr);

            //IEnumerable<string> devs = sbOut.ToString()
            //    .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            //    .Select(line => Regex.Match(line, @"\w{8}(?=\sdevice)"))
            //    .Where(match => match.Success)
            //    .Select(match => match.Value)
            //    ;

        }

        protected override void Write(string message)
        {
            outputStack.Clear();
            cmdProcess.StandardInput.WriteLine("adb -s " + Address + " shell \"" + message + "\"" );

            mreOut.WaitOne();
            //cmdProcess.StandardInput.Close();
            //bool exited = cmdProcess.WaitForExit(5000);

            //if (!exited)
            //    throw new AdbConnectException("The adb devices process did not exit after 5 seconds.");


        }

        protected override string ReadString()
        {
            string data;
            bool success = outputStack.TryPop(out data);
            outputStack.Clear();
            return data;
        }

        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }

        protected override bool CheckForError()
        {
            return false;
        }

        public override bool IsConnected
        {
            get { return true; }
        }

        protected override void Dispose(bool disposing)
        {
            cmdProcess.StandardInput.Close();
            cmdProcess.StandardOutput.Close();
            cmdProcess.StandardError.Close();
            cmdProcess.Close();
            cmdProcess.Dispose();
            base.Dispose(disposing);
        }
    }
}
