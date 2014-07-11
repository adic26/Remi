namespace TsdLib.Instrument
{
    using System;
    using Visa;
    using Telnet;
    
    
    [IdQuery("6632B", "*IDN?")]
    [InitCommands("*RST;*CLS")]
    public class Aglient6632B : InstrumentBase<VisaConnection>, IPowerSupply
    {
        
        static VisaFactory _factory = new VisaFactory();
        
        internal Aglient6632B(VisaConnection connection) : 
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
        
        protected override string ModelNumberRegEx
        {
            get
            {
                return "(?<=(.*,){1}).*(?=(,.*){2})";
            }
        }
        
        protected override string SerialNumberMessage
        {
            get
            {
                return "*IDN?";
            }
        }
        
        protected override string SerialNumberRegEx
        {
            get
            {
                return "(?<=.*,.*,).*(?=,)";
            }
        }
        
        protected override string FirmwareVersionMessage
        {
            get
            {
                return "*IDN?";
            }
        }
        
        protected override string FirmwareVersionRegEx
        {
            get
            {
                return "(?<=(.*,){3}).*";
            }
        }
        
        public static Aglient6632B GetInstance()
        {
            return _factory.GetInstrument<Aglient6632B>();
        }
        
        public static Aglient6632B GetInstance(string address)
        {
            return _factory.GetInstrument<Aglient6632B>(address);
        }
        
        public void SetVoltage(Double voltage)
        {
            Connection.SendCommand("SOURce:VOLTage:LEVel {0}", -1, voltage);
        }
        
        public Double ReadCurrent()
        {
            Connection.SendCommand("MEASure:CURRent?", -1);
            return Connection.GetResponse<Double>();
        }
    }
    
    [IdQuery("4170C", "V", "@")]
    [InitCommands("D0")]
    [CommandDelay("500")]
    [ConnectionSetting("AsrlBaud", "Int32", "57600")]
    public class AIM4170C : InstrumentBase<VisaConnection>
    {
        
        static VisaFactory _factory = new VisaFactory();
        
        internal AIM4170C(VisaConnection connection) : 
                base(connection)
        {
        }
        
        protected override string ModelNumberMessage
        {
            get
            {
                return "V";
            }
        }
        
        protected override string ModelNumberRegEx
        {
            get
            {
                return "(?<=\\s).*?(?=\\s)";
            }
        }
        
        protected override char ModelNumberTermChar
        {
            get
            {
                return '@';
            }
        }
        
        protected override string SerialNumberMessage
        {
            get
            {
                return "V";
            }
        }
        
        protected override string SerialNumberRegEx
        {
            get
            {
                return "(?<=DATE.*\\s.*\\s.*\\s).*?(?=\\s)";
            }
        }
        
        protected override char SerialNumberTermChar
        {
            get
            {
                return '@';
            }
        }
        
        protected override string FirmwareVersionMessage
        {
            get
            {
                return "V";
            }
        }
        
        protected override string FirmwareVersionRegEx
        {
            get
            {
                return "(?<=ver ).*?(?=\\s)";
            }
        }
        
        protected override char FirmwareVersionTermChar
        {
            get
            {
                return '@';
            }
        }
        
        public static AIM4170C GetInstance()
        {
            return _factory.GetInstrument<AIM4170C>();
        }
        
        public static AIM4170C GetInstance(string address)
        {
            return _factory.GetInstrument<AIM4170C>(address);
        }
        
        public void CloseRelay()
        {
            Connection.SendCommand("K3", -1);
        }
        
        public void OpenRelay()
        {
            Connection.SendCommand("K0", -1);
        }
        
        public byte[] MeasureWLC()
        {
            Connection.SendCommand("F0010624E", -1);
            return Connection.GetByteResponse(72);
        }
        
        public byte[] MeasureNFC()
        {
            Connection.SendCommand("F08ADAB9F", -1);
            return Connection.GetByteResponse(72);
        }
        
        public byte[] Measure(String frequencystring)
        {
            Connection.SendCommand("F{0}", -1, frequencystring);
            return Connection.GetByteResponse(72);
        }
    }
    
    [IdQuery("BlackBerry Device")]
    [CommandDelay("200")]
    public class BlackBerryRadio : InstrumentBase<TelnetConnection>, IBlackBerryRadio
    {
        
        static TelnetFactory _factory = new TelnetFactory();
        
        internal BlackBerryRadio(TelnetConnection connection) : 
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
        
        public static BlackBerryRadio GetInstance()
        {
            return _factory.GetInstrument<BlackBerryRadio>();
        }
        
        public static BlackBerryRadio GetInstance(string address)
        {
            return _factory.GetInstrument<BlackBerryRadio>(address);
        }
        
        public void Reboot()
        {
            Connection.SendCommand("echo \'msg::shutdown\\ndat::{\"shutdownType\":0,\"reason\":\"APPS requested\"}\' > /pps/se" +
                    "rvices/power/shutdown/control", -1);
        }
        
        public void EnableRadio()
        {
            Connection.SendCommand("echo \'msg::power\\nid::yyy\\ndat:json:{\"state\":\"ON\"}\' > /pps/services/radioctrl/mod" +
                    "em0/control", -1);
        }
        
        public void DisableRadio()
        {
            Connection.SendCommand("echo \'msg::power\\nid::yyy\\ndat:json:{\"state\":\"OFF\"}\' > /pps/services/radioctrl/mo" +
                    "dem0/control", -1);
        }
        
        public void PlacePhoneCall(Int32 phoneNumber)
        {
            Connection.SendCommand("echo \'msg::start_call\\nid::yyy\\ndat:json:{\"phone_number\":\"{0}\"}\' > /pps/services/" +
                    "phone/private/control", -1, phoneNumber);
        }
        
        public Boolean IsRadioEnabled()
        {
            Connection.SendCommand("grep -q power_status::ON /pps/services/radioctrl/modem0/status_public && echo tru" +
                    "e || echo false", -1);
            return Connection.GetResponse<Boolean>("(?<=\\r\\0\\r\\n).*(?=\\r\\n#)");
        }
    }
}
