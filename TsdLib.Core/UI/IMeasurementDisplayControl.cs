using TsdLib.Measurements;

namespace TsdLib.UI
{
    /// <summary>
    /// Defines methods required to display measurements on the UI.
    /// </summary>
    public interface IMeasurementDisplayControl : ITsdLibControl
    {
        /// <summary>
        /// Add a measurement to the UI display.
        /// </summary>
        /// <param name="measurement">Measurement to add.</param>
        void AddMeasurement(MeasurementBase measurement);
    }
}