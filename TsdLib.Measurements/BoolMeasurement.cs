using System;

namespace TsdLib.Measurements
{
    /// <summary>
    /// A specialized measurement implementation for boolean measurements.
    /// </summary>
    [Serializable]
    public class BoolMeasurement : Measurement<bool>
    {
        /// <summary>
        /// Gets the result of the measurement by comparing the measured value to the expected value.
        /// </summary>
        public override MeasurementResult Result
        {
            get { return MeasuredValue == LowerLimit == UpperLimit ? MeasurementResult.Pass : MeasurementResult.Fail; }
        }

        /// <summary>
        /// Initialize a new boolean measurement object.
        /// </summary>
        /// <param name="measurementName">Name to describe the measurement.</param>
        /// <param name="measuredValue">The true or false measurement.</param>
        /// <param name="expectedValue">The expected measurement value.</param>
        /// <param name="files">OPTIONAL: An array of absolute paths of files to attach.</param>
        /// <param name="comments">OPTIONAL: Any comments to include additional information.</param>
        /// <param name="description">OPTIONAL: A detailed description of the measurement.</param>
        /// <param name="parameters">OPTIONAL: A collection of MeasurementParameter objects describing the measurement conditions.</param>
        public BoolMeasurement(string measurementName, bool measuredValue, bool expectedValue, string[] files = null, string comments = null, string description = null, params MeasurementParameter[] parameters)
            : base(measurementName, measuredValue, "Boolean", expectedValue, expectedValue, files, comments, description, parameters) { }
    }
}