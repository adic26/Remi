using System;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Provides measurement data to pass to a measurement captured event.
    /// </summary>
    [Serializable]
    public class MeasurementEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Measurement object that generated the event.
        /// </summary>
        public MeasurementBase Measurement { get; private set; }

        /// <summary>
        /// Initialize a new instance of the MeasurementEventArgs class.
        /// </summary>
        /// <param name="measurement">Measurement to pass to the event handlers.</param>
        public MeasurementEventArgs(MeasurementBase measurement)
        {
            Measurement = measurement;
        }
    }
}