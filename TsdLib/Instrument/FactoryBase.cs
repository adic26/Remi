﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Contains base functionality to discover and connect to instruments.
    /// </summary>
    public abstract class FactoryBase<TConnection>
        where TConnection : ConnectionBase
    {
        /// <summary>
        /// Search the system for instruments of the specified connection type.
        /// </summary>
        /// <returns>A sequence of instrument addresses.</returns>
        protected abstract IEnumerable<string> SearchForInstruments();

        /// <summary>
        /// Connects to the instrument at the specified address.
        /// </summary>
        /// <param name="address">Address of the instrument.</param>
        /// <param name="defaultDelay">Default delay to wait between commands.</param>
        /// <param name="attributes">Zero or more ConnectionSettingAttributes. Content will be defined by the instrument connection type.</param>
        /// <returns>A VisaConnection object that can be used to communicate with the instrument.</returns>
        protected abstract TConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes);

        /// <summary>
        /// Send a request to identify the instrument via the specified connection.
        /// </summary>
        /// <param name="connection">Connection object representing the connection to the instrument.</param>
        /// <param name="idAttribute">IdQueryAttribute object representing the command to send to the instrument and a termination character (if required) to signal the end of the instrument response.</param>
        /// <returns>The response from the instrument.</returns>
        protected abstract string GetInstrumentIdentifier(TConnection connection, IdQueryAttribute idAttribute);

        /// <summary>
        /// Connects to an instrument of the specified type and returns an object to communicate with it.
        /// </summary>
        /// <typeparam name="TInstrument">Type of instrument to connect to.</typeparam>
        /// <param name="address">OPTIONAL: If specified, limits the search to the specfied address.</param>
        /// <returns>An object of the specified type (derived from InstrumentBase) with a connection of the specified type.</returns>
        public TInstrument GetInstrument<TInstrument>(string address = null)
            where TInstrument : InstrumentBase<TConnection>
        {
            IdQueryAttribute idAtt = (IdQueryAttribute)Attribute.GetCustomAttribute(typeof(TInstrument), typeof(IdQueryAttribute), true);
            ConnectionSettingAttribute[] connectionAttributes = Attribute.GetCustomAttributes(typeof(TInstrument), typeof(ConnectionSettingAttribute), true).Cast<ConnectionSettingAttribute>().ToArray();

            CommandDelayAttribute delayAttribute = (CommandDelayAttribute)Attribute.GetCustomAttribute(typeof(TInstrument), typeof(CommandDelayAttribute), true);
            int defaultDelay = delayAttribute != null ? delayAttribute.Delay : 0;

            string[] instrumentAddresses;

            if (address == null)
            {
                instrumentAddresses = SearchForInstruments().ToArray();
                Debug.WriteLine("Found instruments:" + Environment.NewLine + string.Join(Environment.NewLine, instrumentAddresses));
            }
            else
            {
                instrumentAddresses = new[] {address};
            }

            List<TConnection> connections = new List<TConnection>();
            foreach (string instrumentAddress in instrumentAddresses)
            {
                TConnection conn = CreateConnection(instrumentAddress, defaultDelay, connectionAttributes);

                if (conn != null)
                {
                    Debug.WriteLine("Connecting to " + instrumentAddress);

                    string id = GetInstrumentIdentifier(conn, idAtt);
                    if (id.Contains(idAtt.Response) || id == "Dummy_Device")
                    {
                        Debug.WriteLine("Found identifier match: " + id);
                        connections.Add(conn);
                    }
                    else
                    {
                        Debug.WriteLine("No response from " + instrumentAddress + ". Disposing connection");
                        conn.Dispose();
                    }
                }
            }

            if (connections.Count == 0)
                throw new InstrumentFactoryException("Could not connect to any " + typeof(TInstrument).Name + " instruments via " + typeof(TConnection).Name);

            TInstrument inst = (TInstrument)Activator.CreateInstance(
                typeof(TInstrument),
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { connections[0] },
                null);

            Debug.WriteLine("Connected to " + inst.Description);
            Debug.WriteLine("Model number: " + inst.ModelNumber);
            Debug.WriteLine("Serial number: " + inst.SerialNumber);
            Debug.WriteLine("Firmware version: " + inst.FirmwareVersion);

            return inst;
        }
    }
}
