using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace TsdLib
{
    /// <summary>
    /// Describes possible measurement outcomes.
    /// </summary>
    [Flags]
    public enum MeasurementResult
    {
        /// <summary>
        /// The measurement is within the specified range.
        /// </summary>
        Pass = 0,
        /// <summary>
        /// The measurement is below the specified range.
        /// </summary>
        Fail_Low = 1,
        /// <summary>
        /// The measurement is above the specified range.
        /// </summary>
        Fail_High = 2,
        /// <summary>
        /// The measurement is outside the specified range.
        /// </summary>
        Fail = Fail_Low | Fail_High
    }

    /// <summary>
    /// A weakly-typed Measurement object to serve as a base for generic-typed measurements. Allows adding different types of measurements to the same collection.
    /// </summary>
    public abstract class Measurement
    {
        /// <summary>
        /// Name to describe the measurement.
        /// </summary>
        public readonly string Name;
        /// <summary>
        /// Magnitude of the measurement.
        /// </summary>
        public readonly object MeasuredVal;
        /// <summary>
        /// Minimum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public readonly object LowerLim;
        /// <summary>
        /// Maximum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public readonly object UpperLim;
        /// <summary>
        /// Unit of measure.
        /// </summary>
        public readonly string Units;
        /// <summary>
        /// A collection of MeasurementParameter objects describing the measurement conditions.
        /// </summary>
        public readonly MeasurementParameterCollection Parameters;

        /// <summary>
        /// Timestamp of when the measurement was captured.
        /// </summary>
        public readonly DateTime Timestamp;

        /// <summary>
        /// Gets the pass/fail result by comparing the MeasuredVal to the LowerLim and UpperLim
        /// </summary>
        public abstract MeasurementResult Result { get; }
        /// <summary>
        /// Data type for MeasuredVal, LowerLim and UpperLim. Allows adding measurements of different types into a collection, while preserving type safety.
        /// </summary>
        public abstract Type MeasuremenType { get; }

        /// <summary>
        /// Initialize a new Measurement object.
        /// </summary>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="parameters">A collection of MeasurementParameter objects describing the measurement conditions.</param>
        protected Measurement(string name, object measuredValue, string units, object lowerLimit, object upperLimit, MeasurementParameterCollection parameters)
        {
            Name = name;
            MeasuredVal = measuredValue;
            Units = units;
            LowerLim = lowerLimit;
            UpperLim = upperLimit;
            Parameters = parameters;
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Initialize a new Measurement object.
        /// </summary>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="parameters">Zero or more MeasurementParameter objects describing the measurement conditions.</param>
        protected Measurement(string name, object measuredValue, string units, object lowerLimit, object upperLimit, params MeasurementParameter[] parameters)
            : this(name, measuredValue, units, lowerLimit, upperLimit, new MeasurementParameterCollection(parameters)) { }

        /// <summary>
        /// Returns the Measurement object formatted into a string with comma separator.
        /// </summary>
        /// <returns>A single-line string representing the Measurement object.</returns>
        public override string ToString()
        {
            return ToString(",");
        }

        /// <summary>
        /// Returns the Measurement object formatted into a string with a specified separator.
        /// </summary>
        /// <param name="separator">Delimiter string to insert between each field of the Measurement object.</param>
        /// <returns>A single-line string representing the Measurement object.</returns>
        public string ToString(string separator)
        {
            string parametersToString = string.Join(separator, Parameters.Select(kvp => kvp.Key + " = " + kvp.Value));
            return string.Join(separator, Name, MeasuredVal, Units, LowerLim, UpperLim, Result, parametersToString).TrimEnd(',');
        }
    }

    /// <summary>
    /// Represents a measurement acquired durign a test sequence.
    /// </summary>
    /// <typeparam name="T">Data type of the measured value, which will also apply to the lower and upper limits.</typeparam>
    public class Measurement<T> : Measurement
        where T : IComparable<T>
    {
        /// <summary>
        /// Magnitude of the measurement.
        /// </summary>
        public readonly T MeasuredValue;
        /// <summary>
        /// Minimum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public readonly T LowerLimit;
        /// <summary>
        /// Maximum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public readonly T UpperLimit;

        private readonly MeasurementResult _result;
        /// <summary>
        /// Gets the pass/fail status of the measurement.
        /// </summary>
        public override MeasurementResult Result { get { return _result; } }

        /// <summary>
        /// Gets the data type of the MeasuredValue, LowerLimit and UpperLimit. Can be used to cast into strongly-types objects.
        /// </summary>
        public override Type MeasuremenType { get { return typeof(T); } }

        /// <summary>
        /// Initialize a new Measurement object.
        /// </summary>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="parameters">A collection of MeasurementParameter objects describing the measurement conditions.</param>
        public Measurement(string name, T measuredValue, string units, T lowerLimit, T upperLimit, MeasurementParameterCollection parameters)
            : base(name, measuredValue, units, lowerLimit, upperLimit, parameters)
        {

            MeasuredValue = measuredValue;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;

            if (measuredValue.CompareTo(lowerLimit) < 0)
                _result = MeasurementResult.Fail_Low;
            else if (measuredValue.CompareTo(upperLimit) > 0)
                _result = MeasurementResult.Fail_High;
            else
                _result = MeasurementResult.Pass;
        }

        /// <summary>
        /// Initialize a new Measurement object.
        /// </summary>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="parameters">Zero or more MeasurementParameter objects describing the measurement conditions.</param>
        public Measurement(string name, T measuredValue, string units, T lowerLimit, T upperLimit, params MeasurementParameter[] parameters)
            : this(name, measuredValue, units, lowerLimit, upperLimit, new MeasurementParameterCollection(parameters))
        {

        }
    }

    /// <summary>
    /// Represents a collection of Measurement objects.
    /// </summary>
    public class MeasurementCollection : BindingList<Measurement>
    {
        /// <summary>
        /// Initialize a new Measurement object and add it to the collection.
        /// </summary>
        /// <typeparam name="T">Data type of the measured value, which will also apply to the lower and upper limits.</typeparam>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="parameters">A collection of MeasurementParameter objects describing the measurement conditions.</param>
        public void AddMeasurement<T>(string name, T measuredValue, string units, T lowerLimit, T upperLimit, MeasurementParameterCollection parameters)
            where T : IComparable<T>
        {
            Add(new Measurement<T>(name, measuredValue, units, lowerLimit, upperLimit, parameters));
        }

        /// <summary>
        /// Initialize a new Measurement object and add it to the collection.
        /// </summary>
        /// <typeparam name="T">Data type of the measured value, which will also apply to the lower and upper limits.</typeparam>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="parameters">Zero or more MeasurementParameter objects describing the measurement conditions.</param>
        public void AddMeasurement<T>(string name, T measuredValue, string units, T lowerLimit, T upperLimit, params MeasurementParameter[] parameters)
            where T : IComparable<T>
        {
            AddMeasurement(name, measuredValue, units, lowerLimit, upperLimit, new MeasurementParameterCollection(parameters));
        }

        /// <summary>
        /// Returns all of the Measurement objects, separated by a NewLine. Each Measurement object is delimited with a comma.
        /// </summary>
        /// <returns>A multi-line string representation of all Measurement objects in the collection.</returns>
        public override string ToString()
        {
            return ToString(Environment.NewLine, ",");
        }

        /// <summary>
        /// Returns all of the Measurement objects, separated by the specified row and column delimiters.
        /// </summary>
        /// <param name="rowSeparator">Delimiter string to insert between each field of the Measurement object.</param>
        /// <param name="columnSeparator">Delimiter string to insert between each Measurement object.</param>
        /// <returns></returns>
        public string ToString(string rowSeparator, string columnSeparator)
        {
            return string.Join(rowSeparator, this.Select(meas => meas.ToString(columnSeparator)));
        }
    }

    /// <summary>
    /// A test condition or other information used to describe test conditions.
    /// </summary>
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
    }

    /// <summary>
    /// Represents a collection of MeasurementParameter objects.
    /// </summary>
    [Serializable]
    public class MeasurementParameterCollection : Dictionary<string, object>
    {
        /// <summary>
        /// Initialize a new MeasurementParameterCollection using a sequence of existing MeasurementParameter objects.
        /// </summary>
        /// <param name="parameters">A sequence of existing MeasurementParameter objects.</param>
        public MeasurementParameterCollection(IEnumerable<MeasurementParameter> parameters)
            : base(parameters.ToDictionary(p => p.Name, p => p.Value))
        {
            
        }

        /// <summary>
        /// Initialize a new MeasurementParameterCollection with serialized data.
        /// </summary>
        /// <param name="info">A SerializationInfo object containing the information required to serialize or deserialize the MeasurementParameterCollection.</param>
        /// <param name="context">A StreamingContext structure containing the source and destination of the serialized stream associated with the MeasurementParameterCollection.</param>
        protected MeasurementParameterCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}