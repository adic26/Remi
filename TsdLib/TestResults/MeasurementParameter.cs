using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib.TestResults
{
    /// <summary>
    /// A test condition or other information used to describe test conditions.
    /// </summary>
    [Serializable]
    public class MeasurementParameter
    {
        /// <summary>
        /// Name of the parameter.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Value of the parameter.
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// Initialize a new MeasurementParameter object with the specified name and value.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        public MeasurementParameter(string name, object val)
        {
            Name = name;
            Value = val;
        }

        /// <summary>
        /// Returns a string representation of the MeasurementParameter.
        /// </summary>
        /// <returns>A string containing the name and value, separated by an equals sign.</returns>
        public override string ToString()
        {
            return string.Concat(Name, "=", Value);
        }
    }

    /// <summary>
    /// Represents a collection of MeasurementParameter objects.
    /// </summary>
    [Serializable]
    public class MeasurementParameterCollection : List<MeasurementParameter>
    {
        /// <summary>
        /// Initialize an empty MeasurementParameterCollection.
        /// </summary>
        public MeasurementParameterCollection() { }

        /// <summary>
        /// Initialize a new MeasurementParameterCollection using a sequence of existing MeasurementParameter objects.
        /// </summary>
        /// <param name="parameters">A sequence of existing MeasurementParameter objects.</param>
        public MeasurementParameterCollection(IEnumerable<MeasurementParameter> parameters)
            : base(parameters) { }

        /// <summary>
        /// Gets the value of a MeasurementParameter by name.
        /// </summary>
        /// <param name="measurementName">Name of the MeasurementParameter.</param>
        /// <returns>Value of the MeasurementParameter with the specified name.</returns>
        public object GetMeasurementValue(string measurementName)
        {
            MeasurementParameter mp = this.FirstOrDefault(p => p.Name == measurementName);
            if (mp == null)
                throw new MeasurementParameterException(measurementName);
            return mp.Value;
        }

        /// <summary>
        /// Returns a string representation of the MeasurementParameters in the collection, with MeasurementParameters separated by a comma.
        /// </summary>
        /// <returns>A string representation of the MeasurementParameters in the collection.</returns>
        public override string ToString()
        {
            return ToString(",");
        }

        /// <summary>
        /// Returns a string representation of the MeasurementParameters in the collection, separated by the specified delimiter.
        /// </summary>
        /// <param name="separator">Delimiter string to insert between each MeasurementParameter object.</param>
        /// <returns>A string representation of the MeasurementParameters in the collection.</returns>
        public string ToString(string separator)
        {
            return string.Join(separator, this.Select(p => p.ToString()));
        }
    }
    
}