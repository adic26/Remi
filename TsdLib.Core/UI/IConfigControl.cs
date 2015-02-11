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
        /// Event fired when a selected configuration instance is changed.
        /// </summary>
        event EventHandler ConfigSelectionChanged;
    }

    /// <summary>
    /// Extends <see cref="IConfigControl"/> by adding type parameters for configuration types.
    /// </summary>
    public interface IConfigControl<TStationConfig, TProductConfig, TTestConfig> : IConfigControl
        where TStationConfig : IStationConfig
        where TProductConfig : IProductConfig
        where TTestConfig : ITestConfig
    {
        /// <summary>
        /// Gets or sets the station configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<TStationConfig> StationConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the product configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<TProductConfig> ProductConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the test configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<TTestConfig> TestConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the sequence configuration manager. Suitable for binding to a ListControl/>.
        /// </summary>
        IConfigManager<ISequenceConfig> SequenceConfigManager { get; set; }

        /// <summary>
        /// Gets the selected station configuration instance(s).
        /// </summary>
        TStationConfig[] SelectedStationConfig { get; }
        /// <summary>
        /// Gets the selected product configuration instance(s).
        /// </summary>
        TProductConfig[] SelectedProductConfig { get; }
        /// <summary>
        /// Gets the selected test configuration instance(s).
        /// </summary>
        TTestConfig[] SelectedTestConfig { get; }
        /// <summary>
        /// Gets the selected sequence configuration instance(s).
        /// </summary>
        ISequenceConfig[] SelectedSequenceConfig { get; }
    }
}