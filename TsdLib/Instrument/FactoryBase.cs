using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TsdLib.Instrument
{
    public abstract class FactoryBase<TConnection>
        where TConnection : ConnectionBase
    {
        protected abstract IEnumerable<string> SearchForInstruments();

        protected abstract TConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes);

        protected abstract string GetInstrumentIdentifier(TConnection connection, IdQueryAttribute idAttribute);

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
