using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using TsdLib.Configuration;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Represents a collection of <see cref="TsdLib.TestResults.MeasurementBase"/> objects and <see cref="TsdLib.TestResults.TestInfo"/> objects with a <see cref="TsdLib.Configuration.TestDetails"/> to store metadata and a <see cref="TsdLib.TestResults.TestSummary"/> to summarize the test results.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "TestResults", Namespace = "TsdLib.ResultsFile.xsd")]
    public class TestResultCollection : IXmlSerializable
    {
        static XmlSerializer _serializer = new XmlSerializer(typeof(TestResultCollection));
        static IFormatter _formatter = new BinaryFormatter();

        /// <summary>
        /// Loads an existing test results collection from storage.
        /// </summary>
        /// <param name="filePath">Location of the saved test results collection file. Can be *.xml or *.bin.</param>
        /// <returns>A TestResultsCollection object.</returns>
        public static TestResultCollection Load(FileInfo filePath)
        {
            if (filePath.Extension.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                return (TestResultCollection) _serializer.Deserialize(File.OpenRead(filePath.FullName));
            if (filePath.Extension.Equals("bin", StringComparison.InvariantCultureIgnoreCase))
                return (TestResultCollection)_formatter.Deserialize(File.OpenRead(filePath.FullName));

            throw new ArgumentException("File must be in *.xml or *.bin format", "filePath");
        }

        /// <summary>
        /// Gets the test detail metadata.
        /// </summary>
        public TestDetails Details { get; set; }
        /// <summary>
        /// Gets a summary of the test results.
        /// </summary>
        public TestSummary Summary { get; set; }

        /// <summary>
        /// Gets the information captured during the test.
        /// </summary>
        public IEnumerable<TestInfo> Information { get; set; }

        /// <summary>
        /// Gets the measurements captued during the test.
        /// </summary>
        public IEnumerable<MeasurementBase> Measurements { get; set; }

        private TestResultCollection() { }

        /// <summary>
        /// Initialize a new TestResultCollection.
        /// </summary>
        /// <param name="details">A <see cref="TsdLib.Configuration.TestDetails"/> object describing the test request or job details.</param>
        /// <param name="information">A sequence of informational entries captured during the test sequence.</param>
        /// <param name="measurements">A sequence of measurements captured during the test sequence. Can be any type deriving from <see cref="TsdLib.TestResults.MeasurementBase"/>.</param>
        /// <param name="summary">A <see cref="TsdLib.TestResults.TestSummary"/> object summarizing the test results.</param>
        public TestResultCollection(TestDetails details, IEnumerable<TestInfo> information, IEnumerable<MeasurementBase> measurements, TestSummary summary)
        {
            Details = details;
            Information = information;
            Measurements = measurements;
            Summary = summary;
        }


        /// <summary>
        /// Save the test results to a binary file. Useful when storing in-progress tests that can be recalled later.
        /// </summary>
        /// <param name="directory">A <see cref="System.IO.DirectoryInfo"/> object representing the directory to save the test results file to.</param>
        /// <returns>The absolute path to the binary file generated.</returns>
        public string SaveBinary(DirectoryInfo directory)
        {
            if (!directory.Exists)
                directory.Create();

            
            string formattedFileName = string.Format("{0}-{1}_", Details.JobNumber, Details.UnitNumber.ToString("D4"));

            string fileName = Path.Combine(directory.FullName, formattedFileName + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".bin");

            using (FileStream s = File.Create(Path.Combine(directory.FullName, fileName)))
                _formatter.Serialize(s, this);

            return fileName;
        }

        /// <summary>
        /// Save the test results to an xml file in the specified directory.
        /// </summary>
        /// <param name="directory">A <see cref="System.IO.DirectoryInfo"/> object representing the directory to save the test results file to.</param>
        /// <returns>The absolute path to the xml file generated.</returns>
        public string Save(DirectoryInfo directory)
        {
            if (!directory.Exists)
                directory.Create();

            string jobNumber = string.IsNullOrWhiteSpace(Details.JobNumber) ? "jobNum" : Details.JobNumber;

            string unitNumber = Details.UnitNumber.ToString("D3");
            string timeStamp = Summary.DateStarted.ToString("yyyy-MM-dd_hh-mm-ss");

            string fileName = Path.Combine(directory.FullName, jobNumber + "-" + unitNumber + "-" + timeStamp + ".xml");

            using (Stream s = File.Create(fileName))
                _serializer.Serialize(s, this);

            return fileName;
        }

        /// <summary>
        /// Returns a CSV-friendly representation of the measurements.
        /// </summary>
        /// <returns>A string containing all of the measurements, delimited with Environment.NewLine and commas.</returns>
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
            return string.Join(rowSeparator,
                Details != null ? "Test Details" + rowSeparator + Details : "",
                Summary != null ? "Summary" + rowSeparator + Summary : "",
                //Measurement.GetMeasurementCategories(columnSeparator),
                string.Join(rowSeparator, Measurements.Select(meas => meas.ToString(columnSeparator)))
                );
        }

        #region IXmlSerializable

        /// <summary>
        /// Gets the name of the Xsd target namespace.
        /// </summary>
        public string XsdNamespace { get { return _ns.NamespaceName; } }

        private readonly XNamespace _ns = "TsdLib.ResultsFile.xsd";
        private readonly XmlSerializer _detailsSerializer = new XmlSerializer(typeof(TestDetails));
        private readonly XmlSerializer _measurementSerializer = new XmlSerializer(typeof(MeasurementBase));

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
            writer.WriteStartElement("Header");
            Details.WriteXml(writer);
            writer.WriteElementString("Duration", Summary.Duration.ToString("g").Split('.')[0]);
            writer.WriteElementString("DateCompleted", Summary.DateCompleted.ToString("yyyy-MM-dd-hh-mm-ss"));
            writer.WriteEndElement();

            writer.WriteStartElement("Information");
            foreach (TestInfo testInfo in Information)
            {
                writer.WriteStartElement("Info");
                testInfo.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Measurements");
            foreach (MeasurementBase measurement in Measurements)
            {
                writer.WriteStartElement("Measurement");
                measurement.WriteXml(writer);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Footer");
            writer.WriteElementString("FinalResult", Summary.FinalResult);
            writer.WriteElementString("Duration", Summary.Duration.ToString("g").Split('.')[0]);
            writer.WriteElementString("DateCompleted", Summary.DateCompleted.ToString("yyyy-MM-dd-hh-mm-ss"));
            writer.WriteEndElement();
        }

        /// <summary>
        /// Deserialize and XML representation into a TestResultCollection object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            //XElement testResultsElement = XElement.Load(reader);

            //XElement headerElement = testResultsElement.Element(_ns + "Header");

            //if (headerElement == null)
            //    throw new SerializationException(testResultsElement, _ns.NamespaceName + "Header");

            //CollectionHeader = (TestResultsHeader)_headerSerializer.Deserialize(headerElement.CreateReader());

            //XElement measurementsElement = testResultsElement.Element(_ns + "Measurements");

            //if (measurementsElement == null)
            //    throw new SerializationException(testResultsElement, _ns.NamespaceName + "Measurements");

            //foreach (XElement measurementElement in measurementsElement.Elements(_ns + "Measurement"))
            //    Items.Add((Measurement)_measurementSerializer.Deserialize(measurementElement.CreateReader()));

        }

        #endregion
    }
}