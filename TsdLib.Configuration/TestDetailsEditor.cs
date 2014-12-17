using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class TestDetailsEditor : Form
    {
        public TestDetailsEditor(TestDetails details)
        {
            InitializeComponent();
            
            propertyGrid.SelectedObject = details;
        }
    }
}
