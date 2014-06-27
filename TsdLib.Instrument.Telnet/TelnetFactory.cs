using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace TsdLib.Instrument.Telnet
{
    public class TelnetFactory : FactoryBase<TelnetConnection, Attribute>
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

        protected override TelnetConnection CreateConnection(string address, params Attribute[] attributes)
        {
            return new TelnetConnection(address);
        }

        protected override string GetInstrumentIdentifier(TelnetConnection connection, string idCommand)
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