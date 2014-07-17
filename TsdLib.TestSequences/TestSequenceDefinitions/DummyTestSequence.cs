﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TsdLib.Instrument;

namespace TsdLib.TestSequence
{
    public class DummyTestSequence : TestSequenceBase
    {
        //TODO: figure out how to pull the instrument.Connect calls out of the test sequence?
        protected override void Execute(CancellationToken token)
        {
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
