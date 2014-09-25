using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    public class ConfigManager<TStationConfig, TProductConfig, TTestConfig, TSequenceConfig> : ConfigManager
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
        where TSequenceConfig : Sequence, new()
    {
        public ConfigManager(string testSystemName, string testSystemVersion, string settingsLocation)
            : base(testSystemName, testSystemVersion, settingsLocation)
        {
            GetConfigGroup<TStationConfig>();
            GetConfigGroup<TProductConfig>();
            GetConfigGroup<TTestConfig>();
            GetConfigGroup<TSequenceConfig>();
        }
    }

    public class ConfigManager
    {
        internal static readonly List<IConfigGroup> ConfigGroups = new List<IConfigGroup>();

        internal readonly string _testSystemName;
        internal readonly string _testSystemVersion;
        internal readonly string _settingsLocation;

        public ConfigManager(string testSystemName, string testSystemVersion, string settingsLocation)
        {
            _testSystemName = testSystemName;
            _testSystemVersion = testSystemVersion;
            _settingsLocation = settingsLocation;
        }

        public void Add(IConfigGroup configGroup)
        {
            ConfigGroups.Add(configGroup);
        }

        public IConfigGroup<T> GetConfigGroup<T>()
            where T : ConfigItem, new()
        {
            IConfigGroup cfgGrp = ConfigGroups.FirstOrDefault(cfg => cfg.ConfigType == typeof (T).Name);
            if (cfgGrp == null)
            {
                cfgGrp = (IConfigGroup<T>)Activator.CreateInstance(typeof(ConfigGroup<T>), _testSystemName, _testSystemVersion, _settingsLocation);
                ConfigGroups.Add(cfgGrp);
            }
            return cfgGrp as IConfigGroup<T>;
        }

        public void Edit()
        {
            using (ConfigManagerForm form = new ConfigManagerForm(ConfigGroups, _testSystemName, _testSystemVersion))
            {
                form.ShowDialog();

                if (form.DialogResult == DialogResult.OK)
                    ConfigGroups.ForEach(s => s.Save());
                else
                    for (int i = 0; i < ConfigGroups.Count; i++)
                        ConfigGroups[i] = (IConfigGroup)Activator.CreateInstance(ConfigGroups[i].GetType(), _testSystemName, _testSystemVersion, _settingsLocation);
            }
        }
    }

    ///// <summary>
    ///// Static class to manage configuration items.
    ///// </summary>
    ///// <typeparam name="T">Type of configuration. Must be a class derived from <see cref="TsdLib.Configuration.ConfigItem"/>.</typeparam>
    //[Obsolete("Use ConfigManager instead")]
    //public sealed class ConfigManager<T> : IDisposable
    //    where T : ConfigItem, new()
    //{
    //    #region Singleton Implementation

    //    private static ConfigManager<T> _instance;

    //    /// <summary>
    //    /// Gets an ConfigManager instance used to expose settings (eg. ProductConfig, StationConfig, etc) for a specified test system, using the specified version.
    //    /// </summary>
    //    /// <param name="testSystemName">Name of the test system for which to manage the configuration.</param>
    //    /// <param name="testSystemVersion">OPTIONAL: Version of the test system. Omit to specify the currently released version.</param>
    //    /// <param name="settingsLocation">Local or network location where settings will be stored.</param>
    //    /// <returns>A ConfigManager object to expose the application settings.</returns>
    //    public static ConfigManager<T> GetInstance(string testSystemName, string testSystemVersion, string settingsLocation)
    //    {
    //        return _instance ?? (_instance = new ConfigManager<T>(testSystemName, testSystemVersion, settingsLocation));
    //    }

    //    #endregion

    //    private readonly string _testSystemName;
    //    private readonly string _testSystemVersion;
    //    private readonly string _persistLocation;
    //    private ConfigGroup<T> _configGroup;

    //    private ConfigManager(string testSystemName, string testSystemVersion, string persistLocation)
    //    {
    //        _testSystemName = testSystemName;
    //        _testSystemVersion = testSystemVersion;
    //        _persistLocation = persistLocation;
    //        _configGroup = new ConfigGroup<T>(_testSystemName, _testSystemVersion, persistLocation);
    //    }

    //    /// <summary>
    //    /// Gets the group of configuration instances.
    //    /// </summary>
    //    /// <returns>A configuration group containing all instances of the specified configuration type.</returns>
    //    public IConfigGroup<T> GetConfigGroup()
    //    {
    //        return _configGroup;
    //    }

    //    /// <summary>
    //    /// Gets a named configuration instance.
    //    /// </summary>
    //    /// <param name="name">Name of the configuration instance.</param>
    //    /// <returns>The configuration instance matching the specified name.</returns>
    //    public T GetConfig(string name)
    //    {
    //        T configItem = _configGroup
    //            .FirstOrDefault(config => config.Name == name);
    //        if (configItem == null)
    //            throw new ConfigDoesNotExistException(typeof(T), name);
    //        return configItem;
    //    }

    //    /// <summary>
    //    /// Displays the configuration form to view configuration parameters.
    //    /// </summary>
    //    /// <param name="editable">True to make configuration parameters writable; False to make configuration parameters read-only.</param>
    //    public void Edit(bool editable)
    //    {
    //        var cfg = GetConfigGroup();
    //        using (ConfigForm<T> form = new ConfigForm<T>(cfg, editable))
    //        {
    //            form.ShowDialog();
    //            if (editable)
    //            {
    //                if (form.DialogResult == DialogResult.OK)
    //                    _configGroup.Save();
    //                else
    //                    _configGroup = new ConfigGroup<T>(_testSystemName, _testSystemVersion, _persistLocation); //reload from persisted settings
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Disposes the ConfigManager, allowing to connect to a different application configuration.
    //    /// </summary>
    //    public void Dispose()
    //    {
    //        _instance = null;
    //    }
    //}
}