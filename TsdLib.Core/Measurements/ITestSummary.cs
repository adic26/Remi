using System;
using System.Xml.Serialization;

namespace TsdLib.Measurements
{
    public interface ITestSummary : IXmlSerializable
    {
        DateTime DateStarted { get; set; }
        DateTime DateCompleted { get; set; }

        string ToString(string rowSeparator, string columnSeparator);
    }
}