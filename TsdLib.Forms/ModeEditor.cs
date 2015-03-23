using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.Forms
{
    public partial class ModeEditorForm : Form
    {
        public OperatingMode TargetMode
        {
            get { return (OperatingMode) comboBox.SelectedItem; }
        }

        public ModeEditorForm()
        {
            InitializeComponent();
            comboBox.DataSource = Enum.GetValues(typeof (OperatingMode));
        }
    }
}
