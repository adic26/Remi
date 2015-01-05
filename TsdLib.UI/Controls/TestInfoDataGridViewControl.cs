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

        public override void AddTestInfo(TestInfo testInfo)
        {
            dataGridView.Rows.Add(testInfo.Name, testInfo.Value);
        }

        public override void SetState(State state)
        {
            if (state.HasFlag(State.TestInProgress))
                dataGridView.Rows.Clear();
        }
    }
}
