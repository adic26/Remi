using System;
using System.Diagnostics;
using TsdLib.TestSequence;
using TsdLib.Controller;
using TsdLib.View;

namespace ConsoleApplication1
{
    class ConsoleApplication1Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var c = new ControllerBase(new ViewBase(), new DummyTestSequence(), new Settings());

            Console.WriteLine("Done");
            
        }
    }
}
