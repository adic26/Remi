using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;
using TsdLib.Configuration;
using TsdLib.Proxies;
using TsdLib.Instrument;
using TsdLib.TestResults;

namespace TsdLib.TestSequence
{
    /// <summary>
    /// Contains test sequence functionality common to all TsdLib applications.
    /// </summary>
    /// <typeparam name="TStationConfig">Type of Station Config used in the derived class.</typeparam>
    /// <typeparam name="TProductConfig">Type of Product Config used in the derived class.</typeparam>
    /// <typeparam name="TTestConfig">Type of Test Config used in the derived class.</typeparam>
    public abstract class TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> : MarshalByRefObject
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
        /// Gets test metadata and the collection of Measurement objects captured during the Test Sequence execution.
        /// </summary>
        public TestResultCollection TestResults { get; protected set; }

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
            TestResults = new TestResultCollection();
            TestResults.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list != null && MeasurementEventProxy != null)
                    MeasurementEventProxy.FireEvent(new MeasurementEventArgs((Measurement)list[e.NewIndex]));
            };

            FactoryEvents.Connected += FactoryEvents_Connected;
        }

        void FactoryEvents_Connected(object sender, ConnectedEventArgs e)
        {
            TestResults.AddMeasurement("Description", e.Instrument.Description, "n/a", "n/a", "n/a");
            TestResults.AddMeasurement("Model Number", e.Instrument.ModelNumber, "n/a", "n/a", "n/a");
            TestResults.AddMeasurement("Serial Number", e.Instrument.SerialNumber, "n/a", "n/a", "n/a");
            TestResults.AddMeasurement("Firmware Version", e.Instrument.FirmwareVersion, "n/a", "n/a", "n/a");
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
                //Pre-test
                Trace.WriteLine("Executing test sequence...");
                DateTime starTime = DateTime.Now;

                //Execute test
                Execute(stationConfig, productConfig, testConfig);

                //Post-test
                bool overallPass = TestResults.Any() && TestResults.All(m => m.Result == MeasurementResult.Pass);

                TestResults.AddHeader( new TestResultsHeader(
                    "_JobNumber",
                    "_UnitNumber",
                    "_TestType",
                    "_TestStage",
                    "_BSN",
                    overallPass ? "Pass" : "Fail",
                    starTime,
                    DateTime.Now,
                    "_AdditionalInfo"
                    ));

                XmlSerializer xs = new XmlSerializer(typeof(TestResultCollection));
                string measurementFile = Path.Combine(SpecialFolders.Measurements, "Results.xml");
                using (Stream s = File.Create(measurementFile))
                    xs.Serialize(s, TestResults);

                const string resultsFolder = @"C:\TestResults";
                if (!Directory.Exists(resultsFolder))
                    Directory.CreateDirectory(resultsFolder);
                string resultsFile = "Results_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".xml";
                string resultsPath = Path.Combine(resultsFolder, resultsFile);
                using (Stream s2 = File.Create(resultsPath))
                    xs.Serialize(s2, TestResults);

                Trace.WriteLine("Test sequence completed.");

                Process.Start(resultsPath);
            }
            catch (OperationCanceledException)
            {
                Trace.WriteLine("Test sequence was cancelled by user.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
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
            {
                _cts.Dispose();
                FactoryEvents.Connected -= FactoryEvents_Connected;
            }
        }
    }
}
