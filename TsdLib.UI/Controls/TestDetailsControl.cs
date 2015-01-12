using System;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    public partial class TestDetailsControl : TestDetailsControlBase
    {
        public TestDetailsControl()
        {
            InitializeComponent();
        }

        private void button_EditTestDetails_Click(object sender, EventArgs e)
        {
            OnEditTestDetails(checkBox_DetailsFromDatabase.Checked);
        }
    }
}
