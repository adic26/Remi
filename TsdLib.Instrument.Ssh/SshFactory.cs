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

        private KeyValuePair<string, string> _rtasCredentials = 
            new KeyValuePair<string, string>(
            "r8_rel_lab",
            "r8r3liability");

        public string devicePass { private get; set; }

        protected override IEnumerable<string> SearchForInstruments()
        {
            return ConnectionUtility.GetDeviceAddresses();
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
                if (ConnectionUtility.GetDeviceResponse(address).Contains("BlackBerry Device"))
                    return BbConnect(address);
                else
                    return AvengersConnect();
            }
            catch (Exception ex)
            { 
                Trace.WriteLine(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// Connectes to an Avengers Device
        /// </summary>
        /// <returns>The Connection</returns>
        private SshConnection AvengersConnect()
        {
            return new AvengersSshConn();
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
    }
}
