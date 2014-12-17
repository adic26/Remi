﻿using System;
using System.ComponentModel;
using TsdLib.Configuration;

namespace $safeprojectname$.Configuration
{
    [Serializable]
    public class TestConfig : TestConfigCommon
    {
        //TODO: Create a test configuration structure using public properties with get and set accessors.
        //The values for these properties will be configured by the application at run-time (in Development mode only) or in the database
        //The property values will be accessed by the TestSequence.Execute() method

        [Category("Severity")]
        [Description("How severe the test conditions are")]
        public int LoopIterations { get; set; }

        [Category("Power")]
        [Description("A list of voltage settings to use on the DUT")]
        public List<double> VoltageSettings { get; set; }

        /// <summary>
        /// Initialize the configuration properties to default values. Do not use a default constructor, as it can interfere with deserialization.
        /// </summary>
        public override void Init()
        {
            LoopIterations = 5;

            VoltageSettings = new List<double> { 3.8, 4.0, 4.35 };
        }
    }
}
