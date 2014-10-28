using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.TestResults
{
    /// <summary>
    /// Contains test results metadata.
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName = "Header", Namespace = "TsdLib.ResultsFile.xsd")]
    [Obsolete("TestResultsHeader will soon be obsolete. You will soon need to implement TestDetails and TestResultsSummary instead.")]
    public class TestResultsHeader : ISerializable, IXmlSerializable
    {
        private readonly XNamespace _ns = "TsdLib.ResultsFile.xsd";

        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        public string TestName { get; private set; }
        /// <summary>
        /// Gets the job/request number.
        /// </summary>
        public string JobNumber { get; private set; }
        /// <summary>
        /// Gets the unit number for the current DUT.
        /// </summary>
        public string UnitNumber { get; private set; }
        /// <summary>
        /// Gets the type of test.
        /// </summary>
        public string TestType { get; private set; }
        /// <summary>
        /// Gets the name of the host PC.
        /// </summary>
        public string StationName { get; private set; }
        /// <summary>
        /// Gets the test stage.
        /// </summary>
        public string TestStage { get; private set; }
        /// <summary>
        /// Gets the BSN on the current DUT.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string BSN { get; private set; }
        /// <summary>
        /// Gets the overall test result.
        /// </summary>
        public string FinalResult { get; private set; }
        /// <summary>
        /// Gets the date and time of when the test completed.
        /// </summary>
        public DateTime DateCompleted { get; private set; }
        /// <summary>
        /// Gets the duration of time that the test took.
        /// </summary>
        public TimeSpan Duration { get { return DateCompleted - DateStarted; } }
        /// <summary>
        /// Gets any additional station-specific information.
        /// </summary>
        public string AdditionalInfo { get; private set; }
        /// <summary>
        /// Gets the date and time of when the test started.
        /// </summary>
        [XmlIgnore]
        public DateTime DateStarted { get; private set; }
        /// <summary>
        /// Gets the functional type (SFI: 1/MFI: 2/Acc: 3)
        /// </summary>
        public string FunctionalType { get; private set; }

        // ReSharper disable once UnusedMember.Local - Default constructor required for serialization.
        private TestResultsHeader() { }

        /// <summary>
        /// Initialize a new TestResultsHeader to hold test result metadata.
        /// </summary>
        /// <param name="testName">Name of the test system.</param>
        /// <param name="jobNumber">The job/request number.</param>
        /// <param name="unitNumber">Unit number for the DUT.</param>
        /// <param name="testType">Type of test (drop, tumble, etc).</param>
        /// <param name="testStage">Test stage (baseline, post stress, etc).</param>
        /// <param name="bsn">BSN (board serial number) of the DUT</param>
        /// <param name="finalResult">Overall test pass/fail result.</param>
        /// <param name="dateStarted">DateTime that the test was started.</param>
        /// <param name="dateCompleted">DateTime that the test was completed.</param>
        /// <param name="additionalInfo">Any additional information that does not fit into the previous categories.</param>
        /// <param name="functionalType">The functional type. SFI = 1, MFI = 2, Acc = 3.</param>
        public TestResultsHeader(string testName, string jobNumber, string unitNumber, string testType,
            string testStage, string bsn, string finalResult, DateTime dateStarted,
            DateTime dateCompleted, string additionalInfo, string functionalType)
        {
            TestName = testName;
            JobNumber = jobNumber;
            UnitNumber = unitNumber;
            TestType = testType;
            StationName = Environment.MachineName;
            TestStage = testStage;
            BSN = bsn;
            FinalResult = finalResult;
            DateStarted = dateStarted;
            DateCompleted = dateCompleted;
            AdditionalInfo = additionalInfo;
            FunctionalType = functionalType;
        }

        #region ISerializable

        /// <summary>
        /// Serialize the TestResultsHeader object into a binary stream.
        /// </summary>
        /// <param name="info">Stores all the data needed to serialize the object.</param>
        /// <param name="context">Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.</param>
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("TestName", TestName);
            info.AddValue("JobNumber", JobNumber);
            info.AddValue("UnitNumber", UnitNumber);
            info.AddValue("TestType", TestType);
            info.AddValue("StationName", StationName);
            info.AddValue("TestStage", TestStage);
            info.AddValue("BSN", BSN);
            info.AddValue("FinalResult", FinalResult);
            info.AddValue("DateStarted", DateStarted);
            info.AddValue("DateCompleted", DateCompleted);
            info.AddValue("AdditionalInfo", AdditionalInfo);
            info.AddValue("FunctionalType", FunctionalType);
        }

        /// <summary>
        /// Deserialize a binary stream into a TestResults object.
        /// </summary>
        /// <param name="info">Stores all the data needed to deserialize the object.</param>
        /// <param name="context">Describes the source and destination of a given serialized stream, and provides an additional caller-defined context.</param>
        protected TestResultsHeader(SerializationInfo info, StreamingContext context)
        {
            TestName = info.GetString("TestName");
            JobNumber = info.GetString("JobNumber");
            UnitNumber = info.GetString("UnitNumber");
            TestType = info.GetString("TestType");
            StationName = info.GetString("StationName");
            TestStage = info.GetString("TestStage");
            BSN = info.GetString("BSN");
            FinalResult = info.GetString("FinalResult");
            DateStarted = info.GetDateTime("DateStarted");
            DateCompleted = info.GetDateTime("DateCompleted");
            AdditionalInfo = info.GetString("AdditionalInfo");
            FunctionalType = info.GetString("FunctionalType");
        }

        #endregion

        #region IXmlSerializable

        /// <summary>
        /// Not used. Required for IXmlSerializable.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Serialize the TestResultsHeader object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            new XElement(_ns + "TestName", TestName).WriteTo(writer);
            new XElement(_ns + "JobNumber", JobNumber).WriteTo(writer);
            new XElement(_ns + "UnitNumber", UnitNumber).WriteTo(writer);
            new XElement(_ns + "TestType", TestType).WriteTo(writer);
            new XElement(_ns + "StationName", StationName).WriteTo(writer);
            new XElement(_ns + "TestStage", TestStage).WriteTo(writer);
            new XElement(_ns + "BSN", BSN).WriteTo(writer);
            new XElement(_ns + "FinalResult", FinalResult).WriteTo(writer);
            new XElement(_ns + "Duration", Duration.Subtract(TimeSpan.FromMilliseconds(Duration.Milliseconds)).ToString("c")).WriteTo(writer);
            new XElement(_ns + "AdditionalInfo", AdditionalInfo).WriteTo(writer);
            new XElement(_ns + "DateCompleted", DateCompleted.ToString("yyyy-MM-dd-hh-mm-ss")).WriteTo(writer);
            new XElement(_ns + "FunctionalType", FunctionalType).WriteTo(writer);
        }

        /// <summary>
        /// Deserialize an XML representation into a TestResultsHeader object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            XElement headerElement = XElement.Load(reader);

            TestName = (string)headerElement.Element(_ns + "TestName");
            JobNumber = (string)headerElement.Element(_ns + "JobNumber");
            UnitNumber = (string)headerElement.Element(_ns + "UnitNumber");
            TestType = (string)headerElement.Element(_ns + "TestType");
            StationName = (string)headerElement.Element(_ns + "StationName");
            TestStage = (string)headerElement.Element(_ns + "TestStage");
            BSN = (string)headerElement.Element(_ns + "BSN");
            FinalResult = (string)headerElement.Element(_ns + "FinalResult");

            //DateStarted = DateTime.Parse((string)headerElement.Element(_ns + "DateStarted"));
            //DateCompleted = DateTime.Parse((string)headerElement.Element(_ns + "DateCompleted"));

            int[] dc = ((string)headerElement.Element(_ns + "DateCompleted"))
                .Split('-')
                .Select(Int32.Parse)
                .ToArray();

            DateCompleted = new DateTime(dc[0], dc[1], dc[2], dc[3], dc[4], dc[5]);

            DateStarted = DateCompleted - TimeSpan.Parse((string)headerElement.Element(_ns + "Duration"));
            AdditionalInfo = (string)headerElement.Element(_ns + "AdditionalInfo");
            FunctionalType = (string)headerElement.Element(_ns + "FunctionalType");
        }

        #endregion
    }
}