using System;

namespace TsdLib.TestResults
{
    /// <summary>
    /// A measurement implementation that does not use pass/fail criteria. Result will always be <see cref="MeasurementResult.Undefined"/>.
    /// </summary>
    [Serializable]
    public class MeasurementWithoutResult : MeasurementBase
    {
        public override MeasurementResult Result
        {
            get { return MeasurementResult.Undefined; }
        }

        /// <summary>
        /// Initialize a new measurement object that does not use pass/fail criteria.
        /// </summary>
        /// <param name="measurementName">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        public MeasurementWithoutResult(string measurementName, string measuredValue, string units)
            : base(measurementName, measuredValue, units, "n/a", "n/a")
        {

        }
    }
}
