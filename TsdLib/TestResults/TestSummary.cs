using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Contains a summary of the test result.
    /// </summary>
    [Serializable]
    public class TestSummary : IXmlSerializable
    {
        /// <summary>
        /// Gets the name of the station configuration used to perform the test.
        /// </summary>
        public string StationConfig { get; set; }
        /// <summary>
        /// Gets the name of the product configuration used to perform the test.
        /// </summary>
        public string ProductConfig { get; set; }
        /// <summary>
        /// Gets the name of the test configuration used to perform the test.
        /// </summary>
        public string TestConfig { get; set; }

        /// <summary>
        /// Gets the overall test result.
        /// </summary>
        public string FinalResult { get; set; }

        /// <summary>
        /// Gets the date and time of when the test started.
        /// </summary>
        public DateTime DateStarted { get; set; }
        /// <summary>
        /// Gets the date and time of when the test completed.
        /// </summary>
        public DateTime DateCompleted { get; set; }
        /// <summary>
        /// Gets the duration of time that the test took.
        /// </summary>
        public TimeSpan Duration { get { return DateCompleted - DateStarted; } }

        /// <summary>
        /// Initialize a new TestSummary object.
        /// </summary>
        /// <param name="stationConfig">Name of the station configuration used for the test.</param>
        /// <param name="productConfig">Name of the product configuration used for the test.</param>
        /// <param name="testConfig">Name of the test configuration used for the test.</param>
        /// <param name="finalResult">The overall pass/fail result of the test.</param>
        /// <param name="dateStarted">Date and time when the test was started.</param>
        /// <param name="dateCompleted">Date and time when the test was completed.</param>
        public TestSummary(string stationConfig, string productConfig, string testConfig, string finalResult, DateTime dateStarted, DateTime dateCompleted)
        {
            StationConfig = stationConfig;
            ProductConfig = productConfig;
            TestConfig = testConfig;
            FinalResult = finalResult;
            DateStarted = dateStarted;
            DateCompleted = dateCompleted;
        }

        /// <summary>
        /// Returns a CSV-friendly representation of the test results summary.
        /// </summary>
        /// <returns>A string containing all test results summary properties, delimited with Environment.NewLine and commas.</returns>
        public override string ToString()
        {
            return ToString(Environment.NewLine, ",");
        }

        /// <summary>
        /// Returns the test summary details, separated by the specified row and column delimiters.
        /// </summary>
        /// <param name="rowSeparator">Delimiter string to insert between the name and value of each summary detail.</param>
        /// <param name="columnSeparator">Delimiter string to insert between each summary detail.</param>
        /// <returns>A string representation of the test summary details formatted with row and column delimiters.</returns>
        public string ToString(string rowSeparator, string columnSeparator)
        {
            return string.Join(rowSeparator,
                "Station Config" + columnSeparator + StationConfig,
                "Product Config" + columnSeparator + ProductConfig,
                "Test Config" + columnSeparator + TestConfig,
                "Date Started" + columnSeparator + DateStarted,
                "Date Completed" + columnSeparator + DateCompleted,
                "Duration" + columnSeparator + Duration,
                "Final Result" + columnSeparator + FinalResult
                );
        }

        /// <summary>
        /// Not used. Required for IXmlSerializable.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Serialize the TestSummary object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            
        }

        /// <summary>
        /// Deserialize and XML representation into a TestSummary object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            //TODO: implement ReadXml
            reader.ReadStartElement();

            reader.ReadEndElement();
        }
    }
}
