using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using TsdLib.Forms;

namespace TsdLib.Configuration.Details
{
    /// <summary>
    /// Contains metadata describing the test request.
    /// </summary>
    [Serializable]
    public class TestDetails : ITestDetails
    {
        /// <summary>
        /// Gets the name of the test system.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Test System Name")]
        [Description("Name of the test system")]
        public string TestSystemName { get; set; }

        /// <summary>
        /// Gets the name of the test system with illegal characters removed. Useful for creating namespaces or type names.
        /// </summary>
        public string SafeTestSystemName
        {
            get { return TestSystemName.Replace(" ", "_"); }
        }

        /// <summary>
        /// Gets the version of the test system.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Test System Version")]
        [Description("Version of the test system")]
        [Editor(typeof(VersionEditor), typeof(UITypeEditor))]
        public Version TestSystemVersion { get; set; }

        /// <summary>
        /// Gets the operating mode of the test system (ie. Development, Engineering or Production).
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Test System Mode")]
        [Description("Operating mode of the test system. Determines access to configuration and result publication options.")]
        public OperatingMode TestSystemMode { get; set; }

        /// <summary>
        /// Gets the version of the TSD Framework.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("TSD Framework Version")]
        [Description("Version of the TSD Framework")]
        [ReadOnly(true)]
        public Version TsdFrameworkVersion { get; set; }

        /// <summary>
        /// Gets the job/request number.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Request Number")]
        [Description("Request or job number used to track the testing")]
        public string RequestNumber { get; set; }

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
        [Description("Type of test being performed, eg. T077 Other.")]
        public string TestType { get; set; }

        /// <summary>
        /// Gets the test stage.
        /// </summary>
        [Category("Test Details")]
        [DisplayName("Test Stage")]
        [Description("Current stage of testing, eg. Analysis, Baseline, etc.")]
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
        /// Gets the functional type (SFI: 1/MFI: 2/Acc: 3)
        /// </summary>
        [Category("Test Details")]
        [DisplayName("OS Image")]
        [Description("Type of OS image loaded on the DUT, eg. MFI or SFI")]
        public FunctionalType FunctionalType { get; set; }

        /// <summary>
        /// Default constructor required for serialization.
        /// </summary>
        private TestDetails() { }

        /// <summary>
        /// Initialize a new TestDetails object filled with empty strings.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">The operating mode of the test system (ie. Development, Engineering or Production).</param>
        public TestDetails(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode)
            : this(testSystemName, testSystemVersion, testSystemMode, "QRA-XX-TEST", 1, "T077 Other", "Baseline") {  }

        /// <summary>
        /// Initialize a new TestDetails object.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="testSystemMode">The operating mode of the test system (ie. Development, Engineering or Production).</param>
        /// <param name="jobNumber">Request or job number used to track the testing.</param>
        /// <param name="unitNumber">Identifier for the DUT.</param>
        /// <param name="testType">Type of test being performed, eg. Hardware Test Case Manager test number.</param>
        /// <param name="testStage">Current stage of testing. Could be trial number, modifications performed or some other descriptor to identify what has been performed on the DUT.</param>
        /// <param name="functionalType">OPTIONAL: Type of OS image loaded on the DUT, eg. MFI or SFI.</param>
        public TestDetails(string testSystemName, Version testSystemVersion, OperatingMode testSystemMode, string jobNumber, uint unitNumber, string testType, string testStage, FunctionalType functionalType = FunctionalType.None)
        {
            TestSystemName = testSystemName;
            TestSystemVersion = testSystemVersion;
            TestSystemMode = testSystemMode;
            RequestNumber = jobNumber;
            UnitNumber = unitNumber;
            TestType = testType;
            TestStage = testStage;
            FunctionalType = functionalType;
            TsdFrameworkVersion = Assembly.GetExecutingAssembly().GetName().Version;
            StationName = Environment.MachineName;
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
            string result = string.Join(rowSeparator,
                string.Join(columnSeparator, "Test System Name", TestSystemName),
                string.Join(columnSeparator, "Test System Version", TestSystemVersion),
                string.Join(columnSeparator, "Test System Mode", TestSystemMode),
                string.Join(columnSeparator, "TSD Framework Version", TsdFrameworkVersion),
                string.Join(columnSeparator, "JobN umber", RequestNumber),
                string.Join(columnSeparator, "Unit Number", UnitNumber),
                string.Join(columnSeparator, "Test Type", TestType),
                string.Join(columnSeparator, "Test Stage", TestStage),
                string.Join(columnSeparator, "Station Name", StationName)
                );
            return result;
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
            writer.WriteElementString("TestName", TestSystemName);
            writer.WriteElementString("JobNumber", RequestNumber);
            writer.WriteElementString("UnitNumber", UnitNumber.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("TestType", TestType);
            writer.WriteElementString("TestStage", TestStage);
            writer.WriteElementString("StationName", StationName);
            if (!FunctionalType.HasFlag(FunctionalType.None))
                writer.WriteElementString("FunctionalType", ((int)FunctionalType).ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Deserialize and XML representation into a TestDetails object.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        /// <exception cref="XmlException">Incorrect XML was encountered in the input stream.</exception>
        /// <exception cref="OverflowException">UnitNumber represents a number that is less than <see cref="F:System.UInt32.MinValue" /> or greater than <see cref="F:System.UInt32.MaxValue" />. </exception>
        public void ReadXml(XmlReader reader)
        {
            reader.ReadStartElement();
            TestSystemName = reader.ReadElementContentAsString("TestName", "");
            RequestNumber = reader.ReadElementContentAsString("JobNumber", "");
            UnitNumber = Convert.ToUInt32(reader.ReadElementContentAsString("UnitNumber", ""));
            TestType = reader.ReadElementContentAsString("TestType", "");
            TestStage = reader.ReadElementContentAsString("TestStage", "");
            StationName = reader.ReadElementContentAsString("StationName", "");
            FunctionalType = (FunctionalType)Enum.Parse(typeof (FunctionalType), reader.ReadElementContentAsString("FunctionalType", ""));
            reader.ReadEndElement();
        }

    }

}
