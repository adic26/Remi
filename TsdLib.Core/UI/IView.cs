using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using TsdLib.Measurements;

namespace TsdLib.UI
{
    /// <summary>
    /// Interface used to connect UI implementations to the TsdLib framework.
    /// </summary>
    public interface IView
    {
        BindingList<TestInfo> TestInfoList { set; }

        BindingList<MeasurementBase> MeasurementList { set; }

        /// <summary>
        /// Gets a TraceListener used to write trace and debug information to the user interface.
        /// </summary>
        TraceListener Listener { get; }

        /// <summary>
        /// Event fired when requesting to edit the test details.
        /// </summary>
        event EventHandler<bool> EditTestDetails;

        /// <summary>
        /// Event fired when requesting to view or modify the test system configuration.
        /// </summary>
        event EventHandler ViewEditConfiguration;

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
        /// Add user-defined data to the UI display.
        /// </summary>
        /// <param name="data">Data object to add.</param>
        void AddData(object data);

        /// <summary>
        /// Add a <see cref="TestInfo"/> to the UI display.
        /// </summary>
        /// <param name="info">TestInfo object to add.</param>
        void AddInformation(TestInfo info);

        /// <summary>
        /// Add a <see cref="MeasurementBase"/> to the UI display.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        void AddMeasurement(MeasurementBase measurement);


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
        /// True if Test Sequence results will be published to a database.
        /// </summary>
        public bool PublishResults { get; private set; }

        /// <summary>
        /// Initializes a new instance of the TestSequenceEventArgs class.
        /// </summary>
        /// <param name="stationConfig">Station Config object to pass to the Test Sequence.</param>
        /// <param name="productConfig">Product Config object to pass to the Test Sequence.</param>
        /// <param name="testConfig">Test Config object to pass to the Test Sequence.</param>
        /// <param name="sequenceConfig">Sequence Config object to pass to the Test Sequence.</param>
        /// <param name="publishResults">True to publish Test Sequence results to a database.</param>
        public TestSequenceEventArgs(object stationConfig, object productConfig, object testConfig, object sequenceConfig, bool publishResults)
        {
            StationConfig = stationConfig;
            ProductConfig = productConfig;
            TestConfig = testConfig;
            SequenceConfig = sequenceConfig;
            PublishResults = publishResults;
        }
    }
}
