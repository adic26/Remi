using System;
using TsdLib.Configuration;

namespace TsdLib.UI
{ 

    /// <summary>
    /// Defines methods and events required to control test configuration on the UI.
    /// </summary>
    public interface IConfigControl : ITsdLibControl
    {
        /// <summary>
        /// Event fired when requesting to modify the test system configuration.
        /// </summary>
        event EventHandler<IConfigManager[]> ViewEditConfiguration;

        /// <summary>
        /// Gets or sets the station configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<IStationConfig> StationConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the product configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<IProductConfig> ProductConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the test configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<ITestConfig> TestConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the sequence configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<ISequenceConfig> SequenceConfigManager { get; set; }

        /// <summary>
        /// Gets the selected station configuration instance(s).
        /// </summary>
        IStationConfig[] SelectedStationConfig { get; set; }
        /// <summary>
        /// Gets the selected product configuration instance(s).
        /// </summary>
        IProductConfig[] SelectedProductConfig { get; set; }
        /// <summary>
        /// Gets the selected test configuration instance(s).
        /// </summary>
        ITestConfig[] SelectedTestConfig { get; set; }
        /// <summary>
        /// Gets the selected sequence configuration instance(s).
        /// </summary>
        ISequenceConfig[] SelectedSequenceConfig { get; set; }
    }
}