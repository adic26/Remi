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
                WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "platform-tools")
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
                .Select(line => Regex.Match(line, @"\w{8,}(?=\s+device)"))
                .Where(match => match.Success)
                .Select(match => match.Value)
                ;

            return devs;
        }

        protected override AdbConnection CreateConnection(string address, params ConnectionSettingAttribute[] attributes)
        {
            ConnectionSettingAttribute wifiAttribute = attributes.FirstOrDefault(attr => attr.Name == "WiFi");

            if (wifiAttribute == null)
                return new AdbConnection(address);

            string ip;

            Match ipMatch = Regex.Match(wifiAttribute.ArgumentValue.ToString(), @"(?<=inet )\d+\.\d+\.\d+\.\d+");
            if (ipMatch.Success)
                ip = ipMatch.Value;
            else
            {
                using (AdbConnection conn = new AdbConnection(address))
                {
                    conn.SendCommand("ip -f inet addr show wlan0", 0, false);
                    ip = conn.GetResponse<string>(@"(?<=inet )\d+\.\d+\.\d+\.\d+", false);

                    //TODO: check to make sure ip is valid
                    if (!Regex.IsMatch(ip, @"\d+\.\d+\.\d+\.\d+"))
                        throw new AdbConnectException(string.Format("Invalid IP address {0} read from device {1}", ip, conn.Address));

                    conn.SendCommand("setprop service.adb.tcp.port 5555", 0, false);
                    conn.SendCommand("adb tcpip 5555", 0, false);

                    Thread.Sleep(2000);
                    conn.SendCommand("adb connect " + ip, 0, false);
                    //Thread.Sleep(2000);

                }
            }
            return new AdbConnection(ip + ":5555");
            
        }

        protected override string GetInstrumentIdentifier(AdbConnection connection, IdQueryAttribute idAttribute)
        {
            return "Avengers";
        }
    }
}
