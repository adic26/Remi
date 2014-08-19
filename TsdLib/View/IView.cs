using System;
using System.Collections;

namespace TsdLib.View
{
    /// <summary>
    /// Interface used to connect UI implementations to the TsdLib framework.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Event fired when requesting to modify the Station Config.
        /// </summary>
        event EventHandler EditStationConfig;

        /// <summary>
        /// Event fired when requesting to modify the Product Config.
        /// </summary>
        event EventHandler EditProductConfig;

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
        /// Sets the list of available Station Config instances.
        /// </summary>
        IList StationConfigList { set; }
        /// <summary>
        /// Sets the list of available Product Config instances.
        /// </summary>
        IList ProductConfigList { set; }
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
        /// Initializes a new instance of the TestSequenceEventArgs class.
        /// </summary>
        /// <param name="stationConfig">Station Config object to pass to the Test Sequence.</param>
        /// <param name="productConfig">Product Config object to pass to the Test Sequence.</param>
        public TestSequenceEventArgs(object stationConfig, object productConfig)
        {
            StationConfig = stationConfig;
            ProductConfig = productConfig;
        }
    }
}
