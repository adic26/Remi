using System;
using System.Linq;
using System.Windows.Forms;

namespace TsdLib.Config
{//TODO: make popup on designer to input new configitem name
    sealed partial class ConfigForm<T> : Form
        where T : ConfigItem, new()
    {
        public readonly IConfigGroup<T> ConfigGroup;
        
        public ConfigForm()
        {
            InitializeComponent();
            Text = typeof (T).Name;
            ConfigGroup = Config.Manager.GetConfigGroup<T>();
            comboBox_SettingsGroup.DataSource = ConfigGroup;
            if (ConfigGroup.Any())
                propertyGrid_Settings.SelectedObject = ConfigGroup.ElementAt(0);
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
            T newConfig = (T) Activator.CreateInstance(typeof (T), textBox1.Text, checkBox_RemiSetting.Checked);
            ConfigGroup.Add(newConfig, checkBox_RemiSetting.Checked);
            comboBox_SettingsGroup.SelectedIndex = comboBox_SettingsGroup.Items.Count - 1;
        }
    }
}
