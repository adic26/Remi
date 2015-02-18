using System;
using System.Collections.Generic;
using System.Linq;
using TsdLib.Configuration.Connections;

namespace TsdLib.Configuration
{
    public static class ConfigManagerProvider
    {
        private static List<IConfigManager> _configManagers = new List<IConfigManager>();

        public static IConfigManager<T> GetConfigManager<T>(ITestDetails details, IConfigConnection sharedConfigConnection) where T : ConfigItem, new()
        {
            IConfigManager instance = GetConfigManager(typeof (T));
            if (instance == null)
            {
                instance = new ConfigManager<T>(details, sharedConfigConnection);
                _configManagers.Add(instance);
            }
            return (IConfigManager<T>)instance;
        }

        internal static IConfigManager GetConfigManager(Type configItemType)
        {
            return _configManagers.FirstOrDefault(m => m.ConfigTypeName == configItemType.Name);
        }
    }
}
