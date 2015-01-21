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
        ///// <summary>
        ///// Event fired when a selected configuration instance is changed.
        ///// </summary>
        //event EventHandler<IConfigItem[]> ConfigSelectionChanged;
        /// <summary>
        /// Event fired when a selected configuration instance is changed.
        /// </summary>
        event EventHandler ConfigSelectionChanged;

        /// <summary>
        /// Gets or sets the station configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        IConfigManager StationConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the product configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        IConfigManager ProductConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the test configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        IConfigManager TestConfigManager { get; set; }
        /// <summary>
        /// Gets or sets the sequence configuration manager. Suitable for binding to a <see cref="System.Windows.Forms.ListControl"/>.
        /// </summary>
        IConfigManager SequenceConfigManager { get; set; }

        /// <summary>
        /// Gets the selected station configuration instance(s).
        /// </summary>
        IConfigItem[] SelectedStationConfig { get; }
        /// <summary>
        /// Gets the selected product configuration instance(s).
        /// </summary>
        IConfigItem[] SelectedProductConfig { get; }
        /// <summary>
        /// Gets the selected test configuration instance(s).
        /// </summary>
        IConfigItem[] SelectedTestConfig { get; }
        /// <summary>
        /// Gets the selected sequence configuration instance(s).
        /// </summary>
        IConfigItem[] SelectedSequenceConfig { get; }
    }
}