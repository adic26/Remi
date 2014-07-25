using System;
using System.Diagnostics;
using TsdLib.Controller;
using TsdLib.TestSequence;

namespace TestClient
{
    class TestClientProgram
    {
        private static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            //Define test sequence in TsdLib.TestSequences.TestSequenceDefinitions.DummyTestSequence.cs
            var c = new ControllerBase(new TestClientView(), new DummyTestSequence(), new Settings());
            c.Launch();

            Console.WriteLine("Done");
        }
    }
}
