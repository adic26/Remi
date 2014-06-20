using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace TsdLib.Controller
{
    partial class SettingsForm : Form
    {
        private readonly Settings _settings;

        public SettingsForm(Settings settings)
        {
            InitializeComponent();
            _settings = settings;
            _settings.Save();
            propertyGrid_Settings.SelectedObject = _settings;

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            _settings.Save();
            Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            _settings.Reload();
            Close();
        }
    }
}
