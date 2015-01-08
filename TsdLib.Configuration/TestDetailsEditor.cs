using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class TestDetailsEditor : Form
    {
        public TestDetailsEditor(ITestDetails details)
        {
            InitializeComponent();
            
            propertyGrid.SelectedObject = details;
        }
    }
}
