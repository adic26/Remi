using System.Windows.Forms;
using TsdLib.Measurements;

namespace TsdLib.UI.Controls
{
    public partial class TestInfoDataGridViewControl : UserControl, ITestInfoDisplayControl
    {
        public TestInfoDataGridViewControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Override to add test information to the data grid.
        /// </summary>
        /// <param name="testInfo">Test information to add.</param>
        public void AddTestInfo(ITestInfo testInfo)
        {
            dataGridView.Rows.Add(testInfo.Name, testInfo.Value);
        }

        /// <summary>
        /// Clear the test information from the UI.
        /// </summary>
        public void ClearTestInfo()
        {
            dataGridView.Rows.Clear();
        }

        public void SetState(State state)
        {
            if (state.HasFlag(State.TestStarting))
                ClearTestInfo();
        }
    }
}
