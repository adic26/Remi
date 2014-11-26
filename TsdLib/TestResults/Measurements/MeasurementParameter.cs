using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.TestResults
{
    /// <summary>
    /// A test condition or other information used to describe test conditions.
    /// </summary>
    [Serializable]
    public class MeasurementParameter : IXmlSerializable
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
        /// Not used. Required for IXmlSerializable.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Serialize the MeasurementParameter object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("ParameterName", Name);
            writer.WriteString(Value.ToString());
        }

        /// <summary>
        /// Deserialize and XML representation into a MeasurementParameter object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            //TODO: implement
        }
    }
}