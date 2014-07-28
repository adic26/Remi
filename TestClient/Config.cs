using System.ComponentModel;
using System.Configuration;
using TsdLib.Configuration;

namespace TestClient
{
    public class TestClientProductConfig : ProductConfig
    {
        [Category("TestClient")]
        public string AntennaPlacement { get; set; }
    }

    public class TestClientStationConfig : StationConfig
    {
        public TestClientStationConfig()
        {
            
        }

        [Category("TestClient")]
        public string RadioChipSet { get; set; }
    }
}