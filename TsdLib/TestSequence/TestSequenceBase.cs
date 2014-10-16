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

            _instruments = new List<IInstrument>();
            
            TestResults = new TestResultCollection();
            TestResults.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list != null && MeasurementEventProxy != null)
                    MeasurementEventProxy.FireEvent(new MeasurementEventArgs((Measurement)list[e.NewIndex]));
            };

            FactoryEvents.Connected += FactoryEvents_Connected;
        }

        private void FactoryEvents_Connected(object sender, ConnectedEventArgs e)
        {
            _instruments.Add(e.Instrument);
            TestResults.AddMeasurement(Measurement.CreateMeasurement(e.Instrument.GetType().Name + " Description", e.Instrument.Description, "Info"));
            TestResults.AddMeasurement(Measurement.CreateMeasurement(e.Instrument.GetType().Name + " Model Number", e.Instrument.ModelNumber, "Info"));
            TestResults.AddMeasurement(Measurement.CreateMeasurement(e.Instrument.GetType().Name + " Serial Number", e.Instrument.SerialNumber, "Info"));
            TestResults.AddMeasurement(Measurement.CreateMeasurement(e.Instrument.GetType().Name + " Firmware Version", e.Instrument.FirmwareVersion, "Info"));
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
                    testConfig.TestSystemName,
                    "_JobNumber",
                    "_UnitNumber",
                    "_TestType",
                    "_TestStage",
                    "_BSN",
                    overallPass ? "Pass" : "Fail",
                    starTime,
                    DateTime.Now,
                    "_AdditionalInfo",
                    "FunctionalType"
                    ));

                string measurementFile = TestResults.Save(new DirectoryInfo(SpecialFolders.GetResultsFolder(testConfig.TestSystemName)));
                //Process.Start(measurementFile);

                TestResults.Save(new DirectoryInfo(@"C:\TestResults"));

                string csvResultsFile = Path.ChangeExtension(measurementFile, "csv");
                File.WriteAllText(csvResultsFile, TestResults.ToString(Environment.NewLine, ","));
                //Process.Start(csvResultsFile);

                Trace.WriteLine("Test sequence completed.");
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
