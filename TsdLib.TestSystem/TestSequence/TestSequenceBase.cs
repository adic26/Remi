using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using TsdLib.Configuration.Management;
using TsdLib.Instrument;
using TsdLib.Measurements;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Contains functionality to connect a test sequence to the system controller
    /// </summary>
    public abstract class TestSequenceBase : MarshalByRefObject, ITestSequence, IDisposable
    {
        private bool _runningInRemoteDomain;

        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public Exception Error { get; private set; }

        public CancellationToken Token
        {
            get { return _tokenSource.Token; }
        }

        /// <summary>
        /// Gets a collection of instruments currently controlled used by the test sequence.
        /// </summary>
        public IInstrumentCollection Instruments { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="ConfigManagerProvider"/> that can be used to access configuration data from inside the test sequence.
        /// </summary>
        public ConfigManagerProvider Config { get; set; }

        private readonly List<IMeasurement> _measurements = new List<IMeasurement>();
        /// <summary>
        /// Gets the collection of measurements captured during the test sequence.
        /// </summary>
        public IEnumerable<IMeasurement> Measurements
        {
            get { return _measurements; }
        }
        private readonly List<ITestInfo> _testInfo = new List<ITestInfo>();
        /// <summary>
        /// Gets the collection of information captured during the test sequence.
        /// </summary>
        public IEnumerable<ITestInfo> TestInfo
        {
            get { return _testInfo; }
        }
        /// <summary>
        /// Initializes the TestSequenceBase object.
        /// </summary>
        protected TestSequenceBase()
        {
            Trace.AutoFlush = true;

            Instruments = new TestSequenceInstrumentCollection();
            Instruments.InstrumentConnected += Instruments_InstrumentConnected;
        }

        /// <summary>
        /// Adds the specified TraceListener to the Trace Listeners collection. Useful if running the test sequence from a separate application domain.
        /// </summary>
        /// <param name="listener">TraceListener to add to the Trace Listeners collection.</param>
        public void AddTraceListener(TraceListener listener)
        {
            Trace.Listeners.Add(listener);
        }

        public event EventHandler<DataContainer> DataAdded;
        /// <summary>
        /// Send data to the application controller. NOTE: The object must either be decorated with the <see cref="SerializableAttribute"/> or be derived from <see cref="MarshalByRefObject"/>
        /// </summary>
        /// <param name="data">Object to send.</param>
        protected void SendData(object data)
        {
            SerializableAttribute serializableAttributeCheck = data.GetType().GetCustomAttribute<SerializableAttribute>();
            MarshalByRefObject transferrableData = data as MarshalByRefObject;

            if (serializableAttributeCheck == null && transferrableData == null)
            {
                Trace.WriteLine("WARNING: The data type {0} must either be (1) a value type, (2) marked with the System.SerializableAttribute or (3) derived from System.MarshalByRefObject in order to be passed across Application Domain boundaries.");
                if (_runningInRemoteDomain)
                    return;
            }

            EventHandler<DataContainer> handler = DataAdded;
            if (handler != null)
                handler(this, new DataContainer(data));
        }

        public event EventHandler<ITestInfo> TestInfoAdded; 
        /// <summary>
        /// Add a new <see cref="ITestInfo"/> to the collection of test information.
        /// </summary>
        /// <param name="testInfo">test information to add.</param>
        protected void AddTestInfo(ITestInfo testInfo)
        {
            _testInfo.Add(testInfo);
            Trace.WriteLine("Test info: " + testInfo);

            EventHandler<ITestInfo> handler = TestInfoAdded;
            if (handler != null)
                handler(this, testInfo);
        }

        public event EventHandler<IMeasurement> MeasurementAdded; 
        /// <summary>
        /// Add a new <see cref="IMeasurement"/> to the collection of test measurements.
        /// </summary>
        /// <param name="measurement">Measurement information to add.</param>
        protected void AddMeasurement(IMeasurement measurement)
        {
            _measurements.Add(measurement);
            Trace.WriteLine("Measurement: " + measurement);

            EventHandler<IMeasurement> handler = MeasurementAdded;
            if (handler != null)
                handler(this, measurement);
        }

        public event EventHandler<Tuple<int, int>> ProgressUpdated; 
        /// <summary>
        /// Update the application controller of the current test sequence progress.
        /// </summary>
        /// <param name="currentStep">The current step in the test sequence.</param>
        /// <param name="numberOfSteps">The total number of steps in the test sequence.</param>
        protected void UpdateProgress(int currentStep, int numberOfSteps)
        {
            Trace.WriteLine("Progress: " + currentStep + "/" + numberOfSteps);
            EventHandler<Tuple<int, int>> handler = ProgressUpdated;
            if (handler != null)
                handler(this, new Tuple<int, int>(currentStep, numberOfSteps));
        }

        /// <summary>
        /// Event handler invoked when an instrument is connected to the test sequence.
        /// Adds instrument information to the test information.
        /// Override to modify the behaviour.
        /// </summary>
        /// <param name="sender">The instrument factory responsible for connecting to the instrument.</param>
        /// <param name="instrument">The new instrument.</param>
        protected virtual void Instruments_InstrumentConnected(object sender, IInstrument instrument)
        {
            string instrumentType = instrument.GetType().Name;
            _testInfo.Add(new TestInfo(instrumentType + " Description", instrument.Description));
            _testInfo.Add(new TestInfo(instrumentType + " " + instrument.ModelNumberDescriptor, instrument.ModelNumber));
            _testInfo.Add(new TestInfo(instrumentType + " " + instrument.SerialNumberDescriptor, instrument.SerialNumber));
            _testInfo.Add(new TestInfo(instrumentType + " " + instrument.FirmwareVersionDescriptor, instrument.FirmwareVersion));
        }

        public virtual void Abort()
        {
            _tokenSource.Cancel();
        }

        public virtual void Abort(Exception ex)
        {
            Error = ex;
            _tokenSource.Cancel();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the <see cref="IInstrumentCollection"/>
        /// </summary>
        /// <param name="disposing">True to dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Instruments != null)
                    Instruments.Dispose();
            }
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            _runningInRemoteDomain = true;
            return null;
        }
    }
}
