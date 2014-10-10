using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Represents a collection of <see cref="TsdLib.TestResults.Measurement"/> objects with a <see cref="TsdLib.TestResults.TestResultsHeader"/> to store metadata.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "TestResults", Namespace = "TsdLib.ResultsFile.xsd")]
    public class TestResultCollection : BindingList<Measurement>, ITestResults
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
        /// Add a <see cref="TsdLib.TestResults.Measurement"/> object to the test results collection.
        /// </summary>
        /// <param name="measurement"><see cref="TsdLib.TestResults.Measurement"/> object to add.</param>
        public void AddMeasurement(Measurement measurement)
        {
            Add(measurement);
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
        /// Save the test results to an xml file in C:\TestResults.
        /// </summary>
        /// <returns>The absolute path to the Xml file generated.</returns>
        public string Save()
        {
            return Save(new DirectoryInfo(@"C:\TestResults"));
        }

        /// <summary>
        /// Save the test results to an xml file in the specified directory.
        /// </summary>
        /// <param name="directory">A <see cref="System.IO.DirectoryInfo"/> object representing the directory to save the test results file to.</param>
        /// <returns>The absolute path to the Xml file generated.</returns>
        public string Save(DirectoryInfo directory)
        {
            if (!directory.Exists)
                directory.Create();

            string fileName = Path.Combine(directory.FullName, "Results_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".xml");

            XmlSerializer xs = new XmlSerializer(typeof(TestResultCollection));

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("urn", "xmlns:relab.rim.com/ResultFile.xsd");

            using (Stream s = File.Create(fileName))
                xs.Serialize(s, this, ns);

            //string prefixedNamespace = Regex.Replace(File.ReadAllText(fileName), "<TestResults xmlns=\"TsdLib.ResultsFile.xsd\">", "<TestResults xmlns:urn=\"TsdLib.ResultsFile.xsd\">");

            //File.WriteAllText(fileName, prefixedNamespace);

            return fileName;
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
            List<string> measurements = this.Select(meas => meas.ToString(columnSeparator)).ToList();
            measurements.Insert(0, Measurement.GetMeasurementCategories(columnSeparator));
            return string.Join(rowSeparator, measurements);
        }

        #region ISerializable

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

        /// <summary>
        /// Gets the name of the Xsd target namespace.
        /// </summary>
        public string XsdNamespace { get { return _ns.NamespaceName; } }

        private readonly XNamespace _ns = "TsdLib.ResultsFile.xsd";
        private readonly XmlSerializer _headerSerializer = new XmlSerializer(typeof(TestResultsHeader));
        private readonly XmlSerializer _measurementSerializer = new XmlSerializer(typeof(Measurement));

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

            if (headerElement == null)
                throw new SerializationException(testResultsElement, _ns.NamespaceName + "Header");

            CollectionHeader = (TestResultsHeader)_headerSerializer.Deserialize(headerElement.CreateReader());

            XElement measurementsElement = testResultsElement.Element(_ns + "Measurements");

            if (measurementsElement == null)
                throw new SerializationException(testResultsElement, _ns.NamespaceName + "Measurements");

            foreach (XElement measurementElement in measurementsElement.Elements(_ns + "Measurement"))
                Items.Add((Measurement)_measurementSerializer.Deserialize(measurementElement.CreateReader()));

        }

        #endregion
    }

    /// <summary>
    /// Represents a collection of <see cref="TsdLib.TestResults.Measurement"/> objects with a <see cref="TsdLib.TestResults.TestResultsHeader"/> to store metadata.
    /// </summary>
    public interface ITestResults : ISerializable, IXmlSerializable
    {
        /// <summary>
        /// Gets the name of the Xsd target namespace.
        /// </summary>
        string XsdNamespace { get; }

        /// <summary>
        /// Add the <see cref="TsdLib.TestResults.TestResultsHeader"/> metadata to the test results collection.
        /// </summary>
        /// <param name="header">Header object containing test result metadata.</param>
        void AddHeader(TestResultsHeader header);

        /// <summary>
        /// Add a <see cref="TsdLib.TestResults.Measurement"/> object to the test results collection.
        /// </summary>
        /// <param name="measurement"><see cref="TsdLib.TestResults.Measurement"/> object to add.</param>
        void AddMeasurement(Measurement measurement);

        /// <summary>
        /// Save the test results to an xml file in C:\TestResults.
        /// </summary>
        /// <returns>The absolute path to the Xml file generated.</returns>
        string Save();

        /// <summary>
        /// Save the test results to an xml file in the specified directory.
        /// </summary>
        /// <param name="directory">A <see cref="System.IO.DirectoryInfo"/> object representing the directory to save the test results file to.</param>
        /// <returns>The absolute path to the Xml file generated.</returns>
        string Save(DirectoryInfo directory);

        /// <summary>
        /// Returns all of the Measurement objects, separated by the specified row and column delimiters.
        /// </summary>
        /// <param name="rowSeparator">Delimiter string to insert between each field of the Measurement object.</param>
        /// <param name="columnSeparator">Delimiter string to insert between each Measurement object.</param>
        /// <returns>A string representation of the MeasurementCollection formatted with row and column delimiters.</returns>
        string ToString(string rowSeparator, string columnSeparator);
    }
}