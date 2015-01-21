using System;

namespace TsdLib.Measurements
{
    /// <summary>
    /// The standard measurement implementation, using strongly typed measured value and limits.
    /// </summary>
    /// <typeparam name="T">Type to use for the measured value and limits.</typeparam>
    [Serializable]
    public class Measurement<T> : MeasurementBase
        where T : IComparable<T>, IComparable
    {
        /// <summary>
        /// Gets the magnitude of the measurement.
        /// </summary>
        public new T MeasuredValue { get; protected set; }
        /// <summary>
        /// Gets the minimum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public new T LowerLimit { get; protected set; }
        /// <summary>
        /// Gets the maximum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public new T UpperLimit { get; protected set; }

        /// <summary>
        /// Gets the result of the measurement by comparing to lower and upper limits.
        /// </summary>
        public override MeasurementResult Result
        {
            get
            {
                if (MeasuredValue.CompareTo(LowerLimit) < 0)
                    return MeasurementResult.Fail_BelowLimit;
                if (MeasuredValue.CompareTo(UpperLimit) > 0)
                    return MeasurementResult.Fail_AboveLimit;

                return MeasurementResult.Pass;
            }
        }

        /// <summary>
        /// Initialize a new measurement object.
        /// </summary>
        /// <param name="measurementName">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="files">OPTIONAL: An array of absolute paths of files to attach.</param>
        /// <param name="comments">OPTIONAL: Any comments to include additional information.</param>
        /// <param name="description">OPTIONAL: A detailed description of the measurement.</param>
        /// <param name="parameters">OPTIONAL: A collection of MeasurementParameter objects describing the measurement conditions.</param>
        public Measurement(string measurementName, T measuredValue, string units, T lowerLimit, T upperLimit, string[] files = null, string comments = "", string description = "", params MeasurementParameter[] parameters)
            : base(measurementName, measuredValue.ToString(), units, lowerLimit.ToString(), upperLimit.ToString(), files, comments, description, parameters)
        {
            MeasuredValue = measuredValue;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }
    }
}
