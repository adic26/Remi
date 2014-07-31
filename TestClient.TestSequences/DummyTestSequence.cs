using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib;
using TsdLib.Configuration;
using TsdLib.TestSequence;

namespace TestClient.TestSequences
{
    public class DummyTestSequence : TestSequenceBase
    {
        //TODO: figure out how to pull the instrument.Connect calls out of the test sequence?
        protected override void Execute(CancellationToken token)
        {
            //basicStationConfig object holds all station config properties for the 'Basic' configuration
            StationConfig basicStationConfig = Config<StationConfig>.Manager.GetConfig("Basic");
            Trace.WriteLine(string.Format("{0} is located at {1}", basicStationConfig.Name, basicStationConfig.StationLocation));

            //khanConfig object holds all product config properties for Khan
            ProductConfig khanConfig = Config<ProductConfig>.Manager.GetConfig("Khan");
            Trace.WriteLine(string.Format("{0} has {1} microphones", khanConfig.Name, khanConfig.NumberOfMicrophones));

            //testConfigs is a collection that holds all TestConfig objects
            IConfigGroup<TestConfig> testConfigs = Config<TestConfig>.Manager.GetConfigGroup();
            //Perform test for each type of test config where TestSeverity is set to 'Low'
            foreach (TestConfig config in testConfigs.Where(tc => tc.TestSeverity == "Low"))
                config.Execute(basicStationConfig, khanConfig);


            //This is what a typical test sequence might look like
            Dummy_Aglient6632B powerSupply = Dummy_Aglient6632B.Connect();
            Measurements.AddMeasurement("Model Number", powerSupply.ModelNumber, "Identification", "n/a", "n/a");
            Measurements.AddMeasurement("Serial Number", powerSupply.SerialNumber, "Identification", "n/a", "n/a");
            Measurements.AddMeasurement("Firmware Version", powerSupply.FirmwareVersion, "Identification", "n/a", "n/a");

            powerSupply.SetVoltage(4.0);

            Random random = new Random();

            for (int temp = -20; temp <= 60; temp += 20)
            {
                token.ThrowIfCancellationRequested();
                Thread.Sleep(2000);
                double randomOffset = random.NextDouble() + 0.5; //Random double between 0.5 and 1.5
                powerSupply.DummyConnection.StringToRead = randomOffset.ToString(CultureInfo.InvariantCulture);
                Measurements.AddMeasurement("Current", powerSupply.ReadCurrent(), "Amps", 0.7, 1.3,
                    new MeasurementParameter("Temperature", temp));
            }
        }
    }
}
