using System;

namespace TsdLib.TestResults
{
    /// <summary>
    /// A measurement implementation that does not use pass/fail criteria. Result will always be <see cref="MeasurementResult.Undefined"/>.
    /// </summary>
    [Serializable]
    public class MeasurementWithoutResult : MeasurementBase
    {
        /// <summary>
        /// Always returns <see cref="MeasurementResult.Undefined"/>.
        /// </summary>
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
        /// <param name="files">OPTIONAL: An array of absolute paths of files to attach.</param>
        /// <param name="comments">OPTIONAL: Any comments to include additional information.</param>
        /// <param name="description">OPTIONAL: A detailed description of the measurement.</param>
        /// <param name="parameters">OPTIONAL: A collection of MeasurementParameter objects describing the measurement conditions.</param>
        public MeasurementWithoutResult(string measurementName, object measuredValue, string units, string[] files = null, string comments = "", string description = "", params MeasurementParameter[] parameters)
            : base(measurementName, measuredValue.ToString(), units, "n/a", "n/a", files, comments, description, parameters)
        {

        }
    }
}
