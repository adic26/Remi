using System;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Configuration
{//TODO: make popup on designer to input new configitem name
    sealed partial class ConfigForm<T> : Form
        where T : ConfigItem, new()
    {
        private readonly IConfigGroup<T> _configGroup;
        
        public ConfigForm(IConfigGroup<T> configGroup)
        {
            InitializeComponent();
            Text = typeof(T).Name;
            _configGroup = configGroup;
            comboBox_SettingsGroup.DataSource = _configGroup;
            if (_configGroup.Any())
                propertyGrid_Settings.SelectedObject = _configGroup.ElementAt(0);
            Shown += ConfigForm_Shown;
        }

        void ConfigForm_Shown(object sender, EventArgs e)
        {
            if (!_configGroup.Any())
            {
                new ConfigFormCreate<T>(_configGroup).ShowDialog();
                //comboBox_SettingsGroup.SelectedIndex = comboBox_SettingsGroup.Items.Count - 1;
            }
        }

        private void closeForm(object sender, EventArgs e)
        {
            Close();
        }

        private void comboBox_SettingsGroup_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb != null)
                propertyGrid_Settings.SelectedObject = cb.SelectedItem;
        }
        
        private void button_CreateNew_Click(object sender, EventArgs e)
        {
            new ConfigFormCreate<T>(_configGroup).ShowDialog();

            comboBox_SettingsGroup.SelectedIndex = comboBox_SettingsGroup.Items.Count - 1;
        }
    }
}
