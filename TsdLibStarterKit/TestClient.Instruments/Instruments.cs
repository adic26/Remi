namespace $safeprojectname$
{
    using System;
    using TsdLib.Instrument;
    using TsdLib.Instrument.Dummy;
    
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("MyInstrument, Model 12345")]
    public class MyInstrument : InstrumentBase<DummyConnection>
    {
        
        static DummyFactory _factory = new DummyFactory();
        
        internal MyInstrument(DummyConnection connection) : 
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
        
        public static MyInstrument Connect()
        {
            return _factory.GetInstrument<MyInstrument>();
        }
        
        public static MyInstrument Connect(string address)
        {
            return _factory.GetInstrument<MyInstrument>(address);
        }
    }
}
