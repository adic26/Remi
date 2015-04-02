using System;
using System.Collections.Generic;
using System.Linq;
using TsdLib.Configuration.Connections;
using TsdLib.Configuration.Details;

namespace TsdLib.Configuration.Management
{
    public class ConfigManagerProvider : MarshalByRefObject, IConfigManagerProvider
    {
        private readonly List<IConfigManager> _configManagers = new List<IConfigManager>();

        public ITestDetails TestDetails { get; private set;}

        public IConfigConnection SharedConfigConnection { get; private set; }
        
        /// <summary>
        /// Initialize the ConfigurationManagerProvider by providing the test details and connection information required to save and recal configuration data.
        /// </summary>
        /// <param name="testdetails"></param>
        /// <param name="sharedConfigConnection"></param>
        public ConfigManagerProvider(ITestDetails testdetails, IConfigConnection sharedConfigConnection)
        {
            TestDetails = testdetails;
            SharedConfigConnection = sharedConfigConnection;
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
                manager = new ConfigManager<T>(TestDetails, SharedConfigConnection);
                manager.Reload();
                _configManagers.Add(manager);
            }
            return manager;
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

        public void Reload()
        {
            foreach (IConfigManager configManager in _configManagers)
                configManager.Reload();
        }

        /// <summary>
        /// Allows the remote proxy to stay alive in a remote Application Domain.
        /// </summary>
        /// <returns>null</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        public bool ContainsListCollection
        {
            get { throw new NotImplementedException(); }
        }

        public System.Collections.IList GetList()
        {
            return _configManagers;
        }
    }
}
