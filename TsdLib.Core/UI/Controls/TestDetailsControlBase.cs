using System;

namespace TsdLib.UI.Controls
{
    public partial class TestDetailsControlBase : TsdLibControl
    {
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
    }
}
