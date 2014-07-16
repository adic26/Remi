using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.Instrument
{
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
}
