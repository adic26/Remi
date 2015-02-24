using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TsdLib.Instrument.Adb
{
    /// <summary>
    /// Contains functionality to discover and connect to instruments via adb.exe.
    /// </summary>
    public class AdbFactory : FactoryBase<AdbConnection>
    {
        private const string AdbFileLocation = @"platform-tools\adb.exe";

        protected override IEnumerable<string> SearchForInstruments()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = AdbFileLocation,
                Arguments = "devices",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            List<string> addresses = new List<string>();

            using (Process queryDevicesProcess = new Process { StartInfo =  startInfo})
            {
                queryDevicesProcess.ErrorDataReceived += process_ErrorDataReceived;
                queryDevicesProcess.Start();

                string line;
                while ((line = queryDevicesProcess.StandardOutput.ReadLine()) != null)
                {
                    Match match = Regex.Match(line, @"\w{8}(?=\sdevice)");
                    if (match.Success)
                        addresses.Add(match.Value);
                }
                queryDevicesProcess.WaitForExit(2000);
                if (!queryDevicesProcess.HasExited)
                    throw new AdbConnectException("Adb device query process did not terminate");
            }
            
            return addresses;
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new AdbConnectException("Error received when searching for devices: " + e.Data);
        }

        protected override AdbConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = AdbFileLocation,
                Arguments = "root",
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (Process rootProcess = Process.Start(startInfo))
            {
                if (rootProcess == null)
                    throw new AdbConnectException("Could not start adb.exe to send the root command");
                rootProcess.WaitForExit(10000);
                if (!rootProcess.HasExited)
                    throw new AdbConnectException("Root process did not terminate");
            }
            AdbConnection conn = new AdbConnection(address, AdbFileLocation);

            return conn;
        }

        protected override string GetInstrumentIdentifier(AdbConnection connection, IdQueryAttribute idAttribute)
        {
            return "Avengers";
        }
    }


}
