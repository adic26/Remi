using System;
using System.Collections.Generic;

namespace TsdLib.Configuration.Common
{
    /// <summary>
    /// Contains base station config properties common to every system. Station config properties include those related to a specific system (based on host PC), eg. port/instrument addresses, physical location, etc.
    /// Station config is used to parameterize the test sequence, customizing the sequence to operate on different stations (ie. instruments with different addresses).
    /// </summary>
    [Serializable]
    public abstract class StationConfigCommon : ConfigItem
    {
        /// <summary>
        /// Gets or sets a list of machine names that can use this station config item.
        /// </summary>
        public List<string> MachineNames { get; set; }

        public override bool IsValid
        {
            get { return MachineNames.Contains(Environment.MachineName); }
        }

        public override void InitializeDefaultValues()
        {
            MachineNames = new List<string> { Environment.MachineName };
        }
    }
}
