using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace TsdLib.Instrument.Adb
{
    public class ProcessRunner : IDisposable
    {
        private readonly Process _process;
        private string _lastCommand;
        private readonly string _exitCommand;
        private readonly int _timeoutMilliseconds;

        private readonly ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
        private readonly AutoResetEvent _waitHandleCmd = new AutoResetEvent(false);
        //private readonly AutoResetEvent _waitHandleResponse = new AutoResetEvent(false);

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
                if (string.IsNullOrWhiteSpace(e.Data))
                    return;
                 
                Match junk = Regex.Match(e.Data, ".*(\b)+");
                    if (junk.Success)
                    {
                        string appendThis = e.Data.Remove(0, junk.Length);
                        string[] copy = new string[_queue.Count];
                        _queue.CopyTo(copy, 0);
                        copy[copy.Length - 1] += appendThis;
                        _queue.Clear();
                        foreach (string s in copy)
                            _queue.Enqueue(s);
                    }
                    else
                        _queue.Enqueue(e.Data);
            };
            _process.BeginOutputReadLine();

            _process.ErrorDataReceived += (o, e) =>
            {
                if (e.Data == null)
                { }
                else if (!_process.HasExited)
                    throw new Exception(string.Format("Error from process {0}: On command: {1} Details: {2}", _process.ProcessName, _lastCommand, e.Data));
            };
            _process.BeginErrorReadLine();
        }

        private string returnCodeString = "echo return code: $?";

        public int LastReturnCode { get; private set; }

        public string ReceiveBuffer { get; private set; }

        //TODO: return error code
        public void SendCommand(string command)
        {
            _queue.Clear();

            if (command.StartsWith("|") && command.EndsWith("|"))
                sendPipeCommand(command);
            else
                sendShellCommand(command);
        }

        public void sendPipeCommand(string command)
        {
            _process.StandardInput.WriteLine(command.Trim('|'));
            IEnumerable<string> str = _queue.DequeueUntil(r => _queue.Count == 0);
            Trace.WriteLine(string.Join(Environment.NewLine, str.Where(s => !string.IsNullOrWhiteSpace(s))));
        }

        public void sendShellCommand(string command)
        {
            _process.StandardInput.WriteLine(command + " && echo return code: $?");

            //read until we see the command that we just sent
            IEnumerable<string> str = _queue.DequeueUntil(readFromBuffer => Regex.IsMatch(readFromBuffer, @"\w+@\w+:/\s?[#$]\s?"));
            Trace.WriteLine(string.Join(Environment.NewLine, str.Where(s => !string.IsNullOrWhiteSpace(s))));
           
            IEnumerable<string> responsePlusReturnCode = _queue.DequeueUntil(readFromBuffer => Regex.IsMatch(readFromBuffer, "return code: "));

            ReceiveBuffer = string.Join(Environment.NewLine, responsePlusReturnCode.TakeWhile(readFromBuffer => !Regex.IsMatch(readFromBuffer, "return code: ")));
            LastReturnCode = int.Parse(Regex.Match(responsePlusReturnCode.Last(), @"(?<=return code: )-?\d+").Value);
        }

        public bool IsConnected
        {
            get { return true; }
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

    internal static class QueueExtensions
    {
        public static void Clear(this ConcurrentQueue<string> queue)
        {
            string str;
            List<string> list = new List<string>();
            while (queue.TryDequeue(out str))
                list.Add(str);
        }

        public static IEnumerable<string> DequeueUntil(this ConcurrentQueue<string> queue, Predicate<string> check)
        {
            string readFromBuffer = null;
            List<string> list = new List<string>();
            Stopwatch sw = new Stopwatch();
            sw.Start();

            while (readFromBuffer == null || !check(readFromBuffer))
            {
                if (!queue.TryDequeue(out readFromBuffer))
                    Thread.Sleep(100);
                else
                    list.Add(readFromBuffer);
                if (sw.ElapsedMilliseconds > 5000)
                {
                    list.Add("return code: -1");
                    break;
                }
            }

            return list;
        }
    }
}
