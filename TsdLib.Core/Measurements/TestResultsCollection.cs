using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TsdLib.Configuration;

//TODO: Move out of core
namespace TsdLib.Measurements
{
    /// <summary>
    /// Represents a collection of <see cref="TsdLib.Measurements.MeasurementBase"/> objects and <see cref="TsdLib.Measurements.TestInfo"/> objects with an <see cref="ITestDetails"/> to store metadata and an <see cref="ITestSummary"/> to summarize the test results.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "TestResults", Namespace = SchemaFileName)]
    public class TestResultCollection : ITestResults
    {
        private const string SchemaFileName = "TsdLib.ResultsFile.xsd";

        static IFormatter _formatter = new BinaryFormatter();
        static XmlSerializer _serializer = new XmlSerializer(typeof(TestResultCollection));

        /// <summary>
        /// Loads an existing test results collection from storage.
        /// </summary>
        /// <param name="filePath">Location of the saved test results collection file. Can be *.xml or *.bin.</param>
        /// <returns>A TestResultsCollection object.</returns>
        public static ITestResults Load(FileInfo filePath)
        {
            if (filePath.Extension.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
                return (ITestResults)_serializer.Deserialize(File.OpenRead(filePath.FullName));
            if (filePath.Extension.Equals("bin", StringComparison.InvariantCultureIgnoreCase))
                return (ITestResults)_formatter.Deserialize(File.OpenRead(filePath.FullName));

            throw new ArgumentException("File must be in *.xml or *.bin format", "filePath");
        }

        /// <summary>
        /// Gets the test detail metadata.
        /// </summary>
        public ITestDetails Details { get; set; }
        /// <summary>
        /// Gets a summary of the test results.
        /// </summary>
        public ITestSummary Summary { get; set; }

        /// <summary>
        /// Gets the information captured during the test.
        /// </summary>
        public List<ITestInfo> TestInfo { get; set; }

        /// <summary>
        /// Gets the measurements captued during the test.
        /// </summary>
        public IEnumerable<MeasurementBase> Measurements { get; set; }

        /// <summary>
        /// Gets the name of the xml schema used to validate the serialized output.
        /// </summary>
        public string SchemaFile
        {
            get { return SchemaFileName; }
        }

        private TestResultCollection() { }

        /// <summary>
        /// Initialize a new TestResultCollection.
        /// </summary>
        /// <param name="details">An <see cref="ITestDetails"/> object describing the test request or job details.</param>
        /// <param name="measurements">A sequence of measurements captured during the test sequence. Can be any type deriving from <see cref="TsdLib.Measurements.MeasurementBase"/>.</param>
        /// <param name="summary">A <see cref="TsdLib.Measurements.TestSummary"/> object summarizing the test results.</param>
        /// <param name="information">OPTIONAL: A sequence of informational entries captured during the test sequence.</param>
        public TestResultCollection(ITestDetails details, IEnumerable<MeasurementBase> measurements, ITestSummary summary, IEnumerable<ITestInfo> information = null)
        {
            Details = details;
            Measurements = measurements;
            Summary = summary;
            TestInfo = information != null ? information.ToList() : new List<ITestInfo>();
            
            if (details.TsdFrameworkVersion != null)
                TestInfo.Insert(0, new TestInfo("TSD Framework Version", details.TsdFrameworkVersion));
            if (details.TestSystemVersion != null)
                TestInfo.Insert(0, new TestInfo("Test System Version", details.TestSystemVersion));
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

            string formattedFileName = string.Format("{0}-{1}_", Details.RequestNumber, Details.UnitNumber.ToString("D4"));

            string fileName = Path.Combine(directory.FullName, formattedFileName + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".bin");

            using (FileStream s = File.Create(Path.Combine(directory.FullName, fileName)))
                _formatter.Serialize(s, this);

            return fileName;
        }

        /// <summary>
        /// Save the test results to an xml file in the default test results directory. Useful for uploading to database.
        /// </summary>
        /// <returns>The absolute path to the xml file generated.</returns>
        public string SaveXml()
        {
            return SaveXml(null);
        }

        /// <summary>
        /// Save the test results to an xml file in the specified directory. Useful for uploading to database.
        /// </summary>
        /// <param name="directory">The </param>
        /// <returns>The absolute path to the xml file generated.</returns>
        public string SaveXml(DirectoryInfo directory)
        {
            using (FileStream stream = SpecialFolders.GetResultsFile(Details, Summary, "xml", directory))
            {
                _serializer.Serialize(stream, this);
                return stream.Name;
            }
        }

        /// <summary>
        /// Save the test results to a csv file in the specified directory. Useful for viewing results locally.
        /// </summary>
        /// <returns>The absolute path to the csv file generated.</returns>
        public string SaveCsv()
        {
            return SaveCsv(null);

        }

        /// <summary>
        /// Save the test results to a csv file in the specified directory. Useful for viewing results locally.
        /// </summary>
        /// <param name="directory">A <see cref="System.IO.DirectoryInfo"/> object representing the directory to save the test results file to.</param>
        /// <returns>The absolute path to the csv file generated.</returns>
        public string SaveCsv(DirectoryInfo directory)
        {
            FileStream fileStream = SpecialFolders.GetResultsFile(Details, Summary, "csv", directory);

            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(ToString(Environment.NewLine, ","));
                return fileStream.Name;
            }
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
            IEnumerable<string>[] parameterArrays = Measurements.Select(m => m.Parameters.Select(p => p.Name)).ToArray();
            string parameterHeaders = !parameterArrays.Any() ? "" : parameterArrays.All(p => p.SequenceEqual(parameterArrays[0])) ? string.Join(columnSeparator, parameterArrays[0]) : "";

            return string.Join(rowSeparator,
                Details != null ? "Test Details" + rowSeparator + Details.ToString(rowSeparator, columnSeparator) : "",
                TestInfo != null ? "Information" + rowSeparator + string.Join(rowSeparator, TestInfo.Select(i => i.Name + columnSeparator + i.Value)) + rowSeparator : "",
                Summary != null ? "Summary" + rowSeparator + Summary.ToString(rowSeparator, columnSeparator) : "",
                "Measurements",
                string.Join(columnSeparator, "Measurement Name","Measured Value","Units","Lower Limit","Upper Limit", "Result", parameterHeaders),
                string.Join(rowSeparator, Measurements.Select(meas => meas.ToString(columnSeparator)))
                );
        }

        #region IXmlSerializable
        
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
            writer.WriteEndElement();

            writer.WriteStartElement("Information");
            
            foreach (ITestInfo testInfo in TestInfo)
            {
                writer.WriteStartElement("Info");
                testInfo.WriteXml(writer);
                writer.WriteElementString("TestSystemMode", Details.TestSystemMode.ToString());
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
            Summary.WriteXml(writer);
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