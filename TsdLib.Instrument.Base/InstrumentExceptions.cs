using System;

namespace TsdLib.Instrument
{
    [Serializable]
    public class InstrumentException : TsdLibException
    {
        public InstrumentException(string message)
            : base(message)
        {

        }

        public InstrumentException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }

    [Serializable]
    public class InstrumentFactoryException : TsdLibException
    {
        public InstrumentFactoryException(string message)
            : base(message)
        {
        }

        public InstrumentFactoryException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    [Serializable]
    public class CommandException : TsdLibException
    {
        public CommandException(string message)
            : base(message)
        {
        }

        public CommandException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    [Serializable]
    public class ConnectionSettingAttributeException : TsdLibException
    {
        public ConnectionSettingAttributeException(string type, string name)
            : this(type, name, null)
        {
        }

        public ConnectionSettingAttributeException(string type, string name, Exception inner)
            : base(type + " is not a valid type for the connection setting: " + name, inner)
        {
        }
    }
}