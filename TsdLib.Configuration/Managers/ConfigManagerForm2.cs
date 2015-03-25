﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TsdLib.Configuration.Common;
using TsdLib.Configuration.Exceptions;
using TsdLib.Forms;

namespace TsdLib.Configuration.Managers
{
    /// <summary>
    /// User interface for viewing or editing configuration.
    /// </summary>
    public partial class ConfigManagerForm2 : Form
    {
        private readonly ConfigManagerProvider _configProvider;

        private IConfigManager SelectedConfigManager
        {
            get { return (IConfigManager) comboBox_ConfigType.SelectedItem; }
        }

        private readonly ControlFilter _controlFilter;

        /// <summary>
        /// Gets a collection of IConfigGroup objects that have been modified.
        /// </summary>
        public HashSet<IConfigManager> ModifiedConfigs { get; private set; }

        /// <summary>
        /// Initialize a new form to view/edit configuration.
        /// </summary>
        /// <param name="configProvider">A top-level configuration container.</param>
        public ConfigManagerForm2(ConfigManagerProvider configProvider)
        {
            InitializeComponent();

            _configProvider = configProvider;

            ModifiedConfigs = new HashSet<IConfigManager>();

            textBox_TestSystemName.Text = _configProvider._testDetails.TestSystemName;
            textBox_TestSystemVersion.Text = _configProvider._testDetails.TestSystemVersion.ToString(2);
            textBox_TestSystemMode.Text = _configProvider._testDetails.TestSystemMode.ToString();

            comboBox_ConfigType.DisplayMember = "ConfigTypeName";
            comboBox_ConfigType.DataSource = _configProvider;


            propertyGrid_Settings.CommandsVisibleIfAvailable = true;
            propertyGrid_Settings.Leave += (s, o) => ModifiedConfigs.Add(SelectedConfigManager);

            _controlFilter = new ControlFilter(new Dictionary<Control, OperatingMode>
            {
                { propertyGrid_Settings, OperatingMode.Engineering },
                { button_CloneMode, OperatingMode.Engineering },
                { button_CloneVersion, OperatingMode.Engineering },
                { button_CreateNew, OperatingMode.Engineering },
            },_configProvider._testDetails);

            //propertyGrid_Settings.Enabled = !_configProvider._testDetails.TestSystemMode.HasFlag(OperatingMode.Production);
            //button_PromoteMode.Enabled = button_PromoteVersion.Enabled = button_CreateNew.Enabled = !_configProvider._testDetails.TestSystemMode.HasFlag(OperatingMode.Production);
        }

        private void button_PromoteVersion_Click(object sender, EventArgs e)
        {
            using (VersionEditorForm form = new VersionEditorForm(_configProvider._testDetails.TestSystemVersion))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SelectedConfigManager.Save();
                    Version newVersion = form.TargetVersion;

                    _configProvider._sharedConfigConnection.CloneVersion(_configProvider._testDetails.SafeTestSystemName, _configProvider._testDetails.TestSystemVersion, _configProvider._testDetails.TestSystemMode, SelectedConfigManager.ConfigType, newVersion);
                    _configProvider._testDetails.TestSystemVersion = newVersion;
                    textBox_TestSystemVersion.Text = newVersion.ToString(2);
                }
            }
        }

        private void button_PromoteMode_Click(object sender, EventArgs e)
        {
            using (ModeEditorForm form = new ModeEditorForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SelectedConfigManager.Save();
                    OperatingMode newMode = form.TargetMode;

                    _configProvider._sharedConfigConnection.CloneMode(_configProvider._testDetails.SafeTestSystemName, _configProvider._testDetails.TestSystemVersion, _configProvider._testDetails.TestSystemMode, SelectedConfigManager.ConfigType, newMode);
                    _configProvider._testDetails.TestSystemMode = newMode;
                    textBox_TestSystemMode.Text = newMode.ToString();
                }
            }
        }

        private void comboBox_ConfigType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_ConfigItem.DataSource = SelectedConfigManager;
        }

        private void comboBox_ConfigItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid_Settings.SelectedObject = comboBox_ConfigItem.SelectedItem;
        }
    }
}
