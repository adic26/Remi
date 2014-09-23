using System;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class ConfigFormCreate<T> : Form
        where T : ConfigItem, new()
    {
        private readonly IConfigGroup<T> _configGroup;
        
        public ConfigFormCreate(IConfigGroup<T> configGroup)
        {
            InitializeComponent();
            _configGroup = configGroup;
            Load += (s, o) => Text = "New " + typeof (T).Name;
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            T newConfig = new T
            {
                Name = textBox_Name.Text,
                RemiSetting = radioButton_StoreInRemi.Checked,
            };

            _configGroup.Add(newConfig, radioButton_StoreInRemi.Checked);
        }
    }
}
