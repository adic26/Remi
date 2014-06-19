using System;

namespace TsdLib.Instrument
{
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