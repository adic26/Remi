using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TsdLib.Configuration.Managers
{
    public partial class ConfigManagerForm2 : Form
    {
        private ConfigManagerProvider _configProvider;

        /// <summary>
        /// Gets a collection of IConfigGroup objects that have been modified.
        /// </summary>
        public HashSet<IConfigManager> ModifiedConfigs { get; private set; }

        public ConfigManagerForm2(ConfigManagerProvider configProvider)
        {
            InitializeComponent();

            _configProvider = configProvider;

            ModifiedConfigs = new HashSet<IConfigManager>();

            comboBox_TestSystemName.DataSource = new List<string> {_configProvider._testDetails.TestSystemName};
            comboBox_TestSystemVersion.DataSource = new List<Version> {_configProvider._testDetails.TestSystemVersion};
            comboBox_TestSystemMode.DataSource = new List<OperatingMode> {_configProvider._testDetails.TestSystemMode};

            comboBox_ConfigType.DisplayMember = "ConfigTypeName";
            comboBox_ConfigType.DataSource = _configProvider;

            propertyGrid_Settings.Enabled = _configProvider._testDetails.TestSystemMode.HasFlag(OperatingMode.Developer);

            propertyGrid_Settings.CommandsVisibleIfAvailable = true;
            propertyGrid_Settings.Leave += (s, o) => ModifiedConfigs.Add((IConfigManager)comboBox_ConfigType.SelectedItem);
        }
    }
}
