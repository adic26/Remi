using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace TsdLib.Configuration
{
    sealed class ConfigGroup<T> : ApplicationSettingsBase, IConfigGroup<T>
        where T : ConfigItem, new()
    {
        [UserScopedSetting]
        [SettingsProvider(typeof(RemiSettingsProvider))]
        public BindingList<T> ConfigItems
        {
            get { return (BindingList<T>)this["ConfigItems"]; }
            set { this["ConfigItems"] = value; }
        }

        [UserScopedSetting]
        [SettingsProvider(typeof(LocalFileSettingsProvider))]
        public BindingList<T> LocalConfigItems
        {
            get { return (BindingList<T>)this["LocalConfigItems"]; }
            set { this["LocalConfigItems"] = value; }
        }

        BindingList<T> AllConfigItems { get; set; }

        public ConfigGroup(string testSystemName, string testSystemVersion)
        {
            if (!string.IsNullOrEmpty(testSystemName))
                Context.Add("TestSystemName", testSystemName);
            if (!string.IsNullOrEmpty(testSystemVersion))
                Context.Add("TestSystemVersion", testSystemVersion);

            Synchronized(this);

            if (ConfigItems == null)
                ConfigItems = new BindingList<T>();

            if (LocalConfigItems == null)
                LocalConfigItems = new BindingList<T>();

            //Add a local default config if these is no config available
            if (ConfigItems.Count == 0 && LocalConfigItems.Count == 0)
            {
                T newConfig = new T
                {
                    Name = "Default" + typeof(T).Name,
                    RemiSetting = false,
                };

                LocalConfigItems.Add(newConfig);
                Save();
            }

            AllConfigItems = new BindingList<T>();
            foreach (T configItem in ConfigItems)
                AllConfigItems.Add(configItem);
            foreach (T localConfigItem in LocalConfigItems)
                AllConfigItems.Add(localConfigItem);
        }

        public void Add(T config, bool useRemi = true)
        {
            BindingList<T> list = useRemi ? ConfigItems : LocalConfigItems;

            T existing = list.FirstOrDefault(cfg => cfg.Name == config.Name);

            if (existing != null)
            {
                Trace.WriteLine(string.Format("Config already contains and item named {0}. Replacing.", config.Name));
                list.Remove(existing);
                AllConfigItems.Remove(existing);
            }

            list.Add(config);
            AllConfigItems.Add(config);
        }

        #region IEnumerable implementation

        public IEnumerator<T> GetEnumerator()
        {
            return AllConfigItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region IListSource implementation

        bool IListSource.ContainsListCollection
        {
            get { return false; }
        }

        public IList GetList()
        {
            return AllConfigItems;
        }

        #endregion
    }

    /// <summary>
    /// A group of configuration instances.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IConfigGroup<T> : IListSource, IEnumerable<T>
        where T : ConfigItem
    {
        /// <summary>
        /// Save the configuration group.
        /// </summary>
        void Save();
        /// <summary>
        /// Add a new instance to the configuration group.
        /// </summary>
        /// <param name="configItem">A new configuration instance.</param>
        /// <param name="useRemi">True to store configuration locally and on Remi. False to store locally only.</param>
        void Add(T configItem, bool useRemi = true);
    }
}