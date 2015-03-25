using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using TsdLib.Configuration.Managers;
using TsdLib.Instrument;
using TsdLib.Measurements;
using TsdLib.TestSystem.Controller;
using TsdLib.TestSystem.Observer;

namespace TsdLib.TestSystem.TestSequence
{
    /// <summary>
    /// Contains functionality to connect a test sequence to the system controller
    /// </summary>
    public abstract class TestSequenceBase : MarshalByRefObject, IDisposable, IObservable<TransientData<object>>
    {
        public AppDomain CallbackDomain { get; set; }


        private readonly HashSet<IObserver<TransientData<object>>> _observers = new HashSet<IObserver<TransientData<object>>>();
        public IDisposable Subscribe(IObserver<TransientData<object>> observer)
        {
            _observers.Add(observer);

            return new Unsubscriber<TransientData<object>>(_observers, observer);
        }

        public void SendData(object data)
        {
            foreach (IObserver<TransientData<object>> observer in _observers)
            {
                observer.OnNext(new TransientData<object>(SynchronizationContext.Current, CallbackDomain ?? AppDomain.CurrentDomain, data));
            }
        }



        /// <summary>
        /// Gets an <see cref="ICancellationManager"/> object responsible for cancelling the test sequence due to error or user abort.
        /// </summary>
        public ICancellationManager CancellationManager { get; private set; }

        /// <summary>
        /// Gets a collection of instruments currently controlled used by the test sequence.
        /// </summary>
        public IInstrumentCollection Instruments { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="ConfigManagerProvider"/> that can be used to access configuration data from inside the test sequence.
        /// </summary>
        public ConfigManagerProvider Config { get; set; }

        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send measurement events across AppDomain boundaries.
        /// </summary>
        internal EventProxy<IMeasurement> MeasurementEventProxy { get; set; }
        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send information events across AppDomain boundaries.
        /// </summary>
        internal EventProxy<ITestInfo> InfoEventProxy { get; set; }
        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send progress updates across AppDomain boundaries.
        /// </summary>
        internal EventProxy<Tuple<int, int>> ProgressEventProxy { get; set; }
        /// <summary>
        /// Gets or sets an EventProxy object that can be used to send general data across AppDomain boundaries.
        /// </summary>
        internal EventProxy<object> DataEventProxy { get; set; }

        private readonly List<IMeasurement> _measurements;
        /// <summary>
        /// Gets the collection of measurements captured during the test sequence.
        /// </summary>
        public IEnumerable<IMeasurement> Measurements
        {
            get { return _measurements; }
        }
        private readonly List<ITestInfo> _testInfo;
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

            CancellationManager = new TestSequenceCancellationManager();
            Instruments = new TestSequenceInstrumentCollection();
            Instruments.InstrumentConnected += Instruments_InstrumentConnected;

            _testInfo = new List<ITestInfo>();

            _measurements = new List<IMeasurement>();
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
        /// Add a new <see cref="ITestInfo"/> to the collection of test information.
        /// </summary>
        /// <param name="testInfo">test information to add.</param>
        protected void AddTestInfo(ITestInfo testInfo)
        {
            _testInfo.Add(testInfo);
            Trace.WriteLine(testInfo);
            if (InfoEventProxy != null)
                InfoEventProxy.FireEvent(this, testInfo);
        }
        /// <summary>
        /// Add a new <see cref="MeasurementBase"/> to the collection of test measurements.
        /// </summary>
        /// <param name="measurement">Measurement information to add.</param>
        protected void AddMeasurement(IMeasurement measurement)
        {
            _measurements.Add(measurement);
            Trace.WriteLine(measurement);
            if (MeasurementEventProxy != null)
                MeasurementEventProxy.FireEvent(this, measurement);
        }

        /// <summary>
        /// Update the application controller of the current test sequence progress.
        /// </summary>
        /// <param name="currentStep">The current step in the test sequence.</param>
        /// <param name="numberOfSteps">The total number of steps in the test sequence.</param>
        protected void UpdateProgress(int currentStep, int numberOfSteps)
        {
            if (ProgressEventProxy != null)
                ProgressEventProxy.FireEvent(this, new Tuple<int, int>(currentStep, numberOfSteps));
        }

        /// <summary>
        /// Send data to the application controller.
        /// </summary>
        /// <param name="data">Data that can be marshalled across AppDomain boundaries as a value type.</param>
        protected void SendDataByValue<T>(T data) where T : struct
        {
            if (DataEventProxy != null)
                DataEventProxy.FireEvent(this, data);
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
            if (disposing && Instruments != null)
                Instruments.Dispose();
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
