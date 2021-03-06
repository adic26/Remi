﻿using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;

namespace TsdLib.Instrument.Telnet
{
    /// <summary>
    /// Contains functionality to discover and connect to instruments via the Telnet protocol.
    /// </summary>
    public class TelnetFactory : FactoryBase<TelnetConnection>
    {
        /// <summary>
        /// Search the system for Telnet-based instrument.
        /// </summary>
        /// <returns>A sequence of instrument addresses.</returns>
        protected override IEnumerable<string> SearchForInstruments()
        {
            var result = NetworkInterface.GetAllNetworkInterfaces()
                .Select(ni => ni.GetIPProperties())
                .Where(ipp => ipp.DnsSuffix == "")
                .Select(ipp => ipp.DhcpServerAddresses)
                .Where(ip => ip.Any())
                .Select(ip => ip.First().ToString())
                .ToArray();

            if (!result.Any())
                throw new TelnetException("Could not detect any network instruments");

            return result;
        }

        /// <summary>
        /// Connects to the Telnet-based instrument at the specified address.
        /// </summary>
        /// <param name="address">IP address for the instrument.</param>
        /// <param name="attributes">Zero or more ConnectionSettingAttributes. Not required for TelnetConnection.</param>
        /// <returns>A VisaConnection object that can be used to communicate with the instrument.</returns>
        protected override TelnetConnection CreateConnection(string address, params ConnectionSettingAttribute[] attributes)
        {
            try
            {
                TcpClient tcpSocket = new TcpClient(address, 23);
                TelnetConnection telnetConnection = new TelnetConnection(tcpSocket);
                string initial = "";
                for (int i = 0; i < 20; i++)
                {
                    initial = telnetConnection.GetResponse<string>(".*", false);
                    if (initial.TrimEnd().EndsWith(":"))
                        break;
                    Thread.Sleep(200);
                }
                if (!initial.TrimEnd().EndsWith(":"))
                    return null;

                telnetConnection.SendCommand("root", 0, false);
                string loginResponse = telnetConnection.GetResponse<string>(".*", false);
                if (!loginResponse.TrimEnd().EndsWith(":"))
                    throw new TelnetException("Could not connect to " + address + " via telnet: no password prompt");

                telnetConnection.SendCommand("root", 0, false);
                string passwordResponse = telnetConnection.GetResponse<string>(".*", false);

                if (passwordResponse.Length == 0)
                    throw new TelnetException("Could not read any data from " + address + " via Telnet");

                return telnetConnection;
            }
            catch (SocketException)
            {
                return null;
            }
            
        }

        /// <summary>
        /// Send a request to identify the type of instrument via the specified TelnetConnection.
        /// </summary>
        /// <param name="connection">TelnetConnection object representing the connection to the instrument.</param>
        /// <param name="idAttribute">Not required for TelnetConnection.</param>
        /// <returns>The DHCP server description (ie. BlackBerry Device #..)</returns>
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