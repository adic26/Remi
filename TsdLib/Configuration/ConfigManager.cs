﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        /// <param name="databaseConnection">An <see cref="DatabaseConnection"/> object to handle persistence with a database.</param>
        public ConfigManager(DatabaseConnection databaseConnection)
            : base(databaseConnection)
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
        private static bool _firstRun = true;
        internal static List<IConfigGroup> ConfigGroups = new List<IConfigGroup>();

        internal string TestSystemName;
        internal string TestSystemVersion;

        private readonly DatabaseConnection _databaseConnection;

        /// <summary>
        /// Initialize a new ConfigManager instance to manage the configuration for a specified test system.
        /// </summary>
        /// <param name="databaseConnection">An <see cref="DatabaseConnection"/> object to handle persistence with a database.</param>
        public ConfigManager(DatabaseConnection databaseConnection)
        {
            TestSystemName = databaseConnection.TestSystemName;
            TestSystemVersion = databaseConnection.TestSystemVersion;
            _databaseConnection = databaseConnection;

            //upload assembly to database to support stand-alone app reflection
            if (_firstRun)
            {
                databaseConnection.UploadFileToDatabase(Assembly.GetEntryAssembly().Location, "ClientAssembly", true);
                _firstRun = false;
            }
        }

        /// <summary>
        /// Gets the group of configuration instances.
        /// </summary>
        /// <typeparam name="T">Type of configuration.</typeparam>
        /// <returns>A configuration group containing all instances of the specified configuration type.</returns>
        public IConfigGroup<T> GetConfigGroup<T>()
            where T : ConfigItem, new()
        {
            IConfigGroup cfgGrp = ConfigGroups.FirstOrDefault(cfg => cfg.ConfigType == typeof (T).Name);
            if (cfgGrp == null)
            {
                cfgGrp = (IConfigGroup<T>)Activator.CreateInstance(typeof(ConfigGroup<T>), _databaseConnection);
                ConfigGroups.Add(cfgGrp);
            }
            return cfgGrp as IConfigGroup<T>;
        }

        /// <summary>
        /// Displays the configuration form to view and modify configuration parameters.
        /// </summary>
        /// <param name="editable">True to make configuration parameters writable; False to make configuration parameters read-only.</param>
        public void Edit(bool editable)
        {
            using (ConfigManagerForm form = new ConfigManagerForm(ConfigGroups, TestSystemName, TestSystemVersion, editable))
            {
                form.ShowDialog();

                if (editable)
                {
                    if (form.DialogResult == DialogResult.OK)
                        foreach (IConfigGroup configGroup in form.ModifiedConfigGroups)
                            configGroup.Save();
                    else
                        for (int i = 0; i < ConfigGroups.Count; i++)
                            if (form.ModifiedConfigGroups.Contains(ConfigGroups[i]))
                                ConfigGroups[i] = (IConfigGroup)Activator.CreateInstance(ConfigGroups[i].GetType(), _databaseConnection);
                }
            }
        }
    }
}