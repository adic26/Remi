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
        void AddMeasurement(IMeasurement measurement);

        /// <summary>
        /// Clears all measurements from the UI display.
        /// </summary>
        void ClearMeasurements();

        /// <summary>
        /// True to display or false to hide the upper/lower limit and result display.
        /// </summary>
        bool DisplayLimitsAndResult { get; set; }
    }
}