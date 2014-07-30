using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using TestClient.TestSequences;

namespace TestClient
{
    class TestClientProgram
    {
        [STAThread]
        private static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            Controller c = new Controller(new View(), new DummyTestSequence());
            c.Launch();

            Console.WriteLine("Done");
        }
    }
}
