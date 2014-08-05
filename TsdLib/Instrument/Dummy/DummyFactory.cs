using System.Collections.Generic;

namespace TsdLib.Instrument.Dummy
{
    public class DummyFactory : FactoryBase<DummyConnection>
    {
        protected override DummyConnection CreateConnection(string address, int defaultDelay,
            params ConnectionSettingAttribute[] attributes)
        {
            return new DummyConnection(address);
        }

        protected override string GetInstrumentIdentifier(DummyConnection connection, IdQueryAttribute idAttribute)
        {
            return "Dummy_Device";
        }

        protected override IEnumerable<string> SearchForInstruments()
        {
            return new[] {"Dummy_Device_Address"};
        }

    }
}