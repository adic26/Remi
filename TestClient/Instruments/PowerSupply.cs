//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using TsdLib.Instrument.Dummy;

namespace TestClient.Instruments
{
    using System;
    using TsdLib.Instrument;


    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("MyInstrument, Model 12345")]
    public class PowerSupply : InstrumentBase<DummyConnection>
    {
        
        static DummyFactory _factory = new DummyFactory();
        
        internal PowerSupply(DummyConnection connection) : 
                base(connection)
        {
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
        
        public static PowerSupply Connect()
        {
            return _factory.GetInstrument<PowerSupply>();
        }
        
        public static PowerSupply Connect(string address)
        {
            return _factory.GetInstrument<PowerSupply>(address);
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
