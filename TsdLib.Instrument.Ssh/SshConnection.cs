using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using RootDevice.QConn;

namespace TsdLib.Instrument.Ssh
{
    public class SshConnection : ConnectionBase
    {
        public const string PlinkResourceFolder = @"plink_resources\";
        private const string PublicKeyFile = @"public.key";

        private readonly QConnClient _qconn;
        private readonly StreamReader _streamReader;
        private readonly StreamWriter _streamWriter;

        private readonly Process _sshSession;

        public override bool IsConnected
        {
            get { return !_sshSession.HasExited; }
        }
        public int Timeout = 300;


        internal SshConnection(string rtasUser, string rtasPass, string ipAddr, string devicePass)
            : base(ipAddr)
        {
            _qconn = new QConnClient();
            string deviceIp = ipAddr;

            _qconn.Root(deviceIp, PlinkResourceFolder + PublicKeyFile, rtasUser, rtasPass, devicePass);

            _sshSession = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = PlinkResourceFolder + "plink",
                    Arguments = "-ssh -l root -i " + PlinkResourceFolder + "private.ppk " + deviceIp,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };

            _sshSession.Start();

            _streamReader = _sshSession.StandardOutput;
            _streamWriter = _sshSession.StandardInput;

            _streamWriter.WriteLine("y");
        }

        protected override bool CheckForError()
        {
            //TODO: make sure streams are open or check for errors on QConnClient
            return _sshSession.HasExited;
        }
        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }
        protected override void Write(string message)
        {
            _streamWriter.WriteLine(message);

            if (_streamReader.Read() == 35 &&        //"# "
                _streamReader.Read() == 32 &&
                _streamReader.Peek() == -1)
                _streamWriter.WriteLine(message);     //if plink hasnt fully started up yet, resend the command

            string line = "";
            while (!line.Contains(message))
                line = _streamReader.ReadLine();
        }
        protected override string ReadString()
        {
            Stopwatch timeoutWatch = new Stopwatch();
            StringBuilder output = new StringBuilder();
            timeoutWatch.Start();

            while (timeoutWatch.ElapsedMilliseconds < Timeout)
            {
                string line = _streamReader.ReadLine();
                if (line.StartsWith("#"))
                    break;
                if (line != "")
                    output.AppendLine(line);
            }
            return output.ToString();
        }
        protected override void Dispose(bool disposing)
        {
            if (!_sshSession.HasExited)
                _sshSession.Kill();
            _qconn.Stop();
            base.Dispose(disposing);
        }
    }
}
