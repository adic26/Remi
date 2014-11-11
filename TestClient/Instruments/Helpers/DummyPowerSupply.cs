using System;
using System.Globalization;
using TsdLib.Instrument.Dummy;

namespace TestClient.Instruments
{
    public class DummyPowerSupply : PowerSupply
    {
        private readonly DummyConnection _dummyConnection;
        private readonly Random _random;

        public DummyPowerSupply(string address)
            : base(new DummyConnection(address))
        {
            _dummyConnection = Connection as DummyConnection;
            _random = new Random();
        }

        public override double GetCurrent()
        {
            _dummyConnection.StringToRead = _random.NextDouble().ToString(CultureInfo.InvariantCulture);
            return base.GetCurrent();
        }
    }

}