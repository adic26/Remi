using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TsdLib.Instrument.Adb
{
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
                RedirectStandardInput = true,
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
            }
            
            return addresses;
        }

        void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }

        protected override AdbConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                FileName = AdbFileLocation,
                Arguments = "-s " + address + " shell ",
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };

            AdbConnection conn = new AdbConnection(address, Process.Start(startInfo));

            return conn;
        }

        protected override string GetInstrumentIdentifier(AdbConnection connection, IdQueryAttribute idAttribute)
        {
            return "Avengers";
        }
    }
}
