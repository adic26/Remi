using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TsdLib.Configuration;

namespace TsdLib.Measurements
{
    public interface ITestResults : IXmlSerializable
    {
        ITestDetails Details { get; }
        ITestSummary Summary { get; }
        List<ITestInfo> TestInfo { get; }
        IEnumerable<IMeasurement> Measurements { get; }
        string SchemaFile { get; }

        string SaveXml();
        string SaveXml(DirectoryInfo directory);
        string SaveCsv();
        string SaveCsv(DirectoryInfo directory);
        string SaveBinary(DirectoryInfo directory);
    }

}