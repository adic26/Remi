using System;

namespace TsdLib.TestResults
{
    /// <summary>
    /// A general container for passing data from the test sequence to the controller or UI.
    /// </summary>
    [Serializable]
    public class Data
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Initialize a new Data container with the specifed value.
        /// </summary>
        /// <param name="value">Data to encapsulate.</param>
        public Data(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets a string representation of the contained data by calling Value.ToString().
        /// </summary>
        /// <returns>A string representation of the contained data.</returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
