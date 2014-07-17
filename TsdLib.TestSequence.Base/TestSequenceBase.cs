using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TsdLib.Instrument;
using TsdLib.Instrument.Telnet;

namespace TsdLib.TestSequence
{
    public abstract class TestSequenceBase
    {
        private readonly IEnumerable<IInstrument> _instruments;

        public MeasurementCollection Measurements { get; protected set; }

        protected TestSequenceBase()
        {
            _instruments = new List<IInstrument>();
            Measurements = new MeasurementCollection();
        }

        //This is the method that station-specific test sequences will override to define their test steps
        protected abstract void Execute(CancellationToken token);

        public async Task ExecuteAsync(CancellationToken token)
        {
            await Task.Run(() => ConnectToInstruments(token), token); //This method won't do anything until we figure out how to parse out the instruments before executing the test sequence
            await Task.Run(() => Execute(token), token);
        }

        //TODO: how to populate _instruments?? May just need to rely on Execute method to connect to all required instruments for now
        private void ConnectToInstruments(CancellationToken token)
        {
            foreach (IInstrument instrument in _instruments)
            {
                token.ThrowIfCancellationRequested();
                Measurements.AddMeasurement("Model Number", instrument.ModelNumber, "Identification", "n/a", "n/a");
                Measurements.AddMeasurement("Serial Number", instrument.SerialNumber, "Identification", "n/a", "n/a");
                Measurements.AddMeasurement("Firmware Version", instrument.FirmwareVersion, "Identification", "n/a", "n/a");
            }
        }
    }
}
