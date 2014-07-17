using System;
using System.Diagnostics;
using TsdLib.Controller;
using TsdLib.Instrument;
using TsdLib.TestSequence;
using TsdLib.View;

namespace ConsoleTest
{
    class ConsoleTestProgram
    {
        private static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            //Define test sequence in TsdLib.TestSequences.TestSequenceDefinitions.DummyTestSequence.cs
            var c = new ControllerBase(new ViewBase(), new DummyTestSequence(), new Settings());
            c.Launch();

            Console.WriteLine("Done");
        }
    }
}
