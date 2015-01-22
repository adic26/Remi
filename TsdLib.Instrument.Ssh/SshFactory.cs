using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using RootDevice.Utilities;
using Managed.Adb;

namespace TsdLib.Instrument.Ssh
{
    public class SshFactory : FactoryBase<SshConnection>
    {
        public int NumRetries = 2;

        public string devicePass { private get; set; }

        private List<Device> _aosDeveices;
        
        protected override IEnumerable<string> SearchForInstruments()
        {
            _aosDeveices = new List<Device>(AdbHelper.Instance.GetDevices(AndroidDebugBridge.SocketAddress));

            return ConnectionUtility.GetDeviceAddresses().Concat(_aosDeveices.Select(device => device.SerialNumber));
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
            for (int i = NumRetries; i > 0; i--)
            {
                try
                {
                    Device aosDevice = _aosDeveices.FirstOrDefault(device => device.SerialNumber == address);
                    if (aosDevice != null)
                        return new AvengersSshConn(aosDevice);

                    return new QnxSshConn("r8_rel_lab", "r8r3liability", address, devicePass);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the identifier from a particular connection. NOTE: if it is an Avengers device, will return "Avengers"
        /// </summary>
        /// <param name="connection">The Connection</param>
        /// <param name="idAttribute"></param>
        /// <returns></returns>
        protected override string GetInstrumentIdentifier(SshConnection connection, IdQueryAttribute idAttribute)
        {
            if (connection is AvengersSshConn)
                return "Avengers";

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
    }
}
