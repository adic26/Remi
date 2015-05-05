using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Contains base functionality to discover and connect to instruments.
    /// </summary>
    /// <typeparam name="TConnection">Type of connection that the factory discovers and connects to.</typeparam>
    public abstract class FactoryBase<TConnection>
        where TConnection : ConnectionBase
    {
        /// <summary>
        /// Search the system for instruments of the specified connection type.
        /// </summary>
        /// <returns>A sequence of instrument addresses.</returns>
        protected abstract IEnumerable<string> SearchForInstruments();
        /// <summary>
        /// Creates a connection using the specified address.
        /// </summary>
        /// <param name="address">Address of the instrument.</param>
        /// <param name="attributes">Zero or more ConnectionSettingAttributes. Content will be defined by the instrument connection type.</param>
        /// <returns>A VisaConnection object that can be used to communicate with the instrument.</returns>
        protected abstract TConnection CreateConnection(string address, params ConnectionSettingAttribute[] attributes);
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
        /// <param name="token">A cancellation token used to enable cooperative cancellation.</param>
        /// <param name="address">OPTIONAL: If specified, limits the search to the specfied address.</param>
        /// <returns>An object of the specified type (derived from InstrumentBase) with a connection of the specified type.</returns>
        public TInstrument GetInstrument<TInstrument>(CancellationToken token, string address = null)
            where TInstrument : InstrumentBase<TConnection>
        {
            TConnection[] connections = GetConnection<TInstrument>(token, address);

            if (connections.Length == 0)
                throw new ConnectException(typeof(TInstrument).Name, typeof(TConnection).Name);

            TInstrument inst = GetInstrument<TInstrument>(connections[0]);

            InstrumentEvents.FireConnected(this, inst);

            return inst;
        }

        public TConnection[] GetConnection<TInstrument>(CancellationToken token, string address = null)
            where TInstrument : InstrumentBase<TConnection>
        {
            IdQueryAttribute idAtt = (IdQueryAttribute)Attribute.GetCustomAttribute(typeof(TInstrument), typeof(IdQueryAttribute), true);
            ConnectionSettingAttribute[] connectionAttributes = Attribute.GetCustomAttributes(typeof(TInstrument), typeof(ConnectionSettingAttribute), true).Cast<ConnectionSettingAttribute>().ToArray();

            string[] instrumentAddresses = string.IsNullOrWhiteSpace(address) ? SearchForInstruments().ToArray() : new[] { address };

            List<TConnection> connections = new List<TConnection>();
            foreach (string instrumentAddress in instrumentAddresses)
            {
                TConnection conn = CreateConnection(instrumentAddress, connectionAttributes);

                if (conn != null)
                {
                    conn.Token = token;
                    Trace.WriteLine("Connecting to " + instrumentAddress);

                    string id = GetInstrumentIdentifier(conn, idAtt);
                    if (id.Contains(idAtt.Response) || id == "Dummy_Device")
                    {
                        Trace.WriteLine("Found identifier match: " + id);
                        connections.Add(conn);
                    }
                    else
                    {
                        Trace.WriteLine("Response from " + instrumentAddress + " does not match expected response: " + idAtt.Response + ". Disposing connection");
                        conn.Dispose();
                    }
                }
            }

            return connections.ToArray();
        }

        public TInstrument GetInstrument<TInstrument>(TConnection connection)
            where TInstrument : InstrumentBase<TConnection>
        {
            TInstrument inst = (TInstrument)Activator.CreateInstance(
                typeof(TInstrument),
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new object[] { connection },
                null);

            IEnumerable<MethodInfo> initMethods = typeof (TInstrument)
                .GetMethods()
                .Where(m => m.GetCustomAttributes().OfType<InitCommandAttribute>().Any());

            foreach (MethodInfo initMethod in initMethods)
                initMethod.Invoke(inst, new object[0]);


            Trace.WriteLine("Connected to " + inst.Description);
            Trace.WriteLine("Model number: " + inst.ModelNumber);
            Trace.WriteLine("Serial number: " + inst.SerialNumber);
            Trace.WriteLine("Firmware version: " + inst.FirmwareVersion);

            return inst;
        }
    }


}
