using System;
using System.Collections.Generic;
using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    [Serializable]
    public class TestConfig : TestConfigCommon
    {
        [Category("Severity")]
        [Description("How severe the test conditions are")]
        public int LoopIterations { get; set; }

        [Category("Power")]
        [Description("A list of voltage settings to use on the DUT")]
        public List<double> VoltageSettings { get; set; }

        /// <summary>
        /// Initialize the configuration properties to default values. Do not use a default constructor, as it can interfere with deserialization.
        /// </summary>
        public override void InitializeDefaultValues()
        {
            LoopIterations = 5;

            VoltageSettings = new List<double> { 3.8, 4.0, 4.35 };
        }
    }
}
