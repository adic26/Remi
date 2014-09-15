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
        public TestResultsHeader CollectionHeader { get; private set; }

        public TestResultCollection(IEnumerable<Measurement> measurements)
            : base(measurements.ToList()) { }

        public TestResultCollection()
        {
            AllowNew = true;
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


        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("CollectionHeader", CollectionHeader);
            info.AddValue("Items", Items);
        }

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

        public XmlSchema GetSchema() { return null; }

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