using System.Xml.Serialization;

namespace TsdLib.Measurements
{
    public interface IMeasurementParameter : IXmlSerializable
    {
        string Name { get; }
        object Value { get; }
    }
}