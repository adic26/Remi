using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using RootDevice.Utilities;
using Managed.Adb;

namespace TsdLib.Instrument.Ssh
{
    public class SshFactory : FactoryBase<SshConnection>
    {
        public int numRetries = 2;

        public string devicePass { private get; set; }

        private KeyValuePair<string, string> _rtasCredentials = 
            new KeyValuePair<string, string>(
            "r8_rel_lab",
            "r8r3liability");
        private List<Device> Devices;

        protected override IEnumerable<string> SearchForInstruments()
        {
            Devices = new List<Device>(AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress));

            return ConnectionUtility.GetDeviceAddresses().Concat(GetSerialNumbers(Devices));
        }
        /// <summary>
        /// Will try to connect to either a BB10 or Avengers device, depending on whether or not a BB10 device is detected
        /// </summary>
        /// <param name="address"></param>
        /// <param name="defaultDelay"></param>
        /// <param name="attributes"></param>
        /// <returns>Connection</returns>
        protected override SshConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            try
            {
                IEnumerable<Device> matches = Devices.Where(device => device.SerialNumber == address);
                if (matches.Any())
                {
                    Devices.Remove(matches.First());
                    return new AvengersSshConn(matches.First());
                }
                if (ConnectionUtility.GetDeviceResponse(address).Contains("BlackBerry Device"))
                    return BbConnect(address);
            }
            catch (Exception ex)
            { 
                Trace.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// Connects to a BB10 Device
        /// </summary>
        /// <param name="address">IP address of the device</param>
        /// <returns>The Connection</returns>
        private SshConnection BbConnect(string address)
        {
            var exceptions = new List<Exception>();
            for (int i = numRetries; i > 0; i--)
            {
                try
                {
                    return new QnxSshConn(_rtasCredentials.Key, _rtasCredentials.Value, address, devicePass);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            }
            return null;
        }
        /// <summary>
        /// Gets the identifier from a particular connection. NOTE: if it is an Avengers device, will return "Avenger"
        /// </summary>
        /// <param name="connection">The Connection</param>
        /// <param name="idAttribute"></param>
        /// <returns></returns>
        protected override string GetInstrumentIdentifier(SshConnection connection, IdQueryAttribute idAttribute)
        {
            if (connection is AvengersSshConn)
                return "Avenger";
            string identifier =
                (from ni in NetworkInterface.GetAllNetworkInterfaces()
                 from ip in ni.GetIPProperties().DhcpServerAddresses
                 where ip.ToString() == connection.Address
                 select ni.Description)
                .FirstOrDefault();

            if (identifier == null)
                throw new Exception("Could not locate instrument with address " + connection.Address);

            return identifier;
        }
        /// <summary>
        /// Yields the serial numbers of each device connected
        /// </summary>
        /// <param name="devices"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetSerialNumbers(IEnumerable<Device> devices)
        {
            foreach (var device in devices)
                yield return device.SerialNumber;
        }
    }
}
