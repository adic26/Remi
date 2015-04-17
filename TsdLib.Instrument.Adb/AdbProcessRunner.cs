using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TsdLib.Instrument.Adb
{
    public class ProcessRunner : IDisposable
    {
        private readonly Process _process;
        private string _lastCommand;
        private readonly string _exitCommand;
        private readonly int _timeoutMilliseconds;

        private readonly ConcurrentStack<string> _stack = new ConcurrentStack<string>();

        public event EventHandler<string> OutputDataReceived;

        protected void OnOutputDataReceived(string data)
        {
            var handler = OutputDataReceived;
            if (handler != null)
                handler(this, data);
        }

        public ProcessRunner(string exe, string arguments, string workingDirectory, string exitCommand, int timeoutMilliseconds)
        {
            ProcessStartInfo _startInfo = new ProcessStartInfo
            {
                Arguments = arguments,

                FileName = exe,
                WorkingDirectory = workingDirectory,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };

            _exitCommand = exitCommand;

            _timeoutMilliseconds = timeoutMilliseconds;

            _process = Process.Start(_startInfo);
            if (_process == null)
                throw new Exception("Could not start the process " + _process.ProcessName);

            _process.OutputDataReceived += (o, e) =>
            {
                if (e.Data == null)
                { }
                else
                    _stack.Push(e.Data);
            };
            _process.BeginOutputReadLine();

            _process.ErrorDataReceived += (o, e) =>
            {
                if (e.Data == null)
                { }
                else
                    throw new Exception(string.Format("Error from process {0}: On command: {1} Details: {2}", _process.ProcessName, _lastCommand, e.Data));
            };
            _process.BeginErrorReadLine();
        }

        public void SendCommand(string command)
        {
            
            _lastCommand = command;
            _process.StandardInput.WriteLine(command);

        }

        public string ReadBuffer()
        {
            List<string> data = new List<string>();

            string line;


            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                //Give the async DataReceived event some time to push the latest data onto the stack
                //If stale data is still on the stack, the line.Contains method could go further down into the stale commands
                Thread.Sleep(400);

                if (_stack.TryPop(out line))
                {
                    if (line.Contains(_lastCommand))
                        break;
                    if (!string.IsNullOrWhiteSpace(line))
                        data.Insert(0, line);
                }

                else
                    Trace.Write(".");

                if (sw.ElapsedMilliseconds > _timeoutMilliseconds)
                    return "Could not read any data from the queue";
            }
            _stack.Clear();
            return string.Join(Environment.NewLine, data);
        }

        public bool IsConnected
        {
            get { return true; }
        }

        public bool WaitForExit()
        {

            return _process.WaitForExit(_timeoutMilliseconds);
        }

        public void Dispose()
        {
            if (!string.IsNullOrWhiteSpace(_exitCommand))
                _process.StandardInput.WriteLine(_exitCommand);
            _process.StandardInput.Close();
            _process.CancelErrorRead();
            _process.CancelOutputRead();


            if (!_process.HasExited && !_process.WaitForExit(_timeoutMilliseconds))
                try
                {
                    //if the process exits here (after the timeout but before the Kill), we will get an InvalidOperationException
                    _process.Kill();
                    Trace.WriteLine(_process.ProcessName + " did not terminate, so was forcibly closed.");
                }
                catch (InvalidOperationException) { }

            _process.Dispose();
        }
    }
}
