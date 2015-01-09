using System.Xml.Serialization;

namespace TsdLib.Measurements
{
    public interface ITestInfo : IXmlSerializable
    {
        string Name { get; }
        object Value { get; }
    }
}