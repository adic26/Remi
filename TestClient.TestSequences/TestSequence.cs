//Click here to view or modify
using System;
using System.Globalization;
using System.Threading;
using TestClient.Instruments;
using TsdLib;
using TsdLib.TestSequence;

namespace TestClient.TestSequences
{
    public class TestSequence : TestSequenceBase<StationConfig, ProductConfig>
    {
        protected override void Execute(StationConfig stationConfig, ProductConfig productConfig)
        {
            //This is what a typical test sequence might look like...

            //TODO: figure out how to pull the instrument.Connect calls out of the test sequence?
            Dummy_Aglient6632B powerSupply = Dummy_Aglient6632B.Connect();
            Measurements.AddMeasurement("Model Number", powerSupply.ModelNumber, "Identification", "n/a", "n/a");
            Measurements.AddMeasurement("Serial Number", powerSupply.SerialNumber, "Identification", "n/a", "n/a");
            Measurements.AddMeasurement("Firmware Version", powerSupply.FirmwareVersion, "Identification", "n/a", "n/a");

            powerSupply.SetVoltage(stationConfig.Voltage);

            Random random = new Random();

            for (int temperature = stationConfig.LowTemperature; temperature <= stationConfig.HighTemperature; temperature += stationConfig.TemperatureStepSize)
            {
                Token.ThrowIfCancellationRequested(); //This line should be inserted periodically to support cancellation.
                Thread.Sleep(1000);

                double randomOffset = random.NextDouble() + 0.5; //Random double between 0.5 and 1.5 to simulate measurements
                powerSupply.DummyConnection.StringToRead = randomOffset.ToString(CultureInfo.InvariantCulture);

                Measurements.AddMeasurement(
                    "Current",                                                  //Measurement Name
                    powerSupply.ReadCurrent(),                                  //Measurement Value
                    "Amps",                                                     //Units
                    productConfig.MinChargeCurrentLimit,                        //Low limit
                    productConfig.MaxChargeCurrentLimit,                        //High limit
                    new MeasurementParameter("Temperature", temperature));      //Measurement Parameters
            }
        }
    }
}
