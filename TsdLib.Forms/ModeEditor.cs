using System;
using System.Linq;
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

        public ModeEditorForm(OperatingMode currentMode)
        {
            InitializeComponent();
            comboBox.DataSource = Enum.GetValues(typeof (OperatingMode)).Cast<OperatingMode>().Where(m => m != currentMode).ToList();
        }
    }
}
