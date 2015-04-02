
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.Configuration;
using TsdLib.Configuration.Management;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class ConfigManagerDemo : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //Use configManagerProvider.GetConfigManager to retieve objects that can read, modify and write configuration data from within the test sequence
            IConfigManager<ProductConfig> productConfigManager = Config.GetConfigManager<ProductConfig>();
            IConfigManager<StationConfig> stationConfigManager = Config.GetConfigManager<StationConfig>();

            //To get a config object by name - ie. based on identification information provided by a DUT
            ProductConfig defaultProduct = productConfigManager.GetConfig("Windermere");  //we can dynamically access the Windermere config
            int somethingToReadFromTheConfig = defaultProduct.SettlingTime;

            //To modify the config object passed in from the UI
            stationConfig.PathLoss = 25;

            //To create a new config object by name
            StationConfig newStationConfig = stationConfigManager.Add("Some new Config 2", false);
            newStationConfig.PathLoss = 22;

            //Persist any changes we've made to the config data - this updates the underlying xml config files and pushes changes to the shared config location
            stationConfigManager.Save();

            DummyPowerSupply ps = new DummyPowerSupply("addr");

            for (int i = 0; i < testConfig.LoopIterations; i++)
            {
                foreach (double voltageSetting in testConfig.VoltageSettings)
                {
                    token.ThrowIfCancellationRequested();
                    ps.SetVoltage(voltageSetting);
                    Thread.Sleep(productConfig.SettlingTime);

                    IMeasurementParameter[] measurementParameters =
                    {
                        new MeasurementParameter("Loop Iteration", i),
                        new MeasurementParameter("Voltage", voltageSetting),
                        new MeasurementParameter("Temperature", 22.5)
                    };
                    Measurement<double> measurement = new Measurement<double>("Current", ps.GetCurrent(), "Amps", 0.1, 0.8, parameters: measurementParameters);
                    AddMeasurement(measurement);
                }
            }
            
        }
    }
}
