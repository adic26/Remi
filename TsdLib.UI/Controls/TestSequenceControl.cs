using System;
using TsdLib.UI.Controls.Base;

namespace TsdLib.UI.Controls
{
    /// <summary>
    /// Contains functionality to start and stop a test sequence on the UI.
    /// </summary>
    public partial class TestSequenceControl : TestSequenceControlBase
    {
        /// <summary>
        /// Enables or disables the buttons depending on the state of the test system.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        public override void SetState(State state)
        {
            button_Abort.Enabled = state.HasFlag(State.TestInProgress);
            button_Execute.Enabled = checkBox_PublishResults.Enabled = state.HasFlag(State.ReadyToTest);
        }

        /// <summary>
        /// Initialize the contol.
        /// </summary>
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
            OnAbort();
        }

        /// <summary>
        /// Gets the status of the Publish Results checkbox.
        /// </summary>
        public override bool PublishResults
        {
            get { return checkBox_PublishResults.Checked; }
        }
    }
}
