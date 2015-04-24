using System;
using System.Diagnostics;

namespace TsdLib.TestSystem.TestSequence
{
    class SequenceTraceListener : TraceListener
    {
        private readonly TestSequenceBase _sequence;

        public SequenceTraceListener(TestSequenceBase sequence)
        {
            _sequence = sequence;
        }

        public override void Write(string message)
        {
            _sequence.OnTraceOutput(message);
        }

        public override void WriteLine(string message)
        {
            Write(message + Environment.NewLine);
        }
    }
}
