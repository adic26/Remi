
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

            for (int v = 10; v >= 0; v--)
            {
                Token.ThrowIfCancellationRequested();
                DummyPowerSupply ps = DummyPowerSupply.Connect();
                ps.SetVoltage(v);
                
                TestResults.AddMeasurement("Current", ps.GetCurrent(), "Amps", 5, 10);
                Thread.Sleep(1000);
            }
        }
    }
}
