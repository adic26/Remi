using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TsdLib.Configuration.Exceptions;

namespace TsdLib.Configuration.Managers
{
    /// <summary>
    /// User interface for viewing or editing configuration.
    /// </summary>
    public partial class ConfigManagerForm2 : Form
    {
        private ConfigManagerProvider _configProvider;

        private IConfigManager _selectedConfigManager
        {
            get { return (IConfigManager) comboBox_ConfigType.SelectedItem; }
        }

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

            propertyGrid_Settings.Enabled = !_configProvider._testDetails.TestSystemMode.HasFlag(OperatingMode.Production);

            propertyGrid_Settings.CommandsVisibleIfAvailable = true;
            propertyGrid_Settings.Leave += (s, o) => ModifiedConfigs.Add(_selectedConfigManager);
        }

        private void button_PromoteVersion_Click(object sender, EventArgs e)
        {
            using (PromoteVersionForm form = new PromoteVersionForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _selectedConfigManager.Save();
                    Version newVersion = form.TargetVersion;
                    string typeName = _selectedConfigManager.ConfigTypeName;
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    Type configType = Assembly.GetEntryAssembly().GetTypes().FirstOrDefault(type => type.Name == typeName);
                    if (configType == null)
                        throw new MissingConfigTypeException("Assembly name = " + entryAssembly.GetName().Name);

                    _configProvider._sharedConfigConnection.CloneVersion(_configProvider._testDetails.SafeTestSystemName, _configProvider._testDetails.TestSystemVersion, _configProvider._testDetails.TestSystemMode, configType, newVersion);
                    _configProvider._testDetails.TestSystemVersion = newVersion;
                    textBox_TestSystemVersion.Text = newVersion.ToString(2);
                }
            }
        }

        private void button_PromoteMode_Click(object sender, EventArgs e)
        {
            using (PromoteModeForm form = new PromoteModeForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _selectedConfigManager.Save();
                    OperatingMode newMode = form.TargetMode;
                    string typeName = _selectedConfigManager.ConfigTypeName;
                    Assembly entryAssembly = Assembly.GetEntryAssembly();
                    Type configType = entryAssembly.GetTypes().FirstOrDefault(type => type.Name == typeName);
                    if (configType == null)
                        throw new MissingConfigTypeException("Assembly name = " + entryAssembly.GetName().Name);

                    _configProvider._sharedConfigConnection.CloneMode(_configProvider._testDetails.SafeTestSystemName, _configProvider._testDetails.TestSystemVersion, _configProvider._testDetails.TestSystemMode, configType, newMode);
                    _configProvider._testDetails.TestSystemMode = newMode;
                    textBox_TestSystemMode.Text = newMode.ToString();
                }
            }
        }

        private void comboBox_ConfigType_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox_ConfigItem.DataSource = _selectedConfigManager;
        }

        private void comboBox_ConfigItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid_Settings.SelectedObject = comboBox_ConfigItem.SelectedItem;
        }
    }
}
