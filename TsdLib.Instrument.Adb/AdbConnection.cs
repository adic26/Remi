using System.Diagnostics;
using System.Text;
using System.Threading;

namespace TsdLib.Instrument.Adb
{
    public class AdbConnection : ConnectionBase
    {
        private readonly Process _shellProcess;
        private readonly StringBuilder _output;
        private readonly AutoResetEvent _outputWaitHandle;
        private readonly StringBuilder _error;
        private readonly AutoResetEvent _errorWaitHandle;

        internal AdbConnection(string address, Process shellProcess)
            : base(address)
        {
            _shellProcess = shellProcess;
            _shellProcess.OutputDataReceived += _shellProcess_OutputDataReceived;
            _shellProcess.ErrorDataReceived += _shellProcess_ErrorDataReceived;
            _output = new StringBuilder();
            _outputWaitHandle = new AutoResetEvent(false);
            _error = new StringBuilder();
            _errorWaitHandle = new AutoResetEvent(false);
            _shellProcess.BeginOutputReadLine();
            _shellProcess.BeginErrorReadLine();
        }

        void _shellProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
                _outputWaitHandle.Set();
            else if (!string.IsNullOrWhiteSpace(e.Data))
                _output.AppendLine(e.Data);
        }

        void _shellProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
                _errorWaitHandle.Set();
            else if (!string.IsNullOrWhiteSpace(e.Data))
                _error.AppendLine(e.Data);
        }

        protected override void Write(string message)
        {
            _shellProcess.StandardInput.WriteLine(message);
        }

        protected override string ReadString()
        {
            _outputWaitHandle.WaitOne(5000);
            string data = _output.ToString();
            _output.Clear();

            return data;
        }

        protected override byte ReadByte()
        {
            return (byte)_shellProcess.StandardOutput.BaseStream.ReadByte();
        }

        protected override bool CheckForError()
        {
            return _error.Length != 0;
        }

        public override bool IsConnected
        {
            get { return !_shellProcess.HasExited && _shellProcess.Responding; }
        }

        protected override void Dispose(bool disposing)
        {
            _shellProcess.Kill();
            base.Dispose(disposing);
        }
    }
}
