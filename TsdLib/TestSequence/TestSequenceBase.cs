using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using TsdLib.Configuration;
using TsdLib.Instrument;
using TsdLib.Measurements;
using TsdLib.Proxies;

namespace TsdLib.TestSequence
{
    /// <summary>
    /// Contains test sequence functionality common to all TsdLib applications.
    /// </summary>
    /// <typeparam name="TStationConfig">Type of Station Config used in the derived class.</typeparam>
    /// <typeparam name="TProductConfig">Type of Product Config used in the derived class.</typeparam>
    /// <typeparam name="TTestConfig">Type of Test Config used in the derived class.</typeparam>
    public abstract class TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> : MarshalByRefObject//, ISequence<TStationConfig, TProductConfig, TTestConfig>
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
        where TTestConfig : TestConfigCommon
    {
        private readonly CancellationTokenSource _cts;

        /// <summary>
        /// Gets a CancellationToken that can be used to cancel a test sequence in progress.
        /// </summary>
        protected CancellationToken Token { get { return _cts.Token; } }

        /// <summary>
        /// Gets the collection of Measurement objects captured during the course of the Test Sequence execution.
        /// </summary>
        public MeasurementCollection Measurements { get; protected set; }

        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send events across AppDomain boundaries.
        /// </summary>
        public EventProxy<MeasurementEventArgs> MeasurementEventProxy { get; set; }

        /// <summary>
        /// Initializes the TestSequenceBase object.
        /// </summary>
        protected TestSequenceBase()
        {
            _cts = new CancellationTokenSource();
            Measurements = new MeasurementCollection();
            Measurements.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list != null && MeasurementEventProxy != null)
                    MeasurementEventProxy.FireEvent(new MeasurementEventArgs((Measurement)list[e.NewIndex]));
            };
        }

        /// <summary>
        /// Adds the specified TraceListener to the Trace Listeners collection. Useful if running the test sequence from a separate application domain.
        /// </summary>
        /// <param name="listener">TraceListener to add to the Trace Listeners collection.</param>
        public void AddTraceListener(TraceListener listener)
        {
            Trace.Listeners.Add(listener);
        }

        /// <summary>
        /// Client application overrides this method to define test steps.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfig">Test config instance containing test-specific configuration.</param>
        protected abstract void Execute(TStationConfig stationConfig, TProductConfig productConfig, TTestConfig testConfig);

        /// <summary>
        /// Start execution of the test sequence with the specified configuration objects.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfig">Test config instance containing test-specific configuration.</param>
        public void ExecuteSequence(TStationConfig stationConfig, TProductConfig productConfig, TTestConfig testConfig)
        {
            try
            {
                Trace.WriteLine("Executing test sequence...");
                DateTime starTime = DateTime.Now;
                Execute(stationConfig, productConfig, testConfig);
                var failedMeasurements = Measurements.Select(m => m.Result.HasFlag(MeasurementResult.Fail));

                string finalResult = failedMeasurements.Any() ? "Fail" : "Pass";

                Measurements.AddHeader( new MeasurementCollectionHeader(
                    "_JobNumber",
                    "_UnitNumber",
                    "_TestType",
                    "_TestStage",
                    "_BSN",
                    finalResult,
                    starTime,
                    DateTime.Now,
                    "_AdditionalInfo"
                    ));

                XmlSerializer xs = new XmlSerializer(typeof(MeasurementCollection));
                string measurementFile = Path.Combine(SpecialFolders.Measurements, "Results.xml");
                using (Stream s = File.Create(measurementFile))
                    xs.Serialize(s, Measurements);

                string resultsFolder = @"C:\TestResults";
                if (!Directory.Exists(resultsFolder))
                    Directory.CreateDirectory(resultsFolder);
                string resultsFile = "Results_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xml";
                string resultsPath = Path.Combine(resultsFolder, resultsFile);
                using (Stream s2 = File.Create(resultsPath))
                    xs.Serialize(s2, Measurements);

                Trace.WriteLine("Test sequence completed.");

                Process.Start(resultsPath);
            }
            catch (OperationCanceledException)
            {
                Trace.WriteLine("Test sequence was cancelled by user.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }

        /// <summary>
        /// Abort the currently executing test sequence.
        /// </summary>
        public void Abort()
        {
            _cts.Cancel();
        }

        /// <summary>
        /// Dispose of any resources being used by the test sequence.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of any resources being used by the test sequence.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _cts.Dispose();
        }

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
