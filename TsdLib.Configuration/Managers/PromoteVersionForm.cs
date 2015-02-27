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
    public partial class PromoteVersionForm : Form
    {
        public Version TargetVersion
        {
            get { return new Version(maskedTextBox.Text); }
        }

        public PromoteVersionForm()
        {
            InitializeComponent();
        }
    }
}
