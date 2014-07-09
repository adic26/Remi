using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace TsdLib.Instrument.Telnet
{
    public class TelnetFactory : FactoryBase<TelnetConnection>
    {
        protected override IEnumerable<string> SearchForInstruments()
        {
            var result = NetworkInterface.GetAllNetworkInterfaces()
                .Select(ni => ni.GetIPProperties().DhcpServerAddresses)
                .Where(ip => ip.Any())
                .Select(ip => ip.First().ToString())
                .ToArray();

            if (!result.Any())
                throw new TelnetException("Could not detect any network instruments");

            return result;
        }

        protected override TelnetConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            try
            {
                TcpClient tcpSocket = new TcpClient(address, 23);
                TelnetConnection telnetConnection = new TelnetConnection(tcpSocket, 750); //Use a longer delay for the login operation
                
                string initial = telnetConnection.GetResponse<string>();
                if (!initial.TrimEnd().EndsWith(":"))
                    return null;

                telnetConnection.DefaultDelay = defaultDelay; //Now set the desired delay

                telnetConnection.SendCommand("root", 0);
                string loginResponse = telnetConnection.GetResponse<string>();
                if (!loginResponse.TrimEnd().EndsWith(":"))
                    throw new TelnetException("Could not connect to " + address + " via telnet: no password prompt");

                telnetConnection.SendCommand("root", 0);
                string passwordResponse = telnetConnection.GetResponse<string>();

                if (passwordResponse.Length == 0)
                    throw new TelnetException("Could not read any data from " + address + " via Telnet");

                return telnetConnection;
            }
            catch (SocketException)
            {
                return null;
            }
            
        }

        protected override string GetInstrumentIdentifier(TelnetConnection connection, IdQueryAttribute idAttribute)
        {
            string identifier =
                (from ni in NetworkInterface.GetAllNetworkInterfaces()
                 from ip in ni.GetIPProperties().DhcpServerAddresses
                 where ip.ToString() == connection.Address
                 select ni.Description)
                .FirstOrDefault();
            
            if (identifier == null)
                throw new TelnetException("Could not locate instrument with address " + connection.Address);

            return identifier;
        }
    }

}