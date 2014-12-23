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
        List<TestInfo> TestInfo { get; }
        IEnumerable<MeasurementBase> Measurements { get; }

        string SaveXml(DirectoryInfo directory);
        string SaveCsv(DirectoryInfo directory);
        string SaveBinary(DirectoryInfo directory);
    }

}