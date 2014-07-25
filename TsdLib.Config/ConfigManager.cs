using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Config
{
    public class Config
    {
        private static readonly Config instance = new Config();
        public static Config Manager { get { return instance; } }
        private Config() { }

        private readonly Dictionary<Type, object> _configGroupObjects = new Dictionary<Type, object>();

        internal IConfigGroup<T> GetConfigGroup<T>()
            where T : ConfigItem, new()
        {
            if (_configGroupObjects.ContainsKey(typeof(T)))
                return (IConfigGroup<T>)_configGroupObjects[typeof(T)];

            IConfigGroup<T> configGroup = new ConfigGroup<T>();
            _configGroupObjects.Add(typeof(T), configGroup);

            return configGroup;
        }

        internal T GetConfig<T>(string name = "Default")
            where T : ConfigItem, new()
        {
            IConfigGroup<T> group = GetConfigGroup<T>();
            
            T configItem = group.FirstOrDefault(config => config.Name == name);
            if (configItem == null)//TODO: should we create a new ConfigItem? It will have default values - but not in production mode?
                throw new ConfigException(typeof(T).Name + " named: " + name + " could not be found.");
            return configItem;
        }

        public void EditConfig<T>()
            where T : ConfigItem, new()
        {
            IConfigGroup<T> configGroupObject = GetConfigGroup<T>();

            ConfigForm<T> form = new ConfigForm<T>();
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                configGroupObject.Save();
            else
            {
                _configGroupObjects.Remove(typeof(T));
                GetConfigGroup<T>();
            }
        }

        public void EditConfig()
        {
            //TODO: implement a way to edit all existing configs
            //how to pull in user-defined types? Use reflection to find all types derived from ConfigItem?
        }
    }
}