using System.ComponentModel;

namespace TsdLib.Configuration
{
    public class ConfigItem
    {
        [ReadOnly(true)]
        [Category("Description")]
        public string Name { get; set; }

        [ReadOnly(true)]
        [Category("Description")]
        public bool RemiSetting { get; set; }

        internal ConfigItem()
        {
            
        }

        public ConfigItem(string name = "Default", bool remiSetting = true)
        {
            Name = name;
            RemiSetting = remiSetting;
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class StationConfig : ConfigItem
    {
        public StationConfig() { }

        public StationConfig(string stationName = "Default", bool remiSetting = true)
            : base(stationName, remiSetting) { }

        [Category("Instruments")]
        public string InstrumentAddress { get; set; }
    }

    public class ProductConfig : ConfigItem
    {
        public ProductConfig() { }

        public ProductConfig(string productName, bool remiSetting = true)
            : base(productName, remiSetting) { }

        [Category("RF")]
        public string Bands { get; set; }
    }

    public class TestConfig : ConfigItem
    {
        public TestConfig() { }

        [Category("Sequence")]
        public TestConfig(string testName, bool remiSetting = true)
            : base(testName, remiSetting) { }
    }
}