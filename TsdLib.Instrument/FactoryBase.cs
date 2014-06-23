using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib.Instrument
{
    public abstract class FactoryBase<TConnection>
        where TConnection : ConnectionBase
    {
        protected abstract IEnumerable<string> SearchForInstruments();

        protected abstract string GetInstrumentIdentifier(string instrumentAddress);

        public TInstrument GetInstrument<TInstrument>(string address = null)
            where TInstrument : InstrumentBase<TConnection>
        {
            IEnumerable<string> instumentAddresses = SearchForInstruments(); //TODO: cache instruments to increase search performance

            IdentifierAttribute identifierAttribute = (IdentifierAttribute)Attribute.GetCustomAttribute(typeof(TInstrument), typeof(IdentifierAttribute), true);

            string foundAddress = address == null ? 
                instumentAddresses.FirstOrDefault(addr => GetInstrumentIdentifier(addr) == identifierAttribute.Identifier) :
                instumentAddresses.FirstOrDefault(addr => addr == address && GetInstrumentIdentifier(addr) == identifierAttribute.Identifier);

            if (foundAddress == null)
                throw new InstrumentFinderException("Could not find any instruments");

            TConnection connection = (TConnection)Activator.CreateInstance(typeof(TConnection), true);
            connection.Address = foundAddress;

            TInstrument newInstrument = (TInstrument)Activator.CreateInstance(typeof(TInstrument), true);
            newInstrument.Connection = connection;

            return newInstrument;
        }
    }
}
