using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class TestDetailsEditor : Form
    {
        public TestDetailsEditor(object obj)
        {
            InitializeComponent();
            
            propertyGrid.SelectedObject = obj;
        }
    }
}
