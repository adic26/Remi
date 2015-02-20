using System;
using System.Collections.Generic;
using System.Linq;
using TsdLib.Configuration.Connections;

namespace TsdLib.Configuration.Managers
{
    public class ConfigManagerProvider : MarshalByRefObject
    {
        private readonly List<IConfigManager> _configManagers = new List<IConfigManager>();
        private readonly ITestDetails _testDetails;
        private readonly IConfigConnection _sharedConfigConnection;
        
        /// <summary>
        /// Initialize the ConfigurationManagerProvider by providing the test details and connection information required to save and recal configuration data.
        /// </summary>
        /// <param name="testdetails"></param>
        /// <param name="sharedConfigConnection"></param>
        public ConfigManagerProvider(ITestDetails testdetails, IConfigConnection sharedConfigConnection)
        {
            _testDetails = testdetails;
            _sharedConfigConnection = sharedConfigConnection;
        }

        /// <summary>
        /// Gets an instance of a configuration manager used to save and recall configuration data. Initializes the manager if one is not yet present.
        /// </summary>
        /// <typeparam name="T">Type of ConfigItem (eg. StationConfig, ProductConfig, etc)</typeparam>
        /// <returns>A configuration manager to </returns>
        public IConfigManager<T> GetConfigManager<T>() where T : ConfigItem, new()
        {
            ConfigManager<T> manager = (ConfigManager<T>)GetConfigManager(typeof (T));
            if (manager == null)
            {
                manager = new ConfigManager<T>(_testDetails, _sharedConfigConnection);
                manager.Reload();
                _configManagers.Add(manager);
            }
            return manager;
        }

        public void Reload()
        {
            foreach (IConfigManager configManager in _configManagers)
                configManager.Reload();
        }

        /// <summary>
        /// Gets an instance of a configuration manager used to save and recall configuration data. Returns null if one is not yet present.
        /// </summary>
        /// <param name="configItemType">The type of config items to manage.</param>
        /// <returns>An <see cref="IConfigManager"/>; Or null if the manager hasn't been initialized.</returns>
        internal IConfigManager GetConfigManager(Type configItemType)
        {
            return _configManagers.FirstOrDefault(m => m.ConfigTypeName == configItemType.Name);
        }

        /// <summary>
        /// Allows the remote proxy to stay alive in a remote Application Domain.
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
