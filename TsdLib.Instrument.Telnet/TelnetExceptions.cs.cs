using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.Instrument.Telnet
{
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
