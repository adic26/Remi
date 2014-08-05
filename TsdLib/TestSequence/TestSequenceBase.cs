using System.Diagnostics;
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
        protected CancellationToken Token;

        public MeasurementCollection Measurements { get; protected set; }

        protected TestSequenceBase()
        {
            Measurements = new MeasurementCollection();
        }

        //Asynchronous wrapper to make sure test sequences don't run on the UI thread
        public async Task ExecuteAsync(TStationConfig stationConfig, TProductConfig productConfig, CancellationToken token, params IInstrument[] instruments)
        {
            Token = token;
            Trace.WriteLine(string.Format("Executing test sequence: {0} with station config: {1} and product config: {2}.", GetType().Name, stationConfig.Name, productConfig.Name));
            await Task.Run(() => ConnectToInstruments(instruments), Token); //This method won't do anything until we figure out how to parse out the instruments before executing the test sequence
            await Task.Run(() => Execute(stationConfig, productConfig), Token);
            Trace.WriteLine("Test sequence complete.");
        }
        
        //This is the method that station-specific test sequences will override to define their test steps
        protected abstract void Execute(TStationConfig stationConfig, TProductConfig productConfig);

        private void ConnectToInstruments(params IInstrument[] instruments)
        {
            foreach (IInstrument instrument in instruments)
            {
                Token.ThrowIfCancellationRequested();
                Measurements.AddMeasurement("Model Number", instrument.ModelNumber, "Identification", "n/a", "n/a");
                Measurements.AddMeasurement("Serial Number", instrument.SerialNumber, "Identification", "n/a", "n/a");
                Measurements.AddMeasurement("Firmware Version", instrument.FirmwareVersion, "Identification", "n/a", "n/a");
            }
        }
    }
}
