using System;
using System.Diagnostics;
using TestClient.Configuration;
using TsdLib.Configuration;

namespace TestClient
{
    class TestClientProgram
    {
        [STAThread]
        private static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            IConfigGroup<StationConfig> stationConfigGroup = Config<StationConfig>.Manager.GetConfigGroup();
            IConfigGroup<ProductConfig> productConfigGroup = Config<ProductConfig>.Manager.GetConfigGroup();

            Controller c = new Controller(new View(), new TestSequence.TestSequence());
            c.Launch();

            Console.WriteLine("Done");
        }
    }
}
