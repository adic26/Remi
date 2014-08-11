// ReSharper disable CompareOfFloatsByEqualityOperator
using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    public class ProductConfig : ProductConfigCommon
    {
        //Category attribute is used to organize the PropertyGrid layout.
        //If no category is specified, properties will be placed in the 'Misc' category.
        [Category("Limits")]
        public double MinChargeCurrentLimit { get; set; }

        [Category("Limits")]
        public double MaxChargeCurrentLimit { get; set; }


        //Use public parameterless constructor to set default values
        public ProductConfig()
        {
            if (MinChargeCurrentLimit == default (double))
                MinChargeCurrentLimit = 0.5;

            if (MaxChargeCurrentLimit == default (double))
                MaxChargeCurrentLimit = 1.2;
        }
    }

    public class StationConfig : StationConfigCommon
    {
        [Category("Lab")]
        public string StationLocation { get; set; }

        [Category("Power Supply Settings")]
        public double Voltage { get; set; }

        [Category("Temperature")]
        public int LowTemperature { get; set; }

        [Category("Temperature")]
        public int HighTemperature { get; set; }

        [Category("Temperature")]
        public int TemperatureStepSize { get; set; }

        public StationConfig()
        {
            if (StationLocation == default (string))
                StationLocation = "Cambridge";

            if (Voltage == default (int))
                Voltage = 4;

            if (LowTemperature == default (int))
                LowTemperature = -20;

            if (HighTemperature == default (int))
                HighTemperature = 60;

            if (TemperatureStepSize == default (int))
                TemperatureStepSize = 20;
        }
    }
}
// ReSharper restore CompareOfFloatsByEqualityOperator
