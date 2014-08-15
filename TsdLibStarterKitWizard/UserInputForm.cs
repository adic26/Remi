using System;
using System.Windows.Forms;

namespace TsdLibStarterKitWizard
{
    //TODO: add TsdLib.Instrument.* protocol selection
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
