using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TsdLibStarterKitWizard
{
    public partial class UserInputForm : Form
    {
        public string RootNamespace { get; private set; }

        public UserInputForm()
        {
            InitializeComponent();
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            RootNamespace = textBox_RootNamespace.Text;
            Close();
        }
    }
}
