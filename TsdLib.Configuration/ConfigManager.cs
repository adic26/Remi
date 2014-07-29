using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    public class Config<T>
        where T : ConfigItem, new()
    {
        private static readonly Config<T> instance = new Config<T>();
        public static Config<T> Manager { get { return instance; } }
        private Config() { }

        private IConfigGroup<T> _configGroup;
        internal IConfigGroup<T> ConfigGroup
        {
            get { return _configGroup ?? (_configGroup = new ConfigGroup<T>()); }
        }

        internal T GetConfig(string name = "Default")
        {
            T configItem = ConfigGroup.FirstOrDefault(config => config.Name == name);
            if (configItem == null)//TODO: should we create a new ConfigItem? It will have default values - but not in production mode?
                throw new ConfigException(typeof(T).Name + " named: " + name + " could not be found.");
            return configItem;
        }

        public void Edit()
        {
            ConfigForm<T> form = new ConfigForm<T>(ConfigGroup);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                ConfigGroup.Save();
            else
                _configGroup = new ConfigGroup<T>();
        }
    }

    [Obsolete("Use generic version instead")]
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
            IConfigGroup<T> configGroup = GetConfigGroup<T>();

            ConfigForm<T> form = new ConfigForm<T>(configGroup);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                configGroup.Save();
            else
            {
                _configGroupObjects.Remove(typeof(T));
                GetConfigGroup<T>();
            }
        }

        public void EditConfig<T>(IConfigGroup<T> configGroup)
            where T : ConfigItem, new()
        {
            configGroup = GetConfigGroup<T>();

            ConfigForm<T> form = new ConfigForm<T>(configGroup);
            form.ShowDialog();
            if (form.DialogResult == DialogResult.OK)
                configGroup.Save();
            else
            {
                _configGroupObjects.Remove(typeof(T));
                GetConfigGroup<T>();
            }
        }

        public void EditConfig(Type type)
        {
            Type configType = Type.GetType("TsdLib.Configuration.Config`1").MakeGenericType(type);
            var obj = configType.GetField("instance", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null); 

            MethodInfo method = configType.GetMethod("Edit");
            method.Invoke(obj, new object[ ]{});

        }
    }
}