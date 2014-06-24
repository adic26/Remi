using System.Collections.Generic;
using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    public class VisaFactory : FactoryBase<VisaConnection>
    {
        protected override IEnumerable<string> SearchForInstruments()
        {
            return ResourceManager.GetLocalManager().FindResources("?*INSTR");
        }

        protected override string GetInstrumentIdentifier(string instrumentAddress, string idCommand)
        {
            using (MessageBasedSession session = (MessageBasedSession)ResourceManager.GetLocalManager().Open(instrumentAddress))
                return session.Query(idCommand);
        }
    }
}
