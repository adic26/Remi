using System;
using System.ComponentModel;
using TsdLib.Configuration.Common;

namespace TestClient.Configuration
{
    [Serializable]
    public class StationConfig : StationConfigCommon
    {
        [Category("Power Supply")]
        [Description("The VISA resource name used to identify the power supply")]
        public string PowerSupplyAddress { get; set; }

        [Category("Calibration")]
        [Description("The path loss present in the cables connected to this station.")]
        public double PathLoss { get; set; }

        /// <summary>
        /// Initialize the configuration properties to default values. Do not use a default constructor, as it can interfere with deserialization.
        /// </summary>
        public override void InitializeDefaultValues()
        {
            PowerSupplyAddress = "GPIB0::1::INSTR";
            PathLoss = 0;
        }
    }
}