using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using TsdLib;
using TsdLib.TestSequence;
using TsdLib.Controller;

namespace ConsoleApplication1
{
    class ConsoleApplication1Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var c = new ControllerBase(new Settings(), new TestSequenceBase());
            c.Run();

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
