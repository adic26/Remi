using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using TsdLib.Configuration;
using TsdLib.Instrument;
using TsdLib.Measurements;
using TsdLib.TestSystem.Controller;

namespace TsdLib.TestSystem.TestSequence
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
        public SynchronizationContext UIContext { get; set; }

        protected void AddInformation(params TestInfo[] info)
        {
            SendOrPostCallback addInfo = state => { foreach (TestInfo i in info) _testInfo.Add(i); };
            if (UIContext != null)
                UIContext.Post(addInfo, null);
            else
                addInfo.Invoke(null);
        }
        protected void AddMeasurement(params MeasurementBase[] meas)
        {
            SendOrPostCallback addMeasurements = state => { foreach (MeasurementBase m in meas) _measurements.Add(m); };
            if (UIContext != null)
                UIContext.Post(addMeasurements, null);
            else
                addMeasurements.Invoke(null);
        }
        protected void AddData(params object[] data)
        {
            SendOrPostCallback addData = state => { foreach (object d in data) _data.Add(d); };
            if (UIContext != null)
                UIContext.Post(addData, null);
            else
                addData.Invoke(null);
        }

        private readonly List<IInstrument> _instruments;
        private readonly CancellationTokenSource _cts;

        /// <summary>
        /// Gets a CancellationToken that can be used to cancel a test sequence in progress.
        /// </summary>
        protected CancellationToken Token { get { return _cts.Token; } }

        /// <summary>
        /// Adds configuration information to the <see cref="_testInfo"/> collection.
        /// Override to perform any initialization or connection setup befoer the test begins, but make sure to call base.ExecutePreTest in the overriding method.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfigs">One or more test config instances containing test-specific configuration.</param>
        protected virtual void ExecutePreTest(TStationConfig stationConfig, TProductConfig productConfig, params TTestConfig[] testConfigs)
        {
            AddInformation(new TestInfo("Station Configuration", stationConfig.Name));
            AddInformation(new TestInfo("Product Configuration", productConfig.Name));
            foreach (TTestConfig testConfig in testConfigs)
                AddInformation(new TestInfo("Test Configuration", testConfig.Name));
            AddInformation(new TestInfo("Sequence", GetType().Name));
        }
        /// <summary>
        /// Calls Dispose() on all instruments that were obtained using the static Connect method.
        /// Override to perform any custom teardown or disconnection after the test is complete, but make sure to call base.ExecutePostTest in the overriding method.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfigs">One or more test config instances containing test-specific configuration.</param>
        protected virtual void ExecutePostTest(TStationConfig stationConfig, TProductConfig productConfig, params TTestConfig[] testConfigs)
        {
            foreach (IInstrument instrument in _instruments)
                instrument.Dispose();
        }
        /// <summary>
        /// Client application overrides this method to define test steps.
        /// </summary>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfigs">One or more test config instances containing test-specific configuration.</param>
        protected abstract void ExecuteTest(TStationConfig stationConfig, TProductConfig productConfig, params TTestConfig[] testConfigs);

        /// <summary>
        /// Gets the collection of information captured during the test sequence.
        /// </summary>
        public readonly BindingList<TestInfo> _testInfo;

        public ReadOnlyObservableCollection<TestInfo> TestInfo { get; private set; }

        /// <summary>
        /// Gets the collection of measurements captured during the test sequence.
        /// </summary>
        public readonly BindingList<MeasurementBase> _measurements;

        public ReadOnlyObservableCollection<MeasurementBase> Measurements { get; private set; }

        /// <summary>
        /// Gets the collection of general data captured during the test sequence.
        /// </summary>
        public readonly BindingList<object> _data;
        public ReadOnlyObservableCollection<object> Data { get; private set; }


        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send information events across AppDomain boundaries.
        /// </summary>
        public EventProxy<TestInfo> InfoEventProxy { get; set; }

        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send measurement events across AppDomain boundaries.
        /// </summary>
        public EventProxy<MeasurementBase> MeasurementEventProxy { get; set; }

        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send data events across AppDomain boundaries.
        /// </summary>
        public EventProxy<object> DataEventProxy { get; set; }

        /// <summary>
        /// Initializes the TestSequenceBase object.
        /// </summary>
        protected TestSequenceBase()
        {
            _cts = new CancellationTokenSource();

            _instruments = new List<IInstrument>();

            _testInfo = new BindingList<TestInfo>();
            TestInfo = new ReadOnlyObservableCollection<TestInfo>(new ObservableCollection<TestInfo>(_testInfo));
            //_information.ListChanged += (sender, e) =>
            //{
            //    IBindingList list = sender as IBindingList;
            //    if (list != null && InfoEventProxy != null)
            //        InfoEventProxy.FireEvent(this, (TestInfo) list[e.NewIndex]);
            //};
            
            _measurements = new BindingList<MeasurementBase>();
            Measurements = new ReadOnlyObservableCollection<MeasurementBase>(new ObservableCollection<MeasurementBase>(_measurements));
            //_measurements.ListChanged += (sender, e) =>
            //{
            //    IBindingList list = sender as IBindingList;
            //    if (list != null && MeasurementEventProxy != null)
            //        MeasurementEventProxy.FireEvent(this, (MeasurementBase)list[e.NewIndex]);
            //};

            _data = new BindingList<object>();
            Data = new ReadOnlyObservableCollection<object>(new ObservableCollection<object>(_data));
            //_data.ListChanged += (sender, e) =>
            //{
            //    IBindingList list = sender as IBindingList;
            //    if (list != null && DataEventProxy != null)
            //        DataEventProxy.FireEvent(this, list[e.NewIndex]);
            //};

            InstrumentEvents.Connected += FactoryEvents_Connected;
        }

        private void FactoryEvents_Connected(object sender, IInstrument e)
        {
            _instruments.Add(e);
            string instrumentType = e.GetType().Name;
            _testInfo.Add(new TestInfo(instrumentType + " Description", e.Description));
            _testInfo.Add(new TestInfo(instrumentType + " Model Number", e.ModelNumber));
            _testInfo.Add(new TestInfo(instrumentType + " Serial Number", e.SerialNumber));
            _testInfo.Add(new TestInfo(instrumentType + " Firmware Version", e.FirmwareVersion));
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
        /// <returns>A <see cref="TestResultCollection"/> containing the test results.</returns>
        public ITestResults ExecuteSequence(TStationConfig stationConfig, TProductConfig productConfig, TTestConfig testConfig, TestDetails testDetails)
        {
            DateTime startTime = DateTime.Now;
            Trace.WriteLine("Starting pre-test at " + startTime);

            ExecutePreTest(stationConfig, productConfig, testConfig);

            Trace.WriteLine("Starting main-test at ." + DateTime.Now);

            ExecuteTest(stationConfig, productConfig, testConfig);

            Trace.WriteLine("Starting post-test at ." + DateTime.Now);

            ExecutePostTest(stationConfig, productConfig, testConfig);

            DateTime endTime = DateTime.Now;

            Trace.WriteLine("Completed test sequence at " + endTime);

            bool overallPass = _measurements.Any() && _measurements.All(m => m.Result == MeasurementResult.Pass);

            ITestResults testResults = MeasurementsFactory.CreateTestResults(testDetails, _measurements, overallPass ? "Pass" : "Fail", startTime, endTime, _testInfo);

            return testResults;
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
                InstrumentEvents.Connected -= FactoryEvents_Connected;
            }
        }

        /// <summary>
        /// Returns null to ensure that the remote object's lifetime is as long as the hosting AppDomain.
        /// </summary>
        /// <returns>Null, which corresponds to an unlimited lease time.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
