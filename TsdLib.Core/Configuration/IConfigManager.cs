using System.ComponentModel;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Encapsulates configuration data and provides methods to save and retieve.
    /// </summary>
    public interface IConfigManager : IListSource
    {
        /// <summary>
        /// Add a new <see cref="IConfigItem"/> to the configuration instances.
        /// </summary>
        /// <param name="item"><see cref="IConfigItem"/> to add.</param>
        void Add(IConfigItem item);

        /// <summary>
        /// Saves the configuration data to persisted storage.
        /// </summary>
        void Save();

        /// <summary>
        /// Gets the type of configuration for this <see cref="IConfigManager"/>. Useful when binding to a UI control.
        /// </summary>
        string ConfigTypeName { get; }

        IConfigManager Reload();
    }

}