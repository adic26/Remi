using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient
{
    public class TestClientProductConfig : ProductConfig
    {
        public TestClientProductConfig() { }

        public TestClientProductConfig(string productName, bool useRemi = true)
            : base(productName, useRemi) { }

        [Category("TestClient")]
        public string AntennaPlacement { get; set; }
    }

    public class TestClientStationConfig : StationConfig
    {
        public TestClientStationConfig() { }

        public TestClientStationConfig(string name = "Default", bool useRemi = true)
            : base(name, useRemi) { }

        [Category("TestClient")]
        public string RadioChipSet { get; set; }
    }
}