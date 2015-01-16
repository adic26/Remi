// This is an example of an instrument class automatically generated from an xml file using the TsdLib.InstrumentLibrary tool.
// The NuGet package for the tool can be found in the Tsd NuGet Packages repository.
// Once the NuGet package is installed, this file should be deleted.

using System;
using System.Reflection;

namespace $safeprojectname$.Instruments
{
    using System;
    using System.Globalization;
    using TsdLib.Instrument;
    using TsdLib.Instrument.Dummy;


    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("MyInstrument, Model 12345")]
    public class ExamplePowerSupply : InstrumentBase<DummyConnection>
    {

        static DummyFactory _factory = new DummyFactory();

        private DummyConnection _dummyConnection;
        private readonly Random _random;


        internal ExamplePowerSupply(DummyConnection connection) :
            base(connection)
        {
            _dummyConnection = connection;
            _random = new Random();
            Description = "Simulated Instrument";
        }

        public virtual DummyConnection DummyConnection
        {
            get
            {
                return Connection as DummyConnection;
            }
        }

        protected override string ModelNumberMessage
        {
            get
            {
                return "*IDN?";
            }
        }

        protected override string SerialNumberMessage
        {
            get
            {
                return "*IDN?";
            }
        }

        protected override string FirmwareVersionMessage
        {
            get
            {
                return "*IDN?";
            }
        }

        public static ExamplePowerSupply Connect()
        {
            return _factory.GetInstrument<ExamplePowerSupply>();
        }

        public static ExamplePowerSupply Connect(string address)
        {
            return _factory.GetInstrument<ExamplePowerSupply>(address);
        }

        public virtual void SetVoltage(Double voltage)
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("SET:VOLT {0}", -1, voltage);
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }


        public virtual Double GetCurrent()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("GET:CURRENT?", -1);
                _dummyConnection.StringToRead = _random.NextDouble().ToString(CultureInfo.InvariantCulture);
                return Connection.GetResponse<Double>();
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
    }
}
