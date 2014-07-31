using System.ComponentModel;

namespace TsdLib.Configuration
{
    public class ConfigItem
    {
        internal IConfigGroup Container;

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
}