using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{//TODO: fill in station-specific config properties
    public class ProductConfig : ProductConfigCommon
    {
        //Category attribute is used to organize the PropertyGrid layout.
        //If no category is specified, properties will be placed in the 'Misc' category.
        [Category("Audio")]
        public int NumberOfMicrophones { get; set; }

        //Use public parameterless constructor to set default values
        public ProductConfig()
        {
            if (NumberOfMicrophones == default(int))
                NumberOfMicrophones = 1;
        }
    }

    public class StationConfig : StationConfigCommon
    {
        [Category("Lab")]
        public string StationLocation { get; set; }

        public StationConfig()
        {
            if (StationLocation == default(string))
                StationLocation = "Cambridge";
        }
    }

    public class TestConfig : TestConfigCommon<StationConfig, ProductConfig>
    {
        [Category("Severity")]
        public string TestSeverity { get; set; }
    }
}