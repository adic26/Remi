using System;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using TsdLib.Utilities;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Contains a summary of the test result.
    /// </summary>
    [Serializable]
    public class TestSummary : IXmlSerializable
    {
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
        /// <param name="finalResult">The overall pass/fail result of the test.</param>
        /// <param name="dateStarted">Date and time when the test was started.</param>
        /// <param name="dateCompleted">Date and time when the test was completed.</param>
        public TestSummary(string finalResult, DateTime dateStarted, DateTime dateCompleted)
        {
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
            return this.ToCsv();
        }

        /// <summary>
        /// Returns the test summary details, separated by the specified row and column delimiters.
        /// </summary>
        /// <param name="rowSeparator">Delimiter string to insert between the name and value of each summary detail.</param>
        /// <param name="columnSeparator">Delimiter string to insert between each summary detail.</param>
        /// <returns>A string representation of the test summary details formatted with row and column delimiters.</returns>
        public string ToString(string rowSeparator, string columnSeparator)
        {
            return this.ToCsv(rowSeparator, columnSeparator);
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
            writer.WriteElementString("FinalResult", FinalResult);
            writer.WriteElementString("Duration", Duration.ToString("g").Split('.')[0]);
            writer.WriteElementString("DateCompleted", DateCompleted.ToString("yyyy-MM-dd-hh-mm-ss"));
        }

        /// <summary>
        /// Deserialize and XML representation into a TestSummary object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            FinalResult = reader.ReadElementContentAsString("FinalResult", "");
            FinalResult = reader.ReadElementContentAsString("FinalResult", "");
            int[] dateStarted = reader.ReadElementContentAsString("DateStarted", "").Split('-').Select(Int32.Parse).ToArray();
            DateStarted = new DateTime(dateStarted[0], dateStarted[1], dateStarted[2], dateStarted[3], dateStarted[4], dateStarted[5]);
            int[] dateScompleted = reader.ReadElementContentAsString("DateCompleted", "").Split('-').Select(Int32.Parse).ToArray();
            DateCompleted = new DateTime(dateScompleted[0], dateScompleted[1], dateScompleted[2], dateScompleted[3], dateScompleted[4], dateScompleted[5]);
            reader.ReadEndElement();
        }
    }
}
