using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    /// <summary>
    /// User interface for viewing or editing configuration.
    /// </summary>
    public partial class ConfigManagerForm : Form
    {
        private readonly string _settingsLocation;

        /// <summary>
        /// Initialize a new <see cref="TsdLib.Configuration.ConfigManagerForm"/> to view or edit the configuration at a specified location.
        /// </summary>
        /// <param name="settingsLocation">Path to the folder or network share containing the configuration to view or edit.</param>
        /// <param name="editable">True to allow editing. False to make the configuration read-only.</param>
        public ConfigManagerForm(string settingsLocation, bool editable)
        {
            InitializeComponent();

            _settingsLocation = settingsLocation;

            comboBox_TestSystemName.DataSource = new List<FilePath>(
                Directory.EnumerateDirectories(settingsLocation)
                .Select(s => new FilePath(s, false))
                .ToList());

            panel_SelectControls.Enabled = true;
            panel_EditControls.Enabled = false;
            propertyGrid_Settings.Enabled = editable;
            comboBox_TestSystemName.SelectedValueChanged += comboBox_TestSystemName_SelectedValueChanged;
            comboBox_TestSystemVersion.SelectedValueChanged += comboBox_TestSystemVersion_SelectedValueChanged;
        }

        //For Client app
        /// <summary>
        /// Initialize a new <see cref="TsdLib.Configuration.ConfigManagerForm"/> to view or edit the specified configuration groups.
        /// </summary>
        /// <param name="configGroups">Configuration groups to view or edit.</param>
        /// <param name="testSystemName">Name of the test system.</param>
        /// <param name="testSystemVersion">Version of the test system.</param>
        /// <param name="editable">True to allow editing. False to make the configuration read-only.</param>
        public ConfigManagerForm(IList<IConfigGroup> configGroups, string testSystemName, string testSystemVersion, bool editable)
        {
            InitializeComponent();

            comboBox_TestSystemName.Enabled = false;
            comboBox_TestSystemName.DataSource = new List<string> {testSystemName};
            comboBox_TestSystemVersion.Enabled = false;
            comboBox_TestSystemVersion.DataSource = new List<string> { testSystemVersion };

            comboBox_ConfigType.DisplayMember = "ConfigType";
            comboBox_ConfigType.DataSource = configGroups;
            
            propertyGrid_Settings.Enabled = editable;
        }

        private void comboBox_TestSystemName_SelectedValueChanged(object sender, EventArgs e)
        {
            panel_EditControls.Enabled = false;

            comboBox_TestSystemVersion.DataSource = new List<FilePath>(
                Directory.EnumerateDirectories(((FilePath)comboBox_TestSystemName.SelectedItem).FullPath)
                .Select(s => new FilePath(s, false))
                .ToList());
        }

        private void comboBox_TestSystemVersion_SelectedValueChanged(object sender, EventArgs e)
        {
            panel_EditControls.Enabled = false;
            comboBox_ConfigType.DataSource = new List<FilePath>(
                Directory.EnumerateFiles(((FilePath)comboBox_TestSystemVersion.SelectedItem).FullPath)
                .Where(fullPath => File.ReadLines(fullPath).Count() > 2)
                .Select(fullPath => new FilePath(fullPath, true))
                .ToList());
        }

        private void comboBox_ConfigType_SelectedValueChanged(object sender, EventArgs e)
        {
            panel_EditControls.Enabled = false;
            object configGroupObj = comboBox_ConfigType.SelectedItem;
            IConfigGroup configGroup = configGroupObj as IConfigGroup;
            if (configGroup == null)
                throw new ConfigDoesNotExistException(configGroupObj.GetType(), comboBox_ConfigType.SelectedText);

            comboBox_ConfigItem.DataSource = configGroup;
            IList list = configGroup.GetList();
            if (list.Count == 0)
                throw new EmptyConfigGroupException(configGroup);

            propertyGrid_Settings.SelectedObject = list[0];

            panel_EditControls.Enabled = true;
            button_CreateNew.Enabled = true;
            button_OK.Enabled = true;
        }

        private void comboBox_ConfigItem_SelectedValueChanged(object sender, EventArgs e)
        {
            propertyGrid_Settings.SelectedObject = comboBox_ConfigItem.SelectedItem;
        }

        private void button_CreateNew_Click(object sender, EventArgs e)
        {
            ConfigItem sourcecg = (ConfigItem)comboBox_ConfigItem.SelectedItem;

            ConfigItem newCfgItem = sourcecg.Clone();

            using (ConfigItemCreateForm form = new ConfigItemCreateForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    newCfgItem.Name = form.ConfigItemName;
                    newCfgItem.StoreInDatabase = form.StoreInDatabase;
                    IConfigGroup configs = (IConfigGroup)comboBox_ConfigType.SelectedItem;
                    configs.GetList().Add(newCfgItem);
                    comboBox_ConfigItem.SelectedIndex = comboBox_ConfigItem.Items.Count - 1;
                }
            }


        }
    }
}
