using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RootDevice.QConn;
using RootDevice.Utilities;

namespace TsdLib.Instrument.Ssh
{
    public class SshConnection : ConnectionBase
    {
        public const string plinkResourceFolder = @"plink_resources\";
        private const string publicKeyFile = @"public.key";

        private QConnClient qconn;
        private StreamReader OutputStream;
        private StreamWriter InputStream;
        private string deviceIP;

        private Process sshSession;

        public override bool IsConnected
        {
            get { return !sshSession.HasExited; }
        }
        public int Timeout = 300;


        internal SshConnection(string rtasUser, string rtasPass, string ipAddr, string devicePass) : base(ipAddr)
        {
            qconn = new QConnClient();
            deviceIP = ipAddr;

            qconn.Root(deviceIP, plinkResourceFolder + publicKeyFile, rtasUser, rtasPass, devicePass);

            sshSession = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = plinkResourceFolder + "plink",
                    Arguments = "-ssh -l root -i " + plinkResourceFolder + "private.ppk " + deviceIP,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true
                }
            };

            sshSession.Start();

            OutputStream = sshSession.StandardOutput;
            InputStream = sshSession.StandardInput;

            InputStream.WriteLine("y");
        }

        protected override bool CheckForError()
        {
            return sshSession.HasExited;
        }
        protected override byte ReadByte()
        {
            throw new NotImplementedException();
        }
        protected override void Write(string message)
        {
            InputStream.WriteLine(message);

            if (OutputStream.Read() == 35 &&        //"# "
                OutputStream.Read() == 32 &&
                OutputStream.Peek() == -1)
                InputStream.WriteLine(message);     //if plink hasnt fully started up yet, resend the command

            string line = "";
            while (!line.Contains(message))
                line = OutputStream.ReadLine();
        }
        protected override string ReadString()
        {
            var timeoutWatch = new Stopwatch();
            var output = new StringBuilder();
            timeoutWatch.Start();

            while (timeoutWatch.ElapsedMilliseconds < Timeout)
            {
                string line = OutputStream.ReadLine();
                if (line.StartsWith("#"))
                    break;
                if (line != "")
                    output.Append(line + "\n");
            }
            return output.ToString();
        }
        protected override void Dispose(bool disposing)
        {
            if (!sshSession.HasExited)
                sshSession.Kill();
            qconn.Stop();
            base.Dispose(disposing);
        }
    }
}
