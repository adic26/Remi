using TsdLib.Measurements;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class TestInfoDataGridViewControl : TestInfoDisplayControlBase
    {
        public TestInfoDataGridViewControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Override to add test information to the data grid.
        /// </summary>
        /// <param name="testInfo">Test information to add.</param>
        public override void AddTestInfo(TestInfo testInfo)
        {
            dataGridView.Rows.Add(testInfo.Name, testInfo.Value);
        }

        /// <summary>
        /// Clear the test information from the UI.
        /// </summary>
        public override void ClearTestInfo()
        {
            dataGridView.Rows.Clear();
        }
    }
}
