using System;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Static class to manage configuration items.
    /// </summary>
    /// <typeparam name="T">Type of configuration. Must be a class derived from <see cref="TsdLib.Configuration.ConfigItem"/>.</typeparam>
    public sealed class ConfigManager<T> : IDisposable
        where T : ConfigItem, new()
    {
        private static ConfigManager<T> _instance;

        private ConfigManager(string testSystemName, string testSystemVersion)
        {
            _testSystemName = testSystemName;
            _testSystemVersion = testSystemVersion;
        }

        /// <summary>
        /// Gets an ConfigManager instance used to expose settings (eg. ProductConfig, StationConfig, etc) for a specified test system, using the specified version.
        /// </summary>
        /// <param name="testSystemName">Name of the test system for which to manage the configuration.</param>
        /// <param name="testSystemVersion">OPTIONAL: Version of the test system. Omit to specify the currently released version.</param>
        /// <returns>A ConfigManager object to expose the application settings.</returns>
        public static ConfigManager<T> GetInstance(string testSystemName, string testSystemVersion)
        {
            return _instance ?? (_instance = new ConfigManager<T>(testSystemName, testSystemVersion));
        }

        private readonly string _testSystemName;
        private readonly string _testSystemVersion;

        private ConfigGroup<T> _configGroup;

        /// <summary>
        /// Gets the group of configuration instances.
        /// </summary>
        /// <returns>A configuration group containing all instances of the specified configuration type.</returns>
        public IConfigGroup<T> GetConfigGroup()
        {
            return _configGroup ?? (_configGroup = new ConfigGroup<T>(_testSystemName, _testSystemVersion));
        }

        /// <summary>
        /// Gets a named configuration instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <returns>The configuration instance matching the specified name.</returns>
        public T GetConfig(string name)
        {
            T configItem = _configGroup
                .FirstOrDefault(config => config.Name == name);
            if (configItem == null)
                throw new ConfigException(typeof(T), name);
            return configItem;
        }

        /// <summary>
        /// Displays the configuration form to view configuration parameters.
        /// </summary>
        /// <param name="editable">True to make configuration parameters writable; False to make configuration parameters read-only.</param>
        public void Edit(bool editable)
        {
            using (ConfigForm<T> form = new ConfigForm<T>(_configGroup, editable))
            {
                form.ShowDialog();
                if (editable)
                {
                    if (form.DialogResult == DialogResult.OK)
                        _configGroup.Save();
                    else
                        _configGroup = new ConfigGroup<T>(_testSystemName, _testSystemVersion); //reload from persisted settings
                }
            }
        }

        /// <summary>
        /// Disposes the ConfigManager, allowing to connect to a different application configuration.
        /// </summary>
        public void Dispose()
        {
            _instance = null;
        }
    }
}