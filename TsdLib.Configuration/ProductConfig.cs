using System.ComponentModel;

namespace TsdLib.Configuration
{
    public class ProductConfig : ConfigItem
    {
        public ProductConfig() { }

        public ProductConfig(string productName, bool remiSetting = true)
            : base(productName, remiSetting) { }

        [Category("RF")]
        public string Bands { get; set; }
    }
}