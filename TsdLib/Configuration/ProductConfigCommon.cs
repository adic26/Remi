using System.ComponentModel;

namespace TsdLib.Configuration
{
    public class ProductConfigCommon : ConfigItem
    {
        [Category("RF")]
        public WlanChipset WlanChipset { get; set; }
    }

    public enum WlanChipset
    {
        Broadcomm,
        TexasInstruments
    }
}