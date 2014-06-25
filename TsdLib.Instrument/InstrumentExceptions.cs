using System;

namespace TsdLib.Instrument
{
    [Serializable]
    public class InstrumentFinderException : TsdLibException
    {
        public InstrumentFinderException(string message)
            : base(message)
        {
        }

        public InstrumentFinderException(string message, Exception inner)
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
}