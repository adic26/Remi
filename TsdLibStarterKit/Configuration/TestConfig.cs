﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using TsdLib.Configuration;

namespace $safeprojectname$.Configuration
{
    [Serializable]
    public class TestConfig : TestConfigCommon
    {
        [Category("Severity")]
        public int LoopIterations { get; set; }

        [Category("Power")]
        public List<double> VoltageSettings { get; set; }

        /// <summary>
        /// Initialize a new TestConfig configuration instance from persisted settings.
        /// </summary>
        public TestConfig() { }

        /// <summary>
        /// Initialize a new TestConfig instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        public TestConfig(string name, bool storeInDatabase)
            : base(name, storeInDatabase)
        {
            LoopIterations = 5;

            VoltageSettings = new List<double> { 3.8, 4.0, 4.35 };
        }
    }
}
