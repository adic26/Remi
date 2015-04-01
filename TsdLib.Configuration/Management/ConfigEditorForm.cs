using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TsdLib.Forms;

namespace TsdLib.Configuration.Management
{
    /// <summary>
    /// User interface for viewing or editing configuration.
    /// </summary>
    public partial class ConfigEditorForm : Form
    {
        private readonly IConfigManagerProvider _configProvider;

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
        public ConfigEditorForm(IConfigManagerProvider configProvider)
        {
            InitializeComponent();

            _configProvider = configProvider;

            ModifiedConfigs = new HashSet<IConfigManager>();

            textBox_TestSystemName.Text = _configProvider.TestDetails.TestSystemName;
            textBox_TestSystemVersion.Text = _configProvider.TestDetails.TestSystemVersion.ToString(2);
            textBox_TestSystemMode.Text = _configProvider.TestDetails.TestSystemMode.ToString();

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
            });
        }

        private void button_PromoteVersion_Click(object sender, EventArgs e)
        {
            using (VersionEditorForm form = new VersionEditorForm(_configProvider.TestDetails.TestSystemVersion))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SelectedConfigManager.Save();
                    Version newVersion = form.TargetVersion;

                    _configProvider.SharedConfigConnection.CloneVersion(_configProvider.TestDetails.SafeTestSystemName, _configProvider.TestDetails.TestSystemVersion, _configProvider.TestDetails.TestSystemMode, SelectedConfigManager.ConfigType, newVersion);
                    _configProvider.TestDetails.TestSystemVersion = newVersion;
                    textBox_TestSystemVersion.Text = newVersion.ToString(2);
                }
            }
        }

        private void button_PromoteMode_Click(object sender, EventArgs e)
        {
            using (ModeEditorForm form = new ModeEditorForm(_configProvider.TestDetails.TestSystemMode))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    SelectedConfigManager.Save();
                    OperatingMode newMode = form.TargetMode;

                    _configProvider.SharedConfigConnection.CloneMode(_configProvider.TestDetails.SafeTestSystemName, _configProvider.TestDetails.TestSystemVersion, _configProvider.TestDetails.TestSystemMode, SelectedConfigManager.ConfigType, newMode);
                    _configProvider.TestDetails.TestSystemMode = newMode;
                    textBox_TestSystemMode.Text = newMode.ToString();
                    _controlFilter.Update(newMode);
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

        private void button_CreateNew_Click(object sender, EventArgs e)
        {
            IConfigItem newCfgItem = ((IConfigItem)comboBox_ConfigItem.SelectedItem).Clone();

            using (ConfigItemCreateForm form = new ConfigItemCreateForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    newCfgItem.Name = form.ConfigItemName;
                    newCfgItem.StoreInDatabase = form.StoreInDatabase;
                    newCfgItem.IsDefault = false;
                    IConfigManager manager = (IConfigManager)comboBox_ConfigType.SelectedItem;
                    manager.Add(newCfgItem);
                    //comboBox_ConfigItem.DataSource = manager.GetList();
                    comboBox_ConfigItem.DataSource = manager;
                    comboBox_ConfigItem.SelectedIndex = comboBox_ConfigItem.Items.Count - 1;
                    ModifiedConfigs.Add(manager);
                }
            }
        }
    }
}
