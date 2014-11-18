using System;
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
        [SettingsProvider(typeof(DatabaseSettingsProvider))]
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

        public ConfigGroup(TestDetails testDetails, IDatabaseConnection databaseConnection)
        {
            Context.Add("TestDetails", testDetails);
            Context.Add("IDatabaseConnection", databaseConnection);

            Synchronized(this);
            
            if (ConfigItems == null)
                ConfigItems = new BindingList<T>();

            if (LocalConfigItems == null)
                LocalConfigItems = new BindingList<T>();

            //Add a default config if these is no config available
            if (ConfigItems.Count == 0 && LocalConfigItems.Count == 0)
            {
                //T newConfig;
                //if (typeof(T) == typeof(Sequence))
                //    newConfig = (T)Activator.CreateInstance(typeof(T), "Default" + typeof(T).Name, true, testDetails.TestSystemName);
                //else
                T newConfig = (T) Activator.CreateInstance(typeof (T), "Default" + typeof (T).Name, true);

                ConfigItems.Add(newConfig);
                Save();
            }

            AllConfigItems = new BindingList<T>();
            foreach (T configItem in ConfigItems)
                AllConfigItems.Add(configItem);
            foreach (T localConfigItem in LocalConfigItems)
                AllConfigItems.Add(localConfigItem);

            AllConfigItems.ListChanged += AllConfigItems_ListChanged;
        }

        /// <summary>
        /// Gets an instance of a configuration from the group.
        /// </summary>
        /// <param name="configName">Name of the configuration instance.</param>
        /// <returns>The ConfigItem with a name matching the specified configName.</returns>
        public T GetConfigItem(string configName)
        {
            T cfg = AllConfigItems.FirstOrDefault(c => c.Name == configName);
            if (cfg == null)
                throw new ConfigDoesNotExistException(typeof (T), configName);
            return cfg;
        }

        void AllConfigItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                T cfg = AllConfigItems[e.NewIndex];
                if (cfg.StoreInDatabase)
                    ConfigItems.Add(cfg);
                else
                    LocalConfigItems.Add(cfg);
            }
        }

        public void Add(T config, bool storeInDatabase = true)
        {
            BindingList<T> list = storeInDatabase ? ConfigItems : LocalConfigItems;

            T existing = list.FirstOrDefault(cfg => cfg.Name == config.Name);

            if (existing != null)
            {
                Trace.WriteLine(string.Format("Config already contains and item named {0}. Replacing.", config.Name));
                AllConfigItems.Remove(existing);
                list.Remove(existing);
            }

            AllConfigItems.Add(config);
        }

        /// <summary>
        /// Gets the type of <see cref="TsdLib.Configuration.ConfigItem"/>.
        /// </summary>
        public string ConfigType
        {
            get { return typeof(T).Name; }
        }

        /// <summary>
        /// Gets the base type of <see cref="TsdLib.Configuration.ConfigItem"/>.
        /// </summary>
        public string BaseConfigType
        {
            get
            {
                Type baseType = typeof(T).BaseType;
                if (baseType == null)
                    throw new ConfigDoesNotExistException(typeof(T), "Default");
                return baseType.Name;
            }
        }

        /// <summary>
        /// Gets a description of the <see cref="TsdLib.Configuration.ConfigGroup{T}"/> including type of <see cref="TsdLib.Configuration.ConfigItem"/>.
        /// </summary>
        /// <returns>A string describing the <see cref="TsdLib.Configuration.ConfigGroup{T}"/> including type of <see cref="TsdLib.Configuration.ConfigItem"/>.</returns>
        public override string ToString()
        {
            return ConfigType;
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
    public interface IConfigGroup<T> :IEnumerable<T>, IConfigGroup
        where T : ConfigItem
    {
        /// <summary>
        /// Add a new instance to the configuration group.
        /// </summary>
        /// <param name="configItem">A new configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on the database. False to store locally only.</param>
        void Add(T configItem, bool storeInDatabase = true);

        /// <summary>
        /// Gets an instance of a configuration from the group.
        /// </summary>
        /// <param name="configName">Name of the configuration instance.</param>
        /// <returns>The ConfigItem with a name matching the specified configName.</returns>
        T GetConfigItem(string configName);
    }

    /// <summary>
    /// A group of weakly-typed configuration instances.
    /// </summary>
    public interface IConfigGroup : IListSource
    {
        /// <summary>
        /// Gets the type of <see cref="TsdLib.Configuration.ConfigItem"/>.
        /// </summary>
        string ConfigType { get; }

        /// <summary>
        /// Gets the base type of <see cref="TsdLib.Configuration.ConfigItem"/>.
        /// </summary>
        string BaseConfigType { get; }

        /// <summary>
        /// Save the configuration group.
        /// </summary>
        void Save();
    }
}