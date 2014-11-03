using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using TsdLib.Configuration;
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
    public abstract class TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> : MarshalByRefObject, IDisposable
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
        where TTestConfig : TestConfigCommon
    {
        /// <summary>
        /// Client application overrides this method to define test steps.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfig">Test config instance containing test-specific configuration.</param>
        protected abstract void Execute(TStationConfig stationConfig, TProductConfig productConfig, TTestConfig testConfig);

        private readonly CancellationTokenSource _cts;
        /// <summary>
        /// Gets a CancellationToken that can be used to cancel a test sequence in progress.
        /// </summary>
        protected CancellationToken Token { get { return _cts.Token; } }

        private readonly List<IInstrument> _instruments;

        /// <summary>
        /// Gets the collection of information captured during the test sequence.
        /// </summary>
        public BindingList<TestInfo> Information { get; protected set; }

        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send information events across AppDomain boundaries.
        /// </summary>
        public EventProxy<TestInfo> InfoEventProxy { get; set; }

        /// <summary>
        /// Gets the collection of measurements captured during the test sequence.
        /// </summary>
        public BindingList<MeasurementBase> Measurements { get; protected set; }

        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send measurement events across AppDomain boundaries.
        /// </summary>
        public EventProxy<MeasurementBase> MeasurementEventProxy { get; set; }

        /// <summary>
        /// Gets the collection of general data captured during the test sequence.
        /// </summary>
        public BindingList<Data> Data { get; protected set; }

        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send data events across AppDomain boundaries.
        /// </summary>
        public EventProxy<Data> DataEventProxy { get; set; }

        /// <summary>
        /// Initializes the TestSequenceBase object.
        /// </summary>
        protected TestSequenceBase()
        {
            _cts = new CancellationTokenSource();

            _instruments = new List<IInstrument>();

            Information = new BindingList<TestInfo>();
            Information.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list != null && InfoEventProxy != null)
                    InfoEventProxy.FireEvent((TestInfo)list[e.NewIndex]);
            };

            Measurements = new BindingList<MeasurementBase>();
            Measurements.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list != null && MeasurementEventProxy != null)
                    MeasurementEventProxy.FireEvent((MeasurementBase)list[e.NewIndex]);
            };

            Data = new BindingList<Data>();
            Data.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list != null && DataEventProxy != null)
                    DataEventProxy.FireEvent((Data)list[e.NewIndex]);
            };

            FactoryEvents.Connected += FactoryEvents_Connected;
        }

        private void FactoryEvents_Connected(object sender, ConnectedEventArgs e)
        {
            _instruments.Add(e.Instrument);
            string instrumentType = e.Instrument.GetType().Name;
            Information.Add(new TestInfo(instrumentType + " Description", e.Instrument.Description));
            Information.Add(new TestInfo(instrumentType + " Model Number", e.Instrument.ModelNumber));
            Information.Add(new TestInfo(instrumentType + " Serial Number", e.Instrument.SerialNumber));
            Information.Add(new TestInfo(instrumentType + " Firmware Version", e.Instrument.FirmwareVersion));
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
        /// Start execution of the test sequence with the specified configuration objects.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfig">Test config instance containing test-specific configuration.</param>
        /// <param name="testDetails">Details about the test request or job.</param>
        public void ExecuteSequence(TStationConfig stationConfig, TProductConfig productConfig, TTestConfig testConfig, TestDetails testDetails)
        {
            try
            {
                //Pre-test
                DateTime startTime = DateTime.Now;
                Trace.WriteLine("Executing test sequence...");

                Information.Add(new TestInfo("Station Configuration", stationConfig.Name));
                Information.Add(new TestInfo("Product Configuration", productConfig.Name));
                Information.Add(new TestInfo("Test Configuration", testConfig.Name));

                //Execute test
                Execute(stationConfig, productConfig, testConfig);

                DateTime endTime = DateTime.Now;

                //Post-test
                bool overallPass = Measurements.Any() && Measurements.All(m => m.Result == MeasurementResult.Pass);

                TestSummary summary = new TestSummary(overallPass ? "Pass" : "Fail", startTime, endTime);

                TestResultCollection testResults = new TestResultCollection(testDetails, Measurements, summary, Information);

                DirectoryInfo resultsDirectory = SpecialFolders.GetResultsFolder(testDetails.TestSystemName);

                string measurementFile = testResults.Save(resultsDirectory);

                string formattedFileName = string.Format("{0}-{1}_", testDetails.JobNumber, testDetails.UnitNumber.ToString("D3"));
                string csvResultsFile = Path.Combine(resultsDirectory.FullName, formattedFileName + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv");
                File.WriteAllText(csvResultsFile, testResults.ToString());
                Trace.WriteLine("Test sequence completed.");
                Trace.WriteLine("XML results saved to " + measurementFile);
                Trace.WriteLine("CSV results saved to " + csvResultsFile);
                //Process.Start(measurementFile);
                //Process.Start(csvResultsFile);
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
                foreach (IInstrument instrument in _instruments)
                {
                    instrument.Dispose();
                }
            }
        }
    }
}
