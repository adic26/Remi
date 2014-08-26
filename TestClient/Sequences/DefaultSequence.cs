
using System;
using System.Diagnostics;
using System.Threading;
using TsdLib.TestSequence;

namespace TestClient.Sequences
{
    public class TestSequence : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void Execute(StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //TODO: Create test sequence. This is the step-by-step sequence of instrument and/or DUT commands and measurements

            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            //Debugger.Break();

            for (int i = 10; i >= 0; i--)
            {
                Token.ThrowIfCancellationRequested();
                Measurements.AddMeasurement("name", i, "volts", 5, 10);
                Thread.Sleep(1000);
            }

            //Debugger.Break();
        }
    }
}
