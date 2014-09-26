using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.TestResults
{

    /// <summary>
    /// A weakly-typed Measurement object to serve as a base for generic-typed measurements. Allows adding different types of measurements to the same collection.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Measurement", Namespace = "TsdLib.ResultsFile.xsd")]
    public class Measurement : ISerializable, IXmlSerializable
    {
        private readonly XNamespace _ns = "TsdLib.ResultsFile.xsd";

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

            if (upperLimit.ToString() == "" && lowerLimit.ToString() == "")
                Result = MeasurementResult.Pass;
            else if (measuredValue.CompareTo(lowerLimit) < 0)
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

        /// <summary>
        /// Serialize the Measurement object into a binary stream.
        /// </summary>
        /// <param name="info">Stores all the data needed to serialize the object.</param>
        /// <param name="context">Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.</param>
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

        /// <summary>
        /// Deserialize a binary stream into a Measurement object.
        /// </summary>
        /// <param name="info">Stores all the data needed to deserialize the object.</param>
        /// <param name="context">Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.</param>
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

        /// <summary>
        /// Not used. Required for IXmlSerializable.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Serialize the Measurement object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
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

        /// <summary>
        /// Deserialize and XML representation into a Measurement object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
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
}