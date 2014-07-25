using System.ComponentModel;

namespace TsdLib.Config
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