namespace TestClient.Instruments
{
    using System;
    using TsdLib.Instrument;
    using TsdLib.Instrument.Dummy;
    
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("MyInstrument, Model 12345")]
    public class DummyPowerSupply : InstrumentBase<DummyConnection>
    {
        
        static DummyFactory _factory = new DummyFactory();
        
        internal DummyPowerSupply(DummyConnection connection) : 
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
        
        public static DummyPowerSupply Connect()
        {
            return _factory.GetInstrument<DummyPowerSupply>();
        }
        
        public static DummyPowerSupply Connect(string address)
        {
            return _factory.GetInstrument<DummyPowerSupply>(address);
        }
        
        public void SetVoltage(Double voltage)
        {
            Connection.SendCommand("SET:VOLT {0}", -1, voltage);
        }
        
        public Double GetCurrent()
        {
            Connection.SendCommand("GET:CURRENT?", -1);
            return Connection.GetResponse<Double>();
        }
    }
}
