using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;

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

    public struct Measurement<T>
        where T : IComparable<T>
    {
        public readonly T MeasuredValue;
        public readonly string Units;
        public readonly T LowerLimit;
        public readonly T UpperLimit;
        public readonly Dictionary<string, string> Parameters;

        public readonly MeasurementResult Result;
        public readonly DateTime Timestamp;

        public Measurement(T measuredValue, string units, T lowerLimit, T upperLimit, Dictionary<string, string> parameters)
        {
            Timestamp = DateTime.Now;

            MeasuredValue = measuredValue;
            Units = units;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Parameters = parameters;

            if (measuredValue.CompareTo(lowerLimit) < 0)
                Result = MeasurementResult.Fail_Low;
            else if (measuredValue.CompareTo(upperLimit) > 0)
                Result = MeasurementResult.Fail_High;
            else
                Result = MeasurementResult.Pass;
        }

        public Measurement(T measuredValue, string units, T lowerLimit, T upperLimit, params KeyValuePair<string,string>[] parameters)
        {
            Timestamp = DateTime.Now;

            MeasuredValue = measuredValue;
            Units = units;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Parameters = parameters.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            if (measuredValue.CompareTo(lowerLimit) < 0)
                Result = MeasurementResult.Fail_Low;
            else if (measuredValue.CompareTo(upperLimit) > 0)
                Result = MeasurementResult.Fail_High;
            else
                Result = MeasurementResult.Pass;
        }

        public override string ToString()
        {
            return ToString(",");
        }

        public string ToString(string separator)
        {
            string pToString = string.Join(separator, Parameters.Select(kvp => kvp.Key + "=" + kvp.Value));
            return string.Join(separator, MeasuredValue, Units, LowerLimit, UpperLimit, Result, pToString).TrimEnd(',');
        }
    }

    public class MeasurementCollection<T> : BindingList<Measurement<T>>
        where T : IComparable<T>
    {
        public override string ToString()
        {
            return ToString(Environment.NewLine, ",");
        }

        public string ToString(string rowSeparator, string columnSeparator)
        {
            return string.Join(rowSeparator, this.Select(meas => meas.ToString(columnSeparator)));
        }
    }

}