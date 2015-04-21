using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TsdLib.Instrument.Adb
{
    public class AdbFactory : FactoryBase<AdbConnection>
    {
        static AdbFactory()
        {
            //using (Process p = Process.Start(Path.Combine(Environment.SystemDirectory, "cmd.exe"), "taskkill /F /im adb.exe"))
            //    if (!p.WaitForExit(5000))
            //        Trace.WriteLine("WARNING: FAILED TO KILL ADB PROCESSES");

            foreach (Process adbProcess in Process.GetProcessesByName("adb"))
                adbProcess.Kill();
        }

        protected override IEnumerable<string> SearchForInstruments()
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
                cmdProcess.OutputDataReceived += (o, e) => { if (e.Data == null) mreOut.Set(); else sbOut.AppendLine(e.Data); };
                cmdProcess.BeginOutputReadLine();
                cmdProcess.ErrorDataReceived += (o, e) => { if (e.Data == null) mreErr.Set(); else sbErr.AppendLine(e.Data); };
                cmdProcess.BeginErrorReadLine();

                //TODO: cmdProcess.StandardInput.WriteLine("adb wait-for-device");

                cmdProcess.StandardInput.WriteLine("adb devices");

                cmdProcess.StandardInput.Close();
                bool exited = cmdProcess.WaitForExit(5000);

                if (!exited)
                    throw new AdbConnectException("The adb devices process did not exit after 5 seconds.");

                mreOut.WaitOne();
                mreErr.WaitOne();

                if (cmdProcess.ExitCode != 0)
                    throw new AdbConnectException("The adb devices process exited with code: " + cmdProcess.ExitCode);
            }

            if (sbErr.Length > 0)
                throw new AdbConnectException("The adb devices process generated the error: " + sbErr);

            IEnumerable<string> devs = sbOut.ToString()
                .Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => Regex.Match(line, @"\w{8}(?=\sdevice)"))
                .Where(match => match.Success)
                .Select(match => match.Value)
                ;

            return devs;
        }

        protected override AdbConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            AdbConnection conn = new AdbConnection(address);

            return conn;
        }

        protected override string GetInstrumentIdentifier(AdbConnection connection, IdQueryAttribute idAttribute)
        {
            return "Avengers";
        }
    }
}
