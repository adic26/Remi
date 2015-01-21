using System;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to view or edit the test details on the UI.
    /// </summary>
    public partial class TestDetailsControlBase : TsdLibControl, ITestDetailsControl
    {
        /// <summary>
        /// Initialize the control.
        /// </summary>
        public TestDetailsControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event fired when requesting to edit the test details.
        /// </summary>
        public event EventHandler<bool> EditTestDetails;

        /// <summary>
        /// Fire the <see cref="EditTestDetails"/> event
        /// </summary>
        /// <param name="detailsFromDatabase">True to retrieve the test details from a database. False to use local test details.</param>
        protected virtual void OnEditTestDetails(bool detailsFromDatabase)
        {
            if (EditTestDetails != null)
                EditTestDetails(this, detailsFromDatabase);
        }

        /// <summary>
        /// Enables the control if the test system is ready to test. Otherwise disables.
        /// </summary>
        /// <param name="state">The current state of the test system.</param>
        public override void SetState(State state)
        {
            Enabled = state.HasFlag(State.ReadyToTest);
        }
    }
}
