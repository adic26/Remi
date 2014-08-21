using System;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Exception due to an error discovering or connecting to instruments.
    /// </summary>
    [Serializable]
    public class InstrumentFactoryException : TsdLibException
    {
        /// <summary>
        /// Initialize a new InstrumentFactoryException with a specified message.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        public InstrumentFactoryException(string message)
            : base(message) { }

        /// <summary>
        /// Initialize a new InstrumentFactoryException with a specified message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">The Exception that is the cause of the InstrumentFactoryException.</param>
        public InstrumentFactoryException(string message, Exception inner)
            : base(message, inner) { }
    }

    /// <summary>
    /// Exception due to a communication error or an invalid command being sent to an instrument.
    /// </summary>
    [Serializable]
    public class CommandException : TsdLibException
    {
        /// <summary>
        /// Initialize a new CommandException with a specified message.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        public CommandException(string message)
            : base(message) { }

        /// <summary>
        /// Initialize a new CommandException with a specified message and inner exception.
        /// </summary>
        /// <param name="message">Message describing the exception.</param>
        /// <param name="inner">The Exception that is the cause of the CommandException.</param>
        public CommandException(string message, Exception inner)
            : base(message, inner) { }
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
            : this(type, name, null) { }

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