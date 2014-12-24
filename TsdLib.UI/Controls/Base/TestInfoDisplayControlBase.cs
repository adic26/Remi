using TsdLib.Measurements;

namespace TsdLib.UI.Controls.Base
{
    /// <summary>
    /// Placeholder for a control to display test information on the UI.
    /// </summary>
    public partial class TestInfoDisplayControlBase : TsdLibLabelledControl, ITestInfoDisplayControl
    {
        /// <summary>
        /// Initialize the control.
        /// </summary>
        public TestInfoDisplayControlBase()
        {
            InitializeComponent();
            Text = "Test Info";
        }

        /// <summary>
        /// Override to add test information to the UI display.
        /// </summary>
        /// <param name="testInfo">Test information to add.</param>
        public virtual void AddTestInfo(TestInfo testInfo)
        {

        }
    }
}
