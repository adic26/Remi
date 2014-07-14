using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib
{
    public struct Measurement<T>
        where T : IComparable<T>
    {
        public T MeasuredValue;
        public string Units;
        public T LowerLimit;
        public T UpperLimit;
        public Dictionary<string, string> Parameters;

        public MeasurementResult Result;
        public DateTime Timestamp;

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
            Parameters = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> keyValuePair in parameters)
                Parameters.Add(keyValuePair.Key, keyValuePair.Value);

            if (measuredValue.CompareTo(lowerLimit) < 0)
                Result = MeasurementResult.Fail_Low;
            else if (measuredValue.CompareTo(upperLimit) > 0)
                Result = MeasurementResult.Fail_High;
            else
                Result = MeasurementResult.Pass;
        }
    }

    [Flags]
    public enum MeasurementResult
    {
        Pass = 0,
        Fail_Low = 1,
        Fail_High = 2,
        Fail = Fail_Low | Fail_High
    }

}