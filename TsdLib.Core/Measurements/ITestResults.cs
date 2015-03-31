using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TsdLib.Configuration;
using TsdLib.Configuration.Details;

namespace TsdLib.Measurements
{
    public interface ITestResults : IXmlSerializable
    {
        ITestDetails Details { get; }
        ITestSummary Summary { get; }
        List<ITestInfo> TestInfo { get; }
        IEnumerable<IMeasurement> Measurements { get; }
        string SchemaFile { get; }

        //TODO: move this to ITestResultsSaver
        string SaveXml();
        string SaveXml(DirectoryInfo directory);
        string SaveCsv();
        string SaveCsv(DirectoryInfo directory);
        string SaveBinary(DirectoryInfo directory);
    }

}