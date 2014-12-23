using TsdLib.Measurements;

namespace TsdLib.UI.Controls
{
    public partial class TestInfoDataGridViewControl : TestInfoDisplayControlBase
    {
        public TestInfoDataGridViewControl()
        {
            InitializeComponent();
        }

        public override void AddTestInfo(TestInfo testInfo)
        {
            dataGridView.Rows.Add(testInfo.Name, testInfo.Value);
        }
    }
}
