using System;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Exception due to an error finding or connecting to instruments of a specified type on a specified connection type.
    /// </summary>
    [Serializable]
    public class ConnectException<TInstrument, TConnection> : TsdLibException
    {
        /// <summary>
        /// Initialize a new InstrumentFinderException.
        /// </summary>
        public ConnectException()
            : base("Could not connect to any " + typeof(TInstrument).Name + " instruments via " + typeof(TConnection).Name) { }

        /// <summary>
        /// Initialize a new InstrumentFinderException with a specified inner exception.
        /// </summary>
        /// <param name="inner">The Exception that is the cause of the InstrumentFinderException</param>
        public ConnectException(Exception inner)
            : base("Could not connect to any " + typeof(TInstrument).Name + " instruments via " + typeof(TConnection).Name, inner) { }
    }

    /// <summary>
    /// Exception due to a communication error with an instrument.
    /// </summary>
    [Serializable]
    public class CommunicationException : TsdLibException
    {
        /// <summary>
        /// Initialize a new CommunicationExcption with a specified message.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// <param name="message">Message describing the error.</param>
        public CommunicationException(ConnectionBase connection, string message)
            : base("Communication error on " + connection.Description + Environment.NewLine + message) { }

        /// <summary>
        /// Initialize a new CommunicationExcption with a specified message and inner exception.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// /// <param name="message">Message describing the error.</param>
        /// <param name="inner">The Exception that is the cause of the CommunicationException.</param>
        public CommunicationException(ConnectionBase connection, string message, Exception inner)
            : base("Communication error on " + connection.Description + Environment.NewLine + message, inner) { }
    }

    /// <summary>
    /// Exception due to a communication error or an invalid command being sent to an instrument.
    /// </summary>
    [Serializable]
    public class CommandException : TsdLibException
    {
        /// <summary>
        /// Initialize a new CommandException caused by an error receiving a response.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        public CommandException(ConnectionBase connection)
            : base("Error getting response from " + connection.Description) { }

        /// <summary>
        /// Initialize a new CommandException caused by an error receiving a response with a specified inner exception..
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// <param name="inner">The Exception that is the cause of the CommandException.</param>
        public CommandException(ConnectionBase connection, Exception inner)
            : base("Error getting response from " + connection.Description, inner) { }

        /// <summary>
        /// Initialize a new CommandException caused by the specified command.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// <param name="command">Command that caused the error.</param>
        public CommandException(ConnectionBase connection, string command)
            : base("Error sending command: " + command + " to " + connection.Description) { }

        /// <summary>
        /// Initialize a new CommandException caused by the specified command with a specified inner exception.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// <param name="command">Command that caused the error.</param>
        /// <param name="inner">The Exception that is the cause of the CommandException.</param>
        public CommandException(ConnectionBase connection, string command, Exception inner)
            : base("Error sending command: " + command + " to " + connection.Description, inner) { }
    }

    /// <summary>
    /// Exception due to an invlaid data type being assigned to a ConnectionSettingAttribute.
    /// </summary>
    [Serializable]
    public class ConnectionSettingAttributeException : TsdLibException
    {
        /// <summary>
        /// Initialize a new ConnectionSettingAttributeExceptionfor the specified data type and ConnectionSettingAttribute name.
        /// </summary>
        /// <param name="type">Data type attempting to be assigned to the ConnectionSettingAttribute.</param>
        /// <param name="name">Name of the ConnectionSettingAttribute.</param>
        public ConnectionSettingAttributeException(string type, string name)
            : base(type + " is not a valid type for the connection setting: " + name) { }

        /// <summary>
        /// Initialize a new ConnectionSettingAttributeExceptionfor the specified data type, ConnectionSettingAttribute name and inner Exception.
        /// </summary>
        /// <param name="type">Data type attempting to be assigned to the ConnectionSettingAttribute.</param>
        /// <param name="name">Name of the ConnectionSettingAttribute.</param>
        /// <param name="inner">The Exception that is the cause of the ConnectionSettingAttributeException.</param>
        public ConnectionSettingAttributeException(string type, string name, Exception inner)
            : base(type + " is not a valid type for the connection setting: " + name, inner) { }
    }
}