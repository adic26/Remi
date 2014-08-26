using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.TestSequence
{
    public class TestSequenceException : TsdLibException
    {
        public TestSequenceException(string sequenceFileName)
            : base(sequenceFileName + " does not contain valid test sequence source code.") { }
    }
}
