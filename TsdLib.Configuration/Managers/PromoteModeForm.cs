using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsdLib.Configuration.Managers
{
    public partial class PromoteModeForm : Form
    {
        public OperatingMode TargetMode
        {
            get { return (OperatingMode) comboBox.SelectedItem; }
        }

        public PromoteModeForm()
        {
            InitializeComponent();
            comboBox.DataSource = Enum.GetValues(typeof (OperatingMode));
        }
    }
}
