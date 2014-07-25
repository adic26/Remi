using System.ComponentModel;

namespace TsdLib.Config
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
}