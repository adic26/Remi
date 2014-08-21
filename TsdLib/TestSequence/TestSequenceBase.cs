using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TsdLib.Configuration;
using TsdLib.Instrument;

namespace TsdLib.TestSequence
{
    /// <summary>
    /// Contains test sequence functionality common to all TsdLib applications.
    /// </summary>
    /// <typeparam name="TStationConfig">Type of Station Config used in the derived class.</typeparam>
    /// <typeparam name="TProductConfig">Type of Product Config used in the derived class.</typeparam>
    public abstract class TestSequenceBase<TStationConfig, TProductConfig>
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
    {
        /// <summary>
        /// CancellationToken object that should be periodically checked during test sequence execution, eg. Token.ThrowIfCancellationRequested();
        /// </summary>
        protected CancellationToken Token;

        /// <summary>
        /// The collection of Measurement objects captured during the course of the Test Sequence execution.
        /// </summary>
        public MeasurementCollection Measurements { get; protected set; }

        /// <summary>
        /// Initializes the TestSequenceBase object.
        /// </summary>
        protected TestSequenceBase()
        {
            Measurements = new MeasurementCollection();
        }

        /// <summary>
        /// Execute the test sequence on a worker thread.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="token">CancellationToken to support cancellation during the test sequence.</param>
        /// <param name="instruments">An optional array of instruments to automatically capture and record model/serial number and firmware version.</param>
        /// <returns>An awaitable Task object containing the test sequence status.</returns>
        public async Task ExecuteAsync(TStationConfig stationConfig, TProductConfig productConfig, CancellationToken token, params IInstrument[] instruments)
        {
            Token = token;
            Trace.WriteLine(string.Format("Executing test sequence: {0} with station config: {1} and product config: {2}.", GetType().Name, stationConfig.Name, productConfig.Name));
            await Task.Run(() => ConnectToInstruments(instruments), Token); //This method won't do anything until we figure out how to parse out the instruments before executing the test sequence
            await Task.Run(() => Execute(stationConfig, productConfig), Token);
            Trace.WriteLine("Test sequence complete.");
        }
        
        //This is the method that station-specific test sequences will override to define their test steps
        /// <summary>
        /// The method that station-specific test sequences will override to define their test steps.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
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
