//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestClient.Instruments
{
    using System;
    using System.Threading;
    using TsdLib.Instrument;
    using TsdLib.Instrument.Dummy;
    using TsdLib.Instrument.Telnet;
    
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("BlackBerry Device")]
    public class BB10 : InstrumentBase<TelnetConnection>, IBlackBerry
    {
        
        static TelnetFactory _factory = new TelnetFactory();
        
        internal BB10(TelnetConnection connection) : 
                base(connection)
        {
        }
        
        internal BB10(DummyConnection connection) : 
                base(connection)
        {
        }
        
        public static BB10 Connect(CancellationToken token)
        {
            return _factory.GetInstrument<BB10>(token);
        }
        
        public static BB10 Connect(CancellationToken token, string address)
        {
            return _factory.GetInstrument<BB10>(token, address);
        }
        
        public static BB10 Connect(ConnectionBase connection)
        {
            return _factory.GetInstrument<BB10>(((TelnetConnection)(connection)));
        }
        
        protected override string GetModelNumber()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("cat /pps/services/hw_info/inventory | grep Board_Type::", 200, false);
                return Connection.GetResponse<string>("(?<=Board_Type::).*", false, '\uD800');
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
        
        protected override string GetSerialNumber()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("cat /pps/system/nvram/deviceinfo | grep BSN::", 200, false);
                return Connection.GetResponse<string>("(?<=BSN::).*", false, '\uD800');
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
        
        protected override string GetFirmwareVersion()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("cat /pps/services/deviceproperties | grep scmbundle::", 200, false);
                return Connection.GetResponse<string>("(?<=scmbundle::).*", false, '\uD800');
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
        
        public virtual String GetImei()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("cat /pps/system/nvram/deviceinfo | grep IMEI::", 200, false);
                return Connection.GetResponse<String>("(?<=IMEI::).*(?=\\r\\nInProduction)", false, '\uD800');
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
        
        public virtual String GetChipsetFamily()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("wl_bcm_dhd revinfo | grep chipnum", 200, false);
                return Connection.GetResponse<String>("(?<=chipnum 0x)\\d+", false, '\uD800');
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
    }
}
