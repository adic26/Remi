using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TsdLib.Configuration;
using TsdLib.Instrument;

namespace TsdLib.TestSequence
{
    public abstract class TestSequenceBase<TStationConfig, TProductConfig>
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
    {
        private readonly IEnumerable<IInstrument> _instruments;

        public MeasurementCollection Measurements { get; protected set; }

        protected TestSequenceBase()
        {
            _instruments = new List<IInstrument>();
            Measurements = new MeasurementCollection();
        }

        //This is the method that station-specific test sequences willToverride to define their test steps
        protected abstract void Execute(TStationConfig stationConfig, TProductConfig productConfig, CancellationToken token);

        //Asynchronous wrapper to make sure test sequences don't run on the UI thread
        public async Task ExecuteAsync(TStationConfig stationConfig, TProductConfig productConfig, CancellationToken token)
        {
            await Task.Run(() => ConnectToInstruments(token), token); //This method won't do anything until we figure out how to parse out the instruments before executing the test sequence
            await Task.Run(() => Execute(stationConfig, productConfig, token), token);
        }

        
        private void ConnectToInstruments(CancellationToken token)
        {//TODO: how to populate _instruments?? May just need to rely on Execute method to connect to all required instruments for now
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
