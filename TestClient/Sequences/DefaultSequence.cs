
using System;
using System.Globalization;
using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.TestSequence;

[assembly: AssemblyReference("System.dll")]
[assembly: AssemblyReference("System.Xml.dll")]
[assembly: AssemblyReference("TsdLib.dll")]
[assembly: AssemblyReference("TestClient.exe")]
[assembly: AssemblyReference("TestClient.Instruments.dll")]

namespace TestClient.Sequences
{
    public class DefaultSequence : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void Execute(StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //TODO: Create test sequence. This is the step-by-step sequence of instrument and/or DUT commands and measurements

            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            //Debugger.Break();

            Random random = new Random();
            DummyPowerSupply ps = DummyPowerSupply.Connect();

            for (int v = 10; v >= 0; v--)
            {
                Token.ThrowIfCancellationRequested();
                ps.SetVoltage(v);
                ps.DummyConnection.StringToRead = random.NextDouble().ToString(CultureInfo.InvariantCulture);
                TestResults.AddMeasurement("Current", ps.GetCurrent(), "Amps", 0.1, 0.8);
                Thread.Sleep(1000);
            }
        }
    }
}
