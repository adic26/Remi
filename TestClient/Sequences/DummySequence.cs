
using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class DummySequence : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(StationConfig stationConfig, ProductConfig productConfig, params TestConfig[] testConfigs)
        {
            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            //Debugger.Break();

            DummyPowerSupply ps = new DummyPowerSupply("addr");

            foreach (TestConfig testConfig in testConfigs)
            {


                for (int i = 0; i < testConfig.LoopIterations; i++)
                {
                    foreach (double voltageSetting in testConfig.VoltageSettings)
                    {
                        Token.ThrowIfCancellationRequested();
                        ps.SetVoltage(voltageSetting);
                        Thread.Sleep(productConfig.SettlingTime);

                        MeasurementParameter[] measurementParameters =
                        {
                            new MeasurementParameter("Loop Iteration", i),
                            new MeasurementParameter("Voltage", voltageSetting),
                            new MeasurementParameter("Temperature", 22.5)
                        };
                        Measurement<double> measurement = new Measurement<double>("Current", ps.GetCurrent(), "Amps", 0.1, 0.8, parameters: measurementParameters);
                        AddMeasurement(measurement);
                    }
                    //throw new System.Exception();
                }
            }
        }
    }
}
