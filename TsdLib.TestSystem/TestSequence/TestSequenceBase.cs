using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using TsdLib.Configuration.Common;
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
    public abstract class TestSequenceBase<TStationConfig, TProductConfig, TTestConfig> : SequenceConfigCommon, ICancellable
        where TStationConfig : StationConfigCommon
        where TProductConfig : ProductConfigCommon
        where TTestConfig : TestConfigCommon
    {
        private readonly List<IInstrument> _instruments;

        private CancellationTokenSource UserCancellationTokenSource { get; set; }
        private CancellationTokenSource ErrorCancellationTokenSource { get; set; }
        private CancellationTokenSource linked { get; set; }

        /// <summary>
        /// Returns true if the test sequence was cancelled by the user. False if it was cancelled due to internal error.
        /// </summary>
        public bool CancelledByUser
        {
            get { return UserCancellationTokenSource.Token.IsCancellationRequested; }
        }
        /// <summary>
        /// Gets the internal error responsible for test sequence cancellation.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        /// Override to perform any initialization or connection setup befoer the test begins.
        /// </summary>
        /// <param name="token">A cancellation token used to support cooperative cancellation. Should periodically call <see cref="CancellationToken.ThrowIfCancellationRequested"/>.</param>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        protected virtual void ExecutePreTest(CancellationToken token, TStationConfig stationConfig, TProductConfig productConfig)
        {
            token.ThrowIfCancellationRequested();

        }
        /// <summary>
        /// Client application overrides this method to define test steps.
        /// </summary>
        /// <param name="token">A cancellation token used to support cooperative cancellation. Should periodically call <see cref="CancellationToken.ThrowIfCancellationRequested"/>.</param>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        /// <param name="testConfig">Test config instance containing test-specific configuration.</param>
        protected abstract void ExecuteTest(CancellationToken token, TStationConfig stationConfig, TProductConfig productConfig, TTestConfig testConfig);
        /// <summary>
        /// Calls Dispose() on all instruments that were obtained using the static Connect method.
        /// Override to perform any custom teardown or disconnection after the test is complete, but make sure to call base.ExecutePostTest in the overriding method.
        /// </summary>
        /// <param name="token">A cancellation token used to support cooperative cancellation. Should periodically call <see cref="CancellationToken.ThrowIfCancellationRequested"/>.</param>
        /// <param name="stationConfig">Station config instance containing station-specific configuration.</param>
        /// <param name="productConfig">Product config instance containing product-specific configuration.</param>
        protected virtual void ExecutePostTest(CancellationToken token, TStationConfig stationConfig, TProductConfig productConfig)
        {
            foreach (IInstrument instrument in _instruments)
            {
                token.ThrowIfCancellationRequested();
                instrument.Dispose();
            }
        }

        private readonly BindingList<ITestInfo> _testInfo;
        /// <summary>
        /// Add a new <see cref="ITestInfo"/> to the collection of test information.
        /// </summary>
        /// <param name="testInfo">test information to add.</param>
        protected void AddTestInfo(ITestInfo testInfo)
        {
            _testInfo.Add(testInfo);
        }
        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send information events across AppDomain boundaries.
        /// </summary>
        internal EventProxy<ITestInfo> InfoEventProxy { get; set; }
        /// <summary>
        /// Gets the collection of information captured during the test sequence.
        /// </summary>
        public ReadOnlyCollection<ITestInfo> TestInfo
        {
            get { return new ReadOnlyCollection<ITestInfo>(_testInfo); }
        }

        private readonly BindingList<IMeasurement> _measurements;
        /// <summary>
        /// Add a new <see cref="MeasurementBase"/> to the collection of test measurements.
        /// </summary>
        /// <param name="measurement">Measurement information to add.</param>
        protected void AddMeasurement(IMeasurement measurement)
        {
            _measurements.Add(measurement);
        }
        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send measurement events across AppDomain boundaries.
        /// </summary>
        internal EventProxy<IMeasurement> MeasurementEventProxy { get; set; }
        /// <summary>
        /// Gets the collection of measurements captured during the test sequence.
        /// </summary>
        public ReadOnlyCollection<IMeasurement> Measurements
        {
            get { return new ReadOnlyCollection<IMeasurement>(_measurements); }
        }

        /// <summary>
        /// Update the application controller of the current test sequence progress.
        /// </summary>
        /// <param name="currentStep">The current step in the test sequence.</param>
        /// <param name="numberOfSteps">The total number of steps in the test sequence.</param>
        protected void UpdateProgress(int currentStep, int numberOfSteps)
        {
            ProgressEventProxy.FireEvent(this, new Tuple<int, int>(currentStep, numberOfSteps));
        }
        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send progress updates across AppDomain boundaries.
        /// </summary>
        internal EventProxy<Tuple<int, int>> ProgressEventProxy { get; set; }

        /// <summary>
        /// Send data to the application controller.
        /// </summary>
        /// <param name="data">Data that can be marshalled across AppDomain boundaries.</param>
        protected void SendData(MarshalByRefObject data)
        {
            DataEventProxy.FireEvent(this, data);
        }
        /// <summary>
        /// Send data to the application controller.
        /// </summary>
        /// <param name="data">Data that can be serialized across AppDomain boundaries.</param>
        protected void SendData(ISerializable data)
        {
            DataEventProxy.FireEvent(this, data);
        }
        /// <summary>
        /// Send data to the application controller.
        /// </summary>
        /// <param name="data">Data that can be marshalled across AppDomain boundaries as a value type.</param>
        protected void SendData<T>(T data)
            where T : struct
        {
            DataEventProxy.FireEvent(this, data);
        }
        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send general data across AppDomain boundaries.
        /// </summary>
        internal EventProxy<object> DataEventProxy { get; set; }
        
        /// <summary>
        /// Initializes the TestSequenceBase object.
        /// </summary>
        protected TestSequenceBase()
        {
            Trace.AutoFlush = true;

            UserCancellationTokenSource = new CancellationTokenSource();
            ErrorCancellationTokenSource = new CancellationTokenSource();
            linked = CancellationTokenSource.CreateLinkedTokenSource(UserCancellationTokenSource.Token, ErrorCancellationTokenSource.Token);

            _instruments = new List<IInstrument>();

            _testInfo = new BindingList<ITestInfo>();
            _testInfo.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list == null)
                    throw new TestSequenceException(this, "The TestInfo.ListChanged event was fired by an object not implementing IBindingList.");
                ITestInfo info = list[e.NewIndex] as ITestInfo;
                Trace.WriteLine(info);
                if (InfoEventProxy != null)
                    InfoEventProxy.FireEvent(this, info);
            };

            _measurements = new BindingList<IMeasurement>();
            _measurements.ListChanged += (sender, e) =>
            {
                IBindingList list = sender as IBindingList;
                if (list != null && MeasurementEventProxy != null)
                    MeasurementEventProxy.FireEvent(this, (IMeasurement)list[e.NewIndex]);
            };

            InstrumentEvents.Connected += FactoryEvents_Connected;
        }

        /// <summary>
        /// Event handler invoked when a new instrument is connected.
        /// </summary>
        /// <param name="sender">The factory class responsible for connecting to the new instrument.</param>
        /// <param name="e">The new <see cref="IInstrument"/>.</param>
        protected virtual void FactoryEvents_Connected(object sender, IInstrument e)
        {
            _instruments.Add(e);
            string instrumentType = e.GetType().Name;
            _testInfo.Add(new TestInfo(instrumentType + " Description", e.Description));
            _testInfo.Add(new TestInfo(instrumentType + " " + e.ModelNumberDescriptor, e.ModelNumber));
            _testInfo.Add(new TestInfo(instrumentType + " " + e.SerialNumberDescriptor, e.SerialNumber));
            _testInfo.Add(new TestInfo(instrumentType + " " + e.FirmwareVersionDescriptor, e.FirmwareVersion));
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
        /// <param name="testConfigs">Zero or more test config instances containing test-specific configuration.</param>
        /// <returns>A <see cref="TestResultCollection"/> containing the test results.</returns>
        public void ExecuteSequence(TStationConfig stationConfig, TProductConfig productConfig, params TTestConfig[] testConfigs)
        {
            try
            {
                //TODO: change TestInfo.Name to StationConfigCommon
                _testInfo.Add(new TestInfo(stationConfig.CommonBaseTypeName, stationConfig.Name));
                _testInfo.Add(new TestInfo(productConfig.CommonBaseTypeName, productConfig.Name));
                foreach (TTestConfig testConfig in testConfigs)
                    _testInfo.Add(new TestInfo(testConfig.CommonBaseTypeName, testConfig.Name));
                _testInfo.Add(new TestInfo(CommonBaseTypeName, GetType().Name));
                //_testInfo.Add(new TestInfo("SequenceConfigCommon", GetType().Name)); //Use this if we have trouble deriving from SequenceConfigCommon instead of MarshalByRefObject - that way we can avoid making ConfigItem derive from MarshalByRefObject

                Trace.WriteLine("Starting pre-test at " + DateTime.Now);

                ExecutePreTest(linked.Token, stationConfig, productConfig);

                int testNumber = 0;
                foreach (TTestConfig testConfig in testConfigs)
                {
                    Trace.WriteLine(string.Format("Starting {0} at {1}.", testConfig.Name, DateTime.Now));
                    ProgressEventProxy.FireEvent(this, new Tuple<int, int>(testNumber++, testConfigs.Length));
                        
                    ExecuteTest(linked.Token, stationConfig, productConfig, testConfig);
                }

                Trace.WriteLine("Starting post-test at ." + DateTime.Now);

                ExecutePostTest(linked.Token, stationConfig, productConfig);

                Trace.WriteLine("Completed test sequence at " + DateTime.Now);
                ProgressEventProxy.FireEvent(this, new Tuple<int, int>(1, 1));
            }
            catch (Exception ex)
            {
                Error = ex;
                throw;
            }
        }


        

        /// <summary>
        /// Abort the test sequence due to user cancellation or error.
        /// </summary>
        /// <param name="error">If cancelling due to error, pass the responsible exception. If cancelling to to user, pass null.</param>
        public void Abort(Exception error = null)
        {
            if (error == null)
                UserCancellationTokenSource.Cancel();
            else
            {
                Error = error;
                ErrorCancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// Dispose of any resources being used by the test sequence.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InstrumentEvents.Connected -= FactoryEvents_Connected;
                foreach (IInstrument instrument in _instruments)
                    instrument.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
