using System;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    public partial class ConfigItemCreateForm : Form
    {
        public string ConfigItemName
        {
            get { return textBox_Name.Text; }
        }

        public bool StoreInDatabase
        {
            get { return radioButton_StoreInDatabase.Checked; }
        }

        public ConfigItemCreateForm()
        {
            InitializeComponent();
            button_Ok.Enabled = false;
        }

        public ConfigItemCreateForm(bool storeInDatabase)
            : this()
        {
            radioButton_StoreInDatabase.Checked = storeInDatabase;
            radioButton_StoreInDatabase.Enabled = false;
            radioButton_LocalOnly.Enabled = false;
        }

        private void textBox_Name_TextChanged(object sender, EventArgs e)
        {
            button_Ok.Enabled = textBox_Name.Text.Length > 0;
        }
    }
}
