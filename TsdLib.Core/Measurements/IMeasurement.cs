using System.Xml.Serialization;

namespace TsdLib.Measurements
{
    public interface IMeasurement : IXmlSerializable
    {
        MeasurementResult Result { get; }
        string MeasurementName { get; }
        string MeasuredValue { get; }
        string LowerLimit { get; }
        string UpperLimit { get; }
        string Units { get; }
        IMeasurementParameter[] Parameters { get; }
    }
}