using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib.Instrument
{
    public abstract class FactoryBase<TConnection>
        where TConnection : ConnectionBase
    {//TODO: cache instruments
        protected abstract IEnumerable<string> SearchForInstruments();

        protected abstract string GetInstrumentIdentifier(string instrumentAddress);

        //TODO: make generated assembly friend to TsdLib.Instrument and make connections in dynamic instruments internal
        public TInstrument GetInstrument<TInstrument>(string address = null)
            where TInstrument : InstrumentBase<TConnection>
        {
            IEnumerable<string> visaInstumentAddresses = SearchForInstruments();

            IdentifierAttribute identifierAttribute = (IdentifierAttribute)Attribute.GetCustomAttribute(typeof(TInstrument), typeof(IdentifierAttribute), true);

            string foundAddress = visaInstumentAddresses
                .FirstOrDefault(i => GetInstrumentIdentifier(i) == identifierAttribute.Identifier);

            TConnection connection = (TConnection)Activator.CreateInstance(typeof(TConnection), true);
            connection.Address = foundAddress;

            TInstrument newInstrument = (TInstrument)Activator.CreateInstance(typeof(TInstrument), true);
            newInstrument.Connection = connection;

            return newInstrument;
        }
    }
}
