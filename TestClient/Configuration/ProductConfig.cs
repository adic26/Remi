using System;
using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    [Serializable]
    public class ProductConfig : ProductConfigCommon
    {
        [Category("Timing")]
        [Description("Number of milliseconds to wait after adjusting voltage level")]
        public int SettlingTime { get; set; }

        /// <summary>
        /// Initialize the configuration properties to default values. Do not use a default constructor, as it can interfere with deserialization.
        /// </summary>
        public override void InitializeDefaultValues()
        {
            SettlingTime = 500;
        }
    }
}
