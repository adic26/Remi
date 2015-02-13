using System;
using System.Windows.Forms;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to view or edit the test details on the UI.
    /// </summary>
    public partial class TestDetailsControlBase : UserControl, ITestDetailsControl
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
        public void SetState(State state)
        {
            Enabled = state.HasFlag(State.ReadyToTest);
        }

        /// <summary>
        /// Obtains a lifetime service object to control the lifetime policy for this instance.
        /// </summary>
        /// <returns>An object of type <see cref="T:System.Runtime.Remoting.Lifetime.ILease"/> used to control the lifetime policy for this instance. This is the current lifetime service object for this instance if one exists; otherwise, a new lifetime service object initialized to the value of the <see cref="P:System.Runtime.Remoting.Lifetime.LifetimeServices.LeaseManagerPollTime"/> property.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
