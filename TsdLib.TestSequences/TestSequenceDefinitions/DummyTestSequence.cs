using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TsdLib.Instrument;

namespace TsdLib.TestSequence
{
    public class DummyTestSequence : ITestSequence
    {
        public MeasurementCollection Measurements { get; private set; }

        public DummyTestSequence()
        {
            Measurements = new MeasurementCollection();
        }

        public void Execute()
        {
            Dummy_Aglient6632B ps = Dummy_Aglient6632B.Connect();

            ps.SetVoltage(4.0);

            ps.DummyConnection.StringToRead = "1";

            for (int temp = -20; temp <= 60; temp += 20)
                Measurements.AddMeasurement("Current", ps.ReadCurrent(), "Amps", 0.1, 1.2,
                    new MeasurementParameter("Temperature", temp));
        }
    }
}
