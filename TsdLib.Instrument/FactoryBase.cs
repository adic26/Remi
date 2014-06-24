using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib.Instrument
{
    public abstract class FactoryBase<TConnection>
        where TConnection : ConnectionBase
    {
        protected abstract IEnumerable<string> SearchForInstruments();

        protected abstract string GetInstrumentIdentifier(string instrumentAddress, string idCommand);

        public TInstrument GetInstrument<TInstrument>(string address = null)
            where TInstrument : InstrumentBase<TConnection>
        {
            IEnumerable<string> instumentAddresses = SearchForInstruments(); //TODO: cache instruments to increase performance of subsequenct searches

            IdCommandAttribute idCmdAtt = 
                (IdCommandAttribute)
                    Attribute.GetCustomAttribute(typeof(TInstrument), typeof(IdCommandAttribute), true);

            IdResponseAttribute idRespAtt =
                (IdResponseAttribute)
                    Attribute.GetCustomAttribute(typeof (TInstrument), typeof (IdResponseAttribute), true);

            string foundAddress = address == null ?
                instumentAddresses.FirstOrDefault(addr => GetInstrumentIdentifier(addr, idCmdAtt.IdCommand) == idRespAtt.IdResponse) :
                instumentAddresses.FirstOrDefault(addr => addr == address && GetInstrumentIdentifier(addr, idCmdAtt.IdCommand) == idRespAtt.IdResponse);

            if (foundAddress == null)
                throw new InstrumentFinderException("Could not find any " + typeof(TInstrument).Name + " instruments");

            TConnection connection = (TConnection)Activator.CreateInstance(typeof(TConnection), true);
            connection.Address = foundAddress;

            TInstrument newInstrument = (TInstrument)Activator.CreateInstance(typeof(TInstrument), true);
            newInstrument.Connection = connection;

            return newInstrument;
        }
    }
}
