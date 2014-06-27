using System;

namespace TsdLib.Instrument
{
    public abstract class InstrumentBase<TConnection>
        where TConnection : ConnectionBase
    {
        internal protected TConnection Connection;

        //TODO: check and see if it's ok to send an empty string - might be a nice way to validate the connection in cases where no init command is specified
        protected virtual string InitMessage { get { return ""; }}
        protected void Init()
        {

        }

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
        public string IdCommand { get; private set; }

        public IdCommandAttribute(string idCommand)
        {
            IdCommand = idCommand ?? "";
        }

        /// <summary>
        /// Gets the identification command.
        /// </summary>
        /// <returns>The identification command assigned to the instrument.</returns>
        public override string ToString()
        {
            return IdCommand;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdResponseAttribute : Attribute
    {
        public string IdResponse { get; private set; }
        public char TerminationCharacter { get; private set; }

        public IdResponseAttribute(string idResponse, char terminationCharacter = '\0')
        {
            IdResponse = idResponse ?? "";
            TerminationCharacter = terminationCharacter;
        }

        /// <summary>
        /// Gets the expected identification response.
        /// </summary>
        /// <returns>The expected identification response assigned to the instrument.</returns>
        public override string ToString()
        {
            return IdResponse;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class VisaAttributeAttribute : Attribute
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public string Value { get; private set; }

        public VisaAttributeAttribute(string name, string type, string val)
        {
            Name = name;
            Type = type;
            Value = val;
        }
    }
}
