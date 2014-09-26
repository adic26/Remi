using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Extends the non-generic <see cref="TsdLib.Configuration.ConfigManager"/> to allow initialization of stronly typed configuration groups.
    /// </summary>
    /// <typeparam name="TStationConfig">Type of station configuration. Must be a class derived from <see cref="TsdLib.Configuration.StationConfigCommon"/>.</typeparam>
    /// <typeparam name="TProductConfig">Type of product configuration. Must be a class derived from <see cref="TsdLib.Configuration.ProductConfigCommon"/>.</typeparam>
    /// <typeparam name="TTestConfig">Type of test configuration. Must be a class derived from <see cref="TsdLib.Configuration.TestConfigCommon"/>.</typeparam>
    /// <typeparam name="TSequenceConfig">Type of sequence configuration. Must be a class derived from <see cref="TsdLib.Configuration.Sequence"/>.</typeparam>
    public class ConfigManager<TStationConfig, TProductConfig, TTestConfig, TSequenceConfig> : ConfigManager
        where TStationConfig : StationConfigCommon, new()
        where TProductConfig : ProductConfigCommon, new()
        where TTestConfig : TestConfigCommon, new()
        where TSequenceConfig : Sequence, new()
    {
        /// <summary>
        /// Initialize a new ConfigManager instance to manage the configuration for a specified test system.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="databaseConnection">An <see cref="IDatabaseConnection"/> object to handle persistence with a database.</param>
        public ConfigManager(string testSystemName, string testSystemVersion, IDatabaseConnection databaseConnection)
            : base(testSystemName, testSystemVersion, databaseConnection)
        {
            GetConfigGroup<TStationConfig>();
            GetConfigGroup<TProductConfig>();
            GetConfigGroup<TTestConfig>();
            GetConfigGroup<TSequenceConfig>();
        }
    }

    /// <summary>
    /// Contains functionality to manage the test system configuration.
    /// </summary>
    public class ConfigManager
    {
        private static readonly List<IConfigGroup> configGroups = new List<IConfigGroup>();

        private readonly string _testSystemName;
        private readonly string _testSystemVersion;
        private readonly IDatabaseConnection _databaseConnection;

        /// <summary>
        /// Initialize a new ConfigManager instance to manage the configuration for a specified test system.
        /// </summary>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="databaseConnection">An <see cref="IDatabaseConnection"/> object to handle persistence with a database.</param>
        public ConfigManager(string testSystemName, string testSystemVersion, IDatabaseConnection databaseConnection)
        {
            _testSystemName = testSystemName;
            _testSystemVersion = testSystemVersion;
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// Gets the group of configuration instances.
        /// </summary>
        /// <typeparam name="T">Type of configuration.</typeparam>
        /// <returns>A configuration group containing all instances of the specified configuration type.</returns>
        public IConfigGroup<T> GetConfigGroup<T>()
            where T : ConfigItem, new()
        {
            IConfigGroup cfgGrp = configGroups.FirstOrDefault(cfg => cfg.ConfigType == typeof (T).Name);
            if (cfgGrp == null)
            {
                cfgGrp = (IConfigGroup<T>)Activator.CreateInstance(typeof(ConfigGroup<T>), _testSystemName, _testSystemVersion, _databaseConnection);
                configGroups.Add(cfgGrp);
            }
            return cfgGrp as IConfigGroup<T>;
        }

        /// <summary>
        /// Displays the configuration form to view and modify configuration parameters.
        /// </summary>
        /// <param name="editable">True to make configuration parameters writable; False to make configuration parameters read-only.</param>
        public void Edit(bool editable)
        {
            using (ConfigManagerForm form = new ConfigManagerForm(configGroups, _testSystemName, _testSystemVersion, editable))
            {
                form.ShowDialog();

                if (editable)
                {
                    if (form.DialogResult == DialogResult.OK)
                        configGroups.ForEach(s => s.Save());
                    else
                        for (int i = 0; i < configGroups.Count; i++)
                            configGroups[i] = (IConfigGroup) Activator.CreateInstance(configGroups[i].GetType(), _testSystemName, _testSystemVersion, _databaseConnection);
                }
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