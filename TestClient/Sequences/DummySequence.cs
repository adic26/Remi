
using System;
using System.Diagnostics;
using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class DummySequence : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        private DummyPowerSupply _ps;

        protected override void ExecutePreTest(CancellationToken token, StationConfig stationConfig, ProductConfig productConfig)
        {
            _ps = new DummyPowerSupply("addr");
        }

        protected override void ExecuteTest(CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            //System.Diagnostics.Debugger.Break();
            SendData(new Tuple<int, string>(1, "2"));
            
            for (int i = 0; i < testConfig.LoopIterations; i++)
            {
                Trace.WriteLine("ITERATION " + i);
                UpdateProgress(i, testConfig.LoopIterations);
                foreach (double voltageSetting in testConfig.VoltageSettings)
                {
                    token.ThrowIfCancellationRequested();
                    _ps.SetVoltage(voltageSetting);
                    Thread.Sleep(productConfig.SettlingTime);

                    IMeasurementParameter[] measurementParameters =
                    {
                        new MeasurementParameter("Loop Iteration", i),
                        new MeasurementParameter("Voltage", voltageSetting),
                        new MeasurementParameter("Temperature", 22.5)
                    };
                    Measurement<double> measurement = new Measurement<double>("Current", _ps.GetCurrent(), "Amps", 0.1, 0.8, parameters: measurementParameters);
                    AddMeasurement(measurement);

                }
            }
        }
    }
}
