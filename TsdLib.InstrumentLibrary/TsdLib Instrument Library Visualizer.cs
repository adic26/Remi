using System;
using System.Windows.Forms;
using TsdLib.CodeGenerator;

namespace TsdLib.InstrumentLibrary
{
    public partial class TsdLibInstrumentLibraryVisualizer : Form
    {
        private InstrumentProvider _instrumentProvider;

        public TsdLibInstrumentLibraryVisualizer()
        {
            InitializeComponent();
        }

        private void button_RefreshInstruments_Click(object sender, EventArgs e)
        {
            _instrumentProvider = new InstrumentProvider();
            comboBox_Instrument.DataSource = _instrumentProvider.GetInstrumentsAssembly(Language.CSharp);
        }
    }
}
