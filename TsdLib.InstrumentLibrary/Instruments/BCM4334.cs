//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



namespace TsdLib.InstrumentLibrary.Instruments
{
    using System;
    using TsdLib.Instrument;
    using TsdLib.Instrument.Telnet;
    using TsdLib.InstrumentLibrary.Helpers;
    
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    [IdQuery("BlackBerry Device")]
    [CommandDelay("200")]
    public class BlackBerry : InstrumentBase<TelnetConnection>, IBlackBerryWlan
    {
        
        static TelnetFactory _factory = new TelnetFactory();
        
        internal BlackBerry(TelnetConnection connection) : 
                base(connection)
        {
        }
        
        protected override string ModelNumberMessage
        {
            get
            {
                return "cat /pps/services/hw_info/inventory";
            }
        }
        
        protected override string ModelNumberRegEx
        {
            get
            {
                return "(?<=Board_Type::).*(?=\\r\\nBoot_Devices)";
            }
        }
        
        protected override string SerialNumberMessage
        {
            get
            {
                return "cat /pps/system/nvram/deviceinfo";
            }
        }
        
        protected override string SerialNumberRegEx
        {
            get
            {
                return "(?<=BSN::).*(?=\\r\\nBTMAC)";
            }
        }
        
        protected override string FirmwareVersionMessage
        {
            get
            {
                return "cat /pps/services/deviceproperties";
            }
        }
        
        protected override string FirmwareVersionRegEx
        {
            get
            {
                return "(?<=scmbundle::).*(?=\\r\\nscmbundle0)";
            }
        }
        
        public static BlackBerry Connect()
        {
            return _factory.GetInstrument<BlackBerry>();
        }
        
        public static BlackBerry Connect(string address)
        {
            return _factory.GetInstrument<BlackBerry>(address);
        }
        
        public void EnablePlt()
        {
            Connection.SendCommand("echo fw_pltenable::1>>/pps/services/wifi/escreen", -1);
        }
        
        public void DisableRx()
        {
            Connection.SendCommand("wl_bcm4334 pkteng_stop rx", -1);
        }
        
        public void DisableMpc()
        {
            Connection.SendCommand("wl_bcm4334 mpc 0", -1);
        }
        
        public void Enable()
        {
            Connection.SendCommand("wl_bcm4334 up", -1);
        }
        
        public void SetBand(String band)
        {
            Connection.SendCommand("wl_bcm4334 band {0}", -1, band);
        }
        
        public void SetChannel(Int32 channel)
        {
            Connection.SendCommand("wl_bcm4334 channel {0}", -1, channel);
        }
        
        public void SetRate(Int32 rate)
        {
            Connection.SendCommand("wl_bcm4334 rate {0}", -1, rate);
        }
        
        public void StartRx()
        {
            Connection.SendCommand("wl_bcm4334 pkteng_start 10:20:30:40:50:60 rx", -1);
        }
        
        public void ResetCounters()
        {
            Connection.SendCommand("wl_bcm4334 reset_cnts", -1);
        }
        
        public void StartCounters()
        {
            Connection.SendCommand("wl_bcm4334 counters", -1);
        }
        
        public Int32 IsEnabled()
        {
            Connection.SendCommand("wl_bcm4334 isup", -1);
            return Connection.GetResponse<Int32>();
        }
    }
}