using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Represents a collection of Measurement objects.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "TestResults", Namespace = "TsdLib.ResultsFile.xsd")]
    public class TestResultCollection : BindingList<Measurement>, ISerializable, IXmlSerializable
    {
        /// <summary>
        /// Gets the header containing test results metadata.
        /// </summary>
        public TestResultsHeader CollectionHeader { get; private set; }

        /// <summary>
        /// Initialize a new TestResultCollection.
        /// </summary>
        public TestResultCollection()
        {
            AllowNew = true;
        }

        /// <summary>
        /// Initialize a new TestResultCollection with the specified sequence of measurments.
        /// </summary>
        /// <param name="measurements">A sequence of measurements to add to the TestResultCollection.</param>
        public TestResultCollection(IEnumerable<Measurement> measurements)
            : base(measurements.ToList()) { }

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
            where T : IComparable
        {
            Add(new Measurement(name, measuredValue, units, lowerLimit, upperLimit, parameters));
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
            where T : IComparable
        {
            AddMeasurement(name, measuredValue, units, lowerLimit, upperLimit, new MeasurementParameterCollection(parameters));
        }

        /// <summary>
        /// Add the metadata header to the TestResultCollection.
        /// </summary>
        /// <param name="header">Header object containing test result metadata.</param>
        public void AddHeader(TestResultsHeader header)
        {
            CollectionHeader = header;
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
        /// <returns>A string representation of the MeasurementCollection formatted with row and column delimiters.</returns>
        public string ToString(string rowSeparator, string columnSeparator)
        {
            return string.Join(rowSeparator, this.Select(meas => meas.ToString(columnSeparator)));
        }

        #region ISerializable

        IFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Serialize the TestResultCollection object into a binary stream.
        /// </summary>
        /// <param name="info">Stores all the data needed to serialize the object.</param>
        /// <param name="context">Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CollectionHeader", CollectionHeader);
            info.AddValue("Items", Items);
        }

        /// <summary>
        /// Deserialize a binary stream into a TestResultCollection object.
        /// </summary>
        /// <param name="info">Stores all the data needed to deserialize the object.</param>
        /// <param name="context">Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.</param>
        protected TestResultCollection(SerializationInfo info, StreamingContext context)
        {
            CollectionHeader = (TestResultsHeader)info.GetValue("CollectionHeader", typeof(TestResultsHeader));
            foreach (object o in (IList)info.GetValue("Items", typeof(IList)))
                Items.Add((Measurement)o);
        }

        #endregion

        #region IXmlSerializable

        private XNamespace _ns = "TsdLib.ResultsFile.xsd";
        private XmlSerializer _headerSerializer = new XmlSerializer(typeof(TestResultsHeader));
        private XmlSerializer _measurementSerializer = new XmlSerializer(typeof(Measurement));

        /// <summary>
        /// Not used. Required for IXmlSerializable.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Serialize the TestResultCollection object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("urn", "TsdLib.ResultsFile.xsd") });

            _headerSerializer.Serialize(writer, CollectionHeader);

            writer.WriteStartElement("Measurements", _ns.NamespaceName);

            foreach (Measurement measurement in Items)
                _measurementSerializer.Serialize(writer, measurement);
            writer.WriteEndElement();

            //TODO: remove the urn prefix once DataLogger is fixed
        }

        /// <summary>
        /// Deserialize and XML representation into a TestResultCollection object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            XElement testResultsElement = XElement.Load(reader);

            XElement headerElement = testResultsElement.Element(_ns + "Header");

            CollectionHeader = (TestResultsHeader)_headerSerializer.Deserialize(headerElement.CreateReader());

            XElement measurementsElement = testResultsElement.Element(_ns + "Measurements");

            foreach (XElement measurementElement in measurementsElement.Elements(_ns + "Measurement"))
                Items.Add((Measurement)_measurementSerializer.Deserialize(measurementElement.CreateReader()));

        }

        #endregion
    }
}