using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient
{//TODO: fill in station-specific config properties
    public class ProductConfig : ProductConfigCommon
    {
        [Category("Audio")]
        public int NumberOfMicrophones { get; set; }
    }

    public class StationConfig : StationConfigCommon
    {
        [Category("Lab")]
        public string StationLocation { get; set; }
    }

    public class TestConfig : TestConfigCommon
    {
        [Category("Severity")]
        public string TestSeverity { get; set; }
    }
}