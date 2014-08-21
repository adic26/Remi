using System;
using System.Diagnostics;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Common interface to expose model/serial numbers and firmware version get accessors.
    /// </summary>
    public interface IInstrument : IDisposable
    {
        /// <summary>
        /// Gets the instrument model number.
        /// </summary>
        string ModelNumber { get; }
        /// <summary>
        /// Gets the instrument serial number.
        /// </summary>
        string SerialNumber { get; }
        /// <summary>
        /// Gets the instrument firmware version.
        /// </summary>
        string FirmwareVersion { get; }
    }

    /// <summary>
    /// Common base class for all instrument implementations.
    /// </summary>
    /// <typeparam name="TConnection">Type of connection to use for communication with the instrument.</typeparam>
    public abstract class InstrumentBase<TConnection> : IInstrument
        where TConnection : ConnectionBase
    {
        /// <summary>
        /// A connection object to used to communicate with the instrument.
        /// </summary>
        internal protected readonly TConnection Connection;

        /// <summary>
        /// Gets a description of the instrument, including connection information.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Initialize a new instrument object with the specified connection.
        /// </summary>
        /// <param name="connection">A connection object to communicate with the instrument.</param>
        protected InstrumentBase(TConnection connection)
        {
            Connection = connection;

            InitCommandsAttribute initCommands = (InitCommandsAttribute)Attribute.GetCustomAttribute(GetType(), typeof(InitCommandsAttribute), true);
            if (initCommands != null)
                foreach (string command in initCommands.Commands)
                    Connection.SendCommand(command, -1);

            Description = GetType().Name + " via " + connection.Description;
        }

        /// <summary>
        /// Gets the initialization commands that are sent to the instrument as part of the connection process.
        /// </summary>
        protected virtual string InitCommands { get { return ""; } }

        /// <summary>
        /// Gets the message to send to the instrument to query the model number.
        /// </summary>
        protected abstract string ModelNumberMessage { get; }
        /// <summary>
        /// Gets the regular expression used to parse the response when querying the model number.
        /// </summary>
        protected virtual string ModelNumberRegEx { get { return ".*"; } }
        /// <summary>
        /// Gets the termination character (if any) that the instrument will send to signal the end of the model number query response.
        /// </summary>
        protected virtual char ModelNumberTermChar { get { return '\uD800'; } }
        private string _modelNumber;
        /// <summary>
        /// Gets the instrument model number.
        /// </summary>
        public string ModelNumber
        {
            get
            {
                if (_modelNumber == null)
                {
                    lock (this)
                    {
                        Connection.SendCommand(ModelNumberMessage, -1);
                        _modelNumber = Connection.GetResponse<string>(ModelNumberRegEx, ModelNumberTermChar);
                    }
                }
                return _modelNumber;
            }
        }

        /// <summary>
        /// Gets the message to send to the instrument to query the serial number.
        /// </summary>
        protected abstract string SerialNumberMessage { get; }
        /// <summary>
        /// Gets the regular expression used to parse the response when querying the serial number.
        /// </summary>
        protected virtual string SerialNumberRegEx { get { return ".*"; } }
        /// <summary>
        /// Gets the termination character (if any) that the instrument will send to signal the end of the serial number query response.
        /// </summary>
        protected virtual char SerialNumberTermChar { get { return '\uD800'; } }
        private string _serialNumber;
        /// <summary>
        /// Gets the instrument serial number.
        /// </summary>
        public string SerialNumber
        {
            get
            {
                lock (this)
                {
                    if (_serialNumber == null)
                    {
                        Connection.SendCommand(SerialNumberMessage, -1);
                        _serialNumber = Connection.GetResponse<string>(SerialNumberRegEx, SerialNumberTermChar);
                    }
                }
                return _serialNumber;
            }
        }

        /// <summary>
        /// Gets the message to send to the instrument to query the firmware version.
        /// </summary>
        protected abstract string FirmwareVersionMessage { get; }
        /// <summary>
        /// Gets the regular expression used to parse the response when querying the firmware version.
        /// </summary>
        protected virtual string FirmwareVersionRegEx { get { return ".*"; } }
        /// <summary>
        /// Gets the termination character (if any) that the instrument will send to signal the end of the firmware version query response.
        /// </summary>
        protected virtual char FirmwareVersionTermChar { get { return '\uD800'; } }
        private string _firmwareVersion;
        /// <summary>
        /// Gets the instrument firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get
            {
                lock (this)
                {
                    if (_firmwareVersion == null)
                    {
                        Connection.SendCommand(FirmwareVersionMessage, -1);
                        _firmwareVersion = Connection.GetResponse<string>(FirmwareVersionRegEx, FirmwareVersionTermChar);
                    }
                    return _firmwareVersion;
                }
            }
        }

        /// <summary>
        /// Closes the instrument connection and disposes of any resources being used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Closes the instrument connection and disposes of any resources being used.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Debug.WriteLine("Disposing " + Description);
                Connection.Dispose();
            }
        }
    }

    /// <summary>
    /// Custom attribute to define the initialization commands to send to the instrument during the connection process.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class InitCommandsAttribute : Attribute
    {
        /// <summary>
        /// Gets the commands in an array.
        /// </summary>
        public string[] Commands { get; private set; }

        /// <summary>
        /// Initialize a new InitCommandsAttribute with the specified commands.
        /// </summary>
        /// <param name="commands">Zero or more commands to be sent to the instrument during the connection process.</param>
        public InitCommandsAttribute(params string[] commands)
        {
            Commands = commands;
        }

    }

    /// <summary>
    /// Custom attribute to define the command to query the instrument identification and the expected response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdQueryAttribute : Attribute
    {
        /// <summary>
        /// Gets the identification query response expected to be returned by the instrument.
        /// </summary>
        public string Response { get; private set; }
        /// <summary>
        /// Gets the command to send to the instrument to request identification.
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Gets the termination character (if any) that the instrument sends to signal the end of the identification query response.
        /// </summary>
        public char TermChar { get; private set; }

        /// <summary>
        /// Initialize a new IdQueryAttribute with the specified reponse, command and termination charater.
        /// </summary>
        /// <param name="response">Identification query response expected to be returned by the instrument.</param>
        /// <param name="command">OPTIONAL: Command to send to the instrument to request identification. Only required for instruments where an identification query is originated from the client.</param>
        /// <param name="termChar">OPTIONAL: Termination character (if any) that the instrument sends to signal the end of the identification query response.</param>
        public IdQueryAttribute(string response, string command = "", string termChar = null)
        {
            Response = response;
            Command = command;
            TermChar = termChar != null ? termChar[0] : '\uD800';
        }
    }

    /// <summary>
    /// Custom attribute to define any connection settings specific to the instrument (ie. serial port baud rate, etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ConnectionSettingAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the connection setting.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the data type of the connection setting.
        /// </summary>
        public Type ArgumentType { get; private set; }
        /// <summary>
        /// Gets the value of the connection setting.
        /// </summary>
        public object ArgumentValue { get; private set; }

        /// <summary>
        /// Initialize a new ConnectionSettingAttribute with the specified name, data type and value.
        /// </summary>
        /// <param name="name">Name of the connection setting.</param>
        /// <param name="type">Data type of the connection setting. Will be used to cast the value into a strongly-typed object.</param>
        /// <param name="val">Value of the connection setting.</param>
        public ConnectionSettingAttribute(string name, string type, string val)
        {
            Name = name;

            ArgumentType = Type.GetType("System." + type);
            if (ArgumentType == null)
                throw new ConnectionSettingAttributeException(type, name);

            ArgumentValue = Convert.ChangeType(val, ArgumentType);
        }
    }

    /// <summary>
    /// Custom attribute to define a default delay (in ms) to wait before sending each command to the instrument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandDelayAttribute : Attribute
    {
        /// <summary>
        /// Gets the delay (in ms).
        /// </summary>
        public int Delay { get; private set; }

        /// <summary>
        /// Initialize a new CommandDelayAttribute with the specified delay.
        /// </summary>
        /// <param name="delay">Delay (in ms).</param>
        public CommandDelayAttribute(string delay)
        {
            Delay = int.Parse(delay);
        }
    }
}
