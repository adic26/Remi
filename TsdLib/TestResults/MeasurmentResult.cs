using System;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Describes possible measurement outcomes.
    /// </summary>
    [Flags]
    public enum MeasurementResult
    {
        /// <summary>
        /// The measurement has not yet been performed.
        /// </summary>
        Undefined = 0,
        /// <summary>
        /// The measurement is within the specified range.
        /// </summary>
        Pass = 1,        /// <summary>
        /// The measurement is outside the specified range.
        /// </summary>
        Fail = 2,
        /// <summary>
        /// The measurement is below the specified range.
        /// </summary>
        Fail_BelowLimit = 6,
        /// <summary>
        /// The measurement is above the specified range.
        /// </summary>
        Fail_AboveLimit = 10,

    }
}