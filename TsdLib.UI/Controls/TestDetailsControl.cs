using System;
using System.Windows.Forms;

namespace TsdLib.UI.Controls
{
    public partial class TestDetailsControl : UserControl, ITestDetailsControl
    {
        public TestDetailsControl()
        {
            InitializeComponent();
        }

        private void button_EditTestDetails_Click(object sender, EventArgs e)
        {
            EventHandler<bool> handler = EditTestDetails;
            if (handler != null)
                handler(this, checkBox_DetailsFromDatabase.Checked);
        }

        public virtual void SetState(State state)
        {
            Enabled = state.HasFlag(State.ReadyToTest);
        }

        public event EventHandler<bool> EditTestDetails;
    }
}
