using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using RootDevice.Utilities;

namespace TsdLib.Instrument.Ssh
{
    public class SshFactory : FactoryBase<SshConnection>
    {
        public int numRetries = 2;

        private KeyValuePair<string, string> _rtasCredentials = new KeyValuePair<string, string>(
            "r8_rel_lab",
            "r8r3liability");
        private string devicePass = null;

        protected override IEnumerable<string> SearchForInstruments()
        {
            return ConnectionUtility.GetDeviceAddresses();
        }
        protected override SshConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            if (!ConnectionUtility.GetDeviceResponse(address).Contains("BlackBerry Device"))
                return null;

            var exceptions = new List<Exception>();
            for (int i = numRetries; i > 0; i--)
            {
                try
                {
                    return new SshConnection(_rtasCredentials.Key, _rtasCredentials.Value, address, devicePass);
                }
                catch(Exception e)
                {
                    exceptions.Add(e);
                }
            }
            return null;
        }
        protected override string GetInstrumentIdentifier(SshConnection connection, IdQueryAttribute idAttribute)
        {
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
