
using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.TestResults;
using TsdLib.TestSequence;

namespace TestClient.Sequences
{
    public class DefaultSequence : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void Execute(StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //TODO: Create test sequence. This is the step-by-step sequence of instrument and/or DUT commands and measurements

            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            //Debugger.Break();

            PowerSupply ps = new DummyPowerSupply("addr");

            for (int i = 0; i < testConfig.LoopIterations; i++)
            {
                foreach (double voltageSetting in testConfig.VoltageSettings)
                {
                    Token.ThrowIfCancellationRequested();
                    ps.SetVoltage(voltageSetting);
                    Thread.Sleep(productConfig.SettlingTime);
                    
                    MeasurementParameter measurementParameter = new MeasurementParameter("Voltage", voltageSetting);
                    MeasurementParameter measurementParameter2 = new MeasurementParameter("Temperature", 22.5);
                    Measurement<double> measurement = new Measurement<double>("Current_" + i, ps.GetCurrent(), "Amps", 0.1, 0.8, parameters: new[] { measurementParameter, measurementParameter2 });
                    Measurements.Add(measurement);
                }
            }
        }
    }
}
