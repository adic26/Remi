using System;

namespace TsdLib.UI.Controls
{
    public partial class TestSequenceControl : TestSequenceControlBase
    {
        public override void SetState(State state)
        {
            button_Abort.Enabled = state.HasFlag(State.TestInProgress);
            button_Execute.Enabled = state.HasFlag(State.ReadyToTest);
        }

        public TestSequenceControl()
        {
            InitializeComponent();
        }

        private void button_Execute_Click(object sender, EventArgs e)
        {
            OnExecute();
        }

        private void button_Abort_Click(object sender, EventArgs e)
        {
            OnAbort(new EventArgs());
        }

        public override bool PublishResults
        {
            get { return checkBox_PublishResults.Checked; }
        }
    }
}
