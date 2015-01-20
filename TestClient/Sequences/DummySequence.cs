
using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class DummySequence : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            //System.Diagnostics.Debugger.Break();

            DummyPowerSupply ps = new DummyPowerSupply("addr");

            for (int i = 0; i < testConfig.LoopIterations; i++)
            {
                foreach (double voltageSetting in testConfig.VoltageSettings)
                {
                    token.ThrowIfCancellationRequested();
                    ps.SetVoltage(voltageSetting);
                    Thread.Sleep(productConfig.SettlingTime);

                    MeasurementParameter[] measurementParameters =
                    {
                        new MeasurementParameter("Loop Iteration", i),
                        new MeasurementParameter("Voltage", voltageSetting),
                        new MeasurementParameter("Temperature", 22.5)
                    };
                    throw new TsdLib.Instrument.CommandException("Some command", "testing exception logger");
                    Measurement<double> measurement = new Measurement<double>("Current", ps.GetCurrent(), "Amps", 0.1, 0.8, parameters: measurementParameters);
                    Measurements.Add(measurement);
                }
            }
            
        }
    }
}
