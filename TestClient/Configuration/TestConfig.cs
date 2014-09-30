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
        public int LoopIterations { get; set; }

        [Category("Power")]
        public List<double> VoltageSettings { get; set; }

        public TestConfig()
        {
            if (LoopIterations == default (int))
                LoopIterations = 5;

            if (VoltageSettings == default (List<double>))
                VoltageSettings = new List<double> { 3.8, 4.0, 4.35 };
        }
    }
}
