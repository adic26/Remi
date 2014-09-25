using System;
using System.ComponentModel;
using TsdLib.Configuration;

namespace $safeprojectname$.Configuration
{
    [Serializable]
    public class StationConfig : StationConfigCommon
    {
        //TODO: Create a station configuration structure using public properties with get and set accessors.
        //The values for these properties will be configured by the application at run-time (in Development mode only) or in the database
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