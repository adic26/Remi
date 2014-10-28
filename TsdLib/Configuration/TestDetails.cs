using System;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Contains header information about the test.
    /// </summary>
    [Serializable]
    public class TestDetails : IXmlSerializable
    {
        /// <summary>
        /// Gets the name of the test.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Test Name")]
        [Description("Name of the test system")]
        public string TestName { get; set; }

        /// <summary>
        /// Gets the job/request number.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Job Number")]
        [Description("Request or job number used to track the testing")]
        public string JobNumber { get; set; }

        /// <summary>
        /// Gets the unit number for the current DUT.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Unit Number")]
        [Description("Identifier for the DUT")]
        public uint UnitNumber { get; set; }

        /// <summary>
        /// Gets the type of test.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Test Type")]
        [Description("Type of test being performed, eg. Hardware Test Case Manager test number.")]
        public string TestType { get; set; }

        /// <summary>
        /// Gets the test stage.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Test Stage")]
        [Description("Current stage of testing. Could be trial number, modifications performed or some other descriptor to identify what has been performed on the DUT.")]
        public string TestStage { get; set; }

        /// <summary>
        /// Gets the name of the host PC.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Station Name")]
        [Description("Name of the PC where the test is being performed.")]
        [ReadOnly(true)]
        public string StationName { get; set; }

        /// <summary>
        /// Gets the BSN of the DUT.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("BSN")]
        [Description("BSN of the DUT.")]
        public string BSN { get; set; }

        /// <summary>
        /// Gets the functional type (SFI: 1/MFI: 2/Acc: 3)
        /// </summary>
        [Category("Test Details")]
        [DisplayName("OS Image")]
        [Description("Type of OS image loaded on the DUT, eg. MFI or SFI")]
        public string FunctionalType { get; set; }

        /// <summary>
        /// Initialize a new TestDetails object filled with empty strings.
        /// </summary>
        public TestDetails()
        {
            TestName = JobNumber = TestType = TestStage = BSN = FunctionalType = string.Empty;
            StationName = Environment.MachineName;
            UnitNumber = 0;
        }

        /// <summary>
        /// Initialize a new TestDetails object.
        /// </summary>
        /// <param name="testName">Name of the test system.</param>
        /// <param name="jobNumber">Request or job number used to track the testing.</param>
        /// <param name="unitNumber">Identifier for the DUT.</param>
        /// <param name="testType">Type of test being performed, eg. Hardware Test Case Manager test number.</param>
        /// <param name="testStage">Current stage of testing. Could be trial number, modifications performed or some other descriptor to identify what has been performed on the DUT.</param>
        /// <param name="bsn">OPTIONAL: BSN of the DUT.</param>
        /// <param name="functionalType">OPTIONAL: Type of OS image loaded on the DUT, eg. MFI or SFI.</param>
        public TestDetails(string testName, string jobNumber, uint unitNumber, string testType, string testStage, string bsn = "", string functionalType = "")
        {
            TestName = testName;
            JobNumber = jobNumber;
            UnitNumber = unitNumber;
            TestType = testType;
            TestStage = testStage;
            StationName = Environment.MachineName;
            BSN = bsn;
            FunctionalType = functionalType;
        }

        /// <summary>
        /// Returns a CSV-friendly representation of the test details.
        /// </summary>
        /// <returns>A string containing all test details properties, delimited with Environment.NewLine and commas.</returns>
        public override string ToString()
        {
            return ToString(Environment.NewLine, ",");
        }

        /// <summary>
        /// Returns the test details, separated by the specified row and column delimiters.
        /// </summary>
        /// <param name="rowSeparator">Delimiter string to insert between the name and value of each test detail.</param>
        /// <param name="columnSeparator">Delimiter string to insert between each test detail.</param>
        /// <returns>A string representation of the test details formatted with row and column delimiters.</returns>
        public string ToString(string rowSeparator, string columnSeparator)
        {
            return string.Join(rowSeparator,
                "Test Name" + columnSeparator + TestName,
                "Job Number" + columnSeparator + JobNumber,
                "Unit Number" + columnSeparator + UnitNumber,
                "Test Type" + columnSeparator + TestType,
                "Station Name" + columnSeparator + StationName,
                "Test Stage" + columnSeparator + TestStage,
                "Functional Type" + columnSeparator + FunctionalType
                );
        }

        /// <summary>
        /// Edit the test details using a PropertyGrid user interface.
        /// </summary>
        public void Edit()
        {
            using (TestDetailsEditor editor = new TestDetailsEditor(this))
                editor.ShowDialog();
        }

        /// <summary>
        /// Not used. Required for IXmlSerializable.
        /// </summary>
        /// <returns>null</returns>
        public XmlSchema GetSchema() { return null; }

        /// <summary>
        /// Serialize the TestDetails object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("TestName", TestName);
            writer.WriteElementString("JobNumber", JobNumber);
            writer.WriteElementString("UnitNumber", UnitNumber.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("TestType", TestType);
            writer.WriteElementString("TestStage", TestStage);
            writer.WriteElementString("StationName", StationName);
            writer.WriteElementString("BSN", BSN);
            writer.WriteElementString("FunctionalType", FunctionalType);
        }

        /// <summary>
        /// Deserialize and XML representation into a TestDetails object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            //TODO: figure out optional elements

            reader.ReadStartElement();
            TestName = reader.ReadElementContentAsString("TestName", "");
            JobNumber = reader.ReadElementContentAsString("JobNumber", "");
            UnitNumber = Convert.ToUInt32(reader.ReadElementContentAsString("UnitNumber", ""));
            TestType = reader.ReadElementContentAsString("TestType", "");
            TestStage = reader.ReadElementContentAsString("TestStage", "");
            StationName = reader.ReadElementContentAsString("StationName", "");
            BSN = "Not implemented yet";
            FunctionalType = "Not implemented yet";
            reader.ReadEndElement();
        }
    }
}
