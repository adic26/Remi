using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.TestResults
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
    [Serializable]
    [XmlRoot(ElementName = "Measurement", Namespace = "TsdLib.ResultsFile.xsd")]
    public class Measurement : ISerializable, IXmlSerializable
    {
        private XNamespace _ns = "TsdLib.ResultsFile.xsd";

        /// <summary>
        /// Name to describe the measurement.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Magnitude of the measurement.
        /// </summary>
        public IComparable MeasuredValue { get; private set; }
        /// <summary>
        /// Minimum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public IComparable LowerLimit { get; private set; }
        /// <summary>
        /// Maximum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public IComparable UpperLimit { get; private set; }
        /// <summary>
        /// Unit of measure.
        /// </summary>
        public string Units { get; private set; }
        /// <summary>
        /// A collection of MeasurementParameter objects describing the measurement conditions.
        /// </summary>
        public MeasurementParameterCollection Parameters { get; private set; }

        /// <summary>
        /// Timestamp of when the measurement was captured.
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Gets the pass/fail result by comparing the MeasuredVal to the LowerLim and UpperLim
        /// </summary>
        public MeasurementResult Result { get; private set; }

        // ReSharper disable once UnusedMember.Local - Default constructor required for serialization.
        private Measurement() { }

        /// <summary>
        /// Initialize a new Measurement object.
        /// </summary>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="parameters">A collection of MeasurementParameter objects describing the measurement conditions.</param>
        public Measurement(string name, IComparable measuredValue, string units, IComparable lowerLimit, IComparable upperLimit, MeasurementParameterCollection parameters)
        {
            Name = name;
            MeasuredValue = measuredValue;
            Units = units;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Parameters = parameters;
            Timestamp = DateTime.Now;

            if (measuredValue.CompareTo(lowerLimit) < 0)
                Result = MeasurementResult.Fail_Low;
            else if (measuredValue.CompareTo(upperLimit) > 0)
                Result = MeasurementResult.Fail_High;
            else
                Result = MeasurementResult.Pass;
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
        public Measurement(string name, IComparable measuredValue, string units, IComparable lowerLimit, IComparable upperLimit, params MeasurementParameter[] parameters)
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
            return string.Join(separator, Name, MeasuredValue, Units, LowerLimit, UpperLimit, Result,
                Parameters.ToString(separator));
        }

        #region ISerializable

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("MeasuredValue", MeasuredValue);
            info.AddValue("LowerLimit", LowerLimit);
            info.AddValue("UpperLimit", UpperLimit);
            info.AddValue("Units", Units);
            info.AddValue("Parameters", Parameters);
            info.AddValue("Timestamp", Timestamp);
            info.AddValue("Result", Result);
        }

        protected Measurement(SerializationInfo info, StreamingContext context)
        {
            Name = info.GetString("Name");
            MeasuredValue = (IComparable) info.GetValue("MeasuredValue", typeof(IComparable));
            LowerLimit = (IComparable) info.GetValue("LowerLimit", typeof(IComparable));
            UpperLimit = (IComparable) info.GetValue("UpperLimit", typeof(IComparable));
            Units = info.GetString("Units");
            Parameters = (MeasurementParameterCollection) info.GetValue("Parameters", typeof(MeasurementParameterCollection));
            Timestamp = (DateTime) info.GetValue("Timestamp", typeof (DateTime));
            Result = (MeasurementResult) info.GetValue("Result", typeof (MeasurementResult));
        }

        #endregion

        #region IXmlSerializable

        public XmlSchema GetSchema() { return null; }

        public void WriteXml(XmlWriter writer)
        {
            new XElement(_ns + "MeasurementName", Name).WriteTo(writer);
            new XElement(_ns + "LowerLimit", LowerLimit).WriteTo(writer);
            new XElement(_ns + "UpperLimit", UpperLimit).WriteTo(writer);
            new XElement(_ns + "MeasuredValue", MeasuredValue).WriteTo(writer);
            new XElement(_ns + "PassFail", Result.ToString()).WriteTo(writer);
            new XElement(_ns + "Units", Units).WriteTo(writer);
            XElement parametersElement = new XElement(_ns + "Parameters");
            foreach (MeasurementParameter measurementParameter in Parameters)
                parametersElement.Add(new XElement(_ns + "Parameter",
                    new XAttribute("ParameterName", measurementParameter.Name), measurementParameter.Value));
            if (parametersElement.HasElements)
                parametersElement.WriteTo(writer);
        }

        public void ReadXml(XmlReader reader)
        {
            XElement measurementElement = XElement.Load(reader);

            Name = (string) measurementElement.Element(_ns + "MeasurementName");
            LowerLimit = (string) measurementElement.Element(_ns + "LowerLimit");
            UpperLimit = (string) measurementElement.Element(_ns + "UpperLimit");
            MeasuredValue = (string) measurementElement.Element(_ns + "MeasuredValue");
            Units = (string)measurementElement.Element(_ns + "Units");

            XElement pElement = measurementElement.Element(_ns + "Parameters");
            if (pElement != null)
                Parameters = new MeasurementParameterCollection(pElement.Elements("Parameter")
                    .Select(e => new MeasurementParameter((string)e.Attribute("ParameterName"), e.Value)));

            //Timestamp = DateTime.Parse(reader.ReadElementContentAsString("Timestamp", ""));

            MeasurementResult result;
            Enum.TryParse((string) measurementElement.Element(_ns + "PassFail"), out result);
            Result = result.HasFlag(MeasurementResult.Pass) ? MeasurementResult.Pass : MeasurementResult.Fail;
        }

        #endregion
    }

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
        /// Initialize a new MeasurementParameterCollection using a sequence of existing MeasurementParameter objects.
        /// </summary>
        /// <param name="parameters">A sequence of existing MeasurementParameter objects.</param>
        public MeasurementParameterCollection(IEnumerable<MeasurementParameter> parameters)
            : base(parameters) { }

        /// <summary>
        /// Gets the value of a MeasurementParameter by name.
        /// </summary>
        /// <param name="name">Name of the MeasurementParameter.</param>
        /// <returns>Value of the MeasurementParameter with the specified name.</returns>
        public object this [string name]
        {
            get
            {
                MeasurementParameter mp = this.FirstOrDefault(p => p.Name == name);
                if (mp == null)
                    throw new MeasurementParameterException(name);
                return mp.Value;
            }
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

    /// <summary>
    /// Provides measurement data to pass to a measurement captured event.
    /// </summary>
    [Serializable]
    public class MeasurementEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Measurement object that generated the event.
        /// </summary>
        public Measurement Measurement { get; private set; }

        /// <summary>
        /// Initialize a new instance of the MeasurementEventArgs class.
        /// </summary>
        /// <param name="measurement">Measurement to pass to the event handlers.</param>
        public MeasurementEventArgs(Measurement measurement)
        {
            Measurement = measurement;
        }
    }
}