using System.ComponentModel;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Base class for a specific instance of a configuration. Multiple ConfigItems can be added to a ConfigGroup to form a selectable list used for parameterizing test sequences.
    /// </summary>
    public class ConfigItem
    {
        /// <summary>
        /// Gets or sets the name of the configuration item.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        public string Name { get; set; }

        /// <summary>
        /// True to store configuration locally and on Remi. False to store locally only.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        public bool RemiSetting { get; set; }

        /// <summary>
        /// Returns the name of the configuration item.
        /// </summary>
        /// <returns>Name of the configuration item.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}