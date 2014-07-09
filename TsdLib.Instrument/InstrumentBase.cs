using System;
using System.Diagnostics;

namespace TsdLib.Instrument
{
    public abstract class InstrumentBase<TConnection> : IDisposable
        where TConnection : ConnectionBase
    {
        internal protected TConnection Connection;

        public string Description { get; private set; }

        protected InstrumentBase(TConnection connection)
        {
            Connection = connection;

            InitCommandsAttribute initCommands = (InitCommandsAttribute)Attribute.GetCustomAttribute(GetType(), typeof(InitCommandsAttribute), true);
            if (initCommands != null)
                foreach (string command in initCommands.Commands)
                    Connection.SendCommand(command);

            Description = GetType().Name + " via " + connection.Description;
        }

        protected virtual string InitCommands { get { return ""; } }

        protected abstract string ModelNumberMessage { get; }
        protected virtual string ModelNumberRegEx { get { return ".*"; } }
        protected virtual char ModelNumberTermChar { get { return '\uD800'; } }
        private string _modelNumber;
        public string ModelNumber
        {
            get
            {
                if (_modelNumber == null)
                {
                    lock (this)
                    {
                        Connection.SendCommand(ModelNumberMessage);
                        _modelNumber = Connection.GetResponse<string>(ModelNumberRegEx, ModelNumberTermChar);
                    }
                }
                return _modelNumber;
            }
        }

        protected abstract string SerialNumberMessage { get; }
        protected virtual string SerialNumberRegEx { get { return ".*"; } }
        protected virtual char SerialNumberTermChar { get { return '\uD800'; } }
        private string _serialNumber;
        public string SerialNumber
        {
            get
            {
                lock (this)
                {
                    if (_serialNumber == null)
                    {
                        Connection.SendCommand(SerialNumberMessage);
                        _serialNumber = Connection.GetResponse<string>(SerialNumberRegEx, SerialNumberTermChar);
                    }
                }
                return _serialNumber;
            }
        }

        protected abstract string FirmwareVersionMessage { get; }
        protected virtual string FirmwareVersionRegEx { get { return ".*"; } }
        protected virtual char FirmwareVersionTermChar { get { return '\uD800'; } }
        private string _firmwareVersion;
        public string FirmwareVersion
        {
            get
            {
                lock (this)
                {
                    if (_firmwareVersion == null)
                    {
                        Connection.SendCommand(FirmwareVersionMessage);
                        _firmwareVersion = Connection.GetResponse<string>(FirmwareVersionRegEx, FirmwareVersionTermChar);
                    }
                    return _firmwareVersion;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Debug.WriteLine("Disposing " + Description);
                Connection.Dispose();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class InitCommandsAttribute : Attribute
    {
        public string[] Commands { get; private set; }

        public InitCommandsAttribute(params string[] commands)
        {
            Commands = commands;
        }

    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdQueryAttribute : Attribute
    {
        public string Response { get; private set; }
        public string Command { get; private set; }
        public char TermChar { get; private set; }

        public IdQueryAttribute(string response, string command = "", string termChar = null)
        {
            Response = response;
            Command = command;
            TermChar = termChar != null ? termChar[0] : '\uD800';
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ConnectionSettingAttribute : Attribute
    {
        public string Name { get; private set; }
        public Type ArgumentType { get; private set; }
        public object ArgumentValue { get; private set; }

        public ConnectionSettingAttribute(string name, string type, string val)
        {
            Name = name;

            ArgumentType = Type.GetType("System." + type);
            if (ArgumentType == null)
                throw new ConnectionSettingAttributeException(type, name);

            ArgumentValue = Convert.ChangeType(val, ArgumentType);
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandDelayAttribute : Attribute
    {
        public int Delay { get; private set; }

        public CommandDelayAttribute(string delay)
        {
            Delay = int.Parse(delay);
        }
    }
}
