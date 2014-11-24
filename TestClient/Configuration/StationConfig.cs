using System;
using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    [Serializable]
    public class StationConfig : StationConfigCommon
    {
        [Category("Power Supply")]
        public string PowerSupplyAddress { get; set; }

        /// <summary>
        /// Initialize a new StationConfig configuration instance from persisted settings.
        /// </summary>
        public StationConfig() { }

        /// <summary>
        /// Initialize a new StationConfig instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        public StationConfig(string name, bool storeInDatabase)
            : base(name, storeInDatabase)
        {
            PowerSupplyAddress = "GPIB0::1::INSTR";
        }
    }
}