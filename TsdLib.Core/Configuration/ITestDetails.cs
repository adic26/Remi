using System;
using System.Xml.Serialization;

namespace TsdLib.Configuration
{
    public interface ITestDetails : IXmlSerializable
    {
        event EventHandler<string> TestSystemIdentityChanged;
        string TestSystemName { get; set; }
        string SafeTestSystemName { get; }
        Version TestSystemVersion { get; set; }
        OperatingMode TestSystemMode { get; set; }
        [Obsolete("Need to use a Dictionary<string,Version> for all loaded assemblies beginning with TsdLib.")]
        Version TsdFrameworkVersion { get; set; }
        string RequestNumber { get; set; }
        uint UnitNumber { get; set; }
        string TestType { get; set; }
        string TestStage { get; set; }
        string StationName { get; set; }
        FunctionalType FunctionalType { get; set; }

        void Edit();
        string ToString(string rowSeparator, string columnSeparator);
    }
}