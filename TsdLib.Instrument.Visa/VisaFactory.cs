using System;
using System.Collections.Generic;

namespace TsdLib.Instrument.Visa
{
    public class VisaFactory : FactoryBase<VisaConnection>
    {//TODO: implement VisaFactory
        protected override IEnumerable<string> SearchForInstruments()
        {
            throw new NotImplementedException();
        }

        protected override string GetInstrumentIdentifier(string instrumentAddress)
        {
            throw new NotImplementedException();
        }
    }
}
