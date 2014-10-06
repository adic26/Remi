using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class ConfigManagerForm : Form
    {
        private readonly string _settingsLocation;
        private readonly bool _editable;

        //For stand-alone
        public ConfigManagerForm(string settingsLocation)
        {
            InitializeComponent();

            _settingsLocation = settingsLocation;

            comboBox_TestSystemName.DataSource = new List<FilePath>(
                Directory.EnumerateDirectories(settingsLocation)
                .Select(s => new FilePath(s, false))
                .ToList());

            panel_SelectControls.Enabled = true;
            panel_EditControls.Enabled = false;
        }

        //For Client app
        public ConfigManagerForm(IList<IConfigGroup> configGroups, string testSystemName, string testSystemVersion, bool editable)
        {
            InitializeComponent();

            comboBox_TestSystemName.Enabled = false;
            comboBox_TestSystemName.DataSource = new List<string> {testSystemName};
            comboBox_TestSystemVersion.Enabled = false;
            comboBox_TestSystemVersion.DataSource = new List<string> { testSystemVersion };

            comboBox_ConfigType.DisplayMember = "ConfigType";
            comboBox_ConfigType.DataSource = configGroups;

            _editable = editable;
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
            propertyGrid_Settings.Enabled = _editable;
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

            using (ConfigItemCreate getName = new ConfigItemCreate())
            {
                getName.ShowDialog();
                newCfgItem.Name = getName.ConfigItemName;
                newCfgItem.StoreInDatabase = getName.StoreInDatabase;
            }

            IConfigGroup configs = (IConfigGroup)comboBox_ConfigType.SelectedItem;
            configs.GetList().Add(newCfgItem);

            comboBox_ConfigItem.SelectedIndex = comboBox_ConfigItem.Items.Count - 1;
        }
    }
}
