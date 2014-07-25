using System.ComponentModel;

namespace TsdLib.Config
{
    public class StationConfig : ConfigItem
    {
        public StationConfig() { }

        public StationConfig(string stationName = "Default", bool remiSetting = true)
            : base(stationName, remiSetting)
        {
            
        }

        [Category("Instruments")]
        public string InstrumentAddress { get; set; }
    }
}