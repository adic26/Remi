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

        public override string ToString()
        {
            return Name;
        }
    }

    //TODO: fill in common config properties
    public class StationConfigCommon : ConfigItem
    {

    }

    public class ProductConfigCommon : ConfigItem
    {

    }

    public class TestConfigCommon : ConfigItem
    {

    }
}