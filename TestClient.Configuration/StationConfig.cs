﻿using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    public class StationConfig : StationConfigCommon
    {
        //TODO: Create a station configuration structure using public properties with get and set accessors.
        //The values for these properties will be configured by the application at run-time (in Development mode only) or in Remi
        //The property values will be accessed by the TestSequence.Execute() method

        [Category("RF")]
        public int LoopIterations { get; set; }

        //Use public parameterless constructor to set default values
        public StationConfig()
        {
            if (LoopIterations == default(int))
                LoopIterations = 1;
        }
    }
}