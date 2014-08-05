using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace TsdLib
{
    [Flags]
    public enum MeasurementResult
    {
        Pass = 0,
        Fail_Low = 1,
        Fail_High = 2,
        Fail = Fail_Low | Fail_High
    }

    public abstract class Measurement
    {
        public readonly string Name;
        public readonly object MeasuredVal;
        public readonly object LowerLim;
        public readonly object UpperLim;
        public readonly string Units;
        public readonly MeasurementParameterCollection Parameters;

        public readonly DateTime Timestamp;

        public abstract MeasurementResult Result { get; }
        public abstract Type MeasuremenType { get; }

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

        protected Measurement(string name, object measuredValue, string units, object lowerLimit, object upperLimit, params MeasurementParameter[] parameters)
            : this(name, measuredValue, units, lowerLimit, upperLimit, new MeasurementParameterCollection(parameters)) { }

        public override string ToString()
        {
            return ToString(",");
        }

        public string ToString(string separator)
        {
            string parametersToString = string.Join(separator, Parameters.Select(kvp => kvp.Key + " = " + kvp.Value));
            return string.Join(separator, Name, MeasuredVal, Units, LowerLim, UpperLim, Result, parametersToString).TrimEnd(',');
        }
    }

    public class Measurement<T> : Measurement
        where T : IComparable<T>
    {
        public readonly T MeasuredValue;
        public readonly T LowerLimit;
        public readonly T UpperLimit;

        private readonly MeasurementResult _result;
        public override MeasurementResult Result { get { return _result; } }

        public override Type MeasuremenType { get { return typeof(T); } }

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

        public Measurement(string name, T measuredValue, string units, T lowerLimit, T upperLimit, params MeasurementParameter[] parameters)
            : this(name, measuredValue, units, lowerLimit, upperLimit, new MeasurementParameterCollection(parameters))
        {

        }
    }

    public class MeasurementCollection : BindingList<Measurement>
    {
        public void AddMeasurement<T>(string name, T measuredValue, string units, T lowerLimit, T upperLimit, MeasurementParameterCollection parameters)
            where T : IComparable<T>
        {
            Add(new Measurement<T>(name, measuredValue, units, lowerLimit, upperLimit, parameters));
        }

        public void AddMeasurement<T>(string name, T measuredValue, string units, T lowerLimit, T upperLimit, params MeasurementParameter[] parameters)
            where T : IComparable<T>
        {
            AddMeasurement(name, measuredValue, units, lowerLimit, upperLimit, new MeasurementParameterCollection(parameters));
        }

        public override string ToString()
        {
            return ToString(Environment.NewLine, ",");
        }

        public string ToString(string rowSeparator, string columnSeparator)
        {
            return string.Join(rowSeparator, this.Select(meas => meas.ToString(columnSeparator)));
        }
    }

    public class MeasurementParameter
    {
        public readonly string Name;
        public readonly object Value;

        public MeasurementParameter(string name, object val)
        {
            Name = name;
            Value = val;
        }
    }

    [Serializable]
    public class MeasurementParameterCollection : Dictionary<string, object>
    {
        public MeasurementParameterCollection(IEnumerable<MeasurementParameter> parameters)
            : base(parameters.ToDictionary(p => p.Name, p => p.Value))
        {
            
        }

        protected MeasurementParameterCollection(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}