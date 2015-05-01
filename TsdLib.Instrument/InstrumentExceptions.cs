using System;
using System.Runtime.Serialization;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Exception due to an error finding or connecting to instruments of a specified type on a specified connection type.
    /// </summary>
    /// <typeparam name="TInstrument">Type of instrument.</typeparam>
    /// <typeparam name="TConnection">Type of connection.</typeparam>
    [Serializable]
    [Obsolete("Use the non-generic class instead")]
    public class ConnectException<TInstrument, TConnection> : TsdLibException
        where TConnection : ConnectionBase
        where TInstrument : InstrumentBase<TConnection>
    {
        /// <summary>
        /// Initialize a new InstrumentFinderException.
        /// </summary>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the InstrumentFinderException</param>
        public ConnectException(Exception inner = null)
            : base("Could not connect to any " + typeof(TInstrument).Name + " instruments via " + typeof(TConnection).Name, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the ConnectException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected ConnectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an error finding or connecting to instruments of a specified type on a specified connection type.
    /// </summary>
    [Serializable]
    public class ConnectException : TsdLibException
    {
        /// <summary>
        /// Initialize a new ConnectException.
        /// </summary>
        /// <param name="instrumentType">The type of instrument.</param>
        /// <param name="connectionType">The type of connection.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the ConnectException</param>
        public ConnectException(string instrumentType, string connectionType, Exception inner = null)
            : base("Could not connect to any " + instrumentType + " instruments via " + connectionType, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the ConnectException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected ConnectException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
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
        /// /// <param name="message">Message describing the error.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the CommunicationException.</param>
        public CommunicationException(ConnectionBase connection, string message, Exception inner = null)
            : base("Communication error on " + connection.Description + Environment.NewLine + message, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the CommunicationException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected CommunicationException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an invalid command being sent to an instrument.
    /// </summary>
    [Serializable]
    public class CommandException : TsdLibException
    {
        /// <summary>
        /// Initialize a new CommandException caused by the specified command with a specified inner exception.
        /// </summary>
        /// <param name="command">Command that caused the error.</param>
        /// <param name="message">Message describing the error.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the CommandException.</param>
       
        public CommandException(string command, string message, Exception inner = null)
            : base("Error sending command: " + command + Environment.NewLine + message, inner) { }

        /// <summary>
        /// Initialize a new CommandException caused by the specified command with a specified inner exception.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// <param name="command">Command that caused the error.</param>
        /// <param name="message">Message describing the error.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the CommandException.</param>
        public CommandException(ConnectionBase connection, string command, string message, Exception inner = null)
            : base(string.Format("Error sending command: {0} to {1}.{3}Error details: {2}", command, connection.Description, message, Environment.NewLine), inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the CommandException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected CommandException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an invalid response being received from an instrument.
    /// </summary>
    [Serializable]
    public class ResponseException : TsdLibException
    {
        /// <summary>
        /// Initialize a new ResponseException.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the CommandException.</param>
        public ResponseException(ConnectionBase connection, Exception inner = null)
            : base("Error getting response from " + connection.Description, inner) { }

        /// <summary>
        /// Initialize a new ResponseException caused by an error processing the specified response.
        /// </summary>
        /// <param name="connection">Connection where the error occurred.</param>
        /// <param name="message">Message describing the error.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the CommandException.</param>
        public ResponseException(ConnectionBase connection, string message, Exception inner = null)
            : base("Error processing response: " + message + " from " + connection.Description, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the ResponseException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected ResponseException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }

    /// <summary>
    /// Exception due to an invlaid data type being assigned to a ConnectionSettingAttribute.
    /// </summary>
    [Serializable]
    public class ConnectionSettingAttributeException : TsdLibException
    {
        /// <summary>
        /// Initialize a new ConnectionSettingAttributeExceptionfor the specified data type, ConnectionSettingAttribute name.
        /// </summary>
        /// <param name="type">Data type attempting to be assigned to the ConnectionSettingAttribute.</param>
        /// <param name="name">Name of the ConnectionSettingAttribute.</param>
        /// <param name="inner">OPTIONAL: The Exception that is the cause of the ConnectionSettingAttributeException.</param>
        public ConnectionSettingAttributeException(string type, string name, Exception inner = null)
            : base(type + " is not a valid type for the connection setting: " + name, inner) { }

        /// <summary>
        /// Deserialization constructor used by the .NET Framework to initialize an instance of the ConnectionSettingAttributeException class from serialized data.
        /// </summary>
        /// <param name="info">The SerialzationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains the contextual information about the source or destination.</param>
        protected ConnectionSettingAttributeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}