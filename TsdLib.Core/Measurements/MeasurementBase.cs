using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.Measurements
{
    /// <summary>
    /// Base class for all Measurement implementations. Defines members required to serialize to xml and/or binary.
    /// </summary>
    [Serializable]
    public abstract class MeasurementBase : IXmlSerializable
    {
        /// <summary>
        /// Gets the pass/fail result of the measurement.
        /// </summary>
        public abstract MeasurementResult Result { get; }

        /// <summary>
        /// Gets the name that describes the measurement.
        /// </summary>
        public string MeasurementName { get; private set; }

        /// <summary>
        /// Gets the minimum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public string LowerLimit { get; private set; }

        /// <summary>
        /// Gets the maximum inclusive value of the acceptable range of values for the measurement.
        /// </summary>
        public string UpperLimit { get; private set; }

        /// <summary>
        /// Gets the magnitude of the measurement.
        /// </summary>
        public string MeasuredValue { get; private set; }

        /// <summary>
        /// Gets the unit of measure.
        /// </summary>
        public string Units { get; private set; }

        /// <summary>
        /// Gets an array of absolute paths of attached files.
        /// </summary>
        public string[] Files { get; private set; }

        /// <summary>
        /// Gets any comments that were entered as the measurement was performed.
        /// </summary>
        public string Comments { get; private set; }

        /// <summary>
        /// Gets a detailed description of the measurement.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets an array of MeasurementParameter objects describing the measurement conditions.
        /// </summary>
        public MeasurementParameter[] Parameters { get; private set; }

        private MeasurementBase() { }

        /// <summary>
        /// Initialize a new Measurement object.
        /// </summary>
        /// <param name="measurementName">Name to describe the measurement.</param>
        /// <param name="measuredValue">Magnitude of the measurement.</param>
        /// <param name="units">Unit of measure.</param>
        /// <param name="lowerLimit">Minimum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="upperLimit">Maximum inclusive value of the acceptable range of values for the measurement.</param>
        /// <param name="files">OPTIONAL: An array of absolute paths of files to attach.</param>
        /// <param name="comments">OPTIONAL: Any comments to include additional information.</param>
        /// <param name="description">OPTIONAL: A detailed description of the measurement.</param>
        /// <param name="parameters">OPTIONAL: A collection of MeasurementParameter objects describing the measurement conditions.</param>
        protected MeasurementBase(string measurementName, string measuredValue, string units, string lowerLimit, string upperLimit, string[] files = null, string comments = "", string description = "", params MeasurementParameter[] parameters)
        {
            MeasurementName = measurementName;
            MeasuredValue = measuredValue;
            Units = units;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
            Files = files ?? new string[0];
            Comments = comments;
            Description = description;
            Parameters = parameters ?? new MeasurementParameter[0];
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
            return string.Join(separator, MeasurementName, MeasuredValue, Units, LowerLimit, UpperLimit, Result, string.Join(separator, Parameters.Select(p => p.Name + "=" + p.Value)));
        }

        /// <summary>
        /// Not used. Required for IXmlSerializable.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Serialize the Measurement object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("MeasurementName", MeasurementName);
            writer.WriteElementString("LowerLimit", LowerLimit);
            writer.WriteElementString("UpperLimit", UpperLimit);
            writer.WriteElementString("MeasuredValue", MeasuredValue);
            MeasurementResult result = Result.HasFlag(MeasurementResult.Fail) ? MeasurementResult.Fail : Result;
            writer.WriteElementString("PassFail", result.ToString());
            writer.WriteElementString("Units", Units);

            writer.WriteStartElement("Files");
            foreach (string fileName in Files)
                writer.WriteElementString("FileName", fileName);
            writer.WriteEndElement();
            
            writer.WriteElementString("Comments", Comments);

            writer.WriteElementString("Description", Description);

            writer.WriteStartElement("Parameters");
            foreach (MeasurementParameter parameter in Parameters)
            {
                writer.WriteStartElement("Parameter");
                parameter.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            
        }

        /// <summary>
        /// Deserialize and XML representation into a Measurement object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public virtual void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement("Measurement", "TsdLib.ResultsFile.xsd");

            reader.ReadEndElement();
        }
    }
}