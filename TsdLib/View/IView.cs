using System;
using System.Collections;
using System.Diagnostics;
using TsdLib.TestResults;

namespace TsdLib.View
{
    /// <summary>
    /// Interface used to connect UI implementations to the TsdLib framework.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets a TraceListener used to write trace and debug information to the user interface.
        /// </summary>
        TraceListener Listener { get; }

        /// <summary>
        /// Event fired when requesting to modify the Station Config.
        /// </summary>
        event EventHandler EditStationConfig;
        /// <summary>
        /// Event fired when requesting to modify the Product Config.
        /// </summary>
        event EventHandler EditProductConfig;
        /// <summary>
        /// Event fired when requesting to modify the Test Config.
        /// </summary>
        event EventHandler EditTestConfig;
        /// <summary>
        /// Event fired when requesting to modify the Sequence Config.
        /// </summary>
        event EventHandler EditSequenceConfig;

        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        event EventHandler<TestSequenceEventArgs> ExecuteTestSequence;
        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        event EventHandler AbortTestSequence;

        /// <summary>
        /// Set the appearance and behaviour of IU controls, based on the current status of the system.
        /// </summary>
        /// <param name="state">State to set.</param>
        void SetState(State state);
        /// <summary>
        /// Add a new measurement to the UI display.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        void AddMeasurement(Measurement measurement);

        /// <summary>
        /// Gets or sets the text displayed in the title section of the UI.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Sets the list of available Station Config instances.
        /// </summary>
        IList StationConfigList { set; }
        /// <summary>
        /// Sets the list of available Product Config instances.
        /// </summary>
        IList ProductConfigList { set; }
        /// <summary>
        /// Sets the list of available Test Config instances.
        /// </summary>
        IList TestConfigList { set; }
        /// <summary>
        /// Sets the list of available Sequence Config instances.
        /// </summary>
        IList SequenceConfigList { set; }
    }

    /// <summary>
    /// Provides config data to pass to Test Sequence upon execution.
    /// </summary>
    public class TestSequenceEventArgs : EventArgs
    {
        /// <summary>
        /// Station Config object to pass to the Test Sequence.
        /// </summary>
        public object StationConfig { get; private set; }
        /// <summary>
        /// Product Config object to pass to the Test Sequence.
        /// </summary>
        public object ProductConfig { get; private set; }
        /// <summary>
        /// Test Config object to pass to the Test Sequence.
        /// </summary>
        public object TestConfig { get; private set; }
        /// <summary>
        /// Sequence Config object to pass to the Test Sequence.
        /// </summary>
        public object SequenceConfig { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TestSequenceEventArgs class.
        /// </summary>
        /// <param name="stationConfig">Station Config object to pass to the Test Sequence.</param>
        /// <param name="productConfig">Product Config object to pass to the Test Sequence.</param>
        /// <param name="testConfig">Test Config object to pass to the Test Sequence.</param>
        /// <param name="sequenceConfig">Sequence Config object to pass to the Test Sequence.</param>
        public TestSequenceEventArgs(object stationConfig, object productConfig, object testConfig, object sequenceConfig)
        {
            StationConfig = stationConfig;
            ProductConfig = productConfig;
            TestConfig = testConfig;
            SequenceConfig = sequenceConfig;
        }
    }
}
