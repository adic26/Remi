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
    using TsdLib.Instrument.Adb;
    
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("Avengers")]
    [ConnectionSetting("WiFi", "String", "")]
    public class Aos_WiFi : InstrumentBase<AdbConnection>
    {
        
        static AdbFactory _factory = new AdbFactory();
        
        internal Aos_WiFi(AdbConnection connection) : 
                base(connection)
        {
        }
        
        internal Aos_WiFi(DummyConnection connection) : 
                base(connection)
        {
        }
        
        public static Aos_WiFi Connect(CancellationToken token)
        {
            return _factory.GetInstrument<Aos_WiFi>(token);
        }
        
        public static Aos_WiFi Connect(CancellationToken token, string address)
        {
            return _factory.GetInstrument<Aos_WiFi>(token, address);
        }
        
        public static Aos_WiFi Connect(ConnectionBase connection)
        {
            return _factory.GetInstrument<Aos_WiFi>(((AdbConnection)(connection)));
        }
        
        protected override string GetModelNumber()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("getprop | grep ro.product.board", 0, false);
                return Connection.GetResponse<string>("(?<=: \\[)\\w+(?=\\])", false, '\uD800');
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
                Connection.SendCommand("getprop | grep ro.serialno", 0, false);
                return Connection.GetResponse<string>("(?<=: \\[)\\d+(?=\\])", false, '\uD800');
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
                Connection.SendCommand("getprop | grep ro.build.version.incremental", 0, false);
                return Connection.GetResponse<string>("(?<=: \\[)\\w+(?=\\])", false, '\uD800');
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
        
        public virtual String GetDeviceProperties()
        {
            System.Threading.Monitor.Enter(Connection.SyncRoot);
            try
            {
                Connection.SendCommand("getprop", 0, false);
                return Connection.GetResponse<String>(".*", false, '\uD800');
            }
            finally
            {
                System.Threading.Monitor.Exit(Connection.SyncRoot);
            }
        }
    }
}
