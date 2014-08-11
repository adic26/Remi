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

        public ConfigGroup()
        {
            Synchronized(this);
            if (ConfigItems == null)
                ConfigItems = new BindingList<T>();
            if (LocalConfigItems == null)
                LocalConfigItems = new BindingList<T>();

            AllConfigItems = new BindingList<T>();
            foreach (T configItem in ConfigItems)
                AllConfigItems.Add(configItem);
            foreach (T localConfigItem in LocalConfigItems)
                AllConfigItems.Add(localConfigItem);

            ConfigItems.ListChanged += ConfigItems_ListChanged;
            LocalConfigItems.ListChanged += ConfigItems_ListChanged;
        }

        void ConfigItems_ListChanged(object sender, ListChangedEventArgs e)
        {
            BindingList<T> configItems = sender as BindingList<T>;
            if (configItems != null && e.ListChangedType == ListChangedType.ItemAdded)
                AllConfigItems.Add(configItems[e.NewIndex]);
        }

        public void Add(T config, bool useRemi = true)
        {
            if (AllConfigItems.Any(cfg => cfg.Name == config.Name))
            {
                Trace.WriteLine("Config already contains and item named " + config.Name);
                return;
            }

            if (useRemi)
                ConfigItems.Add(config);
            else
                LocalConfigItems.Add(config);
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

    public interface IConfigGroup<T> : IListSource, IEnumerable<T>
        where T : ConfigItem
    {
        void Save();
        void Add(T configItem, bool useRemi = true);
    }
}