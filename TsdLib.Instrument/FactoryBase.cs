using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib.Instrument
{
    public abstract class FactoryBase<TConnection, TAttribute>
        where TConnection : ConnectionBase
        where TAttribute : Attribute
    {
        protected abstract IEnumerable<string> SearchForInstruments();
        
        protected abstract TConnection CreateConnection(string address, params TAttribute[] attributes);

        protected abstract string GetInstrumentIdentifier(TConnection connection, string idCommand);

        public TInstrument GetInstrument<TInstrument>(string address = null)
            where TInstrument : InstrumentBase<TConnection>
        {//TODO: cache instruments to increase performance of subsequenct searches
            IdCommandAttribute idCmdAtt = 
                (IdCommandAttribute)
                    Attribute.GetCustomAttribute(typeof(TInstrument), typeof(IdCommandAttribute), true);

            IdResponseAttribute idRespAtt =
                (IdResponseAttribute)
                    Attribute.GetCustomAttribute(typeof (TInstrument), typeof (IdResponseAttribute), true);

            //TODO: extend to find multiple attributes of the same type
            TAttribute customAttributes =
                (TAttribute)
                    Attribute.GetCustomAttribute(typeof (TInstrument), typeof (TAttribute), true);
            
            IEnumerable<string> instrumentAddresses = SearchForInstruments();

            List<TConnection> connections = new List<TConnection>();
            foreach (string instrumentAddress in instrumentAddresses)
            {
                TConnection conn = CreateConnection(instrumentAddress, customAttributes);
                string id = GetInstrumentIdentifier(conn, idCmdAtt.ToString());
                if (id == idRespAtt.ToString())
                    connections.Add(conn);
                else
                    conn.Dispose();
            }

            //TODO: Create instrument and inject the connection

            throw new NotImplementedException();
        }
    }
}
