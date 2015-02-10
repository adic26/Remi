using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.Measurements
{
    /// <summary>
    /// An informational entry in the test results collection.
    /// </summary>
    [Serializable]
    public class TestInfo : ITestInfo
    {
        /// <summary>
        /// Name of the information entry.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Value of the information entry.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Initialize a new TestInfo object with the specified name and value.
        /// </summary>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        public TestInfo(string name, object val)
        {
            Name = name;
            Value = val;
        }


        /// <summary>
        /// Returns a string that represents the <see cref="TestInfo"/> object.
        /// </summary>
        /// <returns>A string containing the name and value of the <see cref="TestInfo"/> object.</returns>
        public override string ToString()
        {
            return Name + " = " + Value;
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
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("Value", Value.ToString());
        }

        /// <summary>
        /// Deserialize and XML representation into a MeasurementParameter object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            Name = reader.ReadElementContentAsString("Name", "");
            Value = reader.ReadElementContentAsObject("Value", "");
            reader.ReadEndElement();
        }
    }
}