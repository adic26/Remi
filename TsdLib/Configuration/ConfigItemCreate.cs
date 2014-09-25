using System;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class ConfigItemCreate : Form
    {
        public string ConfigItemName
        {
            get { return textBox_Name.Text; }
        }

        public bool StoreInDatabase
        {
            get { return radioButton_StoreInDatabase.Checked; }
        }

        public ConfigItemCreate()
        {
            InitializeComponent();
            button_Ok.Enabled = false;
        }

        public ConfigItemCreate(bool storeInDatabase)
            : this()
        {
            radioButton_StoreInDatabase.Checked = storeInDatabase;
            radioButton_StoreInDatabase.Enabled = false;
        }

        private void textBox_Name_TextChanged(object sender, EventArgs e)
        {
            button_Ok.Enabled = textBox_Name.Text.Length > 0;
        }
    }
}
