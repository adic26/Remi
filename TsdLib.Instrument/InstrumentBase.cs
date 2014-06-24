using System;

namespace TsdLib.Instrument
{
    public abstract class InstrumentBase<TConnection>
        where TConnection : ConnectionBase
    {
        //define model/serial/firmware messages/parsers as abstract string properties and carry out the logic in here
        internal protected TConnection Connection;

        protected abstract string ModelNumberMessage { get; }
        protected virtual string ModelNumberRegEx { get { return ".*"; } }
        private string _modelNumber;
        public string ModelNumber
        {
            get
            {
                return _modelNumber ??
                       (_modelNumber = Connection.SendCommand<string>(ModelNumberMessage, ModelNumberRegEx));
            }
        }

        protected abstract string SerialNumberMessage { get; }
        protected virtual string SerialNumberRegEx { get { return ".*"; } }
        private string _serialNumber;
        public string SerialNumber
        {
            get
            {
                return _serialNumber ??
                       (_serialNumber = Connection.SendCommand<string>(SerialNumberMessage, SerialNumberRegEx));
            }
        }

        protected abstract string FirmwareVersionMessage { get; }
        protected virtual string FirmwareVersionRegEx { get { return ".*"; } }
        private string _firmwareVersion;
        public string FirmwareVersion
        {
            get
            {
                return _firmwareVersion ??
                       (_firmwareVersion = Connection.SendCommand<string>(FirmwareVersionMessage, FirmwareVersionRegEx));
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdCommandAttribute : Attribute
    {
        public readonly string IdCommand;

        public IdCommandAttribute(string idCommand)
        {
            IdCommand = idCommand ?? "";
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdResponseAttribute : Attribute
    {
        public readonly string IdResponse;

        public IdResponseAttribute(string idResponse)
        {
            IdResponse = idResponse ?? "";
        }
    }
}
