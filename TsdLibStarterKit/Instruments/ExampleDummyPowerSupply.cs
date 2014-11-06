// This is an example of an instrument class automatically generated from an xml file using the TsdLib.InstrumentLibrary tool.
// The NuGet package for the tool can be found in the Tsd NuGet Packages repository.
// Once the NuGet package is installed, this file should be deleted.

namespace $safeprojectname$.Instruments
{
    using System;
    using TsdLib.Instrument;
    using TsdLib.Instrument.Dummy;


    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("MyInstrument, Model 12345")]
    public class ExampleDummyPowerSupply : InstrumentBase<DummyConnection>
    {

        static DummyFactory _factory = new DummyFactory();

        internal ExampleDummyPowerSupply(DummyConnection connection) :
            base(connection)
        {
        }

        public virtual DummyConnection DummyConnection
        {
            get
            {
                return Connection;
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

        public static ExampleDummyPowerSupply Connect()
        {
            return _factory.GetInstrument<ExampleDummyPowerSupply>();
        }

        public static ExampleDummyPowerSupply Connect(string address)
        {
            return _factory.GetInstrument<ExampleDummyPowerSupply>(address);
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
                return Connection.GetResponse<Double>();
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
    }

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("MyInstrument, Model 12345")]
    public class DummyPowerSupply2 : InstrumentBase<DummyConnection>
    {

        static DummyFactory _factory = new DummyFactory();

        internal DummyPowerSupply2(DummyConnection connection) :
            base(connection)
        {
        }

        public virtual DummyConnection DummyConnection
        {
            get
            {
                return Connection;
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

        public static DummyPowerSupply2 Connect()
        {
            return _factory.GetInstrument<DummyPowerSupply2>();
        }

        public static DummyPowerSupply2 Connect(string address)
        {
            return _factory.GetInstrument<DummyPowerSupply2>(address);
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
                return Connection.GetResponse<Double>();
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
    }
}
