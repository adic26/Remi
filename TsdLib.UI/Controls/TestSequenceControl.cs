using System;
using System.Windows.Forms;

namespace TsdLib.UI.Controls
{
    /// <summary>
    /// Contains functionality to start and stop a test sequence on the UI.
    /// </summary>
    public partial class TestSequenceControl : UserControl, ITestSequenceControl
    {
        /// <summary>
        /// Initialize the contol.
        /// </summary>
        public TestSequenceControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        public event EventHandler ExecuteTestSequence;

        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        public event EventHandler AbortTestSequence;

        /// <summary>
        /// Gets the status of the Publish Results checkbox.
        /// </summary>
        public bool PublishResults
        {
            get { return checkBox_PublishResults.Checked; }
        }

        /// <summary>
        /// Enables or disables the buttons depending on the state of the test system.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        public void SetState(State state)
        {
            button_Abort.Enabled = state.HasFlag(State.TestStarting);
            button_Execute.Enabled = checkBox_PublishResults.Enabled = state.HasFlag(State.ReadyToTest);
        }

        private void button_Execute_Click(object sender, EventArgs e)
        {
            EventHandler handler = ExecuteTestSequence;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void button_Abort_Click(object sender, EventArgs e)
        {
            EventHandler handler = AbortTestSequence;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}
