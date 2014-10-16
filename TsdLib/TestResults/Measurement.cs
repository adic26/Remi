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
        /// <summary>
        /// Gets a delimited string containing the names of the <see cref="TsdLib.TestResults.Measurement"/> fields. Useful for creating a header row.
        /// </summary>
        /// <param name="separator">Delimiter to use for separating the fields.</param>
        /// <returns>A header row string with the names of the <see cref="TsdLib.TestResults.Measurement"/> fields.</returns>
        public static string GetMeasurementCategories(string separator)
        {
            return string.Join(separator, "Measurement Name", "Measured Value", "Units", "Lower Limit", "Upper Limit", "Result");
        }

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
        /// Gets a detailed description of the measurement.
        /// </summary>
        public string Description { get; private set; }
        /// <summary>
        /// Gets any comments that were entered as the measurement was performed.
        /// </summary>
        public string Comments { get; private set; }
        /// <summary>
        /// Gets the absolute path to a file attachment.
        /// </summary>
        public string FilePath { get; private set; }

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
        /// <param name="lowerLimit">OPTIONAL: Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">OPTIONAL: Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="description">OPTIONAL: A detailed description of the measurement.</param>
        /// <param name="comments">OPTIONAL: Any comments to include additional information.</param>
        /// <param name="filePath">OPTIONAL: The absolute path of a file to attach.</param>
        /// <param name="parameters">OPTIONAL: A collection of MeasurementParameter objects describing the measurement conditions.</param>
        private Measurement(string name, IComparable measuredValue, string units, IComparable lowerLimit = null, IComparable upperLimit = null, string description = "", string comments = "", string filePath = "", params MeasurementParameter[] parameters)
        {
            Name = name;
            MeasuredValue = measuredValue;
            Units = units;
            LowerLimit = lowerLimit ?? "N/A";
            UpperLimit = upperLimit ?? "N/A";
            Description = description;
            Comments = comments;
            FilePath = filePath;
            Parameters = parameters.Length > 0 ? new MeasurementParameterCollection(parameters) : new MeasurementParameterCollection();
            Timestamp = DateTime.Now;

            //Case 1: no limits entered - result is a pass
            if (lowerLimit == null && upperLimit == null)
                Result = MeasurementResult.Pass;

            //Case 2: both limits entered - check against both
            else if (lowerLimit != null && upperLimit != null)
                if (measuredValue is bool && lowerLimit is bool && upperLimit is bool)
                    Result = ((bool) measuredValue == (bool) lowerLimit == (bool) upperLimit) ? MeasurementResult.Pass : MeasurementResult.Fail;
                else if (measuredValue.CompareTo(lowerLimit) < 0)
                    Result = MeasurementResult.Fail_BelowLimit;
                else if (measuredValue.CompareTo(upperLimit) > 0)
                    Result = MeasurementResult.Fail_AboveLimit;
                else
                    Result = MeasurementResult.Pass;

            //Case 3: only lower limit entered - check against it
            else if (lowerLimit != null)
                Result = measuredValue.CompareTo(lowerLimit) > 0 ? MeasurementResult.Pass : MeasurementResult.Fail_BelowLimit;

            //Case 4: only upper limit entered - check against it
            else
                Result = measuredValue.CompareTo(upperLimit) < 0 ? MeasurementResult.Pass : MeasurementResult.Fail_AboveLimit;

        }

        /// <summary>
        /// Create a new Measurement object.
        /// </summary>
        /// <typeparam name="T">Data type of the measured value and upper/lower limits. Must implement IComparable.</typeparam>
        /// <param name="name">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">OPTIONAL: Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">OPTIONAL: Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="description">OPTIONAL: A detailed description of the measurement.</param>
        /// <param name="comments">OPTIONAL: Any comments to include additional information.</param>
        /// <param name="filePath">OPTIONAL: The absolute path of a file to attach.</param>
        /// <param name="parameters">OPTIONAL: A collection of MeasurementParameter objects describing the measurement conditions.</param>
        /// <returns>The new <see cref="TsdLib.TestResults.Measurement"/> object.</returns>
        public static Measurement CreateMeasurement<T>(string name, T measuredValue, string units, T lowerLimit = default(T), T upperLimit = default(T), string description = "", string comments = "", string filePath = "", params MeasurementParameter[] parameters)
            where T : IComparable
        {
            return new Measurement(name, measuredValue, units, lowerLimit, upperLimit, description, comments, filePath, parameters);
        }

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
            //TODO: make sure it's ok to serialize null values
            info.AddValue("Name", Name);
            info.AddValue("MeasuredValue", MeasuredValue);
            info.AddValue("Units", Units);
            info.AddValue("LowerLimit", LowerLimit);
            info.AddValue("UpperLimit", UpperLimit);
            info.AddValue("Description", Description);
            info.AddValue("Comments", Comments);
            info.AddValue("FilePath", FilePath);
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
            Units = info.GetString("Units");
            LowerLimit = (IComparable)info.GetValue("LowerLimit", typeof(IComparable));
            UpperLimit = (IComparable) info.GetValue("UpperLimit", typeof(IComparable));
            Description = info.GetString("Description");
            Comments = info.GetString("Comments");
            FilePath = info.GetString("FilePath");
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
            if (LowerLimit != null)
                new XElement(_ns + "LowerLimit", LowerLimit).WriteTo(writer);
            if (UpperLimit != null)
                new XElement(_ns + "UpperLimit", UpperLimit).WriteTo(writer);
            new XElement(_ns + "MeasuredValue", MeasuredValue).WriteTo(writer);
            new XElement(_ns + "PassFail", Result.ToString()).WriteTo(writer);
            new XElement(_ns + "Units", Units).WriteTo(writer);
            if (!string.IsNullOrWhiteSpace(FilePath))
                new XElement(_ns + "FileName", FilePath).WriteTo(writer);
            if (!string.IsNullOrWhiteSpace(Comments))
                new XElement(_ns + "Comments", Comments).WriteTo(writer);
            if (!string.IsNullOrWhiteSpace(Description))
                new XElement(_ns + "Description", Description).WriteTo(writer);
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
            XElement lowerLimitElement = measurementElement.Element(_ns + "LowerLimit");
            LowerLimit = lowerLimitElement != null? (string) lowerLimitElement : null;
            XElement upperLimitElement = measurementElement.Element(_ns + "UpperLimit");
            UpperLimit = upperLimitElement != null ? (string)upperLimitElement : null;
            MeasuredValue = (string) measurementElement.Element(_ns + "MeasuredValue");
            Units = (string)measurementElement.Element(_ns + "Units");

            XElement filePathElement = measurementElement.Element(_ns + "FileName");
            FilePath = filePathElement != null ? (string) filePathElement : null;

            XElement commentsElement = measurementElement.Element(_ns + "Comments");
            FilePath = commentsElement != null ? (string)commentsElement : null;
            XElement descriptionElement = measurementElement.Element(_ns + "Description");
            FilePath = descriptionElement != null ? (string)descriptionElement : null;
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