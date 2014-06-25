﻿using System;

namespace TsdLib.Instrument.Telnet
{
    [Serializable]
    public class TelnetException : TsdLibException
    {
        public TelnetException(string message)
            : base(message)
        {
        }

        public TelnetException(string message, Exception inner)
            : base(message, inner)
        {
        }

    }
}
