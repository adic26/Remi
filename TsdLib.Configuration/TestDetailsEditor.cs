using System.Windows.Forms;

namespace TsdLib.Configuration
{
    partial class TestDetailsEditor : Form
    {
        /// <summary>
        /// Initialize a new <see cref="TestDetailsEditor"/>
        /// </summary>
        /// <param name="details">An <see cref="ITestDetails"/> object containing metadata information relevent to the test.</param>
        public TestDetailsEditor(ITestDetails details)
        {
            InitializeComponent();
            
            propertyGrid.SelectedObject = details;
        }
    }
}
